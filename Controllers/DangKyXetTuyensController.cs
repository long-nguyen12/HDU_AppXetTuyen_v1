using HDU_AppXetTuyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HDU_AppXetTuyen.Controllers
{
    public class NguyenVong
    {
        public string Nganh_ID { get; set; }
        public string Thm_ID { get; set; }
        public string Dkxt_Diem_M1 { get; set; }
        public string Dkxt_Diem_M2 { get; set; }
        public string Dkxt_Diem_M3 { get; set; }
        public string Dkxt_Diem_Tong { get; set; }
        public string Dkxt_Diem_Tong_Full { get; set; }
        public string Dkxt_XepLoaiHocLuc_12 { get; set; }
        public string Dkxt_XepLoaiHanhKiem_12 { get; set; }
    }

    public class DangKyXetTuyensController : Controller
    {
        [ThiSinhSessionCheck]
        public ActionResult iddkxthocba()
        {
            DbConnecttion db_tsdk = new DbConnecttion();
            DbConnecttion db_dkxt = new DbConnecttion();
            if (Session["login_session"] != null)
            {
                string str_login_session = Session["login_session"].ToString();
                //ViewBag.str_login_session = str_login_session;

                var tsdk_Detail = db_tsdk.ThiSinhDangKies.Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault(ts => ts.ThiSinh_MatKhau.Equals(str_login_session));

                long _thisinh_id = tsdk_Detail.ThiSinh_ID;

                var dkxt_Detail_list = db_dkxt.DangKyXetTuyens.Include(d => d.DoiTuong).
                                      Include(d => d.DotXetTuyen).
                                      Include(d => d.KhuVuc).
                                      Include(d => d.Nganh).
                                      Include(d => d.PhuongThucXetTuyen).
                                      Include(d => d.ThiSinhDangKy).
                                      Include(d => d.ToHopMon).Where(ts => ts.ThiSinh_ID == _thisinh_id).ToList();
                return View(dkxt_Detail_list);
            }
            return View();
        }

        [ThiSinhSessionCheck]
        public ActionResult dkxthocba()
        {
            return View();
        }
        public JsonResult dkxthocbaListAll()
        {
            int ptxt_check = 3;
            //var thiSinhDangKies = db.ThiSinhDangKies.Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc);

            DbConnecttion db_tsdk = new DbConnecttion();
            DbConnecttion db_dkxt = new DbConnecttion();
            if (Session["login_session"] != null)
            {
                string str_login_session = Session["login_session"].ToString();
                //ViewBag.str_login_session = str_login_session;
                var tsdk_Detail = db_tsdk.ThiSinhDangKies.Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault(ts => ts.ThiSinh_MatKhau.Equals(str_login_session));

                long _thisinh_id = tsdk_Detail.ThiSinh_ID;

                var dkxt_Detail_list = db_dkxt.DangKyXetTuyens.
                                       Include(d => d.DoiTuong).
                                       Include(d => d.DotXetTuyen).
                                       Include(d => d.KhuVuc).
                                       Include(d => d.Nganh).
                                       Include(d => d.PhuongThucXetTuyen).
                                       Include(d => d.ThiSinhDangKy).
                                       Include(d => d.ToHopMon).Where(ts => ts.ThiSinh_ID == _thisinh_id && ts.Ptxt_ID == ptxt_check).ToList();
                var select_list_dkxt_model = dkxt_Detail_list.Select(s => new
                {
                    Dkxt_ID = s.Dkxt_ID,
                    ThiSinh_ID = s.ThiSinh_ID,
                    Ptxt_ID = s.Ptxt_ID,
                    Nganh_ID = s.Nganh_ID,
                    Thm_ID = s.Thm_ID,
                    DoiTuong_ID = s.DoiTuong_ID,
                    KhuVuc_ID = s.KhuVuc_ID,
                    Dkxt_TrangThai = s.Dkxt_TrangThai,
                    Dkxt_NguyenVong = s.Dkxt_NguyenVong,
                    DotXT_ID = s.DotXT_ID,
                    Dkxt_TenAll = new
                    {
                        KhoiNganh_Ten = s.Nganh.KhoiNganh.KhoiNganh_Ten,
                        Nganh_GhiChu = s.Nganh.Nganh_GhiChu,
                        Thm_MaTen = s.ToHopMon.Thm_MaTen,
                    },
                    Dkxt_Diem_M1 = s.Dkxt_Diem_M1,
                    Dkxt_Diem_M2 = s.Dkxt_Diem_M2,
                    Dkxt_Diem_M3 = s.Dkxt_Diem_M3,
                    Dkxt_Diem_Tong = s.Dkxt_Diem_Tong,
                    Dkxt_Diem_Tong_Full = s.Dkxt_Diem_Tong_Full,
                });
                return Json(select_list_dkxt_model.ToList(), JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult KhoiNganhListAll()
        {
            DbConnecttion khoinganh_db = new DbConnecttion();
            var selectResult_KhoiNganh = khoinganh_db.KhoiNganhs.Select(s => new
            {
                khoiNganh_ID = s.KhoiNganh_ID,
                khoiNganh_Ten = s.KhoiNganh_Ten
            });
            return Json(selectResult_KhoiNganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult NganhListAll(int id)
        {
            DbConnecttion nganh_db = new DbConnecttion();

            var selectResult_Nganh = nganh_db.Nganhs.Where(x => x.KhoiNganh_ID == id || x.Nganh_ID == 0).OrderBy(x => x.Nganh_ID).Select(s => new
            {
                nganh_ID = s.Nganh_ID,
                nganh_GhiChu = s.Nganh_GhiChu,
                khoiNganh_ID = s.KhoiNganh_ID
            });
            return Json(selectResult_Nganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ToHopMonNganhListAll(int id)
        {
            DbConnecttion thm_nganh_db = new DbConnecttion();
            var selectResult_tohopmon_nganh = thm_nganh_db.ToHopMonNganhs.Include(n => n.ToHopMon).Where(x => x.Nganh_ID == id || x.ToHopMon.Thm_ID == 0).OrderBy(x => x.ToHopMon.Thm_ID).Select(s => new
            {
                thm_ID = s.Thm_ID,
                thm_MaTen = s.ToHopMon.Thm_MaTen,
                thm_TenToHop = s.ToHopMon.Thm_TenToHop
            });
            return Json(selectResult_tohopmon_nganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ToHopMonGeByID(str_infor data)
        {
            DbConnecttion thm_db = new DbConnecttion();
            DbConnecttion dkxt_db = new DbConnecttion();
            DbConnecttion tsdk_db = new DbConnecttion();

            string str_login_session = Session["login_session"].ToString();
            var get_tsdk_byid = tsdk_db.ThiSinhDangKies.Include(t => t.DoiTuong).Include(t => t.KhuVuc).FirstOrDefault(ts => ts.ThiSinh_MatKhau.Equals(str_login_session));

            var get_tohopmon_byid = thm_db.ToHopMons.FirstOrDefault(x => x.Thm_ID == data.int_input_a);
            if (data.int_input_b < 0)// sửa đăng ký
            {
                var information_to_client = new
                {
                    thm_Mon1 = new { TenMon1 = get_tohopmon_byid.Thm_Mon1, HK1 = 0, HK2 = 0, HK3 = 0, DiemTrungBinh = 0 },
                    thm_Mon2 = new { TenMon2 = get_tohopmon_byid.Thm_Mon2, HK1 = 0, HK2 = 0, HK3 = 0, DiemTrungBinh = 0 },
                    thm_Mon3 = new { TenMon3 = get_tohopmon_byid.Thm_Mon3, HK1 = 0, HK2 = 0, HK3 = 0, DiemTrungBinh = 0 },
                    _ttbs_ut = new
                    {
                        ut_doituong_ten = get_tsdk_byid.DoiTuong.DoiTuong_Ten,
                        ut_doituong_diem_ut = get_tsdk_byid.DoiTuong.DoiTuong_DiemUuTien,
                        ut_khuvuc_ten = get_tsdk_byid.KhuVuc.KhuVuc_Ten,
                        ut_khuvuc_diem_ut = get_tsdk_byid.KhuVuc.KhuVuc_DiemUuTien,
                        _xlhocluc_12 = "",
                        xl_hankiem_12 = "",

                    },
                };
                return Json(information_to_client, JsonRequestBehavior.AllowGet);
            }
            if (data.int_input_b > 0)// sửa đăng ký
            {
                var dkxt_detail_getby_id = dkxt_db.DangKyXetTuyens.Include(d => d.DoiTuong).Include(d => d.KhuVuc).FirstOrDefault(x => x.Dkxt_ID == data.int_input_b);

                var information_to_client = new
                {
                    thm_Mon1 = new { dkxt_detail_getby_id.Dkxt_Diem_M1 },
                    thm_Mon2 = new { dkxt_detail_getby_id.Dkxt_Diem_M2 },
                    thm_Mon3 = new { dkxt_detail_getby_id.Dkxt_Diem_M3 },
                    _ttbs_ut = new
                    {
                        ut_doituong_ten = dkxt_detail_getby_id.DoiTuong.DoiTuong_Ten,
                        ut_doituong_diem_ut = dkxt_detail_getby_id.DoiTuong.DoiTuong_DiemUuTien,
                        ut_khuvuc_ten = dkxt_detail_getby_id.KhuVuc.KhuVuc_Ten,
                        ut_khuvuc_diem_ut = dkxt_detail_getby_id.KhuVuc.KhuVuc_DiemUuTien,
                        _xlhocluc_12 = dkxt_detail_getby_id.Dkxt_XepLoaiHocLuc_12,
                        xl_hankiem_12 = dkxt_detail_getby_id.Dkxt_XepLoaiHanhKiem_12,
                    },
                };
                return Json(information_to_client, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddNguyenVong(ThongTinNguyenVong nguyenvong)
        {
            DbConnecttion db = new DbConnecttion();
            var session = Session["login_session"].ToString();
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc).FirstOrDefault();
            var dotxettuyen = db.DotXetTuyens.Where(n => n.Dxt_TrangThai == 1).FirstOrDefault();
            if(ts != null)
            {
                var nvs = db.DangKyXetTuyens.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID).ToList();
                DangKyXetTuyen dkxt = new DangKyXetTuyen();
                dkxt.Nganh_ID = int.Parse(nguyenvong.Nganh_ID);
                dkxt.Thm_ID = int.Parse(nguyenvong.Thm_ID);
                dkxt.Dkxt_XepLoaiHocLuc_12 = nguyenvong.Dkxt_XepLoaiHocLuc_12;
                dkxt.Dkxt_XepLoaiHanhKiem_12 = nguyenvong.Dkxt_XepLoaiHanhKiem_12;
                dkxt.Dkxt_Diem_M1 = nguyenvong.Dkxt_Diem_M1;
                dkxt.Dkxt_Diem_M2 = nguyenvong.Dkxt_Diem_M2;
                dkxt.Dkxt_Diem_M3 = nguyenvong.Dkxt_Diem_M3;
                dkxt.Dkxt_Diem_Tong = nguyenvong.Dkxt_Diem_Tong;
                dkxt.ThiSinh_ID = ts.ThiSinh_ID;
                dkxt.DoiTuong_ID = ts.DoiTuong_ID;
                dkxt.KhuVuc_ID = ts.KhuVuc_ID;
                dkxt.DotXT_ID = dotxettuyen.Dxt_ID;
                dkxt.Dkxt_TrangThai = 0;
                var diemTong = float.Parse(nguyenvong.Dkxt_Diem_Tong);
                var diemDoiTuong = ts.DoiTuong.DoiTuong_DiemUuTien;
                var khuVucDoiTuong = ts.KhuVuc.KhuVuc_DiemUuTien;
                var diemTongFull = diemTong + diemDoiTuong + khuVucDoiTuong;
                dkxt.Dkxt_Diem_Tong_Full = diemTongFull.ToString();
                dkxt.Ptxt_ID = 3;
                dkxt.Dkxt_NguyenVong = nvs.Count + 1;
                db.DangKyXetTuyens.Add(dkxt);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
    }

    public class str_infor
    {
        public int int_input_a { get; set; }
        public int int_input_b { get; set; }
    }

    public class ThongTinNguyenVong
    {
        public string Nganh_ID { get; set; }
        public string Thm_ID { get; set; }
        public string Dkxt_XepLoaiHocLuc_12 { get; set; }
        public string Dkxt_XepLoaiHanhKiem_12 { get; set; }
        public string Dkxt_Diem_M1 { get; set; }
        public string Dkxt_Diem_M2 { get; set; }
        public string Dkxt_Diem_M3 { get; set; }
        public string Dkxt_Diem_Tong { get; set; }
    }
}