using System;
using System.Collections.Generic;
using System.Text;
using SALIBusinessLogic;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SFAFGlobalObjects;

namespace MvcPrototype
{
    public interface BondedPage
    {
        bool IsLoginPage();
        void SetErrorMsg(string message);
        List<BondedContentPage.BondedSecurityControl> PageSecurityControls { get; }
        string HelpPageUrl { get; }
        void SFAFPageInit(object sender, EventArgs e);
        string SecurityObjectName { get; }
        bool ShowSecurityIcon { get; }
        string AdditionalSecurityInfo { get; }
    }

    public interface BondedMaster
    {
        SALIMainBusinessLogic BusinessLogicObject { get; }
        void Logout();
        void Logout(bool showParameters);
        bool LoginCustomerContact(string userName, string password);
        bool LoginCustomerContact(string userName, string password, List<Delegate> postLoginDelegates);
        bool LoginCustomerContact(string userName, string password, List<Delegate> postLoginDelegates, bool isDemo);
        string SFAFAppName { get; }
        string CurrentUserName { get; }
        string CurrentUserPassword { get; }
        int CurrentUserId { get; }
        SALIUserTypes CurrentUserType { get; }
        string CurrentSessionId { get; }
        string CurrentLoginId { get; }
        void SetPageTitle(string title);
        string LeftBackColor { get; }
        string LeftForeColor { get; }
        bool NeedSave { get; }
        void ResetSaveRedirectInfo(string redirectUrl);
        ScriptManager MasterScriptManager { get; }
        string DefaultCalendarStartDate { get; set; }
        string DefaultCalendarEndDate { get; set; }
        string SetFontColorSessionLocation { get; }
        string SetCenterImageSessionLocation { get; }
        ISFAFPresentation MasterPresenter { get; }
        bool IsMobileBrowser();
        void SetClientInfo(string os, string osVersion, string browser, string browserVersion, string userAgent);
        string ClientOperatingSystem { get; }
        string ClientOperatingSystemVersion { get; }
        string ClientBrowser { get; }
        string ClientBrowserVersion { get; }
        string ClientUserAgent { get; }
        string CustomerListSessionName { get; }
    }
}
