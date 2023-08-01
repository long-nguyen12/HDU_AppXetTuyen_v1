using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using HDU_AppXetTuyen.Models;

namespace HDU_AppXetTuyen.Controllers
{
    public class KinhPhiInfo
    {
        public string KinhPhi_SoTC { get; set; }
        public string KinhPhi_TepMinhChung { get; set; }
        public string key_dkxt_id { get; set; }
        public string key_dkxt_pt { get; set; }
    }

    public class LePhiXetTuyensController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: LePhiXetTuyens
        [ThiSinhSessionCheck]
        public ActionResult Index()
        {
            var lePhiXetTuyens = db.LePhiXetTuyens.Include(l => l.ThiSinhDangKy);
            return View(lePhiXetTuyens.ToList());
        }

        [HttpGet]
        [ThiSinhSessionCheck]
        public JsonResult GetAllLePhi()
        {
            var session = Session["login_session"].ToString();
            var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc).FirstOrDefault();
            var nguyenvongs = db.DangKyXetTuyens.Include(l => l.Nganh).Include(l => l.ToHopMon).Include(l => l.DotXetTuyen).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
            {
                Ptxt_ID = n.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.NganhTenNganh
                },
                Thm = new
                {
                    ToHopMon = n.ToHopMon.Thm_MaToHop
                },
                Dkxt_ID = n.Dkxt_ID,
                Dkxt_TrangThai = n.Dkxt_TrangThai,
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                DotXT_ID = n.DotXT_ID,
                Dkxt_Diem_Tong = n.Dkxt_Diem_Tong,
                Dkxt_Diem_Tong_Full = n.Dkxt_Diem_Tong_Full,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();
            var nguyenVongTuyenThang = db.DangKyXetTuyenThangs.Include(l => l.Nganh).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
            {
                Ptxt_ID = n.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.NganhTenNganh
                },
                Dkxt_ID = n.Dkxt_ID,
                Dkxt_TrangThai = n.Dkxt_TrangThai,
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                DotXT_ID = n.DotXT_ID,
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_MonDatGiai = n.Dkxt_MonDatGiai,
                Dkxt_NamDatGiai = n.Dkxt_NamDatGiai,
                Dkxt_LoaiGiai = n.Dkxt_LoaiGiai,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();
            var nguyenVongNgoaiNgu = db.DangKyXetTuyenKhacs.Include(l => l.Nganh).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP5")).Select(n => new
            {
                Ptxt_ID = n.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.NganhTenNganh
                },
                Dkxt_ID = n.Dkxt_ID,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                Dkxt_TrangThai = n.Dkxt_TrangThai,
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                DotXT_ID = n.DotXT_ID,
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                Dkxt_TongDiem = n.Dkxt_TongDiem,
                Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();
            var nguyenVongDanhGia = db.DangKyXetTuyenKhacs.Include(l => l.Nganh).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP6")).Select(n => new
            {
                Ptxt_ID = n.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.NganhTenNganh
                },
                Dkxt_ID = n.Dkxt_ID,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                Dkxt_TrangThai = n.Dkxt_TrangThai,
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                DotXT_ID = n.DotXT_ID,
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                Dkxt_TongDiem = n.Dkxt_TongDiem,
                Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            return Json(new
            {
                success = true,
                HocBa = nguyenvongs,
                TuyenThang = nguyenVongTuyenThang,
                NgoaiNgu = nguyenVongNgoaiNgu,
                DanhGia = nguyenVongDanhGia
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadLePhiMinhChung()
        {
            try
            {
                var session = Session["login_session"];
                var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).FirstOrDefault();

                string minhchungs = "";
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];
                        var fileName = Path.GetFileName(file.FileName);
                        fileName = thiSinh.ThiSinh_CCCD + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + fileName;
                        var filePath = Path.Combine(Server.MapPath("~/Uploads/UploadMinhChungs"), fileName);
                        var savePath = "Uploads/UploadMinhChungs/" + fileName;
                        if (i != Request.Files.Count - 1)
                        {
                            minhchungs = minhchungs + savePath + "#";
                        }
                        else
                        {
                            minhchungs += savePath;
                        }
                        file.SaveAs(filePath);
                    }
                    return Json(new { success = true, message = minhchungs }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, data = Request.Files.Count }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Create(KinhPhiInfo kp)
        {
            var session = Session["login_session"];
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session.ToString())).FirstOrDefault();
            var Dkxt_ID = int.Parse(kp.key_dkxt_id);
            var Ptxt_ID = int.Parse(kp.key_dkxt_pt);
            var kinhPhi = db.KinhPhis.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID && n.Ptxt_ID == Ptxt_ID && n.Dkxt_ID == Dkxt_ID).FirstOrDefault();
            if (kinhPhi != null)
            {
                kinhPhi.KinhPhi_TepMinhChung = kp.KinhPhi_TepMinhChung;
                kinhPhi.KinhPhi_SoTC = kp.KinhPhi_SoTC;
                kinhPhi.KinhPhi_NgayThang = DateTime.Now.ToString("dd/MM/yyyy");
                db.SaveChanges();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
