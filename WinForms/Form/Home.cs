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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            FormMain formMain = new FormMain();
            formMain.ShowDialog();
            this.Close();
        }

        private Form curentFormChild;
        private void OpenChildForm(Form childForm)
        {
            if(curentFormChild != null)
            {
                curentFormChild.Close();
            }
            curentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlBody.Controls.Add(childForm);
            pnlBody.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormMain());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormHocPhi());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormAward());
        }

        private void button13_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormSubject());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormQLDiem());
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (curentFormChild != null)
            {
                curentFormChild.Close();
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormMain());
        }

        private void button20_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormQLDiem());
        }

        private void button22_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormSubject());
        }

        private void button24_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormHocPhi());
        }

        private void button27_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormAward());
        }

        private void button19_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void button21_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void button23_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void button28_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void button11_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void button14_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void button15_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void button16_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormNoData());
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                FormLogin formLogin = new FormLogin();
                formLogin.ShowDialog();
                this.Close();

            }
        }
    }
}
