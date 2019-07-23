using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTApi.Controllers
{
    public class ReportController : Controller
    {
        #region ActionExportReport
        public ActionResult ExportWorkSheet(string id, string name_report)
        {
            //link use Home/ExportWorkSheet?id=2
            ReportDataSource rds = new ReportDataSource("DataSetGetWorkSheet", GetWeekSheetReport(id));
            LocalReport report = new LocalReport();
            report.ReportPath = Path.Combine(Server.MapPath("~/Report"), name_report + ".rdlc");
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
            using (FileStream fs = new FileStream(Server.MapPath("~/Report/" + FileName), FileMode.Create))
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
            Response.WriteFile(Server.MapPath("~/Report/" + FileName));

            Response.Flush();
            System.IO.File.Delete(Server.MapPath("~/Report/" + FileName));
            Response.Close();
            Response.End();
        }

        private DataTable GetWeekSheetReport(string id)
        {
            //Models.Home.DataSetGetWorkSheetTableAdapters.sp_GetWorkSheetReportTableAdapter da = new Models.Home.DataSetGetWorkSheetTableAdapters.sp_GetWorkSheetReportTableAdapter();
            //DataSetGetWorkSheet.sp_GetWorkSheetReportDataTable dt = new DataSetGetWorkSheet.sp_GetWorkSheetReportDataTable();
            //da.Fill(dt, id);
            //return dt;
            return new DataTable();
        }
        #endregion
    }
}