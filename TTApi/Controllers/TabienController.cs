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
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("Tabien/Report")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TabienController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetReport1All")]
        public List<Report1> GetReport1All()
        {
            HomeController hc = new HomeController();
            List<Report1> ul = new List<Report1>();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = "SELECT * from report1";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        Report1 rv = new Report1();
                        rv.id = _Item["id"].ToString();
                        rv.txt1 = _Item["txt1"].ToString();
                        rv.txt2 = _Item["txt2"].ToString();
                        rv.txt3 = _Item["txt3"].ToString();
                        rv.txt4 = _Item["txt4"].ToString();
                        rv.txt5 = _Item["txt5"].ToString();
                        rv.txt6 = _Item["txt6"].ToString();
                        rv.txt7 = _Item["txt7"].ToString();
                        rv.txt8 = _Item["txt8"].ToString();
                        rv.txt9 = _Item["txt9"].ToString();
                        rv.txt10 = _Item["txt10"].ToString();
                        rv.txt11 = _Item["txt11"].ToString();
                        rv.txt12 = _Item["txt12"].ToString();
                        rv.txt13 = _Item["txt13"].ToString();
                        rv.txt14 = _Item["txt14"].ToString();
                        rv.txt15 = _Item["txt15"].ToString();
                        rv.txt16 = _Item["txt16"].ToString();
                        rv.txt17 = _Item["txt17"].ToString();
                        rv.txt18 = _Item["txt18"].ToString();
                        rv.txt19 = _Item["txt19"].ToString();
                        rv.txt20 = _Item["txt20"].ToString();
                        rv.txt21 = _Item["txt21"].ToString();
                        rv.txt22 = _Item["txt22"].ToString();
                        rv.txt23 = _Item["txt23"].ToString();
                        rv.txt24 = _Item["txt24"].ToString();
                        rv.txt25 = _Item["txt25"].ToString();
                        rv.txt26 = _Item["txt26"].ToString();
                        rv.txt27 = _Item["txt27"].ToString();
                        rv.txt28 = _Item["txt28"].ToString();
                        rv.txt29 = _Item["txt29"].ToString();
                        rv.txt30 = _Item["txt30"].ToString();
                        rv.txt31 = _Item["txt31"].ToString();
                        rv.txt32 = _Item["txt32"].ToString();
                        rv.txt33 = _Item["txt33"].ToString();
                        rv.txt34 = _Item["txt34"].ToString();
                        rv.txt35 = _Item["txt35"].ToString();
                        ul.Add(rv);
                    }
                }
                con.Close();
            }
            return ul;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("InsertReport1")]
        public ExecuteModels InsertReport1(Report1 val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                try
                {
                    string _SQL = "insert into report1 (txt1, txt2, txt3, txt4, txt5, txt6, txt7, txt8, txt9, txt10, txt11, txt12, txt13, txt14, txt15, txt16, txt17, txt18, txt19,txt20, txt21, txt22, txt23, txt24, txt25, txt26, txt27, txt28, txt29, txt30, txt31, txt32, txt33, txt34, txt35, create_by_user_id) output inserted.id values (N'" + val.txt1 + "', N'" + val.txt2 + "', N'" + val.txt3 + "', N'" + val.txt4 + "', N'" + val.txt5 + "', N'" + val.txt6 + "', N'" + val.txt7 + "', N'" + val.txt8 + "', N'" + val.txt9 + "', N'" + val.txt10 + "', N'" + val.txt11 + "', N'" + val.txt12 + "', N'" + val.txt13 + "', N'" + val.txt14 + "', N'" + val.txt15 + "', N'" + val.txt16 + "', N'" + val.txt17 + "', N'" + val.txt18 + "', N'" + val.txt19 + "', N'" + val.txt20 + "', N'" + val.txt21 + "', N'" + val.txt22 + "', N'" + val.txt23 + "', N'" + val.txt24 + "', N'" + val.txt25 + "', N'" + val.txt26 + "', N'" + val.txt27 + "', N'" + val.txt28 + "', N'" + val.txt29 + "', N'" + val.txt30 + "', N'" + val.txt31 + "', N'" + val.txt32 + "', N'" + val.txt33 + "', N'" + val.txt34 + "', N'" + val.txt35 + "', 1)";
                    using (SqlCommand cmd = new SqlCommand(_SQL, con))
                    {
                        var id_return = Int32.Parse(cmd.ExecuteScalar().ToString());
                        if (id_return >= 1)
                        {
                            ecm.result = 0;
                            ecm.code = "OK";
                            ecm.id_return = id_return.ToString();
                        }
                        else
                        {
                            ecm.result = 1;
                            ecm.code = "NOK";                            
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("UpdateReport1")]
        public ExecuteModels UpdateReport1(Report1 val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = string.Empty;
                try
                {
                    string _SQL_Set = string.Empty;
                    string[] Col_Arr = { "txt1", "txt2", "txt3", "txt4", "txt5", "txt6", "txt7", "txt8", "txt9", "txt10", "txt11", "txt12", "txt13", "txt14", "txt15", "txt16", "txt17", "txt18", "txt19", "txt20", "txt21", "txt22", "txt23", "txt24", "txt25", "txt26", "txt27", "txt28", "txt29", "txt30", "txt31", "txt32", "txt33", "txt34", "txt35" };
                    string[] Val_Arr = { val.txt1, val.txt2, val.txt3, val.txt4, val.txt5, val.txt6, val.txt7, val.txt8, val.txt9, val.txt10, val.txt11, val.txt12, val.txt13, val.txt14, val.txt15, val.txt16, val.txt17, val.txt18, val.txt19, val.txt20, val.txt21, val.txt22, val.txt23, val.txt24, val.txt25, val.txt26, val.txt27, val.txt28, val.txt29, val.txt30, val.txt31, val.txt32, val.txt33, val.txt34, val.txt35 };
                    for (int n = 0; n <= Val_Arr.Length - 1; n++)
                    {
                        if (Val_Arr[n] != null)
                        {
                            _SQL_Set += Col_Arr[n] + " = N'" + Val_Arr[n] + "', ";
                        }
                    }
                    _SQL = "update report1 set " + _SQL_Set + " create_by_user_id = 1 where id = " + val.id;
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
                }
                catch (Exception ex)
                {
                    ecm.result = 1;
                    ecm.code = ex.Message + " sql => " + _SQL;
                }

                con.Close();
            }
            return ecm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DeleteReport1")]
        public ExecuteModels DelReport1(Report1 val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                try
                {                    
                    string _SQL = "delete from report1 where id = " + val.id;
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
    }
}
