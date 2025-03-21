using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapNhom
{
    public partial class FormRePassword : Form
    {
        public FormRePassword()
        {
            InitializeComponent();
            lblKQ.Text = "";
        }
        Modify modify = new Modify();

        private void btnLayMK_Click(object sender, EventArgs e)
        {
            string email = txtEmailDK.Text;
            lblThongBao.Text = "";
            if (email.Trim() == "")
            {
                lblThongBao.Text = "Bạn chưa nhập Email";
                return;
            }
            else
            {
                string query = "Select * from TaiKhoan where Email = '" + email + "'";
                if (modify.TaiKhoans(query).Count() > 0)
                {
                    lblKQ.ForeColor = Color.Blue;
                    lblKQ.Text = "Mật khẩu: " + modify.TaiKhoans(query)[0].MatKhau;
                }
                else
                {
                    lblThongBao.Text = "Email chưa được đăng ký";
                }
            }
        }
    }
}
