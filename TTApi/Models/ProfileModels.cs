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
        /// license_id
        /// </summary>
        public string license_id { get; set; }
        /// <summary>
        /// display
        /// </summary>
        public string display { get; set; }
        /// <summary>
        /// expire
        /// </summary>
        public string expire { get; set; }
    }

    /// <summary>
    /// ข้อมูลใบอนุญาต
    /// </summary>
    public class ExpiredOtherView
    {
        /// <summary>
        /// license_id
        /// </summary>
        public string license_id { get; set; }
        /// <summary>
        /// display
        /// </summary>
        public string display { get; set; }
        /// <summary>
        /// expire
        /// </summary>
        public string expire { get; set; }
        /// <summary>
        /// detail
        /// </summary>
        public string detail { get; set; }
        /// <summary>
        /// detail2
        /// </summary>
        public string detail2 { get; set; }


    }

    /// <summary>
    /// รายละเอียด
    /// </summary>
    public class DetailLicense
    {
        /// <summary>
        /// license_id
        /// </summary>
        public string license_id { get; set; }
        /// <summary>
        /// ลักษณะ 
        /// </summary>
        public string style_car { get; set; }
        /// <summary>
        /// น้ำหนักรถเปล่า
        /// </summary>
        public string weight_car { get; set; }
        /// <summary>
        /// ยี่ห้อ 
        /// </summary>
        public string brand_engine { get; set; }
        /// <summary>
        /// รุ่น 
        /// </summary>
        public string model_car { get; set; }
        /// <summary>
        /// รูป
        /// </summary>
        public List<ListGallery> gallery { get; set; }


    }

    public class ListGallery
    {
        /// <summary>
        /// file id
        /// </summary>
        public string file_id { get; set; }
        /// <summary>
        /// path img
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// position img
        /// </summary>
        public string position { get; set; }
    }
    #endregion

    #region Customer

    public class CustomerIdModels
    {
        public string cus_id { get; set; }
    }

    public class CustomerModels
    {
        public string cus_id { get; set; }
        public string cus_name { get; set; }
    }

    /// <summary>
    /// ลูกค้าทั้งหมด
    /// </summary>
    public class CustomerAllView
    {
        /// <summary>
        /// Customer id
        /// </summary>
        public string cus_id { get; set; }
        /// <summary>
        /// Customer name
        /// </summary>
        public string cus_name { get; set; }

    }

    /// <summary>
    /// สาขา
    /// </summary>
    public class BranchCustomerAllView
    {
        /// <summary>
        /// Branch id
        /// </summary>
        public string branch_id { get; set; }
        /// <summary>
        /// Branch name
        /// </summary>
        public string branch_name { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// Zip code
        /// </summary>
        public string zip_code { get; set; }
        /// <summary>
        /// Province id
        /// </summary>
        public string province_id { get; set; }
        /// <summary>
        /// Customer id
        /// </summary>
        public string cus_id { get; set; }

    }

    public class BranchCustomerIdModels
    {
        public string branch_id { get; set; }
    }

    public class BranchCustomerModels
    {
        public string branch_id { get; set; }
        public string branch_name { get; set; }
        public string address { get; set; }
        public string zip_code { get; set; }
        public string province_id { get; set; }
        public string cus_id { get; set; }
    }

    public class TrunkView
    {
        public string trunk_id { get; set; }
        public string source { get; set; }
        public string destination { get; set; }
    }

    public class TrunkIdModels
    {
        public string trunk_id { get; set; }
    }

    public class TrunkModels
    {
        public string trunk_id { get; set; }
        public string source { get; set; }
        public string destination { get; set; }
        public string branch_id { get; set; }
    }

    public class ContactView
    {
        public string contact_id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string tel { get; set; }
        public string line { get; set; }
        public string email { get; set; }
    }

    public class ContactIdModels
    {
        public string contact_id { get; set; }
    }

    public class ContactModels
    {
        public string contact_id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string tel { get; set; }
        public string line { get; set; }
        public string email { get; set; }
        public string branch_id { get; set; }
    }

    public class Product
    {
        /// <summary>
        /// Product id
        /// </summary>
        public string product_id { get; set; }
        /// <summary>
        /// Product name
        /// </summary>
        public string product_name { get; set; }
        /// <summary>
        /// Fleet
        /// </summary>
        public string fleet { get; set; }
        /// <summary>
        /// Style
        /// </summary>
        public string style_name { get; set; }
        /// <summary>
        /// พขร/รถบรรทุก
        /// </summary>
        public string DriverOrTruck { get; set; }
        /// <summary>
        /// เอกสาร/อุปกรณ์
        /// </summary>
        public string DocumentOrEquipment { get; set; }

    }
    #endregion

    #region Driver
    public class DriverAllView
    {
        public string driver_id { get; set; }
        public string driver_name { get; set; }
        public string sex { get; set; }
        public string age { get; set; }
        public string other { get; set; }
        public string path { get; set; }
    }

    public class DriverIdModels
    {
        public string driver_id { get; set; }
    }

    public class DriverLicenseView
    {
        public string driver_id { get; set; }
        public string display { get; set; }
        public string expire { get; set; }
    }
    #endregion
}