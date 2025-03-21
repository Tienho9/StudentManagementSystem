using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapNhom
{
    internal class SinhVien
    {
        private string _MaSV;
        private string _HoTen;
        private string _MaLop;
        private string _MaKhoa;
        private DateTime _NgaySinh;
        private string _GioiTinh;
        private string _DiaChi;

        public SinhVien()
        {
        }

        public SinhVien(string maSV, string hoTen, string maLop, string maKhoa, string gioiTinh, DateTime ngaySinh, string diaChi)
        {
            _MaSV = maSV;
            _HoTen = hoTen;
            _DiaChi = diaChi;
            _MaLop = maLop;
            _GioiTinh = gioiTinh;
            _NgaySinh = ngaySinh;
            _MaKhoa = maKhoa;
        }

        public string MaSV { get => _MaSV; set => _MaSV = value; }
        public string HoTen { get => _HoTen; set => _HoTen = value; }
        public string DiaChi { get => _DiaChi; set => _DiaChi = value; }
        public string MaLop { get => _MaLop; set => _MaLop = value; }
        public string GioiTinh { get => _GioiTinh; set => _GioiTinh = value; }
        public DateTime NgaySinh { get => _NgaySinh; set => _NgaySinh = value; }
        public string MaKhoa { get => _MaKhoa; set => _MaKhoa = value; }
    }
}
