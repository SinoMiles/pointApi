using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GWeb.library
{
    public static class UserHelper
    {
        public static String getUser(Microsoft.AspNetCore.Http.HttpRequest Request, Models.girlContext _context,int loginType = 0)
        {
            var userName = Request.HttpContext.User.Identity.Name;
            if (userName == null)
            {
                return "" ;
            }
            else
            {
                var userId = 0;
                switch (loginType)
                {
                    //微信
                    case 0:
                        userId = _context.Users.Where(b => b.OpenId == userName).FirstOrDefault().Id;
                        break;
                    case 1:
                        userId = _context.Users.Where(b => b.UserName == userName).FirstOrDefault().Id;
                        break;
                    default:
                        break;
                   
                }
                return userId.ToString();
            }
        }
    }
}
      
    
        

