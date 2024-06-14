using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

using GWeb.Models;
using Microsoft.AspNetCore.Http;
using GWeb.library;

namespace Point.Controllers // 请替换为实际的命名空间
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly girlContext _context;
        private readonly IJwtAuth _jwtAuth;

        public LoginController(IConfiguration configuration, IHttpClientFactory httpClientFactory, girlContext context, IJwtAuth jwtAuth)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _context = context;
            _jwtAuth = jwtAuth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            // 获取配置信息
            var appId = _configuration["app:appId"];
            var appSecret = _configuration["app:appSecret"];

            // 构建请求URL
            string apiUrl = $"https://api.weixin.qq.com/sns/jscode2session?appid={appId}&secret={appSecret}&js_code={request.Code}&grant_type=authorization_code";

            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                // 发送GET请求
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                // 确认请求成功
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(responseContent);
                    string openid = jsonObject["openid"].ToString();
                    string sessionKey = jsonObject["session_key"].ToString();

                    // 解密手机号信息
                    var phoneNumber = DecryptPhoneNumber(sessionKey, request.EncryptedData, request.Iv);

                    var user = _context.Users.SingleOrDefault(b => b.OpenId == openid);
                    if (user == null)
                    {
                        User newUser = new User
                        {
                            OpenId = openid,
                            NickName = "用户昵称",
                            Phone  = phoneNumber,
                            HeadImgUrl = "https://img.zcool.cn/community/01460b57e4a6fa0000012e7ed75e83.png"
                        };
                        _context.Users.Add(newUser);
                        await _context.SaveChangesAsync();
                    }

                    var token = _jwtAuth.WeChatAuthentication(openid);
                    return Ok(new { success = true, token });
                }
                else
                {
                    Console.WriteLine($"HTTP请求失败: {response.StatusCode}");
                    return BadRequest("HTTP请求失败");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生异常: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "服务器发生错误");
            }
        }

        private string DecryptPhoneNumber(string sessionKey, string encryptedData, string iv)
        {
            try
            {
                var aes = Aes.Create();
                aes.Key = Convert.FromBase64String(sessionKey);
                aes.IV = Convert.FromBase64String(iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                var encryptedBytes = Convert.FromBase64String(encryptedData);

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    var resultArray = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    var resultStr = Encoding.UTF8.GetString(resultArray);

                    JObject resultJson = JObject.Parse(resultStr);
                    return resultJson["phoneNumber"].ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"解密失败: {ex.Message}");
                return null;
            }
        }
    }

    public class LoginRequest
    {
        public string Code { get; set; }
        public string EncryptedData { get; set; }
        public string Iv { get; set; }
    }
}