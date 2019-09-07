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
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("Tabien/Report")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TabienController : ApiController
    {
        #region Report
        #region report1
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
        #endregion

        #region report2
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetReport2All")]
        public List<Report2> GetReport2All()
        {
            HomeController hc = new HomeController();
            List<Report2> ul = new List<Report2>();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = "SELECT * from report2";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        Report2 rv = new Report2();
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
                        rv.txt36 = _Item["txt36"].ToString();
                        rv.txt37 = _Item["txt37"].ToString();
                        rv.txt38 = _Item["txt38"].ToString();
                        rv.txt39 = _Item["txt39"].ToString();
                        rv.txt40 = _Item["txt40"].ToString();
                        rv.txt41 = _Item["txt41"].ToString();
                        rv.txt42 = _Item["txt42"].ToString();
                        rv.txt43 = _Item["txt43"].ToString();
                        rv.txt44 = _Item["txt44"].ToString();
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
        [Route("InsertReport2")]
        public ExecuteModels InsertReport2(Report2 val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                try
                {
                    string _SQL = "insert into report2 (txt1, txt2, txt3, txt4, txt5, txt6, txt7, txt8, txt9, txt10, txt11, txt12, txt13, txt14, txt15, txt16, txt17, txt18, txt19,txt20, txt21, txt22, txt23, txt24, txt25, txt26, txt27, txt28, txt29, txt30, txt31, txt32, txt33, txt34, txt35, txt36, txt37, txt38, txt39, txt40, txt41, txt42, txt43, txt44, create_by_user_id) output inserted.id values (N'" + val.txt1 + "', N'" + val.txt2 + "', N'" + val.txt3 + "', N'" + val.txt4 + "', N'" + val.txt5 + "', N'" + val.txt6 + "', N'" + val.txt7 + "', N'" + val.txt8 + "', N'" + val.txt9 + "', N'" + val.txt10 + "', N'" + val.txt11 + "', N'" + val.txt12 + "', N'" + val.txt13 + "', N'" + val.txt14 + "', N'" + val.txt15 + "', N'" + val.txt16 + "', N'" + val.txt17 + "', N'" + val.txt18 + "', N'" + val.txt19 + "', N'" + val.txt20 + "', N'" + val.txt21 + "', N'" + val.txt22 + "', N'" + val.txt23 + "', N'" + val.txt24 + "', N'" + val.txt25 + "', N'" + val.txt26 + "', N'" + val.txt27 + "', N'" + val.txt28 + "', N'" + val.txt29 + "', N'" + val.txt30 + "', N'" + val.txt31 + "', N'" + val.txt32 + "', N'" + val.txt33 + "', N'" + val.txt34 + "', N'" + val.txt35 + "', N'" + val.txt36 + "', N'" + val.txt37 + "', N'" + val.txt38 + "', N'" + val.txt39 + "', N'" + val.txt40 + "', N'" + val.txt41 + "', N'" + val.txt42 + "', N'" + val.txt43 + "', N'" + val.txt44 + "', 1)";
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
        [Route("UpdateReport2")]
        public ExecuteModels UpdateReport2(Report2 val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = string.Empty;
                try
                {
                    string _SQL_Set = string.Empty;
                    string[] Col_Arr = { "txt1", "txt2", "txt3", "txt4", "txt5", "txt6", "txt7", "txt8", "txt9", "txt10", "txt11", "txt12", "txt13", "txt14", "txt15", "txt16", "txt17", "txt18", "txt19", "txt20", "txt21", "txt22", "txt23", "txt24", "txt25", "txt26", "txt27", "txt28", "txt29", "txt30", "txt31", "txt32", "txt33", "txt34", "txt35", "txt36", "txt37", "txt38", "txt39", "txt40", "txt41", "txt42", "txt43", "txt44" };
                    string[] Val_Arr = { val.txt1, val.txt2, val.txt3, val.txt4, val.txt5, val.txt6, val.txt7, val.txt8, val.txt9, val.txt10, val.txt11, val.txt12, val.txt13, val.txt14, val.txt15, val.txt16, val.txt17, val.txt18, val.txt19, val.txt20, val.txt21, val.txt22, val.txt23, val.txt24, val.txt25, val.txt26, val.txt27, val.txt28, val.txt29, val.txt30, val.txt31, val.txt32, val.txt33, val.txt34, val.txt35, val.txt36, val.txt37, val.txt38, val.txt39, val.txt40, val.txt41, val.txt42, val.txt43, val.txt44 };
                    for (int n = 0; n <= Val_Arr.Length - 1; n++)
                    {
                        if (Val_Arr[n] != null)
                        {
                            _SQL_Set += Col_Arr[n] + " = N'" + Val_Arr[n] + "', ";
                        }
                    }
                    _SQL = "update report2 set " + _SQL_Set + " create_by_user_id = 1 where id = " + val.id;
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
        [Route("DeleteReport2")]
        public ExecuteModels DelReport2(Report1 val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                try
                {
                    string _SQL = "delete from report2 where id = " + val.id;
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
        #endregion

        #region report3
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetReport3All")]
        public List<Report3> GetReport3All()
        {
            HomeController hc = new HomeController();
            List<Report3> ul = new List<Report3>();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = "SELECT * from report3";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        Report3 rv = new Report3();
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
        [Route("InsertReport3")]
        public ExecuteModels InsertReport3(Report3 val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                try
                {
                    string _SQL = "insert into report3 (txt1, txt2, txt3, txt4, txt5, txt6, txt7, txt8, txt9, txt10, txt11, txt12, txt13, txt14, txt15, txt16, txt17, txt18, txt19,txt20, txt21, txt22, txt23, txt24, create_by_user_id) output inserted.id values (N'" + val.txt1 + "', N'" + val.txt2 + "', N'" + val.txt3 + "', N'" + val.txt4 + "', N'" + val.txt5 + "', N'" + val.txt6 + "', N'" + val.txt7 + "', N'" + val.txt8 + "', N'" + val.txt9 + "', N'" + val.txt10 + "', N'" + val.txt11 + "', N'" + val.txt12 + "', N'" + val.txt13 + "', N'" + val.txt14 + "', N'" + val.txt15 + "', N'" + val.txt16 + "', N'" + val.txt17 + "', N'" + val.txt18 + "', N'" + val.txt19 + "', N'" + val.txt20 + "', N'" + val.txt21 + "', N'" + val.txt22 + "', N'" + val.txt23 + "', N'" + val.txt24 + "', 1)";
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
        [Route("UpdateReport3")]
        public ExecuteModels UpdateReport3(Report3 val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = string.Empty;
                try
                {
                    string _SQL_Set = string.Empty;
                    string[] Col_Arr = { "txt1", "txt2", "txt3", "txt4", "txt5", "txt6", "txt7", "txt8", "txt9", "txt10", "txt11", "txt12", "txt13", "txt14", "txt15", "txt16", "txt17", "txt18", "txt19", "txt20", "txt21", "txt22", "txt23", "txt24" };
                    string[] Val_Arr = { val.txt1, val.txt2, val.txt3, val.txt4, val.txt5, val.txt6, val.txt7, val.txt8, val.txt9, val.txt10, val.txt11, val.txt12, val.txt13, val.txt14, val.txt15, val.txt16, val.txt17, val.txt18, val.txt19, val.txt20, val.txt21, val.txt22, val.txt23, val.txt24 };
                    for (int n = 0; n <= Val_Arr.Length - 1; n++)
                    {
                        if (Val_Arr[n] != null)
                        {
                            _SQL_Set += Col_Arr[n] + " = N'" + Val_Arr[n] + "', ";
                        }
                    }
                    _SQL = "update report3 set " + _SQL_Set + " create_by_user_id = 1 where id = " + val.id;
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
        [Route("DeleteReport3")]
        public ExecuteModels DelReport3(Report3 val)
        {
            ExecuteModels ecm = new ExecuteModels();
            HomeController hc = new HomeController();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                try
                {
                    string _SQL = "delete from report3 where id = " + val.id;
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
        #endregion
        #endregion

        #region Upload Pic License
        [AllowAnonymous]
        [Route("UploadPicLicense")]
        public ExecuteModels UploadPicLicense()
        {
            ExecuteModels ecm = new ExecuteModels();
            NameValueCollection nvc = HttpContext.Current.Request.Form;
            var val = new LocationModels();
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
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL_Set = string.Empty;
                
                // Upload file
                string path = string.Empty;
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    if (HttpContext.Current.Request.Files.Count > 0)
                    {
                        try
                        {
                            val.path_img = HttpContext.Current.Request.Files[0];
                            path = HttpContext.Current.Request.MapPath(@"~/Files/lp/" + val.license_id + "_" + val.loc_img + ".png");
                            val.path_img.SaveAs(path);
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


                string _SQL = "update license set p" + val.loc_img + " = '" + path + "' where license_id = " + val.license_id;
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

        [AllowAnonymous]
        [Route("GetLicenseNotComplete")]
        public List<LicenseNotComplete> GetLicenseNotComplete()
        {
            HomeController hc = new HomeController();
            List<LicenseNotComplete> ul = new List<LicenseNotComplete>();
            using (SqlConnection con = hc.ConnectDatabaseTT1995())
            {
                string _SQL = "sp_GetLicenseNotComplete";
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    DataTable _Dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_Dt);
                    da.Dispose();
                    foreach (DataRow _Item in _Dt.Rows)
                    {
                        LicenseNotComplete rv = new LicenseNotComplete();
                        rv.license_id = _Item["license_id"].ToString();
                        rv.number_car = _Item["number_car"].ToString();
                        rv.license_car = _Item["license_car"].ToString();
                        rv.notcomplete = _Item["NotComplete"].ToString();
                        rv.show_pic = _Item["show_pic"].ToString();
                        rv.upload = _Item["upload"].ToString();                        
                        ul.Add(rv);
                    }
                }
                con.Close();
            }
            return ul;
        }
    }

}
