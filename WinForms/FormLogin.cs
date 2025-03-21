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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        Modify modify = new Modify();

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tenTK = txtTenTK.Text;
            string MatKhau = txtMatKhau.Text;
            lblThongBao1.Text = "";
            if (tenTK.Trim() == "")
            {
                lblThongBao1.Text = "Vui lòng nhập tên tài khoản";
            }
            else if (MatKhau.Trim() == "")
            {
                lblThongBao2.Text = "Vui lòng nhập mật khẩu";
            }
            else
            {
                string query = "Select * from TaiKhoan where TenTaiKhoan = '" + tenTK + "' and MatKhau = '" + MatKhau + "'";
                if (modify.TaiKhoan(query).Count() > 0)
                {
                    MessageBox.Show(
                        "Đăng nhập thành công",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                        );
                    this.Hide();
                    Home home = new Home();
                    home.ShowDialog();
                    this.Close();
                }
                else
                {
                    lblThongBao2.Text = "Tên tài khoản hoặc mật khẩu không chính xác!";
                }
            }
        }

        private void picView_Click(object sender, EventArgs e)
        {
            if (txtMatKhau.PasswordChar == '*')
            {
                picHide.BringToFront();
                txtMatKhau.PasswordChar = '\0';
            }
        }

        private void picHide_Click(object sender, EventArgs e)
        {
            if (txtMatKhau.PasswordChar == '\0')
            {
                picView.BringToFront();
                txtMatKhau.PasswordChar = '*';
            }
        }

        private void linkLabelQuenMK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormRePassword quenMK = new FormRePassword();
            quenMK.ShowDialog();
        }

        private void linkLabelDangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormSignUp dangKy = new FormSignUp();
            dangKy.ShowDialog();
        }
    }
}
