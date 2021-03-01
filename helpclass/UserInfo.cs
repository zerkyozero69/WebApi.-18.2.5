#region Assembly DevExpress.Persistent.BaseImpl.v18.2, Version=18.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// E:\@Workspace\Maiz\C#\WebApi.Jwt-master\WebApi.Jwt\bin\DevExpress.Persistent.BaseImpl.v18.2.dll
#endregion

using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.Security;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace DevExpress.Persistent.BaseImpl.PermissionPolicy.UserInfo
{
    [DefaultProperty("UserName")]
    [System.ComponentModel.DisplayName("User")]
    [ImageName("BO_User")]
    [Persistent("PermissionPolicyUser")]
    [RuleCriteria("PermissionPolicyUser_XPO_Prevent_delete_logged_in_user", DefaultContexts.Delete, "[Oid] != CurrentUserId()", "Cannot delete the current logged-in user. Please log in using another user account and retry.")]
    public class UserInfo : BaseObject, IPermissionPolicyUser, ISecurityUser, ISecurityUserWithRoles, IAuthenticationActiveDirectoryUser, IAuthenticationStandardUser
    {
        public UserInfo(Session session);

        public bool ChangePasswordOnFirstLogon { get; set; }
        public bool IsActive { get; set; }
        [Association]
        public XPCollection<PermissionPolicyRole> Roles { get; }
        [RuleRequiredField("PermissionPolicyUser_UserName_RuleRequiredField", DefaultContexts.Save)]
        [RuleUniqueValue("PermissionPolicyUser_UserName_RuleUniqueValue", DefaultContexts.Save, "The login with the entered user name was already registered within the system")]
        public string UserName { get; set; }
        [Browsable(false)]
        [Persistent]
        [SecurityBrowsable]
        [Size(-1)]
        protected string StoredPassword { get; set; }

        public bool ComparePassword(string password);
        public void SetPassword(string password);
        protected virtual IEnumerable<ISecurityRole> GetSecurityRoles();
    }
}


