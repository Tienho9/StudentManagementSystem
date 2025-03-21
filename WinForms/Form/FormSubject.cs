using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using BaiTapNhom.OOP;

namespace BaiTapNhom
{
    public partial class FormSubject : Form
    {
        public FormSubject()
        {
            InitializeComponent();
        }

        Modify modify = new Modify();
        Subject subject;

        private void setTenCot()
        {
            dataGridView1.Columns[1].HeaderText = "Mã MH";
            dataGridView1.Columns[2].HeaderText = "Tên môn học";
            dataGridView1.Columns[3].HeaderText = "Số TC";
            dataGridView1.Columns[4].HeaderText = "Học kỳ";
            dataGridView1.Columns[5].HeaderText = "Mô tả";
        }

        private void FormSubject_Load(object sender, EventArgs e)
        {
            modify = new Modify();
            try
            {
                dataGridView1.DataSource = modify.getAllSubject();
                dataGridView1.Columns[0].Width = 60;
                dataGridView1.Columns[1].Width = 110;
                dataGridView1.Columns[2].Width = 240;
                dataGridView1.Columns[3].Width = 80;
                dataGridView1.Columns[4].Width = 90;
                setTenCot();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaMH.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtTenMH.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            txtSoTinChi.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtMoTa.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            if (txtMaMH.Text == "")
            {
                txtMaMH.Enabled = true;
            }
            else txtMaMH.Enabled = false;

            if (dataGridView1.SelectedRows[0].Cells[4].Value.ToString() != "")
            {
                cobHocKy.SelectedItem = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            }
            else
            {
                cobHocKy.SelectedIndex = -1;
            }
        }

        private void getValues()
        {
            string mamh = txtMaMH.Text;
            string tenmh = txtTenMH.Text;
            int sotc = int.Parse(txtSoTinChi.Text);
            int hocky = int.Parse(cobHocKy.Text);
            string mota = txtMoTa.Text;
            subject = new Subject(mamh, tenmh, sotc, hocky, mota);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                getValues();
                if (modify.insertSubject(subject))
                {
                    dataGridView1.DataSource = modify.getAllSubject(); // hiển thị dữ liệu vào datagridview
                }
                else
                {
                    MessageBox.Show("Lỗi khi thêm!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                getValues();
                if (modify.updateSubject(subject))
                {
                    dataGridView1.DataSource = modify.getAllSubject(); // hiển thị dữ liệu vào datagridview
                }
                else
                {
                    MessageBox.Show("Lỗi khi sửa!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count != 1)
            {
                string MaMH = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                if (modify.deleteSubject(MaMH))
                {
                    dataGridView1.DataSource = modify.getAllSubject();
                }
                else
                {
                    MessageBox.Show("Lỗi khi xóa");
                }
            }
            else
            {
                MessageBox.Show("Không thể xóa dòng trống!");
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string name = txtTimKiem.Text.Trim();
            if (name == "")
            {
                FormSubject_Load(sender, e);
            }
            else
            {
                string filter = $"SubjectID like '%{name}%' OR SubjectName like '%{name}%'";
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filter;
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
