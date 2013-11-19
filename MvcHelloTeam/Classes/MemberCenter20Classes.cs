using System;
using System.Collections.Generic;
using System.Text;
using SALIBusinessLogic;
using System.Web.UI;
using SFAFGlobalObjects;
using System.Web;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using SALI;
using System.Drawing;
using System.IO;
using SFAFUtilityObjects;
using System.Text.RegularExpressions;
using SFABase;
using System.Web.Profile;
using System.Net;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Web.SessionState;
using System.Web.Caching;

namespace MemberCenter20NS
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

    public class SFAFMemberCenter20ContentPage : BasePage, ISFAFMemberCenter20Page
    {
        #region Public Variables

        public string _intranetLocation = "intranet2-dev1";
        
        #endregion

        #region Private Variables

        private List<SFAFMemberCenter20SecurityControl> _securityControls;

        private bool _isRedirecting = false;

        private DataAccessTypes _accessType = DataAccessTypes.NetworkDatabase;

        #endregion

        #region Constructor/Destructor Methods

        public SFAFMemberCenter20ContentPage()
        {
            _securityControls = new List<SFAFMemberCenter20SecurityControl>();

            _intranetLocation = ReturnAsString(ConfigurationManager.AppSettings["IntranetLocation"]);
        }

        #endregion

        #region Private Properties

        private object[] GetUserPreferences
        {
            get
            {
                List<StoredProcParameter> input = new List<StoredProcParameter>();
                input.Add(new StoredProcParameter(System.Data.DbType.String, "@UserName", SFAFMemberCenter20MasterPage.CurrentUserName, int.MaxValue));
                List<StoredProcParameter> output = new List<StoredProcParameter>();
                output.Add(new StoredProcParameter(System.Data.DbType.String, "@TopNavBehavior", false, sizeof(bool)));
                output.Add(new StoredProcParameter(System.Data.DbType.String, "@PlayChatSound", false, sizeof(bool)));
                output.Add(new StoredProcParameter(System.Data.DbType.String, "@PlayMsgSound", false, sizeof(bool)));

                DatabaseExecution.ExecuteStoredProcNonQuery("_SALI_GetMemberCenter20UserPreferences", input, output, ConfigurationManager.AppSettings["Database"]);

                return new object[] { output[0].Value, output[1].Value, output[2].Value };
            }
        }

        #endregion

        #region Protected Properties

        public ISFAFMemberCenter20Master SFAFMemberCenter20MasterPage
        {
            get
            {
                return this.Master as ISFAFMemberCenter20Master;
            }
        }

        protected Color StaticGridCellColor
        {
            get
            {
                return Color.Cornsilk;
            }
        }

        protected DataAccessTypes AccessType
        {
            get
            {
                return _accessType;
            }
            set
            {
                _accessType = value;
            }
        }

        protected string CurrentURL
        {
            get
            {
                string currentPageUrl = "";

                if (Request.ServerVariables["SERVER_PORT"].ToString().Trim() != "443")
                {
                    currentPageUrl = Request.ServerVariables["SERVER_PROTOCOL"].ToString().ToLower().Substring(0, 4).Replace("/", string.Empty) + "://" + Request.ServerVariables["SERVER_NAME"].ToString() + ":" + Request.ServerVariables["SERVER_PORT"].ToString() + Request.ServerVariables["SCRIPT_NAME"].ToString().Substring(0, Request.ServerVariables["SCRIPT_NAME"].ToString().LastIndexOf('/'));
                }
                else
                {
                    currentPageUrl = Request.ServerVariables["SERVER_PROTOCOL"].ToString().ToLower().Substring(0, 5).Replace("/", string.Empty) + "://" + Request.ServerVariables["SERVER_NAME"].ToString() + ":" + Request.ServerVariables["SERVER_PORT"].ToString() + Request.ServerVariables["SCRIPT_NAME"].ToString().Substring(0, Request.ServerVariables["SCRIPT_NAME"].ToString().LastIndexOf('/'));
                }

                if (!currentPageUrl.EndsWith("/"))
                    currentPageUrl += "/";

                return currentPageUrl;
            }
        }

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

        protected bool TopNavBehavior
        {
            get
            {
                return ReturnAsBool(ReturnAsString(GetUserPreferences[0]));
            }
            set
            {
                List<StoredProcParameter> input = new List<StoredProcParameter>();
                input.Add(new StoredProcParameter(System.Data.DbType.String, "@UserName", SFAFMemberCenter20MasterPage.CurrentUserName, int.MaxValue));
                input.Add(new StoredProcParameter(System.Data.DbType.String, "@TopNavBehavior", value, sizeof(bool)));
                List<StoredProcParameter> output = new List<StoredProcParameter>();

                DatabaseExecution.ExecuteStoredProcNonQuery("_SALI_SaveMemberCenter20UserPreferences", input, output, ConfigurationManager.AppSettings["Database"]);
            }
        }

        protected bool PlayChatSound
        {
            get
            {
                return ReturnAsBool(ReturnAsString(GetUserPreferences[1]));
            }
            set
            {
                List<StoredProcParameter> input = new List<StoredProcParameter>();
                input.Add(new StoredProcParameter(System.Data.DbType.String, "@UserName", SFAFMemberCenter20MasterPage.CurrentUserName, int.MaxValue));
                input.Add(new StoredProcParameter(System.Data.DbType.String, "@PlayChatSound", value, sizeof(bool)));
                List<StoredProcParameter> output = new List<StoredProcParameter>();

                DatabaseExecution.ExecuteStoredProcNonQuery("_SALI_SaveMemberCenter20UserPreferences", input, output, ConfigurationManager.AppSettings["Database"]);
            }
        }

        protected bool PlayMsgSound
        {
            get
            {
                return ReturnAsBool(ReturnAsString(GetUserPreferences[2]));
            }
            set
            {
                List<StoredProcParameter> input = new List<StoredProcParameter>();
                input.Add(new StoredProcParameter(System.Data.DbType.String, "@UserName", SFAFMemberCenter20MasterPage.CurrentUserName, int.MaxValue));
                input.Add(new StoredProcParameter(System.Data.DbType.String, "@PlayMsgSound", value, sizeof(bool)));
                List<StoredProcParameter> output = new List<StoredProcParameter>();

                DatabaseExecution.ExecuteStoredProcNonQuery("_SALI_SaveMemberCenter20UserPreferences", input, output, ConfigurationManager.AppSettings["Database"]);
            }
        }

        #endregion

        #region Public Properties

        public string CurrentPhysicalPath
        {
            get
            {
                string path = this.Request.PhysicalApplicationPath.Replace("\\", "/");
                return SFAFileFunction.FormatDirectory(path);
            }
        }

        public int CurrentCustomerId
        {
            get
            {
                return ReturnAsInt(Session[((ISFAFMemberCenter20Master)this.Page.Master).SFAFAppName + "CurrentCustomerId"]);
            }
        }

        public SALIUserTypes CurrentUserType
        {
            get
            {
                return MasterPresenter.CurrentUserType;
            }
            set
            {
                MasterPresenter.CurrentUserType = value;
            }
        }

        public string CurrentSchoolYearString
        {
            get
            {
                return ((SALISchoolYearEnum)SFAFMemberCenter20MasterPage.BusinessLogicObject.GetSchoolYears().ActualObject).ItemValueByID(SFAFMemberCenter20MasterPage.BusinessLogicObject.GetCurrentSchoolYear());
            }
        }

        public int CurrentSchoolYearId
        {
            get
            {
                return SFAFMemberCenter20MasterPage.BusinessLogicObject.GetCurrentSchoolYear();
            }
        }

        public ISFAFPresentation MasterPresenter
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).MasterPresenter;
            }
        }

        public virtual string SecurityObjectName
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion

        #region Protected Methods

        protected decimal? GetStudentTestScore(int schoolTestId, int studentId)
        {
            decimal? score = null;

            List<StoredProcParameter> input = new List<StoredProcParameter>();
            input.Add(new StoredProcParameter(System.Data.DbType.Int32, "@SchoolTestId ", schoolTestId, sizeof(int)));
            input.Add(new StoredProcParameter(System.Data.DbType.Int32, "@StudentId", studentId, sizeof(int)));

            List<StoredProcParameter> output = new List<StoredProcParameter>();
            DataTable dt = SALI.DatabaseExecution.ExecuteStoredProcQuery("_SALI_GetStudentTestScore", input, output, ConfigurationManager.AppSettings["Database"]).Tables[0];

            DataRow row = dt.Rows[0];
            if (row["Score"] != null)
                score = (decimal)row["Score"];

            return score;
        }

        public void AddSecurityControl(Button securityControl, string objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.Button, objectType, minimumSecurityLevel, minimumScopeLevel));
        }

        public void AddSecurityControl(Button securityControl, string[] objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.Button, objectType, minimumSecurityLevel, minimumScopeLevel));
        }

        public void AddSecurityControl(Button securityControl, string[] objectType, string[] minimumSecurityLevel, string minimumScopeLevel)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.Button, objectType, minimumSecurityLevel, minimumScopeLevel));
        }

        public void AddSecurityControl(Button securityControl, string objectType, string minimumSecurityLevel, string minimumScopeLevel, bool isSaveButton)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.Button, objectType, minimumSecurityLevel, minimumScopeLevel, isSaveButton));
        }

        public void AddSecurityControl(Button securityControl, string[] objectType, string minimumSecurityLevel, string minimumScopeLevel, bool isSaveButton)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.Button, objectType, minimumSecurityLevel, minimumScopeLevel, isSaveButton));
        }

        public void AddSecurityControl(Button securityControl, string[] objectType, string[] minimumSecurityLevel, string minimumScopeLevel, bool isSaveButton)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.Button, objectType, minimumSecurityLevel, minimumScopeLevel, isSaveButton));
        }

        public void AddSecurityControl(TextBox securityControl, string objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.TextBox, objectType, minimumSecurityLevel, minimumScopeLevel));
        }

        public void AddSecurityControl(Label securityControl, string objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.Label, objectType, minimumSecurityLevel, minimumScopeLevel));
        }

        public void AddSecurityControl(HyperLink securityControl, string objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.HyperLink, objectType, minimumSecurityLevel, minimumScopeLevel));
        }

        public void AddSecurityControl(LinkButton securityControl, string objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.LinkButton, objectType, minimumSecurityLevel, minimumScopeLevel));
        }

        public void AddSecurityControl(DropDownList securityControl, string objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.DropDownList, objectType, minimumSecurityLevel, minimumScopeLevel));
        }

        public void AddSecurityControl(ListBox securityControl, string objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.ListBox, objectType, minimumSecurityLevel, minimumScopeLevel));
        }

        public void AddSecurityControl(HtmlAnchor securityControl, string objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.HtmlAnchor, objectType, minimumSecurityLevel, minimumScopeLevel));
        }

        public void AddSecurityControl(CheckBox securityControl, string objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.CheckBox, objectType, minimumSecurityLevel, minimumScopeLevel));
        }

        public void AddSecurityControl(ImageButton securityControl, string objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.ImageButton, objectType, minimumSecurityLevel, minimumScopeLevel));
        }

        public void AddSecurityControl(ImageButton securityControl, string objectType, string minimumSecurityLevel, string minimumScopeLevel, bool isSaveButton)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.ImageButton, objectType, minimumSecurityLevel, minimumScopeLevel, isSaveButton));
        }

        public void AddSecurityControl(RadTab securityControl, string objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.RadTab, objectType, minimumSecurityLevel, minimumScopeLevel));
        }

        public void AddSecurityControl(FileUpload securityControl, string objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            _securityControls.Add(new SFAFMemberCenter20SecurityControl(securityControl, SALIWebControlTypes.FileUpload, objectType, minimumSecurityLevel, minimumScopeLevel));
        }

        protected void LoadYesNoAllList(DropDownList dropDown)
        {
            AddListItem(dropDown, "", "");
            AddListItem(dropDown, "1", "Yes");
            AddListItem(dropDown, "0", "No");
        }

        protected void LoadYesNoList(DropDownList dropDown)
        {
            AddListItem(dropDown, "1", "Yes");
            AddListItem(dropDown, "0", "No");
        }

        protected void LoadValuesList(DropDownList dropDown, Dictionary<string, string> values)
        {
            foreach (KeyValuePair<string, string> item in values)
                AddListItem(dropDown, item.Key, item.Value);
        }

        protected void AddListItem(DropDownList dropDown, string value, string text)
        {
            ListItem listItem;

            listItem = new ListItem();
            listItem.Value = value;
            listItem.Text = text;
            dropDown.Items.Add(listItem);
        }

        protected void LoadRadioButtonList(RadioButtonList rbl, DataTable data, string valueField, string textField, string selectedText, string selectedValue)
        {
            if (data.Columns.Contains(valueField) && data.Columns.Contains(textField))
            {
                rbl.Items.Clear();

                for (int x = 0; x < data.Rows.Count; x++)
                {
                    ListItem li = new ListItem(data.Rows[x][textField].ToString(), data.Rows[x][valueField].ToString());
                    li.Selected = false;

                    if (!string.IsNullOrEmpty(selectedValue))
                    {
                        if (data.Rows[x][valueField].ToString() == selectedValue)
                            li.Selected = true;
                    }
                    else
                    {
                        if (data.Rows[x][textField].ToString() == selectedText)
                            li.Selected = true;
                    }

                    rbl.Items.Add(li);
                }
            }
        }

        protected void LoadDropDownList(DropDownList ddl, DataTable data, string valueField, string textField, string selectedText, string selectedValue)
        {
            LoadDropDownList(ddl, data, valueField, textField, selectedText, selectedValue, false, false, null);
        }

        protected void LoadDropDownList(DropDownList ddl, DataTable data, string valueField, string textField, string selectedText, string selectedValue, bool addNullValue)
        {
            LoadDropDownList(ddl, data, valueField, textField, selectedText, selectedValue, addNullValue, false, null);
        }

        protected void LoadDropDownList(DropDownList ddl, DataTable data, string valueField, string textField, string selectedText, string selectedValue, bool removeDefaultValue, string defaultValue)
        {
            LoadDropDownList(ddl, data, valueField, textField, selectedText, selectedValue, true, removeDefaultValue, defaultValue);
        }

        protected void LoadDropDownList(DropDownList ddl, DataTable data, string valueField, string textField, string selectedText, string selectedValue, bool addNullValue, bool removeDefaultText, string defaultText)
        {
            if (data.Columns.Contains(valueField) && data.Columns.Contains(textField))
            {
                ddl.Items.Clear();

                if (addNullValue)
                {
                    ListItem li = new ListItem();
                    li.Selected = false;
                    li.Text = string.Empty;
                    li.Value = "-1";
                    ddl.Items.Add(li);
                }

                for (int x = 0; x < data.Rows.Count; x++)
                {
                    if ((removeDefaultText && data.Rows[x][textField].ToString() != defaultText) || !removeDefaultText)
                    {
                        ListItem li = new ListItem(data.Rows[x][textField].ToString(), data.Rows[x][valueField].ToString());
                        li.Attributes.Add("title", data.Rows[x][textField].ToString());
                        li.Selected = false;

                        if (!string.IsNullOrEmpty(selectedValue))
                        {
                            if (data.Rows[x][valueField].ToString() == selectedValue)
                                li.Selected = true;
                        }
                        else
                        {
                            if (data.Rows[x][textField].ToString() == selectedText)
                                li.Selected = true;
                        }

                        ddl.Items.Add(li);
                    }
                }
            }
        }

        protected void LoadDropDownListREMG2Titles(DropDownList ddl, DataTable data, string valueField, string textField, string selectedText, string selectedValue, bool addNullValue)
        {
            LoadDropDownListREMG2Titles(ddl, data, valueField, textField, selectedText, selectedValue, addNullValue, false, null);
        }

        protected void LoadDropDownListREMG2Titles(DropDownList ddl, DataTable data, string valueField, string textField, string selectedText, string selectedValue, bool addNullValue, bool removeDefaultText, string defaultText)
        {
            if (data.Columns.Contains(valueField) && data.Columns.Contains(textField))
            {
                ddl.Items.Clear();

                if (addNullValue)
                {
                    ListItem li = new ListItem();
                    li.Selected = false;
                    li.Text = string.Empty;
                    li.Value = "-1";
                    ddl.Items.Add(li);
                }

                for (int x = 0; x < data.Rows.Count; x++)
                {
                    if ((removeDefaultText && data.Rows[x][textField].ToString() != defaultText) || !removeDefaultText)
                    {
                        //FOR REMG2, REMOVE RESEARCH INDICATOR FOR TITLES DROPDOWN LIST
                        String _title = "";
                        if (data.Rows[x][textField].ToString().Substring(0, 4) == "|R| ")
                        {
                            _title = "\xA0\xA0\xA0\xA0\xA0" + data.Rows[x][textField].ToString().Remove(0, 4);
                        }
                        else
                        {
                            _title = data.Rows[x][textField].ToString();
                        }

                        ListItem li = new ListItem(_title, data.Rows[x][valueField].ToString());
                        li.Attributes.Add("title", _title);
                        li.Selected = false;

                        if (!string.IsNullOrEmpty(selectedValue))
                        {
                            if (data.Rows[x][valueField].ToString() == selectedValue)
                                li.Selected = true;
                        }
                        else
                        {
                            if (data.Rows[x][textField].ToString() == selectedText)
                                li.Selected = true;
                        }

                        ddl.Items.Add(li);
                    }
                }
            }
        }

        protected void SetDropDownListSelection(DropDownList ddl, string selectedText, string selectedValue)
        {
            for (int x = 0; x < ddl.Items.Count; x++)
            {
                ddl.Items[x].Selected = false;
            }

            for (int x = 0; x < ddl.Items.Count; x++)
            {
                if (!string.IsNullOrEmpty(selectedValue))
                {
                    if (ddl.Items[x].Value == selectedValue)
                    {
                        ddl.Items[x].Selected = true;
                        break;
                    }
                }
                else
                {
                    if (ddl.Items[x].Text == selectedText)
                    {
                        ddl.Items[x].Selected = true;
                        break;
                    }
                }
            }
        }

        protected void SetDropDownListSelectionToLastItem(DropDownList ddl)
        {
            for (int x = 0; x < ddl.Items.Count; x++)
            {
                ddl.Items[x].Selected = false;
            }

            if (ddl.Items.Count > 0)
            {
                ddl.Items[ddl.Items.Count - 1].Selected = true;
            }
        }

        protected void SetDropDownListSelectionToFirstItem(DropDownList ddl)
        {
            for (int x = 0; x < ddl.Items.Count; x++)
            {
                ddl.Items[x].Selected = false;
            }

            if (ddl.Items.Count > 0)
            {
                ddl.Items[0].Selected = true;
            }
        }

        protected string GetDropDownListSelectedText(DropDownList ddl)
        {
            string result = string.Empty;

            if (ddl.SelectedItem != null)
                result = ReturnAsString(ddl.SelectedItem.Text);

            return result;
        }

        protected string GetDropDownListSelectedValue(DropDownList ddl)
        {
            string result = string.Empty;

            if (ddl.SelectedItem != null)
                result = ReturnAsString(ddl.SelectedItem.Value);

            return result;
        }

        protected bool DropDownListValueSelected(DropDownList ddl)
        {
            bool result = true;

            if (GetDropDownListSelectedValue(ddl) == "-1" || GetDropDownListSelectedValue(ddl) == string.Empty)
                result = false;

            return result;
        }

        public void AssignChangeIndicatorToControl(TextBox control)
        {
            AssignChangeIndicatorToControl(control, string.Empty);
        }

        public void AssignChangeIndicatorToControl(TextBox control, string idPrefix)
        {
            control.Attributes.Add("onchange", "controlChange('" + idPrefix + control.ClientID + "');");
        }

        public void AssignChangeIndicatorToControl(DropDownList control)
        {
            control.Attributes.Add("onchange", "controlChange('" + control.ClientID + "');");
        }

        public void AssignChangeIndicatorToControl(ListBox control)
        {
            control.Attributes.Add("onchange", "controlChange('" + control.ClientID + "');");
        }

        public void AssignChangeIndicatorToControl(CheckBox control)
        {
            AssignChangeIndicatorToControl(control, string.Empty);
        }

        public void AssignChangeIndicatorToControl(CheckBox control, string idPrefix)
        {
            control.Attributes.Add("onclick", "controlChange('" + idPrefix + control.ClientID + "');");
        }

        public void AssignChangeIndicatorToControl(FileUpload control)
        {
            control.Attributes.Add("onchange", "controlChange('" + control.ClientID + "');");
        }

        protected DataTable RemoveDefaultValueFromEnumDataTable(DataTable dt, string defaultValue)
        {
            DataTable result = dt.Clone();

            for (int x = 0; x < dt.Rows.Count; x++)
            {
                if (dt.Rows[x]["Name"].ToString() != defaultValue)
                {
                    result.Rows.Add(dt.Rows[x].ItemArray);
                }
            }

            return result;
        }

        protected Control GetPostBackControl()
        {
            Page page = this;

            Control postbackControlInstance = null;

            string postbackControlName = page.Request.Params.Get("__EVENTTARGET");

            if (postbackControlName != null && postbackControlName != string.Empty)
            {
                postbackControlInstance = page.FindControl(postbackControlName);
            }
            else
            {
                // handle the Button control postbacks
                for (int i = 0; i < page.Request.Form.Keys.Count; i++)
                {
                    postbackControlInstance = page.FindControl(page.Request.Form.Keys[i]);

                    if (postbackControlInstance is System.Web.UI.WebControls.Button)
                    {
                        return postbackControlInstance;
                    }
                }
            }

            // handle the ImageButton postbacks

            if (postbackControlInstance == null)
            {
                for (int i = 0; i < page.Request.Form.Count; i++)
                {
                    if (page.Request.Form.Keys[i] != null)
                    {
                        if ((page.Request.Form.Keys[i].EndsWith(".x")) || (page.Request.Form.Keys[i].EndsWith(".y")))
                        {
                            postbackControlInstance = page.FindControl(page.Request.Form.Keys[i].Substring(0, page.Request.Form.Keys[i].Length - 2));

                            return postbackControlInstance;
                        }
                    }
                }
            }

            if (postbackControlInstance == null)
            {
                //postbackControlInstance = SFAFFindControl(postbackControlName);
            }

            return postbackControlInstance;
        }

        protected int GetEventArgumentIndex()
        {
            int result = -1;

            for (int x = 0; x < Request.Form.Keys.Count; x++)
            {
                if (Request.Form.Keys[x] == "__EVENTARGUMENT")
                {
                    result = x;
                    break;
                }
            }

            return result;
        }

        protected Control SFAFFindControl(string controlId)
        {
            return SFAFFindControl(controlId, this);
        }

        protected Control SFAFFindControl(string controlId, Control startingControl)
        {
            Control result = null;

            if (!string.IsNullOrEmpty(controlId))
            {
                result = startingControl.FindControl(controlId);

                if (result == null)
                {
                    foreach (Control c in startingControl.Controls)
                    {
                        result = SFAFFindControl(controlId, c);

                        if (result != null)
                            break;
                    }
                }
            }

            return result;
        }

        protected bool IsDateBefore(DateTime date, DateTime beforeDate)
        {
            return SFAFMemberCenter20MasterPage.BusinessLogicObject.IsDateBefore(date, beforeDate);
        }

        protected bool IsDateAfter(DateTime date, DateTime afterDate)
        {
            return SFAFMemberCenter20MasterPage.BusinessLogicObject.IsDateAfter(date, afterDate);
        }

        protected bool IsDateEqual(DateTime date, DateTime sameDate)
        {
            return SFAFMemberCenter20MasterPage.BusinessLogicObject.IsDateEqual(date, sameDate);
        }

        virtual protected void ShowSuccessMessage(Control control, string message)
        {
            ((ISFAFMemberCenter20NestedMasterContent)this.Master).SetSuccessMessage(message);
            ScriptManager.RegisterClientScriptBlock(control, typeof(Page), "showSuccessMessageScript", "showSuccessMessage();", true);
        }

        virtual protected void ShowFailMessage(Control control, string message)
        {
            ((ISFAFMemberCenter20NestedMasterContent)this.Master).SetFailMessage(message);
            ScriptManager.RegisterClientScriptBlock(control, typeof(Page), "showSuccessMessageScript", "showFailMessage();", true);
        }

        protected string ReturnAsString(object o)
        {
            string result = string.Empty;

            if (o != null)
                result = o.ToString();

            return result;
        }

        protected string ReturnAsHtmlString(string s)
        {
            string result = s.Trim();

            if (string.IsNullOrEmpty(result))
                result = "&nbsp;";

            return result;
        }

        protected int ReturnAsInt(object o)
        {
            string s = string.Empty;

            if (o != null)
                s = o.ToString();

            return ReturnAsInt(s);
        }

        protected int ReturnAsInt(string s)
        {
            int result = -1;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    result = Convert.ToInt32(s);
                }
                catch 
                {
                    result = -1;
                }
            }

            return result;
        }

        protected int ReturnAsInt(string s, int baseSystem)
        {
            int result = -1;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    if (baseSystem == 36)
                        result = Convert.ToInt32(SFAUtilityFunctions.DecodeBase36(s));
                    else
                        result = Convert.ToInt32(s, baseSystem);
                }
                catch
                {
                    result = -1;
                }
            }

            return result;
        }

        protected decimal ReturnAsDecimal(string s)
        {
            decimal result = -1;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    result = Convert.ToDecimal(s);
                    result += 0.00m;
                }
                catch
                {
                    result = -1;
                }
            }

            return result;
        }

        protected double ReturnAsDouble(string s)
        {
            double result = -1;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    result = Convert.ToDouble(s);
                }
                catch
                {
                    result = -1;
                }
            }

            return result;
        }

        protected bool ReturnAsBool(string s)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    s = s.ToLower().Trim();

                    switch (s)
                    {
                        case "1":
                        case "true":
                        case "yes":
                            result = true;
                            break;
                        default:
                            result = false;
                            break;
                    }
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        protected DateTime ReturnAsDateTime(string s)
        {
            DateTime result = SFAFConstants.SFAFNullDate;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    result = Convert.ToDateTime(s);

                    if (IsDateBefore(result, SFAFConstants.SFAFNullDate))
                        result = SFAFConstants.SFAFNullDate;
                }
                catch
                {
                    result = SFAFConstants.SFAFNullDate;
                }
            }

            return result;
        }

        protected string ConvertDateTimeToShortString(DateTime dt)
        {
            string result = string.Empty;

            if (dt.CompareTo(SFAFConstants.SFAFNullDate) > 0 && dt.CompareTo(SFAFConstants.SFAFNullDate2) > 0)
                result = dt.ToShortDateString();

            return result;
        }

        protected void ExportToExcel(RadGrid grdExtracts, string filename)
        {
            ConfigureExport(grdExtracts, filename);
            grdExtracts.MasterTableView.ExportToExcel();
        }

        protected void ExportToWord(RadGrid grdExtracts, string filename)
        {
            ConfigureExport(grdExtracts, filename);
            grdExtracts.MasterTableView.ExportToWord();
        }

        protected void ExportToPDF(RadGrid grdExtracts, string filename)
        {
            ConfigureExport(grdExtracts, filename);
            grdExtracts.MasterTableView.ExportToPdf();
        }

        protected void ExportToCSV(RadGrid grdExtracts, string filename)
        {
            ConfigureExport(grdExtracts, filename);
            grdExtracts.MasterTableView.ExportToCSV();
        }

        protected void ConfigureExport(RadGrid grdExtracts, string filename)
        {
            grdExtracts.ExportSettings.ExportOnlyData = true;
            grdExtracts.ExportSettings.IgnorePaging = true;
            grdExtracts.ExportSettings.OpenInNewWindow = true;
            grdExtracts.ExportSettings.FileName = filename;
        }

        protected void DisableControl(LinkButton lb)
        {
            lb.Enabled = false;
            lb.Attributes["onclick"] = "return false;";
            lb.OnClientClick = string.Empty;
            lb.CssClass = "DisabledLinkClass";
        }

        protected void ConfigureExportToExcel(HyperLink hl, string exportDataSet)
        {
            ConfigureExportToExcel(hl, exportDataSet, true);
        }

        protected void ConfigureExportToExcel(HyperLink hl, string exportDataSet, bool addIcon)
        {
            hl.NavigateUrl = "~/FrontOffice/Extracts.aspx?ExtractType=" + exportDataSet + "&FileType=" + (int)SALIExtractFileTypes.Excel;

            if (addIcon)
                hl.CssClass += " ExcelLink";
        }

        protected bool CheckForRedirect()
        {
            bool result = false;

            Control c = GetPostBackControl();
            if (c != null)
            {
                if (c.GetType().Equals(typeof(LinkButton)))
                {
                    if (((LinkButton)c).CommandName == "redirect")
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        protected void SetControlTextFromFile(Literal l, string filePath)
        {
            l.Text = GetFileContents(filePath);
        }

        protected void SetControlTextFromFile(TextBox t, string filePath)
        {
            t.Text = GetFileContents(filePath);
        }

        protected void SetControlTextFromFile(HiddenField h, string filePath)
        {
            h.Value = GetFileContents(filePath);
        }

        protected void GoToPageInMyCenter(string url)
        {
            GoToPageInCenter(url, MemberCenter20Constants.MY_CENTER_TEXT);
        }

        protected void GoToPageInFrontOffice(string url)
        {
            GoToPageInCenter(url, MemberCenter20Constants.FRONT_OFFICE_TEXT);
        }

        protected void GoToPageInClassroomCenter(string url)
        {
            GoToPageInCenter(url, MemberCenter20Constants.CLASSROOM_CENTER_TEXT);
        }

        protected void GoToPageInGroupingCenter(string url)
        {
            GoToPageInCenter(url, MemberCenter20Constants.GROUPING_CENTER_TEXT);
        }

        protected void GoToPageInReportsCenter(string url)
        {
            GoToPageInCenter(url, MemberCenter20Constants.REPORTS_CENTER_TEXT);
        }

        protected void GoToPageInStudentCenter(string url)
        {
            GoToPageInCenter(url, MemberCenter20Constants.STUDENT_CENTER_TEXT);
        }

        protected void GoToPageInTestingCenter(string url)
        {
            GoToPageInCenter(url, MemberCenter20Constants.TESTING_CENTER_TEXT);
        }

        protected bool CheckForDemo()
        {
            bool isDemo = false;

            if (ReturnAsString(Session[ConfigurationManager.AppSettings["SFAFAppName"] + "Environment"]).ToLower().Contains("demo") || ReturnAsBool(ConfigurationManager.AppSettings["IsDemo"]))
            {
                isDemo = true;
            }

            return isDemo;
        }

        public DataTable ConvertEnumToDataTable(Type enumType)
        {
            return SFAFMemberCenter20MasterPage.BusinessLogicObject.ConvertEnumToDataTable(enumType);
        }

        protected void CleanupControl(Control c)
        {
            c.Dispose();
            c = null;
        }

        protected bool UserHasAccessToClassroom(int classroomAssignmentId)
        {
            bool result = false;

            if (SFAFMemberCenter20MasterPage.BusinessLogicObject.IsDemo)
                result = true;
            else
            {
                Security s = new Security(SFAFMemberCenter20MasterPage.CurrentUserName, AccessType, SFAFMemberCenter20MasterPage.CurrentSessionId);

                result = s.UserHasAccessToClassroom(classroomAssignmentId);
            }

            return result;
        }

        protected bool UserHasAccessToLesson(int lessonId)
        {
            bool result = false;

            if (SFAFMemberCenter20MasterPage.BusinessLogicObject.IsDemo)
                result = true;
            else
            {
                Security s = new Security(SFAFMemberCenter20MasterPage.CurrentUserName, AccessType, SFAFMemberCenter20MasterPage.CurrentSessionId);

                result = s.UserHasAccessToLesson(lessonId);
            }

            return result;
        }

        protected bool UserHasAccessToCustomer(int customerId)
        {
            bool result = false;

            if (SFAFMemberCenter20MasterPage.BusinessLogicObject.IsDemo)
                result = true;
            else
            {
                Security s = new Security(SFAFMemberCenter20MasterPage.CurrentUserName, AccessType, SFAFMemberCenter20MasterPage.CurrentSessionId);

                result = s.UserHasAccessToCustomer(customerId);
            }


            return result;
        }

        protected bool IsTestId4Sight(int testId)
        {
            bool result = false;

            string[] ids = ConfigurationManager.AppSettings["Current4SightTestIDs"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            for (int x = 0; x < ids.Length; x++)
            {
                if (ReturnAsInt(ids[x]) == testId)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        #endregion

        #region Page Methods

        protected void Page_Init(object sender, EventArgs e)
        {
            if (ReturnAsString(Session[ConfigurationManager.AppSettings["SFAFAppName"] + "Environment"]).ToLower().Contains("demo"))
                AccessType = DataAccessTypes.DemoDatabase;

            bool hasSecurity = true;

            if (!string.IsNullOrEmpty(((ISFAFMemberCenter20Page)this).SecurityObjectName))
                hasSecurity = SFAFMemberCenter20MasterPage.BusinessLogicObject.CheckUserObjectPermissionDB(SFAFMemberCenter20MasterPage.CurrentUserName, ((ISFAFMemberCenter20Page)this).SecurityObjectName, SFAFSecurityLevel.SELECT, SFAFSecurityScope.SELF);

            if (hasSecurity)
            {
                if (CheckForRedirect())
                {
                    PageRedirecting();
                    _isRedirecting = true;
                    SFAFMemberCenter20MasterPage.BusinessLogicObject.WriteAuditHistory(AuditTypesEnum.Trace, AuditSubjectsEnum.All, this.CurrentWebPageWithURL + " Closed");
                }
                else
                {
                    if (this.Master != null)
                    {
                        if ((this.CurrentCustomerId > 0 && MasterPresenter.CurrentCustomerId <= 0) || (this.CurrentCustomerId != MasterPresenter.CurrentCustomerId))
                        {
                            MasterPresenter.CurrentCustomerId = this.CurrentCustomerId;
                            MasterPresenter.IsTrackSchool = SFAFMemberCenter20MasterPage.BusinessLogicObject.DoesSchoolHaveTracks(this.CurrentCustomerId, this.CurrentSchoolYearId);
                            MasterPresenter.IsDistrict = SFAFMemberCenter20MasterPage.BusinessLogicObject.IsDistrict(this.CurrentCustomerId);
                            MasterPresenter.ClientBrowser = SFAFMemberCenter20MasterPage.ClientBrowser;
                            MasterPresenter.ClientBrowserVersion = SFAFMemberCenter20MasterPage.ClientBrowserVersion;
                        }
                    }
                    SFAFPageInit(sender, e);

                    if (!IsPostBack)
                        SFAFMemberCenter20MasterPage.BusinessLogicObject.WriteAuditHistory(AuditTypesEnum.Trace, AuditSubjectsEnum.All, this.CurrentWebPageWithURL + " Opened");
                }
            }
            else
            {
                SFAFMemberCenter20MasterPage.BusinessLogicObject.WriteAuditHistory(AuditTypesEnum.Trace, AuditSubjectsEnum.Login, "Attempted to access " + this.CurrentWebPageWithURL + " without proper security");
                Response.Redirect(ConfigurationManager.AppSettings["PostLoginLandingPage"] + "?r=ns", true);
            }
        }

        protected virtual void PageRedirecting()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!_isRedirecting || ((ISFAFMemberCenter20Master)Page.Master).NeedSave)
                SFAFPageLoad(sender, e);
        }

        protected override void RaisePostBackEvent(IPostBackEventHandler source, String eventArgument)
        {
            base.RaisePostBackEvent(source, eventArgument);
        }

        #endregion

        #region Private Methods

        private void GoToPageInCenter(string url, string center)
        {
            ((ISFAFMemberCenter20NavigationPage)this.Master).CenterLinkClick(center);
            Response.Redirect(url);
        }

        private string GetFileContents(string filePath)
        {
            Exception e = null;

            string result = SFAFUtilityObjects.SFAFileFunction.ReadFileAsString(filePath, out e);

            return result;
        }

        #endregion

        #region Public Methods

        public virtual void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        public virtual void SFAFPageInit(object sender, EventArgs e)
        {

        }

        public virtual void SFAFPageLoad(object sender, EventArgs e)
        {

        }

        #endregion

        #region ISFAFMemberCenter20Page Members

        public List<SFAFMemberCenter20SecurityControl> PageSecurityControls
        {
            get
            {
                return _securityControls;
            }
        }

        public virtual bool IsLoginPage()
        {
            return false;
        }

        public virtual void SetErrorMsg(string message)
        {
        }

        public virtual string HelpPageUrl
        {
            get
            {
                return string.Empty;
            }
        }

        public virtual bool ShowSecurityIcon
        {
            get
            {
                return true;
            }
        }

        public virtual string AdditionalSecurityInfo
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion

    }

    public class SFAFMemberCenter20RadWindowEditPage : SFAFMemberCenter20ContentPage
    {
        const string ID_SEPARATOR = "~";

        public virtual List<SFAFMemberCenter20RequiredFieldControl> RequiredControls
        {
            get
            {
                List<SFAFMemberCenter20RequiredFieldControl> result = new List<SFAFMemberCenter20RequiredFieldControl>();

                return result;
            }
        }

        protected string[] GetIds()
        {
            return SplitQueryStringForIds("ids");
        }

        protected string[] SplitQueryStringForIds(string arg)
        {
            string s = ReturnAsString(Request.QueryString[arg]);

            string[] result = new string[0];

            if (!string.IsNullOrEmpty(s))
                result = s.Split(new string[] { ID_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

            return result;
        }
    }

    public class SFAFMemberCenter20LoginPage : SFAFMemberCenter20ContentPage
    {
        public SFAFMemberCenter20LoginPage()
            : base()
        {
        }

        public override bool IsLoginPage()
        {
            return true;
        }
    }

    public class SFAFMemberCenter20SupportPage : SFAFMemberCenter20ContentPage
    {
        public string _osName = string.Empty;
        public string _osVersion = string.Empty;
        public string _browserName = string.Empty;
        public string _browserVersion = string.Empty;
        public string _userAgent = string.Empty;
        public string _mobileBrowser = string.Empty;
        public string _whyMobileBrowser = string.Empty;
        public string _isDemo = string.Empty;

        public override void SFAFPageInit(object sender, EventArgs e)
        {
            _osName = ((ISFAFMemberCenter20Master)this.Master).ClientOperatingSystem;
            _osVersion = ((ISFAFMemberCenter20Master)this.Master).ClientOperatingSystemVersion;
            _browserName = ((ISFAFMemberCenter20Master)this.Master).ClientBrowser;
            _browserVersion = ((ISFAFMemberCenter20Master)this.Master).ClientBrowserVersion;
            _userAgent = ((ISFAFMemberCenter20Master)this.Master).ClientUserAgent;
            _mobileBrowser = ReturnAsString(((ISFAFMemberCenter20Master)this.Master).IsMobileBrowser());
            _whyMobileBrowser = ReturnAsString(Session[SFAFMemberCenter20MasterPage.SFAFAppName + "WhyMobileBrowse"]) + " " + ReturnAsString(Session[SFAFMemberCenter20MasterPage.SFAFAppName + "MobileBrowseType"]);

            if (CheckForDemo())
                _isDemo = "True";
            else
                _isDemo = "False";

            SFAFSupportPageInit(sender, e);
        }

        public virtual void SFAFSupportPageInit(object sender, EventArgs e)
        {
        }
    }

    public class SFAFMemberCenter20ReportPage : SFAFMemberCenter20ContentPage
    {
        protected const bool LIMIT_CLASSROOMS_BASED_ON_SECURITY = true;
    }

    public class SFAFMemberCenter20NestedMasterPage : System.Web.UI.MasterPage, ISFAFMemberCenter20Master
    {
        #region ISFAFMemberCenter20Master Members

        public void SetClientInfo(string os, string osVersion, string browser, string browserVersion, string userAgent)
        {
            ((ISFAFMemberCenter20Master)this.Master).SetClientInfo(os, osVersion, browser, browserVersion, userAgent);
        }

        public string ClientUserAgent
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).ClientUserAgent;
            }
        }

        public string ClientOperatingSystem
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).ClientOperatingSystem;
            }
        }

        public string ClientOperatingSystemVersion
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).ClientOperatingSystemVersion;
            }
        }

        public string ClientBrowser
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).ClientBrowser;
            }
        }

        public string ClientBrowserVersion
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).ClientBrowserVersion;
            }
        }

        public bool IsMobileBrowser()
        {
            return ((ISFAFMemberCenter20Master)this.Master).IsMobileBrowser();
        }

        public string LeftBackColor
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).LeftBackColor;
            }
        }

        public string LeftForeColor
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).LeftForeColor;
            }
        }

        public string CurrentSessionId
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).CurrentSessionId;
            }
        }

        public string CurrentLoginId
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).CurrentLoginId;
            }
        }

        public int CurrentUserId
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).CurrentUserId;
            }
        }

        public SALIUserTypes CurrentUserType
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).CurrentUserType;
            }
        }

        public string CurrentUserName
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).CurrentUserName;
            }
        }

        public string CurrentUserPassword
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).CurrentUserPassword;
            }
        }

        public string SFAFAppName
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).SFAFAppName;
            }
        }

        public bool NeedSave
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).NeedSave;
            }
        }

        public SALIMainBusinessLogic BusinessLogicObject
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).BusinessLogicObject;
            }
        }

        public bool LoginCustomerContact(string userName, string password)
        {
            return ((ISFAFMemberCenter20Master)this.Master).LoginCustomerContact(userName, password);
        }

        public bool LoginCustomerContact(string userName, string password, List<Delegate> postLoginDelegates)
        {
            return ((ISFAFMemberCenter20Master)this.Master).LoginCustomerContact(userName, password, postLoginDelegates);
        }

        public bool LoginCustomerContact(string userName, string password, List<Delegate> postLoginDelegates, bool isDemo)
        {
            return ((ISFAFMemberCenter20Master)this.Master).LoginCustomerContact(userName, password, postLoginDelegates, isDemo);
        }

        public void Logout()
        {
            ((ISFAFMemberCenter20Master)this.Master).Logout();
        }

        public void Logout(bool showParameters)
        {
            ((ISFAFMemberCenter20Master)this.Master).Logout(showParameters);
        }

        public void SetPageTitle(string title)
        {
            ((ISFAFMemberCenter20Master)this.Master).SetPageTitle(title);
        }

        public void ResetSaveRedirectInfo(string redirectUrl)
        {
            ((ISFAFMemberCenter20Master)this.Master).ResetSaveRedirectInfo(redirectUrl);
        }

        public ScriptManager MasterScriptManager
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).MasterScriptManager;
            }
        }

        public RadWindowManager MasterWindowManager
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).MasterWindowManager;
            }
        }

        public string DefaultCalendarStartDate
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).DefaultCalendarStartDate;
            }
            set
            {
                ((ISFAFMemberCenter20Master)this.Master).DefaultCalendarStartDate = value;
            }
        }

        public string DefaultCalendarEndDate
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).DefaultCalendarEndDate;
            }
            set
            {
                ((ISFAFMemberCenter20Master)this.Master).DefaultCalendarEndDate = value;
            }
        }

        public string SetFontColorSessionLocation
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).SetFontColorSessionLocation;
            }
        }

        public string SetCenterImageSessionLocation
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).SetCenterImageSessionLocation;
            }
        }

        public string CustomerListSessionName
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).CustomerListSessionName;
            }
        }

        public ISFAFPresentation MasterPresenter
        {
            get
            {
                return ((ISFAFMemberCenter20Master)this.Master).MasterPresenter;
            }
        }

        #endregion

        public string CurrentPhysicalPath
        {
            get
            {
                return SFAFileFunction.FormatDirectory(this.Request.PhysicalPath.Replace("\\", "/").Replace(this.Request.Path, string.Empty));
            }
        }

        protected string ReturnAsString(object o)
        {
            string result = string.Empty;

            if (o != null)
                result = o.ToString();

            return result;
        }

        protected bool ReturnAsBool(string s)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    s = s.ToLower().Trim();

                    switch (s)
                    {
                        case "1":
                        case "true":
                        case "yes":
                            result = true;
                            break;
                        case "0":
                        case "false":
                        case "no":
                            result = false;
                            break;
                        default:
                            result = false;
                            break;
                    }
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        protected int ReturnAsInt(object o)
        {
            string s = string.Empty;

            if (o != null)
                s = o.ToString();

            return ReturnAsInt(s);
        }

        protected int ReturnAsInt(string s)
        {
            int result = -1;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    result = Convert.ToInt32(s);
                }
                catch
                {
                    result = -1;
                }
            }

            return result;
        }

        protected DateTime ReturnAsDateTime(string s)
        {
            DateTime result = SFAFConstants.SFAFNullDate;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    result = Convert.ToDateTime(s);

                    if (BusinessLogicObject.IsDateBefore(result, SFAFConstants.SFAFNullDate))
                        result = SFAFConstants.SFAFNullDate;
                }
                catch
                {
                    result = SFAFConstants.SFAFNullDate;
                }
            }

            return result;
        }

        protected void SetDropDownListSelection(DropDownList ddl, string selectedText, string selectedValue)
        {
            for (int x = 0; x < ddl.Items.Count; x++)
            {
                ddl.Items[x].Selected = false;

                if (!string.IsNullOrEmpty(selectedValue))
                {
                    if (ddl.Items[x].Value == selectedValue)
                        ddl.Items[x].Selected = true;
                }
                else
                {
                    if (ddl.Items[x].Text == selectedText)
                        ddl.Items[x].Selected = true;
                }
            }
        }

        public bool TopNavBehavior
        {
            get
            {
                List<StoredProcParameter> input = new List<StoredProcParameter>();
                input.Add(new StoredProcParameter(System.Data.DbType.String, "@UserName", CurrentUserName, int.MaxValue));
                List<StoredProcParameter> output = new List<StoredProcParameter>();
                output.Add(new StoredProcParameter(System.Data.DbType.String, "@TopNavBehavior", false, sizeof(bool)));

                DatabaseExecution.ExecuteStoredProcNonQuery("_SALI_GetMemberCenter20UserPreferences", input, output, ConfigurationManager.AppSettings["Database"]);
                return ReturnAsBool(ReturnAsString(output[0].Value));
            }
        }

        public bool PlayMsgSound
        {
            get
            {
                List<StoredProcParameter> input = new List<StoredProcParameter>();
                input.Add(new StoredProcParameter(System.Data.DbType.String, "@UserName", CurrentUserName, int.MaxValue));
                List<StoredProcParameter> output = new List<StoredProcParameter>();
                output.Add(new StoredProcParameter(System.Data.DbType.String, "@PlayMsgSound", false, sizeof(bool)));

                DatabaseExecution.ExecuteStoredProcNonQuery("_SALI_GetMemberCenter20UserPreferences", input, output, ConfigurationManager.AppSettings["Database"]);
                return ReturnAsBool(ReturnAsString(output[0].Value));
            }
        }

        protected void LoadDropDownList(DropDownList ddl, DataTable data, string valueField, string textField, string selectedText, string selectedValue)
        {
            LoadDropDownList(ddl, data, valueField, textField, selectedText, selectedValue, false, false, null);
        }

        protected void LoadDropDownList(DropDownList ddl, DataTable data, string valueField, string textField, string selectedText, string selectedValue, bool addNullValue)
        {
            LoadDropDownList(ddl, data, valueField, textField, selectedText, selectedValue, addNullValue, false, null);
        }

        protected void LoadDropDownList(DropDownList ddl, DataTable data, string valueField, string textField, string selectedText, string selectedValue, bool removeDefaultValue, string defaultValue)
        {
            LoadDropDownList(ddl, data, valueField, textField, selectedText, selectedValue, true, removeDefaultValue, defaultValue);
        }

        protected void LoadDropDownList(DropDownList ddl, DataTable data, string valueField, string textField, string selectedText, string selectedValue, bool addNullValue, bool removeDefaultText, string defaultText)
        {
            if (data.Columns.Contains(valueField) && data.Columns.Contains(textField))
            {
                ddl.Items.Clear();

                if (addNullValue)
                {
                    ListItem li = new ListItem();
                    li.Selected = false;
                    li.Text = string.Empty;
                    li.Value = "-1";
                    ddl.Items.Add(li);
                }

                for (int x = 0; x < data.Rows.Count; x++)
                {
                    if ((removeDefaultText && data.Rows[x][textField].ToString() != defaultText) || !removeDefaultText)
                    {
                        ListItem li = new ListItem(data.Rows[x][textField].ToString(), data.Rows[x][valueField].ToString());
                        li.Attributes.Add("title", data.Rows[x][textField].ToString());
                        li.Selected = false;

                        if (!string.IsNullOrEmpty(selectedValue))
                        {
                            if (data.Rows[x][valueField].ToString() == selectedValue)
                                li.Selected = true;
                        }
                        else
                        {
                            if (data.Rows[x][textField].ToString() == selectedText)
                                li.Selected = true;
                        }

                        ddl.Items.Add(li);
                    }
                }
            }
        }
    }

    public class SFAFMemberCenter20NestedRadWindowMasterPage : SFAFMemberCenter20NestedMasterPage, ISFAFMemberCenter20RadWindowMaster
    {
        #region ISFAFMemberCenter20RadWindowMaster Members

        public void AddCloseDelegate(Delegate del)
        {
            ((ISFAFMemberCenter20RadWindowMaster)this.Master).AddCloseDelegate(del);
        }

        public void SetParentRefresh()
        {
            ((ISFAFMemberCenter20RadWindowMaster)this.Master).SetParentRefresh();
        }

        public void HideCloseButton()
        {
            ((ISFAFMemberCenter20RadWindowMaster)this.Master).HideCloseButton();
        }

        #endregion
    }

    public class SFAFMemberCenter20NestedRadWindowContentMasterPage : SFAFMemberCenter20NestedRadWindowMasterPage, ISFAFMemberCenter20RadWindowContentMaster
    {
        #region ISFAFMemberCenter20RadWindowContentMaster Members

        public void SetPageTitle(string centerName, string pageName, string windowName)
        {
            ((ISFAFMemberCenter20RadWindowContentMaster)this.Master).SetPageTitle(centerName, pageName, windowName);
        }

        public void SetPageTitle(string centerName, string pageName, string windowName, string overrideColor)
        {
            ((ISFAFMemberCenter20RadWindowContentMaster)this.Master).SetPageTitle(centerName, pageName, windowName, overrideColor);
        }

        #endregion
    }

    public class SFAFMemberCenter20SecurityControl
    {
        private Control _securityControl = null;
        private SALIWebControlTypes _controlType = SALIWebControlTypes.None;
        private List<string> _objectType = new List<string>();
        private List<string> _minimumSecurityLevel = new List<string>();
        private string _minimumScopeLevel = string.Empty;
        private bool _isSaveButton = false;

        public SFAFMemberCenter20SecurityControl(Control securityControl, SALIWebControlTypes controlType, string objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            SecurityControl = securityControl;
            _objectType.Add(objectType);
            _minimumSecurityLevel.Add(minimumSecurityLevel);
            MinimumScopeLevel = minimumScopeLevel;
            ControlType = controlType;
        }

        public SFAFMemberCenter20SecurityControl(Control securityControl, SALIWebControlTypes controlType, string objectType, string minimumSecurityLevel, string minimumScopeLevel, bool isSaveButton)
        {
            SecurityControl = securityControl;
            _objectType.Add(objectType);
            _minimumSecurityLevel.Add(minimumSecurityLevel);
            MinimumScopeLevel = minimumScopeLevel;
            ControlType = controlType;
            _isSaveButton = isSaveButton;
        }

        public SFAFMemberCenter20SecurityControl(Control securityControl, SALIWebControlTypes controlType, string[] objectType, string minimumSecurityLevel, string minimumScopeLevel)
        {
            SecurityControl = securityControl;

            for (int x = 0; x < objectType.Length; x++)
                _objectType.Add(objectType[x]);

            _minimumSecurityLevel.Add(minimumSecurityLevel);
            MinimumScopeLevel = minimumScopeLevel;
            ControlType = controlType;
        }

        public SFAFMemberCenter20SecurityControl(Control securityControl, SALIWebControlTypes controlType, string[] objectType, string minimumSecurityLevel, string minimumScopeLevel, bool isSaveButton)
        {
            SecurityControl = securityControl;

            for (int x = 0; x < objectType.Length; x++)
                _objectType.Add(objectType[x]);

            _minimumSecurityLevel.Add(minimumSecurityLevel);
            MinimumScopeLevel = minimumScopeLevel;
            ControlType = controlType;
            _isSaveButton = isSaveButton;
        }

        public SFAFMemberCenter20SecurityControl(Control securityControl, SALIWebControlTypes controlType, string[] objectType, string[] minimumSecurityLevel, string minimumScopeLevel)
        {
            SecurityControl = securityControl;

            for (int x = 0; x < objectType.Length; x++)
                _objectType.Add(objectType[x]);

            for (int x = 0; x < minimumSecurityLevel.Length; x++)
                _minimumSecurityLevel.Add(minimumSecurityLevel[x]);

            MinimumScopeLevel = minimumScopeLevel;
            ControlType = controlType;
        }

        public SFAFMemberCenter20SecurityControl(Control securityControl, SALIWebControlTypes controlType, string[] objectType, string[] minimumSecurityLevel, string minimumScopeLevel, bool isSaveButton)
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

        public Control SecurityControl
        {
            get
            {
                return _securityControl;
            }
            private set
            {
                _securityControl = value;
            }
        }

        public string[] MinimumSecurityLevel
        {
            get
            {
                return _minimumSecurityLevel.ToArray();
            }
        }

        public string MinimumScopeLevel
        {
            get
            {
                return _minimumScopeLevel;
            }
            private set
            {
                _minimumScopeLevel = value;
            }
        }

        public string[] ObjectType
        {
            get
            {
                return _objectType.ToArray();
            }
        }

        public SALIWebControlTypes ControlType
        {
            get
            {
                return _controlType;
            }
            private set
            {
                _controlType = value;
            }
        }

        public bool IsSaveButton
        {
            get
            {
                return _isSaveButton;
            }
        }
    }

    [Serializable]
    public class SFAFMemberCenter20Presenter : ISFAFPresentation
    {
        private object Session;
        private string SFAFAppName;
        private bool ENABLE_GLOBAL_CACHE = false;
        Cache Cache = null;

        private List<string> _cacheKeys = new List<string>();

        public SFAFMemberCenter20Presenter(HttpSessionState session, string appName, bool enableGlobalCache, Cache cache)
        {
            Session = session;
            SFAFAppName = appName;
            ENABLE_GLOBAL_CACHE = enableGlobalCache;
            Cache = cache;
        }

        #region ISFAFPresentation Members

        public SALIUserTypes CurrentUserType
        {
            get
            {
                SALIUserTypes result = SALIUserTypes.Unknown;

                if (((HttpSessionState)Session)["Current" + SFAFAppName + "UserType"] != null)
                    result = (SALIUserTypes)((HttpSessionState)Session)["Current" + SFAFAppName + "UserType"];

                return result;
            }
            set
            {
                ((HttpSessionState)Session)["Current" + SFAFAppName + "UserType"] = value;
            }
        }

        public string CurrentSessionId
        {
            get
            {
                string result = string.Empty;

                if (((HttpSessionState)Session)["Current" + SFAFAppName + "SessionId"] != null)
                    result = ((HttpSessionState)Session)["Current" + SFAFAppName + "SessionId"].ToString();

                return result;
            }
            set
            {
                ((HttpSessionState)Session)["Current" + SFAFAppName + "SessionId"] = value;
            }
        }

        public string CurrentLoginId
        {
            get
            {
                string result = string.Empty;

                if (((HttpSessionState)Session)["Current" + SFAFAppName + "LoginId"] != null)
                    result = ((HttpSessionState)Session)["Current" + SFAFAppName + "LoginId"].ToString();

                return result;
            }
            set
            {
                ((HttpSessionState)Session)["Current" + SFAFAppName + "LoginId"] = value;
            }
        }

        public string CurrentUserName
        {
            get
            {
                string result = string.Empty;

                if (((HttpSessionState)Session)["Current" + SFAFAppName + "UserName"] != null)
                    result = ((HttpSessionState)Session)["Current" + SFAFAppName + "UserName"].ToString();

                return result;
            }
            set
            {
                ((HttpSessionState)Session)["Current" + SFAFAppName + "UserName"] = value;
            }
        }

        public string CurrentUserPassword
        {
            get
            {
                string result = string.Empty;

                if (((HttpSessionState)Session)["Current" + SFAFAppName + "Password"] != null)
                    result = ((HttpSessionState)Session)["Current" + SFAFAppName + "Password"].ToString();

                return result;
            }
            set
            {
                ((HttpSessionState)Session)["Current" + SFAFAppName + "Password"] = value;
            }
        }

        public string CurrentUserFirstName
        {
            get
            {
                string result = string.Empty;

                if (((HttpSessionState)Session)["Current" + SFAFAppName + "FirstName"] != null)
                    result = ((HttpSessionState)Session)["Current" + SFAFAppName + "FirstName"].ToString();

                return result;
            }
            set
            {
                ((HttpSessionState)Session)["Current" + SFAFAppName + "FirstName"] = value;
            }
        }

        public string CurrentUserMiddleName
        {
            get
            {
                string result = string.Empty;

                if (((HttpSessionState)Session)["Current" + SFAFAppName + "MiddletName"] != null)
                    result = ((HttpSessionState)Session)["Current" + SFAFAppName + "MiddletName"].ToString();

                return result;
            }
            set
            {
                ((HttpSessionState)Session)["Current" + SFAFAppName + "MiddletName"] = value;
            }
        }

        public string CurrentUserLastName
        {
            get
            {
                string result = string.Empty;

                if (((HttpSessionState)Session)["Current" + SFAFAppName + "LastName"] != null)
                    result = ((HttpSessionState)Session)["Current" + SFAFAppName + "LastName"].ToString();

                return result;
            }
            set
            {
                ((HttpSessionState)Session)["Current" + SFAFAppName + "LastName"] = value;
            }
        }

        public string CurrentCustomerName
        {
            get
            {
                string result = string.Empty;

                if (((HttpSessionState)Session)[SFAFAppName + "CurrentCustomerName"] != null)
                    result = (string)((HttpSessionState)Session)[SFAFAppName + "CurrentCustomerName"];

                return result;
            }
            set
            {
                ((HttpSessionState)Session)[SFAFAppName + "CurrentCustomerName"] = value;
            }
        }

        public string CurrentSchoolYear
        {
            get
            {
                string result = string.Empty;

                if (((HttpSessionState)Session)["Current" + SFAFAppName + "SchoolYear"] != null)
                    result = (string)((HttpSessionState)Session)["Current" + SFAFAppName + "SchoolYear"];

                return result;
            }
            set
            {
                ((HttpSessionState)Session)["Current" + SFAFAppName + "SchoolYear"] = value;
            }
        }

        public string ClientBrowser
        {
            get
            {
                return ReturnAsString(((HttpSessionState)Session)[SFAFAppName + "Browser"]);
            }
            set
            {
                ((HttpSessionState)Session)[SFAFAppName + "Browser"] = value;
            }
        }

        public string ClientBrowserVersion
        {
            get
            {
                return ReturnAsString(((HttpSessionState)Session)[SFAFAppName + "BrowserVersion"]);
            }
            set
            {
                ((HttpSessionState)Session)[SFAFAppName + "BrowserVersion"] = value;
            }
        }

        public int CurrentUserId
        {
            get
            {
                int result = -1;

                if (((HttpSessionState)Session)["Current" + SFAFAppName + "UserId"] != null)
                    result = ReturnAsInt(ReturnAsString(((HttpSessionState)Session)["Current" + SFAFAppName + "UserId"]));

                return result;
            }
            set
            {
                ((HttpSessionState)Session)["Current" + SFAFAppName + "UserId"] = value;
            }
        }

        public int CurrentCustomerId
        {
            get
            {
                int result = -1;

                if (((HttpSessionState)Session)[SFAFAppName + "CurrentCustomerId"] != null)
                    result = ReturnAsInt(((HttpSessionState)Session)[SFAFAppName + "CurrentCustomerId"].ToString());

                return result;
            }
            set
            {
                ((HttpSessionState)Session)[SFAFAppName + "CurrentCustomerId"] = value;
            }
        }

        public int CurrentSchoolYearId
        {
            get
            {
                int result = -1;

                if (((HttpSessionState)Session)["Current" + SFAFAppName + "SchoolYearId"] != null)
                    result = (int)((HttpSessionState)Session)["Current" + SFAFAppName + "SchoolYearId"];

                return result;
            }
            set
            {
                ((HttpSessionState)Session)["Current" + SFAFAppName + "SchoolYearId"] = value;
            }
        }

        public bool IsDemo
        {
            get
            {
                bool result = false;

                if (((HttpSessionState)Session)["Current" + SFAFAppName + "IsDemo"] != null)
                    result = (bool)((HttpSessionState)Session)["Current" + SFAFAppName + "IsDemo"];

                return result;
            }
            set
            {
                ((HttpSessionState)Session)["Current" + SFAFAppName + "IsDemo"] = value;
            }
        }

        public bool IsDistrict
        {
            get
            {
                bool result = false;

                if (((HttpSessionState)Session)["Current" + SFAFAppName + "IsDistrict"] != null)
                    result = (bool)((HttpSessionState)Session)["Current" + SFAFAppName + "IsDistrict"];

                return result;
            }
            set
            {
                ((HttpSessionState)Session)["Current" + SFAFAppName + "IsDistrict"] = value;
            }
        }

        public bool IsTrackSchool
        {
            get
            {
                bool result = false;

                if (((HttpSessionState)Session)["Current" + SFAFAppName + "IsTrackSchool"] != null)
                    result = (bool)((HttpSessionState)Session)["Current" + SFAFAppName + "IsTrackSchool"];

                return result;
            }
            set
            {
                ((HttpSessionState)Session)["Current" + SFAFAppName + "IsTrackSchool"] = value;
            }
        }

        public void WriteMessage(string message)
        {
        }

        public bool SaveToPresenterCache(string key, object value)
        {
            bool result = false;

            key = key.Trim();

            if (!string.IsNullOrEmpty(key))
            {
                try
                {
                    ((HttpSessionState)Session)[SFAFAppName + key] = value;

                    if (!_cacheKeys.Contains(SFAFAppName + key))
                        _cacheKeys.Add(SFAFAppName + key);

                    ((HttpSessionState)Session)[SFAFAppName + "CacheKeys"] = _cacheKeys;

                    result = true;
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        public object GetFromPresenterCache(string key)
        {
            return ((HttpSessionState)Session)[SFAFAppName + key];
        }

        public bool ClearPresenterCache()
        {
            bool result = false;

            for (int x = 0; x < _cacheKeys.Count; x++)
            {
                try
                {
                    ((HttpSessionState)Session)[_cacheKeys[x]] = null;
                }
                catch { }

                try
                {
                    ((HttpSessionState)Session).Remove(_cacheKeys[x]);
                }
                catch { }
            }

            try
            {
                _cacheKeys.Clear();
            }
            catch { }

            return result;
        }

        public bool DeleteFromPresenterCache(string key)
        {
            bool result = false;

            for (int x = 0; x < _cacheKeys.Count; x++)
            {
                if (_cacheKeys[x] == SFAFAppName + key)
                {
                    try
                    {
                        ((HttpSessionState)Session)[_cacheKeys[x]] = null;
                    }
                    catch { }

                    try
                    {
                        ((HttpSessionState)Session).Remove(_cacheKeys[x]);
                    }
                    catch { }

                    try
                    {
                        _cacheKeys.Remove(_cacheKeys[x]);
                    }
                    catch { }

                    result = true;
                    break;
                }
            }

            return result;
        }

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

        public object GetFromGlobalCache(string key)
        {
            object result = null;

            if (ENABLE_GLOBAL_CACHE)
                result = Cache.Get(key);

            return result;
        }

        public bool DeleteFromGlobalCache(string key)
        {
            if (ENABLE_GLOBAL_CACHE)
            {
                try
                {
                    Cache.Remove(key);
                }
                catch { }
            }

            return true;
        }

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
                    catch { }
                }
            }

            return true;
        }

        #endregion

        private bool ReturnAsBool(string s)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    s = s.ToLower().Trim();

                    switch (s)
                    {
                        case "1":
                        case "true":
                        case "yes":
                            result = true;
                            break;
                        default:
                            result = false;
                            break;
                    }
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        protected string ReturnAsString(object o)
        {
            string result = string.Empty;

            if (o != null)
                result = o.ToString();

            return result;
        }

        protected int ReturnAsInt(string s)
        {
            int result = -1;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    result = Convert.ToInt32(s);
                }
                catch
                {
                    result = -1;
                }
            }

            return result;
        }
    }

    public class ErrorModule : IHttpModule
    {
        private HttpApplication _application;

        #region IHttpModule Members

        public void Init(HttpApplication application)
        {
            application.Error += new EventHandler(application_Error);
            _application = application;
        }
        
        public void Dispose() { }

        #endregion

        public void application_Error(object sender, EventArgs e)
        {
            //handle error
            Exception lastError = _application.Server.GetLastError();

            Exception exc = lastError;

            if (lastError != null)
            {
                if (lastError.InnerException != null)
                    exc = lastError.InnerException;

                _application.Server.ClearError();

                string errorCode = ((int)MemberCenter20Constants.SFAFMemberCenter20ExceptionCodes.DefaultExceptionCode).ToString();

                string redirectPage = ConfigurationManager.AppSettings["ErrorPage"];

                string errorMessage = exc.ToString();

                switch (exc.GetType().Name)
                {
                    case "SFAFSessionException":
                        redirectPage = "~/Login.aspx";
                        errorMessage = MemberCenter20Constants.SFAFSessionExpiredExceptionMessage;
                        errorCode = ((int)MemberCenter20Constants.SFAFMemberCenter20ExceptionCodes.SessionExceptionCode).ToString();
                        break;
                    case "SFAFSecurityException":
                        errorCode = ((int)MemberCenter20Constants.SFAFMemberCenter20ExceptionCodes.SecurityExceptionCode).ToString();
                        errorMessage = ((SFAFSecurityException)exc).ToString();
                        break;
                    case "SFAFGenericError":
                        errorMessage = ((SFAFGenericError)exc).ToString();
                        errorCode = ((int)MemberCenter20Constants.SFAFMemberCenter20ExceptionCodes.GenericExceptionCode).ToString();
                        break;
                    case "HttpException":
                        if (exc.Message.StartsWith("The file") && exc.Message.EndsWith(" does not exist."))
                        {
                            errorMessage = "Page not Found.";
                            errorCode = "404";
                            redirectPage = "~/CustomPages/Custom404.aspx";
                        }
                        break;
                    default:
                        break;
                }

                string msg = _application.Server.UrlEncode(errorMessage);
                msg = msg.Replace("(", "-").Replace(")", "_");

                bool redirectToErrorPage = true;

                try
                {
                    if (_application.Session[ConfigurationManager.AppSettings["SFAFAppName"] + "ErrorMessage"] != null)
                    {
                        _application.Session[ConfigurationManager.AppSettings["SFAFAppName"] + "ErrorMessage"] = msg;
                        msg = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Session state is not available in this context.")
                    {
                        redirectToErrorPage = false;
                    }
                }

                if (redirectToErrorPage)
                    _application.Response.Redirect(redirectPage + "?ErrorCode=" + errorCode + "&ErrorMessage=" + msg, true);
            }
        }
    }

    public class HttpHeaderCleanup : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += new EventHandler(context_PreSendRequestHeaders);
        }

        private void context_PreSendRequestHeaders(object sender, EventArgs e)
        {
            if (HttpContext.Current != null)
            {
                HttpResponse response = HttpContext.Current.Response;

                if (response != null)
                {
                    string[] headers = new string[] { "Server", "ETag" };

                    try
                    {
                        for (int x = 0; x < headers.Length; x++)
                            response.Headers.Remove(headers[x]);
                    }
                    catch { }
                }
            }
        }

        public void Dispose() { }
    }

    [Serializable]
    public class SFAFMemberCenter20TopNavigation
    {
        private string _type = string.Empty;
        private string _text = string.Empty;
        private string _url = string.Empty;
        private string[] _objectType = new string[0];
        private string _privilegeLevel = string.Empty;
        private string _scope = string.Empty;
        private string _centerName = string.Empty;
        private string _description = string.Empty;
        private string _centerDescription = string.Empty;

        private bool _hasPermission = false;

        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        public string[] ObjectType
        {
            get
            {
                return _objectType;
            }
            set
            {
                _objectType = value;
            }
        }

        public string PrivilegeLevel
        {
            get
            {
                return _privilegeLevel;
            }
            set
            {
                _privilegeLevel = value;
            }
        }

        public string Scope
        {
            get
            {
                return _scope;
            }
            set
            {
                _scope = value;
            }
        }

        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }

        public string CenterName
        {
            get
            {
                return _centerName;
            }
            set
            {
                _centerName = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public string CenterDescription
        {
            get
            {
                return _centerDescription;
            }
            set
            {
                _centerDescription = value;
            }
        }

        public bool HasPermission
        {
            get
            {
                return _hasPermission;
            }
            set
            {
                _hasPermission = value;
            }
        }
    }

    public class SFAFMemberCenter20RequiredFieldControl
    {
        private Control _securityControl = null;
        private SALIWebControlTypes _controlType = SALIWebControlTypes.None;
        private string _errorMessage = string.Empty;

        public SFAFMemberCenter20RequiredFieldControl(Control requiredControl, SALIWebControlTypes controlType, string errorMessage)
        {
            RequiredControl = requiredControl;
            ControlType = controlType;
            ErrorMessage = errorMessage;
        }

        public Control RequiredControl
        {
            get
            {
                return _securityControl;
            }
            private set
            {
                _securityControl = value;
            }
        }

        public SALIWebControlTypes ControlType
        {
            get
            {
                return _controlType;
            }
            private set
            {
                _controlType = value;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            private set
            {
                _errorMessage = value;
            }
        }
    }

    ///<summary>
    /// A helper class that decodes, deserializes the '__ViewState' hidden field
    /// of an ASP.Net in to an object and then XML serializes the object to provide
    /// a navigable map of the view state and control state.
    ///</summary>
    public class ViewStateParser
    {
        #region Fields

        private readonly XmlDocument _controlState;

        private readonly string _text;

        private readonly object _value;

        private readonly XmlDocument _viewState;

        #endregion

        #region Constructors

        ///<summary>
        ///</summary>
        ///<param name="fieldValue"></param>
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

        ///<summary>
        ///</summary>
        public XmlDocument ControlState
        {
            get { return _controlState; }
        }

        ///<summary>
        /// The unencoded __ViewState
        ///</summary>
        public string Text
        {
            get { return _text; }
        }


        ///<summary>
        /// The ViewState object
        ///</summary>
        public object Value
        {
            get { return _value; }
        }


        ///<summary>
        ///
        ///</summary>
        public XmlDocument ViewState
        {
            get { return _viewState; }
        }

        #endregion

        #region Private Methods

        private static void XmlSerializeValue(XmlDocument viewState, XmlDocument controlState, XmlNode parentNode, object value)
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