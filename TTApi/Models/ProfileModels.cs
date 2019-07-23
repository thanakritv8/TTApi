using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTApi.Models
{
    #region EquipmentSafety
    public class EquipmentSafetyView
    {
        public string eq_safety_id { get; set; }
        public string eq_safety_code { get; set; }
        public string eq_name { get; set; }
        public string style { get; set; }
        public string property { get; set; }
        public string suggestion { get; set; }
        public string eq_type_id { get; set; }
        public string eq_type { get; set; }
        public string eq_path { get; set; }
    }

    public class EquipmentSafety
    {
        public string eq_safety_id { get; set; }
        public string eq_safety_code { get; set; }
        public string eq_name { get; set; }
        public string style { get; set; }
        public string property { get; set; }
        public string suggestion { get; set; }
        public string eq_type_id { get; set; }
        public HttpPostedFile Image { get; set; }
    }
    #endregion

    #region EquipmentTransport
    public class EquipmentTransportView
    {
        public string eq_tran_id { get; set; }
        public string eq_tran_code { get; set; }
        public string eq_name { get; set; }
        public string style { get; set; }
        public string property { get; set; }
        public string suggestion { get; set; }
        public string eq_type_id { get; set; }
        public string eq_type { get; set; }
        public string eq_path { get; set; }
    }

    public class EquipmentTransport
    {
        public string eq_tran_id { get; set; }
        public string eq_tran_code { get; set; }
        public string eq_name { get; set; }
        public string style { get; set; }
        public string property { get; set; }
        public string suggestion { get; set; }
        public string eq_type_id { get; set; }
        public HttpPostedFile Image { get; set; }
    }
    #endregion
}