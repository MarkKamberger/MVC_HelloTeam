using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MemberCenter20NS.Models;

namespace MvcHelloTeam.Models
{
    public class ClientModel 
    {
        public string Os { get; set; }
        public string OsVersion { get; set; }
        public string Browser { get; set; }
        public string BrowserVersion { get; set; }
        public string Message { get; set; }
        public bool LoginSuccess { get; set; }
        public bool IsLoggedIn { get; set; }
        public int CurrentLoginId { get; set; }
        public string CurrentSessionId { get; set; }

    }
}