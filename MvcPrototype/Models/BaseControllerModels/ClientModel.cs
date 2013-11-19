using SFAFGlobalObjects;

namespace MvcPrototype.BaseModels
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
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}