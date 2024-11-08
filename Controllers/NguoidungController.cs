using MvcBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBookStore.Controllers
{
    public class NguoidungController : Controller
    {
        private readonly QLBANSACHEntities _context = new QLBANSACHEntities();
        

        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:M/d/yyyy}", collection["Ngaysinh"]);

            // Kiểm tra Họ tên
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            // Kiểm tra Tên đăng nhập
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập";
            }
            // Kiểm tra Mật khẩu
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            }
            // Kiểm tra Mật khẩu nhập lại
            if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }
            // Kiểm tra Email
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được bỏ trống";
            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Địa chỉ không được bỏ trống";
            }
            // Kiểm tra Điện thoại
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = "Phải nhập điện thoại";
            }

            // Nếu không có lỗi, tiến hành lưu thông tin khách hàng
            if (String.IsNullOrEmpty(ViewData["Loi1"] as string) &&
                String.IsNullOrEmpty(ViewData["Loi2"] as string) &&
                String.IsNullOrEmpty(ViewData["Loi3"] as string) &&
                String.IsNullOrEmpty(ViewData["Loi4"] as string) &&
                String.IsNullOrEmpty(ViewData["Loi5"] as string) &&
                String.IsNullOrEmpty(ViewData["Loi6"] as string))
            {
                // Gán giá trị cho đối tượng khách hàng (kh)
                kh.HoTen = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienthoaiKH = dienthoai;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);

                // Thêm khách hàng vào cơ sở dữ liệu
                _context.KHACHHANGs.Add(kh);
                _context.SaveChanges();

                // Chuyển hướng về trang đăng nhập
                return RedirectToAction("Dangnhap");
            }

            // Trường hợp quay lại trang đăng ký nếu có lỗi
            return this.Dangky();

        }
        
        [HttpGet]    
        public ActionResult DangNhap()
        {
            return View();
        }

        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = _context.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "BookStore");
                }
                else
                    ViewBag.Thongbao = "Sai tên đăng nhập hoặc mật khẩu";
            }
            return View();
        }
    }

}