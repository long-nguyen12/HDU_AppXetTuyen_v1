﻿using System;
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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace HDU_AppXetTuyen.Controllers
{


    public class LePhiXetTuyensController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: LePhiXetTuyens
        [ThiSinhSessionCheck]
        public ActionResult Index()
        {
            //var lePhiXetTuyens = db.LePhiXetTuyens.Include(l => l.ThiSinhDangKy);
            //return View(lePhiXetTuyens.ToList());
            return View();
        }

        [HttpGet]
        [ThiSinhSessionCheck]
        public JsonResult GetAllLePhi()
        {
            var session = Session["login_session"].ToString();
            var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault();

            var nguyenvongTHPTQG = db.DangKyXetTuyenKQTQGs.Include(l => l.Nganh).Include(l => l.Nganh).Include(l => l.ToHopMon).Include(l => l.DotXetTuyen).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
            {
                Ptxt_ID = n.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                Thm = new
                {
                    ThmMaTen = n.ToHopMon.Thm_MaTen,
                },
                Dkxt_ID = n.Dkxt_KQTQG_ID,
                Dkxt_TrangThai = n.Dkxt_KQTQG_TrangThai,
                Dkxt_NguyenVong = n.Dkxt_KQTQG_NguyenVong,
                DotXT_ID = new {TenDotXet ="Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " +  n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_Diem_Tong = n.Dkxt_KQTQG_Diem_Tong,
                Dkxt_Diem_Tong_Full = n.Dkxt_KQTQG_TongDiem_Full,
                Dkxt_TrangThai_KetQua = n.Dkxt_KQTQG_TrangThai_KetQua,
                Dkxt_NgayDK = n.Dkxt_KQTQG_NgayDangKy,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            var nguyenvongHocBa = db.DangKyXetTuyenHBs.Include(l => l.Nganh).Include(l => l.ToHopMon).Include(l => l.DotXetTuyen).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
            {
                Ptxt_ID = n.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                Thm = new
                {
                    ThmMaTen = n.ToHopMon.Thm_MaTen,
                },
                Dkxt_ID = n.Dkxt_HB_ID,
                Dkxt_TrangThai = n.Dkxt_HB_TrangThai,
                Dkxt_NguyenVong = n.Dkxt_HB_NguyenVong,
                DotXT_ID = new { TenDotXet = "Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " + n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_Diem_Tong = n.Dkxt_HB_Diem_Tong,
                Dkxt_Diem_Tong_Full = n.Dkxt_HB_Diem_Tong_Full,
                Dkxt_TrangThai_KetQua = n.Dkxt_HB_TrangThai_KetQua,
                Dkxt_NgayDK = n.Dkxt_HB_NgayDangKy,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            var nguyenVongTuyenThang = db.DangKyXetTuyenThangs.Include(l => l.Nganh).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(n => new
            {
                Ptxt_ID = n.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                Dkxt_ID = n.Dkxt_ID,
                Dkxt_TrangThai = n.Dkxt_TrangThai,
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                DotXT_ID = new { TenDotXet = "Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " + n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_MonDatGiai = n.Dkxt_MonDatGiai,
                Dkxt_NamDatGiai = n.Dkxt_NamDatGiai,
                Dkxt_LoaiGiai = n.Dkxt_LoaiGiai,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                Dkxt_NgayDK = n.Dkxt_NgayDangKy,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            var nguyenVongNgoaiNgu = db.DangKyXetTuyenKhacs.Include(x => x.Nganh).Include(x => x.ChungChi).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP5")).Select(n => new
            {
                Ptxt_ID = n.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },
                ChungChi_ID = new { Ten = n.ChungChi.ChungChi_Ten },
                Dkxt_ID = n.Dkxt_ID,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                Dkxt_TrangThai = n.Dkxt_TrangThai,
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                DotXT_ID = new { TenDotXet = "Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " + n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                Dkxt_TongDiem = n.Dkxt_TongDiem,
                Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
                Dkxt_NgayDK = n.Dkxt_NgayDangKy,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();
            //var nguyenVongNgoaiNgu = db.DangKyXetTuyenKhacs.Include(x => x.Nganh).Include(x => x.ChungChi).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP5")).Select(n => new
            var nguyenVongDanhGia = db.DangKyXetTuyenKhacs.Include(x => x.Nganh).Include(x => x.ChungChi).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID && n.Dkxt_ToHopXT.Equals("HDP6")).Select(n => new
            {
                Ptxt_ID = n.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = n.Nganh.Nganh_MaNganh,
                    NganhTenNganh = n.Nganh.Nganh_TenNganh
                },

                ChungChi_ID = new { Ten = n.ChungChi.ChungChi_Ten },

                Dkxt_ID = n.Dkxt_ID,
                Dkxt_TrangThai_KetQua = n.Dkxt_TrangThai_KetQua,
                Dkxt_TrangThai = n.Dkxt_TrangThai,
                Dkxt_NguyenVong = n.Dkxt_NguyenVong,
                DotXT_ID = new { TenDotXet = "Từ " + n.DotXetTuyen.Dxt_ThoiGian_BatDau + " đến " + n.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_ToHopXT = n.Dkxt_ToHopXT,
                Dkxt_DonViToChuc = n.Dkxt_DonViToChuc,
                Dkxt_KetQuaDatDuoc = n.Dkxt_KetQuaDatDuoc,
                Dkxt_TongDiem = n.Dkxt_TongDiem,
                Dkxt_NgayDuThi = n.Dkxt_NgayDuThi,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            var dangkyduthiNK = db.DangKyDuThiNangKhieus.Include(x => x.Nganh).Include(x => x.ToHopMon).Include(x => x.DotXetTuyen).Where(x => x.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(x => new
            {
                Ptxt_ID = x.Ptxt_ID,
                Nganh_ID = new
                {
                    Nganh_MaNganh = x.Nganh.Nganh_MaNganh,
                    NganhTenNganh = x.Nganh.Nganh_TenNganh
                },
                Thm = new
                {
                    ToHopMon = x.ToHopMon.Thm_MaToHop
                },
                Dkxt_ID = x.Dkdt_NK_ID,
                Dkxt_TrangThai = x.Dkdt_NK_TrangThai,
                Dkxt_NguyenVong = x.Dkdt_NK_NguyenVong,
                
                DotXT_ID = new { TenDotXet = "Ngày " + x.DotXetTuyen.Dxt_ThoiGian_KetThuc },
                Dkxt_TrangThai_KetQua = x.Dkdt_NK_TrangThai_KetQua,
            }).OrderBy(n => n.Dkxt_NguyenVong).ToList();

            return Json(new
            {
                success = true,
                TrungHocPT = nguyenvongTHPTQG,
                HocBa = nguyenvongHocBa,
                TuyenThang = nguyenVongTuyenThang,
                NgoaiNgu = nguyenVongNgoaiNgu,
                DanhGia = nguyenVongDanhGia,
                DuThiNK = dangkyduthiNK
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
                kinhPhi.KinhPhi_TrangThai = 1;
                kinhPhi.KinhPhi_NgayThang_NopMC = DateTime.Now.ToString("dd/MM/yyyy");
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
