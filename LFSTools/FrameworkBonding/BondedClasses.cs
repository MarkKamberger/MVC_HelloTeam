using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using SFAFGlobalObjects;
using System.Configuration;
using SALI;
using System.IO;
using SFAFUtilityObjects;
using SFABase;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Web.SessionState;
using System.Web.Caching;

namespace LFSTools
{
    public class BasePage : System.Web.UI.Page
    {
        private static string[] aspNetFormElements = new string[]
            {
                "__EVENTTARGET",
                "__EVENTARGUMENT",
                "__VIEWSTATE",
                "__EVENTVALIDATION",
                "__VIEWSTATEENCRYPTED",
            };

        protected override void Render(HtmlTextWriter writer)
        {
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
            base.Render(htmlWriter);
            string html = stringWriter.ToString();
            int formStart = html.IndexOf("<form");
            int endForm = -1;
            if (formStart >= 0)
                endForm = html.IndexOf(">", formStart);

            if (endForm >= 0)
            {
                StringBuilder viewStateBuilder = new StringBuilder();
                foreach (string element in aspNetFormElements)
                {
                    int startPoint = html.IndexOf("<input type=\"hidden\" name=\"" + element + "\"");
                    if (startPoint >= 0 && startPoint > endForm)
                    {
                        int endPoint = html.IndexOf("/>", startPoint);
                        if (endPoint >= 0)
                        {
                            endPoint += 2;
                            int length = endPoint - startPoint;

                            if (length > 0)
                            {
                                string viewStateInput = html.Substring(startPoint, length);
                                html = html.Remove(startPoint, endPoint - startPoint);
                                viewStateBuilder.Append(viewStateInput).Append("\r\n");
                            }
                        }
                    }
                }

                if (viewStateBuilder.Length > 0)
                {
                    viewStateBuilder.Insert(0, "\r\n");
                    html = html.Insert(endForm + 1, viewStateBuilder.ToString());
                }
            }

            writer.Write(html);
        }
    }

    public class BondedContentPage : BasePage
    {
        #region Public Variables

        public string _intranetLocation = "intranet2-dev1";

        #endregion

        #region Private Variables

        private List<BondedSecurityControl> _securityControls;

        private bool _isRedirecting = false;

        private DataAccessTypes _accessType = DataAccessTypes.NetworkDatabase;

        #endregion

