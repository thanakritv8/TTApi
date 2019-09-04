using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTApi.Models;

namespace TTApi.Controllers
{
    public class ReportController : Controller
    {
        #region ActionExportReport
        public ActionResult ExportWorkSheet(string id, string name_report)
        {
            string name_dataset = string.Empty;
            if(name_report == "Report1")
            {
                name_dataset = "DataSetGetReport1";
            }
            else if (name_report == "Report2")
            {
                name_dataset = "DataSetGetReport2";
            }
            else if (name_report == "Report3")
            {
                name_dataset = "DataSetGetReport3";
            }
            //link use Home/ExportWorkSheet?id=2
            ReportDataSource rds = new ReportDataSource(name_dataset, GetReport(id, name_report));
            LocalReport report = new LocalReport();
            report.ReportPath = Path.Combine(Server.MapPath("~/Reports"), name_report + ".rdlc");
            report.DataSources.Clear();
            //report.SetParameters(new ReportParameter[] { parameter });
            report.DataSources.Add(rds);
            report.Refresh();
            PrintPDF(report);
            return View();
        }

        private void PrintPDF(LocalReport report)
        {
            string FileName = "temp.pdf";
            string extension;
            string encoding;
            string mimeType;
            string[] streams;
            Warning[] warnings;
            Byte[] mybytes = report.Render("PDF", null,
                          out extension, out encoding,
                          out mimeType, out streams, out warnings);
            using (FileStream fs = new FileStream(Server.MapPath("~/Reports/" + FileName), FileMode.Create))
            {
                fs.Write(mybytes, 0, mybytes.Length);
            }
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.Clear();
            Response.Charset = "";
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + FileName + "\"");
            Response.WriteFile(Server.MapPath("~/Reports/" + FileName));

            Response.Flush();
            System.IO.File.Delete(Server.MapPath("~/Reports/" + FileName));
            Response.Close();
            Response.End();
        }

        private DataTable GetReport(string id, string name_report)
        {
            if (name_report == "Report1")
            {
                Models.DataSetGetReport1TableAdapters.sp_GetReport1TableAdapter da = new Models.DataSetGetReport1TableAdapters.sp_GetReport1TableAdapter();
                DataSetGetReport1.sp_GetReport1DataTable dt = new DataSetGetReport1.sp_GetReport1DataTable();
                da.Fill(dt, id);
                return dt;
            }
            else if(name_report == "Report2")
            {
                Models.DataSetGetReport2TableAdapters.sp_GetReport2TableAdapter da = new Models.DataSetGetReport2TableAdapters.sp_GetReport2TableAdapter();
                DataSetGetReport2.sp_GetReport2DataTable dt = new DataSetGetReport2.sp_GetReport2DataTable();
                da.Fill(dt, id);
                return dt;
            }
            else if(name_report == "Report3")
            {
                Models.DataSetGetReport3TableAdapters.sp_GetReport3TableAdapter da = new Models.DataSetGetReport3TableAdapters.sp_GetReport3TableAdapter();
                DataSetGetReport3.sp_GetReport3DataTable dt = new DataSetGetReport3.sp_GetReport3DataTable();
                da.Fill(dt, id);
                return dt;
            }
            else
            {
                return new DataTable();
            }
                       
        }
        #endregion

        #region CheckList
        public ActionResult CheckListWorkSheet(string id, string name_report)
        {
            string name_dataset = string.Empty;
            if (name_report == "Report4")
            {
                name_dataset = "DataSetWorkSheet";
            }

            //link use Report/ExportWorkSheet?id=2
            ReportDataSource rds = new ReportDataSource(name_dataset, GetReportWorkSheet(id, name_report));
            LocalReport report = new LocalReport();
            report.ReportPath = Path.Combine(Server.MapPath("~/Reports"), name_report + ".rdlc");
            report.DataSources.Clear();
            report.DataSources.Add(rds);
            report.Refresh();
            PrintPDF(report);
            return View();
        }

        private DataTable GetReportWorkSheet(string id, string name_report)
        {
            if (name_report == "Report4")
            {
                Models.DataSetWorkSheetTableAdapters.sp_WorkSheetTableAdapter da = new Models.DataSetWorkSheetTableAdapters.sp_WorkSheetTableAdapter();
                DataSetWorkSheet.sp_WorkSheetDataTable dt = new DataSetWorkSheet.sp_WorkSheetDataTable();
                da.Fill(dt, id);
                return dt;
            }            
            else
            {
                return new DataTable();
            }

        }
        
        #endregion
    }
}