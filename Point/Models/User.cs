using System;
using System.Collections.Generic;

#nullable disable

namespace GWeb.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string OpenId { get; set; }
        public string Phone { get; set; }
        public string NickName { get; set; } 
        public string HeadImgUrl { get; set; }
        public bool IsMerchant { get; set; } // 表示是否为商家身份
    }
}
