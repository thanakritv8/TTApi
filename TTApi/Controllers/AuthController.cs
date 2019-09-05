using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using TTApi.Models;

namespace TTApi.Controllers
{
    [Authorize]
    [RoutePrefix("Auth")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthController : ApiController
    {

        #region User

        [AllowAnonymous]
        [Route("CheckLogin")]
        public LoginViews CheckLogin(LoginModels val)
        {
            LoginViews lm = new LoginViews();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabaseAuth())
            {
                string _SQL = "select * from account where username = '" + val.username + "' and password = '" + hc.EncryptSHA256Managed(val.password) + "'";
                SqlCommand cmd = new SqlCommand(_SQL, con);
                DataTable _Dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(_Dt);
                da.Dispose();
                if (_Dt.Rows.Count > 0)
                {
                    lm.user_id = _Dt.Rows[0]["user_id"].ToString();
                    lm.firstname = _Dt.Rows[0]["firstname"].ToString();
                    lm.lastname = _Dt.Rows[0]["lastname"].ToString();
                    lm.tel = _Dt.Rows[0]["tel"].ToString();
                    lm.address = _Dt.Rows[0]["address"].ToString();
                    lm.email = _Dt.Rows[0]["email"].ToString();
                    lm.group_id = _Dt.Rows[0]["group_id"].ToString();
                    _SQL = "select * from permission where group_id = " + lm.group_id;
                    cmd = new SqlCommand(_SQL, con);
                    _Dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        PermissionModels p = new PermissionModels();
                        p.app_id = _Item["app_id"].ToString();
                        p.access_id = _Item["access_id"].ToString();
                        lm.lsPermission.Add(p);
                    }
                }
                con.Close();
            }
            return lm;
        }

