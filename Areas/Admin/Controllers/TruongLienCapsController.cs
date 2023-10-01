using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Antlr.Runtime.Misc;
using HDU_AppXetTuyen.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
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
        public ActionResult LcTrunghocCs(string filteriDotxt, string filteriKhoangDiem, string filteriHoSo, string searchString, string SearchCurrent, int? page)
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
                if (_value_kd == 25)
                {
                    model = model.Where(x => x.TongDiem >= 25 && x.TongDiem < 30).ToList();
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
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa KT" });
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
                    filteri_items_hoso.Add(new StatusTracking() { St_ID = 0, St_Name = "Chưa KT" });
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
        public ActionResult DetailsThcs(long? id)
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
        public ActionResult EditThcs(long? id)
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
        public ActionResult EditThcs([Bind(Include = "HocSinh_ID,HocSinh_DinhDanh,HocSinh_HoTen,HocSinh_GioiTinh,HocSinh_NgaySinh,HocSinh_NoiSinh,HocSinh_Email,HocSinh_NoiCuTru,HocSinh_TruongTH,HocSinh_UuTien,HocSinh_ThongTinCha,HocSinh_ThongTinMe,HocSinh_DiemHocTap,HocSinh_MucDoNangLuc,HocSinh_MucDoPhamChat,HocSinh_MinhChungHB,HocSinh_MinhChungGiayKS,HocSinh_MinhChungMaDinhDanh,HocSinh_GiayUuTien,HocSinh_XacNhanLePhi,HocSinh_Activation,HocSinh_TrangThai,HocSinh_GhiChu")] LienCapTHCS lienCapTHCS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lienCapTHCS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LcTrunghocCs");
            }
            return View(lienCapTHCS);
        }

        // GET: Admin/LienCapTHCS/Delete/5
        [AdminSessionCheck]
        public ActionResult DeleteThcs(long? id)
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

        // POST: Admin/LienCapTHCS/DeleteThcs/5
        [HttpPost, ActionName("DeleteThcs")]
        [ValidateAntiForgeryToken]
        [AdminSessionCheck]
        public ActionResult DeleteConfirmedThcs(long id)
        {
            LienCapTHCS lienCapTHCS = db.LienCapTHCSs.Find(id);
            db.LienCapTHCSs.Remove(lienCapTHCS);
            db.SaveChanges();
            return RedirectToAction("LcTrunghocCs");
        }


        // GET: Admin/LienCapTieuHocs/Details/5
        [AdminSessionCheck]
        public ActionResult DetailsTh(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTieuHoc lienCapTieuHoc = db.LienCapTieuHocs.Find(id);
            if (lienCapTieuHoc == null)
            {
                return HttpNotFound();
            }
            ViewBag.URL = DEFAULT_URL;
            return View(lienCapTieuHoc);
        }

        // GET: Admin/LienCapTieuHocs/Edit/5
        [AdminSessionCheck]
        public ActionResult EditTh(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTieuHoc lienCapTieuHoc = db.LienCapTieuHocs.Find(id);
            if (lienCapTieuHoc == null)
            {
                return HttpNotFound();
            }
            return View(lienCapTieuHoc);
        }

        // POST: Admin/LienCapTieuHocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminSessionCheck]
        public ActionResult EditTh([Bind(Include = "HocSinh_ID,HocSinh_DinhDanh,HocSinh_HoTen,HocSinh_GioiTinh,HocSinh_NgaySinh,HocSinh_NoiSinh,HocSinh_Email,HocSinh_NoiCuTru,HocSinh_TruongMN,HocSinh_UuTien,HocSinh_ThongTinCha,HocSinh_NgheNghiepCha,HocSinh_DienThoaiCha,HocSinh_ThongTinMe,HocSinh_NgheNghiepMe,HocSinh_DienThoaiMe,HocSinh_MinhChungMN,HocSinh_MinhChungGiayKS,HocSinh_MinhChungMaDinhDanh,HocSinh_GiayUuTien,HocSinh_XacNhanLePhi,HocSinh_TrangThai,HocSinh_GhiChu,HocSinh_MinhChungLePhi,HocSinh_Activation")] LienCapTieuHoc lienCapTieuHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lienCapTieuHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LcTieuhoc");
            }
            return View(lienCapTieuHoc);
        }

        // GET: Admin/LienCapTieuHocs/Delete/5
        [AdminSessionCheck]
        public ActionResult DeleteTh(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienCapTieuHoc lienCapTieuHoc = db.LienCapTieuHocs.Find(id);
            if (lienCapTieuHoc == null)
            {
                return HttpNotFound();
            }
            return View(lienCapTieuHoc);
        }

        // POST: Admin/LienCapTieuHocs/Delete/5
        [HttpPost, ActionName("DeleteTh")]
        [ValidateAntiForgeryToken]
        [AdminSessionCheck]
        public ActionResult DeleteConfirmedTh(long id)
        {
            LienCapTieuHoc lienCapTieuHoc = db.LienCapTieuHocs.Find(id);
            db.LienCapTieuHocs.Remove(lienCapTieuHoc);
            db.SaveChanges();
            return RedirectToAction("LcTieuhoc");
        }

        public void ExportHocSinhTH()
        {
            var dxt_hientai = db.DotXetTuyens.FirstOrDefault(d => d.Dxt_Classify == 2 && d.Dxt_TrangThai_Xt == 1);

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
            try
            {
                using (ExcelPackage _excelpackage = new ExcelPackage())
                {
                    _excelpackage.Workbook.Properties.Author = "208Team";  // đặt tên người tạo file                       
                    _excelpackage.Workbook.Properties.Title = "TKHVDKDuTuyen"; // đặt tiêu đề cho file                    

                    //Tạo sheet để làm việc 
                    _excelpackage.Workbook.Worksheets.Add("ThongKeHVDKDuTuyen");
                    string[] arr_col_number = { "TT", "Họ tên", "Mã định danh", "Ngày sinh", "Giới tính","Trường mầm non", "Diện ưu tiên",
                        "Họ tên mẹ", "Điện thoại mẹ", "Họ tên cha", "Điện thoại cha","Email" ,"Địa chỉ liên hệ"};

                    ExcelWorksheet ws = null; // khai báo để thao tác với ws

                    // lấy sheet vừa add ra để thao tác 
                    if (model.Count > 0)
                    {
                        ws = _excelpackage.Workbook.Worksheets[1];

                        ws.Name = "ThongKeHSTH";  // đặt tên cho sheet                       
                        ws.Cells.Style.Font.Size = 12;  // fontsize mặc định cho cả sheet                       
                        ws.Cells.Style.Font.Name = "Times New Roman"; // font family mặc định cho cả sheet

                        ws.Cells[1, 1].Value = "DANH SÁCH HỌC SINH DỰ TUYỂN TIỂU HỌC";
                        ws.Cells[1, 1, 1, 12].Merge = true;
                        //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                        ws.Cells[2, 1].Value = "Số liệu thống kê dự tuyển " + dxt_hientai.Dxt_Ten + ", Từ ngày " + dxt_hientai.Dxt_ThoiGian_BatDau + " đến ngày " + dxt_hientai.Dxt_ThoiGian_KetThuc;
                        ws.Cells[2, 1, 2, 12].Merge = true;
                        //ws.Cells["A1:F1"].Merge = true;

                        // Tạo danh sách các tiêu đề cho cột (column header)                         
                        int colIndex = 1, rowIndex = 3;
                        //tạo các header từ column header đã tạo từ bên trên
                        foreach (var item in arr_col_number)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = item;
                            colIndex++;
                        }

                        rowIndex = 3;
                        // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                        foreach (var item in model)
                        {
                            colIndex = 1; // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0
                            rowIndex++;  // rowIndex tương ứng từng dòng dữ liệu
                            //gán giá trị cho từng cell                      
                            ws.Cells.Style.Font.Bold = false;
                            ws.Cells.Style.WrapText = true;
                            ws.Cells[rowIndex, colIndex++].Value = (rowIndex - 3);                          //  1 số thư tự 
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_HoTen;        //  2 
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_DinhDanh;          //  3
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_NgaySinh;     //  4
                            if (item.HocSinh_GioiTinh == 1)                                              //  5
                            {
                                ws.Cells[rowIndex, colIndex++].Value = "Nữ";
                            }
                            if (item.HocSinh_GioiTinh == 0)
                            {
                                ws.Cells[rowIndex, colIndex++].Value = "Nam";
                            }
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_TruongMN;     //  6
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_UuTien;     //  7
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_ThongTinMe;      //  8
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_DienThoaiMe;        //  9
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_ThongTinCha;            //  10
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_DienThoaiCha;      //  11
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_Email;      //  12
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_NoiCuTru;     //  13
                            ws.Cells[rowIndex, colIndex++].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                        for (int indexCol = 1; indexCol <= arr_col_number.Count(); indexCol++)
                        {
                            if (indexCol == 1) { ws.Column(indexCol).Width = 6; }       //1
                            if (indexCol == 2) { ws.Column(indexCol).Width = 20; }      //2
                            if (indexCol == 3) { ws.Column(indexCol).Width = 20; }     //3
                            if (indexCol == 4) { ws.Column(indexCol).Width = 14.3; }    //4
                            if (indexCol == 5) { ws.Column(indexCol).Width = 13; }      //5
                            if (indexCol == 6) { ws.Column(indexCol).Width = 25; }      //6 
                            if (indexCol == 7) { ws.Column(indexCol).Width = 15; }      //7
                            if (indexCol == 8) { ws.Column(indexCol).Width = 20; }      //8
                            if (indexCol == 9) { ws.Column(indexCol).Width = 15; }      //9
                            if (indexCol == 10) { ws.Column(indexCol).Width = 20; }      //10
                            if (indexCol == 11) { ws.Column(indexCol).Width = 15; }     //11
                            if (indexCol == 12) { ws.Column(indexCol).Width = 25; }     //12
                            if (indexCol == 13) { ws.Column(indexCol).Width = 25; }     //13

                            ws.Cells[3, 1, 3, indexCol].Style.Font.Bold = true;         // đặt tiêu đề cho bảng có kiểu chữ đậm
                        }
                        //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                        ws.Cells[1, 1].Style.Font.Bold = true;
                        ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIndex, colIndex++].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    }
                    //Lưu file lại   //string excelName = "ThongKeHVDKDuTuyen";
                    using (var memoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + ws.Name + ".xlsx"); // tên file lưu
                        _excelpackage.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                //return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        public void ExportHocSinhTHCS()
        {
            var dxt_hientai = db.DotXetTuyens.FirstOrDefault(d => d.Dxt_Classify == 2 && d.Dxt_TrangThai_Xt == 1);
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
                item_prs.HocSinh_ThongTinCha = lc.HocSinh_ThongTinCha;
                item_prs.HocSinh_ThongTinMe = lc.HocSinh_ThongTinMe;
                item_prs.HocSinh_DiemHocTap = lc.HocSinh_DiemHocTap;
                item_prs.TongDiem = item_prs.Monhocs.Toan + item_prs.Monhocs.TiengViet + item_prs.Monhocs.TuNhien + item_prs.Monhocs.LichSuDiaLy + item_prs.Monhocs.TiengAnh;

                model.Add(item_prs);
            }
            model = model.ToList();
            try
            {
                using (ExcelPackage _excelpackage = new ExcelPackage())
                {
                    _excelpackage.Workbook.Properties.Author = "208Team";  // đặt tên người tạo file                       
                    _excelpackage.Workbook.Properties.Title = "TKHVDKDuTuyen"; // đặt tiêu đề cho file                    

                    //Tạo sheet để làm việc 
                    _excelpackage.Workbook.Worksheets.Add("ThongKeHVDKDuTuyen");
                    string[] arr_col_number = { "TT", "Họ tên", "Mã định danh", "Ngày sinh", "Giới tính","Trường tiểu học", "Diện ưu tiên",
"Điểm học tập","Tổng điểm","Mức độ phẩm chất","Mức độ năng lực","Thông tin mẹ", "Thông tin cha", "Email" ,"Địa chỉ liên hệ"};

                    ExcelWorksheet ws = null; // khai báo để thao tác với ws

                    // lấy sheet vừa add ra để thao tác 
                    if (model.Count > 0)
                    {
                        ws = _excelpackage.Workbook.Worksheets[1];

                        ws.Name = "ThongKeHSTHCS";  // đặt tên cho sheet                       
                        ws.Cells.Style.Font.Size = 12;  // fontsize mặc định cho cả sheet                       
                        ws.Cells.Style.Font.Name = "Times New Roman"; // font family mặc định cho cả sheet

                        ws.Cells[1, 1].Value = "DANH SÁCH HỌC SINH DỰ TUYỂN TRUNG HỌC CƠ SỞ";
                        ws.Cells[1, 1, 1, 12].Merge = true;
                        //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                        ws.Cells[2, 1].Value = "Số liệu thống kê dự tuyển " + dxt_hientai.Dxt_Ten + ", Từ ngày " + dxt_hientai.Dxt_ThoiGian_BatDau + " đến ngày " + dxt_hientai.Dxt_ThoiGian_KetThuc;
                        ws.Cells[2, 1, 2, 12].Merge = true;
                        //ws.Cells["A1:F1"].Merge = true;

                        // Tạo danh sách các tiêu đề cho cột (column header)                         
                        int colIndex = 1, rowIndex = 3;
                        //tạo các header từ column header đã tạo từ bên trên
                        foreach (var item in arr_col_number)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = item;
                            colIndex++;
                        }

                        rowIndex = 3;
                        // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                        foreach (var item in model)
                        {
                            colIndex = 1; // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0
                            rowIndex++;  // rowIndex tương ứng từng dòng dữ liệu
                            //gán giá trị cho từng cell                      
                            ws.Cells.Style.Font.Bold = false;
                            ws.Cells.Style.WrapText = true;
                            ws.Cells[rowIndex, colIndex++].Value = (rowIndex - 3);                          //  1 số thư tự 
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_HoTen;        //  2 
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_DinhDanh;          //  3
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_NgaySinh;     //  4
                            if (item.HocSinh_GioiTinh == 1)                                              //  5
                            {
                                ws.Cells[rowIndex, colIndex++].Value = "Nữ";
                            }
                            if (item.HocSinh_GioiTinh == 0)
                            {
                                ws.Cells[rowIndex, colIndex++].Value = "Nam";
                            }
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_TruongTH;     //  6
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_UuTien;     //  7
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_DiemHocTap;     //  8
                            ws.Cells[rowIndex, colIndex++].Value = item.TongDiem;     //  9
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_MucDoPhamChat;     //  10
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_MucDoNangLuc;     //  11
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_ThongTinCha;      //  12
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_ThongTinMe;            //  13
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_Email;      //  14
                            ws.Cells[rowIndex, colIndex++].Value = item.HocSinh_NoiCuTru;     //  15
                            ws.Cells[rowIndex, colIndex++].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                        for (int indexCol = 1; indexCol <= arr_col_number.Count(); indexCol++)
                        {
                            if (indexCol == 1) { ws.Column(indexCol).Width = 6; }       //1
                            if (indexCol == 2) { ws.Column(indexCol).Width = 20; }      //2
                            if (indexCol == 3) { ws.Column(indexCol).Width = 20; }     //3
                            if (indexCol == 4) { ws.Column(indexCol).Width = 14.3; }    //4
                            if (indexCol == 5) { ws.Column(indexCol).Width = 13; }      //5
                            if (indexCol == 6) { ws.Column(indexCol).Width = 25; }      //6 
                            if (indexCol == 7) { ws.Column(indexCol).Width = 15; }      //7
                            if (indexCol == 8) { ws.Column(indexCol).Width = 25; }      //8
                            if (indexCol == 9) { ws.Column(indexCol).Width = 20; }      //9
                            if (indexCol == 10) { ws.Column(indexCol).Width = 15; }     //10
                            if (indexCol == 11) { ws.Column(indexCol).Width = 15; }     //11
                            if (indexCol == 12) { ws.Column(indexCol).Width = 25; }     //12
                            if (indexCol == 13) { ws.Column(indexCol).Width = 25; }     //13
                            if (indexCol == 14) { ws.Column(indexCol).Width = 25; }     //14
                            if (indexCol == 15) { ws.Column(indexCol).Width = 25; }     //15

                            ws.Cells[3, 1, 3, indexCol].Style.Font.Bold = true;         // đặt tiêu đề cho bảng có kiểu chữ đậm
                        }
                        //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
                        ws.Cells[1, 1].Style.Font.Bold = true;
                        ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIndex, colIndex++].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Column(9).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Column(10).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Column(11).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    }
                    //Lưu file lại   //string excelName = "ThongKeHVDKDuTuyen";
                    using (var memoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + ws.Name + ".xlsx"); // tên file lưu
                        _excelpackage.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                //return Json(new { success = false }, JsonRequestBehavior.AllowGet);
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
