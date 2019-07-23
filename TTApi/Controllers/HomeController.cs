using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
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

        public SqlConnection ConnectDatabaseTT1995()
        {
            string conn = ConfigurationManager.ConnectionStrings["dbconnectiontt1995"].ConnectionString;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            return connection;
        }
    }
}
