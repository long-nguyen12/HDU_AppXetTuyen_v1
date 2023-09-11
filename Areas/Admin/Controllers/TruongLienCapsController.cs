using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Antlr.Runtime.Misc;
using HDU_AppXetTuyen.Models;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using PagedList;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class TruongLienCapsController : Controller
    {
        private DbConnecttion db = new DbConnecttion();
        public string DEFAULT_URL = Constant.DEFAULT_URL;

        // GET: Admin/LienCapTHCS
        [AdminSessionCheck]
        public ActionResult LcTrunghocCs(string filteriDotxt, string filteriKhoangDiem,  string filteriHoSo, string searchString, string SearchCurrent, int? page)
        {
            List<LienCapTHCS> model = new List<LienCapTHCS>();

            foreach (LienCapTHCS lc in db.LienCapTHCSs.ToList().OrderByDescending(x => x.HocSinh_ID))
            {
                LienCapTHCS item_prs = new LienCapTHCS();
                item_prs.HocSinh_ID = lc.HocSinh_ID;
                item_prs.HocSinh_DinhDanh = lc.HocSinh_DinhDanh;
                item_prs.HocSinh_HoTen = lc.HocSinh_HoTen;
                item_prs.HocSinh_GioiTinh = lc.HocSinh_GioiTinh;
                item_prs.HocSinh_NgaySinh = lc.HocSinh_NgaySinh;
                item_prs.HocSinh_NoiSinh = lc.HocSinh_NoiSinh;
                item_prs.HocSinh_Email = lc.HocSinh_Email;
                item_prs.HocSinh_NoiCuTru = lc.HocSinh_NoiCuTru;
                item_prs.HocSinh_TruongTH = lc.HocSinh_TruongTH;
                item_prs.HocSinh_UuTien = lc.HocSinh_UuTien;
                item_prs.HocSinh_MucDoNangLuc = lc.HocSinh_MucDoNangLuc;
                item_prs.HocSinh_MucDoPhamChat = lc.HocSinh_MucDoPhamChat;
                item_prs.HocSinh_MinhChungHB = lc.HocSinh_MinhChungHB;
                item_prs.HocSinh_MinhChungGiayKS = lc.HocSinh_MinhChungGiayKS;
                item_prs.HocSinh_MinhChungMaDinhDanh = lc.HocSinh_MinhChungMaDinhDanh;
                item_prs.HocSinh_GiayUuTien = lc.HocSinh_GiayUuTien;
                item_prs.HocSinh_XacNhanLePhi = lc.HocSinh_XacNhanLePhi;
                item_prs.HocSinh_TrangThai = lc.HocSinh_TrangThai;
                item_prs.HocSinh_GhiChu = lc.HocSinh_GhiChu;
                item_prs.HocSinh_Activation = lc.HocSinh_Activation;
                item_prs.PhBo = JsonConvert.DeserializeObject<PhuHuynh>(lc.HocSinh_ThongTinCha);
                item_prs.PhMe = JsonConvert.DeserializeObject<PhuHuynh>(lc.HocSinh_ThongTinMe);
                item_prs.Monhocs = JsonConvert.DeserializeObject<MonHocTHCS>(lc.HocSinh_DiemHocTap);
                item_prs.TongDiem = item_prs.Monhocs.Toan + item_prs.Monhocs.TiengViet + item_prs.Monhocs.TuNhien + item_prs.Monhocs.LichSuDiaLy + item_prs.Monhocs.TiengAnh;

                model.Add(item_prs);
            }
            model = model.ToList();

            #region xử lý lọc dữ liệu học sinh đăng ký theo khoảng điểm (lấy từ class KhoangDiem khai báo trong  Model.LibraryUsers )
            List<KhoangDiem> list_khoangDiem = new List<KhoangDiem>();

            list_khoangDiem.Add(new KhoangDiem() { value_kd = 25, text_kd = "Điểm từ 25 đến dưới 30" });
            list_khoangDiem.Add(new KhoangDiem() { value_kd = 30, text_kd = "Điểm từ 30 đến dưới 35" });
            list_khoangDiem.Add(new KhoangDiem() { value_kd = 35, text_kd = "Điểm từ 30 đến dưới 40" });
            list_khoangDiem.Add(new KhoangDiem() { value_kd = 40, text_kd = "Điểm từ 40 đến dưới 45" });
            list_khoangDiem.Add(new KhoangDiem() { value_kd = 45, text_kd = " Điểm từ 45 đến dưới 50" });
            ViewBag.filteriKhoangDiem = new SelectList(list_khoangDiem.ToList(), "value_kd", "text_kd");

            if (!String.IsNullOrEmpty(filteriKhoangDiem))
            {
                int _value_kd = Int32.Parse(filteriKhoangDiem);
                if(_value_kd == 25)
                {
                    model = model.Where(x => x.TongDiem >=25 && x.TongDiem <30).ToList();
                }
                if (_value_kd == 30)
                {
                    model = model.Where(x => x.TongDiem >= 30 && x.TongDiem < 35).ToList();
                }
                if (_value_kd == 35)
                {
                    model = model.Where(x => x.TongDiem >= 35 && x.TongDiem < 40).ToList();
                }
                if (_value_kd == 40)
                {
                    model = model.Where(x => x.TongDiem >= 40 && x.TongDiem < 45).ToList();
                }
                if (_value_kd == 45)
                {
                    model = model.Where(x => x.TongDiem >= 45).ToList();
                }
            }
            #endregion
            #region xử lý lọc dữ liệu theo trạng thái theo dõi hồ sơ  trạng thái hồ sơ (lấy từ class StatusTracking khai báo trong  Model.LibraryUsers )

            var list_items_hoso = (from item in model select item.HocSinh_TrangThai).Distinct().ToList();
            List<StatusTracking> filteri_items_hoso = new List<StatusTracking>();

            foreach (var _item in list_items_hoso)
            {
                if (_item == 0)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 0,St_Name = "Chưa KT" });
                }
                if (_item == 1)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 1, St_Name = "MC sai" });
                }
                if (_item == 2)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 2, St_Name = "Đã duyệt" });
                }
            }
            ViewBag.filteriHoSo = new SelectList(filteri_items_hoso.OrderBy(x => x.St_ID).ToList(), "St_ID", "St_Name");

            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int _hs_TrangThai = Int32.Parse(filteriHoSo);
                model = model.Where(x => x.HocSinh_TrangThai == _hs_TrangThai).ToList();
            }
            #endregion

            #region Tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(m => m.HocSinh_HoTen.ToUpper().Contains(searchString.ToUpper())
                                      || m.HocSinh_DinhDanh.ToUpper().Contains(searchString.ToUpper())
                                      || m.TongDiem.ToString().Contains(searchString)
                                      || m.PhBo.HoTen.Contains(searchString)
                                      || m.PhBo.SoDienThoai.Contains(searchString)
                                      || m.PhMe.HoTen.Contains(searchString)
                                      || m.PhMe.SoDienThoai.Contains(searchString)
                                      || m.HocSinh_Email.Contains(searchString)).ToList();
            }
            #endregion
            // thực hiện phân trang
            #region Phân trang
            if (page == null) page = 1;
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            #endregion
            // tham số khác
            #region Tham số khác
            if (searchString != null) { page = 1; }
            else { searchString = SearchCurrent; }


            ViewBag.DotxtFilteri = filteriDotxt;
            ViewBag.KhoangDiemFilteri = filteriKhoangDiem;         
            ViewBag.HoSoFilteri = filteriHoSo;
            ViewBag.SearchString = searchString;
            ViewBag.PageCurren = page;

            ViewBag.totalRecod = model.Count();

            #endregion
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/LienCapTHCS/Details/5
        [AdminSessionCheck]
        public ActionResult DetailsLcthcs(long? id)
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
        public ActionResult LcTieuhoc(string filteriDotxt, string filteriHoSo, string searchString, string SearchCurrent, int? page)
        {
            List<LienCapTieuHoc> model = new List<LienCapTieuHoc>();

            foreach (LienCapTieuHoc item_check in db.LienCapTieuHocs.ToList().OrderByDescending(x => x.HocSinh_ID))
            {
                LienCapTieuHoc item_prs = new LienCapTieuHoc();
                item_prs.HocSinh_ID = item_check.HocSinh_ID;
                item_prs.HocSinh_DinhDanh = item_check.HocSinh_DinhDanh;
                item_prs.HocSinh_HoTen = item_check.HocSinh_HoTen;
                item_prs.HocSinh_GioiTinh = item_check.HocSinh_GioiTinh;
                item_prs.HocSinh_NgaySinh = item_check.HocSinh_NgaySinh;
                item_prs.HocSinh_NoiSinh = item_check.HocSinh_NoiSinh;
                item_prs.HocSinh_Email = item_check.HocSinh_Email;
                item_prs.HocSinh_NoiCuTru = item_check.HocSinh_NoiCuTru;
                item_prs.HocSinh_TruongMN = item_check.HocSinh_TruongMN;
                item_prs.HocSinh_UuTien = item_check.HocSinh_UuTien;
                item_prs.HocSinh_ThongTinCha = item_check.HocSinh_ThongTinCha;
                item_prs.HocSinh_NgheNghiepCha = item_check.HocSinh_NgheNghiepCha;
                item_prs.HocSinh_DienThoaiCha = item_check.HocSinh_DienThoaiCha;
                item_prs.HocSinh_ThongTinMe = item_check.HocSinh_ThongTinMe;
                item_prs.HocSinh_NgheNghiepMe = item_check.HocSinh_NgheNghiepMe;
                item_prs.HocSinh_DienThoaiMe = item_check.HocSinh_DienThoaiMe;
                item_prs.HocSinh_MinhChungMN = item_check.HocSinh_MinhChungMN;
                item_prs.HocSinh_MinhChungGiayKS = item_check.HocSinh_MinhChungGiayKS;
                item_prs.HocSinh_MinhChungMaDinhDanh = item_check.HocSinh_MinhChungMaDinhDanh;
                item_prs.HocSinh_GiayUuTien = item_check.HocSinh_GiayUuTien;
                item_prs.HocSinh_XacNhanLePhi = item_check.HocSinh_XacNhanLePhi;
                item_prs.HocSinh_TrangThai = item_check.HocSinh_TrangThai;
                item_prs.HocSinh_GhiChu = item_check.HocSinh_GhiChu;
                item_prs.HocSinh_MinhChungLePhi = item_check.HocSinh_MinhChungLePhi;
                item_prs.HocSinh_Activation = item_check.HocSinh_Activation;   
                model.Add(item_prs);
            }
            model = model.ToList();

            #region xử lý lọc dữ liệu theo trạng thái theo dõi hồ sơ  trạng thái hồ sơ (lấy từ class StatusTracking khai báo trong  Model.LibraryUsers )

            var list_items_hoso = (from item in model select item.HocSinh_TrangThai).Distinct().ToList();
            List<StatusTracking> filteri_items_hoso = new List<StatusTracking>();

            foreach (var _item in list_items_hoso)
            {
                if (_item == 0)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 0,St_Name = "Chưa KT" });
                }
                if (_item == 1)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 1,St_Name = "MC sai" });
                }
                if (_item == 2)
                {
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 2,St_Name = "Đã duyệt" });
                }
            }
           
            ViewBag.filteriHoSo = new SelectList(filteri_items_hoso.OrderBy(x => x.St_ID).ToList(), "st_ID", "st_Name");

            if (!String.IsNullOrEmpty(filteriHoSo))
            {
                int _hs_TrangThai = Int32.Parse(filteriHoSo);
                model = model.Where(x => x.HocSinh_TrangThai == _hs_TrangThai).ToList();
            }
            #endregion

            #region Tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(m => m.HocSinh_HoTen.ToUpper().Contains(searchString.ToUpper())
                                      || m.HocSinh_DinhDanh.ToUpper().Contains(searchString.ToUpper())                                     
                                      || m.HocSinh_ThongTinCha.Contains(searchString)
                                      || m.HocSinh_DienThoaiCha.Contains(searchString)
                                      || m.HocSinh_ThongTinMe.Contains(searchString)
                                      || m.HocSinh_DienThoaiMe.Contains(searchString)
                                      || m.HocSinh_Email.Contains(searchString)).ToList();
            }
            #endregion
            // thực hiện phân trang
            #region Phân trang
            if (page == null) page = 1;
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            #endregion
            // tham số khác
            #region Tham số khác
            if (searchString != null) { page = 1; }
            else { searchString = SearchCurrent; }


            ViewBag.DotxtFilteri = filteriDotxt;       
            ViewBag.HoSoFilteri = filteriHoSo;
            ViewBag.SearchString = searchString;
            ViewBag.PageCurren = page;

            ViewBag.totalRecod = model.Count();

            #endregion
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/LienCapTHCS/Details/5
        [AdminSessionCheck]
        public ActionResult DetailsTh(long? id)
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
