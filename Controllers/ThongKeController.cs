using Microsoft.AspNetCore.Mvc;
using MvcBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcBookStore.Controllers
{
    public class ThongKeController : Controller
    {
        QLBANSACHEntities1 db = new QLBANSACHEntities1();
        // GET: ThongKe
        public ActionResult Index()
        {
            // Lấy dữ liệu số lượng sách theo từng chủ đề từ bảng CHUDE
            var thongKeData = db.CHUDEs
                .Select(chude => new ThongKeViewModel
                {
                    TenChuDe = chude.TenChuDe,
                    SoLuong = chude.SACHes.Count()
                })
                .ToList();

            return View(thongKeData);
        }

        public ActionResult ThongKePartial()
        {
            return PartialView();
        }
    }
}