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

    #region EquipmentType
    public class EquipmentType
    {
        /// <summary>
        /// Equipment Type ID
        /// </summary>
        public string eq_type_id { get; set; }
        /// <summary>
        /// Equipment Type Name
        /// </summary>
        public string eq_type { get; set; }
    }
    #endregion

    #region Document
    public class DocumentView
    {
        public string doc_id { get; set; }
        public string doc_code { get; set; }
        public string doc_name { get; set; }
        public string doc_path { get; set; }
        public string remark { get; set; }
        public string doc_type_id { get; set; }
        public string doc_type { get; set; }
    }

    public class Document
    {
        public string doc_id { get; set; }
        public string doc_code { get; set; }
        public string doc_name { get; set; }
        public string doc_path { get; set; }
        public string remark { get; set; }
        public string doc_type_id { get; set; }
        public HttpPostedFile Image { get; set; }
    }
    #endregion

    #region DocumentType
    public class DocumentType
    {
        /// <summary>
        /// Document Type Id
        /// </summary>
        public string doc_type_id { get; set; }
        /// <summary>
        /// Document Type Name
        /// </summary>
        public string doc_type { get; set; }
    }
    #endregion

    #region license
    public class LicenseAllView
    {
        public string license_id { get; set; }
        public string number_car { get; set; }
        public string license_car { get; set; }

    }

    public class LicenseModels
    {
        public string license_id { get; set; }

    }

    /// <summary>
    /// ข้อมูลภาษี/ประกันภัย
    /// </summary>
    public class ExpiredView
    {
        /// <summary>
        /// ประกันภัยรถยนต์
        /// </summary>
        public string mi_expired { get; set; }
        /// <summary>
        /// พรบ
        /// </summary>
        public string ai_expired { get; set; }
        /// <summary>
        /// ประกันภัยสิ่งแวดล้อม
        /// </summary>
        public string ei_expired { get; set; }
        /// <summary>
        /// ประกันภัยภายในประเทศ
        /// </summary>
        public string dpi_expired { get; set; }

    }
    #endregion
}