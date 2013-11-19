using System;
using System.Collections.Generic;
using System.Text;
using SALIBusinessLogic;
using Telerik.Web.UI;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SFAFGlobalObjects;

namespace MemberCenter20NS
{
    public interface ISFAFMemberCenter20Page
    {
        bool IsLoginPage();
        void SetErrorMsg(string message);
        List<SFAFMemberCenter20SecurityControl> PageSecurityControls { get; }
        void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e);
        string HelpPageUrl { get; }
        void SFAFPageInit(object sender, EventArgs e);
        string SecurityObjectName { get; }
        bool ShowSecurityIcon { get; }
        string AdditionalSecurityInfo { get; }
    }

    public interface ISFAFMemberCenter20Master
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
        RadWindowManager MasterWindowManager { get; }
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

    public interface ISFAFMemberCenter20NestedMaster
    {
        bool IsMasterLoggingOut { get; set; }
    }

    public interface ISFAFMemberCenter20NestedMasterContent
    {
        void SetPageTitle(string centerName, string pageName);
        bool ShowSuccess{ set; }
        bool ShowFail { set; }
        void SetSuccessMessage(string message);
        void SetFailMessage(string message);
        void ShowHelpIcon();
    }

    public interface ISFAFMemberCenter20CustomerCollection
    {
        string CustomerName(int customerId);
        int CurrentCustomerId { get; }
        string CurrentCustomerName { get; }
        void SetCustomerName(string customerName);
    }

    public interface ISFAFMemberCenter20NavigationPage
    {
        void CenterLinkClick(string center);
    }

    public interface ISFAFMemberCenter20MasterNavigation
    {
        string[] Centers { get; }
        string[] CenterImages { get; }
    }

    public interface ISFAFMemberCenter20SearchPage
    {
        bool HideAdvancedSearch { get; }
        void Search_Click(object sender, EventArgs e);
        SortedList<string, bool> PossibleResultFields { get; }
    }

    public interface ISFAFMemberCenterEditMaster
    {
        void LoadList(NameValueCollection list);
        void LoadList(NameValueCollection list, bool selectAllItems);
        void SetMessage(string message);
        void AssignSecurityToSave(string objectType, string privilegeLevel, string scope);
        void AssignSecurityToSave(string[] objectType, string[] privilegeLevel, string[] scope);
        void HideSaveButton();
        string CurrentSelectedItem { get; }
        string CurrentSelectedItemText { get; }
        List<string> GetSelectedItems();
    }

    public interface ISFAFMemberCenterEditPage
    {
        void Item_Changed(string value);
        bool Save_Click(string[] value);
        bool AllowMultiSelection { get; }
        List<SFAFMemberCenter20RequiredFieldControl> RequiredControls { get; }
    }

    public interface ISFAFMemberCenter20RadWindowContentPage
    {
        bool RefreshParentOnClose { get; }
    }

    public interface ISFAFMemberCenter20RadWindowMaster
    {
        void AddCloseDelegate(Delegate del);
        void SetParentRefresh();
        void HideCloseButton();
    }

    public interface ISFAFMemberCenter20RadWindowContentMaster
    {
        void SetPageTitle(string centerName, string pageName, string windowName);
        void SetPageTitle(string centerName, string pageName, string windowName, string overrideColor);
    }

    public interface ISFAFMemberCenter20SearchMaster
    {
        string[] SelectedResultFields { get; }
        string SearchButtonClientId { get; }
        void DisableSearchPostback();
        void SetSearchButtonClientOnClick(string clickScript, bool preventPostback);
    }

    public interface ISFAFMemberCenter20RadWindowSupportMaster
    {
        string OperatingSystemClientID { get; }
        string OperatingSystemVersionClientID { get; }
        string BrowserClientID { get; }
        string BrowserVersionClientID { get; }
        string UserAgentClientID { get; }
    }

    public interface ISFAFMemberCenter20RadWindowSubForm
    {
        void SetHeaderLabel(string header);
    }

    public interface ISFAFMemberCenter20NewsContainer
    {
        void SetContinueMessage(string message);
    }
}
