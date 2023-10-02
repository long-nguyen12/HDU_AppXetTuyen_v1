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
        private DbConnecttion db = null;
        private IList<TongHopSoLieuXetTuyen> ListLePhiXetTuyen;

        #region Theo dõi thí sinh nộp kinh phí
        public ActionResult KpKiemTra2()
        {
            return View();
        }
        /*
        [HttpPost]
        public JsonResult GetAllDataKinhPhi()
        {
            ListLePhiXetTuyen = new List<TongHopSoLieuXetTuyen>();
            db = new DbConnecttion();
            var model_dxt_present = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var data_md = new
            {
                model_dxt_present.Dxt_ID,
            };

            var model_xt2 = db.DangKyXetTuyenKQTQGs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_KQTQG_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_KQTQG_NguyenVong,

                TrangThai = s.Dkxt_KQTQG_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_KQTQG_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_KQTQG_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_KQTQG_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_KQTQG_KinhPhi_TepMinhChung,
            }).ToList();

            foreach (var item in model_xt2)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = int.Parse(item.Ptxt_ID.ToString());
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP= item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXetTuyen.Add(item_lp);
            }

            var model_xt3 = db.DangKyXetTuyenHBs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_HB_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_HB_NguyenVong,
                TrangThai = s.Dkxt_HB_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_HB_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_HB_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_HB_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_HB_KinhPhi_TepMinhChung,
            }).ToList();
            foreach (var item in model_xt3)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = item.Ptxt_ID.ToString();
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXetTuyen.Add(item_lp);
            }
            var model_xt4 = db.DangKyXetTuyenThangs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_NguyenVong,
                TrangThai = s.Dkxt_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_KinhPhi_TepMinhChung,
            }).ToList();
            foreach (var item in model_xt4)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = item.Ptxt_ID.ToString();
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXetTuyen.Add(item_lp);
            }
            var model_xt5 = db.DangKyXetTuyenKhacs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_NguyenVong,
                TrangThai = s.Dkxt_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_KinhPhi_TepMinhChung,
            }).ToList();
            foreach (var item in model_xt5)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = item.Ptxt_ID.ToString();
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXetTuyen.Add(item_lp);
            }
            ListLePhiXetTuyen = ListLePhiXetTuyen.OrderByDescending(x => x.NgayThangNopLP)
                                                 .ThenBy(x => x.TrangThaiLP).ThenByDescending(x => x.Dkxt_ID)
                                                 .ThenByDescending(x => x.ThiSinh_ID).ToList();
            return Json(new { success = true, data = ListLePhiXetTuyen }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult KpKiemTra()
        {
            IList<TongHopSoLieuXetTuyen>  ListLePhiXT = new List<TongHopSoLieuXetTuyen>();
            db = new DbConnecttion();
            var model_dxt_present = db.DotXetTuyens.Where(x => x.Dxt_Classify == 0 && x.Dxt_TrangThai_Xt == 1).FirstOrDefault();
            var data_md = new
            {
                model_dxt_present.Dxt_ID,
            };

            var model_xt2 = db.DangKyXetTuyenKQTQGs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_KQTQG_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_KQTQG_NguyenVong,
                TrangThai = s.Dkxt_KQTQG_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_KQTQG_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_KQTQG_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_KQTQG_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_KQTQG_KinhPhi_TepMinhChung,
            }).ToList();

            foreach (var item in model_xt2)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = item.Ptxt_ID.ToString();
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXT.Add(item_lp);
            }

            var model_xt3 = db.DangKyXetTuyenHBs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_HB_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_HB_NguyenVong,
                TrangThai = s.Dkxt_HB_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_HB_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_HB_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_HB_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_HB_KinhPhi_TepMinhChung,
            }).ToList();
            foreach (var item in model_xt3)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = item.Ptxt_ID.ToString();
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXT.Add(item_lp);
            }
            var model_xt4 = db.DangKyXetTuyenThangs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_NguyenVong,
                TrangThai = s.Dkxt_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_KinhPhi_TepMinhChung,
            }).ToList();
            foreach (var item in model_xt4)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = item.Ptxt_ID.ToString();
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXT.Add(item_lp);
            }
            var model_xt5 = db.DangKyXetTuyenKhacs.Include(x => x.ThiSinhDangKy).Include(x => x.PhuongThucXetTuyen).Include(x => x.DotXetTuyen).Where(x => x.DotXT_ID == model_dxt_present.Dxt_ID).Select(s => new
            {
                ThiSinh_ID = s.ThiSinhDangKy.ThiSinh_ID.ToString(),
                Dkxt_ID = s.Dkxt_ID.ToString(),
                Dxt_ID = s.DotXT_ID.ToString(),
                HoDem = s.ThiSinhDangKy.ThiSinh_HoLot,
                Ten = s.ThiSinhDangKy.ThiSinh_Ten,
                DienThoai = s.ThiSinhDangKy.ThiSinh_DienThoai,
                Ptxt_ID = s.Ptxt_ID,
                NguyenVong = s.Dkxt_NguyenVong,
                TrangThai = s.Dkxt_TrangThai_KinhPhi.ToString(),
                SoThamChieu = s.Dkxt_KinhPhi_SoThamChieu,
                NgayThang_NopMC = s.Dkxt_KinhPhi_NgayThang_NopMC,
                NgayThang_CheckMC = s.Dkxt_KinhPhi_NgayThang_CheckMC,
                KinhPhi_TepMinhChung = s.Dkxt_KinhPhi_TepMinhChung,
            }).ToList();
            foreach (var item in model_xt5)
            {
                TongHopSoLieuXetTuyen item_lp = new TongHopSoLieuXetTuyen();
                item_lp.ThiSinh_ID = item.ThiSinh_ID.ToString();
                item_lp.Dkxt_ID = item.Dkxt_ID.ToString();
                item_lp.Dxt_ID = item.Dxt_ID.ToString();
                item_lp.HoDem = item.HoDem.ToString();
                item_lp.Ten = item.Ten.ToString();
                item_lp.DienThoai = item.DienThoai.ToString();
                item_lp.Ptxt_ID = item.Ptxt_ID.ToString();
                item_lp.NguyenVong = item.NguyenVong.ToString();
                item_lp.TrangThaiLP = item.TrangThai.ToString();
                item_lp.SoThamChieuLP = item.SoThamChieu.ToString();
                item_lp.NgayThangNopLP = item.NgayThang_NopMC.ToString();
                item_lp.NgayThangCheckLP = item.NgayThang_CheckMC.ToString();
                item_lp.MinhChungLP = item.KinhPhi_TepMinhChung;
                ListLePhiXT.Add(item_lp);
            }
            ListLePhiXT = ListLePhiXT.OrderByDescending(x => x.NgayThangNopLP)
                                                 .ThenBy(x => x.TrangThaiLP).ThenByDescending(x => x.Dkxt_ID)
                                                 .ThenByDescending(x => x.ThiSinh_ID).ToList();
            return View(ListLePhiXT);
           //  return Json(new { success = true, data = ListLePhiXT }, JsonRequestBehavior.AllowGet);
        }
       */
        #endregion

    }
}
