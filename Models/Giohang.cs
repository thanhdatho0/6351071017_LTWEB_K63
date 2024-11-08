using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBookStore.Models
{
    public class Giohang
    {
        private readonly QLBANSACHEntities _context = new QLBANSACHEntities();

        public int iMasach {  get; set; }
        public string sTensach { get; set; }
        public string sAnhbia { get; set; }
        public double dDongia { get; set; }
        public int iSoluong { get; set; }

        public double dThanhtien
        {
            get => iSoluong * dDongia;
        }

        public Giohang(int Masach)
        {
            iMasach = Masach;
            SACH sach = _context.SACHes.Single(n => n.Masach == iMasach);
            sTensach = sach.Tensach;
            sAnhbia = sach.Anhbia;
            dDongia = double.Parse(sach.Giaban.ToString());
            iSoluong = 1;
        }
    }
}