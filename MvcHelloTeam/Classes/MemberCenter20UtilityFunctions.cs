using System.Collections.Generic;
using System.Xml;
using System.Configuration;
using SALIBusinessLogic;
using System;
using SFAFUtilityObjects;
using SALI;
using SFAFGlobalObjects;

namespace MemberCenter20NS
{
    public static class MemberCenter20UtilityFunctions
    {
        public static List<SFAFMemberCenter20TopNavigation> GetTopNavigation(SALIMainBusinessLogic businessLogic, string userName, string path, int customerId, bool isDemo)
        {
            string objectList = string.Empty;
            string levelList = string.Empty;
            string scopeList = string.Empty;
            string delimiter = string.Empty;

            List<SFAFMemberCenter20TopNavigation> result = new List<SFAFMemberCenter20TopNavigation>();

            XmlDocument mainXml = new XmlDocument();
            mainXml.Load(SFAFileFunction.FormatDirectory(path) + "TopNavMenus.xml");

            XmlNodeList mainNodes = mainXml.SelectNodes("TopNavMenus/TopNavMenu");

            for (int count = 0; count < mainNodes.Count; count++)
            {
                if (count > 0)
                    delimiter = "~";

                XmlNode node = mainNodes[count].SelectSingleNode("Name");
                XmlNode centerDescription = mainNodes[count].SelectSingleNode("Description");

                XmlNodeList nodes = mainNodes[count].SelectNodes("Links/Link");

                for (int x = 0; x < nodes.Count; x++)
                {
                    if (x > 0)
                        delimiter = "~";

                    string text = nodes[x].SelectSingleNode("Text").InnerText;

                    string url = nodes[x].SelectSingleNode("Url").InnerText;

                    string type = nodes[x].SelectSingleNode("Type").InnerText;

                    string[] objectType = nodes[x].SelectSingleNode("ObjectType").InnerText.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    string level = nodes[x].SelectSingleNode("PrivilegeLevel").InnerText;

                    string scope = nodes[x].SelectSingleNode("Scope").InnerText;

                    string description = text;

                    XmlNode nd = nodes[x].SelectSingleNode("Description");

                    if (nd != null)
                        description = nd.InnerText;

                    bool hasPermission = false;

                    if (objectType != null && objectType.Length > 0)
                    {
                        objectList += delimiter + objectType[0];
                        levelList += delimiter + Convert.ToString((int)businessLogic.ConvertStringToSecurityLevelEnum(level));
                        scopeList += delimiter + Convert.ToString((int)businessLogic.ConvertStringToSecurityScopeEnum(scope));
                    }
                    else
                    {
                        objectList += delimiter + "skip";
                        levelList += delimiter + "1";
                        scopeList += delimiter + "0";
                        hasPermission = true;
                    }

                    if (type.Trim().ToLower() == "separator" && text == string.Empty)
                        text = string.Empty.PadLeft(60, '-');

                    SFAFMemberCenter20TopNavigation tn = new SFAFMemberCenter20TopNavigation();
                    tn.CenterName = node.InnerText;
                    tn.CenterDescription = centerDescription.InnerText;
                    tn.ObjectType = objectType;
                    tn.Text = text;
                    tn.Url = url;
                    tn.Type = type;
                    tn.PrivilegeLevel = level;
                    tn.Scope = scope;
                    tn.HasPermission = hasPermission;
                    tn.Description = description;

                    result.Add(tn);
                }
            }

            List<StoredProcParameter> input = new List<StoredProcParameter>();

            input.Add(new StoredProcParameter(System.Data.DbType.String, "@UserName", userName, int.MaxValue));
            input.Add(new StoredProcParameter(System.Data.DbType.String, "@ObjectList", objectList, int.MaxValue));
            input.Add(new StoredProcParameter(System.Data.DbType.String, "@LevelList", levelList, int.MaxValue));
            input.Add(new StoredProcParameter(System.Data.DbType.String, "@ScopeList", scopeList, int.MaxValue));
            input.Add(new StoredProcParameter(System.Data.DbType.Int32, "@CustomerId", customerId, sizeof(int)));

            List<StoredProcParameter> output = new List<StoredProcParameter>();

            output.Add(new StoredProcParameter(System.Data.DbType.String, "@ReturnList", null, int.MaxValue));

            DatabaseExecution.ExecuteStoredProcNonQuery("_SALI_CheckUserObjectPermissionBatch", input, output, ConfigurationManager.AppSettings["Database"]);

            string[] results = output[0].Value.ToString().Split(new string[] { "~" }, StringSplitOptions.None);

            for (int x = 0; x < result.Count; x++)
            {
                if (isDemo)
                    result[x].HasPermission = true;
                else
                {
                    if (results[x] == "1")
                        result[x].HasPermission = true;
                    else
                    {
                        result[x].HasPermission = false;
                        result[x].Url = string.Empty;
                    }
                }
            }

            return result;
        }
    }
}