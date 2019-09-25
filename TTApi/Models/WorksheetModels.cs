using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTApi.Models
{
    #region Worksheet

    public class WorksheetIdModels
    {
        public string tran_id { get; set; }
    }

    public class WorksheetModels
    {
        public string tran_id { get; set; }
        public string tran_code { get; set; }
        public string number_po { get; set; }
        public string cus_id { get; set; }
        public string branch_id { get; set; }
        public string contact_id { get; set; }
        public string product_id { get; set; }
        public string trunk_id { get; set; }
        public string driver_id_1 { get; set; }
        public string driver_id_2 { get; set; }
        public string driver_id_3 { get; set; }
        public string driver_id_4 { get; set; }
        public string license_id_head { get; set; }
        public string license_id_tail { get; set; }
        public string remark { get; set; }
        public string tran_status_id { get; set; }
        public string create_by_user_id { get; set; }
        public string sheet_name { get; set; }
        public string cont1 { get; set; }
        public string cont2 { get; set; }
        public string update_by_user_id { get; set; }
        public string bugget_tran { get; set; }
        public string date_work { get; set; }
        public string date_start { get; set; }
        public string date_end { get; set; }
        public string value_order { get; set; }
        public string type_tran { get; set; }
        public string size_cont1 { get; set; }
        public string size_cont2 { get; set; }
        public string condition_tran { get; set; }
        public string special_order { get; set; }
        public string tank_number { get; set; }
        public string style_tank { get; set; }
        public string weight_tank { get; set; }
    }

    public class WorksheetAllView
    {
        public string tran_id { get; set; }
        public string tran_code { get; set; }
        public string number_po { get; set; }
        public string cus_id { get; set; }
        public string branch_id { get; set; }
        public string contact_id { get; set; }
        public string product_id { get; set; }
        public string trunk_id { get; set; }
        public string driver_id_1 { get; set; }
        public string driver_id_2 { get; set; }
        public string driver_id_3 { get; set; }
        public string driver_id_4 { get; set; }
        public string license_id_head { get; set; }
        public string license_id_tail { get; set; }
        public string remark { get; set; }
        public string tran_status_id { get; set; }
        public string sheet_name { get; set; }
        public string cont1 { get; set; }
        public string cont2 { get; set; }
        public string create_date { get; set; }
        public string create_by_user_id { get; set; }
        public string update_date { get; set; }
        public string update_by_user_id { get; set; }
        public string bugget_tran { get; set; }
        public string date_work { get; set; }
        public string date_start { get; set; }
        public string date_end { get; set; }
        public string value_order { get; set; }
        public string type_tran { get; set; }
        public string size_cont1 { get; set; }
        public string size_cont2 { get; set; }
        public string condition_tran { get; set; }
        public string special_order { get; set; }
        public string tank_number { get; set; }
        public string style_tank { get; set; }
        public string weight_tank { get; set; }
    }
    public class WorksheetView
    {
        public string kind { get; set; }
        public string tran_code { get; set; }
        public string number_po { get; set; }
        public string cus_name { get; set; }
        public string branch_name { get; set; }
        public string address { get; set; }
        public string contact_name { get; set; }
        public string position { get; set; }
        public string tel { get; set; }
        public string source { get; set; }
        public string destination { get; set; }
        public string station { get; set; }
        public string driver1 { get; set; }
        public string driver1_license_start { get; set; }
        public string driver1_license_expire { get; set; }
        public string driver2 { get; set; }
        public string driver2_license_start { get; set; }
        public string driver2_license_expire { get; set; }
        public string driver3 { get; set; }
        public string driver3_license_start { get; set; }
        public string driver3_license_expire { get; set; }
        public string driver4 { get; set; }
        public string driver4_license_start { get; set; }
        public string driver4_license_expire { get; set; }
        public string license_head { get; set; }
        public string style_car_head { get; set; }
        public string license_tail { get; set; }
        public string style_car_tail { get; set; }
        public string doc_code { get; set; }
        public string doc_name { get; set; }
        public string doc_type_id { get; set; }
        public string eq_code { get; set; }
        public string eq_name { get; set; }
        public string eq_amount { get; set; }
        public string product_name { get; set; }
        public string fleet { get; set; }
        public string method_contain { get; set; }
        public string method_normal { get; set; }
        public string method_special { get; set; }
        public string method_style { get; set; }
        public string cont1 { get; set; }
        public string cont2 { get; set; }
        public string remark { get; set; }
        public string update_by_user_id { get; set; }
        public string update_date { get; set; }
        public string Approve_By { get; set; }
        public string bugget_tran { get; set; }
        public string date_work { get; set; }
        public string date_start { get; set; }
        public string date_end { get; set; }
        public string value_order { get; set; }
        public string type_tran { get; set; }
        public string size_cont1 { get; set; }
        public string size_cont2 { get; set; }
        public string condition_tran { get; set; }
        public string special_order { get; set; }
        public string tank_number { get; set; }
        public string style_tank { get; set; }
        public string weight_tank { get; set; }
    }

    #endregion


    #region TempWorksheet

    public class TempWorksheetIdModels
    {
        public string tran_id { get; set; }
    }

    public class TempWorksheetModels
    {
        public string tran_id { get; set; }
        public string tran_code { get; set; }
        public string number_po { get; set; }
        public string cus_id { get; set; }
        public string branch_id { get; set; }
        public string contact_id { get; set; }
        public string product_id { get; set; }
        public string trunk_id { get; set; }
        public string driver_id_1 { get; set; }
        public string driver_id_2 { get; set; }
        public string driver_id_3 { get; set; }
        public string driver_id_4 { get; set; }
        public string license_id_head { get; set; }
        public string license_id_tail { get; set; }
        public string remark { get; set; }
        public string tran_status_id { get; set; }
        public string sheet_name { get; set; }
        public string cont1 { get; set; }
        public string cont2 { get; set; }
    }

    public class TempWorksheetAllView
    {
        public string tran_id { get; set; }
        public string tran_code { get; set; }
        public string number_po { get; set; }
        public string cus_id { get; set; }
        public string branch_id { get; set; }
        public string contact_id { get; set; }
        public string product_id { get; set; }
        public string trunk_id { get; set; }
        public string driver_id_1 { get; set; }
        public string driver_id_2 { get; set; }
        public string driver_id_3 { get; set; }
        public string driver_id_4 { get; set; }
        public string license_id_head { get; set; }
        public string license_id_tail { get; set; }
        public string remark { get; set; }
        public string tran_status_id { get; set; }
        public string sheet_name { get; set; }
        public string cont1 { get; set; }
        public string cont2 { get; set; }
        public string create_date { get; set; }
        public string create_by_user_id { get; set; }
        public string update_date { get; set; }
        public string update_by_user_id { get; set; }

    }

    #endregion


    #region StatusWorksheet

    public class StatusWorksheetModels
    {
        public string tran_id { get; set; }
        public string tran_status_id { get; set; }
    }


    #endregion
}