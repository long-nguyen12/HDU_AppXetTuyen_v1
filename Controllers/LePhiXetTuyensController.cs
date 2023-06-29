using System;
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
            var nguyenvongs = db.DangKyXetTuyens.Include(l => l.Nganh).Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).OrderBy(n => n.Dkxt_NguyenVong).ToList();
            var dataList = nguyenvongs.Select(s => new
            {
                Dkxt_NguyenVong = s.Dkxt_NguyenVong,
                Nganh_ID = s.Nganh_ID,
                Dkxt_Nganh = new
                {
                    Nganh_TenNganh = s.Nganh.NganhTenNganh
                }
            });
            return Json(new { success = true, data = dataList }, JsonRequestBehavior.AllowGet);
        }

        // GET: LePhiXetTuyens/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LePhiXetTuyen lePhiXetTuyen = db.LePhiXetTuyens.Find(id);
            if (lePhiXetTuyen == null)
            {
                return HttpNotFound();
            }
            return View(lePhiXetTuyen);
        }

        // GET: LePhiXetTuyens/Create
        public ActionResult Create()
        {
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD");
            return View();
        }

        // POST: LePhiXetTuyens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Lpxt_ID,ThiSinh_ID,Lpxt_SoTienDong,Lpxt_TrangThai,Lpxt_MinhChung,Lpxt_GhiChu")] LePhiXetTuyen lePhiXetTuyen)
        {
            if (ModelState.IsValid)
            {
                db.LePhiXetTuyens.Add(lePhiXetTuyen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", lePhiXetTuyen.ThiSinh_ID);
            return View(lePhiXetTuyen);
        }

        // GET: LePhiXetTuyens/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LePhiXetTuyen lePhiXetTuyen = db.LePhiXetTuyens.Find(id);
            if (lePhiXetTuyen == null)
            {
                return HttpNotFound();
            }
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", lePhiXetTuyen.ThiSinh_ID);
            return View(lePhiXetTuyen);
        }

        // POST: LePhiXetTuyens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Lpxt_ID,ThiSinh_ID,Lpxt_SoTienDong,Lpxt_TrangThai,Lpxt_MinhChung,Lpxt_GhiChu")] LePhiXetTuyen lePhiXetTuyen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lePhiXetTuyen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ThiSinh_ID = new SelectList(db.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", lePhiXetTuyen.ThiSinh_ID);
            return View(lePhiXetTuyen);
        }

        // GET: LePhiXetTuyens/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LePhiXetTuyen lePhiXetTuyen = db.LePhiXetTuyens.Find(id);
            if (lePhiXetTuyen == null)
            {
                return HttpNotFound();
            }
            return View(lePhiXetTuyen);
        }

        // POST: LePhiXetTuyens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            LePhiXetTuyen lePhiXetTuyen = db.LePhiXetTuyens.Find(id);
            db.LePhiXetTuyens.Remove(lePhiXetTuyen);
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
