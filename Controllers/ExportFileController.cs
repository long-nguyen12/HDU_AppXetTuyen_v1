using HDU_AppXetTuyen.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xceed.Words.NET;

namespace HDU_AppXetTuyen.Controllers
{
    public class ExportFileController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: ExportFile
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DownloadFile_XetTuyenHB(string idThiSinh)
        {
            // HARCODE 
            //var idThiSinhInt = int.Parse(idThiSinh);
            var idThiSinhInt = 1;
            string templateFilePath = Server.MapPath("~/Content/static/Mau_HB.docx");
            // Từ idDkxt lấy ra thông tin thí sinh
            var thiSinhInfo = db.ThiSinhDangKies.Find(idThiSinhInt);
            // Từ id thí sinh lấy ra tất cả nguyện vọng đăng ký xét tuyển của thí sinh
            var listDkxt = db.DangKyXetTuyens.Where(x => x.ThiSinh_ID == idThiSinhInt).ToArray();
            // Map các list đăng ký xét tuyển lấy ra thông tin chi tiết  của mỗi bản ghi
            //foreach(var item in listDkxt)
            //{
            //    thông tin chi tiết
            //}
            using (DocX document = DocX.Load(templateFilePath))
            {
                // Replace placeholders with actual data
                string gt = thiSinhInfo.ThiSinh_GioiTinh == 0 ? "Nam" : "Nữ";
                // Replace placeholders with actual data
                document.ReplaceText("<<ThiSinh_HoLot>>", thiSinhInfo.ThiSinh_HoLot);
                document.ReplaceText("<<ThiSinh_Ten>>", thiSinhInfo.ThiSinh_Ten);
                document.ReplaceText("<<ThiSinh_CCCD>>", thiSinhInfo.ThiSinh_CCCD);
                document.ReplaceText("<<ThiSinh_NgaySinh>>", thiSinhInfo.ThiSinh_NgaySinh);
                document.ReplaceText("<<ThiSinh_GioiTinh>>", gt);
                document.ReplaceText("<<ThiSinh_DanToc>>", thiSinhInfo.ThiSinh_DanToc);
                document.ReplaceText("<<ThiSinh_DienThoai>>", thiSinhInfo.ThiSinh_DienThoai);
                // Add more ReplaceText statements for each placeholder you want to fill

                // Generate a unique file name
                string fileName = thiSinhInfo.ThiSinh_Ten + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".docx";

                // Save the filled document to a temporary location on the server
                string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
                document.SaveAs(tempFilePath);

                // Return the file for download
                byte[] fileBytes = System.IO.File.ReadAllBytes(tempFilePath);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }
        }
    }
}