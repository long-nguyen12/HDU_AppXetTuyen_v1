using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml.Linq;
using HDU_AppXetTuyen.Models;
using PagedList;
using Newtonsoft.Json.Linq;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class NganhsController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/Nganhs
        [AdminSessionCheck]
        public ActionResult Index(int? page)
        {
            
            if (page == null) page = 1;
            var nganhs = (from h in db.Nganhs
                          select h).OrderByDescending(x => x.Nganh_ID).ThenBy(x => x.Khoa.Khoa_TenKhoa).ThenBy(x => x.Nganh_TenNganh). Include(n => n.KhoiNganh).Include(n => n.Khoa).Where(x => x.Nganh_ID >0);
            int pageSize = 7;
            int pageNumber = (page ?? 1);
            return View(nganhs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Nganhs/Details/5
        [AdminSessionCheck]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nganh nganh = db.Nganhs.Find(id);
            if (nganh == null)
            {
                return HttpNotFound();
            }
            return View(nganh);
        }

        // GET: Admin/Nganhs/Create
        [AdminSessionCheck]
        public ActionResult Create()
        {
            ViewBag.KhoiNganh_ID = new SelectList(db.KhoiNganhs, "KhoiNganh_ID", "KhoiNganh_Ten");
            ViewBag.Khoa_ID = new SelectList(db.Khoas, "Khoa_ID", "Khoa_TenKhoa");
            return View();
        }

        // POST: Admin/Nganhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nganh_ID,Nganh_MaNganh,Nganh_TenNganh,Khoa_ID,Nganh_GhiChu,KhoiNganh_ID")] Nganh nganh)
        {
            if (ModelState.IsValid)
            {
                nganh.Nganh_GhiChu = nganh.Nganh_MaNganh + " " + nganh.Nganh_TenNganh;
                db.Nganhs.Add(nganh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KhoiNganh_ID = new SelectList(db.KhoiNganhs, "KhoiNganh_ID", "KhoiNganh_Ten", nganh.KhoiNganh_ID);
            ViewBag.Khoa_ID = new SelectList(db.Khoas, "Khoa_ID", "Khoa_TenKhoa", nganh.Khoa_ID);
            return View(nganh);
        }

        // GET: Admin/Nganhs/Edit/5
        [AdminSessionCheck]
        public ActionResult Edit(int? id)
        {
            var listTHM = db.ToHopMonNganhs.Where(x => x.Nganh_ID == id).ToList();
            int[] arrId_THM = listTHM.Select(thm => thm.Thm_ID).ToArray();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nganh nganh = db.Nganhs.Find(id);
            if (nganh == null)
            {
                return HttpNotFound();
            }
            ViewBag.arrId_THM = arrId_THM;
            ViewBag.Nganh_ID = nganh.Nganh_ID;
            ViewBag.KhoiNganh_ID = new SelectList(db.KhoiNganhs, "KhoiNganh_ID", "KhoiNganh_Ten", nganh.KhoiNganh_ID);
            ViewBag.Khoa_ID = new SelectList(db.Khoas, "Khoa_ID", "Khoa_TenKhoa", nganh.Khoa_ID);
        
            return View(nganh);
        }

        // POST: Admin/Nganhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminSessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Nganh_ID,Nganh_MaNganh,Nganh_TenNganh,Khoa_ID,Nganh_GhiChu,KhoiNganh_ID")] Nganh nganh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nganh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KhoiNganh_ID = new SelectList(db.KhoiNganhs, "KhoiNganh_ID", "KhoiNganh_Ten", nganh.KhoiNganh_ID);
            ViewBag.Khoa_ID = new SelectList(db.Khoas, "Khoa_ID", "Khoa_TenKhoa", nganh.Khoa_ID);
            return View(nganh);
        }

        // GET: Admin/Nganhs/Delete/5
        [AdminSessionCheck]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nganh nganh = db.Nganhs.Find(id);
            if (nganh == null)
            {
                return HttpNotFound();
            }
            return View(nganh);
        }

        // POST: Admin/Nganhs/Delete/5
        [AdminSessionCheck]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Nganh nganh = db.Nganhs.Find(id);
            db.Nganhs.Remove(nganh);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult getAllToHopMon()
        {
            var tohops = db.ToHopMons.Select(n => new
            {
                Thm_ID = n.Thm_ID,
                Thm_MaToHop = n.Thm_MaToHop,
                Thm_TenToHop = n.Thm_TenToHop,
                Thm_Mon1 = n.Thm_Mon1,
                Thm_Mon2 = n.Thm_Mon2,
                Thm_Mon3 = n.Thm_Mon3,
                Thm_MaTen = n.Thm_MaTen,
                Thm_Thi_NK = n.Thm_Thi_NK,
            }).ToList();
            if (tohops != null && tohops.Count > 0)
            {
                return Json(new { success = true, tohops = tohops }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getAllNganhs()
        {
            var nganhs = db.Nganhs.Select(n => new
            {
                Nganh_ID = n.Nganh_ID,
                Nganh_MaNganh = n.Nganh_MaNganh,
                NganhTenNganh = n.Nganh_TenNganh,
                Khoa_ID = n.Khoa_ID,
                Nganh_GhiChu = n.Nganh_GhiChu,
                KhoiNganh_ID = n.KhoiNganh_ID,
                Nganh_ThiNK = n.Nganh_ThiNK,
            }).ToList();
            if (nganhs != null && nganhs.Count > 0)
            {
                return Json(new { success = true, nganhs = nganhs }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public class NganhTemp
        {
            public int Nganh_ID { get; set; }
            public string Nganh_MaNganh { get; set; }
            public string Nganh_TenNganh { get; set; }
            public int Khoa_ID { get; set; }
            public string Nganh_GhiChu { get; set; }
            public int? KhoiNganh_ID { get; set; }

            public int? Nganh_ThiNK { get; set; }

            // Mảng các tổ hợp môn được chọn từ View
            public int [] array_THM { get; set; }

        }
        [HttpPost]
        public JsonResult createData(NganhTemp dataNganh)
        {
            try
            {
                Nganh nganh = new Nganh();
                nganh.Nganh_MaNganh = dataNganh.Nganh_MaNganh;
                nganh.Nganh_TenNganh = dataNganh.Nganh_TenNganh;
                nganh.Khoa_ID = int.Parse(dataNganh.Khoa_ID.ToString());
                nganh.Nganh_GhiChu = dataNganh.Nganh_GhiChu;
                nganh.KhoiNganh_ID = int.Parse(dataNganh.KhoiNganh_ID.ToString());
                nganh.Nganh_ThiNK = 1;
                int [] arr_THMNganh = dataNganh.array_THM;
                var addedNganh = db.Nganhs.Add(nganh);
                db.SaveChanges();
                if (addedNganh != null && arr_THMNganh.Length > 0)
                {
                    int newNganhId = addedNganh.Nganh_ID; // Lấy ra Id của đối tượng nganh vừa được thêm mới
                    for(int i = 0; i < arr_THMNganh.Length; i++)
                    {
                        ToHopMonNganh tmhNganh = new ToHopMonNganh(); // Tạo một đối tượng tmhNganh mới cho mỗi bản ghi
                        var tohopSearch = db.ToHopMons.Find(arr_THMNganh[i]);
                        var ghichu = "Ngành: " + addedNganh.Nganh_TenNganh + ";";
                        ghichu += " Tổ hợp: " + tohopSearch.Thm_MaToHop + " - " + tohopSearch.Thm_Mon1 + "-" + tohopSearch.Thm_Mon2 + "-" + tohopSearch.Thm_Mon3;
                        tmhNganh.Nganh_ID = newNganhId;
                        tmhNganh.Thm_N_TrangThai = 1;
                        tmhNganh.Thm_ID = arr_THMNganh[i];
                        tmhNganh.Thm_N_GhiChu = ghichu;
                        db.ToHopMonNganhs.Add(tmhNganh);
                    }
                    db.SaveChanges();

                    return Json(new { success = true, }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("e", e);
                return Json(new { success = false, data = e }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult updateData(NganhTemp dataNganh)
        {
            try
            {
                Nganh nganh = new Nganh();
                nganh.Nganh_MaNganh = dataNganh.Nganh_MaNganh;
                nganh.Nganh_TenNganh = dataNganh.Nganh_TenNganh;
                nganh.Khoa_ID = int.Parse(dataNganh.Khoa_ID.ToString());
                nganh.Nganh_GhiChu = dataNganh.Nganh_GhiChu;
                nganh.KhoiNganh_ID = int.Parse(dataNganh.KhoiNganh_ID.ToString());
                nganh.Nganh_ThiNK = 1;
                //db.Entry(nganh).State = EntityState.Modified;
                var listTHM = db.ToHopMonNganhs.Where(x => x.Nganh_ID == dataNganh.Nganh_ID).ToList();
                int[] arr_THMNganh_Original = listTHM.Select(thm => thm.Thm_ID).ToArray();
                int[] arr_THMNganh_New = dataNganh.array_THM;
                // Bước 1: Lấy ra các giá trị giống nhau
                var commonValues = arr_THMNganh_New.Intersect(arr_THMNganh_Original).ToArray();
                // Bước 2: Lấy ra các giá trị mới
                var newValues = arr_THMNganh_New.Except(arr_THMNganh_Original).ToArray();
                // Bước 3: Xóa các giá trị không còn
                var valuesToRemove = arr_THMNganh_Original.Except(arr_THMNganh_New).ToArray();
                // Bước 4: Thực hiện cập nhật trong CSDL
                // Xóa các giá trị không còn
                System.Diagnostics.Debug.WriteLine("commonValues", commonValues);
                System.Diagnostics.Debug.WriteLine("newValues", newValues);
                System.Diagnostics.Debug.WriteLine("valuesToRemove", valuesToRemove);
                if(valuesToRemove.Length > 0)
                {
                    foreach (var value in valuesToRemove)
                    {
                        var toRemove = db.ToHopMonNganhs.Where(t => t.Nganh_ID == dataNganh.Nganh_ID && t.Thm_ID == value).FirstOrDefault();
                        if (toRemove != null)
                        {
                            System.Diagnostics.Debug.WriteLine("toRemove", toRemove);
                            db.ToHopMonNganhs.Remove(toRemove);
                        }
                        db.SaveChanges();
                    }
                }
                
                // Thêm các giá trị mới
                if (dataNganh != null && newValues.Length > 0)
                {
                    int NganhId = dataNganh.Nganh_ID; // Lấy ra Id của đối tượng nganh vừa được thêm mới
                    foreach (var value in newValues)
                    {
                        ToHopMonNganh tmhNganh = new ToHopMonNganh(); // Tạo một đối tượng tmhNganh mới cho mỗi bản ghi
                        var tohopSearch = db.ToHopMons.Find(value);
                        var ghichu = "Ngành: " + dataNganh.Nganh_TenNganh + ";";
                        ghichu += " Tổ hợp: " + tohopSearch.Thm_MaToHop + " - " + tohopSearch.Thm_Mon1 + "-" + tohopSearch.Thm_Mon2 + "-" + tohopSearch.Thm_Mon3;
                        tmhNganh.Nganh_ID = NganhId;
                        tmhNganh.Thm_N_TrangThai = 1;
                        tmhNganh.Thm_ID = value;
                        tmhNganh.Thm_N_GhiChu = ghichu;
                        db.ToHopMonNganhs.Add(tmhNganh);
                    }
                    db.SaveChanges();
                    return Json(new { success = true, }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("e", e);
                return Json(new { success = false, data = e }, JsonRequestBehavior.AllowGet);
            }
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
