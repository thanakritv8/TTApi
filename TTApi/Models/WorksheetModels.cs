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
        public string license_id_head { get; set; }
        public string license_id_tail { get; set; }
        public string remark { get; set; }
        public string tran_status_id { get; set; }
    }
 
 
    #endregion
}