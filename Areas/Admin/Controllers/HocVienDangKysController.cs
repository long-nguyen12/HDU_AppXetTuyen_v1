using HDU_AppXetTuyen.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;


namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class HocVienDangKysController : Controller
    {
        private DbConnecttion db = new DbConnecttion();
        // GET: Admin/HocVienDangKys
        public ActionResult Index(string searchString, string currentFilter, string filteriDotxt, int? page)
        {
            var hocviens =  db.HocVienDangKies.OrderBy(x => x.HocVien_ID);

            ViewBag.filteriNam = db.NamHocs.Where(x => x.NamHoc_TrangThai == 1).FirstOrDefault().NamHoc_Ten;
            /*
            #region lọc dữ liệu theo đợt
            var dotxts = db.DotXetTuyens.Include(x => x.NamHoc).Where(x => x.NamHoc.NamHoc_TrangThai == 1).ToList();
            dotxts.Add(new DotXetTuyen() { Dxt_ID = 0, Dxt_Ten = "Tất cả" });
            int _dotxt_hientai = dotxts.Where(x => x.Dxt_TrangThai_Xt == 1 && x.Dxt_Classify == 0).FirstOrDefault().Dxt_ID;
            ViewBag.filteriDotxt = new SelectList(dotxts.OrderBy(x => x.Dxt_ID).ToList(), "Dxt_ID", "Dxt_Ten", _dotxt_hientai);
            // nếu không có truyền vào thì gán giá trị cho đợt xét tuyển là hiện tại
            if (String.IsNullOrEmpty(filteriDotxt) == true)
            {
                filteriDotxt = dotxts.Where(x => x.Dxt_TrangThai_Xt == 1).FirstOrDefault().Dxt_ID.ToString();
            }
            // thực hiện lọc 

            #endregion
            */
            // thưc hiện tìm kiếm: theo họ, tên, cccd, điện thoại, email
            #region Tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                //hocviens = hocviens.Where(m => m.HocVien_Ten.ToUpper().Contains(searchString.ToUpper())
                //                    || m.HocVien_HoDem.ToUpper().Contains(searchString.ToUpper())
                //                    || m.HocVien_CCCD.Contains(searchString)
                //                    || m.HocVien_DienThoai.Contains(searchString)).ToList();
                                    
            }
            #endregion
            // thực hiện phân trang
            #region Phân trang
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            #endregion
            // tham số khác
            #region Tham số khác
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }

            ViewBag.pageCurren = page;
            ViewBag.SearchString = searchString;
            ViewBag.filteriDotxtSort = filteriDotxt;
            ViewBag.totalRecod = hocviens.Count();

            #endregion
            return View(hocviens.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult QLDotDuTuyenSDH()
        {
            var model = db.DotXetTuyens.OrderByDescending(x => x.Dxt_ID).Where(x => x.Dxt_Classify == 2);

                return View(model.ToList());
        }
    }
}