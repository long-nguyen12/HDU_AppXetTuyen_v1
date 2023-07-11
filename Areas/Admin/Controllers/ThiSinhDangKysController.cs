using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HDU_AppXetTuyen.Models;
using PagedList;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class ThiSinhDangKysController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/ThiSinhDangKys
        [AdminSessionCheck]
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var thisinhs = (from h in db.ThiSinhDangKies
                          select h).OrderBy(x => x.ThiSinh_ID).Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(thisinhs.ToPagedList(pageNumber, pageSize));
        }

        [AdminSessionCheck]

        public ActionResult ThongTinThiSinh(long? id)
        {
            if (id != null)
            {
                var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_ID == id).Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc).FirstOrDefault();
                return View(thiSinh);
            }
            ThiSinhDangKy ts = new ThiSinhDangKy();
            return View(ts);
        }

        [AdminSessionCheck]
        public JsonResult GetAllNguyenVongs(int? id)
        {
            try
            {
                var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_ID == id).Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc).FirstOrDefault();
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
            catch (Exception e)
            {
                return Json(new
                {
                    success = false
                });
            }
        }

        // GET: Admin/ThiSinhDangKys/Details/5
        [AdminSessionCheck]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThiSinhDangKy thiSinhDangKy = db.ThiSinhDangKies.Find(id);
            if (thiSinhDangKy == null)
            {
                return HttpNotFound();
            }
            return View(thiSinhDangKy);
        }

        // GET: Admin/ThiSinhDangKys/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten");
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten");
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten");
            return View();
        }

        // POST: Admin/ThiSinhDangKys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ThiSinh_ID,ThiSinh_CCCD,ThiSinh_MatKhau,ThiSinh_DienThoai,ThiSinh_Email,ThiSinh_HoLot,ThiSinh_Ten,ThiSinh_NgaySinh,ThiSinh_NamTotNghiep,ThiSinh_ResetCode,ThiSinh_NgayDangKy,ThiSinh_DanToc,ThiSinh_GioiTinh,ThiSinh_DCNhanGiayBao,ThiSinh_HoKhauThuongTru,ThiSinh_HoKhauThuongTru_Check,KhuVuc_ID,DoiTuong_ID,ThiSinh_TruongCapBa_Ma,ThiSinh_TruongCapBa,DotXT_ID,ThiSinh_TrangThai,ThiSinh_TruongCapBa_Tinh_ID,ThiSinh_GhiChu")] ThiSinhDangKy thiSinhDangKy)
        {
            if (ModelState.IsValid)
            {
                db.ThiSinhDangKies.Add(thiSinhDangKy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten", thiSinhDangKy.DoiTuong_ID);
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", thiSinhDangKy.DotXT_ID);
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten", thiSinhDangKy.KhuVuc_ID);
            return View(thiSinhDangKy);
        }

        // GET: Admin/ThiSinhDangKys/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThiSinhDangKy thiSinhDangKy = db.ThiSinhDangKies.Find(id);
            if (thiSinhDangKy == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten", thiSinhDangKy.DoiTuong_ID);
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", thiSinhDangKy.DotXT_ID);
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten", thiSinhDangKy.KhuVuc_ID);
            return View(thiSinhDangKy);
        }

        // POST: Admin/ThiSinhDangKys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ThiSinh_ID,ThiSinh_CCCD,ThiSinh_MatKhau,ThiSinh_DienThoai,ThiSinh_Email,ThiSinh_HoLot,ThiSinh_Ten,ThiSinh_NgaySinh,ThiSinh_NamTotNghiep,ThiSinh_ResetCode,ThiSinh_NgayDangKy,ThiSinh_DanToc,ThiSinh_GioiTinh,ThiSinh_DCNhanGiayBao,ThiSinh_HoKhauThuongTru,ThiSinh_HoKhauThuongTru_Check,KhuVuc_ID,DoiTuong_ID,ThiSinh_TruongCapBa_Ma,ThiSinh_TruongCapBa,DotXT_ID,ThiSinh_TrangThai,ThiSinh_TruongCapBa_Tinh_ID,ThiSinh_GhiChu")] ThiSinhDangKy thiSinhDangKy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thiSinhDangKy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten", thiSinhDangKy.DoiTuong_ID);
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", thiSinhDangKy.DotXT_ID);
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten", thiSinhDangKy.KhuVuc_ID);
            return View(thiSinhDangKy);
        }

        // GET: Admin/ThiSinhDangKys/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThiSinhDangKy thiSinhDangKy = db.ThiSinhDangKies.Find(id);
            if (thiSinhDangKy == null)
            {
                return HttpNotFound();
            }
            return View(thiSinhDangKy);
        }

        // POST: Admin/ThiSinhDangKys/Delete/5
        [AdminSessionCheck]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ThiSinhDangKy thiSinhDangKy = db.ThiSinhDangKies.Find(id);
            db.ThiSinhDangKies.Remove(thiSinhDangKy);
            db.SaveChanges();
            return RedirectToAction("Index");
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
