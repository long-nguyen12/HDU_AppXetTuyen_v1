﻿using HDU_AppXetTuyen.Models;
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
using HDU_AppXetTuyen.Ultils;
using System.Web.UI.WebControls;

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
        public ActionResult DownloadFile_XetTuyenHB(string idThiSinh)
        {
            // Không cần sử dụng string idThiSinh vì lấy id Thí sinh từ session

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
            var listDkxt = db.DangKyXetTuyens.Where(x => x.ThiSinh_ID == idThiSinhInt).OrderBy(x => x.Dkxt_NguyenVong).ToArray();

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

                    // string to obj   
                    var jsonStringMon1 = JsonConvert.SerializeObject(item.Dkxt_Diem_M1);
                    var jsonStringMon2 = JsonConvert.SerializeObject(item.Dkxt_Diem_M2);
                    var jsonStringMon3 = JsonConvert.SerializeObject(item.Dkxt_Diem_M3);

                    DiemMon1 diemmon1 = JsonConvert.DeserializeObject<DiemMon1>(item.Dkxt_Diem_M1);
                    DiemMon2 diemmon2 = JsonConvert.DeserializeObject<DiemMon2>(item.Dkxt_Diem_M2);
                    DiemMon3 diemmon3 = JsonConvert.DeserializeObject<DiemMon3>(item.Dkxt_Diem_M3);

                    // Lấy ra thông tin chi tiết của từng môn1
                    var mon1_tenmon = diemmon1.TenMon1.ToString();
                    var mon1_hk1 = diemmon1.HK1.ToString();
                    var mon1_hk2 = diemmon1.HK2.ToString();
                    var mon1_hk3 = diemmon1.HK3.ToString();
                    var mon1_diemtb = diemmon1.DiemTrungBinh != null ? diemmon1.DiemTrungBinh.ToString() : " ";

                    // Lấy ra thông tin chi tiết của từng môn2
                    var mon2_tenmon = diemmon2.TenMon2.ToString();
                    var mon2_hk1 = diemmon2.HK1.ToString();
                    var mon2_hk2 = diemmon2.HK2.ToString();
                    var mon2_hk3 = diemmon2.HK3.ToString();
                    var mon2_diemtb = diemmon2.DiemTrungBinh != null ? diemmon2.DiemTrungBinh.ToString() : " ";

                    // Lấy ra thông tin chi tiết của từng môn3
                    var mon3_tenmon = diemmon3.TenMon3.ToString();
                    var mon3_hk1 = diemmon3.HK1.ToString();
                    var mon3_hk2 = diemmon3.HK2.ToString();
                    var mon3_hk3 = diemmon3.HK3.ToString();
                    var mon3_diemtb = diemmon3.DiemTrungBinh != null ? diemmon3.DiemTrungBinh.ToString() : " ";

                    // 1 nguyện vọng chèn 3 row
                    table.InsertRow();
                    table.InsertRow();
                    table.InsertRow();
                    // Thêm dữ liệu vào table
                    table.Rows[index].Cells[0].Paragraphs[0].Append(item.Dkxt_NguyenVong.ToString()).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[1].Paragraphs[0].Append(tenNganh).Font("Times New Roman");
                    table.Rows[index].Cells[2].Paragraphs[0].Append(maNganh).Font("Times New Roman").Alignment = Alignment.center;
                    table.Rows[index].Cells[3].Paragraphs[0].Append("Môn 1:" + mon1_tenmon).Font("Times New Roman");
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
        public ActionResult DownloadFile_XetTuyenThang(string idThiSinh)
        {
            // Không cần sử dụng string idThiSinh vì lấy id Thí sinh từ session

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
        public ActionResult DownloadFile_XetTuyenCCNN(string idThiSinh)
        {
            // Không cần sử dụng string idThiSinh vì lấy id Thí sinh từ session

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
        public ActionResult DownloadFile_XetTuyenDGNL_DGTD(string idThiSinh)
        {
            // Không cần sử dụng string idThiSinh vì lấy id Thí sinh từ session

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
        public ActionResult DownloadFile_XetTuyenKQTQG(string idThiSinh)
        {
            // Không cần sử dụng string idThiSinh vì lấy id Thí sinh từ session

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

                    DiemThiGQMon1 diemmon1 = JsonConvert.DeserializeObject<DiemThiGQMon1>(item.Dkxt_KQTQG_Diem_M1);
                    DiemThiGQMon2 diemmon2 = JsonConvert.DeserializeObject<DiemThiGQMon2>(item.Dkxt_KQTQG_Diem_M2);
                    DiemThiGQMon3 diemmon3 = JsonConvert.DeserializeObject<DiemThiGQMon3>(item.Dkxt_KQTQG_Diem_M3);

                    // Lấy ra thông tin chi tiết của từng môn1
                    var mon1_tenmon = diemmon1.TenMon1.ToString();
                    var mon1_diem = diemmon1.DiemM1.ToString();
                    // Lấy ra thông tin chi tiết của từng môn2
                    var mon2_tenmon = diemmon2.TenMon2.ToString();
                    var mon2_diem = diemmon2.DiemM2.ToString();
                    // Lấy ra thông tin chi tiết của từng môn3
                    var mon3_tenmon = diemmon3.TenMon3.ToString();
                    var mon3_diem = diemmon3.DiemM3.ToString();

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