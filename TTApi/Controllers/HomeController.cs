using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace TTApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public SqlConnection ConnectDatabase()
        {
            string conn = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            return connection;
        }

        public SqlConnection ConnectDatabaseAuth()
        {
            string conn = ConfigurationManager.ConnectionStrings["dbconnectionAuth"].ConnectionString;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            return connection;
        }

        public SqlConnection ConnectDatabaseTT1995()
        {
            string conn = ConfigurationManager.ConnectionStrings["dbconnectiontt1995"].ConnectionString;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            return connection;
        }

        public string EncryptSHA256Managed(string StrInput)
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytClearString = uEncode.GetBytes(StrInput);
            System.Security.Cryptography.SHA256Managed sha = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha.ComputeHash(bytClearString);
            return Convert.ToBase64String(hash);
        }
            
    }
}
