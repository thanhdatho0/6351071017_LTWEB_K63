using MvcBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBookStore.Controllers
{
    public class GiohangController : Controller
    {
        private readonly QLBANSACHEntities _context = new QLBANSACHEntities();
        // GET: Giohang
        public ActionResult Index()
        {
            return View();
        }

        public List<Giohang> Laygiohang()
        {
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if(listGiohang == null)
            {
                listGiohang = new List<Giohang>();
                Session["Giohang"] = listGiohang;
            }
            return listGiohang;
        }

        public ActionResult ThemGioHang(int Masach, string strURL)
        {
            List<Giohang> listGiohang = Laygiohang();
            Giohang sanPham = listGiohang.Find(n => n.iMasach == Masach);
            if(sanPham == null)
            {
                sanPham = new Giohang(Masach);
                listGiohang.Add(sanPham);
                return Redirect(strURL);
            }
            else
            {
                sanPham.iSoluong++;
                return Redirect(strURL);
            }
        }

        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> listGiohang = Session["GioHang"] as List <Giohang>;
            if(listGiohang != null)
            {
                iTongSoLuong = listGiohang.Sum(n => n.iSoluong); 
            }
            return iTongSoLuong;
        }

        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> listGiohang = Session["GioHang"] as List<Giohang>;
            if (listGiohang != null)
            {
                iTongTien = listGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }

        public ActionResult GioHang()
        {
            List<Giohang> listGiohang = Laygiohang();
            if(listGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(listGiohang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        public ActionResult XoaGioHang(int iMaSP)
        {
            List<Giohang> listGioHang = Laygiohang();
            Giohang sp = listGioHang.SingleOrDefault(n => n.iMasach == iMaSP);
            if(sp != null)
            {
                listGioHang.RemoveAll(n => n.iMasach == iMaSP);
                return RedirectToAction("GioHang");
            }
            if(listGioHang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int iMaSP, FormCollection f)
        {
            List<Giohang> listGioHang = Laygiohang();
            Giohang sp = listGioHang.SingleOrDefault(n => n.iMasach == iMaSP);

            if(sp != null)
            {
                sp.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            Session["Giohang"] = null;
            return RedirectToAction("Index", "BookStore");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "BookStore");
            }

            List<Giohang> listGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(listGioHang);
        }

        public ActionResult DatHang(FormCollection f)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", f["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            _context.DONDATHANGs.Add(ddh);
            _context.SaveChanges();

            foreach(var item in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.Masach = item.iMasach;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal) item.dDongia;
                _context.CHITIETDONTHANGs.Add(ctdh);
            }

            _context.SaveChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}