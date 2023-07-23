using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using HDU_AppXetTuyen.Models;
using HDU_AppXetTuyen.Ultils;
using Newtonsoft.Json;
using PagedList;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class LienCapTHCSController : Controller
    {
        private DbConnecttion db = new DbConnecttion();
        public string DEFAULT_URL = Constant.DEFAULT_URL;

        // GET: Admin/LienCapTHCS
        [AdminSessionCheck]
        public ActionResult Index(string filteriDotxt, string filteriNvong, string filteriNganh, string filteriLePhi, string filteriHoSo, string currentFilter, string searchString, int? page)
        {    

            List<LienCapTHCS> model = new List<LienCapTHCS>();

            foreach (LienCapTHCS s in db.LienCapTHCSs.ToList().OrderByDescending(x => x.HocSinh_ID))
            {
                LienCapTHCS item_prs = new LienCapTHCS();
                item_prs.HocSinh_ID = s.HocSinh_ID;
                item_prs.HocSinh_DinhDanh = s.HocSinh_DinhDanh;
                item_prs.HocSinh_HoTen = s.HocSinh_HoTen;
                item_prs.HocSinh_GioiTinh = s.HocSinh_GioiTinh;
                item_prs.HocSinh_NgaySinh = s.HocSinh_NgaySinh;
                item_prs.HocSinh_NoiSinh = s.HocSinh_NoiSinh;
                item_prs.HocSinh_Email = s.HocSinh_Email;
                item_prs.HocSinh_NoiCuTru = s.HocSinh_NoiCuTru;
                item_prs.HocSinh_TruongTH = s.HocSinh_TruongTH;
                item_prs.HocSinh_UuTien = s.HocSinh_UuTien;              
                item_prs.HocSinh_MucDoNangLuc = s.HocSinh_MucDoNangLuc;
                item_prs.HocSinh_MucDoPhamChat = s.HocSinh_MucDoPhamChat;
                item_prs.HocSinh_MinhChungHB = s.HocSinh_MinhChungHB;
                item_prs.HocSinh_MinhChungGiayKS = s.HocSinh_MinhChungGiayKS;
                item_prs.HocSinh_MinhChungMaDinhDanh = s.HocSinh_MinhChungMaDinhDanh;
                item_prs.HocSinh_GiayUuTien = s.HocSinh_GiayUuTien;
                item_prs.HocSinh_XacNhanLePhi = s.HocSinh_XacNhanLePhi;
                item_prs.HocSinh_TrangThai = s.HocSinh_TrangThai;
                item_prs.HocSinh_GhiChu = s.HocSinh_GhiChu;
                item_prs.HocSinh_Activation = s.HocSinh_Activation;
                item_prs.PhBo = JsonConvert.DeserializeObject<PhuHuynh>(s.HocSinh_ThongTinCha);
                item_prs.PhMe = JsonConvert.DeserializeObject<PhuHuynh>(s.HocSinh_ThongTinMe);
                item_prs.Monhocs = JsonConvert.DeserializeObject<MonHocTHCS>(s.HocSinh_DiemHocTap);
                item_prs.TongDiem = item_prs.Monhocs.Toan + item_prs.Monhocs.TiengViet + item_prs.Monhocs.TuNhien + item_prs.Monhocs.LichSuDiaLy + item_prs.Monhocs.TiengAnh;
                model.Add(item_prs);
            }

            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/LienCapTHCS/Details/5
        [AdminSessionCheck]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTHCS lienCapTHCS = db.LienCapTHCSs.Find(id);
            if (lienCapTHCS == null)
            {
                return HttpNotFound();
            }
            ViewBag.URL = DEFAULT_URL;
            return View(lienCapTHCS);
        }

        // GET: Admin/LienCapTHCS/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/LienCapTHCS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HocSinh_ID,HocSinh_DinhDanh,HocSinh_HoTen,HocSinh_GioiTinh,HocSinh_NgaySinh,HocSinh_NoiSinh,HocSinh_Email,HocSinh_NoiCuTru,HocSinh_TruongTH,HocSinh_UuTien,HocSinh_ThongTinCha,HocSinh_ThongTinMe,HocSinh_DiemHocTap,HocSinh_MucDoNangLuc,HocSinh_MucDoPhamChat,HocSinh_MinhChungHB,HocSinh_MinhChungGiayKS,HocSinh_MinhChungMaDinhDanh,HocSinh_GiayUuTien,HocSinh_XacNhanLePhi,HocSinh_Activation,HocSinh_TrangThai,HocSinh_GhiChu")] LienCapTHCS lienCapTHCS)
        {
            if (ModelState.IsValid)
            {
                db.LienCapTHCSs.Add(lienCapTHCS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lienCapTHCS);
        }

        // GET: Admin/LienCapTHCS/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTHCS lienCapTHCS = db.LienCapTHCSs.Find(id);
            if (lienCapTHCS == null)
            {
                return HttpNotFound();
            }
            return View(lienCapTHCS);
        }

        // POST: Admin/LienCapTHCS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminSessionCheck]
        public ActionResult Edit([Bind(Include = "HocSinh_ID,HocSinh_DinhDanh,HocSinh_HoTen,HocSinh_GioiTinh,HocSinh_NgaySinh,HocSinh_NoiSinh,HocSinh_Email,HocSinh_NoiCuTru,HocSinh_TruongTH,HocSinh_UuTien,HocSinh_ThongTinCha,HocSinh_ThongTinMe,HocSinh_DiemHocTap,HocSinh_MucDoNangLuc,HocSinh_MucDoPhamChat,HocSinh_MinhChungHB,HocSinh_MinhChungGiayKS,HocSinh_MinhChungMaDinhDanh,HocSinh_GiayUuTien,HocSinh_XacNhanLePhi,HocSinh_Activation,HocSinh_TrangThai,HocSinh_GhiChu")] LienCapTHCS lienCapTHCS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lienCapTHCS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lienCapTHCS);
        }

        // GET: Admin/LienCapTHCS/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTHCS lienCapTHCS = db.LienCapTHCSs.Find(id);
            if (lienCapTHCS == null)
            {
                return HttpNotFound();
            }
            return View(lienCapTHCS);
        }

        // POST: Admin/LienCapTHCS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AdminSessionCheck]
        public ActionResult DeleteConfirmed(long id)
        {
            LienCapTHCS lienCapTHCS = db.LienCapTHCSs.Find(id);
            db.LienCapTHCSs.Remove(lienCapTHCS);
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
