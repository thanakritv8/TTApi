﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTApi.Models
{
    public class FileAllView
    {
        public string seq { get; set; }
        public string path { get; set; }
    }

    public class FileAllModels
    {
        public string seq { get; set; }
    }

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
        public List<FileAllView> path { get; set; }
        public string eq_weight { get; set; }
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
        public string eq_weight { get; set; }
        public string user_id { get; set; }
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
        public string eq_weight { get; set; }
        public List<FileAllView> path { get; set; }
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
        public string eq_weight { get; set; }
        public string user_id { get; set; }
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
        public List<FileAllView> path { get; set; }
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
        public string user_id { get; set; }
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
        public string fleet { get; set; }
        public string col1 = "VIEW";
        public string col2 = "VIEW";
        public string col3 = "VIEW";
        public string col4 = "แก้ไข";

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
        /// <summary>
        /// 0 = Normal
        /// 1 = Approve
        /// หรือถ้าไม่ใส่จะดึงค้าทั้งหมด
        /// </summary>
        public int typeGet { get; set; }
    }

    public class CustomerModels
    {
        public string cus_id { get; set; }
        public string cus_name { get; set; }
        public string user_id { get; set; }
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
        public string user_id { get; set; }
    }

    public class TrunkView
    {
        public string trunk_id { get; set; }
        public string source { get; set; }
        public string destination { get; set; }
        public string station { get; set; }
        public string cus_id { get; set; }
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
        public string station { get; set; }
        public string cus_id { get; set; }
        public string user_id { get; set; }
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
        public string user_id { get; set; }
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

    #region Product
    public class ProductAllView
    {
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string fleet { get; set; }
        public string method_style { get; set; }
        public string method_normal { get; set; }
        public string method_contain { get; set; }
        public string method_special { get; set; }
        public string col1 = "VIEW";
        public string col2 = "VIEW";
        public string col3 = "แก้ไข";
        //public List<RelDocProductModels> rel_doc { get; set;}
        //public List<DocumentView> rel_doc { get; set; }
    }

    public class ProductIdModels
    {
        public string product_id { get; set; }

    }

    public class ProductModels
    {
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string fleet { get; set; }
        public string method_style { get; set; }
        public string method_normal { get; set; }
        public string method_contain { get; set; }
        public string method_special { get; set; }
        public string cus_id { get; set; }
        public string user_id { get; set; }
    }

    public class RelDriverProductModels
    {
        public string driver_id { get; set; }
        public string product_id { get; set; }
        public string score { get; set; }
        public string user_id { get; set; }
    }

    public class RelDocProductModels
    {
        public string doc_id { get; set; }
        public string product_id { get; set; }
        public string doc_type_id { get; set; }
        public string user_id { get; set; }
    }

    public class RelSafetyProductModels
    {
        public string eq_safety_id { get; set; }
        public string product_id { get; set; }
        public string amount { get; set; }
        public string user_id { get; set; }
    }

    public class RelTranProductModels
    {
        public string eq_tran_id { get; set; }
        public string product_id { get; set; }
        public string amount { get; set; }
        public string user_id { get; set; }
    }

    public class RelLicenseProductModels
    {
        public string license_id { get; set; }
        public string product_id { get; set; }
        public string user_id { get; set; }
    }

    public class RelCustomerProductModels
    {
        public string cus_id { get; set; }
        public string product_id { get; set; }
        public string user_id { get; set; }
    }

    public class RelDriverProductView
    {
        public string rel_id { get; set; }
        public string driver_id { get; set; }
        public string driver_name { get; set; }
        public string rel_status { get; set; }
        public string score { get; set; }
        public string status_approve { get; set; }
    }

    /// <summary>
    /// แสดงความสัมพันธ์ของเอกสารและสินค้า
    /// </summary>
    public class RelDocProductView
    {
        /// <summary>
        /// เท่ากับ 0 แสดงว่าไม่มีข้อมูลใน table relation
        /// ถ้ามีค่ามากกว่า 1 แสดงว่ามีข้อมูลใน table relation สามารถใส่ checked ได้
        /// </summary>
        public string rel_status { get; set; }
        /// <summary>
        /// PK relation
        /// </summary>
        public string rel_id { get; set; }
        /// <summary>
        /// PK เอกสาร
        /// </summary>
        public string doc_id { get; set; }
        /// <summary>
        /// รหัสเอกสาร
        /// </summary>
        public string doc_code { get; set; }
        /// <summary>
        /// ชนิดเอกสาร นำไป = 1, นำกลับ = 2, ทั้งไปทั้งกลับ = 3 ที่อยู่ใน table relation ใช้ในกรณีอัพเดทหรือแสดงให้ใช้ column นี้
        /// </summary>
        public string type_rel { get; set; }
        /// <summary>
        /// สถานะการ approve ของ relation document กับ product
        /// </summary>
        public string status_approve { get; set; }
        /// <summary>
        /// ชื่อเอกสาร
        /// </summary>
        public string doc_name { get; set; }
        /// <summary>
        /// อื่นๆ
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// เหมือน column type_rel ต่างกันตรงที่ column นี้ใช้ตอน create เพื่อใช้เป็น default
        /// </summary>
        public string type_default { get; set; }

        /// <summary>
        /// ผ่านการโปรเซสมาแล้ว
        /// </summary>
        public string type_show { get; set; }
    }

    public class RelSafetyProductView
    {
        public string rel_id { get; set; }
        public string eq_safety_id { get; set; }
        public string eq_safety_code { get; set; }
        public string amount { get; set; }
        public string status_approve { get; set; }
        public string eq_name { get; set; }
        public string eq_path { get; set; }
        public string eq_type_id { get; set; }
        public string property { get; set; }
        public string suggestion { get; set; }
        public string style { get; set; }
        public string rel_status { get; set; }
    }

    public class RelTranProductView
    {
        public string rel_id { get; set; }
        public string eq_tran_id { get; set; }
        public string eq_tran_code { get; set; }
        public string amount { get; set; }
        public string status_approve { get; set; }
        public string eq_name { get; set; }
        public string eq_path { get; set; }
        public string eq_type_id { get; set; }
        public string property { get; set; }
        public string suggestion { get; set; }
        public string style { get; set; }
        public string rel_status { get; set; }
    }

    public class RelLicenseProductView
    {
        public string rel_id { get; set; }
        public string license_id { get; set; }
        public string license_car { get; set; }
        public string number_car { get; set; }
        public string rel_status { get; set; }
        public string status_approve { get; set; }
        public string fleet { get; set; }
    }

    public class RelCusProductView
    {
        public string id { get; set; }
        public string cus_id { get; set; }
        public string product_id { get; set; }
        public string status_approve { get; set; }
    }

    public class IdModels
    {
        public string id { get; set; }
    }

    public class RelIdModels
    {
        public string pk_id { get; set; }
        public string fk_id { get; set; }
    }



    #endregion

    #region Approve
    public class ApproveModels
    {
        public string id { get; set; }
        public string nametable { get; set; }
        public string nameid { get; set; }
        public string user_id { get; set; }
    }

    public class RelContactBranch
    {
        public string rel_id { get; set; }
        public string contact_name { get; set; }
        public string branch_name { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string tel { get; set; }
        public string line { get; set; }
        public string status_approve = "รออนุมัติ";
        public string user_id { get; set; }
    }

    public class RelDocumentProduct
    {
        public string rel_id { get; set; }
        public string doc_type { get; set; }
        public string doc_code { get; set; }
        public string doc_name { get; set; }
        public string doc_path { get; set; }
        public string remark { get; set; }
        public string product_name { get; set; }
        public string fleet { get; set; }
        public string method_style { get; set; }
        public string method_normal { get; set; }
        public string method_contain { get; set; }
        public string method_special { get; set; }
        public string product_path { get; set; }
        public string status_approve = "รออนุมัติ";
        public string cus_name { get; set; }
        public string user_id { get; set; }
    }

    public class RelDriverProduct
    {
        public string rel_id { get; set; }
        public string driver_name { get; set; }
        public string product_name { get; set; }
        public string fleet { get; set; }
        public string method_style { get; set; }
        public string method_normal { get; set; }
        public string method_contain { get; set; }
        public string method_special { get; set; }
        public string product_path { get; set; }
        public string score { get; set; }
        public string status_approve = "รออนุมัติ";
        public string user_id { get; set; }
    }

    public class RelSafetyProduct
    {
        public string rel_id { get; set; }
        public string eq_safety_code { get; set; }
        public string eq_name { get; set; }
        public string style { get; set; }
        public string property { get; set; }
        public string suggestion { get; set; }
        public string eq_type { get; set; }
        public string eq_path { get; set; }
        public string product_name { get; set; }
        public string fleet { get; set; }
        public string method_style { get; set; }
        public string method_normal { get; set; }
        public string method_contain { get; set; }
        public string method_special { get; set; }
        public string product_path { get; set; }
        public string amount { get; set; }
        public string status_approve = "รออนุมัติ";
        public string user_id { get; set; }
    }

    public class RelTranProduct
    {
        public string rel_id { get; set; }
        public string eq_tran_code { get; set; }
        public string eq_name { get; set; }
        public string style { get; set; }
        public string property { get; set; }
        public string suggestion { get; set; }
        public string eq_type { get; set; }
        public string eq_path { get; set; }
        public string product_name { get; set; }
        public string fleet { get; set; }
        public string method_style { get; set; }
        public string method_normal { get; set; }
        public string method_contain { get; set; }
        public string method_special { get; set; }
        public string product_path { get; set; }
        public string amount { get; set; }
        public string status_approve = "รออนุมัติ";
        public string user_id { get; set; }
    }

    public class RelLicenseProduct
    {
        public string rel_id { get; set; }
        public string number_car { get; set; }
        public string license_car { get; set; }
        public string product_name { get; set; }
        public string fleet { get; set; }
        public string method_style { get; set; }
        public string method_normal { get; set; }
        public string method_contain { get; set; }
        public string method_special { get; set; }
        public string product_path { get; set; }
        public string amount { get; set; }
        public string status_approve = "รออนุมัติ";
        public string user_id { get; set; }
    }

    public class RelCustomerProduct
    {
        public string rel_id { get; set; }
        public string cus_name { get; set; }
        public string product_name { get; set; }
        public string fleet { get; set; }
        public string method_style { get; set; }
        public string method_normal { get; set; }
        public string method_contain { get; set; }
        public string method_special { get; set; }
        public string product_path { get; set; }
        public string amount { get; set; }
        public string status_approve = "รออนุมัติ";
        public string user_id { get; set; }
    }

    #endregion
}