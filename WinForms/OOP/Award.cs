using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapNhom
{
    internal class Award
    {
        private string _maKT;
        private string _maSV;
        private DateTime _ngayKT;
        private string _loaiKT;
        private string _lyDo;

        public Award()
        {
        }

        public Award(string maKT, string maSV, DateTime ngayKT, string loaiKT, string lyDo) // (123, abc, 4, 5);
        {
            _maKT = maKT;
            _maSV = maSV;
            _ngayKT = ngayKT;
            _loaiKT = loaiKT;
            _lyDo = lyDo;
        }

        public string MaKT { get => _maKT; set => _maKT = value; } // (lấy MaMH, gán MaMH)
        public string MaSV { get => _maSV; set => _maSV = value; }
        public DateTime NgayKT { get => _ngayKT; set => _ngayKT = value; }
        public string LoaiKT { get => _loaiKT; set => _loaiKT = value; }
        public string LyDo { get => _lyDo ; set => _lyDo = value; }
    }
}