        #region Constructor/Destructor Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="BondedContentPage"/> class.
        /// </summary>
        public BondedContentPage()
        {
            _securityControls = new List<BondedSecurityControl>();

            _intranetLocation = StringHelpers.ReturnAsString(ConfigurationManager.AppSettings["IntranetLocation"]);
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the get user preferences.
        /// </summary>
        /// <value>
        /// The get user preferences.
        /// </value>
        private object[] GetUserPreferences
        {
            get
            {
                List<StoredProcParameter> input = new List<StoredProcParameter>();
                input.Add(new StoredProcParameter(System.Data.DbType.String, "@UserName",
                                                  BondedMasterPage.CurrentUserName, int.MaxValue));
                List<StoredProcParameter> output = new List<StoredProcParameter>();
                output.Add(new StoredProcParameter(System.Data.DbType.String, "@TopNavBehavior", false, sizeof (bool)));
                output.Add(new StoredProcParameter(System.Data.DbType.String, "@PlayChatSound", false, sizeof (bool)));
                output.Add(new StoredProcParameter(System.Data.DbType.String, "@PlayMsgSound", false, sizeof (bool)));

                DatabaseExecution.ExecuteStoredProcNonQuery("_SALI_GetMemberCenter20UserPreferences", input, output,
                                                            ConfigurationManager.AppSettings["Database"]);

                return new object[] {output[0].Value, output[1].Value, output[2].Value};
            }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets the bonded master page.
        /// </summary>
        /// <value>
        /// The bonded master page.
        /// </value>
        public BondedMaster BondedMasterPage
        {
            get { return this.Master as BondedMaster; }
        }


        /// <summary>
        /// Gets or sets the type of the access.
        /// </summary>
        /// <value>
        /// The type of the access.
        /// </value>
        protected DataAccessTypes AccessType
        {
            get { return _accessType; }
            set { _accessType = value; }
        }



        /// <summary>
        /// Gets the current web page with URL.
        /// </summary>
        /// <value>
        /// The current web page with URL.
        /// </value>
        protected string CurrentWebPageWithURL
        {
            get
            {
                string qs = Request.QueryString.ToString().Trim();

                if (!string.IsNullOrEmpty(qs))
                    return Request.Url.ToString() + "?" + Request.QueryString.ToString();
                else
                    return Request.Url.ToString();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the current physical path.
        /// </summary>
        /// <value>
        /// The current physical path.
        /// </value>
        public string CurrentPhysicalPath
        {
            get
            {
                string path = this.Request.PhysicalApplicationPath.Replace("\\", "/");
                return SFAFileFunction.FormatDirectory(path);
            }
        }

        /// <summary>
        /// Gets the current customer id.
        /// </summary>
        /// <value>
        /// The current customer id.
        /// </value>
        public int CurrentCustomerId
        {
            get
            {
                return
                    NumberHelpers.ReturnAsInt(
                        Session[((BondedMaster) this.Page.Master).SFAFAppName + "CurrentCustomerId"]);
            }
        }

        /// <summary>
        /// Gets or sets the type of the current user.
        /// </summary>
        /// <value>
        /// The type of the current user.
        /// </value>
        public SALIUserTypes CurrentUserType
        {
            get { return MasterPresenter.CurrentUserType; }
            set { MasterPresenter.CurrentUserType = value; }
        }

        /// <summary>
        /// Gets the current school year string.
        /// </summary>
        /// <value>
        /// The current school year string.
        /// </value>
        public string CurrentSchoolYearString
        {
            get
            {
                return
                    ((SALISchoolYearEnum) BondedMasterPage.BusinessLogicObject.GetSchoolYears().ActualObject)
                        .ItemValueByID(BondedMasterPage.BusinessLogicObject.GetCurrentSchoolYear());
            }
        }

        /// <summary>
        /// Gets the current school year id.
        /// </summary>
        /// <value>
        /// The current school year id.
        /// </value>
        public int CurrentSchoolYearId
        {
            get { return BondedMasterPage.BusinessLogicObject.GetCurrentSchoolYear(); }
        }

        /// <summary>
        /// Gets the master presenter.
        /// </summary>
        /// <value>
        /// The master presenter.
        /// </value>
        public ISFAFPresentation MasterPresenter
        {
            get { return ((BondedMaster) this.Master).MasterPresenter; }
        }

        /// <summary>
        /// Gets the name of the security object.
        /// </summary>
        /// <value>
        /// The name of the security object.
        /// </value>
        public virtual string SecurityObjectName
        {
            get { return string.Empty; }
        }

        #endregion

        #region

        /// <summary>
        /// Gets the page security controls.
        /// </summary>
        /// <value>
        /// The page security controls.
        /// </value>
        public List<BondedSecurityControl> PageSecurityControls
        {
            get { return _securityControls; }
        }

        /// <summary>
        /// Determines whether [is login page].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is login page]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsLoginPage()
        {
            return false;
        }



        /// <summary>
        /// Gets the help page URL.
        /// </summary>
        /// <value>
        /// The help page URL.
        /// </value>
        public virtual string HelpPageUrl
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets a value indicating whether [show security icon].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show security icon]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool ShowSecurityIcon
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the additional security info.
        /// </summary>
        /// <value>
        /// The additional security info.
        /// </value>
        public virtual string AdditionalSecurityInfo
        {
            get { return string.Empty; }
        }

        #endregion









        /// <summary>
        /// Security Class
        /// </summary>
        public class BondedSecurityControl
        {
            private Control _securityControl = null;
            private SALIWebControlTypes _controlType = SALIWebControlTypes.None;
            private List<string> _objectType = new List<string>();
            private List<string> _minimumSecurityLevel = new List<string>();
            private string _minimumScopeLevel = string.Empty;
            private bool _isSaveButton = false;

            /// <summary>
            /// Initializes a new instance of the <see cref="BondedSecurityControl"/> class.
            /// </summary>
            /// <param name="securityControl">The security control.</param>
            /// <param name="controlType">Type of the control.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <param name="minimumSecurityLevel">The minimum security level.</param>
            /// <param name="minimumScopeLevel">The minimum scope level.</param>
            public BondedSecurityControl(Control securityControl, SALIWebControlTypes controlType, string objectType,
                                         string minimumSecurityLevel, string minimumScopeLevel)
            {
                SecurityControl = securityControl;
                _objectType.Add(objectType);
                _minimumSecurityLevel.Add(minimumSecurityLevel);
                MinimumScopeLevel = minimumScopeLevel;
                ControlType = controlType;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BondedSecurityControl"/> class.
            /// </summary>
            /// <param name="securityControl">The security control.</param>
            /// <param name="controlType">Type of the control.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <param name="minimumSecurityLevel">The minimum security level.</param>
            /// <param name="minimumScopeLevel">The minimum scope level.</param>
            /// <param name="isSaveButton">if set to <c>true</c> [is save button].</param>
            public BondedSecurityControl(Control securityControl, SALIWebControlTypes controlType, string objectType,
                                         string minimumSecurityLevel, string minimumScopeLevel, bool isSaveButton)
            {
                SecurityControl = securityControl;
                _objectType.Add(objectType);
                _minimumSecurityLevel.Add(minimumSecurityLevel);
                MinimumScopeLevel = minimumScopeLevel;
                ControlType = controlType;
                _isSaveButton = isSaveButton;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BondedSecurityControl"/> class.
            /// </summary>
            /// <param name="securityControl">The security control.</param>
            /// <param name="controlType">Type of the control.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <param name="minimumSecurityLevel">The minimum security level.</param>
            /// <param name="minimumScopeLevel">The minimum scope level.</param>
            public BondedSecurityControl(Control securityControl, SALIWebControlTypes controlType, string[] objectType,
                                         string minimumSecurityLevel, string minimumScopeLevel)
            {
                SecurityControl = securityControl;

                for (int x = 0; x < objectType.Length; x++)
                    _objectType.Add(objectType[x]);

                _minimumSecurityLevel.Add(minimumSecurityLevel);
                MinimumScopeLevel = minimumScopeLevel;
                ControlType = controlType;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BondedSecurityControl"/> class.
            /// </summary>
            /// <param name="securityControl">The security control.</param>
            /// <param name="controlType">Type of the control.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <param name="minimumSecurityLevel">The minimum security level.</param>
            /// <param name="minimumScopeLevel">The minimum scope level.</param>
            /// <param name="isSaveButton">if set to <c>true</c> [is save button].</param>
            public BondedSecurityControl(Control securityControl, SALIWebControlTypes controlType, string[] objectType,
                                         string minimumSecurityLevel, string minimumScopeLevel, bool isSaveButton)
            {
                SecurityControl = securityControl;

                for (int x = 0; x < objectType.Length; x++)
                    _objectType.Add(objectType[x]);

                _minimumSecurityLevel.Add(minimumSecurityLevel);
                MinimumScopeLevel = minimumScopeLevel;
                ControlType = controlType;
                _isSaveButton = isSaveButton;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BondedSecurityControl"/> class.
            /// </summary>
            /// <param name="securityControl">The security control.</param>
            /// <param name="controlType">Type of the control.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <param name="minimumSecurityLevel">The minimum security level.</param>
            /// <param name="minimumScopeLevel">The minimum scope level.</param>
            public BondedSecurityControl(Control securityControl, SALIWebControlTypes controlType, string[] objectType,
                                         string[] minimumSecurityLevel, string minimumScopeLevel)
            {
                SecurityControl = securityControl;

                for (int x = 0; x < objectType.Length; x++)
                    _objectType.Add(objectType[x]);

                for (int x = 0; x < minimumSecurityLevel.Length; x++)
                    _minimumSecurityLevel.Add(minimumSecurityLevel[x]);

                MinimumScopeLevel = minimumScopeLevel;
                ControlType = controlType;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BondedSecurityControl"/> class.
            /// </summary>
            /// <param name="securityControl">The security control.</param>
            /// <param name="controlType">Type of the control.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <param name="minimumSecurityLevel">The minimum security level.</param>
            /// <param name="minimumScopeLevel">The minimum scope level.</param>
            /// <param name="isSaveButton">if set to <c>true</c> [is save button].</param>
            public BondedSecurityControl(Control securityControl, SALIWebControlTypes controlType, string[] objectType,
                                         string[] minimumSecurityLevel, string minimumScopeLevel, bool isSaveButton)
            {
                SecurityControl = securityControl;

                for (int x = 0; x < objectType.Length; x++)
                    _objectType.Add(objectType[x]);

                for (int x = 0; x < minimumSecurityLevel.Length; x++)
                    _minimumSecurityLevel.Add(minimumSecurityLevel[x]);

                MinimumScopeLevel = minimumScopeLevel;
                ControlType = controlType;
                _isSaveButton = isSaveButton;
            }

            /// <summary>
            /// Gets the security control.
            /// </summary>
            /// <value>
            /// The security control.
            /// </value>
            public Control SecurityControl
            {
                get { return _securityControl; }
                private set { _securityControl = value; }
            }

            /// <summary>
            /// Gets the minimum security level.
            /// </summary>
            /// <value>
            /// The minimum security level.
            /// </value>
            public string[] MinimumSecurityLevel
            {
                get { return _minimumSecurityLevel.ToArray(); }
            }

            public string MinimumScopeLevel
            {
                get { return _minimumScopeLevel; }
                private set { _minimumScopeLevel = value; }
            }

            /// <summary>
            /// Gets the type of the object.
            /// </summary>
            /// <value>
            /// The type of the object.
            /// </value>
            public string[] ObjectType
            {
                get { return _objectType.ToArray(); }
            }

            /// <summary>
            /// Gets the type of the control.
            /// </summary>
            /// <value>
            /// The type of the control.
            /// </value>
            public SALIWebControlTypes ControlType
            {
                get { return _controlType; }
                private set { _controlType = value; }
            }

            /// <summary>
            /// Gets a value indicating whether this instance is save button.
            /// </summary>
            /// <value>
            /// <c>true</c> if this instance is save button; otherwise, <c>false</c>.
            /// </value>
            public bool IsSaveButton
            {
                get { return _isSaveButton; }
            }
        }

        /// <summary>
        /// Presenter Class
        /// </summary>
        [Serializable]
        public class BondedPresenter : ISFAFPresentation
        {
            private object Session;
            private string SFAFAppName;
            private bool ENABLE_GLOBAL_CACHE = false;
            private Cache Cache = null;

            private List<string> _cacheKeys = new List<string>();

            /// <summary>
            /// Initializes a new instance of the <see cref="BondedPresenter"/> class.
            /// </summary>
            /// <param name="session">The session.</param>
            /// <param name="appName">Name of the app.</param>
            /// <param name="enableGlobalCache">if set to <c>true</c> [enable global cache].</param>
            /// <param name="cache">The cache.</param>
            public BondedPresenter(HttpSessionState session, string appName, bool enableGlobalCache, Cache cache)
            {
                Session = session;
                SFAFAppName = appName;
                ENABLE_GLOBAL_CACHE = enableGlobalCache;
                Cache = cache;
            }

            #region ISFAFPresentation Members

            /// <summary>
            /// Gets or sets the type of the current user.
            /// </summary>
            /// <value>
            /// The type of the current user.
            /// </value>
            public SALIUserTypes CurrentUserType
            {
                get
                {
                    SALIUserTypes result = SALIUserTypes.Unknown;

                    if (((HttpSessionState) Session)["Current" + SFAFAppName + "UserType"] != null)
                        result = (SALIUserTypes) ((HttpSessionState) Session)["Current" + SFAFAppName + "UserType"];

                    return result;
                }
                set { ((HttpSessionState) Session)["Current" + SFAFAppName + "UserType"] = value; }
            }

            /// <summary>
            /// Gets or sets the current session id.
            /// </summary>
            /// <value>
            /// The current session id.
            /// </value>
            public string CurrentSessionId
            {
                get
                {
                    string result = string.Empty;

                    if (((HttpSessionState) Session)["Current" + SFAFAppName + "SessionId"] != null)
                        result = ((HttpSessionState) Session)["Current" + SFAFAppName + "SessionId"].ToString();

                    return result;
                }
                set { ((HttpSessionState) Session)["Current" + SFAFAppName + "SessionId"] = value; }
            }

            /// <summary>
            /// Gets or sets the current login id.
            /// </summary>
            /// <value>
            /// The current login id.
            /// </value>
            public string CurrentLoginId
            {
                get
                {
                    string result = string.Empty;

                    if (((HttpSessionState) Session)["Current" + SFAFAppName + "LoginId"] != null)
                        result = ((HttpSessionState) Session)["Current" + SFAFAppName + "LoginId"].ToString();

                    return result;
                }
                set { ((HttpSessionState) Session)["Current" + SFAFAppName + "LoginId"] = value; }
            }

            /// <summary>
            /// Gets or sets the name of the current user.
            /// </summary>
            /// <value>
            /// The name of the current user.
            /// </value>
            public string CurrentUserName
            {
                get
                {
                    string result = string.Empty;

                    if (((HttpSessionState) Session)["Current" + SFAFAppName + "UserName"] != null)
                        result = ((HttpSessionState) Session)["Current" + SFAFAppName + "UserName"].ToString();

                    return result;
                }
                set { ((HttpSessionState) Session)["Current" + SFAFAppName + "UserName"] = value; }
            }

            /// <summary>
            /// Gets or sets the current user password.
            /// </summary>
            /// <value>
            /// The current user password.
            /// </value>
            public string CurrentUserPassword
            {
                get
                {
                    string result = string.Empty;

                    if (((HttpSessionState) Session)["Current" + SFAFAppName + "Password"] != null)
                        result = ((HttpSessionState) Session)["Current" + SFAFAppName + "Password"].ToString();

                    return result;
                }
                set { ((HttpSessionState) Session)["Current" + SFAFAppName + "Password"] = value; }
            }

            /// <summary>
            /// Gets or sets the first name of the current user.
            /// </summary>
            /// <value>
            /// The first name of the current user.
            /// </value>
            public string CurrentUserFirstName
            {
                get
                {
                    string result = string.Empty;

                    if (((HttpSessionState) Session)["Current" + SFAFAppName + "FirstName"] != null)
                        result = ((HttpSessionState) Session)["Current" + SFAFAppName + "FirstName"].ToString();

                    return result;
                }
                set { ((HttpSessionState) Session)["Current" + SFAFAppName + "FirstName"] = value; }
            }

            /// <summary>
            /// Gets or sets the name of the current user middle.
            /// </summary>
            /// <value>
            /// The name of the current user middle.
            /// </value>
            public string CurrentUserMiddleName
            {
                get
                {
                    string result = string.Empty;

                    if (((HttpSessionState) Session)["Current" + SFAFAppName + "MiddletName"] != null)
                        result = ((HttpSessionState) Session)["Current" + SFAFAppName + "MiddletName"].ToString();

                    return result;
                }
                set { ((HttpSessionState) Session)["Current" + SFAFAppName + "MiddletName"] = value; }
            }

            /// <summary>
            /// Gets or sets the last name of the current user.
            /// </summary>
            /// <value>
            /// The last name of the current user.
            /// </value>
            public string CurrentUserLastName
            {
                get
                {
                    string result = string.Empty;

                    if (((HttpSessionState) Session)["Current" + SFAFAppName + "LastName"] != null)
                        result = ((HttpSessionState) Session)["Current" + SFAFAppName + "LastName"].ToString();

                    return result;
                }
                set { ((HttpSessionState) Session)["Current" + SFAFAppName + "LastName"] = value; }
            }

            /// <summary>
            /// Gets or sets the name of the current customer.
            /// </summary>
            /// <value>
            /// The name of the current customer.
            /// </value>
            public string CurrentCustomerName
            {
                get
                {
                    string result = string.Empty;

                    if (((HttpSessionState) Session)[SFAFAppName + "CurrentCustomerName"] != null)
                        result = (string) ((HttpSessionState) Session)[SFAFAppName + "CurrentCustomerName"];

                    return result;
                }
                set { ((HttpSessionState) Session)[SFAFAppName + "CurrentCustomerName"] = value; }
            }

            /// <summary>
            /// Gets or sets the current school year.
            /// </summary>
            /// <value>
            /// The current school year.
            /// </value>
            public string CurrentSchoolYear
            {
                get
                {
                    string result = string.Empty;

                    if (((HttpSessionState) Session)["Current" + SFAFAppName + "SchoolYear"] != null)
                        result = (string) ((HttpSessionState) Session)["Current" + SFAFAppName + "SchoolYear"];

                    return result;
                }
                set { ((HttpSessionState) Session)["Current" + SFAFAppName + "SchoolYear"] = value; }
            }

            /// <summary>
            /// Gets or sets the client browser.
            /// </summary>
            /// <value>
            /// The client browser.
            /// </value>
            public string ClientBrowser
            {
                get { return StringHelpers.ReturnAsString(((HttpSessionState) Session)[SFAFAppName + "Browser"]); }
                set { ((HttpSessionState) Session)[SFAFAppName + "Browser"] = value; }
            }

            /// <summary>
            /// Gets or sets the client browser version.
            /// </summary>
            /// <value>
            /// The client browser version.
            /// </value>
            public string ClientBrowserVersion
            {
                get { return StringHelpers.ReturnAsString(((HttpSessionState) Session)[SFAFAppName + "BrowserVersion"]); }
                set { ((HttpSessionState) Session)[SFAFAppName + "BrowserVersion"] = value; }
            }

            /// <summary>
            /// Gets or sets the current user id.
            /// </summary>
            /// <value>
            /// The current user id.
            /// </value>
            public int CurrentUserId
            {
                get
                {
                    int result = -1;

                    if (((HttpSessionState) Session)["Current" + SFAFAppName + "UserId"] != null)
                        result =
                            NumberHelpers.ReturnAsInt(
                                StringHelpers.ReturnAsString(
                                    ((HttpSessionState) Session)["Current" + SFAFAppName + "UserId"]));

                    return result;
                }
                set { ((HttpSessionState) Session)["Current" + SFAFAppName + "UserId"] = value; }
            }

            /// <summary>
            /// Gets or sets the current customer id.
            /// </summary>
            /// <value>
            /// The current customer id.
            /// </value>
            public int CurrentCustomerId
            {
                get
                {
                    int result = -1;

                    if (((HttpSessionState) Session)[SFAFAppName + "CurrentCustomerId"] != null)
                        result =
                            NumberHelpers.ReturnAsInt(
                                ((HttpSessionState) Session)[SFAFAppName + "CurrentCustomerId"].ToString());

                    return result;
                }
                set { ((HttpSessionState) Session)[SFAFAppName + "CurrentCustomerId"] = value; }
            }

            /// <summary>
            /// Gets or sets the current school year id.
            /// </summary>
            /// <value>
            /// The current school year id.
            /// </value>
            public int CurrentSchoolYearId
            {
                get
                {
                    int result = -1;

                    if (((HttpSessionState) Session)["Current" + SFAFAppName + "SchoolYearId"] != null)
                        result = (int) ((HttpSessionState) Session)["Current" + SFAFAppName + "SchoolYearId"];

                    return result;
                }
                set { ((HttpSessionState) Session)["Current" + SFAFAppName + "SchoolYearId"] = value; }
            }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is demo.
            /// </summary>
            /// <value>
            ///   <c>true</c> if this instance is demo; otherwise, <c>false</c>.
            /// </value>
            public bool IsDemo
            {
                get
                {
                    bool result = false;

                    if (((HttpSessionState) Session)["Current" + SFAFAppName + "IsDemo"] != null)
                        result = (bool) ((HttpSessionState) Session)["Current" + SFAFAppName + "IsDemo"];

                    return result;
                }
                set { ((HttpSessionState) Session)["Current" + SFAFAppName + "IsDemo"] = value; }
            }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is district.
            /// </summary>
            /// <value>
            /// <c>true</c> if this instance is district; otherwise, <c>false</c>.
            /// </value>
            public bool IsDistrict
            {
                get
                {
                    bool result = false;

                    if (((HttpSessionState) Session)["Current" + SFAFAppName + "IsDistrict"] != null)
                        result = (bool) ((HttpSessionState) Session)["Current" + SFAFAppName + "IsDistrict"];

                    return result;
                }
                set { ((HttpSessionState) Session)["Current" + SFAFAppName + "IsDistrict"] = value; }
            }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is track school.
            /// </summary>
            /// <value>
            /// <c>true</c> if this instance is track school; otherwise, <c>false</c>.
            /// </value>
            public bool IsTrackSchool
            {
                get
                {
                    bool result = false;

                    if (((HttpSessionState) Session)["Current" + SFAFAppName + "IsTrackSchool"] != null)
                        result = (bool) ((HttpSessionState) Session)["Current" + SFAFAppName + "IsTrackSchool"];

                    return result;
                }
                set { ((HttpSessionState) Session)["Current" + SFAFAppName + "IsTrackSchool"] = value; }
            }

            public void WriteMessage(string message)
            {
            }

            /// <summary>
            /// Saves to presenter cache.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public bool SaveToPresenterCache(string key, object value)
            {
                bool result = false;

                key = key.Trim();

                if (!string.IsNullOrEmpty(key))
                {
                    try
                    {
                        ((HttpSessionState) Session)[SFAFAppName + key] = value;

                        if (!_cacheKeys.Contains(SFAFAppName + key))
                            _cacheKeys.Add(SFAFAppName + key);

                        ((HttpSessionState) Session)[SFAFAppName + "CacheKeys"] = _cacheKeys;

                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                }

                return result;
            }

            /// <summary>
            /// Gets from presenter cache.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            public object GetFromPresenterCache(string key)
            {
                return ((HttpSessionState) Session)[SFAFAppName + key];
            }

            /// <summary>
            /// Clears the presenter cache.
            /// </summary>
            /// <returns></returns>
            public bool ClearPresenterCache()
            {
                bool result = false;

                for (int x = 0; x < _cacheKeys.Count; x++)
                {
                    try
                    {
                        ((HttpSessionState) Session)[_cacheKeys[x]] = null;
                    }
                    catch
                    {
                    }

                    try
                    {
                        ((HttpSessionState) Session).Remove(_cacheKeys[x]);
                    }
                    catch
                    {
                    }
                }

                try
                {
                    _cacheKeys.Clear();
                }
                catch
                {
                }

                return result;
            }

            /// <summary>
            /// Deletes from presenter cache.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            public bool DeleteFromPresenterCache(string key)
            {
                bool result = false;

                for (int x = 0; x < _cacheKeys.Count; x++)
                {
                    if (_cacheKeys[x] == SFAFAppName + key)
                    {
                        try
                        {
                            ((HttpSessionState) Session)[_cacheKeys[x]] = null;
                        }
                        catch
                        {
                        }

                        try
                        {
                            ((HttpSessionState) Session).Remove(_cacheKeys[x]);
                        }
                        catch
                        {
                        }

                        try
                        {
                            _cacheKeys.Remove(_cacheKeys[x]);
                        }
                        catch
                        {
                        }

                        result = true;
                        break;
                    }
                }

                return result;
            }

            /// <summary>
            /// Saves to global cache.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public bool SaveToGlobalCache(string key, object value)
            {
                bool result = true;

                if (ENABLE_GLOBAL_CACHE)
                {
                    IDictionaryEnumerator e = Cache.GetEnumerator();

                    bool found = false;

                    while (e.MoveNext())
                    {
                        if (e.Key == key)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        DateTime exp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                        Cache.Insert(key, value, null, exp, System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        Cache[key] = value;
                    }
                }

                return result;
            }

            /// <summary>
            /// Gets from global cache.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            public object GetFromGlobalCache(string key)
            {
                object result = null;

                if (ENABLE_GLOBAL_CACHE)
                    result = Cache.Get(key);

                return result;
            }

            /// <summary>
            /// Deletes from global cache.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            public bool DeleteFromGlobalCache(string key)
            {
                if (ENABLE_GLOBAL_CACHE)
                {
                    try
                    {
                        Cache.Remove(key);
                    }
                    catch
                    {
                    }
                }

                return true;
            }

            /// <summary>
            /// Clears the global cache.
            /// </summary>
            /// <returns></returns>
            public bool ClearGlobalCache()
            {
                if (ENABLE_GLOBAL_CACHE)
                {
                    IDictionaryEnumerator e = Cache.GetEnumerator();

                    List<string> keys = new List<string>();

                    while (e.MoveNext())
                    {
                        keys.Add(e.Key.ToString());
                    }

                    for (int x = 0; x < keys.Count; x++)
                    {
                        try
                        {
                            Cache.Remove(keys[x]);
                        }
                        catch
                        {
                        }
                    }
                }

                return true;
            }

            #endregion
        }






        public class ViewStateParser
        {
            #region Fields

            private readonly XmlDocument _controlState;

            private readonly string _text;

            private readonly object _value;

            private readonly XmlDocument _viewState;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="ViewStateParser"/> class.
            /// </summary>
            /// <param name="fieldValue">The field value.</param>
            public ViewStateParser(string fieldValue)
            {
                _text = fieldValue;

                (_viewState = new XmlDocument())
                    .AppendChild(_viewState.CreateElement("viewstate"));

                (_controlState = new XmlDocument())
                    .AppendChild(_controlState.CreateElement("controlstate"));

                _value = new LosFormatter().Deserialize(_text);

                XmlSerializeValue(_viewState, _controlState, _viewState.DocumentElement, _value);
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the state of the control.
            /// </summary>
            /// <value>
            /// The state of the control.
            /// </value>
            public XmlDocument ControlState
            {
                get { return _controlState; }
            }

            /// <summary>
            /// The unencoded __ViewState
            /// </summary>
            /// <value>
            /// The text.
            /// </value>
            public string Text
            {
                get { return _text; }
            }


            /// <summary>
            /// The ViewState object
            /// </summary>
            /// <value>
            /// The value.
            /// </value>
            public object Value
            {
                get { return _value; }
            }


            /// <summary>
            /// Gets the state of the view.
            /// </summary>
            /// <value>
            /// The state of the view.
            /// </value>
            public XmlDocument ViewState
            {
                get { return _viewState; }
            }

            #endregion

            #region Private Methods

            /// <summary>
            /// XMLs the serialize value.
            /// </summary>
            /// <param name="viewState">State of the view.</param>
            /// <param name="controlState">State of the control.</param>
            /// <param name="parentNode">The parent node.</param>
            /// <param name="value">The value.</param>
            private static void XmlSerializeValue(XmlDocument viewState, XmlDocument controlState,
                                                  XmlNode parentNode,
                                                  object value)
            {
                if (value != null)
                {
                    string typename = value.GetType().Name;

                    if (typename == "HybridDictionary")
                    {
                        XmlElement childNode = controlState.CreateElement(typename);

                        HybridDictionary hybridDictionary = (HybridDictionary) value;

                        if (controlState.DocumentElement != null)
                        {
                            controlState.DocumentElement.AppendChild(childNode);

                            foreach (object item in hybridDictionary)
                            {
                                XmlSerializeValue(controlState, controlState, childNode, item);
                            }
                        }
                    }
                    else
                    {
                        XmlElement childNode = viewState.CreateElement(typename);

                        parentNode.AppendChild(childNode);

                        switch (typename)
                        {
                            case "Triplet":
                                Triplet triplet = (Triplet) value;

                                XmlSerializeValue(viewState, controlState, childNode, triplet.First);
                                XmlSerializeValue(viewState, controlState, childNode, triplet.Second);
                                XmlSerializeValue(viewState, controlState, childNode, triplet.Third);
                                break;

                            case "Pair":
                                Pair pair = (Pair) value;
                                XmlSerializeValue(viewState, controlState, childNode, pair.First);
                                XmlSerializeValue(viewState, controlState, childNode, pair.Second);
                                break;

                            case "ArrayList":
                                foreach (object item in (ArrayList) value)
                                {
                                    XmlSerializeValue(viewState, controlState, childNode, item);
                                }
                                break;

                            case "Array":
                                foreach (object item in (Array) value)
                                {
                                    XmlSerializeValue(viewState, controlState, childNode, item);
                                }
                                break;

                            case "DictionaryEntry":
                                DictionaryEntry dictionaryEntry = (DictionaryEntry) value;
                                XmlSerializeValue(viewState, controlState, childNode, dictionaryEntry.Key);
                                XmlSerializeValue(viewState, controlState, childNode, dictionaryEntry.Value);
                                break;

                            case "IndexedString":
                                childNode.InnerText = ((IndexedString) value).Value;
                                break;

                            default:
                                childNode.InnerText = value.ToString();
                                break;
                        }
                    }
                }
            }

            #endregion
        }
    }
}
