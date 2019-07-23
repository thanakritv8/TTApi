using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTApi.Models
{
    /// <summary>
    /// ผลการ Execute ใช้ใน insert, update, delete
    /// </summary>
    public class ExecuteModels
    {
        /// <summary>
        /// result : ผล ok = 0, error = 1  
        /// </summary>
        public int result { get; set; }
        /// <summary>
        /// code : ใช้แสดงข้อความจากระบบ
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// id_return : จะมีค่าก็ต่อเมื่อมีการ insert
        /// </summary>
        public string id_return { get; set; }
    }
}