using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTApi.Models
{
    public class LoginModels
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class LoginViews
    {
        public string user_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string tel { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string group_id { get; set; }
        public List<PermissionModels> lsPermission { get; set;} 
    }

    public class GroupsModels
    {
        public string group_id { get; set; }
        public string group_name { get; set; }
        public string remark { get; set; }
        public string create_date { get; set; }
    }

    public class AppsModels
    {
        public string app_id { get; set; }
        public string app_name { get; set; }
    }

    public class PermissionModels
    {
        public string app_id { get; set; }
        public string access_id { get; set; }
        public string group_id { get; set; }
    }

    public class UserModels
    {
        public string user_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string tel { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string group_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }

    public class AccessModels
    {
        public string access_id { get; set; }
        public string access_name { get; set; }
    }
}