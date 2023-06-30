﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using HDU_AppXetTuyen.Models;
using PagedList;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class DangKyXetTuyensController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/DangKyXetTuyens
        [AdminSessionCheck]
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var dangKyXetTuyens = (from h in db.DangKyXetTuyens
                          select h).OrderBy(x => x.Dkxt_ID).Include(d => d.DoiTuong).Include(d => d.DotXetTuyen).Include(d => d.KhuVuc).Include(d => d.Nganh).Include(d => d.PhuongThucXetTuyen).Include(d => d.ThiSinhDangKy).Include(d => d.ToHopMon);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(dangKyXetTuyens.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/DangKyXetTuyens/Details/5
        [AdminSessionCheck]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyXetTuyen dangKyXetTuyen = db.DangKyXetTuyens.Find(id);
            if (dangKyXetTuyen == null)
            {
                return HttpNotFound();
            }
            return View(dangKyXetTuyen);
        }

        // GET: Admin/DangKyXetTuyens/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten");
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten");
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten");
            ViewBag.Nganh_ID = new SelectList(db.Nganhs, "Nganh_ID", "Nganh_MaNganh");
            ViewBag.Ptxt_ID = new SelectList(db.PhuongThucXetTuyens, "Ptxt_ID", "Ptxt_TenPhuongThuc");
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD");
            ViewBag.Thm_ID = new SelectList(db.ToHopMons, "Thm_ID", "Thm_MaToHop");
            return View();
        }

        // POST: Admin/DangKyXetTuyens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Dkxt_ID,ThiSinh_ID,Ptxt_ID,Nganh_ID,Thm_ID,DoiTuong_ID,KhuVuc_ID,Dkxt_TrangThai,Dkxt_NguyenVong,DotXT_ID,Dkxt_GhiChu,Dkxt_Diem_M1,Dkxt_Diem_M2,Dkxt_Diem_M3,Dkxt_Diem_Tong,Dkxt_Diem_Tong_Full")] DangKyXetTuyen dangKyXetTuyen)
        {
            if (ModelState.IsValid)
            {
                db.DangKyXetTuyens.Add(dangKyXetTuyen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten", dangKyXetTuyen.DoiTuong_ID);
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", dangKyXetTuyen.DotXT_ID);
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten", dangKyXetTuyen.KhuVuc_ID);
            ViewBag.Nganh_ID = new SelectList(db.Nganhs, "Nganh_ID", "Nganh_MaNganh", dangKyXetTuyen.Nganh_ID);
            ViewBag.Ptxt_ID = new SelectList(db.PhuongThucXetTuyens, "Ptxt_ID", "Ptxt_TenPhuongThuc", dangKyXetTuyen.Ptxt_ID);
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", dangKyXetTuyen.ThiSinh_ID);
            ViewBag.Thm_ID = new SelectList(db.ToHopMons, "Thm_ID", "Thm_MaToHop", dangKyXetTuyen.Thm_ID);
            return View(dangKyXetTuyen);
        }

        // GET: Admin/DangKyXetTuyens/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyXetTuyen dangKyXetTuyen = db.DangKyXetTuyens.Find(id);
            if (dangKyXetTuyen == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten", dangKyXetTuyen.DoiTuong_ID);
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", dangKyXetTuyen.DotXT_ID);
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten", dangKyXetTuyen.KhuVuc_ID);
            ViewBag.Nganh_ID = new SelectList(db.Nganhs, "Nganh_ID", "Nganh_MaNganh", dangKyXetTuyen.Nganh_ID);
            ViewBag.Ptxt_ID = new SelectList(db.PhuongThucXetTuyens, "Ptxt_ID", "Ptxt_TenPhuongThuc", dangKyXetTuyen.Ptxt_ID);
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", dangKyXetTuyen.ThiSinh_ID);
            ViewBag.Thm_ID = new SelectList(db.ToHopMons, "Thm_ID", "Thm_MaToHop", dangKyXetTuyen.Thm_ID);
            return View(dangKyXetTuyen);
        }

        // POST: Admin/DangKyXetTuyens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Dkxt_ID,ThiSinh_ID,Ptxt_ID,Nganh_ID,Thm_ID,DoiTuong_ID,KhuVuc_ID,Dkxt_TrangThai,Dkxt_NguyenVong,DotXT_ID,Dkxt_GhiChu,Dkxt_Diem_M1,Dkxt_Diem_M2,Dkxt_Diem_M3,Dkxt_Diem_Tong,Dkxt_Diem_Tong_Full")] DangKyXetTuyen dangKyXetTuyen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dangKyXetTuyen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoiTuong_ID = new SelectList(db.DoiTuongs, "DoiTuong_ID", "DoiTuong_Ten", dangKyXetTuyen.DoiTuong_ID);
            ViewBag.DotXT_ID = new SelectList(db.DotXetTuyens, "Dxt_ID", "Dxt_Ten", dangKyXetTuyen.DotXT_ID);
            ViewBag.KhuVuc_ID = new SelectList(db.KhuVucs, "KhuVuc_ID", "KhuVuc_Ten", dangKyXetTuyen.KhuVuc_ID);
            ViewBag.Nganh_ID = new SelectList(db.Nganhs, "Nganh_ID", "Nganh_MaNganh", dangKyXetTuyen.Nganh_ID);
            ViewBag.Ptxt_ID = new SelectList(db.PhuongThucXetTuyens, "Ptxt_ID", "Ptxt_TenPhuongThuc", dangKyXetTuyen.Ptxt_ID);
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", dangKyXetTuyen.ThiSinh_ID);
            ViewBag.Thm_ID = new SelectList(db.ToHopMons, "Thm_ID", "Thm_MaToHop", dangKyXetTuyen.Thm_ID);
            return View(dangKyXetTuyen);
        }

        // GET: Admin/DangKyXetTuyens/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyXetTuyen dangKyXetTuyen = db.DangKyXetTuyens.Find(id);
            if (dangKyXetTuyen == null)
            {
                return HttpNotFound();
            }
            return View(dangKyXetTuyen);
        }

        // POST: Admin/DangKyXetTuyens/Delete/5
        [AdminSessionCheck]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            DangKyXetTuyen dangKyXetTuyen = db.DangKyXetTuyens.Find(id);
            db.DangKyXetTuyens.Remove(dangKyXetTuyen);
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
