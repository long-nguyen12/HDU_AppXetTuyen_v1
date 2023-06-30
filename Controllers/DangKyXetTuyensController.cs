using HDU_AppXetTuyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HDU_AppXetTuyen.Controllers
{


    public class DangKyXetTuyensController : Controller
    {
        #region test
        [ThiSinhSessionCheck]
        public ActionResult index()
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
        #endregion 
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
                                       Include(d => d.ToHopMon).Where(ts => ts.ThiSinh_ID == _thisinh_id && ts.Ptxt_ID == ptxt_check).OrderBy(x=> x.Dkxt_NguyenVong).ToList();
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
                    Dkxt_Diem_Tong = s.Dkxt_Diem_Tong.toFixed(2),
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
                nganh_GhiChu = s.NganhTenNganh,
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
                thm_MaTen = s.ToHopMon.Thm_TenToHop,
                thm_TenToHop = s.ToHopMon.Thm_TenToHop
            });
            return Json(selectResult_tohopmon_nganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DangKyXetTuyen_Get_ByID(str_infor data)
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
                    dkxt_id_update = data.int_input_b,
                    thm_Mon1 = new { TenMon1 = get_tohopmon_byid.Thm_Mon1, HK1 = 0, HK2 = 0, HK3 = 0, DiemTrungBinh = 0 },
                    thm_Mon2 = new { TenMon2 = get_tohopmon_byid.Thm_Mon2, HK1 = 0, HK2 = 0, HK3 = 0, DiemTrungBinh = 0 },
                    thm_Mon3 = new { TenMon3 = get_tohopmon_byid.Thm_Mon3, HK1 = 0, HK2 = 0, HK3 = 0, DiemTrungBinh = 0 },
                    ttBosung_Ut = new
                    {
                        ut_doituong_ten = get_tsdk_byid.DoiTuong.DoiTuong_Ten,
                        ut_doituong_diem_ut = get_tsdk_byid.DoiTuong.DoiTuong_DiemUuTien,
                        ut_khuvuc_ten = get_tsdk_byid.KhuVuc.KhuVuc_Ten,
                        ut_khuvuc_diem_ut = get_tsdk_byid.KhuVuc.KhuVuc_DiemUuTien,
                        xeploai_hocluc_12 = get_tsdk_byid.ThiSinh_HocLucLop12,
                        xeploai_hanhkiem_12 = get_tsdk_byid.ThiSinh_HanhKiemLop12,
                    },
                };
                return Json(information_to_client, JsonRequestBehavior.AllowGet);
            }
            if (data.int_input_b > 0)// sửa đăng ký
            {
                var dkxt_detail_getby_id = dkxt_db.DangKyXetTuyens.Include(d => d.DoiTuong).Include(d => d.KhuVuc).FirstOrDefault(x => x.Dkxt_ID == data.int_input_b);

                var information_to_client = new
                {
                    dkxt_id_update = data.int_input_b,
                    thm_Mon1 = new { dkxt_detail_getby_id.Dkxt_Diem_M1 },
                    thm_Mon2 = new { dkxt_detail_getby_id.Dkxt_Diem_M2 },
                    thm_Mon3 = new { dkxt_detail_getby_id.Dkxt_Diem_M3 },
                    ttBosung_Ut = new
                    {
                        ut_doituong_ten = dkxt_detail_getby_id.DoiTuong.DoiTuong_Ten,
                        ut_doituong_diem_ut = dkxt_detail_getby_id.DoiTuong.DoiTuong_DiemUuTien,
                        ut_khuvuc_ten = dkxt_detail_getby_id.KhuVuc.KhuVuc_Ten,
                        ut_khuvuc_diem_ut = dkxt_detail_getby_id.KhuVuc.KhuVuc_DiemUuTien,
                        xeploai_hocluc_12 = get_tsdk_byid.ThiSinh_HocLucLop12,
                        xeploai_hanhkiem_12 = get_tsdk_byid.ThiSinh_HanhKiemLop12,
                    },
                };
                return Json(information_to_client, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult Delete(string idDkxt)
        {
            DbConnecttion db = new DbConnecttion();

            if (!string.IsNullOrWhiteSpace(idDkxt))
            {
                int id = int.Parse(idDkxt);
                System.Diagnostics.Debug.WriteLine(id);
                DangKyXetTuyen dangKyXetTuyen = db.DangKyXetTuyens.Find(id);
                int nv_current = (int) dangKyXetTuyen.Dkxt_NguyenVong;
                db.DangKyXetTuyens.Remove(dangKyXetTuyen);
                foreach (var item in db.DangKyXetTuyens.Where(nv => nv.Dkxt_NguyenVong > nv_current))
                {
                    DangKyXetTuyen dangKyXetTuyen_change = db.DangKyXetTuyens.FirstOrDefault(i => i.Dkxt_NguyenVong == item.Dkxt_NguyenVong);
                    dangKyXetTuyen_change.Dkxt_NguyenVong = item.Dkxt_NguyenVong - 1;
                }
                db.SaveChanges();
                return Json(new
                {
                    status = true,
                    msg = "Xoá dữ liệu thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false,
                msg = "Có lỗi xảy ra."
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DangKyXetTuyen_Add(ThongTinNguyenVong nguyenvong)
        {
            DbConnecttion db = new DbConnecttion();
            var session = Session["login_session"].ToString();
            var ts = db.ThiSinhDangKies.Where(n => n.ThiSinh_MatKhau.Equals(session)).
                Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc).FirstOrDefault();

            var dotxettuyen = db.DotXetTuyens.Where(n => n.Dxt_TrangThai == 1).FirstOrDefault();
            if (ts != null)
            {
                var nvs = db.DangKyXetTuyens.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID).ToList();
                DangKyXetTuyen dkxt = new DangKyXetTuyen();
                dkxt.Nganh_ID = int.Parse(nguyenvong.Nganh_ID);
                dkxt.Thm_ID = int.Parse(nguyenvong.Thm_ID);
                dkxt.Dkxt_XepLoaiHocLuc_12 = ts.ThiSinh_HocLucLop12;
                dkxt.Dkxt_XepLoaiHanhKiem_12 = ts.ThiSinh_HanhKiemLop12;
                dkxt.Dkxt_Diem_M1 = nguyenvong.Dkxt_Diem_M1;
                dkxt.Dkxt_Diem_M2 = nguyenvong.Dkxt_Diem_M2;
                dkxt.Dkxt_Diem_M3 = nguyenvong.Dkxt_Diem_M3;
                dkxt.Dkxt_Diem_Tong = nguyenvong.Dkxt_Diem_Tong;
                dkxt.ThiSinh_ID = ts.ThiSinh_ID;
                dkxt.DoiTuong_ID = ts.DoiTuong_ID;
                dkxt.KhuVuc_ID = ts.KhuVuc_ID;
                dkxt.DotXT_ID = dotxettuyen.Dxt_ID;
                dkxt.Dkxt_TrangThai = 0;
                dkxt.Dkxt_TrangThai_KetQua = 0;
                var diemTong = float.Parse(nguyenvong.Dkxt_Diem_Tong);
                var diemDoiTuong = ts.DoiTuong.DoiTuong_DiemUuTien;
                var khuVucDoiTuong = ts.KhuVuc.KhuVuc_DiemUuTien;
                var diemTongFull = diemTong + diemDoiTuong + khuVucDoiTuong;
                dkxt.Dkxt_Diem_Tong_Full = diemTongFull.ToString();
                dkxt.Ptxt_ID = 3;
                dkxt.Dkxt_NguyenVong = nvs.Count + 1;
                db.DangKyXetTuyens.Add(dkxt);

                // add LePhiXetTuyen
                var lePhiRecord = db.LePhiXetTuyens.Where(n => n.ThiSinh_ID == ts.ThiSinh_ID).FirstOrDefault();
                if (lePhiRecord == null)
                {
                    LePhiXetTuyen lpxt = new LePhiXetTuyen();
                    lpxt.ThiSinh_ID = ts.ThiSinh_ID;
                    lpxt.Lpxt_TrangThai = 0;
                    db.LePhiXetTuyens.Add(lpxt);
                }

                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("upData")]
        public JsonResult upData(string idDkxt)
        {
            DbConnecttion db = new DbConnecttion();
            if (!string.IsNullOrWhiteSpace(idDkxt))
            {
                int id = int.Parse(idDkxt);
                System.Diagnostics.Debug.WriteLine(id);
                DangKyXetTuyen dangKyXetTuyen_current = db.DangKyXetTuyens.Find(id);
                int nv_current = (int) dangKyXetTuyen_current.Dkxt_NguyenVong;
                dangKyXetTuyen_current.Dkxt_NguyenVong = nv_current - 1;
                DangKyXetTuyen dangKyXetTuyen_up = db.DangKyXetTuyens.FirstOrDefault(i => i.Dkxt_NguyenVong == nv_current - 1);
                dangKyXetTuyen_up.Dkxt_NguyenVong = nv_current;
                if (ModelState.IsValid)
                {
                    db.Entry(dangKyXetTuyen_current).State = EntityState.Modified;
                    db.Entry(dangKyXetTuyen_up).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new
                    {
                        status = true,
                        msg = "Thay đổi dữ liệu thành công"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                status = false,
                msg = "Có lỗi xảy ra."
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("downData")]
        public JsonResult downData(string idDkxt)
        {
            DbConnecttion db = new DbConnecttion();
            if (!string.IsNullOrWhiteSpace(idDkxt))
            {
                int id = int.Parse(idDkxt);
                System.Diagnostics.Debug.WriteLine(id);
                DangKyXetTuyen dangKyXetTuyen_current = db.DangKyXetTuyens.Find(id);
                int nv_current = (int) dangKyXetTuyen_current.Dkxt_NguyenVong;
                dangKyXetTuyen_current.Dkxt_NguyenVong = nv_current + 1;
                DangKyXetTuyen dangKyXetTuyen_down = db.DangKyXetTuyens.FirstOrDefault(i => i.Dkxt_NguyenVong == nv_current + 1);
                dangKyXetTuyen_down.Dkxt_NguyenVong = nv_current;
                if (ModelState.IsValid)
                {
                    db.Entry(dangKyXetTuyen_current).State = EntityState.Modified;
                    db.Entry(dangKyXetTuyen_down).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new
                    {
                        status = true,
                        msg = "Thay đổi dữ liệu thành công"
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new
            {
                status = false,
                msg = "Có lỗi xảy ra."
            }, JsonRequestBehavior.AllowGet);
        }
    }


    public class NguyenVong
    {
        public string Nganh_ID { get; set; }
        public string Thm_ID { get; set; }
        public string Dkxt_Diem_M1 { get; set; }
        public string Dkxt_Diem_M2 { get; set; }
        public string Dkxt_Diem_M3 { get; set; }
        public string Dkxt_Diem_Tong { get; set; }
        public string Dkxt_Diem_Tong_Full { get; set; }      
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