using HDU_AppXetTuyen.Models;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Xceed.Document.NET;
using Xceed.Words.NET;

using System.Web.UI.WebControls;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace HDU_AppXetTuyen.Controllers
{
    public class ExportFileController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: ExportFile
        public ActionResult Index()
        {
            return View();
        }
        [Obsolete]
        public ActionResult DownloadFile_XetTuyenHB()
        {
            var idThiSinhInt = 0;
            // Check login session có tồn tại hay không nếu không tồn tại thì FIX idThiSinhInt = 2
            if (Session["login_session"] == null)
            {
                idThiSinhInt = 2;
                System.Diagnostics.Debug.WriteLine("login_session is NULL: ");
            }
            else
            {
                string str_thisinh_session = Session["login_session"].ToString();
                var thisinh_session = db.ThiSinhDangKies.Where(x => x.ThiSinh_MatKhau == str_thisinh_session).FirstOrDefault();
                idThiSinhInt = (int)thisinh_session.ThiSinh_ID;
                System.Diagnostics.Debug.WriteLine("idThiSinhInt: " + idThiSinhInt);
            }

            string templateFilePath = Server.MapPath("~/Content/static/export-62-bieu-mau-xet-hoc-ba.docx");
            // Từ idDkxt lấy ra thông tin thí sinh
            var thiSinhInfo = db.ThiSinhDangKies.Find(idThiSinhInt);
            // Từ id thí sinh lấy ra tất cả nguyện vọng đăng ký xét tuyển của thí sinh
            var listDkxt = db.DangKyXetTuyenHBs.Where(x => x.ThiSinh_ID == idThiSinhInt).OrderBy(x => x.Dkxt_HB_NguyenVong).ToArray();

            int idKhuVucTS = (int) thiSinhInfo.KhuVuc_ID;
            // Từ khu vực id lấy ra tên khu vực
            var tenKhuVuc = db.KhuVucs.Find(idKhuVucTS).KhuVuc_Ten;

            int idDoiTuongTS = (int) thiSinhInfo.DoiTuong_ID;
            // Từ đối tượng id lấy ra tên đối tượng
            var tenDoiTuong = db.DoiTuongs.Find(idDoiTuongTS).DoiTuong_Ten;
            if (tenDoiTuong == null) tenDoiTuong = " ";
            using (DocX document = DocX.Load(templateFilePath))
            {
                // Replace placeholders with actual data
                string gt = thiSinhInfo.ThiSinh_GioiTinh == 0 ? "Nam" : "Nữ";
                // Replace placeholders with actual data
                document.ReplaceText("<<ThiSinh_HoLot>>", thiSinhInfo.ThiSinh_HoLot);
                document.ReplaceText("<<ThiSinh_Ten>>", thiSinhInfo.ThiSinh_Ten);
                document.ReplaceText("<<ThiSinh_CCCD>>", thiSinhInfo.ThiSinh_CCCD);
                document.ReplaceText("<<ThiSinh_NgaySinh>>", thiSinhInfo.ThiSinh_NgaySinh);
                document.ReplaceText("<<ThiSinh_GioiTinh>>", gt);
                document.ReplaceText("<<ThiSinh_DanToc>>", thiSinhInfo.ThiSinh_DanToc);
                document.ReplaceText("<<ThiSinh_HoKhauThuongTru>>", thiSinhInfo.ThiSinh_HoKhauThuongTru);
                document.ReplaceText("<<ThiSinh_TruongCapBa>>", thiSinhInfo.ThiSinh_TruongCapBa);
                document.ReplaceText("<<ThiSinh_TruongCapBa_Ma>>", thiSinhInfo.ThiSinh_TruongCapBa_Ma);
                document.ReplaceText("<<ThiSinh_DienThoai>>", thiSinhInfo.ThiSinh_DienThoai);
                document.ReplaceText("<<ThiSinh_KhuVuc>>", tenKhuVuc);
                document.ReplaceText("<<ThiSinh_DoiTuong>>", tenDoiTuong);
                document.ReplaceText("<<ThiSinh_DCNhanGiayBao>>", thiSinhInfo.ThiSinh_DCNhanGiayBao);

                // Xử lý học lực và hạnh kiểm
                string hocluc = getHocLucById((int)listDkxt[0].Dkxt_HB_XepLoaiHocLuc_12);
                string hanhkiem = getHanhKiemById((int)listDkxt[0].Dkxt_HB_XepLoaiHanhKiem_12);
                document.ReplaceText("<<ThiSinh_HocLuc12>>", hocluc);
                document.ReplaceText("<<ThiSinh_HanhKiem12>>", hanhkiem);

                // Lấy ra table đầu tiên trong file word
                var table = document.Tables[0];
                int index = 1;

                // Lấy ra tất cả  nguyện vọng của thí sinh
                foreach (var item in listDkxt)
                {
                    // Chuẩn bị dữ liệu cho table
                    //Từ ngành id lấy ra tên ngành và mã ngành
                    var tenNganh = db.Nganhs.Find(item.Nganh_ID).NganhTenNganh;
                    var maNganh = db.Nganhs.Find(item.Nganh_ID).Nganh_MaNganh;

                    // string to obj   
                    var jsonStringMon1 = JsonConvert.SerializeObject(item.Dkxt_HB_Diem_M1);
                    var jsonStringMon2 = JsonConvert.SerializeObject(item.Dkxt_HB_Diem_M2);
                    var jsonStringMon3 = JsonConvert.SerializeObject(item.Dkxt_HB_Diem_M3);
                  
                    // MonDiem khai báo trong  Model.LibraryUsers
                    MonDiem diemmon1 = JsonConvert.DeserializeObject<MonDiem>(item.Dkxt_HB_Diem_M1);
                    MonDiem diemmon2 = JsonConvert.DeserializeObject<MonDiem>(item.Dkxt_HB_Diem_M2);
                    MonDiem diemmon3 = JsonConvert.DeserializeObject<MonDiem>(item.Dkxt_HB_Diem_M3);

                    // Lấy ra thông tin chi tiết của từng môn1
                    var mon1_tenmon = diemmon1.TenMon.ToString();
                    var mon1_hk1 = diemmon1.HK1.ToString();
                    var mon1_hk2 = diemmon1.HK2.ToString();
                    var mon1_hk3 = diemmon1.HK3.ToString();
                    var mon1_diemtb = diemmon1.DiemTrungBinh != null ? diemmon1.DiemTrungBinh.ToString() : " ";

                    // Lấy ra thông tin chi tiết của từng môn2
                    var mon2_tenmon = diemmon2.TenMon.ToString();
                    var mon2_hk1 = diemmon2.HK1.ToString();
                    var mon2_hk2 = diemmon2.HK2.ToString();
                    var mon2_hk3 = diemmon2.HK3.ToString();
                    var mon2_diemtb = diemmon2.DiemTrungBinh != null ? diemmon2.DiemTrungBinh.ToString() : " ";

                    // Lấy ra thông tin chi tiết của từng môn3
                    var mon3_tenmon = diemmon3.TenMon.ToString();
                    var mon3_hk1 = diemmon3.HK1.ToString();
                    var mon3_hk2 = diemmon3.HK2.ToString();
                    var mon3_hk3 = diemmon3.HK3.ToString();
                    var mon3_diemtb = diemmon3.DiemTrungBinh != null ? diemmon3.DiemTrungBinh.ToString() : " ";

                    // 1 nguyện vọng chèn 3 row
                    table.InsertRow();
                    table.InsertRow();
                    table.InsertRow();
                    // Thêm dữ liệu vào table
                    table.Rows[index].Cells[0].Paragraphs[0].Append(item.Dkxt_HB_NguyenVong.ToString()).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[1].Paragraphs[0].Append(tenNganh).Font("Times New Roman");
                    table.Rows[index].Cells[2].Paragraphs[0].Append(maNganh).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[3].Paragraphs[0].Append("Môn 1: " + mon1_tenmon).Font("Times New Roman");
                    table.Rows[index].Cells[4].Paragraphs[0].Append(mon1_hk1).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[5].Paragraphs[0].Append(mon1_hk2).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[6].Paragraphs[0].Append(mon1_hk3).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[7].Paragraphs[0].Append(mon1_diemtb).Font("Times New Roman").Alignment = Alignment.center;

                    table.Rows[index + 1].Cells[3].Paragraphs[0].Append("Môn 2: " + mon2_tenmon).Font("Times New Roman");
                    table.Rows[index + 1].Cells[4].Paragraphs[0].Append(mon2_hk1).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index + 1].Cells[5].Paragraphs[0].Append(mon2_hk2).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index + 1].Cells[6].Paragraphs[0].Append(mon2_hk3).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index + 1].Cells[7].Paragraphs[0].Append(mon2_diemtb).Font("Times New Roman").Alignment = Alignment.center;

                    table.Rows[index + 2].Cells[3].Paragraphs[0].Append("Môn 3: " + mon3_tenmon).Font("Times New Roman");
                    table.Rows[index + 2].Cells[4].Paragraphs[0].Append(mon3_hk1).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index + 2].Cells[5].Paragraphs[0].Append(mon3_hk2).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index + 2].Cells[6].Paragraphs[0].Append(mon3_hk3).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index + 2].Cells[7].Paragraphs[0].Append(mon3_diemtb).Font("Times New Roman").Alignment = Alignment.center;

                    // Merge đúng định dạng
                    table.MergeCellsInColumn(0, index, index + 2);
                    table.MergeCellsInColumn(1, index, index + 2);
                    table.MergeCellsInColumn(2, index, index + 2);

                    // Căn giữa theo chiều dọc cell

                    table.Rows[index].Cells[0].VerticalAlignment = VerticalAlignment.Center; ;
                    table.Rows[index].Cells[1].VerticalAlignment = VerticalAlignment.Center; ;
                    table.Rows[index].Cells[2].VerticalAlignment = VerticalAlignment.Center; ;

                    // Tăng giá trị index để fill nguyện vọng tiếp theo
                    index = index + 3;

                }
                // Generate a unique file name
                string fileName = thiSinhInfo.ThiSinh_Ten + "_bieu-mau-xet-hoc-ba_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".docx";

                // Save the filled document to a temporary location on the server
                string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
                document.SaveAs(tempFilePath);

                // Return the file for download
                byte[] fileBytes = System.IO.File.ReadAllBytes(tempFilePath);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }
           
        }

        [Obsolete]
        public ActionResult DownloadFile_XetTuyenThang()
        {

            var idThiSinhInt = 0;
            // Check login session có tồn tại hay không nếu không tồn tại thì FIX idThiSinhInt = 2 - DEV
            if (Session["login_session"] == null)
            {
                idThiSinhInt = 2;
                System.Diagnostics.Debug.WriteLine("login_session is NULL: ");
            }
            else
            {
                string str_thisinh_session = Session["login_session"].ToString();
                var thisinh_session = db.ThiSinhDangKies.Where(x => x.ThiSinh_MatKhau == str_thisinh_session).FirstOrDefault();
                idThiSinhInt = (int)thisinh_session.ThiSinh_ID;
                System.Diagnostics.Debug.WriteLine("idThiSinhInt: " + idThiSinhInt);
            }

            string templateFilePath = Server.MapPath("~/Content/static/export-63-bieu-mau-xet-tuyen-thang.docx");
            // Từ idDkxt lấy ra thông tin thí sinh
            var thiSinhInfo = db.ThiSinhDangKies.Find(idThiSinhInt);
            // Từ id thí sinh lấy ra tất cả nguyện vọng đăng ký xét tuyển của thí sinh
            var listDkxt = db.DangKyXetTuyenThangs.Where(x => x.ThiSinh_ID == idThiSinhInt).OrderBy(x => x.Dkxt_NguyenVong).ToArray();

            int idKhuVucTS = (int)thiSinhInfo.KhuVuc_ID;
            // Từ khu vực id lấy ra tên khu vực
            var tenKhuVuc = db.KhuVucs.Find(idKhuVucTS).KhuVuc_Ten;

            int idDoiTuongTS = (int)thiSinhInfo.DoiTuong_ID;
            // Từ đối tượng id lấy ra tên đối tượng
            var tenDoiTuong = db.DoiTuongs.Find(idDoiTuongTS).DoiTuong_Ten;
            if (tenDoiTuong == null) tenDoiTuong = " ";
            using (DocX document = DocX.Load(templateFilePath))
            {
                // Replace placeholders with actual data
                string gt = thiSinhInfo.ThiSinh_GioiTinh == 0 ? "Nam" : "Nữ";
                // Replace placeholders with actual data
                document.ReplaceText("<<ThiSinh_HoLot>>", thiSinhInfo.ThiSinh_HoLot);
                document.ReplaceText("<<ThiSinh_Ten>>", thiSinhInfo.ThiSinh_Ten);
                document.ReplaceText("<<ThiSinh_CCCD>>", thiSinhInfo.ThiSinh_CCCD);
                document.ReplaceText("<<ThiSinh_NgaySinh>>", thiSinhInfo.ThiSinh_NgaySinh);
                document.ReplaceText("<<ThiSinh_GioiTinh>>", gt);
                document.ReplaceText("<<ThiSinh_DanToc>>", thiSinhInfo.ThiSinh_DanToc);
                document.ReplaceText("<<ThiSinh_HoKhauThuongTru>>", thiSinhInfo.ThiSinh_HoKhauThuongTru);
                document.ReplaceText("<<ThiSinh_TruongCapBa>>", thiSinhInfo.ThiSinh_TruongCapBa);
                document.ReplaceText("<<ThiSinh_TruongCapBa_Ma>>", thiSinhInfo.ThiSinh_TruongCapBa_Ma);
                document.ReplaceText("<<ThiSinh_DienThoai>>", thiSinhInfo.ThiSinh_DienThoai);
                document.ReplaceText("<<ThiSinh_KhuVuc>>", tenKhuVuc);
                document.ReplaceText("<<ThiSinh_DoiTuong>>", tenDoiTuong);
                document.ReplaceText("<<ThiSinh_DCNhanGiayBao>>", thiSinhInfo.ThiSinh_DCNhanGiayBao);

                // Xử lý học lực và hạnh kiểm
                string hocluc = getHocLucById((int)listDkxt[0].Dkxt_XepLoaiHocLuc_12);
                string hanhkiem = getHanhKiemById((int)listDkxt[0].Dkxt_XepLoaiHanhKiem_12);
                document.ReplaceText("<<ThiSinh_HocLuc12>>", hocluc);
                document.ReplaceText("<<ThiSinh_HanhKiem12>>", hanhkiem);

                // Lấy ra table đầu tiên trong file word
                var table = document.Tables[0];
                int index = 1;

                // Lấy ra tất cả  nguyện vọng của thí sinh
                foreach (var item in listDkxt)
                {
                    // Chuẩn bị dữ liệu cho table
                    //Từ ngành id lấy ra tên ngành và mã ngành
                    var tenNganh = db.Nganhs.Find(item.Nganh_ID).NganhTenNganh;
                    var maNganh = db.Nganhs.Find(item.Nganh_ID).Nganh_MaNganh;

                    // 1 nguyện vọng chèn 1 row
                    table.InsertRow();
                    // Thêm dữ liệu vào table
                    table.Rows[index].Cells[0].Paragraphs[0].Append(item.Dkxt_NguyenVong.ToString()).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[1].Paragraphs[0].Append(tenNganh).Font("Times New Roman");
                    table.Rows[index].Cells[2].Paragraphs[0].Append(maNganh).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[3].Paragraphs[0].Append(item.Dkxt_ToHopXT).Font("Times New Roman");
                    table.Rows[index].Cells[4].Paragraphs[0].Append(item.Dkxt_MonDatGiai).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[5].Paragraphs[0].Append(item.Dkxt_LoaiGiai).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[6].Paragraphs[0].Append(item.Dkxt_NamDatGiai).Font("Times New Roman").Alignment = Alignment.center;

                    // Tăng giá trị index để fill nguyện vọng tiếp theo
                    index = index + 1;

                }
                // Generate a unique file name
                string fileName = thiSinhInfo.ThiSinh_Ten + "_bieu-mau-xet-tuyen-thang_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".docx";

                // Save the filled document to a temporary location on the server
                string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
                document.SaveAs(tempFilePath);

                // Return the file for download
                byte[] fileBytes = System.IO.File.ReadAllBytes(tempFilePath);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }

        }

        // Xét tuyển theo chứng chỉ ngoại ngữ
        [Obsolete]
        public ActionResult DownloadFile_XetTuyenCCNN()
        {

            var idThiSinhInt = 0;
            // Check login session có tồn tại hay không nếu không tồn tại thì FIX idThiSinhInt = 2 - DEV
            if (Session["login_session"] == null)
            {
                idThiSinhInt = 2;
                System.Diagnostics.Debug.WriteLine("login_session is NULL: ");
            }
            else
            {
                string str_thisinh_session = Session["login_session"].ToString();
                var thisinh_session = db.ThiSinhDangKies.Where(x => x.ThiSinh_MatKhau == str_thisinh_session).FirstOrDefault();
                idThiSinhInt = (int)thisinh_session.ThiSinh_ID;
                System.Diagnostics.Debug.WriteLine("idThiSinhInt: " + idThiSinhInt);
            }

            string templateFilePath = Server.MapPath("~/Content/static/export-64-bieu-mau-xet-ccnn.docx");
            // Từ idDkxt lấy ra thông tin thí sinh
            var thiSinhInfo = db.ThiSinhDangKies.Find(idThiSinhInt);
            // Từ id thí sinh lấy ra tất cả nguyện vọng đăng ký xét tuyển của thí sinh
            var listDkxt = db.DangKyXetTuyenKhacs.Where(x => x.ThiSinh_ID == idThiSinhInt && x.Dkxt_ToHopXT == "HDP5").OrderBy(x => x.Dkxt_NguyenVong).ToArray();

            int idKhuVucTS = (int)thiSinhInfo.KhuVuc_ID;
            // Từ khu vực id lấy ra tên khu vực
            var tenKhuVuc = db.KhuVucs.Find(idKhuVucTS).KhuVuc_Ten;

            int idDoiTuongTS = (int)thiSinhInfo.DoiTuong_ID;
            // Từ đối tượng id lấy ra tên đối tượng
            var tenDoiTuong = db.DoiTuongs.Find(idDoiTuongTS).DoiTuong_Ten;
            if (tenDoiTuong == null) tenDoiTuong = " ";
            using (DocX document = DocX.Load(templateFilePath))
            {
                // Replace placeholders with actual data
                string gt = thiSinhInfo.ThiSinh_GioiTinh == 0 ? "Nam" : "Nữ";
                // Replace placeholders with actual data
                document.ReplaceText("<<ThiSinh_HoLot>>", thiSinhInfo.ThiSinh_HoLot);
                document.ReplaceText("<<ThiSinh_Ten>>", thiSinhInfo.ThiSinh_Ten);
                document.ReplaceText("<<ThiSinh_CCCD>>", thiSinhInfo.ThiSinh_CCCD);
                document.ReplaceText("<<ThiSinh_NgaySinh>>", thiSinhInfo.ThiSinh_NgaySinh);
                document.ReplaceText("<<ThiSinh_GioiTinh>>", gt);
                document.ReplaceText("<<ThiSinh_DanToc>>", thiSinhInfo.ThiSinh_DanToc);
                document.ReplaceText("<<ThiSinh_HoKhauThuongTru>>", thiSinhInfo.ThiSinh_HoKhauThuongTru);
                document.ReplaceText("<<ThiSinh_TruongCapBa>>", thiSinhInfo.ThiSinh_TruongCapBa);
                document.ReplaceText("<<ThiSinh_TruongCapBa_Ma>>", thiSinhInfo.ThiSinh_TruongCapBa_Ma);
                document.ReplaceText("<<ThiSinh_DienThoai>>", thiSinhInfo.ThiSinh_DienThoai);
                document.ReplaceText("<<ThiSinh_KhuVuc>>", tenKhuVuc);
                document.ReplaceText("<<ThiSinh_DoiTuong>>", tenDoiTuong);
                document.ReplaceText("<<ThiSinh_DCNhanGiayBao>>", thiSinhInfo.ThiSinh_DCNhanGiayBao);

                // Xử lý học lực và hạnh kiểm
                string hocluc = getHocLucById((int)listDkxt[0].Dkxt_XepLoaiHocLuc_12);
                string hanhkiem = getHanhKiemById((int)listDkxt[0].Dkxt_XepLoaiHanhKiem_12);
                document.ReplaceText("<<ThiSinh_HocLuc12>>", hocluc);
                document.ReplaceText("<<ThiSinh_HanhKiem12>>", hanhkiem);

                // Lấy ra table đầu tiên trong file word
                var table = document.Tables[0];
                int index = 1;

                // Lấy ra tất cả  nguyện vọng của thí sinh
                foreach (var item in listDkxt)
                {
                    // Chuẩn bị dữ liệu cho table
                    //Từ ngành id lấy ra tên ngành và mã ngành
                    var tenNganh = db.Nganhs.Find(item.Nganh_ID).NganhTenNganh;
                    var maNganh = db.Nganhs.Find(item.Nganh_ID).Nganh_MaNganh;

                    // 1 nguyện vọng chèn 1 row
                    table.InsertRow();
                    // Thêm dữ liệu vào table
                    table.Rows[index].Cells[0].Paragraphs[0].Append(item.Dkxt_NguyenVong.ToString()).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[1].Paragraphs[0].Append(tenNganh).Font("Times New Roman");
                    table.Rows[index].Cells[2].Paragraphs[0].Append(maNganh).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[3].Paragraphs[0].Append(item.Dkxt_ToHopXT).Font("Times New Roman");
                    table.Rows[index].Cells[4].Paragraphs[0].Append(item.Dkxt_DonViToChuc).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[5].Paragraphs[0].Append(item.Dkxt_KetQuaDatDuoc.ToString() + "/" + item.Dkxt_TongDiem.ToString() ).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[6].Paragraphs[0].Append(item.Dkxt_NgayDuThi).Font("Times New Roman").Alignment = Alignment.center;

                    // Tăng giá trị index để fill nguyện vọng tiếp theo
                    index = index + 1;

                }
                // Generate a unique file name
                string fileName = thiSinhInfo.ThiSinh_Ten + "_bieu-mau-xet-ccnn_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".docx";

                // Save the filled document to a temporary location on the server
                string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
                document.SaveAs(tempFilePath);

                // Return the file for download
                byte[] fileBytes = System.IO.File.ReadAllBytes(tempFilePath);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }

        }

        // Xét tuyển đánh giá năng lực đánh giá tư duy
        [Obsolete]
        public ActionResult DownloadFile_XetTuyenDGNL_DGTD()
        {

            var idThiSinhInt = 0;
            // Check login session có tồn tại hay không nếu không tồn tại thì FIX idThiSinhInt = 2 - DEV
            if (Session["login_session"] == null)
            {
                idThiSinhInt = 2;
                System.Diagnostics.Debug.WriteLine("login_session is NULL: ");
            }
            else
            {
                string str_thisinh_session = Session["login_session"].ToString();
                var thisinh_session = db.ThiSinhDangKies.Where(x => x.ThiSinh_MatKhau == str_thisinh_session).FirstOrDefault();
                idThiSinhInt = (int)thisinh_session.ThiSinh_ID;
                System.Diagnostics.Debug.WriteLine("idThiSinhInt: " + idThiSinhInt);
            }

            string templateFilePath = Server.MapPath("~/Content/static/export-65-bieu-mau-xet-diem-dgnl-dgtd.docx");
            // Từ idDkxt lấy ra thông tin thí sinh
            var thiSinhInfo = db.ThiSinhDangKies.Find(idThiSinhInt);
            // Từ id thí sinh lấy ra tất cả nguyện vọng đăng ký xét tuyển của thí sinh
            var listDkxt = db.DangKyXetTuyenKhacs.Where(x => x.ThiSinh_ID == idThiSinhInt && x.Dkxt_ToHopXT == "HDP6").OrderBy(x => x.Dkxt_NguyenVong).ToArray();

            int idKhuVucTS = (int)thiSinhInfo.KhuVuc_ID;
            // Từ khu vực id lấy ra tên khu vực
            var tenKhuVuc = db.KhuVucs.Find(idKhuVucTS).KhuVuc_Ten;

            int idDoiTuongTS = (int)thiSinhInfo.DoiTuong_ID;
            // Từ đối tượng id lấy ra tên đối tượng
            var tenDoiTuong = db.DoiTuongs.Find(idDoiTuongTS).DoiTuong_Ten;
            if (tenDoiTuong == null) tenDoiTuong = " ";
            using (DocX document = DocX.Load(templateFilePath))
            {
                // Replace placeholders with actual data
                string gt = thiSinhInfo.ThiSinh_GioiTinh == 0 ? "Nam" : "Nữ";
                // Replace placeholders with actual data
                document.ReplaceText("<<ThiSinh_HoLot>>", thiSinhInfo.ThiSinh_HoLot);
                document.ReplaceText("<<ThiSinh_Ten>>", thiSinhInfo.ThiSinh_Ten);
                document.ReplaceText("<<ThiSinh_CCCD>>", thiSinhInfo.ThiSinh_CCCD);
                document.ReplaceText("<<ThiSinh_NgaySinh>>", thiSinhInfo.ThiSinh_NgaySinh);
                document.ReplaceText("<<ThiSinh_GioiTinh>>", gt);
                document.ReplaceText("<<ThiSinh_DanToc>>", thiSinhInfo.ThiSinh_DanToc);
                document.ReplaceText("<<ThiSinh_HoKhauThuongTru>>", thiSinhInfo.ThiSinh_HoKhauThuongTru);
                document.ReplaceText("<<ThiSinh_TruongCapBa>>", thiSinhInfo.ThiSinh_TruongCapBa);
                document.ReplaceText("<<ThiSinh_TruongCapBa_Ma>>", thiSinhInfo.ThiSinh_TruongCapBa_Ma);
                document.ReplaceText("<<ThiSinh_DienThoai>>", thiSinhInfo.ThiSinh_DienThoai);
                document.ReplaceText("<<ThiSinh_KhuVuc>>", tenKhuVuc);
                document.ReplaceText("<<ThiSinh_DoiTuong>>", tenDoiTuong);
                document.ReplaceText("<<ThiSinh_DCNhanGiayBao>>", thiSinhInfo.ThiSinh_DCNhanGiayBao);

                // Xử lý học lực và hạnh kiểm
                string hocluc = getHocLucById((int)listDkxt[0].Dkxt_XepLoaiHocLuc_12);
                string hanhkiem = getHanhKiemById((int)listDkxt[0].Dkxt_XepLoaiHanhKiem_12);
                document.ReplaceText("<<ThiSinh_HocLuc12>>", hocluc);
                document.ReplaceText("<<ThiSinh_HanhKiem12>>", hanhkiem);

                // Lấy ra table đầu tiên trong file word
                var table = document.Tables[0];
                int index = 1;

                // Lấy ra tất cả  nguyện vọng của thí sinh
                foreach (var item in listDkxt)
                {
                    // Chuẩn bị dữ liệu cho table
                    //Từ ngành id lấy ra tên ngành và mã ngành
                    var tenNganh = db.Nganhs.Find(item.Nganh_ID).NganhTenNganh;
                    var maNganh = db.Nganhs.Find(item.Nganh_ID).Nganh_MaNganh;

                    // 1 nguyện vọng chèn 1 row
                    table.InsertRow();
                    // Thêm dữ liệu vào table
                    table.Rows[index].Cells[0].Paragraphs[0].Append(item.Dkxt_NguyenVong.ToString()).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[1].Paragraphs[0].Append(tenNganh).Font("Times New Roman");
                    table.Rows[index].Cells[2].Paragraphs[0].Append(maNganh).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[3].Paragraphs[0].Append(item.Dkxt_ToHopXT).Font("Times New Roman");
                    table.Rows[index].Cells[4].Paragraphs[0].Append(item.Dkxt_DonViToChuc).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[5].Paragraphs[0].Append(item.Dkxt_KetQuaDatDuoc.ToString() + "/" + item.Dkxt_TongDiem.ToString() ).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[6].Paragraphs[0].Append(item.Dkxt_NgayDuThi).Font("Times New Roman").Alignment = Alignment.center;

                    // Tăng giá trị index để fill nguyện vọng tiếp theo
                    index = index + 1;

                }
                // Generate a unique file name
                string fileName = thiSinhInfo.ThiSinh_Ten + "_bieu-mau-xet-diem-dgnl-dgtd_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".docx";

                // Save the filled document to a temporary location on the server
                string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
                document.SaveAs(tempFilePath);

                // Return the file for download
                byte[] fileBytes = System.IO.File.ReadAllBytes(tempFilePath);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }

        }
        [Obsolete]
        public ActionResult DownloadFile_XetTuyenKQTQG()
        {

            var idThiSinhInt = 0;
            // Check login session có tồn tại hay không nếu không tồn tại thì FIX idThiSinhInt = 2
            if (Session["login_session"] == null)
            {
                idThiSinhInt = 2;
                System.Diagnostics.Debug.WriteLine("login_session is NULL: ");
            }
            else
            {
                string str_thisinh_session = Session["login_session"].ToString();
                var thisinh_session = db.ThiSinhDangKies.Where(x => x.ThiSinh_MatKhau == str_thisinh_session).FirstOrDefault();
                idThiSinhInt = (int)thisinh_session.ThiSinh_ID;
                System.Diagnostics.Debug.WriteLine("idThiSinhInt: " + idThiSinhInt);
            }

            string templateFilePath = Server.MapPath("~/Content/static/export-61-bieu-mau-xet-thpt-2021-2022.docx");
            // Từ idDkxt lấy ra thông tin thí sinh
            var thiSinhInfo = db.ThiSinhDangKies.Find(idThiSinhInt);
            // Từ id thí sinh lấy ra tất cả nguyện vọng đăng ký xét tuyển của thí sinh
            var listDkxt = db.DangKyXetTuyenKQTQGs.Where(x => x.ThiSinh_ID == idThiSinhInt).OrderBy(x => x.Dkxt_KQTQG_NguyenVong).ToArray();

            int idKhuVucTS = (int)thiSinhInfo.KhuVuc_ID;
            // Từ khu vực id lấy ra tên khu vực
            var tenKhuVuc = db.KhuVucs.Find(idKhuVucTS).KhuVuc_Ten;

            int idDoiTuongTS = (int)thiSinhInfo.DoiTuong_ID;
            // Từ đối tượng id lấy ra tên đối tượng
            var tenDoiTuong = db.DoiTuongs.Find(idDoiTuongTS).DoiTuong_Ten;
            if (tenDoiTuong == null) tenDoiTuong = " ";
            using (DocX document = DocX.Load(templateFilePath))
            {
                // Replace placeholders with actual data
                string gt = thiSinhInfo.ThiSinh_GioiTinh == 0 ? "Nam" : "Nữ";
                // Replace placeholders with actual data
                document.ReplaceText("<<ThiSinh_HoLot>>", thiSinhInfo.ThiSinh_HoLot);
                document.ReplaceText("<<ThiSinh_Ten>>", thiSinhInfo.ThiSinh_Ten);
                document.ReplaceText("<<ThiSinh_CCCD>>", thiSinhInfo.ThiSinh_CCCD);
                document.ReplaceText("<<ThiSinh_NgaySinh>>", thiSinhInfo.ThiSinh_NgaySinh);
                document.ReplaceText("<<ThiSinh_GioiTinh>>", gt);
                document.ReplaceText("<<ThiSinh_DanToc>>", thiSinhInfo.ThiSinh_DanToc);
                document.ReplaceText("<<ThiSinh_HoKhauThuongTru>>", thiSinhInfo.ThiSinh_HoKhauThuongTru);
                document.ReplaceText("<<ThiSinh_TruongCapBa>>", thiSinhInfo.ThiSinh_TruongCapBa);
                document.ReplaceText("<<ThiSinh_TruongCapBa_Ma>>", thiSinhInfo.ThiSinh_TruongCapBa_Ma);
                document.ReplaceText("<<ThiSinh_DienThoai>>", thiSinhInfo.ThiSinh_DienThoai);
                document.ReplaceText("<<ThiSinh_KhuVuc>>", tenKhuVuc);
                document.ReplaceText("<<ThiSinh_DoiTuong>>", tenDoiTuong);
                document.ReplaceText("<<ThiSinh_DCNhanGiayBao>>", thiSinhInfo.ThiSinh_DCNhanGiayBao);

                // Xử lý học lực và hạnh kiểm
                string hocluc = getHocLucById((int)listDkxt[0].Dkxt_KQTQG_XepLoaiHocLuc_12);
                string hanhkiem = getHanhKiemById((int)listDkxt[0].Dkxt_KQTQG_XepLoaiHanhKiem_12);
                document.ReplaceText("<<ThiSinh_HocLuc12>>", hocluc);
                document.ReplaceText("<<ThiSinh_HanhKiem12>>", hanhkiem);

                // Lấy ra table đầu tiên trong file word
                var table = document.Tables[0];
                int index = 1;

                // Lấy ra tất cả  nguyện vọng của thí sinh
                foreach (var item in listDkxt)
                {
                    // Chuẩn bị dữ liệu cho table
                    //Từ ngành id lấy ra tên ngành và mã ngành
                    var tenNganh = db.Nganhs.Find(item.Nganh_ID).NganhTenNganh;
                    var maNganh = db.Nganhs.Find(item.Nganh_ID).Nganh_MaNganh;
                   
                    // DiemThiGQMon khai báo trong  Model.LibraryUsers
                    DiemThiGQMon diemmon1 = JsonConvert.DeserializeObject<DiemThiGQMon>(item.Dkxt_KQTQG_Diem_M1);
                    DiemThiGQMon diemmon2 = JsonConvert.DeserializeObject<DiemThiGQMon>(item.Dkxt_KQTQG_Diem_M2);
                    DiemThiGQMon diemmon3 = JsonConvert.DeserializeObject<DiemThiGQMon>(item.Dkxt_KQTQG_Diem_M3);

                    // Lấy ra thông tin chi tiết của từng môn1
                    var mon1_tenmon = diemmon1.TenMon.ToString();
                    var mon1_diem = diemmon1.Diem.ToString();
                    // Lấy ra thông tin chi tiết của từng môn2
                    var mon2_tenmon = diemmon2.TenMon.ToString();
                    var mon2_diem = diemmon2.Diem.ToString();
                    // Lấy ra thông tin chi tiết của từng môn3
                    var mon3_tenmon = diemmon3.TenMon.ToString();
                    var mon3_diem = diemmon3.Diem.ToString();

                    var tongdiem = item.Dkxt_KQTQG_Diem_Tong;

                    // 1 nguyện vọng chèn 3 row
                    table.InsertRow();
                    table.InsertRow();
                    table.InsertRow();
                    // Thêm dữ liệu vào table
                    table.Rows[index].Cells[0].Paragraphs[0].Append(item.Dkxt_KQTQG_NguyenVong.ToString()).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[0].VerticalAlignment = VerticalAlignment.Center; ;

                    table.Rows[index].Cells[1].Paragraphs[0].Append(tenNganh).Font("Times New Roman");
                    table.Rows[index].Cells[1].VerticalAlignment = VerticalAlignment.Center; ;

                    table.Rows[index].Cells[2].Paragraphs[0].Append(maNganh).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[2].VerticalAlignment = VerticalAlignment.Center; ;

                    table.Rows[index].Cells[3].Paragraphs[0].Append("Môn 1:" + mon1_tenmon).Font("Times New Roman");
                    table.Rows[index].Cells[4].Paragraphs[0].Append(mon1_diem).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[5].Paragraphs[0].Append(tongdiem).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[5].VerticalAlignment = VerticalAlignment.Center; ;


                    table.Rows[index + 1].Cells[3].Paragraphs[0].Append("Môn 2: " + mon2_tenmon).Font("Times New Roman");
                    table.Rows[index + 1].Cells[4].Paragraphs[0].Append(mon2_diem).Font("Times New Roman").Alignment = Alignment.center;

                    table.Rows[index + 2].Cells[3].Paragraphs[0].Append("Môn 3: " + mon3_tenmon).Font("Times New Roman");
                    table.Rows[index + 2].Cells[4].Paragraphs[0].Append(mon3_diem).Font("Times New Roman").Alignment = Alignment.center;

                    // Merge đúng định dạng
                    table.MergeCellsInColumn(0, index, index + 2);
                    table.MergeCellsInColumn(1, index, index + 2);
                    table.MergeCellsInColumn(2, index, index + 2);
                    table.MergeCellsInColumn(5, index, index + 2);

                    // Tăng giá trị index để fill nguyện vọng tiếp theo
                    index = index + 3;

                }
                // Generate a unique file name
                string fileName = thiSinhInfo.ThiSinh_Ten + "_bieu-mau-xet-thpt-2021-2022_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".docx";

                // Save the filled document to a temporary location on the server
                string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
                document.SaveAs(tempFilePath);

                // Return the file for download
                byte[] fileBytes = System.IO.File.ReadAllBytes(tempFilePath);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }

        }
        
        // Xét tuyển thi năng khiếu
        [Obsolete]
        public ActionResult DownloadFile_XetTuyenThiNangKhieu()
        {
            var idThiSinhInt = 0;
            // Check login session có tồn tại hay không nếu không tồn tại thì FIX idThiSinhInt = 2
            if (Session["login_session"] == null)
            {
                idThiSinhInt = 2;
                System.Diagnostics.Debug.WriteLine("login_session is NULL: ");
            }
            else
            {
                string str_thisinh_session = Session["login_session"].ToString();
                var thisinh_session = db.ThiSinhDangKies.Where(x => x.ThiSinh_MatKhau == str_thisinh_session).FirstOrDefault();
                idThiSinhInt = (int)thisinh_session.ThiSinh_ID;
                System.Diagnostics.Debug.WriteLine("idThiSinhInt: " + idThiSinhInt);
            }

            string templateFilePath = Server.MapPath("~/Content/static/export-66-bieu-mau-thi-nk-20230302101245-e.docx");
            // Từ idDkxt lấy ra thông tin thí sinh
            var thiSinhInfo = db.ThiSinhDangKies.Find(idThiSinhInt);
            // Từ id thí sinh lấy ra nguyện vọng 1 của thí sinh
            var nv1_dkxt = db.DangKyDuThiNangKhieus.Where(x => x.ThiSinh_ID == idThiSinhInt && x.Dkdt_NK_NguyenVong == 1);

            // từ id ngành lấy ra mã ngành và tên ngành
            var maNganh = db.Nganhs.Where(x => x.Nganh_ID == nv1_dkxt.FirstOrDefault().Nganh_ID).FirstOrDefault().Nganh_MaNganh;

            using (DocX document = DocX.Load(templateFilePath))
            {
                // Replace placeholders with actual data
                document.ReplaceText("<<ThiSinh_HoLot>>", thiSinhInfo.ThiSinh_HoLot);
                document.ReplaceText("<<ThiSinh_Ten>>", thiSinhInfo.ThiSinh_Ten);
                document.ReplaceText("<<ThiSinh_CCCD>>", thiSinhInfo.ThiSinh_CCCD);
                document.ReplaceText("<<ThiSinh_NgaySinh>>", thiSinhInfo.ThiSinh_NgaySinh);
                document.ReplaceText("<<ThiSinh_DienThoai>>", thiSinhInfo.ThiSinh_DienThoai);
                document.ReplaceText("<<ThiSinh_DCNhanGiayBao>>", thiSinhInfo.ThiSinh_DCNhanGiayBao);
                document.ReplaceText("<<ThiSinh_Email>>", thiSinhInfo.ThiSinh_Email);
                document.ReplaceText("<<ThiSinh_Email>>", thiSinhInfo.ThiSinh_Email);

                //Nganh_ID	Nganh_MaNganh	NganhTenNganh	Khoa_ID	Nganh_GhiChu
                //4    7140201 Giáo dục Mầm non    5   7140201 Giáo dục Mầm non
                // 5   7140206 Giáo dục thể chất   11  7140206 Giáo dục thể chất
                // 6   7140202 Giáo dục Tiểu học   4   7140202 Giáo dục Tiểu học
                //Ngành sử dụng môn thi năng khiếu để xét tuyển vào(đánh dấu X vào ô trống):
                var checkbox_x = "x";
                var xt_cb_mn = " ";
                var xt_cb_th = " ";
                var xt_cb_tc = " ";
                var thi_cb_mnth = " ";
                var thi_cb_tc = " ";

                if (maNganh == "7140201")
                {
                    xt_cb_mn = checkbox_x;
                    thi_cb_mnth = checkbox_x;
                }
                else if(maNganh == "7140202")
                {
                    xt_cb_th = checkbox_x;
                    thi_cb_mnth = checkbox_x;
                }
                else if(maNganh == "7140206")
                {
                    xt_cb_tc = checkbox_x;
                    thi_cb_tc = checkbox_x;
                }
                document.ReplaceText("<<xt_cb_mn>>", xt_cb_mn);
                document.ReplaceText("<<xt_cb_th>>", xt_cb_th);
                document.ReplaceText("<<xt_cb_tc>>", xt_cb_tc);

                //Đăng kí môn thi năng khiếu(chỉ đánh dấu X vào 1 ô trống):
                document.ReplaceText("<<thi_cb_mnth>>", thi_cb_mnth);
                document.ReplaceText("<<thi_cb_tc>>", thi_cb_tc);

                // Generate a unique file name
                string fileName = thiSinhInfo.ThiSinh_Ten + "_bieu-mau-thi-nk_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".docx";

                // Save the filled document to a temporary location on the server
                string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
                document.SaveAs(tempFilePath);

                // Return the file for download
                byte[] fileBytes = System.IO.File.ReadAllBytes(tempFilePath);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }

        }

        public string getHocLucById(int id)
        {
            switch (id)
            {
                case 1:
                    return "Trung bình";
                case 2:
                    return "Khá";
                case 3:
                    return "Giỏi";
                case 4:
                    return "Xuất sắc";
                default:
                    return " ";
            }
        }

        public string getHanhKiemById(int id)
        {
            switch (id)
            {
                case 1:
                    return "Yếu";
                case 2:
                    return "Trung bình";
                case 3:
                    return "Khá";
                case 4:
                    return "Tốt";
                default:
                    return " ";
            }
        }

        // Sử dụng cho phương thức xét tuyển thẳng
        public string getTenMonById(int id)
        {
            switch (id)
            {
                case 1:
                    return "Toán";
                case 2:
                    return "Vật lí";
                case 3:
                    return "Hóa học";
                case 4:
                    return "Sinh học";
                case 5:
                    return "Tin học";
                case 6:
                    return "Ngữ văn";
                case 7:
                    return "Lịch sử";
                case 8:
                    return "Địa lí";
                case 9:
                    return "Tiếng Anh";
                case 10:
                    return "Tiếng Nga";
                case 11:
                    return "Tiếng Pháp";
                case 12:
                    return "Tiếng Trung";
                default:
                    return " ";
            }
        }

        public string getGiaiById(int id)
        {
            switch (id)
            {
                case 1:
                    return "Giải nhất";
                case 2:
                    return "Giải nhì";
                case 3:
                    return "Giải ba";
                default:
                    return " ";
            }
        }
    }
}   