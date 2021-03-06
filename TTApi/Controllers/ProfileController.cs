﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
    /// Link http://http://tabien.threetrans.com/TTApi/CheckList/Profile
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
                string _SQL = "SELECT es.[eq_safety_id], es.[eq_safety_code], es.[eq_name], es.[style], es.[property], es.[suggestion], es.[eq_type_id], et.[eq_type], es.[create_date], es.[create_by_user_id], es.[update_date], es.[update_by_user_id], es.eq_weight FROM [equipment_safety] as es join equipment_type as et on es.eq_type_id = et.eq_type_id ";
                SqlCommand cmd = new SqlCommand(_SQL, con);                
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
                    esv.eq_weight = _Item["eq_weight"].ToString();
                    _SQL = "SELECT * FROM file_all where table_id = 1 and fk_id = " + esv.eq_safety_id;
                    cmd = new SqlCommand(_SQL, con);
                    DataTable _DtFile = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(_DtFile);
                    da.Dispose();
                    esv.path = new List<FileAllView>();
                    foreach (DataRow _Path in _DtFile.Rows)
                    {
                        FileAllView f = new FileAllView();
                        f.seq = _Path["seq"].ToString();
                        f.path = _Path["path"].ToString();
                        esv.path.Add(f);
                    }
                    ul.Add(esv);
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
                string _SQL = "insert into equipment_safety (eq_safety_code, eq_name, style, property, suggestion, eq_type_id, eq_weight, create_by_user_id) output inserted.eq_safety_id values (N'" + val.eq_safety_code + "', N'" + val.eq_name + "', N'" + val.style + "', N'" + val.property + "', N'" + val.suggestion + "', " + val.eq_type_id + ", '" + val.eq_weight + "', " + val.user_id + ")";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    try
                    {
                        var id_return = Int32.Parse(cmd.ExecuteScalar().ToString());
                        if (id_return >= 1)
                        {
                            // Upload File
                            if (HttpContext.Current.Request.Files.Count > 0)
                            {
                                for(int n = 0; n <= HttpContext.Current.Request.Files.Count - 1; n++)
                                {
                                    try
                                    {
                                        System.Threading.Thread.Sleep(10);
                                        Random random = new Random();
                                        string filename = id_return.ToString() + DateTime.Now.ToString("yyyyMMddhhmmssffftt") + random.Next(0, 999999);
                                        string path = string.Empty;
                                        val.Image = HttpContext.Current.Request.Files[n];
                                        path = HttpContext.Current.Request.MapPath(@"~/Files/es/" + filename + ".png");
                                        val.Image.SaveAs(path);
                                        _SQL = "insert into file_all ([fk_id],[table_id],[path],[create_by_user_id]) VALUES (" + id_return + ", 1, N'" + path + "', " + val.user_id + ")";
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
                                                ecm.code = "error about insert file_all";
                                                return ecm;
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
                            }
                            else
                            {
                                ecm.result = 0;
                                ecm.code = "OK";
                                ecm.id_return = id_return.ToString();
                            }
                            // End Upload File                            
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
                string[] Col_Arr = { "eq_safety_code", "eq_name", "style", "property", "suggestion", "eq_type_id", "eq_weight" };
                string[] Val_Arr = { val.eq_safety_code, val.eq_name, val.style, val.property, val.suggestion, val.eq_type_id, val.eq_weight };
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
                    for (int n = 0; n <= HttpContext.Current.Request.Files.Count - 1; n++)
                    {
                        try
                        {
                            System.Threading.Thread.Sleep(10);
                            Random random = new Random();
                            string filename = val.eq_safety_id + DateTime.Now.ToString("yyyyMMddhhmmssffftt") + random.Next(0, 999999);
                            string path = string.Empty;
                            val.Image = HttpContext.Current.Request.Files[n];
                            path = HttpContext.Current.Request.MapPath(@"~/Files/es/" + filename + ".png");
                            val.Image.SaveAs(path);
                            string _SQL_file = "insert into file_all ([fk_id],[table_id],[path],[create_by_user_id]) VALUES (" + val.eq_safety_id + ", 1, N'" + path + "', " + val.user_id + ")";
                            using (SqlCommand cmd_update = new SqlCommand(_SQL_file, con))
                            {
                                if (Int32.Parse(cmd_update.ExecuteNonQuery().ToString()) == 1)
                                {
                                    ecm.result = 0;
                                    ecm.code = "OK";
                                }
                                else
                                {
                                    ecm.result = 1;
                                    ecm.code = "error about insert file_all";
                                    return ecm;
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
                }
                // End Upload file                

                string _SQL = "update equipment_safety set " + _SQL_Set + " update_by_user_id = " + val.user_id + " where eq_safety_id = " + val.eq_safety_id;
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
                string _SQL = "SELECT es.[eq_tran_id], es.[eq_tran_code], es.[eq_name], es.[style], es.[property], es.[suggestion], es.[eq_type_id], et.[eq_type], es.[eq_path], es.[create_date], es.[create_by_user_id], es.[update_date], es.[update_by_user_id], es.eq_weight FROM [equipment_transport] as es join equipment_type as et on es.eq_type_id = et.eq_type_id ";
                SqlCommand cmd = new SqlCommand(_SQL, con);
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
                    etv.eq_weight = _Item["eq_weight"].ToString();
                    _SQL = "SELECT * FROM file_all where table_id = 2 and fk_id = " + etv.eq_tran_id;
                    cmd = new SqlCommand(_SQL, con);
                    DataTable _DtFile = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(_DtFile);
                    da.Dispose();
                    etv.path = new List<FileAllView>();
                    foreach (DataRow _Path in _DtFile.Rows)
                    {
                        FileAllView f = new FileAllView();
                        f.seq = _Path["seq"].ToString();
                        f.path = _Path["path"].ToString();
                        etv.path.Add(f);
                    }
                    ul.Add(etv);
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
                string _SQL = "insert into equipment_transport (eq_tran_code, eq_name, style, property, suggestion, eq_type_id, eq_weight, create_by_user_id) output inserted.eq_tran_id values (N'" + val.eq_tran_code + "', N'" + val.eq_name + "', N'" + val.style + "', N'" + val.property + "', N'" + val.suggestion + "', " + val.eq_type_id + ", '" + val.eq_weight + "', " + val.user_id + ")";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    try
                    {
                        var id_return = Int32.Parse(cmd.ExecuteScalar().ToString());
                        if (id_return >= 1)
                        {
                            // Upload file
                            if (HttpContext.Current.Request.Files.Count > 0)
                            {
                                for (int n = 0; n <= HttpContext.Current.Request.Files.Count - 1; n++)
                                {
                                    try
                                    {
                                        System.Threading.Thread.Sleep(10);
                                        Random random = new Random();
                                        string filename = id_return.ToString() + DateTime.Now.ToString("yyyyMMddhhmmssffftt") + random.Next(0, 999999);
                                        string path = string.Empty;
                                        val.Image = HttpContext.Current.Request.Files[n];
                                        path = HttpContext.Current.Request.MapPath(@"~/Files/et/" + filename + ".png");
                                        val.Image.SaveAs(path);
                                        string _SQL_file = "insert into file_all ([fk_id],[table_id],[path],[create_by_user_id]) VALUES (" + id_return.ToString() + ", 2, N'" + path + "', " + val.user_id + ")";
                                        using (SqlCommand cmd_update = new SqlCommand(_SQL_file, con))
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
                                                ecm.code = "error about insert file_all";
                                                return ecm;
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
                            }
                            else
                            {
                                ecm.result = 0;
                                ecm.code = "OK";
                                ecm.id_return = id_return.ToString();
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
                string[] Col_Arr = { "eq_tran_code", "eq_name", "style", "property", "suggestion", "eq_type_id", "eq_weight" };
                string[] Val_Arr = { val.eq_tran_code, val.eq_name, val.style, val.property, val.suggestion, val.eq_type_id, val.eq_weight };
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
                    for (int n = 0; n <= HttpContext.Current.Request.Files.Count - 1; n++)
                    {
                        try
                        {
                            System.Threading.Thread.Sleep(10);
                            Random random = new Random();
                            string filename = val.eq_tran_id + DateTime.Now.ToString("yyyyMMddhhmmssffftt") + random.Next(0, 999999);
                            string path = string.Empty;
                            val.Image = HttpContext.Current.Request.Files[n];
                            path = HttpContext.Current.Request.MapPath(@"~/Files/et/" + filename + ".png");
                            val.Image.SaveAs(path);
                            string _SQL_file = "insert into file_all ([fk_id],[table_id],[path],[create_by_user_id]) VALUES (" + val.eq_tran_id + ", 2, N'" + path + "', " + val.user_id + ")";
                            using (SqlCommand cmd_update = new SqlCommand(_SQL_file, con))
                            {
                                if (Int32.Parse(cmd_update.ExecuteNonQuery().ToString()) == 1)
                                {
                                    ecm.result = 0;
                                    ecm.code = "OK";
                                }
                                else
                                {
                                    ecm.result = 1;
                                    ecm.code = "error about insert file_all";
                                    return ecm;
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
                }
                // End Upload file

                string _SQL = "update equipment_transport set " + _SQL_Set + " update_by_user_id = " + val.user_id + " where eq_tran_id = " + val.eq_tran_id;
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

        #region EquipmentType
        /// <summary>
        /// Get data equipment type
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetEquipmentType")]
        public List<EquipmentType> GetEquipmentType()
        {
            HomeController hc = new HomeController();
            List<EquipmentType> ul = new List<EquipmentType>();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = "SELECT * FROM [TT1995_CheckList].[dbo].[equipment_type]";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        EquipmentType et = new EquipmentType();
                        et.eq_type_id = _Item["eq_type_id"].ToString();
                        et.eq_type = _Item["eq_type"].ToString();

                        ul.Add(et);
                    }
                }
                con.Close();
            }
            return ul;
        }
        #endregion
        #endregion

        #region Document
        // POST CheckList/Profile/GetDocumentAll
        [AllowAnonymous]
        [Route("GetDocumentAll")]
        public List<DocumentView> GetDocumentAll()
        {
            HomeController hc = new HomeController();
            List<DocumentView> ul = new List<DocumentView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "SELECT d.*, dt.doc_type from document as d join document_type as dt on d.doc_type_id = dt.doc_type_id";
                SqlCommand cmd = new SqlCommand(_SQL, con);
                
                DataTable _Dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(_Dt);
                da.Dispose();
                foreach (DataRow _Item in _Dt.Rows)
                {
                    DocumentView dv = new DocumentView();
                    dv.doc_id = _Item["doc_id"].ToString();
                    dv.doc_code = _Item["doc_code"].ToString();
                    dv.doc_name = _Item["doc_name"].ToString();
                    dv.remark = _Item["remark"].ToString();
                    dv.doc_type_id = _Item["doc_type_id"].ToString();
                    dv.doc_type = _Item["doc_type"].ToString();
                    _SQL = "SELECT * FROM file_all where table_id = 3 and fk_id = " + dv.doc_id;
                    cmd = new SqlCommand(_SQL, con);
                    DataTable _DtFile = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(_DtFile);
                    da.Dispose();
                    dv.path = new List<FileAllView>();
                    foreach (DataRow _Path in _DtFile.Rows)
                    {
                        FileAllView f = new FileAllView();
                        f.seq = _Path["seq"].ToString();
                        f.path = _Path["path"].ToString();
                        dv.path.Add(f);
                    }
                    ul.Add(dv);
                }
                
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/InsertDocument
        /// <summary>
        /// var model = new FormData();
        /// model.append('doc_code', 'รหัสเอกสาร');
        /// model.append('doc_name', 'ชื่อเอกสาร');
        /// model.append('remark', 'คำอธิบาย');
        /// model.append('doc_type_id', 'ประเภทเอกสาร');
        /// model.append('Image', $("#myFile")[0].files[0]);
        /// ***ajax*** 
        /// data: model,
        /// processData: false,
        /// contentType: false
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertDocument")]
        public ExecuteModels InsertDocument()
        {
            ExecuteModels ecm = new ExecuteModels();
            NameValueCollection nvc = HttpContext.Current.Request.Form;
            var val = new Document();
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
                string _SQL = "insert into document (doc_code, doc_name, remark, doc_type_id, create_by_user_id) output inserted.doc_id values (N'" + val.doc_code + "', N'" + val.doc_name + "', N'" + val.remark + "', N'" + val.doc_type_id + "', " + val.user_id + ")";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    try
                    {
                        var id_return = Int32.Parse(cmd.ExecuteScalar().ToString());
                        if (id_return >= 1)
                        {
                            // Upload file
                            if (HttpContext.Current.Request.Files.Count > 0)
                            {
                                for (int n = 0; n <= HttpContext.Current.Request.Files.Count - 1; n++)
                                {
                                    try
                                    {
                                        System.Threading.Thread.Sleep(10);
                                        Random random = new Random();
                                        string filename = id_return.ToString() + DateTime.Now.ToString("yyyyMMddhhmmssffftt") + random.Next(0, 999999);
                                        string path = string.Empty;
                                        val.Image = HttpContext.Current.Request.Files[n];
                                        path = HttpContext.Current.Request.MapPath(@"~/Files/d/" + filename + ".png");
                                        val.Image.SaveAs(path);
                                        string _SQL_file = "insert into file_all ([fk_id],[table_id],[path],[create_by_user_id]) VALUES (" + id_return.ToString() + ", 3, N'" + path + "', " + val.user_id + ")";
                                        using (SqlCommand cmd_update = new SqlCommand(_SQL_file, con))
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
                                                ecm.code = "error about insert file_all";
                                                return ecm;
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
                            }
                            else
                            {
                                ecm.result = 0;
                                ecm.code = "OK";
                                ecm.id_return = id_return.ToString();
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

        // POST CheckList/Profile/UpdateDocument
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("UpdateDocument")]
        public ExecuteModels UpdateDocument()
        {
            ExecuteModels ecm = new ExecuteModels();
            NameValueCollection nvc = HttpContext.Current.Request.Form;
            var val = new Document();
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
                string[] Col_Arr = { "doc_code", "doc_name", "remark", "doc_type_id" };
                string[] Val_Arr = { val.doc_code, val.doc_name, val.remark, val.doc_type_id };
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
                    for (int n = 0; n <= HttpContext.Current.Request.Files.Count - 1; n++)
                    {
                        try
                        {
                            System.Threading.Thread.Sleep(10);
                            Random random = new Random();
                            string filename = val.doc_id + DateTime.Now.ToString("yyyyMMddhhmmssffftt") + random.Next(0, 999999);
                            string path = string.Empty;
                            val.Image = HttpContext.Current.Request.Files[n];
                            path = HttpContext.Current.Request.MapPath(@"~/Files/d/" + filename + ".png");
                            val.Image.SaveAs(path);
                            string _SQL_file = "insert into file_all ([fk_id],[table_id],[path],[create_by_user_id]) VALUES (" + val.doc_id + ", 3, N'" + path + "', " + val.user_id + ")";
                            using (SqlCommand cmd_update = new SqlCommand(_SQL_file, con))
                            {
                                if (Int32.Parse(cmd_update.ExecuteNonQuery().ToString()) == 1)
                                {
                                    ecm.result = 0;
                                    ecm.code = "OK";
                                }
                                else
                                {
                                    ecm.result = 1;
                                    ecm.code = "error about insert file_all";
                                    return ecm;
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
                }
                // End Upload file


                string _SQL = "update document set " + _SQL_Set + " update_by_user_id = " + val.user_id + " where doc_id = " + val.doc_id;
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
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
                }
                con.Close();
            }
            return ecm;
        }

        // POST CheckList/Profile/DeleteDocument
        [AllowAnonymous]
        [Route("DeleteDocument")]
        public ExecuteModels DelDocument(Document val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "delete from document where doc_id = " + val.doc_id;
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
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
                }
                con.Close();
            }
            return ecm;
        }

        #region DocumentType
        /// <summary>
        /// Get data document type
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetDocumentType")]
        public List<DocumentType> GetDocumentType()
        {
            HomeController hc = new HomeController();
            List<DocumentType> ul = new List<DocumentType>();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = "SELECT * FROM [TT1995_CheckList].[dbo].[document_type]";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        DocumentType dt = new DocumentType();
                        dt.doc_type_id = _Item["doc_type_id"].ToString();
                        dt.doc_type = _Item["doc_type"].ToString();

                        ul.Add(dt);
                    }
                }
                con.Close();
            }
            return ul;
        }
        #endregion
        #endregion

        #region License

        // POST CheckList/Profile/GetLicenseAllHead
        /// <summary>
        /// ข้อมูลหัวรถบรรทุก
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetLicenseAllHead")]
        public List<LicenseAllView> GetLicenseAllHead()
        {
            HomeController hc = new HomeController();
            List<LicenseAllView> ul = new List<LicenseAllView>();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = "SELECT license_id, number_car, license_car, fleet from license WHERE number_car NOT LIKE 'T%'";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        LicenseAllView lav = new LicenseAllView();
                        lav.license_id = _Item["license_id"].ToString();
                        lav.number_car = _Item["number_car"].ToString();
                        lav.license_car = _Item["license_car"].ToString();
                        lav.fleet = _Item["fleet"].ToString();

                        ul.Add(lav);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/GetLicenseAllTail
        /// <summary>
        /// ข้อมูลหางรถบรรทุก
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetLicenseAllTail")]
        public List<LicenseAllView> GetLicenseAllTail()
        {
            HomeController hc = new HomeController();
            List<LicenseAllView> ul = new List<LicenseAllView>();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = "SELECT license_id, number_car, license_car, fleet from license WHERE number_car LIKE 'T%'";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        LicenseAllView lav = new LicenseAllView();
                        lav.license_id = _Item["license_id"].ToString();
                        lav.number_car = _Item["number_car"].ToString();
                        lav.license_car = _Item["license_car"].ToString();
                        lav.fleet = _Item["fleet"].ToString();

                        ul.Add(lav);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/GetExpiredById
        /// <summary>
        /// ข้อมูลภาษี/ประกันภัย
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetExpiredById")]
        public List<ExpiredView> ExpiredById(LicenseModels val)
        {

            HomeController hc = new HomeController();
            List<ExpiredView> ul = new List<ExpiredView>();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = "select _data.license_id,ct.display, _data.expire from " +
                                "( " +
                                    "select li.license_id, tax_expire as expire, '3' as table_id from tax inner join license li on tax.license_id = li.license_id " +
                                    "union " +
                                    "select li.license_id, dpi.end_date as expire, '16' as table_id from domestic_product_insurance dpi inner join license li on dpi.license_id = li.license_id " +
                                    "union " +
                                    "select li.license_id, ai.end_date as expire, '17' as table_id from act_insurance ai inner join license li on ai.license_id = li.license_id " +
                                    "union " +
                                    "select li.license_id, ei.end_date as expire, '18' as table_id from environment_insurance ei inner join license li on ei.license_id = li.license_id " +
                                ") _data inner join config_table ct on ct.table_id = _data.table_id where _data.license_id = '" + val.license_id + "' ";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        ExpiredView ev = new ExpiredView();
                        ev.license_id = _Item["license_id"].ToString();
                        ev.display = _Item["display"].ToString();
                        ev.expire = _Item["expire"].ToString();
                        ul.Add(ev);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/GetExpiredOtherById
        /// <summary>
        /// ข้อมูลใบอนุญาต
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetExpiredOtherById")]
        public List<ExpiredOtherView> ExpiredOtherById(LicenseModels val)
        {

            HomeController hc = new HomeController();
            List<ExpiredOtherView> ul = new List<ExpiredOtherView>();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = "select _data.license_id,ct.display, _data.expire, _data.detail, _data.detail2 from " +
                                "( " +
                                "select lcf.license_id, lcf.expire_date as expire, '36' as table_id, lcf.id_no as detail, lcf.name_factory as detail2 from license_car_factory lcf " +
                                  "union " +
                                "select lv8.license_id, lv8.lv8_expire as expire, '28' as table_id, lv8_number as detail, lv8.ownership as detail2 from license_v8 lv8 " +
                                "union " +
                                "select lmrp.license_id_head as license_id, lmr.lmr_expire, '23' as table_id, lmr.lmr_number as detail, lmr.country_code as detail2 " +
                                "from license_mekong_river_permission as lmrp " +
                                "inner join license_mekong_river as lmr on lmrp.lmr_id = lmr.lmr_id " +
                                "union " +
                                "select lcp.license_id_head as license_id, lc.lc_expire, '22' as table_id, lc.lc_number as detail, lc.country_code as detail2 " +
                                "from license_cambodia as lc " +
                                "inner join license_cambodia_permission as lcp on lcp.lc_id = lc.lc_id " +
                                "union " +
                                "select bip.license_id as license_id, bi.business_expire, '13' as table_id, bi.business_number as detail, bi.business_name as detail2 " +
                                "from business_in as bi " +
                                "inner join business_in_permission as bip on bip.business_id = bi.business_id " +
                                "union " +
                                "select bop.license_id as license_id, bo.business_expire, '20' as table_id, bo.business_number as detail, bo.business_name as detail2 " +
                                "from business_out as bo " +
                                "inner join business_out_permission as bop on bop.business_id = bo.business_id " +
                                ") _data inner join config_table ct on ct.table_id = _data.table_id where _data.license_id = '" + val.license_id + "' ";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        ExpiredOtherView eov = new ExpiredOtherView();
                        eov.license_id = _Item["license_id"].ToString();
                        eov.display = _Item["display"].ToString();
                        eov.expire = _Item["expire"].ToString();
                        eov.detail = _Item["detail"].ToString();
                        eov.detail2 = _Item["detail2"].ToString();
                        ul.Add(eov);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/GetDetailLicense
        /// <summary>
        /// ข้อมูลรายละเอียด
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetDetailLicense")]
        public List<DetailLicense> DetailLicense(LicenseModels val)
        {

            HomeController hc = new HomeController();
            List<DetailLicense> ul = new List<DetailLicense>();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = "select license_id, style_car, weight_car, brand_engine, model_car from license li where li.license_id = '" + val.license_id + "'";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        DetailLicense DL = new DetailLicense();
                        DL.license_id = _Item["license_id"].ToString();
                        DL.style_car = _Item["style_car"].ToString();
                        DL.weight_car = _Item["weight_car"].ToString();
                        DL.brand_engine = _Item["brand_engine"].ToString();
                        DL.model_car = _Item["model_car"].ToString();

                        //_SQL = "SELECT " +
                        //          "[file_id], " +
                        //          "[path_file] as path, " +
                        //          "case position " +
                        //          "when 1 then N'ด้านหน้า' " +
                        //          "when 2 then N'ด้านท้าย' " +
                        //          "when 3 then N'ด้านข้างซ้าย' " +
                        //          "when 4 then N'ด้านข้างขวา' " +
                        //          "when 5 then N'มุมด้านหน้าขวา' " +
                        //          "when 6 then N'มุมด้านหน้าซ้าย' " +
                        //          "when 7 then N'มุมด้านท้ายขวา' " +
                        //          "when 8 then N'มุมด้านท้ายซ้าย' " +
                        //          "end as 'position' " +
                        //      "FROM[TT1995].[dbo].[files_all] " +
                        //            "where table_id = 1 and fk_id = " + val.license_id + " and position<> '' and position is not null";
                        string[] position_pic = { "ด้านหน้า", "ด้านท้าย", "ด้านข้างซ้าย", "ด้านข้างขวา", "มุมด้านหน้าขวา", "มุมด้านหน้าซ้าย", "มุมด้านท้ายขวา", "มุมด้านท้ายซ้าย" };
                        _SQL = "SELECT " +
                            "[p1] as path, " +
                            "N'ด้านหน้า' as position " +
                            "from [TT1995].[dbo].[license] " +
                            "where [p1] is not null and license_id = " + val.license_id;
                        for (int c = 2; c <= 8; c++)
                        {
                            _SQL += " union all SELECT " +
                            "[p" + c + "] as path, " +
                            "N'" + position_pic[c - 1] + "' as position " +
                            "from [TT1995].[dbo].[license] " +
                            "where  [p" + c + "] is not null and license_id = " + val.license_id;
                        }


                        using (SqlCommand cmdTemp = new SqlCommand(_SQL, con))
                        {
                            DataTable _DtTemp = new DataTable();
                            SqlDataAdapter daTemp = new SqlDataAdapter(cmdTemp);
                            daTemp.Fill(_DtTemp);
                            daTemp.Dispose();

                            ListGallery LG = new ListGallery();

                            DL.gallery = new List<ListGallery>();

                            for (int i = 0; i < _DtTemp.Rows.Count; i++)
                            {
                                LG = new ListGallery();
                                //LG.file_id = _DtTemp.Rows[i]["file_id"].ToString();
                                LG.path = _DtTemp.Rows[i]["path"].ToString();
                                LG.position = _DtTemp.Rows[i]["position"].ToString();
                                DL.gallery.Add(LG);
                            }
                        }
                        ul.Add(DL);
                    }
                }
                con.Close();
            }
            return ul;
        }
        #endregion

        #region Customer

        #region Customer
        // POST CheckList/Profile/GetCustomerAll
        /// <summary>
        /// เรียกดูข้อมูลลูกค้า
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetCustomerAll")]
        public List<CustomerAllView> GetCustomerAll()
        {
            HomeController hc = new HomeController();
            List<CustomerAllView> ul = new List<CustomerAllView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "select cus_id, cus_name from customer where enable_status <> 1";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        CustomerAllView cav = new CustomerAllView();
                        cav.cus_id = _Item["cus_id"].ToString();
                        cav.cus_name = _Item["cus_name"].ToString();

                        ul.Add(cav);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/InsertCustomer   
        /// <summary>
        /// เพิ่มลูกค้า
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertCustomer")]
        public ExecuteModels InsertCustomer(CustomerModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "insert into customer (cus_name, enable_status, create_by_user_id) output inserted.cus_id values (N'" + val.cus_name + "', 0, " + val.user_id + ")";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
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
                }
                con.Close();
            }
            return ecm;
        }

        // POST CheckList/Profile/UpdateEquipmentSafety
        /// <summary>
        /// อัพเดทลูกค้า
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("UpdateCustomer")]
        public ExecuteModels UpdateCustomer(CustomerModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "update customer set cus_name = N'" + val.cus_name + "', update_by_user_id = " + val.user_id + " where cus_id = " + val.cus_id;
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
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
                }
                con.Close();
            }
            return ecm;
        }

        // POST CheckList/Profile/DeleteCustomer
        /// <summary>
        /// ลบลูกค้า
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DeleteCustomer")]
        public ExecuteModels DelCustomer(CustomerModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "update customer set enable_status = 1 where cus_id = " + val.cus_id;
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
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
                }
                con.Close();
            }
            return ecm;
        }
        #endregion

        #region Branch
        // POST CheckList/Profile/GetBranchCustomer
        /// <summary>
        /// เรียกดูข้อมูลสาขา
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetBranchCustomerAll")]
        public List<BranchCustomerAllView> BranchCustomerAll(CustomerIdModels val)
        {
            HomeController hc = new HomeController();
            List<BranchCustomerAllView> ul = new List<BranchCustomerAllView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "select * from branch_customer where cus_id = " + val.cus_id;

                if (val.typeGet == 1)
                {
                    _SQL += " and status_approve = 1";
                }

                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        BranchCustomerAllView bcav = new BranchCustomerAllView();
                        bcav.branch_id = _Item["branch_id"].ToString();
                        bcav.branch_name = _Item["branch_name"].ToString();
                        bcav.address = _Item["address"].ToString();
                        bcav.zip_code = _Item["zip_code"].ToString();
                        bcav.province_id = _Item["province_id"].ToString();
                        bcav.cus_id = _Item["cus_id"].ToString();
                        ul.Add(bcav);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/InsertBranchCustomer   
        /// <summary>
        /// เพิ่มสาขา
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertBranchCustomer")]
        public ExecuteModels InsertBranchCustomer(BranchCustomerModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "insert into branch_customer (address,branch_name,zip_code,cus_id,create_by_user_id) output inserted.branch_id " +
                    "values (N'" + val.address + "', N'" + val.branch_name + "', N'" + val.zip_code + "', " + val.cus_id + ", " + val.user_id + ")";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
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
                }
                con.Close();
            }
            return ecm;
        }

        // POST CheckList/Profile/UpdateBranchCustomer
        /// <summary>
        /// อัพเดทสาขา
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("UpdateBranchCustomer")]
        public ExecuteModels UpdateBranchCustomer(BranchCustomerModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            string _SQL_Set = string.Empty;
            string[] Col_Arr = { "address", "zip_code", "province_id", "branch_name" };
            string[] Val_Arr = { val.address, val.zip_code, val.province_id, val.branch_name };
            for (int n = 0; n <= Val_Arr.Length - 1; n++)
            {
                if (Val_Arr[n] != null)
                {
                    _SQL_Set += Col_Arr[n] + " = N'" + Val_Arr[n] + "', ";
                }
            }
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "update branch_customer set " + _SQL_Set + " update_by_user_id = " + val.user_id + " where branch_id = " + val.branch_id;
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
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
                }
                con.Close();
            }
            return ecm;
        }

        // POST CheckList/Profile/DeleteBranchCustomer
        /// <summary>
        /// ลบสาขา
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DeleteBranchCustomer")]
        public ExecuteModels DelBranchCustomer(BranchCustomerIdModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "delete from relation_contact_branch where branch_id = " + val.branch_id;
                SqlCommand cmd = new SqlCommand(_SQL, con);
                cmd.ExecuteNonQuery().ToString();
                try
                {
                    _SQL = "delete from branch_customer where branch_id = " + val.branch_id;
                    cmd = new SqlCommand(_SQL, con);
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

        #region Truck
        // POST CheckList/Profile/GetTrunkAll
        /// <summary>
        /// เรียกดูข้อมูลเส้นทาง
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetTrunkAll")]
        public List<TrunkView> TrunkAll(CustomerIdModels val)
        {
            HomeController hc = new HomeController();
            List<TrunkView> ul = new List<TrunkView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "SELECT * from trunk where cus_id = " + val.cus_id;
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        TrunkView tv = new TrunkView();
                        tv.trunk_id = _Item["trunk_id"].ToString();
                        tv.source = _Item["source"].ToString();
                        tv.destination = _Item["destination"].ToString();
                        tv.station = _Item["station"].ToString();
                        tv.cus_id = _Item["cus_id"].ToString();
                        ul.Add(tv);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/InsertTrunk 
        /// <summary>
        /// เพิ่มเส้นทาง
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertTrunk")]
        public ExecuteModels InsertTrunk(TrunkModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "insert into trunk (source,destination,station,cus_id,create_by_user_id) output inserted.trunk_id " +
                    "values (N'" + val.source + "', N'" + val.destination + "',N'" + val.station + "', " + val.cus_id + ", " + val.user_id + ")";
                SqlCommand cmd = new SqlCommand(_SQL, con);
                try
                {
                    var id_return = Int32.Parse(cmd.ExecuteScalar().ToString());
                    if (id_return >= 1)
                    {

                        ecm.result = 0;
                        ecm.code = "OK";
                        ecm.id_return = id_return.ToString();
                        //_SQL = "insert into relation_trunk_customer (trunk_id, cus_id) values (" + val.trunk_id + ", " + val.cus_id + ")";
                        //if (Int32.Parse(cmd.ExecuteNonQuery().ToString()) == 1)
                        //{
                        //    ecm.result = 0;
                        //    ecm.code = "OK";
                        //    ecm.id_return = id_return.ToString();
                        //}
                        //else
                        //{
                        //    ecm.result = 1;
                        //    ecm.code = _SQL;
                        //}                            
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

        // POST CheckList/Profile/UpdateTrunk
        /// <summary>
        /// อัพเดทเส้นทาง
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("UpdateTrunk")]
        public ExecuteModels UpdateTrunk(TrunkModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            string _SQL_Set = string.Empty;
            string[] Col_Arr = { "source", "destination", "station", "cus_id" };
            string[] Val_Arr = { val.source, val.destination, val.station, val.cus_id };
            for (int n = 0; n <= Val_Arr.Length - 1; n++)
            {
                if (Val_Arr[n] != null)
                {
                    _SQL_Set += Col_Arr[n] + " = N'" + Val_Arr[n] + "', ";
                }
            }
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "update trunk set " + _SQL_Set + " update_by_user_id = " + val.user_id + " where trunk_id = " + val.trunk_id;
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
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
                }
                con.Close();
            }
            return ecm;
        }

        // POST CheckList/Profile/DeleteTrunk
        /// <summary>
        /// ลบเส้นทาง
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DeleteTrunk")]
        public ExecuteModels DelTrunk(TrunkIdModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "delete from trunk where trunk_id = " + val.trunk_id;
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

        #region Contact
        // POST CheckList/Profile/GetContactAll
        /// <summary>
        /// เรียกดูข้อมูลผู้ติดต่อ
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetContactAll")]
        public List<ContactView> ContactAll(BranchCustomerIdModels val)
        {
            HomeController hc = new HomeController();
            List<ContactView> ul = new List<ContactView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "SELECT cc.* from relation_contact_branch as rcb join contact_customer as cc on rcb.contact_id = cc.contact_id where rcb.branch_id = " + val.branch_id;
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        ContactView cv = new ContactView();
                        cv.contact_id = _Item["contact_id"].ToString();
                        cv.name = _Item["name"].ToString();
                        cv.position = _Item["position"].ToString();
                        cv.tel = _Item["tel"].ToString();
                        cv.line = _Item["line"].ToString();
                        cv.email = _Item["email"].ToString();
                        ul.Add(cv);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/InsertContact 
        /// <summary>
        /// เพิ่มผู้ติดต่อ
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertContact")]
        public ExecuteModels InsertContact(ContactModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "insert into contact_customer (name,position,tel,line,email,create_by_user_id) output inserted.contact_id " +
                    "values (N'" + val.name + "', N'" + val.position + "', N'" + val.tel + "', N'" + val.line + "', N'" + val.email + "', " + val.user_id + ")";
                SqlCommand cmd = new SqlCommand(_SQL, con);
                try
                {
                    var id_return = Int32.Parse(cmd.ExecuteScalar().ToString());
                    if (id_return >= 1)
                    {
                        _SQL = "insert into relation_contact_branch (contact_id, branch_id, create_by_user_id) values (" + id_return + ", " + val.branch_id + ", " + val.user_id + ")";
                        cmd = new SqlCommand(_SQL, con);
                        if (Int32.Parse(cmd.ExecuteNonQuery().ToString()) == 1)
                        {
                            ecm.result = 0;
                            ecm.code = "OK";
                            ecm.id_return = id_return.ToString();
                        }
                        else
                        {
                            ecm.result = 1;
                            ecm.code = _SQL;
                        }
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

        // POST CheckList/Profile/UpdateContact
        /// <summary>
        /// อัพเดทผู้ติดต่อ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("UpdateContact")]
        public ExecuteModels UpdateContact(ContactModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            string _SQL_Set = string.Empty;
            string[] Col_Arr = { "name", "position", "tel", "line", "email" };
            string[] Val_Arr = { val.name, val.position, val.tel, val.line, val.email };
            for (int n = 0; n <= Val_Arr.Length - 1; n++)
            {
                if (Val_Arr[n] != null)
                {
                    _SQL_Set += Col_Arr[n] + " = N'" + Val_Arr[n] + "', ";
                }
            }
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "update contact_customer set " + _SQL_Set + " update_by_user_id = " + val.user_id + " where contact_id = " + val.contact_id;
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
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
                }
                con.Close();
            }
            return ecm;
        }

        // POST CheckList/Profile/DeleteContact
        /// <summary>
        /// ลบผู้ติดต่อ
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DeleteContact")]
        public ExecuteModels DelContact(ContactModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "delete from relation_contact_branch where contact_id = " + val.contact_id;
                SqlCommand cmd = new SqlCommand(_SQL, con);
                try
                {
                    if (Int32.Parse(cmd.ExecuteNonQuery().ToString()) == 1)
                    {
                        _SQL = "delete from contact_customer where contact_id = " + val.contact_id;
                        cmd = new SqlCommand(_SQL, con);
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

        #endregion

        #region Driver
        // POST CheckList/Profile/GetDriverAll
        /// <summary>
        /// ข้อมูลพนักงานขับรถ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetDriverAll")]
        public List<DriverAllView> GetDriverAll()
        {
            HomeController hc = new HomeController();
            List<DriverAllView> ul = new List<DriverAllView>();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = "SELECT * from driver_profile";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        DriverAllView dav = new DriverAllView();
                        dav.driver_id = _Item["driver_id"].ToString();
                        dav.driver_name = _Item["driver_name"].ToString();
                        dav.sex = _Item["sex"].ToString();
                        dav.age = _Item["age"].ToString();
                        dav.other = _Item["other"].ToString();
                        dav.path = _Item["path"].ToString();

                        ul.Add(dav);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/GetDriverLicenseById
        /// <summary>
        /// ใบอนุญาติวันหมดอายุ
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetDriverLicenseById")]
        public List<DriverLicenseView> DriverLicenseById(DriverIdModels val)
        {
            HomeController hc = new HomeController();
            List<DriverLicenseView> ul = new List<DriverLicenseView>();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = "select _data.driver_id,ct.display, _data.expire from " +
                    "(select driver_id, expire_date as expire, '31' as table_id from driving_license union " +
                    "select driver_id, expire_date as expire, '33' as table_id from driving_license_oil_transportation union " +
                    "select driver_id, expire_date as expire, '34' as table_id from driving_license_natural_gas_transportation union " +
                    "select driver_id, expire_date as expire, '35' as table_id from driving_license_dangerous_objects_transportation) " +
                    "_data inner join config_table ct on ct.table_id = _data.table_id where _data.driver_id = " + val.driver_id;
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        DriverLicenseView dlv = new DriverLicenseView();
                        dlv.driver_id = _Item["driver_id"].ToString();
                        dlv.display = _Item["display"].ToString();
                        dlv.expire = _Item["expire"].ToString();

                        ul.Add(dlv);
                    }
                }
                con.Close();
            }
            return ul;
        }
        #endregion

        #region Product

        // POST CheckList/Profile/GetProductAll
        [AllowAnonymous]
        [Route("GetProductAll")]
        public List<ProductAllView> ProductAll(CustomerIdModels val)
        {
            HomeController hc = new HomeController();
            List<ProductAllView> ul = new List<ProductAllView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "SELECT * FROM product as p join relation_product_customer as rpb on p.product_id = rpb.product_id where rpb.cus_id = " + val.cus_id;
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        ProductAllView pav = new ProductAllView();
                        pav.product_id = _Item["product_id"].ToString();
                        pav.product_name = _Item["product_name"].ToString();
                        pav.method_style = _Item["method_style"].ToString();
                        pav.method_special = _Item["method_special"].ToString();
                        pav.method_normal = _Item["method_normal"].ToString();
                        pav.method_contain = _Item["method_contain"].ToString();
                        pav.fleet = _Item["fleet"].ToString();
                        ul.Add(pav);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/InsertProduct 
        /// <summary>
        /// เพิ่มสินค้า
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertProduct")]
        public ExecuteModels InsertProduct(ProductModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "insert into product (product_name,fleet,method_style,method_normal,method_contain,method_special,create_by_user_id) output inserted.product_id " +
                    "values (N'" + val.product_name + "', N'" + val.fleet + "', N'" + val.method_style + "', N'" + val.method_normal + "', N'" + val.method_contain + "', N'" + val.method_special + "', " + val.user_id + ")";
                SqlCommand cmd = new SqlCommand(_SQL, con);
                try
                {
                    var id_return = Int32.Parse(cmd.ExecuteScalar().ToString());
                    if (id_return >= 1)
                    {
                        _SQL = "insert into relation_product_customer (product_id, cus_id, create_by_user_id) values (" + id_return + ", " + val.cus_id + ", " + val.user_id + ")";
                        cmd = new SqlCommand(_SQL, con);
                        if (Int32.Parse(cmd.ExecuteNonQuery().ToString()) == 1)
                        {
                            ecm.result = 0;
                            ecm.code = "OK";
                            ecm.id_return = id_return.ToString();
                        }
                        else
                        {
                            ecm.result = 1;
                            ecm.code = _SQL;
                        }
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

        // POST CheckList/Profile/UpdateProduct
        /// <summary>
        /// อัพเดทสินค้า
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("UpdateProduct")]
        public ExecuteModels UpdateProduct(ProductModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            string _SQL_Set = string.Empty;
            string[] Col_Arr = { "product_name", "fleet", "method_style", "method_normal", "method_contain", "method_special" };
            string[] Val_Arr = { val.product_name, val.fleet, val.method_style, val.method_normal, val.method_contain, val.method_special };
            for (int n = 0; n <= Val_Arr.Length - 1; n++)
            {
                if (Val_Arr[n] != null)
                {
                    _SQL_Set += Col_Arr[n] + " = N'" + Val_Arr[n] + "', ";
                }
            }
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "update product set " + _SQL_Set + " update_by_user_id = " + val.user_id + " where product_id = " + val.product_id;
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
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
                }
                con.Close();
            }
            return ecm;
        }

        // POST CheckList/Profile/DeleteProduct
        /// <summary>
        /// ลบสินค้า
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DeleteProduct")]
        public ExecuteModels DelProduct(ProductIdModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = string.Empty;
                try
                {
                    _SQL = "delete from relation_document_product where product_id = " + val.product_id;
                    SqlCommand cmd = new SqlCommand(_SQL, con);
                    cmd.ExecuteNonQuery();
                    _SQL = "delete from relation_driver_product where product_id = " + val.product_id;
                    cmd = new SqlCommand(_SQL, con);
                    cmd.ExecuteNonQuery();
                    _SQL = "delete from relation_equipment_safety_product where product_id = " + val.product_id;
                    cmd = new SqlCommand(_SQL, con);
                    cmd.ExecuteNonQuery();
                    _SQL = "delete from relation_equipment_transport_product where product_id = " + val.product_id;
                    cmd = new SqlCommand(_SQL, con);
                    cmd.ExecuteNonQuery();
                    _SQL = "delete from relation_license_product where product_id = " + val.product_id;
                    cmd = new SqlCommand(_SQL, con);
                    cmd.ExecuteNonQuery();
                    _SQL = "delete from relation_product_customer where product_id = " + val.product_id;
                    cmd = new SqlCommand(_SQL, con);
                    cmd.ExecuteNonQuery();
                    _SQL = "delete from product where product_id = " + val.product_id;
                    cmd = new SqlCommand(_SQL, con);
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
                    ecm.code = ex.Message + " => " + _SQL;
                }
                con.Close();
            }
            return ecm;
        }
        #endregion

        #region Relation Product

        // POST CheckList/Profile/InsertRelDriverProduct 
        /// <summary>
        /// เพิ่มความสัมพันธ์พนักงานขับรถและสินค้า
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertRelDriverProduct")]
        public ExecuteModels InsertRelDriverProduct(RelDriverProductModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "insert into relation_driver_product (driver_id,product_id,score,create_by_user_id) output inserted.rel_d_p_id " +
                    "values ('" + val.driver_id + "', '" + val.product_id + "', '" + val.score + "', " + val.user_id + ")";
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

        // POST CheckList/Profile/InsertRelDocProduct 
        /// <summary>
        /// เพิ่มความสัมพันธ์เอกสารและสินค้า
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertRelDocProduct")]
        public ExecuteModels InsertRelDocProduct(RelDocProductModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "insert into relation_document_product (doc_id,product_id,doc_type_id,create_by_user_id) output inserted.rel_d_p_id " +
                    "values ('" + val.doc_id + "', '" + val.product_id + "', '" + val.doc_type_id + "', " + val.user_id + ")";
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

        // POST CheckList/Profile/InsertRelSafetyProduct 
        /// <summary>
        /// เพิ่มความสัมพันธ์อุปกรณ์ Safety และสินค้า
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertRelSafetyProduct")]
        public ExecuteModels InsertRelSafetyProduct(RelSafetyProductModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "insert into relation_equipment_safety_product (eq_safety_id,product_id,amount,create_by_user_id) output inserted.rel_e_s_p_id " +
                    "values ('" + val.eq_safety_id + "', '" + val.product_id + "', '" + val.amount + "', " + val.user_id + ")";
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

        // POST CheckList/Profile/InsertRelTranProduct 
        /// <summary>
        /// เพิ่มความสัมพันธ์อุปกรณ์ Tran และสินค้า
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertRelTranProduct")]
        public ExecuteModels InsertRelTranProduct(RelTranProductModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "insert into relation_equipment_transport_product (eq_tran_id,product_id,amount,create_by_user_id) output inserted.rel_e_t_p_id " +
                    "values ('" + val.eq_tran_id + "', '" + val.product_id + "', '" + val.amount + "', " + val.user_id + ")";
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

        // POST CheckList/Profile/InsertRelLicenseProduct 
        /// <summary>
        /// เพิ่มความสัมพันธ์รถบรรทุกและสินค้า
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertRelLicenseProduct")]
        public ExecuteModels InsertRelLicenseProduct(RelLicenseProductModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "insert into relation_license_product (license_id,product_id,create_by_user_id) output inserted.rel_l_p_id " +
                    "values ('" + val.license_id + "', '" + val.product_id + "', " + val.user_id + ")";
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

        // POST CheckList/Profile/DeleteRelDriverProduct
        /// <summary>
        /// ลบความสัมพันธ์พนักงานขับรถและสินค้า
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DeleteRelDriverProduct")]
        public ExecuteModels DelRelDriverProduct(IdModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = string.Empty;
                try
                {
                    _SQL = "delete from relation_driver_product where product_id = " + val.id;
                    SqlCommand cmd = new SqlCommand(_SQL, con);
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
                    ecm.code = ex.Message + " => " + _SQL;
                }
                con.Close();
            }
            return ecm;
        }

        // POST CheckList/Profile/DelRelDocProduct
        /// <summary>
        /// ลบความสัมพันธ์เอกสารและสินค้า
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DeleteRelDocProduct")]
        public ExecuteModels DelRelDocProduct(IdModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = string.Empty;
                try
                {
                    _SQL = "delete from relation_document_product where product_id = " + val.id;
                    SqlCommand cmd = new SqlCommand(_SQL, con);
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
                    ecm.code = ex.Message + " => " + _SQL;
                }
                con.Close();
            }
            return ecm;
        }

        // POST CheckList/Profile/DeleteRelSafetyProduct
        /// <summary>
        /// ลบความสัมพันธ์อุปกรณ์ Safety และสินค้า
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DeleteRelSafetyProduct")]
        public ExecuteModels DelRelSafetyProduct(IdModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = string.Empty;
                try
                {
                    _SQL = "delete from relation_equipment_safety_product where product_id = " + val.id;
                    SqlCommand cmd = new SqlCommand(_SQL, con);
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
                    ecm.code = ex.Message + " => " + _SQL;
                }
                con.Close();
            }
            return ecm;
        }

        // POST CheckList/Profile/DeleteRelTranProduct
        /// <summary>
        /// ลบความสัมพันธ์อุปกรณ์ Tran และสินค้า
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DeleteRelTranProduct")]
        public ExecuteModels DelRelTranProduct(IdModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = string.Empty;
                try
                {
                    _SQL = "delete from relation_equipment_transport_product where product_id = " + val.id;
                    SqlCommand cmd = new SqlCommand(_SQL, con);
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
                    ecm.code = ex.Message + " => " + _SQL;
                }
                con.Close();
            }
            return ecm;
        }

        // POST CheckList/Profile/DeleteRelLicenseProduct
        /// <summary>
        /// ลบความสัมพันธ์รถบรรทุกและสินค้า
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DeleteRelLicenseProduct")]
        public ExecuteModels DelRelLicenseProduct(IdModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = string.Empty;
                try
                {
                    _SQL = "delete from relation_license_product where product_id = " + val.id;
                    SqlCommand cmd = new SqlCommand(_SQL, con);
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
                    ecm.code = ex.Message + " => " + _SQL;
                }
                con.Close();
            }
            return ecm;
        }

        // POST CheckList/Profile/GetRelDriverProduct
        /// <summary>
        /// product_id
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetRelDriverProduct")]
        public List<RelDriverProductView> RelDriverProduct(IdModels val)
        {
            HomeController hc = new HomeController();
            List<RelDriverProductView> ul = new List<RelDriverProductView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "select rel.status_approve, d.driver_id, d.driver_name, rel.score, rel.rel_d_p_id as rel_id," +
                    "case when rel.product_id = " + val.id + " then 1 else 0 end as rel_status " +
                    "from [TT1995].[dbo].[driver_profile] as d left join (select * from relation_driver_product where product_id = " + val.id + ") as rel on d.driver_id = rel.driver_id order by rel_status desc, d.driver_name asc";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        RelDriverProductView rel = new RelDriverProductView();
                        rel.rel_id = _Item["rel_id"].ToString();
                        rel.driver_id = _Item["driver_id"].ToString();
                        rel.driver_name = _Item["driver_name"].ToString();
                        rel.status_approve = _Item["status_approve"].ToString();
                        rel.score = _Item["score"].ToString();
                        rel.rel_status = _Item["rel_status"].ToString();
                        ul.Add(rel);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/GetRelDocProduct
        /// <summary>
        /// product_id
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetRelDocProduct")]
        public List<RelDocProductView> RelDocProduct(IdModels val)
        {
            HomeController hc = new HomeController();
            List<RelDocProductView> ul = new List<RelDocProductView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "select d.status_approve, d.doc_id, d.doc_code, d.doc_name, d.remark, d.doc_type_id as type_default, rel.doc_type_id as type_rel, rel.rel_d_p_id as rel_id," +
                    "case when rel.product_id = " + val.id + " then 1 else 0 end as rel_status " +
                    "from document as d left join (select * from relation_document_product where product_id = " + val.id + ") as rel on d.doc_id = rel.doc_id";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        RelDocProductView rel = new RelDocProductView();
                        rel.rel_id = _Item["rel_id"].ToString();
                        rel.rel_status = _Item["rel_status"].ToString();
                        rel.doc_id = _Item["doc_id"].ToString();
                        rel.doc_code = _Item["doc_code"].ToString();
                        rel.doc_name = _Item["doc_name"].ToString();
                        rel.remark = _Item["remark"].ToString();
                        rel.type_default = _Item["type_default"].ToString();
                        rel.status_approve = _Item["status_approve"].ToString();
                        rel.type_rel = _Item["type_rel"].ToString();
                        if (_Item["rel_status"].ToString() == "1")
                        {
                            rel.type_show = _Item["type_rel"].ToString();
                        }
                        else
                        {
                            rel.type_show = _Item["type_default"].ToString();
                        }
                        ul.Add(rel);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/GetRelSafetyProduct
        /// <summary>
        /// product_id
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetRelSafetyProduct")]
        public List<RelSafetyProductView> RelSafetyProduct(IdModels val)
        {
            HomeController hc = new HomeController();
            List<RelSafetyProductView> ul = new List<RelSafetyProductView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "select rel.status_approve, es.eq_safety_id, es.eq_safety_code, es.eq_name, es.eq_path, es.eq_type_id, es.property, es.suggestion, es.style, rel.amount, rel.rel_e_s_p_id as rel_id," +
                    "case when rel.product_id = " + val.id + " then 1 else 0 end as rel_status " +
                    "from equipment_safety as es left join (select * from relation_equipment_safety_product where product_id = " + val.id + ") as rel on es.eq_safety_id = rel.eq_safety_id";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        RelSafetyProductView rel = new RelSafetyProductView();
                        rel.rel_id = _Item["rel_id"].ToString();
                        rel.eq_safety_id = _Item["eq_safety_id"].ToString();
                        rel.eq_safety_code = _Item["eq_safety_code"].ToString();
                        rel.eq_name = _Item["eq_name"].ToString();
                        rel.eq_path = _Item["eq_path"].ToString();
                        rel.eq_type_id = _Item["eq_type_id"].ToString();
                        rel.property = _Item["property"].ToString();
                        rel.suggestion = _Item["suggestion"].ToString();
                        rel.style = _Item["style"].ToString();
                        rel.status_approve = _Item["status_approve"].ToString();
                        rel.amount = _Item["amount"].ToString();
                        rel.rel_status = _Item["rel_status"].ToString();
                        ul.Add(rel);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/GetRelTranProduct
        /// <summary>
        /// product_id
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetRelTranProduct")]
        public List<RelTranProductView> RelTranProduct(IdModels val)
        {
            HomeController hc = new HomeController();
            List<RelTranProductView> ul = new List<RelTranProductView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "select rel.status_approve, es.eq_tran_id, es.eq_tran_code, es.eq_name, es.eq_path, es.eq_type_id, es.property, es.suggestion, es.style, rel.amount, rel.rel_e_t_p_id as rel_id," +
                    "case when rel.product_id = " + val.id + " then 1 else 0 end as rel_status " +
                    "from equipment_transport as es left join (select * from relation_equipment_transport_product where product_id = " + val.id + ") as rel on es.eq_tran_id = rel.eq_tran_id";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        RelTranProductView rel = new RelTranProductView();
                        rel.rel_id = _Item["rel_id"].ToString();
                        rel.eq_tran_id = _Item["eq_tran_id"].ToString();
                        rel.eq_tran_code = _Item["eq_tran_code"].ToString();
                        rel.eq_name = _Item["eq_name"].ToString();
                        rel.eq_path = _Item["eq_path"].ToString();
                        rel.eq_type_id = _Item["eq_type_id"].ToString();
                        rel.property = _Item["property"].ToString();
                        rel.suggestion = _Item["suggestion"].ToString();
                        rel.style = _Item["style"].ToString();
                        rel.status_approve = _Item["status_approve"].ToString();
                        rel.amount = _Item["amount"].ToString();
                        rel.rel_status = _Item["rel_status"].ToString();
                        ul.Add(rel);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/GetRelTranProduct
        /// <summary>
        /// product_id
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetRelLicenseProduct")]
        public List<RelLicenseProductView> RelLicenseProduct(IdModels val)
        {
            HomeController hc = new HomeController();
            List<RelLicenseProductView> ul = new List<RelLicenseProductView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "select rel.status_approve, l.fleet, l.license_id, l.license_car, l.number_car, rel.rel_l_p_id as rel_id," +
                    "case when rel.product_id = " + val.id + " then 1 else 0 end as rel_status " +
                    "from [TT1995].[dbo].[license] as l left join (select * from relation_license_product where product_id = " + val.id + ") as rel on l.license_id = rel.license_id order by rel_status desc, l.number_car asc";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        RelLicenseProductView rel = new RelLicenseProductView();
                        rel.rel_id = _Item["rel_id"].ToString();
                        rel.license_id = _Item["license_id"].ToString();
                        rel.license_car = _Item["license_car"].ToString();
                        rel.number_car = _Item["number_car"].ToString();
                        rel.rel_status = _Item["rel_status"].ToString();
                        rel.fleet = _Item["fleet"].ToString();
                        rel.status_approve = _Item["status_approve"].ToString();
                        ul.Add(rel);
                    }
                }
                con.Close();
            }
            return ul;
        }

        #endregion

        #region Approve
        // POST CheckList/Profile/UpdateApprove
        /// <summary>
        /// Update Approve
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("UpdateApprove")]
        public ExecuteModels UpdateApprove(ApproveModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            try
            {
                HomeController hc = new HomeController();
                using (SqlConnection con = hc.ConnectDatabase())
                {

                    string _SQL = "update " + val.nametable + " set status_approve = 1, update_by_user_id = " + val.user_id + " where " + val.nameid + " = " + val.id;
                    using (SqlCommand cmd = new SqlCommand(_SQL, con))
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
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ecm.result = 1;
                ecm.code = ex.Message;
            }
            return ecm;
        }

        [AllowAnonymous]
        [Route("GetApproveRelContactBranch")]
        public List<RelContactBranch> GetApproveRelContactBranch()
        {
            HomeController hc = new HomeController();
            List<RelContactBranch> ul = new List<RelContactBranch>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "SELECT rel.rel_c_b_id, a.name, a.email, a.line, a.position, a.tel, b.[address], b.branch_name " +
                    "FROM relation_contact_branch as rel join contact_customer as a on rel.contact_id = a.contact_id " +
                    "join branch_customer as b on rel.branch_id = b.branch_id where rel.status_approve is null";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        RelContactBranch rel = new RelContactBranch();
                        rel.rel_id = _Item["rel_c_b_id"].ToString();
                        rel.contact_name = _Item["name"].ToString();
                        rel.branch_name = _Item["branch_name"].ToString();
                        rel.address = _Item["address"].ToString();
                        rel.email = _Item["email"].ToString();
                        rel.line = _Item["line"].ToString();
                        rel.tel = _Item["tel"].ToString();
                        ul.Add(rel);
                    }
                }
                con.Close();
            }
            return ul;
        }

        [AllowAnonymous]
        [Route("GetApproveRelDocumentProduct")]
        public List<RelDocumentProduct> GetApproveRelDocumentProduct()
        {
            HomeController hc = new HomeController();
            List<RelDocumentProduct> ul = new List<RelDocumentProduct>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "SELECT c.cus_name, rel.rel_d_p_id, d.doc_code,d.doc_name,d.doc_path,d.remark,p.product_name,p.fleet,p.method_style,p.method_normal,p.method_contain,p.method_special,p.product_path,dt.doc_type " +
                    "FROM relation_document_product as rel join document_type as dt on rel.doc_type_id = dt.doc_type_id " +
                    "join relation_product_customer as rpc on rel.product_id = rpc.product_id " +
                    "join customer as c on c.cus_id = rpc.cus_id " +
                    "join document as d on rel.doc_id = d.doc_id join product as p on rel.product_id = p.product_id where rel.status_approve is null";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        RelDocumentProduct rel = new RelDocumentProduct();
                        rel.rel_id = _Item["rel_d_p_id"].ToString();
                        rel.doc_type = _Item["doc_type"].ToString();
                        rel.doc_code = _Item["doc_code"].ToString();
                        rel.doc_name = _Item["doc_name"].ToString();
                        rel.doc_path = _Item["doc_path"].ToString();
                        rel.remark = _Item["remark"].ToString();
                        rel.product_name = _Item["product_name"].ToString();
                        rel.fleet = _Item["fleet"].ToString();
                        rel.method_style = _Item["method_style"].ToString();
                        rel.method_normal = _Item["method_normal"].ToString();
                        rel.method_contain = _Item["method_contain"].ToString();
                        rel.method_special = _Item["method_special"].ToString();
                        rel.product_path = _Item["product_path"].ToString();
                        rel.cus_name = _Item["cus_name"].ToString();
                        ul.Add(rel);
                    }
                }
                con.Close();
            }
            return ul;
        }

        [AllowAnonymous]
        [Route("GetApproveRelDriverProduct")]
        public List<RelDriverProduct> GetApproveRelDriverProduct()
        {
            HomeController hc = new HomeController();
            List<RelDriverProduct> ul = new List<RelDriverProduct>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "SELECT rel.rel_d_p_id, d.driver_name,p.product_name,p.fleet,p.method_style,p.method_normal,p.method_contain,p.method_special,p.product_path,rel.score " +
                    "FROM relation_driver_product as rel join [TT1995].[dbo].[driver_profile] as d on rel.driver_id = d.driver_id join product as p on rel.product_id = p.product_id " +
                    "where rel.status_approve is null";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        RelDriverProduct rel = new RelDriverProduct();
                        rel.rel_id = _Item["rel_d_p_id"].ToString();
                        rel.score = _Item["score"].ToString();
                        rel.driver_name = _Item["driver_name"].ToString();
                        rel.product_name = _Item["product_name"].ToString();
                        rel.fleet = _Item["fleet"].ToString();
                        rel.method_style = _Item["method_style"].ToString();
                        rel.method_normal = _Item["method_normal"].ToString();
                        rel.method_contain = _Item["method_contain"].ToString();
                        rel.method_special = _Item["method_special"].ToString();
                        rel.product_path = _Item["product_path"].ToString();
                        ul.Add(rel);
                    }
                }
                con.Close();
            }
            return ul;
        }

        [AllowAnonymous]
        [Route("GetApproveRelSafetyProduct")]
        public List<RelSafetyProduct> GetApproveRelSafetyProduct()
        {
            HomeController hc = new HomeController();
            List<RelSafetyProduct> ul = new List<RelSafetyProduct>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "SELECT rel.rel_e_s_p_id, d.eq_safety_code,d.eq_name,d.style,d.property,d.suggestion,et.eq_type,d.eq_path,p.product_name,p.fleet,p.method_style,p.method_normal,p.method_contain,p.method_special,p.product_path,rel.amount " +
                    "FROM relation_equipment_safety_product as rel join equipment_safety as d on rel.eq_safety_id = d.eq_safety_id join product as p on rel.product_id = p.product_id " +
                    "join equipment_type as et on d.eq_type_id = et.eq_type_id where rel.status_approve is null";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        RelSafetyProduct rel = new RelSafetyProduct();
                        rel.rel_id = _Item["rel_e_s_p_id"].ToString();
                        rel.eq_safety_code = _Item["eq_safety_code"].ToString();
                        rel.eq_name = _Item["eq_name"].ToString();
                        rel.style = _Item["style"].ToString();
                        rel.property = _Item["property"].ToString();
                        rel.suggestion = _Item["suggestion"].ToString();
                        rel.eq_type = _Item["eq_type"].ToString();
                        rel.eq_path = _Item["eq_path"].ToString();
                        rel.product_name = _Item["product_name"].ToString();
                        rel.fleet = _Item["fleet"].ToString();
                        rel.method_style = _Item["method_style"].ToString();
                        rel.method_normal = _Item["method_normal"].ToString();
                        rel.method_contain = _Item["method_contain"].ToString();
                        rel.method_special = _Item["method_special"].ToString();
                        rel.product_path = _Item["product_path"].ToString();
                        ul.Add(rel);
                    }
                }
                con.Close();
            }
            return ul;
        }

        [AllowAnonymous]
        [Route("GetApproveRelTranProduct")]
        public List<RelTranProduct> GetApproveRelTranProduct()
        {
            HomeController hc = new HomeController();
            List<RelTranProduct> ul = new List<RelTranProduct>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "SELECT rel.rel_e_t_p_id, d.eq_tran_code,d.eq_name,d.style,d.property,d.suggestion,et.eq_type,d.eq_path,p.product_name,p.fleet,p.method_style,p.method_normal,p.method_contain,p.method_special,p.product_path,rel.amount " +
                    "FROM relation_equipment_transport_product as rel join equipment_transport as d on rel.eq_tran_id = d.eq_tran_id join product as p on rel.product_id = p.product_id " +
                    "join equipment_type as et on d.eq_type_id = et.eq_type_id where rel.status_approve is null";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        RelTranProduct rel = new RelTranProduct();
                        rel.rel_id = _Item["rel_e_t_p_id"].ToString();
                        rel.eq_tran_code = _Item["eq_tran_code"].ToString();
                        rel.eq_name = _Item["eq_name"].ToString();
                        rel.style = _Item["style"].ToString();
                        rel.property = _Item["property"].ToString();
                        rel.suggestion = _Item["suggestion"].ToString();
                        rel.eq_type = _Item["eq_type"].ToString();
                        rel.eq_path = _Item["eq_path"].ToString();
                        rel.product_name = _Item["product_name"].ToString();
                        rel.fleet = _Item["fleet"].ToString();
                        rel.method_style = _Item["method_style"].ToString();
                        rel.method_normal = _Item["method_normal"].ToString();
                        rel.method_contain = _Item["method_contain"].ToString();
                        rel.method_special = _Item["method_special"].ToString();
                        rel.product_path = _Item["product_path"].ToString();
                        ul.Add(rel);
                    }
                }
                con.Close();
            }
            return ul;
        }

        [AllowAnonymous]
        [Route("GetApproveRelLicenseProduct")]
        public List<RelLicenseProduct> GetApproveRelLicenseProduct()
        {
            HomeController hc = new HomeController();
            List<RelLicenseProduct> ul = new List<RelLicenseProduct>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "SELECT rel.rel_l_p_id, d.license_car,d.number_car,p.product_name,p.fleet,p.method_style,p.method_normal,p.method_contain,p.method_special,p.product_path " +
                    "FROM relation_license_product as rel join [TT1995].[dbo].license as d on rel.license_id = d.license_id join product as p on rel.product_id = p.product_id " +
                    "where rel.status_approve is null order by d.number_car asc";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        RelLicenseProduct rel = new RelLicenseProduct();
                        rel.rel_id = _Item["rel_l_p_id"].ToString();
                        rel.number_car = _Item["number_car"].ToString();
                        rel.license_car = _Item["license_car"].ToString();
                        rel.product_name = _Item["product_name"].ToString();
                        rel.fleet = _Item["fleet"].ToString();
                        rel.method_style = _Item["method_style"].ToString();
                        rel.method_normal = _Item["method_normal"].ToString();
                        rel.method_contain = _Item["method_contain"].ToString();
                        rel.method_special = _Item["method_special"].ToString();
                        rel.product_path = _Item["product_path"].ToString();
                        ul.Add(rel);
                    }
                }
                con.Close();
            }
            return ul;
        }

        [AllowAnonymous]
        [Route("GetApproveRelCustomerProduct")]
        public List<RelCustomerProduct> GetApproveRelCustomerProduct()
        {
            HomeController hc = new HomeController();
            List<RelCustomerProduct> ul = new List<RelCustomerProduct>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "SELECT rel.rel_p_c_id, d.cus_name,p.product_name,p.fleet,p.method_style,p.method_normal,p.method_contain,p.method_special,p.product_path " +
                    "FROM relation_product_customer as rel join customer as d on rel.cus_id = d.cus_id join product as p on rel.product_id = p.product_id " +
                    "where rel.status_approve is null order by d.cus_name asc";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        RelCustomerProduct rel = new RelCustomerProduct();
                        rel.rel_id = _Item["rel_p_c_id"].ToString();
                        rel.cus_name = _Item["cus_name"].ToString();
                        rel.product_name = _Item["product_name"].ToString();
                        rel.fleet = _Item["fleet"].ToString();
                        rel.method_style = _Item["method_style"].ToString();
                        rel.method_normal = _Item["method_normal"].ToString();
                        rel.method_contain = _Item["method_contain"].ToString();
                        rel.method_special = _Item["method_special"].ToString();
                        rel.product_path = _Item["product_path"].ToString();
                        ul.Add(rel);
                    }
                }
                con.Close();
            }
            return ul;
        }

        #endregion

        #region File All
        [AllowAnonymous]
        [Route("DeleteFile")]
        public ExecuteModels DelFile(FileAllModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "select * from file_all where seq = " + val.seq;
                SqlCommand cmd = new SqlCommand(_SQL, con);
                try
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        File.Delete(_Item["path"].ToString());
                        _SQL = "delete from file_all where seq = " + val.seq;
                        cmd = new SqlCommand(_SQL, con);
                        if (Int32.Parse(cmd.ExecuteNonQuery().ToString()) >= 1)
                        {
                            ecm.result = 0;
                            ecm.code = "OK";
                        }
                        else
                        {
                            ecm.result = 1;
                            ecm.code = _SQL;
                            return ecm;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ecm.result = 1;
                    ecm.code = ex.Message;
                    return ecm;
                }
                con.Close();
            }
            return ecm;
        }
        #endregion
    }
}