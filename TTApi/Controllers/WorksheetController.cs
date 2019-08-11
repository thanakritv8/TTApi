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

        // POST CheckList/Profile/CreateWorksheet
        // POST CheckList/Profile/InsertWorksheet
        /// <summary>
        /// var model = new FormData(); 
        /// model.append('tran_code', 'เลขที่ใบงาน');
        /// model.append('number_po', 'เลขที่ออเดอร์');
        /// model.append('cus_id', 'PKลูกค้า');
        /// model.append('branch_id', 'PKสาขา');
        /// model.append('contact_id', 'PKผู้ติดต่อ');
        /// model.append('product_id', 'PKสินค้า');
        /// model.append('trunk_id', 'PK เส้นทาง');
        /// model.append('driver_id_1', 'PK พขร 1');
        /// model.append('driver_id_2', 'PK พขร 2');
        /// model.append('driver_id_3', 'PK พขร ฝึกหัด');
        /// model.append('license_id_head', 'PK รถหัวลาก');
        /// model.append('license_id_tail', 'PK หางกึ่งพ่วง'); 
        /// model.append('sheet_name', 'ชื่อใบงาน');
        /// model.append('cont1', 'เลขที่ตู้1');
        /// model.append('cont2', 'เลขที่ตู้2');
        /// model.append('remark', 'หมายเหตุ');  
        /// ***ajax*** 
        /// 
        /// data: model,
        /// processData: false,
        /// contentType: false,
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
                string _SQL = "INSERT INTO [dbo].[transport] (tran_code,number_po  ,cus_id  ,branch_id  ,contact_id ,product_id  ,trunk_id " +
                    ",driver_id_1  ,driver_id_2 ,driver_id_3  ,license_id_head  ,license_id_tail ,remark ,tran_status_id ,create_by_user_id ,sheet_name ,cont1,cont2   ) " +
     " output inserted.tran_id VALUES (N'" + val.tran_code + "', N'" + val.number_po + "', '" + val.cus_id + "', '" + val.branch_id + "', '" + val.contact_id + "', '" + val.product_id + "'" +
           ", '" + val.trunk_id + "' ,'" + val.driver_id_1 + "' , '" + val.driver_id_2 + "', '" + val.driver_id_3 + "', '" + val.license_id_head + "' , '" + val.license_id_tail + "'" +
           ", N'" + val.remark + "'  , 1  , 1  , '" + val.sheet_name + "' , '" + val.cont1 + "', '" + val.cont2 + "')";
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
        /// อัพเดทWorksheet
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("UpdateWorksheet")]
        public ExecuteModels UpdateWorksheet(WorksheetModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            { 
                string _SQL = "UPDATE  transport SET tran_code = N'" + val.tran_code + "'  , number_po = N'" + val.number_po + "' , cus_id = " + val.cus_id + " , branch_id = " + val.branch_id + " , contact_id = " + val.contact_id +
     " , product_id = " + val.product_id + " , trunk_id = " + val.trunk_id + ", driver_id_1 = " + val.driver_id_1 + ", driver_id_2 = " + val.driver_id_2 + ", driver_id_3 = " + val.driver_id_3 + ", license_id_head = " + val.license_id_head + 
     " , license_id_tail = " + val.license_id_tail + ", remark = " + val.remark + " , tran_status_id = " + val.tran_status_id +  ", update_by_user_id = 1 , sheet_name = N'" + val.sheet_name + "' , cont1 = N'" + val.cont1 + "', cont2 = N'" + val.cont2 + "' where tran_id = " + val.tran_id;
 
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

        // POST CheckList/Profile/GetWorksheetAll
        /// <summary>
        /// เรียกดูข้อมูลWorksheet
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
                string _SQL = "select * from transport";
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

        #region TempWorksheet

        // POST CheckList/Profile/TempWorksheet
        // POST CheckList/Profile/InsertTempWorksheet
        /// <summary>
        /// var model = new FormData(); 
        /// model.append('tran_code', 'เลขที่ใบงาน');
        /// model.append('number_po', 'เลขที่ออเดอร์');
        /// model.append('cus_id', 'PKลูกค้า');
        /// model.append('branch_id', 'PKสาขา');
        /// model.append('contact_id', 'PKผู้ติดต่อ');
        /// model.append('product_id', 'PKสินค้า');
        /// model.append('trunk_id', 'PK เส้นทาง');
        /// model.append('driver_id_1', 'PK พขร 1');
        /// model.append('driver_id_2', 'PK พขร 2');
        /// model.append('driver_id_3', 'PK พขร ฝึกหัด');
        /// model.append('license_id_head', 'PK รถหัวลาก');
        /// model.append('license_id_tail', 'PK หางกึ่งพ่วง'); 
        /// model.append('sheet_name', 'ชื่อใบงาน');
        /// model.append('cont1', 'เลขที่ตู้1');
        /// model.append('cont2', 'เลขที่ตู้2');
        /// model.append('remark', 'หมายเหตุ');  
        /// ***ajax*** 
        /// 
        /// data: model,
        /// processData: false,
        /// contentType: false,
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
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "UPDATE  transport_temp SET tran_code = N'" + val.tran_code + "'  , number_po = N'" + val.number_po + "' , cus_id = " + val.cus_id + " , branch_id = " + val.branch_id + " , contact_id = " + val.contact_id +
     " , product_id = " + val.product_id + " , trunk_id = " + val.trunk_id + ", driver_id_1 = " + val.driver_id_1 + ", driver_id_2 = " + val.driver_id_2 + ", driver_id_3 = " + val.driver_id_3 + ", license_id_head = " + val.license_id_head +
     " , license_id_tail = " + val.license_id_tail + ", remark = " + val.remark + " , tran_status_id = " + val.tran_status_id + ", update_by_user_id = 1 , sheet_name = N'" + val.sheet_name + "' , cont1 = N'" + val.cont1 + "', cont2 = N'" + val.cont2 + "' where tran_id = " + val.tran_id;

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


        #region UpdateStatusWorksheet

        // POST CheckList/Profile/UpdateStatusWorksheet
        /// <summary>
        /// UpdateStatusWorksheet
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("UpdateStatusWorksheet")]
        public ExecuteModels UpdateStatusWorksheet(StatusWorksheetModels val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabase())
            {
                string _SQL = "UPDATE  transport SET  tran_status_id = " + val.tran_status_id + ", update_by_user_id = 1  where tran_id = " + val.tran_id;

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