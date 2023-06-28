﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HDU_AppXetTuyen.Models;

namespace HDU_AppXetTuyen.Controllers
{
    public class ThiSinhDangKiesController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: ThiSinhDangKies
        public ActionResult Index()
        {
            var thiSinhDangKies = db.ThiSinhDangKies.Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc);
            return View(thiSinhDangKies.ToList());
        }

        // GET: ThiSinhDangKies/Details/5
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

        // GET: ThiSinhDangKies/Create
        public ActionResult Create()
        {
            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten");
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten");
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten");
            return View();
        }

        // POST: ThiSinhDangKies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ThiSinh_ID,ThiSinh_CCCD,ThiSinh_MatKhau,ThiSinh_DienThoai,ThiSinh_Email,ThiSinh_HoLot,ThiSinh_Ten,ThiSinh_NgaySinh,ThiSinh_NamTotNghiep,ThiSinh_ResetCode,ThiSinh_NgayDangKy,ThiSinh_DanToc,ThiSinh_GioiTinh,ThiSinh_DCNhanGiayBao,ThiSinh_HoKhauThuongTru,ThiSinh_HoKhauThuongTru_Check,KhuVuc_ID,DoiTuong_ID,ThiSinh_TruongCapBa_Ma,ThiSinh_TruongCapBa,DotXT_ID,ThiSinh_TrangThai,ThiSinh_GhiChu")] ThiSinhDangKy thiSinhDangKy)
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

        // GET: ThiSinhDangKies/Edit/5
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

        // POST: ThiSinhDangKies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ThiSinh_ID,ThiSinh_CCCD,ThiSinh_MatKhau,ThiSinh_DienThoai,ThiSinh_Email,ThiSinh_HoLot,ThiSinh_Ten,ThiSinh_NgaySinh,ThiSinh_NamTotNghiep,ThiSinh_ResetCode,ThiSinh_NgayDangKy,ThiSinh_DanToc,ThiSinh_GioiTinh,ThiSinh_DCNhanGiayBao,ThiSinh_HoKhauThuongTru,ThiSinh_HoKhauThuongTru_Check,KhuVuc_ID,DoiTuong_ID,ThiSinh_TruongCapBa_Ma,ThiSinh_TruongCapBa,DotXT_ID,ThiSinh_TrangThai,ThiSinh_GhiChu")] ThiSinhDangKy thiSinhDangKy)
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

        // GET: ThiSinhDangKies/Delete/5
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

        // POST: ThiSinhDangKies/Delete/5
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
