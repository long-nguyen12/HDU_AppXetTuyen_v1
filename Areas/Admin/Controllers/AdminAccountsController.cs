using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using HDU_AppXetTuyen.Models;
using PagedList;
using BC = BCrypt.Net.BCrypt;

namespace HDU_AppXetTuyen.Areas.Admin.Controllers
{
    public class AdminAccountsController : Controller
    {
        private DbConnecttion db = new DbConnecttion();

        // GET: Admin/AdminAccounts
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var adminAccounts = (from h in db.AdminAccounts
                                 select h).OrderBy(x => x.Admin_ID);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(adminAccounts.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/AdminAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminAccount adminAccount = db.AdminAccounts.Find(id);
            if (adminAccount == null)
            {
                return HttpNotFound();
            }
            return View(adminAccount);
        }

        // GET: Admin/AdminAccounts/Create
        public ActionResult Create()
        {
            ViewBag.Khoa_ID = new SelectList(db.Khoas, "Khoa_ID", "Khoa_TenKhoa");
            return View();
        }

        // POST: Admin/AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Admin_ID,Admin_Username,Admin_Pass,Admin_Ho,Admin_Ten,Admin_Quyen,Khoa_ID,Admin_Note")] AdminAccount adminAccount)
        {
            if (ModelState.IsValid)
            {
                var password = ComputeHash(adminAccount.Admin_Username, adminAccount.Admin_Pass);
                adminAccount.Admin_Pass = password;
                // Sét mặc định Khoa_ID = 1
                adminAccount.Khoa_ID = 1;
                db.AdminAccounts.Add(adminAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Khoa_ID = new SelectList(db.Khoas, "Khoa_ID", "Khoa_TenKhoa", adminAccount.Khoa_ID);
            return View(adminAccount);
        }

        // GET: Admin/AdminAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminAccount adminAccount = db.AdminAccounts.Find(id);
            //adminAccount.Admin_Pass = "";
            if (adminAccount == null)
            {
                return HttpNotFound();
            }
            return View(adminAccount);
        }

        // POST: Admin/AdminAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Admin_ID,Admin_Username,Admin_Pass,Admin_Ho,Admin_Ten,Admin_Quyen,Khoa_ID,Admin_Note")] AdminAccount adminAccount)
        {
            if (ModelState.IsValid)
            {
                adminAccount.Khoa_ID = 1;
                db.Entry(adminAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adminAccount);
        }

        // GET: Admin/AdminAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminAccount adminAccount = db.AdminAccounts.Find(id);
            if (adminAccount == null)
            {
                return HttpNotFound();
            }
            return View(adminAccount);
        }

        // POST: Admin/AdminAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminAccount adminAccount = db.AdminAccounts.Find(id);
            db.AdminAccounts.Remove(adminAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(FormCollection formCollection)
        {
            var adminAccount = db.AdminAccounts.Find(int.Parse(formCollection["Admin_ID"]));

            if (adminAccount != null)
            {
                var password = ComputeHash(adminAccount.Admin_Username, formCollection["pass_new"]);
                adminAccount.Admin_Pass = password;
                db.Entry(adminAccount).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "AdminAccounts");

        }
        public string ComputeHash(string input_user, string input_pass)
        {
            string input = input_user.Trim() + input_pass.Trim();
            string hashedPassword = BC.HashPassword(input);
            return hashedPassword;
        }

        public bool Verify(string input_user, string input_pass, string hash_pass)
        {
            string input = input_user.Trim() + input_pass.Trim();
            bool isPasswordValid = BC.Verify(input, hash_pass);
            return isPasswordValid;
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
