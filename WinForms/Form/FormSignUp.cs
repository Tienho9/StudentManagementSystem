using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace BaiTapNhom
{
    public partial class FormSignUp : Form
    {
        public FormSignUp()
        {
            InitializeComponent();
        }

        public bool CheckAccount(string ac) // check tên tài khoản và mật khẩu
        {
            return Regex.IsMatch(ac, "^[a-zA-Z0-9]{5,24}$");
        }

        public bool CheckEmail(string em) // check email
        {
            return Regex.IsMatch(em, @"^[a-zA-Z0-9_]{3,20}@gmail.com(.vn|)$");
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string tentk = txtTenTK.Text;
            string matkhau = txtMK.Text;
            string xnmk = txtXNMK.Text;
            string email = txtEmail.Text;
            lblThongBao1.Text = "";
            lblThongBao2.Text = "";
            lblThongBao3.Text = "";
            lblThongBao4.Text = "";
            if (!CheckAccount(tentk))
            {
                lblThongBao1.Text = "Tên tài khoản dài 6-24 kí tự, bao gồm số và chữ";
                return;
            }
            if (!CheckAccount(matkhau))
            {
                lblThongBao2.Text = "Mật khẩu dài 6-24 kí tự, bao gồm số và chữ";
                return;
            }
            if (matkhau != xnmk)
            {
                lblThongBao3.Text = "Mật khẩu không trùng khớp";
                return;
            }

            if (!CheckEmail(email))
            {
                lblThongBao4.Text = "Email sai định dạng";
                return;
            }

            Modify modify = new Modify();
            if (modify.TaiKhoans("Select * from TaiKhoan where Email = '" + email + "'").Count() > 0)
            {
                lblThongBao4.Text = "Email đã được đăng ký";
                return;
            }

            try
            {
                string query = "Insert into TaiKhoan values ('" + tentk + "', '" + matkhau + "', '" + email + "')";
                modify.Command(query);
                MessageBox.Show(
                        "Đăng ký thành công",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                        );
                this.Close();
            }
            catch (Exception ex)
            {
                lblThongBao1.Text = "Tên tài khoản này đã được đăng ký";
            }
        }

        private void picView_Click(object sender, EventArgs e)
        {
            if (txtMK.PasswordChar == '*')
            {
                picHide.BringToFront();
                txtMK.PasswordChar = '\0';
            }
        }

        private void picHide_Click(object sender, EventArgs e)
        {
            if (txtMK.PasswordChar == '\0')
            {
                picView.BringToFront();
                txtMK.PasswordChar = '*';
            }
        }

        private void picReView_Click(object sender, EventArgs e)
        {
            if (txtXNMK.PasswordChar == '*')
            {
                picReHide.BringToFront();
                txtXNMK.PasswordChar = '\0';
            }
        }

        private void picReHide_Click(object sender, EventArgs e)
        {
            if (txtXNMK.PasswordChar == '\0')
            {
                picReView.BringToFront();
                txtXNMK.PasswordChar = '*';
            }
        }
    }
}