        [AllowAnonymous]
        [Route("InsertUser")]
        public ExecuteModels InsertUser(UserModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabaseAuth())
            {
                string _SQL = "insert into account (firstname, lastname, tel, address, email, username, password, group_id, create_by_user_id) output inserted.user_id " +
                    "values (N'" + val.firstname + "', N'" + val.lastname + "', '" + val.tel+ "', N'" + val.address + "', '" + val.email + "', '" + val.username + "', '" + hc.EncryptSHA256Managed(val.password) + "', " + val.group_id + ", 1)";
                SqlCommand cmd = new SqlCommand(_SQL, con);
                try
                {
                    var id_return = Int32.Parse(cmd.ExecuteScalar().ToString());
                    if (id_return >= 1)
                    {
                        ecm.result = 0;
                        ecm.code = "OK";
                        ecm.id_return = id_return.ToString();
                    }
                }
                catch (Exception ex)
                {
                    ecm.result = 1;
                    ecm.code = ex.Message;
                }
                con.Close();
            }
            return ecm;
        }

        [AllowAnonymous]
        [Route("UpdateUser")]
        public ExecuteModels UpdateUser(UserModels val)
        {
            HomeController hc = new HomeController();
            ExecuteModels ecm = new ExecuteModels();
            string _SQL_Set = string.Empty;
            string[] Col_Arr = { "firstname", "lastname", "tel", "address", "email", "username", "password", "group_id" };
            string[] Val_Arr = { val.firstname, val.lastname, val.tel, val.address, val.email, val.username, hc.EncryptSHA256Managed(val.password), val.group_id };
            for (int n = 0; n <= Val_Arr.Length - 1; n++)
            {
                if (Val_Arr[n] != null)
                {
                    _SQL_Set += Col_Arr[n] + " = N'" + Val_Arr[n] + "', ";
                }
            }
            
            using (SqlConnection con = hc.ConnectDatabaseAuth())
            {
                string _SQL = "update account set " + _SQL_Set + " create_by_user_id = 1 where user_id = " + val.user_id;
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    try
                    {
                        if (Int32.Parse(cmd.ExecuteNonQuery().ToString()) == 1)
                        {
                            ecm.result = 0;
                            ecm.code = "OK";
                        }
                        else
                        {
                            ecm.result = 1;
                            ecm.code = _SQL;
                        }
                    }
                    catch (Exception ex)
                    {
                        ecm.result = 1;
                        ecm.code = ex.Message;
                    }
                }
                con.Close();
            }
            return ecm;
        }

        [AllowAnonymous]
        [Route("DeleteUser")]
        public ExecuteModels DelUser(UserModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabaseAuth())
            {
                string _SQL = "delete from account where user_id = " + val.user_id;
                SqlCommand cmd = new SqlCommand(_SQL, con);
                try
                {
                    if (Int32.Parse(cmd.ExecuteNonQuery().ToString()) == 1)
                    {
                        ecm.result = 0;
                        ecm.code = "OK";                        
                    }
                    else
                    {
                        ecm.result = 1;
                        ecm.code = _SQL;
                    }
                }
                catch (Exception ex)
                {
                    ecm.result = 1;
                    ecm.code = ex.Message;
                }
                con.Close();
            }
            return ecm;
        }

        [AllowAnonymous]
        [Route("GetUsersAll")]
        public List<UserModels> GetUsersAll()
        {
            HomeController hc = new HomeController();
            List<UserModels> ul = new List<UserModels>();
            using (SqlConnection con = hc.ConnectDatabaseAuth())
            {
                string _SQL = "select * from account";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        UserModels m = new UserModels();
                        m.user_id = _Item["user_id"].ToString();
                        m.username = _Item["username"].ToString();
                        m.firstname = _Item["firstname"].ToString();
                        m.lastname = _Item["lastname"].ToString();
                        m.tel = _Item["tel"].ToString();
                        m.email = _Item["email"].ToString();
                        m.address = _Item["address"].ToString();
                        m.group_id = _Item["group_id"].ToString();
                        ul.Add(m);
                    }
                }
                con.Close();
            }
            return ul;
        }

        #endregion

        #region Groups
        [AllowAnonymous]
        [Route("GetGroupAll")]
        public List<GroupsModels> GetGroupAll()
        {
            HomeController hc = new HomeController();
            List<GroupsModels> ul = new List<GroupsModels>();
            using (SqlConnection con = hc.ConnectDatabaseAuth())
            {
                string _SQL = "select * from [group]";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        GroupsModels m = new GroupsModels();
                        m.group_id = _Item["group_id"].ToString();
                        m.group_name = _Item["group_name"].ToString();
                        m.remark = _Item["remark"].ToString();
                        m.create_date = _Item["create_Date"].ToString();
                        ul.Add(m);
                    }
                }
                con.Close();
            }
            return ul;
        }

        [AllowAnonymous]
        [Route("InsertGroup")]
        public ExecuteModels InsertGroup(GroupsModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabaseAuth())
            {
                string _SQL = "insert into [group] (group_name, remark, project_id, create_by_user_id) output inserted.group_id " +
                    "values (N'" + val.group_name + "', N'" + val.remark + "', 2, 1)";
                SqlCommand cmd = new SqlCommand(_SQL, con);
                try
                {
                    var id_return = Int32.Parse(cmd.ExecuteScalar().ToString());
                    if (id_return >= 1)
                    {
                        ecm.result = 0;
                        ecm.code = "OK";
                        ecm.id_return = id_return.ToString();
                    }
                }
                catch (Exception ex)
                {
                    ecm.result = 1;
                    ecm.code = ex.Message;
                }
                con.Close();
            }
            return ecm;
        }

        [AllowAnonymous]
        [Route("UpdateGroup")]
        public ExecuteModels UpdateGroup(GroupsModels val)
        {
            HomeController hc = new HomeController();
            ExecuteModels ecm = new ExecuteModels();
            string _SQL_Set = string.Empty;
            string[] Col_Arr = { "group_name", "remark" };
            string[] Val_Arr = { val.group_name, val.remark };
            for (int n = 0; n <= Val_Arr.Length - 1; n++)
            {
                if (Val_Arr[n] != null)
                {
                    _SQL_Set += Col_Arr[n] + " = N'" + Val_Arr[n] + "', ";
                }
            }

            using (SqlConnection con = hc.ConnectDatabaseAuth())
            {
                string _SQL = "update [group] set " + _SQL_Set + " create_by_user_id = 1 where group_id = " + val.group_id;
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    try
                    {
                        if (Int32.Parse(cmd.ExecuteNonQuery().ToString()) == 1)
                        {
                            ecm.result = 0;
                            ecm.code = "OK";
                        }
                        else
                        {
                            ecm.result = 1;
                            ecm.code = _SQL;
                        }
                    }
                    catch (Exception ex)
                    {
                        ecm.result = 1;
                        ecm.code = ex.Message;
                    }
                }
                con.Close();
            }
            return ecm;
        }

        [AllowAnonymous]
        [Route("DeleteGroup")]
        public ExecuteModels DelGroup(GroupsModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabaseAuth())
            {
                string _SQL = "delete from [group] where group_id = " + val.group_id;
                SqlCommand cmd = new SqlCommand(_SQL, con);
                try
                {
                    if (Int32.Parse(cmd.ExecuteNonQuery().ToString()) >= 1)
                    {
                        ecm.result = 0;
                        ecm.code = "OK";
                    }
                    else
                    {
                        ecm.result = 1;
                        ecm.code = _SQL;
                    }
                }
                catch (Exception ex)
                {
                    ecm.result = 1;
                    ecm.code = ex.Message;
                }
                con.Close();
            }
            return ecm;
        }
        #endregion

        #region Permission

        [AllowAnonymous]
        [Route("GetPermissionAll")]
        public List<PermissionModels> GetPermissionAll()
        {
            HomeController hc = new HomeController();
            List<PermissionModels> ul = new List<PermissionModels>();
            using (SqlConnection con = hc.ConnectDatabaseAuth())
            {
                string _SQL = "select * from permission";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        PermissionModels m = new PermissionModels();
                        m.access_id = _Item["access_id"].ToString();
                        m.app_id = _Item["app_id"].ToString();
                        m.group_id = _Item["group_id"].ToString();
                        ul.Add(m);
                    }
                }
                con.Close();
            }
            return ul;
        }

        #endregion

        #region Application

        [AllowAnonymous]
        [Route("GetApplicationAll")]
        public List<AppsModels> GetApplicationAll()
        {
            HomeController hc = new HomeController();
            List<AppsModels> ul = new List<AppsModels>();
            using (SqlConnection con = hc.ConnectDatabaseAuth())
            {
                string _SQL = "select * from application";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        AppsModels m = new AppsModels();
                        m.app_id = _Item["app_id"].ToString();
                        m.app_name = _Item["app_name"].ToString();
                        ul.Add(m);
                    }
                }
                con.Close();
            }
            return ul;
        }

        #endregion
    }
}
