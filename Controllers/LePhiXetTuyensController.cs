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
            var lePhi = db.LePhiXetTuyens.Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).Select(s => new
            {
                Lpxt_SoTienDong = s.Lpxt_SoTienDong,
                Lpxt_TrangThai = s.Lpxt_TrangThai,
                Lpxt_MinhChung = s.Lpxt_MinhChung,
                Lpxt_GhiChu = s.Lpxt_GhiChu
            }).FirstOrDefault();
            var dataList = nguyenvongs.Select(s => new
            {
                Dkxt_NguyenVong = s.Dkxt_NguyenVong,
                Nganh_ID = s.Nganh_ID,
                Dkxt_Nganh = new
                {
                    Nganh_TenNganh = s.Nganh.NganhTenNganh
                }
            });
            return Json(new { success = true, data = dataList, lePhi = lePhi }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadLePhiMinhChung()
        {
            try
            {
                var session = Session["login_session"].ToString();
                var thiSinh = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc).FirstOrDefault();
                var lePhi = db.LePhiXetTuyens.Where(n => n.ThiSinh_ID == thiSinh.ThiSinh_ID).FirstOrDefault();
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
                    if (lePhi != null)
                    {
                        lePhi.Lpxt_MinhChung = minhchungs;
                        lePhi.Lpxt_TrangThai = 1;
                        db.SaveChanges();
                    }
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
