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
        public ActionResult dkxthocba()
        {
            return View();
            /*
            if (Session["login_session"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Login");
            }*/

        }
        public JsonResult dkxthocbaListAll()
        {
            int ptxt_check = 3;
            //var thiSinhDangKies = db.ThiSinhDangKies.Include(t => t.DoiTuong).Include(t => t.DotXetTuyen).Include(t => t.KhuVuc);

            DbConnecttion db_tsdk = new DbConnecttion();
            DbConnecttion db_dkxt = new DbConnecttion();
            Session["login_session"] = "11111111"; // => vào chương trình k cần dòng code này
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
                    Dkxt_TenAll = new {
                        KhoiNganh_Ten = s.Nganh.KhoiNganh.KhoiNganh_Ten,
                        Nganh_GhiChu = s.Nganh.Nganh_GhiChu,
                        Thm_MaTen =  s.ToHopMon.Thm_MaTen,
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
                KhoiNganh_ID = s.KhoiNganh_ID,
                KhoiNganh_Ten = s.KhoiNganh_Ten               
            });
            return Json(selectResult_KhoiNganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult NganhListAll(int id)
        {
            DbConnecttion nganh_db = new DbConnecttion();
            var selectResult_Nganh = nganh_db.Nganhs.Where(x => x.KhoiNganh_ID == id).Select(s => new
            {
                Nganh_ID = s.Nganh_ID,
                Nganh_GhiChu = s.Nganh_GhiChu,
                KhoiNganh_ID = s.KhoiNganh_ID
            });
            return Json(selectResult_Nganh.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ToHopMonNganhListAll(int id)
        {
            DbConnecttion thm_db = new DbConnecttion();
            var selectResult_tohopmon_nganh = thm_db.ToHopMonNganhs.Include(n => n.ToHopMon).Where(x => x.Nganh_ID == id).Select(s => new
            {
                Thm_ID = s.Thm_ID,
                Thm_MaTen = s.ToHopMon.Thm_MaTen,
                Thm_TenToHop = s.ToHopMon.Thm_TenToHop               
            });
            return Json(selectResult_tohopmon_nganh.ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}