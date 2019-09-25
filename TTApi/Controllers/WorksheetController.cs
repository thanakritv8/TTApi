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

    [Authorize]
    [RoutePrefix("CheckList/Worksheet")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WorksheetController : ApiController
    {
        #region CreateWorksheet

        // POST CheckList/Profile/InsertWorksheet
        /// <summary>
        /// สร้างใบงาน
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertWorksheet")]
        public ExecuteModels InsertWorksheet(WorksheetModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();


            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _no = "";
                string SQL_COUNT = "SELECT (COUNT(*) + 1 ) AS NO  FROM [TT1995_CheckList].[dbo].[transport] WHERE create_date BETWEEN '" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59' ";
                using (SqlCommand cmd = new SqlCommand(SQL_COUNT, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        _no = _Item["NO"].ToString();
                    }
                }
                _no = int.Parse(_no).ToString("000");
                var tran_code = "CHECK" + DateTime.Now.ToString("yyMMdd") + _no;
                string _SQL = "INSERT INTO [dbo].[transport] (tran_code,number_po  ,cus_id  ,branch_id  ,contact_id ,product_id  ,trunk_id " +
                    ",driver_id_1  ,driver_id_2 ,driver_id_3  ,license_id_head  ,license_id_tail ,remark ,tran_status_id ,create_by_user_id ,sheet_name ,cont1,cont2 ,driver_id_4 " +
                    " , bugget_tran  , date_work , date_start , date_end , value_order , type_tran  , size_cont1  , size_cont2   , condition_tran , special_order , tank_number ,  style_tank ,  weight_tank) " +
     " output inserted.tran_id VALUES (N'" + tran_code + "', N'" + val.number_po + "', '" + val.cus_id + "', '" + val.branch_id + "', '" + val.contact_id + "', '" + val.product_id + "'" +
           ", '" + val.trunk_id + "' ,'" + val.driver_id_1 + "' , '" + val.driver_id_2 + "', '" + val.driver_id_3 + "', '" + val.license_id_head + "' , '" + val.license_id_tail + "'" +
           ", N'" + val.remark + "'  , 1  , " + val.create_by_user_id + "  , '" + val.sheet_name + "' , '" + val.cont1 + "', '" + val.cont2 + "', '" + val.driver_id_4 + "'" +
           ",'" + val.bugget_tran + "',  '" + val.date_work + "',  '" + val.date_start + "',  '" + val.date_end + "',  '" + val.value_order + "',  '" + val.type_tran + "',  '" + val.size_cont1 + "',  '" + val.size_cont2 + "',  '" + val.condition_tran + "',  '" + val.special_order + "',  '" + val.tank_number + "',   '" + val.style_tank + "',   '" + val.weight_tank + "')";
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

        // POST CheckList/Profile/UpdateWorksheet
        /// <summary>
        /// อัพเดทใบงาน
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("UpdateWorksheet")]
        public ExecuteModels UpdateWorksheet(WorksheetModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            string _SQL_Set = string.Empty;
            string[] Col_Arr = { "tran_code", "number_po", "cus_id", "branch_id", "contact_id", "product_id", "trunk_id", "driver_id_1", "driver_id_2", "driver_id_3", "license_id_head", "license_id_tail", "remark", "tran_status_id", "update_by_user_id", "sheet_name", "cont1", "cont2", "driver_id_4", "bugget_tran", "date_work", "date_start", "date_end", "value_order", "type_tran", "size_cont1", "size_cont2", "condition_tran", "special_order", "tank_number", "style_tank", "weight_tank" };
            string[] Val_Arr = { val.tran_code, val.number_po, val.cus_id, val.branch_id, val.contact_id, val.product_id, val.trunk_id, val.driver_id_1, val.driver_id_2, val.driver_id_3, val.license_id_head, val.license_id_tail, val.remark, val.tran_status_id, val.update_by_user_id, val.sheet_name, val.cont1, val.cont2, val.driver_id_4, val.bugget_tran, val.date_work, val.date_start, val.date_end, val.value_order, val.type_tran, val.size_cont1, val.size_cont2, val.condition_tran, val.special_order, val.tank_number, val.style_tank, val.weight_tank };
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
                string _SQL = "UPDATE  transport set " + _SQL_Set + " update_by_user_id = 1  where tran_id = " + val.tran_id;

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

        // POST CheckList/Profile/DeleteWorksheet
        /// <summary>
        /// ลบWorksheet
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DeleteWorksheet")]
        public ExecuteModels DelWorksheet(WorksheetModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "delete from transport where tran_id = " + val.tran_id;
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

        // Get CheckList/Profile/GetWorksheetAll
        /// <summary>
        /// เรียกดูข้อมูลWorksheetAll
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetWorksheetAll")]
        public List<WorksheetAllView> GetWorksheetAll()
        {
            HomeController hc = new HomeController();
            List<WorksheetAllView> ul = new List<WorksheetAllView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "select * from transport Order by tran_id desc";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        WorksheetAllView wsa = new WorksheetAllView();
                        wsa.tran_id = _Item["tran_id"].ToString();
                        wsa.tran_code = _Item["tran_code"].ToString();
                        wsa.number_po = _Item["number_po"].ToString();
                        wsa.cus_id = _Item["cus_id"].ToString();
                        wsa.branch_id = _Item["branch_id"].ToString();
                        wsa.contact_id = _Item["contact_id"].ToString();
                        wsa.product_id = _Item["product_id"].ToString();
                        wsa.trunk_id = _Item["trunk_id"].ToString();
                        wsa.driver_id_1 = _Item["driver_id_1"].ToString();
                        wsa.driver_id_2 = _Item["driver_id_2"].ToString();
                        wsa.driver_id_3 = _Item["driver_id_3"].ToString();
                        wsa.driver_id_4 = _Item["driver_id_4"].ToString();
                        wsa.license_id_head = _Item["license_id_head"].ToString();
                        wsa.license_id_tail = _Item["license_id_tail"].ToString();
                        wsa.remark = _Item["remark"].ToString();
                        wsa.tran_status_id = _Item["tran_status_id"].ToString();
                        wsa.sheet_name = _Item["sheet_name"].ToString();
                        wsa.cont1 = _Item["cont1"].ToString();
                        wsa.cont2 = _Item["cont2"].ToString();
                        wsa.create_date = _Item["create_date"].ToString();
                        wsa.create_by_user_id = _Item["create_by_user_id"].ToString();
                        wsa.update_date = _Item["update_date"].ToString();
                        wsa.update_by_user_id = _Item["update_by_user_id"].ToString();
                        wsa.bugget_tran = _Item["bugget_tran"].ToString();
                        wsa.date_work = _Item["date_work"].ToString();
                        wsa.date_start = _Item["date_start"].ToString();
                        wsa.date_end = _Item["date_end"].ToString();
                        wsa.value_order = _Item["value_order"].ToString();
                        wsa.type_tran = _Item["type_tran"].ToString();
                        wsa.size_cont1 = _Item["size_cont1"].ToString();
                        wsa.size_cont2 = _Item["size_cont2"].ToString();
                        wsa.condition_tran = _Item["condition_tran"].ToString();
                        wsa.special_order = _Item["special_order"].ToString();
                        wsa.tank_number = _Item["tank_number"].ToString();
                        wsa.style_tank = _Item["style_tank"].ToString();
                        wsa.type_tran = _Item["weight_tank"].ToString();
                        ul.Add(wsa);
                    }
                }
                con.Close();
            }
            return ul;
        }

        // POST CheckList/Profile/GetWorksheet
        /// <summary>
        /// เรียกดูข้อมูลWorksheet
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetWorksheet")]
        public List<WorksheetView> Worksheet(WorksheetIdModels val)
        {
            HomeController hc = new HomeController();
            List<WorksheetView> ul = new List<WorksheetView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = " EXEC [sp_WorkSheet] '" + val.tran_id + "' ";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        WorksheetView ws = new WorksheetView();
                        ws.kind = _Item["kind"].ToString();
                        ws.tran_code = _Item["tran_code"].ToString();
                        ws.number_po = _Item["number_po"].ToString();
                        ws.cus_name = _Item["cus_name"].ToString();
                        ws.branch_name = _Item["branch_name"].ToString();
                        ws.address = _Item["address"].ToString();
                        ws.contact_name = _Item["contact_name"].ToString();
                        ws.position = _Item["position"].ToString();
                        ws.tel = _Item["tel"].ToString();
                        ws.source = _Item["source"].ToString();
                        ws.destination = _Item["destination"].ToString();
                        ws.station = _Item["station"].ToString();
                        ws.driver1 = _Item["driver1"].ToString();
                        ws.driver1_license_start = _Item["driver1_license_start"].ToString();
                        ws.driver1_license_expire = _Item["driver1_license_expire"].ToString();
                        ws.driver2 = _Item["driver2"].ToString();
                        ws.driver2_license_start = _Item["driver2_license_start"].ToString();
                        ws.driver2_license_expire = _Item["driver2_license_expire"].ToString();
                        ws.driver3 = _Item["driver3"].ToString();
                        ws.driver3_license_start = _Item["driver3_license_start"].ToString();
                        ws.driver3_license_expire = _Item["driver3_license_expire"].ToString();
                        ws.driver4 = _Item["driver4"].ToString();
                        ws.driver4_license_start = _Item["driver4_license_start"].ToString();
                        ws.driver4_license_expire = _Item["driver4_license_expire"].ToString();
                        ws.license_head = _Item["license_head"].ToString();
                        ws.style_car_head = _Item["style_car_head"].ToString();
                        ws.license_tail = _Item["license_tail"].ToString();
                        ws.style_car_tail = _Item["style_car_tail"].ToString();
                        ws.doc_code = _Item["doc_code"].ToString();
                        ws.doc_name = _Item["doc_name"].ToString();
                        ws.doc_type_id = _Item["doc_type_id"].ToString();
                        ws.eq_code = _Item["eq_code"].ToString();
                        ws.eq_name = _Item["eq_name"].ToString();
                        ws.eq_amount = _Item["eq_amount"].ToString();
                        ws.product_name = _Item["product_name"].ToString();
                        ws.fleet = _Item["fleet"].ToString();
                        ws.method_contain = _Item["method_contain"].ToString();
                        ws.method_normal = _Item["method_normal"].ToString();
                        ws.method_special = _Item["method_special"].ToString();
                        ws.method_style = _Item["method_style"].ToString();
                        ws.cont1 = _Item["cont1"].ToString();
                        ws.cont2 = _Item["cont2"].ToString();
                        ws.remark = _Item["remark"].ToString();
                        ws.update_by_user_id = _Item["update_by_user_id"].ToString();
                        ws.update_date = _Item["update_date"].ToString();
                        ws.Approve_By = _Item["Approve_By"].ToString();
                        ws.bugget_tran = _Item["bugget_tran"].ToString();
                        ws.date_work = _Item["date_work"].ToString();
                        ws.date_start = _Item["date_start"].ToString();
                        ws.date_end = _Item["date_end"].ToString();
                        ws.value_order = _Item["value_order"].ToString();
                        ws.type_tran = _Item["type_tran"].ToString();
                        ws.size_cont1 = _Item["size_cont1"].ToString();
                        ws.size_cont2 = _Item["size_cont2"].ToString();
                        ws.condition_tran = _Item["condition_tran"].ToString();
                        ws.special_order = _Item["special_order"].ToString();
                        ws.tank_number = _Item["tank_number"].ToString();
                        ws.style_tank = _Item["style_tank"].ToString();
                        ws.weight_tank = _Item["weight_tank"].ToString();
                        ul.Add(ws);
                    }
                }
                con.Close();
            }
            return ul;
        }

        #endregion

        #region TempWorksheet

        // POST CheckList/Profile/InsertTempWorksheet
        /// <summary>
        /// เพิ่ม temp worksheet
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertTempWorksheet")]
        public ExecuteModels InsertTempWorksheet(TempWorksheetModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "INSERT INTO transport_temp (tran_code,number_po  ,cus_id  ,branch_id  ,contact_id ,product_id  ,trunk_id " +
                    ",driver_id_1  ,driver_id_2 ,driver_id_3  ,license_id_head  ,license_id_tail ,remark ,tran_status_id ,create_by_user_id ,sheet_name ,cont1,cont2   ) " +
     " output inserted.tran_id VALUES (N'" + val.tran_code + "', N'" + val.number_po + "', '" + val.cus_id + "', '" + val.branch_id + "', '" + val.contact_id + "', '" + val.product_id + "'" +
           ", '" + val.trunk_id + "' ,'" + val.driver_id_1 + "' , '" + val.driver_id_2 + "', '" + val.driver_id_3 + "', '" + val.license_id_head + "' , '" + val.license_id_tail + "'" +
           ", N'" + val.remark + "'  , '" + val.tran_status_id + "'  , 1  , '" + val.sheet_name + "' , '" + val.cont1 + "', '" + val.cont2 + "')";
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

        // POST CheckList/Profile/UpdateTempWorksheet
        /// <summary>
        /// อัพเดทTempWorksheet
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("UpdateTempWorksheet")]
        public ExecuteModels UpdateTempWorksheet(TempWorksheetModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            string _SQL_Set = string.Empty;
            string[] Col_Arr = { "tran_code", "number_po", "cus_id", "branch_id", "contact_id", "product_id", "trunk_id", "driver_id_1", "driver_id_2", "driver_id_3", "license_id_head", "license_id_tail", "remark", "tran_status_id", "sheet_name", "cont1", "cont2" };
            string[] Val_Arr = { val.tran_code, val.number_po, val.cus_id, val.branch_id, val.contact_id, val.product_id, val.trunk_id, val.driver_id_1, val.driver_id_2, val.driver_id_3, val.license_id_head, val.license_id_tail, val.remark, val.tran_status_id, val.sheet_name, val.cont1, val.cont2 };
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
                string _SQL = "UPDATE  transport_temp set " + _SQL_Set + " update_by_user_id = 1 where tran_id = " + val.tran_id;

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

        // POST CheckList/Profile/DeleteTempWorksheet
        /// <summary>
        /// ลบTempWorksheet
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DeleteTempWorksheet")]
        public ExecuteModels DelTempWorksheet(TempWorksheetModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "delete from transport_temp where tran_id = " + val.tran_id;
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

        // POST CheckList/Profile/GetTempWorksheetAll
        /// <summary>
        /// เรียกดูข้อมูลTempWorksheet
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetTempWorksheetAll")]
        public List<TempWorksheetAllView> GetTempWorksheetAll()
        {
            HomeController hc = new HomeController();
            List<TempWorksheetAllView> ul = new List<TempWorksheetAllView>();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "select * from transport_temp";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        TempWorksheetAllView wsa = new TempWorksheetAllView();
                        wsa.tran_id = _Item["tran_id"].ToString();
                        wsa.tran_code = _Item["tran_code"].ToString();
                        wsa.number_po = _Item["number_po"].ToString();
                        wsa.cus_id = _Item["cus_id"].ToString();
                        wsa.branch_id = _Item["branch_id"].ToString();
                        wsa.contact_id = _Item["contact_id"].ToString();
                        wsa.product_id = _Item["product_id"].ToString();
                        wsa.trunk_id = _Item["trunk_id"].ToString();
                        wsa.driver_id_1 = _Item["driver_id_1"].ToString();
                        wsa.driver_id_2 = _Item["driver_id_2"].ToString();
                        wsa.driver_id_3 = _Item["driver_id_3"].ToString();
                        wsa.license_id_head = _Item["license_id_head"].ToString();
                        wsa.license_id_tail = _Item["license_id_tail"].ToString();
                        wsa.remark = _Item["remark"].ToString();
                        wsa.tran_status_id = _Item["tran_status_id"].ToString();
                        wsa.sheet_name = _Item["sheet_name"].ToString();
                        wsa.cont1 = _Item["cont1"].ToString();
                        wsa.cont2 = _Item["cont2"].ToString();
                        wsa.create_date = _Item["create_date"].ToString();
                        wsa.create_by_user_id = _Item["create_by_user_id"].ToString();
                        wsa.update_date = _Item["update_date"].ToString();
                        wsa.update_by_user_id = _Item["update_by_user_id"].ToString();
                        ul.Add(wsa);
                    }
                }
                con.Close();
            }
            return ul;
        }

        #endregion

        //#region UpdateStatusWorksheet

        //// POST CheckList/Profile/UpdateStatusWorksheet
        ///// <summary>
        ///// UpdateStatusWorksheet
        ///// </summary>
        ///// <returns></returns>
        //[AllowAnonymous]
        //[Route("UpdateStatusWorksheet")]
        //public ExecuteModels UpdateStatusWorksheet(StatusWorksheetModels val)
        //{
        //    ExecuteModels ecm = new ExecuteModels();
        //    HomeController hc = new HomeController();
        //    using (SqlConnection con = hc.ConnectDatabase())
        //    {
        //        string _SQL = "UPDATE  transport SET  tran_status_id = " + val.tran_status_id + ", update_by_user_id = 1  where tran_id = " + val.tran_id;

        //        using (SqlCommand cmd = new SqlCommand(_SQL, con))
        //        {
        //            try
        //            {
        //                if (Int32.Parse(cmd.ExecuteNonQuery().ToString()) == 1)
        //                {
        //                    ecm.result = 0;
        //                    ecm.code = "OK";
        //                }
        //                else
        //                {
        //                    ecm.result = 1;
        //                    ecm.code = _SQL;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                ecm.result = 1;
        //                ecm.code = ex.Message;
        //            }
        //        }
        //        con.Close();
        //    }
        //    return ecm;
        //}


        //#endregion
    }
}