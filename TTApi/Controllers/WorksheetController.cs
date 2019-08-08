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
    /// Link http://43.254.133.49:8015/TTApi/CheckList/CreateWorksheet
    /// </summary>
    [Authorize]
    [RoutePrefix("CheckList/Worksheet")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WorksheetController : ApiController
    {
        #region EquipmentSafety

        // POST CheckList/Profile/CreateWorksheet
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
                    ",driver_id_1  ,driver_id_2 ,driver_id_3  ,license_id_head  ,license_id_tail ,remark ,tran_status_id   ,create_by_user_id ) "+
     " output inserted.tran_id VALUES (N'" + val.tran_code + "', N'" + val.number_po + "', '" + val.cus_id + "', '" + val.branch_id + "', '" + val.contact_id + "', '" + val.product_id + "'" +
           ", '" + val.trunk_id + "' ,'" + val.driver_id_1 + "' , '" + val.driver_id_2 + "', '" + val.driver_id_3 + "', '" + val.license_id_head + "' , '" + val.license_id_tail + "'" +
           ", N'" + val.remark + "'  , '" + val.tran_status_id + "'  , 1 )";
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
#endregion

    }
}