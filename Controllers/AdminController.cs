using MvcBookStore.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using System.IO;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using System.Data;

namespace MvcBookStore.Controllers
{
    public class AdminController : Controller
    {
        QLBANSACHEntities1 db = new QLBANSACHEntities1();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection f) 
        {
            var username = f["username"];
            var password = f["password"];
            if (string.IsNullOrEmpty(username))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }else if(string.IsNullOrEmpty(password))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == username && n.PassAdmin == password);
                if(ad != null)
                {
                    Session["TaiKhoanAdmin"] = ad;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

        public ActionResult Sach(int? page)
        {
            int pageNum = page ?? 1;
            int pageSize = 5;
            return View(db.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult ThemMoiSach()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiSach(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            // Kiểm tra đường dẫn file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View(sach);
            }
            else
            {
                // Thêm vào CSDL
                if (ModelState.IsValid)
                {
                    // Lưu tên file, lưu ý bổ sung thư viện using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    // Lưu đường dẫn của file
                    var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);

                    // Kiểm tra hình ảnh tồn tại chưa?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        // Lưu hình ảnh vào đường dẫn
                        fileUpload.SaveAs(path);
                    }
                    sach.Anhbia = fileName;
                    Console.WriteLine(sach.Tensach);
                    Console.WriteLine("Hello");
                    // Lưu vào CSDL
                    db.SACHes.Add(sach);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            }
                        }
                    }
                }
                return RedirectToAction("Sach");
            }
        }

        public ActionResult ChiTietSach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpGet]
        public ActionResult XoaSach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(s => s.Masach == id);
            ViewBag.Masach = sach.Masach;
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost, ActionName("XoaSach")]
        public ActionResult XacNhanXoa(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(s => s.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SACHes.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("Sach");
        }

        [HttpGet]
        public ActionResult SuaSach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(s => s.Masach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSach(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            // Kiểm tra đường dẫn file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View(sach);
            }
            else
            {
                // Thêm vào CSDL
                if (ModelState.IsValid)
                {
                    // Lưu tên file, lưu ý bổ sung thư viện using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    // Lưu đường dẫn của file
                    var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);

                    // Kiểm tra hình ảnh tồn tại chưa?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        // Lưu hình ảnh vào đường dẫn
                        fileUpload.SaveAs(path);
                    }
                    sach.Anhbia = fileName;
                    db.Entry(sach).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Sach");
            }
        }
    }
}