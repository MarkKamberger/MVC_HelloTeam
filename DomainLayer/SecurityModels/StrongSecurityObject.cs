using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using DomainLayer.Helper;

namespace DomainLayer
{
    /// <summary>
    /// Creates the Secuurity Object Model 
    /// </summary>
    public class SSOFactory
    {
        
        public static StrongSecurityObject MakeObject(DataTable securityTable)
        {
            const BindingFlags bindings = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public |
                                          System.Reflection.BindingFlags.SetProperty;
            StrongSecurityObject securityObject = new StrongSecurityObject();
            securityObject.obj = new List<SecurityObjectSSO>();
            securityObject.Roles = new List<RoleSSO>();
            foreach (var row in securityTable.AsEnumerable())
            {
                securityObject.UserName = (string) row[1];
                if (!securityObject.Roles.Contains(row[7].ToString().ConvertToEnum<RoleSSO>()))
                {
                    securityObject.Roles.Add(row[7].ToString().ConvertToEnum<RoleSSO>());
                }
                securityObject.obj.Add(new SecurityObjectSSO
                {
                    Id = (Guid)row[3],
                    Object = row[2].ToString().ConvertToEnum<ObjectsSSO>(),
                    Privilege = row[4].ToString().ConvertToEnum<PrivilegeSSO>(),
                    Scope = row[5].ToString().ConvertToEnum<ScopeSSO>(),
                });
            }
            return securityObject;
        }
    }

    public class StrongSecurityObject
    {
        public IList<SecurityObjectSSO> obj;
        public string UserName { get; set; }
        public IList<RoleSSO> Roles;

    }


    #region Public Object

    #endregion

    #region Enumerables

    public enum ObjectsSSO
    {
        NONE = 0,
        AYPReports,
        Classroom,
        ClassroomsAndGroupsReports,
        Contact,
        Customer,
        CustomerImplementationReports,
        ExternalLink,
        Extracts,
        FilingCabinet,
        FrontOfficeReports,
        Imports,
        KinderCornerClassroom,
        LeadingForSuccess,
        LFS,
        Login,
        LoginIntranet,
        LongLesson,
        ManageClassrooms,
        ManageHotList,
        ManageLessons,
        ManageStaff,
        ManageStudents,
        ManageTests,
        ManageTestScores,
        ManageTutoringGroups,
        ReadingReports,
        SchoolConfiguration,
        SecuritySetup,
        SessionVisitLetter,
        SFAProgramsReports,
        StudentPromotion,
        StudentReports,
        StudentTestTargets,
        StudentTransfer,
        TeamSessionVisitLetter,
        TestReports,
        All
    }

    public enum PrivilegeSSO
    {
        NONE = 0,
        SELECT,
        INSERT,
        UPDATE,
        DELETE
    }
    public enum RoleSSO
    {
        AreaManager = 0,
        DataToolsFacilitator,
        DistrictUser,
        DistrictUserRestricted,
        IntranetUser,
        KinderCornerDataTools,
        LFS,
        MemberCenterStaffUser,
        ReportAdministrator,
        SecurityAdministrator,
        TechnicalSupport,
        clinek,
        FullAccess,
        MemberCenterUser,
        DefaultRole,
        ViewOnly,
        DefaultCustomerRole



    }
    public enum ScopeSSO
    {
        NONE = 0,
        SELF,
        SIBLING,
        CHILDREN,
        PARENT,
        ALL

    }
    #endregion

    public class SecurityObjectSSO
    {
        public ObjectsSSO Object { get; set; }
        public PrivilegeSSO Privilege { get; set; }
        public ScopeSSO Scope { get; set; }
        public Guid Id { get; set; }
    }
}
