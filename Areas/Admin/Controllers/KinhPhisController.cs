using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HDU_AppXetTuyen.Models;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class KinhPhisController : Controller
    {


        #region Theo dõi thí sinh nộp kinh phí
        public ActionResult KpKiemTra()
        {
            DbConnecttion dbkp = new DbConnecttion();
            var model = dbkp.KinhPhis.Include(m => m.ThiSinhDangKy).Include(m => m.DotXetTuyen).Include(m => m.PhuongThucXetTuyen)
                           .OrderBy(m => m.Ptxt_ID).ThenBy(x => x.KinhPhi_NguyenVong);
            return View(model.ToList());
        }
        public ActionResult Edit(long? id)
        {
            DbConnecttion dbkp = new DbConnecttion();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KinhPhi kinhPhi = dbkp.KinhPhis.Find(id);
            if (kinhPhi == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dxt_ID = new SelectList(dbkp.DotXetTuyens, "Dxt_ID", "Dxt_Ten", kinhPhi.Dxt_ID);
            ViewBag.Ptxt_ID = new SelectList(dbkp.PhuongThucXetTuyens, "Ptxt_ID", "Ptxt_TenPhuongThuc", kinhPhi.Ptxt_ID);
            ViewBag.ThiSinh_ID = new SelectList(dbkp.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", kinhPhi.ThiSinh_ID);
            return View(kinhPhi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KinhPhi_ID,ThiSinh_ID,Dkxt_ID,Dxt_ID,Ptxt_ID,KinhPhi_NguyenVong,KinhPhi_SoTC,KinhPhi_NgayThang_NopMC,KinhPhi_NgayThang_CheckMC,KinhPhi_TepMinhChung,KinhPhi_TrangThai,KinhPhi_GhiChu")] KinhPhi kinhPhi)
        {
            DbConnecttion dbkp = new DbConnecttion();
            if (ModelState.IsValid)
            {
                dbkp.Entry(kinhPhi).State = EntityState.Modified;
                dbkp.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Dxt_ID = new SelectList(dbkp.DotXetTuyens, "Dxt_ID", "Dxt_Ten", kinhPhi.Dxt_ID);
            ViewBag.Ptxt_ID = new SelectList(dbkp.PhuongThucXetTuyens, "Ptxt_ID", "Ptxt_TenPhuongThuc", kinhPhi.Ptxt_ID);
            ViewBag.ThiSinh_ID = new SelectList(dbkp.ThiSinhDangKies, "ThiSinh_ID", "ThiSinh_CCCD", kinhPhi.ThiSinh_ID);
            return View(kinhPhi);
        }
        #endregion

    }
}
