using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using TTApi.Models;

namespace TTApi.Controllers
{
    /// <summary>
    /// Link http://43.254.133.49:8015/TTApi/CheckList/Profile
    /// </summary>
    [Authorize]
    [RoutePrefix("CheckList/Profile")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProfileController : ApiController
    {

        #region EquipmentSafety
        // POST CheckList/Profile/GetEquipmentSafetyAll
        /// <summary>
        /// แสดงข้อมูลอุปกรณ์ขนส่งทั้งหมด
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetEquipmentSafetyAll")]
        public List<EquipmentSafetyView> GetEquipmentSafetyAll()
        {
            HomeController hc = new HomeController();
            List<EquipmentSafetyView> ul = new List<EquipmentSafetyView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "SELECT es.[eq_safety_id], es.[eq_safety_code], es.[eq_name], es.[style], es.[property], es.[suggestion], es.[eq_type_id], et.[eq_type], es.[eq_path], es.[create_date], es.[create_by_user_id], es.[update_date], es.[update_by_user_id] FROM [equipment_safety] as es join equipment_type as et on es.eq_type_id = et.eq_type_id ";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        EquipmentSafetyView esv = new EquipmentSafetyView();
                        esv.eq_safety_id = _Item["eq_safety_id"].ToString();
                        esv.eq_safety_code = _Item["eq_safety_code"].ToString();
                        esv.eq_name = _Item["eq_name"].ToString();
                        esv.style = _Item["style"].ToString();
                        esv.property = _Item["property"].ToString();
                        esv.suggestion = _Item["suggestion"].ToString();
                        esv.eq_type_id = _Item["eq_type_id"].ToString();
                        esv.eq_type = _Item["eq_type"].ToString();
                        esv.eq_path = _Item["eq_path"].ToString();
                        ul.Add(esv);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/InsertEquipmentSafety
        /// <summary>
        /// var model = new FormData();
        /// model.append('eq_safety_code', 'รหัสอุปกรณ์');
        /// model.append('eq_name', 'ชื่ออุปกรณ์');
        /// model.append('style', 'ลักษณะ');
        /// model.append('property', 'คุณสมบัติ');
        /// model.append('suggestion', 'คำแนะนำ');
        /// model.append('eq_type_id', 'ปกติ = 1, พิเศษ = 2');
        /// model.append('Image', $("#myFile")[0].files[0]);
        /// ***ajax*** 
        /// data: model,
        /// processData: false,
        /// contentType: false,
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertEquipmentSafety")]
        public ExecuteModels InsertEquipmentSafety()
        {
            ExecuteModels ecm = new ExecuteModels();
            NameValueCollection nvc = HttpContext.Current.Request.Form;
            var val = new EquipmentSafety();
            foreach (string kvp in nvc.AllKeys)
            {
                if (kvp != "Image")
                {
                    PropertyInfo pi = val.GetType().GetProperty(kvp, BindingFlags.Public | BindingFlags.Instance);
                    if (pi != null)
                    {
                        if (nvc[kvp] != "undefined")
                        {
                            pi.SetValue(val, nvc[kvp], null);
                        }
                    }
                }
            }            
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "insert into equipment_safety (eq_safety_code, eq_name, style, property, suggestion, eq_type_id, create_by_user_id) output inserted.eq_safety_id values (N'" + val.eq_safety_code + "', N'" + val.eq_name + "', N'" + val.style + "', N'" + val.property + "', N'" + val.suggestion + "', " + val.eq_type_id + ", 1)";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    try
                    {
                        var id_return = Int32.Parse(cmd.ExecuteScalar().ToString());
                        if (id_return >= 1)
                        {
                            // Upload file
                            string path = string.Empty;
                            if (HttpContext.Current.Request.Files.Count > 0)
                            {
                                try
                                {
                                    val.Image = HttpContext.Current.Request.Files[0];
                                    path = HttpContext.Current.Request.MapPath(@"~/Files/es/" + id_return.ToString() + ".png");
                                    val.Image.SaveAs(path);
                                    _SQL = "update equipment_safety set eq_path = N'" + path + "' where eq_safety_id = " + id_return;
                                    using (SqlCommand cmd_update = new SqlCommand(_SQL, con))
                                    {
                                        if (Int32.Parse(cmd_update.ExecuteNonQuery().ToString()) == 1)
                                        {
                                            ecm.result = 0;
                                            ecm.code = "OK";
                                            ecm.id_return = id_return.ToString();
                                        }
                                        else
                                        {
                                            ecm.result = 1;
                                            ecm.code = "error about update eq_path";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ecm.result = 1;
                                    ecm.code = ex.Message;
                                    return ecm;
                                }
                            }
                            // End Upload file
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

        // POST CheckList/Profile/UpdateEquipmentSafety
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("UpdateEquipmentSafety")]
        public ExecuteModels UpdateEquipmentSafety()
        {
            ExecuteModels ecm = new ExecuteModels();
            NameValueCollection nvc = HttpContext.Current.Request.Form;
            var val = new EquipmentSafety();
            foreach (string kvp in nvc.AllKeys)
            {
                if(kvp != "Image")
                {
                    PropertyInfo pi = val.GetType().GetProperty(kvp, BindingFlags.Public | BindingFlags.Instance);
                    if (pi != null)
                    {
                        if(nvc[kvp] != "undefined")
                        {
                            pi.SetValue(val, nvc[kvp], null);
                        }                        
                    }
                }                
            }
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL_Set = string.Empty;
                string[] Col_Arr = { "eq_safety_code", "eq_name", "style", "property", "suggestion", "eq_type_id" };
                string[] Val_Arr = { val.eq_safety_code, val.eq_name, val.style, val.property, val.suggestion, val.eq_type_id };
                for (int n = 0; n <= Val_Arr.Length - 1; n++)
                {
                    if (Val_Arr[n] != null)
                    {
                        _SQL_Set += Col_Arr[n] + " = N'" + Val_Arr[n] + "', ";
                    }
                }

                // Upload file
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    string path = string.Empty;
                    if (HttpContext.Current.Request.Files.Count > 0)
                    {
                        try
                        {
                            val.Image = HttpContext.Current.Request.Files[0];
                            path = HttpContext.Current.Request.MapPath(@"~/Files/es/" + val.eq_safety_id + ".png");
                            val.Image.SaveAs(path);                            
                        }
                        catch (Exception ex)
                        {
                            ecm.result = 1;
                            ecm.code = ex.Message;
                            return ecm;
                        }
                    }
                }
                // End Upload file

                string _SQL = "update equipment_safety set " + _SQL_Set + " create_by_user_id = 1 where eq_safety_id = " + val.eq_safety_id;
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

        // POST CheckList/Profile/DelEquipmentSafety
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DeleteEquipmentSafety")]
        public ExecuteModels DelEquipmentSafety(EquipmentSafety val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "delete from equipment_safety where eq_safety_id = " + val.eq_safety_id;
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
        #endregion

        #region EquipmentTransport
        // POST CheckList/Profile/GetEquipmentTransportAll
        [AllowAnonymous]
        [Route("GetEquipmentTransportAll")]
        public List<EquipmentTransportView> GetEquipmentTransportAll()
        {
            HomeController hc = new HomeController();
            List<EquipmentTransportView> ul = new List<EquipmentTransportView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "SELECT es.[eq_tran_id], es.[eq_tran_code], es.[eq_name], es.[style], es.[property], es.[suggestion], es.[eq_type_id], et.[eq_type], es.[eq_path], es.[create_date], es.[create_by_user_id], es.[update_date], es.[update_by_user_id] FROM [equipment_transport] as es join equipment_type as et on es.eq_type_id = et.eq_type_id ";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        EquipmentTransportView etv = new EquipmentTransportView();
                        etv.eq_tran_id = _Item["eq_tran_id"].ToString();
                        etv.eq_tran_code = _Item["eq_tran_code"].ToString();
                        etv.eq_name = _Item["eq_name"].ToString();
                        etv.style = _Item["style"].ToString();
                        etv.property = _Item["property"].ToString();
                        etv.suggestion = _Item["suggestion"].ToString();
                        etv.eq_type_id = _Item["eq_type_id"].ToString();
                        etv.eq_type = _Item["eq_type"].ToString();
                        etv.eq_path = _Item["eq_path"].ToString();
                        ul.Add(etv);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/InsertEquipmentTransport
        /// <summary>
        /// var model = new FormData();
        /// model.append('eq_tran_code', 'รหัสอุปกรณ์');
        /// model.append('eq_name', 'ชื่ออุปกรณ์');
        /// model.append('style', 'ลักษณะ');
        /// model.append('property', 'คุณสมบัติ');
        /// model.append('suggestion', 'คำแนะนำ');
        /// model.append('eq_type_id', 'ปกติ = 1, พิเศษ = 2');
        /// model.append('Image', $("#myFile")[0].files[0]);
        /// ***ajax*** 
        /// data: model,
        /// processData: false,
        /// contentType: false
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertEquipmentTransport")]
        public ExecuteModels InsertEquipmentTransport()
        {
            ExecuteModels ecm = new ExecuteModels();
            NameValueCollection nvc = HttpContext.Current.Request.Form;
            var val = new EquipmentTransport();
            foreach (string kvp in nvc.AllKeys)
            {
                if (kvp != "Image")
                {
                    PropertyInfo pi = val.GetType().GetProperty(kvp, BindingFlags.Public | BindingFlags.Instance);
                    if (pi != null)
                    {
                        if (nvc[kvp] != "undefined")
                        {
                            pi.SetValue(val, nvc[kvp], null);
                        }
                    }
                }
            }
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "insert into equipment_transport (eq_tran_code, eq_name, style, property, suggestion, eq_type_id, create_by_user_id) output inserted.eq_tran_id values (N'" + val.eq_tran_code + "', N'" + val.eq_name + "', N'" + val.style + "', N'" + val.property + "', N'" + val.suggestion + "', " + val.eq_type_id + ", 1)";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    try
                    {
                        var id_return = Int32.Parse(cmd.ExecuteScalar().ToString());
                        if (id_return >= 1)
                        {
                            // Upload file
                            string path = string.Empty;
                            if (HttpContext.Current.Request.Files.Count > 0)
                            {
                                try
                                {
                                    val.Image = HttpContext.Current.Request.Files[0];
                                    path = HttpContext.Current.Server.MapPath("~/Files/et/" + id_return.ToString() + ".png");
                                    val.Image.SaveAs(path);
                                    _SQL = "update equipment_transport set eq_path = N'" + path + "' where eq_tran_id = " + id_return;
                                    using (SqlCommand cmd_update = new SqlCommand(_SQL, con))
                                    {
                                        if (Int32.Parse(cmd_update.ExecuteNonQuery().ToString()) == 1)
                                        {
                                            ecm.result = 0;
                                            ecm.code = "OK";
                                            ecm.id_return = id_return.ToString();
                                        }
                                        else
                                        {
                                            ecm.result = 1;
                                            ecm.code = "error about update eq_path";                                            
                                        }                                        
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ecm.result = 1;
                                    ecm.code = ex.Message;
                                }
                            }
                            // End Upload file
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

        // POST CheckList/Profile/UpdateEquipmentTransport
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("UpdateEquipmentTransport")]
        public ExecuteModels UpdateEquipmentTransport()
        {
            ExecuteModels ecm = new ExecuteModels();
            NameValueCollection nvc = HttpContext.Current.Request.Form;
            var val = new EquipmentTransport();
            foreach (string kvp in nvc.AllKeys)
            {
                if (kvp != "Image")
                {
                    PropertyInfo pi = val.GetType().GetProperty(kvp, BindingFlags.Public | BindingFlags.Instance);
                    if (pi != null)
                    {
                        if (nvc[kvp] != "undefined")
                        {
                            pi.SetValue(val, nvc[kvp], null);
                        }
                    }
                }
            }
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL_Set = string.Empty;
                string[] Col_Arr = { "eq_tran_code", "eq_name", "style", "property", "suggestion", "eq_type_id" };
                string[] Val_Arr = { val.eq_tran_code, val.eq_name, val.style, val.property, val.suggestion, val.eq_type_id };
                for (int n = 0; n <= Val_Arr.Length - 1; n++)
                {
                    if(Val_Arr[n] != null)
                    {
                        _SQL_Set += Col_Arr[n] + " = N'" + Val_Arr[n] + "', ";
                    }
                }

                // Upload file
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    string path = string.Empty;
                    if (HttpContext.Current.Request.Files.Count > 0)
                    {
                        try
                        {
                            val.Image = HttpContext.Current.Request.Files[0];
                            path = HttpContext.Current.Request.MapPath(@"~/Files/et/" + val.eq_tran_id + ".png");
                            val.Image.SaveAs(path);
                        }
                        catch (Exception ex)
                        {
                            ecm.result = 1;
                            ecm.code = ex.Message;
                            return ecm;
                        }
                    }
                }
                // End Upload file

                string _SQL = "update equipment_transport set " + _SQL_Set + " create_by_user_id = 1 where eq_tran_id = " + val.eq_tran_id;
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

        // POST CheckList/Profile/DeleteEquipmentTransport
        [AllowAnonymous]
        [Route("DeleteEquipmentTransport")]
        public ExecuteModels DelEquipmentTransport(EquipmentTransport val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "delete from equipment_transport where eq_tran_id = " + val.eq_tran_id;
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
        #endregion
    }
}
