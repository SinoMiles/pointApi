﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GWeb.library
{
    public interface IJwtAuth
    {
        string Authentication(string username, string password);
        string WeChatAuthentication(string openId);
    }
}
