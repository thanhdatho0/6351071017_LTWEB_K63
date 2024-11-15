using MvcBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace MvcBookStore.Controllers
{
    public class BookStoreController : Controller
    {
        QLBANSACHEntities1 data = new QLBANSACHEntities1();
        private List<SACH> Laysachmoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).ToList();
        }


        public ActionResult Index(int? page)
        {
            int pageSize = 2;
            int pageNum = (page ?? 1);
            var sachmoi = Laysachmoi(pageSize);
            return View(sachmoi.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes where s.Masach == id select s;
            return View(sach.Single());
        }

        public ActionResult CHUDE()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }

        public ActionResult SPTheochude(int id, int? page)
        {
            int pageSize = 2;
            int pageNum = (page ?? 1);
            var sach = from s in data.SACHes where s.MaCD == id orderby s.Tensach select s;
            return View(sach.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Nhaxuatban()
        {
            var nxb = from cd in data.NHAXUATBANs select cd;
            return PartialView(nxb);
        }

        public ActionResult SPTheoNXB(int id, int? page)
        {
            int pageSize = 2;
            int pageNum = (page ?? 1);
            var sach = from s in data.SACHes where s.MaNXB == id orderby s.Tensach select s;
            return View(sach.ToPagedList(pageNum, pageSize));
        }
    }
}