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
            // HARCODE 
            //var idThiSinhInt = int.Parse(idThiSinh);
            var idThiSinhInt = 2;
            string templateFilePath = Server.MapPath("~/Content/static/Mau_HB.docx");
            // Từ idDkxt lấy ra thông tin thí sinh
            var thiSinhInfo = db.ThiSinhDangKies.Find(idThiSinhInt);
            // Từ id thí sinh lấy ra tất cả nguyện vọng đăng ký xét tuyển của thí sinh
            var listDkxt = db.DangKyXetTuyens.Where(x => x.ThiSinh_ID == idThiSinhInt).ToArray();

            int idKhuVucTS = (int) thiSinhInfo.KhuVuc_ID;
            // Từ khu vực id lấy ra tên khu vực
            var tenKhuVuc = db.KhuVucs.Find(idKhuVucTS).KhuVuc_Ten;

            int idDoiTuongTS = (int) thiSinhInfo.DoiTuong_ID;
            // Từ đối tượng id lấy ra tên đối tượng
            var tenDoiTuong = db.DoiTuongs.Find(idDoiTuongTS).DoiTuong_Ten;

            // Map các list đăng ký xét tuyển lấy ra thông tin chi tiết  của mỗi bản ghi
            //foreach(var item in listDkxt)
            //{
            //    thông tin chi tiết
            //}
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
                document.ReplaceText("<<ThiSinh_Doituong>>", tenDoiTuong);

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

                    DiemMon1 diemon1 = JsonConvert.DeserializeObject<DiemMon1>(item.Dkxt_Diem_M1);
                    DiemMon2 diemon2 = JsonConvert.DeserializeObject<DiemMon2>(item.Dkxt_Diem_M2);
                    DiemMon3 diemon3 = JsonConvert.DeserializeObject<DiemMon3>(item.Dkxt_Diem_M3);

                    // Lấy ra thông tin chi tiết của từng môn1
                    var mon1_tenmon = diemon1.TenMon1.ToString();
                    var mon1_hk1 = diemon1.HK1.ToString();
                    var mon1_hk2 = diemon1.HK2.ToString();
                    var mon1_hk3 = diemon1.HK3.ToString();
                    var mon1_diemtb = diemon1.DiemTrungBinh.ToString();

                    // Lấy ra thông tin chi tiết của từng môn2
                    var mon2_tenmon = diemon2.TenMon2.ToString();
                    var mon2_hk1 = diemon2.HK1.ToString();
                    var mon2_hk2 = diemon2.HK2.ToString();
                    var mon2_hk3 = diemon2.HK3.ToString();
                    var mon2_diemtb = diemon2.DiemTrungBinh.ToString();

                    // Lấy ra thông tin chi tiết của từng môn3
                    var mon3_tenmon = diemon3.TenMon3.ToString();
                    var mon3_hk1 = diemon3.HK1.ToString();
                    var mon3_hk2 = diemon3.HK2.ToString();
                    var mon3_hk3 = diemon3.HK3.ToString();
                    var mon3_diemtb = diemon3.DiemTrungBinh.ToString();

                    // 1 nguyện vọng chèn 3 row
                    table.InsertRow();
                    table.InsertRow();
                    table.InsertRow();

                    // Thêm dữ liệu vào table
                    table.Rows[index].Cells[0].Paragraphs[0].Append(item.Dkxt_NguyenVong.ToString()).Font("Times New Roman");
                    table.Rows[index].Cells[1].Paragraphs[0].Append(tenNganh).Font("Times New Roman");
                    table.Rows[index].Cells[2].Paragraphs[0].Append(maNganh).Font("Times New Roman");
                    table.Rows[index].Cells[3].Paragraphs[0].Append("Môn 1:" + mon1_tenmon).Font("Times New Roman");
                    table.Rows[index].Cells[4].Paragraphs[0].Append(mon1_hk1).Font("Times New Roman");
                    table.Rows[index].Cells[5].Paragraphs[0].Append(mon1_hk2).Font("Times New Roman");
                    table.Rows[index].Cells[6].Paragraphs[0].Append(mon1_hk3).Font("Times New Roman");
                    table.Rows[index].Cells[7].Paragraphs[0].Append(mon1_diemtb).Font("Times New Roman");

                    table.Rows[index + 1].Cells[3].Paragraphs[0].Append("Môn 2: " + mon2_tenmon).Font("Times New Roman");
                    table.Rows[index + 1].Cells[4].Paragraphs[0].Append(mon2_hk1).Font("Times New Roman");
                    table.Rows[index + 1].Cells[5].Paragraphs[0].Append(mon2_hk2).Font("Times New Roman");
                    table.Rows[index + 1].Cells[6].Paragraphs[0].Append(mon2_hk3).Font("Times New Roman");
                    table.Rows[index + 1].Cells[7].Paragraphs[0].Append(mon2_diemtb).Font("Times New Roman");

                    table.Rows[index + 2].Cells[3].Paragraphs[0].Append("Môn 1: " + mon3_tenmon).Font("Times New Roman");
                    table.Rows[index + 2].Cells[4].Paragraphs[0].Append(mon3_hk1).Font("Times New Roman");
                    table.Rows[index + 2].Cells[5].Paragraphs[0].Append(mon3_hk2).Font("Times New Roman");
                    table.Rows[index + 2].Cells[6].Paragraphs[0].Append(mon3_hk3).Font("Times New Roman");
                    table.Rows[index + 2].Cells[7].Paragraphs[0].Append(mon3_diemtb).Font("Times New Roman");

                    // Merge đúng định dạng
                    table.MergeCellsInColumn(0, index, index + 2);
                    table.MergeCellsInColumn(1, index, index + 2);
                    table.MergeCellsInColumn(2, index, index + 2);

                    // Tăng giá trị index để fill nguyện vọng tiếp theo
                    index = index + 3;

                }
                // Generate a unique file name
                string fileName = thiSinhInfo.ThiSinh_Ten + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".docx";

                // Save the filled document to a temporary location on the server
                string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
                document.SaveAs(tempFilePath);

                // Return the file for download
                byte[] fileBytes = System.IO.File.ReadAllBytes(tempFilePath);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }
        }
    }
}