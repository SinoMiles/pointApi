using System;
namespace Point.Controllers.dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public string HeadImgUrl { get; set; }
        public bool IsMerchant { get; set; }
    }
}

