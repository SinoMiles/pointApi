using GWeb.library;
using GWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Point.Controllers.dto;
using Point.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Point.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        girlContext _context;
        private readonly IJwtAuth jwtAuth;
        public PointController(girlContext context, IJwtAuth jwtAuth,IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            this.jwtAuth = jwtAuth;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        [HttpGet("userinfo")]
        public async Task<string> GetUserInfoAsync( string code)
        {

            var appId = _configuration.GetSection("app")["appId"];
            var appSecret = _configuration.GetSection("app")["appSecret"];
            // 构建请求URL
            string apiUrl = $"https://api.weixin.qq.com/sns/jscode2session?appid={appId}&secret={appSecret}&js_code={code}&grant_type=authorization_code";

            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                // 发送GET请求
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                // 确认请求成功
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    //创建账号
                    JObject jsonObject = JObject.Parse(responseContent);
                    string openid = jsonObject["openid"].ToString();
                    var user=_context.Users.Where(b=> b.OpenId== openid).FirstOrDefault();
                    if (user == null)
                    {
                        User user1 = new User();
                        user1.OpenId = openid;
                        user1.NickName = "用户昵称";
                        user1.HeadImgUrl = "https://img.zcool.cn/community/01460b57e4a6fa0000012e7ed75e83.png";
                        _context.Users.Add(user1);
                        _context.SaveChanges();
                    }
                    var token = jwtAuth.WeChatAuthentication(openid);

                    return token;
                }
                else
                {
                    // 处理请求失败的情况
                    Console.WriteLine($"HTTP请求失败: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                Console.WriteLine($"发生异常: {ex.Message}");
                return null;
            }
        }

        [HttpPost("upload-avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            try
            {
                if (file == null || file.Length <= 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Success = false,
                        ErrorMessage = "No file uploaded"
                    });
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");

                // Create the directory if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName; // Unique file name
                var filePath = Path.Combine(uploadsFolder, uniqueFileName); // File save path

                // Handle file upload logic
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return the URL of the uploaded image
           
                var relativePath = Path.Combine( "uploads", "avatars", uniqueFileName).Replace('\\', '/'); // Relative path to the image
                var imageUrl = relativePath;
                int userId = int.Parse(UserHelper.getUser(Request, _context));
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                user.HeadImgUrl = _configuration["imageUrl"] + imageUrl;
                _context.SaveChanges();
                // Return success response with the image URL
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Data = new { ImageUrl = imageUrl }
                });
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse<object>
                {
                    Success = false,
                    ErrorMessage = "File upload failed: " + ex.Message
                });
            }
        }

        [HttpPost("upload-cover")]
        public async Task<IActionResult> UploadCover(IFormFile file)
        {
            try
            {
                if (file == null || file.Length <= 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Success = false,
                        ErrorMessage = "No file uploaded"
                    });
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "covers");

                // Create the directory if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName; // Unique file name
                var filePath = Path.Combine(uploadsFolder, uniqueFileName); // File save path

                // Handle file upload logic
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return the URL of the uploaded image
                var relativePath = Path.Combine("uploads", "covers", uniqueFileName).Replace('\\', '/'); // Relative path to the image
                var imageUrl = relativePath;
                var imgHost= _configuration["imageUrl"] + imageUrl;
                // Return success response with the image URL
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Data = new { ImageUrl = imgHost }
                });
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse<object>
                {
                    Success = false,
                    ErrorMessage = "File upload failed: " + ex.Message
                });
            }
        }


        [HttpPost("update-nickname")]
        public async Task<IActionResult> UpdateNickname(string name)
        {
            try
            {
                int userId = int.Parse(UserHelper.getUser(Request, _context));
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                user.NickName = name;
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = name
                }) ;
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    ErrorMessage = "Failed to update nickname: " + ex.Message
                });
            }
        }

        [HttpPost("verify")]
        public IActionResult VerifyWinner(string code)
        {
            var winner = _context.Participants.FirstOrDefault(p => p.Code == code && p.Winner);

            if (winner == null)
            {
                return NotFound("未找到中奖用户或者核销码不正确");
            }

            winner.Verify = true;
            _context.SaveChanges();

            return Ok("核销成功！");
        }


        [HttpGet("getuser")]
        [Authorize]
        public IActionResult GetUser()
        {

            int userId = int.Parse(UserHelper.getUser(Request, _context));
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    ErrorMessage = "User not found"
                });
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                OpenId = user.OpenId,
                NickName = user.NickName,
                HeadImgUrl = user.HeadImgUrl,
                IsMerchant=user.IsMerchant
            };

            return Ok(new ApiResponse<UserDto>
            {
                Success = true,
                Data = userDto
            });
        }

        [HttpPost("goodlist")]
        [Authorize]
        public async Task<List<GoodList>> GetWinningInfoForUser()
        {

                int userId = int.Parse(UserHelper.getUser(Request, _context));
                var winningList =await _context.Participants
                    .Where(p => p.UserId == userId && p.Winner)
                    .Join(_context.LotteryActivitys,
                        participant => participant.LotteryActivityId,
                        activity => activity.Id,
                        (participant, activity) => new GoodList { Participant = participant, LotteryActivity = activity })
                    .ToListAsync();

                return winningList;

        }


        [NonAction]
        public async Task SendSubscriptionMessage(string openid, string thing1, string thing2, string phrase3, string thing4)
        {
            try
            {
                var accessToken = await GetAccessToken();
                var url = $"https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token={accessToken}";
                var data = new
                {
                    touser = openid,
                    template_id = "xJ7PxhUXrnJHUh3SAQ1zMcbMkD-qkce73RrhtFCH2bQ",
                    data = new
                    {
                        thing1 = new { value = thing1 },
                        thing2 = new { value = thing2 },
                        phrase3 = new { value = phrase3 },
                        thing4 = new { value = thing4 }
                    },
                    miniprogram_state = "developer",
                    lang = "zh_CN"
                };
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var httpClient = _httpClientFactory.CreateClient();
                await httpClient.PostAsync(url, content);
            }
            catch (Exception ex)
            {
                // 处理异常
                Console.WriteLine($"Failed to send notification: {ex.Message}");
            }
        }

        [HttpPost("enroll")]
        [Authorize]
        public async Task<IActionResult> Enroll([FromBody] ParticipantDto participant)
        {
            ApiResponse<string> response = new ApiResponse<string>();

            try
            {
                // 获取当前活动
                var activity = await _context.LotteryActivitys.FindAsync(participant.LotteryActivityId);

                if (activity == null)
                {
                    response.Success = false;
                    response.ErrorMessage = "找不到活动";
                    return Ok(response);
                }

                // 检查用户是否首次参加活动
                int userId = int.Parse(UserHelper.getUser(Request, _context));
                var userInfo =await _context.Users.Where(b => b.Id == userId).FirstOrDefaultAsync();
                bool isFirstTimeParticipant = !_context.Participants.Any(p => p.UserId == userId);

                if (isFirstTimeParticipant)
                {
                    // 直接标记用户为中奖者
                    Participant participant1 = new Participant
                    {
                        UserId = userId,
                        LotteryActivityId = participant.LotteryActivityId,
                        RegistrationDate = DateTime.Now,
                        Image = userInfo.HeadImgUrl,
                        Code= GenerateVerificationCode(),
                        Winner = true // 首次参与活动，直接标记为中奖
                    };
                    _context.Participants.Add(participant1);
                    _context.SaveChanges();
                    response.Success = true;
                    response.Data = "中奖啦！";
                    return Ok(response);
                }
                else
                {
                    // 其他参与者的报名逻辑
                    if (!activity.IsOpen)
                    {
                        response.Success = false;
                        response.ErrorMessage = "活动已关闭";
                        return Ok(response);
                    }
                    else
                    {
                        activity.HaveParticipants++;
                        Participant participant1 = new Participant
                        {
                            UserId = userId,
                            LotteryActivityId = participant.LotteryActivityId,
                            RegistrationDate = DateTime.Now,
                            Image=userInfo.HeadImgUrl        
                        };
                        _context.Participants.Add(participant1);
                        await _context.SaveChangesAsync();
                        if (activity.HaveParticipants >= activity.MaxParticipants)
                        {
                            Random random = new Random();
                            int randomIndex = random.Next(0, activity.MaxParticipants);
                            var list = _context.Participants.OrderBy(b => b.RegistrationDate).Where(b => b.LotteryActivityId == participant.LotteryActivityId).ToList();
                            var info = list[randomIndex];
                            info.Winner = true;
                            info.Code = GenerateVerificationCode();
                            activity.IsOpen = false;
                            await SendSubscriptionMessage(userInfo.OpenId, userInfo.NickName, activity.Name, "一等奖", "恭喜您获奖啦！");
                        }
                    }
                }

                await _context.SaveChangesAsync();
                response.Success = true;
                response.Data = "报名成功";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return Ok(response);
            }
        }
        private string GenerateVerificationCode()
        {
            Random random = new Random();
            int verificationCode = random.Next(1000, 10000);
            return verificationCode.ToString();
        }

        private async Task<string> GetAccessToken()
        {
            try
            {
                var appId = _configuration.GetSection("app")["appId"];
                var appSecret = _configuration.GetSection("app")["appSecret"];
                var url = $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appId}&secret={appSecret}";

                var httpClient = _httpClientFactory.CreateClient();
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var tokenInfo = JsonConvert.DeserializeObject<TokenResponse>(result);

                    return tokenInfo?.access_token;
                }
                else
                {
                    // 处理获取失败的情况
                    Console.WriteLine("Failed to fetch access token");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                Console.WriteLine($"Failed to fetch access token: {ex.Message}");
                return null;
            }
        }

        [HttpGet("activity/{id}/check-participation")]
        [Authorize]
        public async Task<IActionResult> CheckParticipation(int id)
        {
            int userId = int.Parse(UserHelper.getUser(Request, _context));
            var participant = await _context.Participants
                .FirstOrDefaultAsync(p => p.LotteryActivityId == id && p.UserId == userId);
            if (participant != null)
            {
                return Ok(true); // 用户已参加活动
            }

            return Ok(false); // 用户未参加活动
        }


        [HttpGet("activity/{id}")]
        public async Task<IActionResult> GetActivityDetails(int id)
        {
            var activity = await _context.LotteryActivitys
                .Where(a => a.Id == id).Include(b=> b.CreatorUser).Select(a=> new {
                    a.Id,
                    a.Name,
                    a.Description,
                    a.StartDate,
                    a.EndDate,
                    a.MaxParticipants,
                    a.IsOpen,
                    a.image,
                    a.HaveParticipants,
                    UserId = a.CreatorUser.Id,
                    NickName = a.CreatorUser.NickName,
                    HeadImgUrl = a.CreatorUser.HeadImgUrl
                }).FirstOrDefaultAsync();
            if (activity == null)
            {
                return NotFound("Activity not found");
            }
            var par =await _context.Participants.Where(b => b.LotteryActivityId == id ).ToListAsync();
            List<GetActivityDetailsResultDto> list =new List<GetActivityDetailsResultDto>();
            foreach (var item in par)
            {
                GetActivityDetailsResultDto dto = new GetActivityDetailsResultDto();
                dto.UserID = item.UserId;
                dto.NickName = item.NickName;
                dto.Image = item.Image;
                list.Add(dto);
            }
           
            var result = new
            {
                Activitie = activity,
                list = list,
            };
            return Ok(result);
        }
        [HttpPost("createactivity")]
        public async Task<IActionResult> CreateActivity([FromBody] CreateLotteryDto request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    ErrorMessage = "Invalid request data"
                });
            }

            try
            {
                int userId = int.Parse(UserHelper.getUser(Request, _context));
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                var activity = new LotteryActivity
                {
                    Name = request.Name,
                    Description = request.Description,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    MaxParticipants = request.MaxParticipants,
                    image = request.image,
                    IsOpen = true,
                    CreatorUser = user,
                    CreateTime = DateTime.Now
                };

                _context.LotteryActivitys.Add(activity);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    ErrorMessage = "An error occurred while creating the activity: " + ex.Message
                });
            }
        }

        [HttpGet("activitylist")]
        public async Task<IActionResult> GetActivityList([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // 计算数据库中的总活动数
            int totalActivities = await _context.LotteryActivitys.CountAsync();

            // 根据总活动数和每页大小计算总页数
            int totalPage = (int)Math.Ceiling(totalActivities / (double)pageSize);

            // 确保页码和每页大小在有效范围内
            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPage)
            {
                page = totalPage;
            }

            // 计算要跳过的记录数，考虑到调整后的页码值
            int skip = (page - 1) * pageSize;

            // 查询数据库以获取分页后的活动列表
            var activities = await _context.LotteryActivitys
                .OrderByDescending(a => a.StartDate)
                .OrderBy(a=> a.IsOpen).OrderBy(a=> a.CreateTime)
                .Skip(skip)
                .Take(pageSize)
                .Include(b => b.CreatorUser)
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Description,
                    a.StartDate,
                    a.EndDate,
                    a.MaxParticipants,
                    a.IsOpen,
                    a.image,
                    a.HaveParticipants,
                    UserId = a.CreatorUser.Id,
                    NickName = a.CreatorUser.NickName,
                    HeadImgUrl = a.CreatorUser.HeadImgUrl
                })
                .ToListAsync();

            // 创建一个包含活动列表和总页数的匿名对象
            var result = new
            {
                Activities = activities,
                TotalPage = totalPage
            };

            return Ok(result);
        }
        [HttpGet("my-activities")]
        public async Task<IActionResult> GetMyActivities(int page = 1, int pageSize = 10)
        {
            try
            {
                int userId = int.Parse(UserHelper.getUser(Request, _context));

                // 计算跳过的记录数
                int skip = (page - 1) * pageSize;

                // 查询自己发布的活动
                var activities = await _context.LotteryActivitys
                    .Where(a => a.CreatorUser.Id == userId)
                    .OrderByDescending(a => a.CreateTime) // 按创建时间降序排列
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();

                // 查询总记录数
                int totalActivities = await _context.LotteryActivitys
                    .Where(a => a.CreatorUser.Id == userId)
                    .CountAsync();

                // 构造返回包装
                var response = new ApiResponse<object>
                {
                    Success = true,
                    Data = new
                    {
                        Activities = activities,
                        TotalCount = totalActivities,
                        Page = page,
                        PageSize = pageSize
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // 处理异常情况
                var response = new ApiResponse<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };

                return StatusCode(500, response);
            }
        }
        [HttpPost("deleteActivity")]
        public async Task<IActionResult> DeleteActivity(int activityId)
        {
            try
            {
                int userId = int.Parse(UserHelper.getUser(Request, _context));
                var activity = _context.LotteryActivitys.FirstOrDefault(a => a.Id == activityId);

                if (activity == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Success = false,
                        ErrorMessage = "活动不存在"
                    });
                }

                if (activity.CreatorUser.Id != userId)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Success = false,
                        ErrorMessage = "您没有权限删除此活动"
                    });
                }

                if (DateTime.Now < activity.EndDate)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Success = false,
                        ErrorMessage = "活动正在进行，无法删除"
                    });
                }

                _context.LotteryActivitys.Remove(activity);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse<object>
                {
                    Success = false,
                    ErrorMessage = "删除活动失败：" + ex.Message
                });
            }
        }

    }
}
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string ErrorMessage { get; set; }
}
public class TokenResponse
{
    public string access_token { get; set; }
    // 其他可能的字段
}
public class GoodList {
    public Participant Participant { get; set; }
    public LotteryActivity LotteryActivity { get; set; }
}