using HDU_AppXetTuyen.Models;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HDU_AppXetTuyen.Controllers
{
    public class DangKyXetTuyenThangController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: DangKyXetTuyenThang
        [ThiSinhSessionCheck]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllKhoiNganh()
        {
            var khoinganhs = db.KhoiNganhs.Select(n => new
            {
                KhoiNganh_ID = n.KhoiNganh_ID,
                KhoiNganh_Ten = n.KhoiNganh_Ten
            });
            return Json(khoinganhs.ToList(), JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllNganhs(string idKhoiNganh)
        {
            int id = int.Parse(idKhoiNganh);
            var nganhs = db.Nganhs.Where(n => n.KhoiNganh_ID == id).Select(n => new
            {
                Nganh_ID = n.Nganh_ID,
                Nganh_MaNganh = n.Nganh_MaNganh,
                NganhTenNganh = n.NganhTenNganh
            });
            if (nganhs != null)
                return Json(new { success = true, data = nganhs.ToList() }, JsonRequestBehavior.AllowGet);
            else return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [ThiSinhSessionCheck]
        public JsonResult GetAllNguyenVongs()
        {
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).Select(n => new
            {
                ThiSinh_ID = n.ThiSinh_ID,
                ThiSinh_HocLucLop12 = n.ThiSinh_HocLucLop12,
                ThiSinh_HanhKiemLop12 = n.ThiSinh_HanhKiemLop12
            }).FirstOrDefault();

            return Json(new
            {
                success = true,
                thisinh = ts,
                data = "",
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadFiles()
        {
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).FirstOrDefault();
            var formName = Request.Form["formName"];
            var recordId = Request.Form["recordId"];
            string savePath = "Uploads/UploadMinhChungs/";
            string fileNameStored = "";
            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = ts.ThiSinh_CCCD + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + fileName;
                    var filePath = Path.Combine(Server.MapPath("~/" + savePath), fileName);
                    var savedPath = savePath + fileName;
                    if (i != Request.Files.Count - 1)
                    {
                        fileNameStored = fileNameStored + savedPath + "#";
                    }
                    else
                    {
                        fileNameStored += savedPath;
                    }
                    file.SaveAs(filePath);
                }

                return Json(new { success = true, message = fileNameStored });
            }

            return Json(new { success = false, message = "No files were uploaded." });
        }
    }
}