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
    public partial class FormHocPhi : Form
    {
        public FormHocPhi()
        {
            InitializeComponent();
        }
        Modify modify = new Modify();
        private void setTenCot()
        {
            dataGridView1.Columns[1].HeaderText = "Mã sinh viên";
            dataGridView1.Columns[2].HeaderText = "Học kỳ";
            dataGridView1.Columns[3].HeaderText = "Tổng học phí";
        }

        private void FormHocPhi_Load(object sender, EventArgs e)
        {
            modify = new Modify();
            try
            {
                dataGridView1.DataSource = modify.getTuitionFee();
                setTenCot();
                dataGridView1.Columns[0].Width = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaSV.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtHocPhi.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            if (txtMaSV.Text == "")
            {
                txtMaSV.Enabled = true;
            }
            else txtMaSV.Enabled = false;

            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() != "")
            {
                cobHocKy.SelectedItem = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            }
            else
            {
                cobHocKy.SelectedIndex = -1;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string studentID = txtSerMSV.Text.Trim();
            if (string.IsNullOrEmpty(studentID))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên.");
                return;
            }
            int semester;
            if (string.IsNullOrEmpty(cobSerHocKy.Text) || !int.TryParse(cobSerHocKy.Text, out semester))
            {
                MessageBox.Show("Vui lòng chọn học kỳ hợp lệ.");
                return;
            }

            try
            {
                DataTable tuitionDetailTable = modify.getStudentTuitionDetail(studentID, semester);
                dataGridView1.DataSource = tuitionDetailTable;

                // Thiết lập tiêu đề cột
                dataGridView1.Columns[0].HeaderText = "Mã MH";
                dataGridView1.Columns[1].HeaderText = "Tên Môn Học";
                dataGridView1.Columns[2].HeaderText = "Số TC";
                dataGridView1.Columns[3].HeaderText = "Mã SV";
                dataGridView1.Columns[4].HeaderText = "Học Kỳ";
                dataGridView1.Columns[5].HeaderText = "Học Phí";
                dataGridView1.Columns[0].Width = 100;
                dataGridView1.Columns[1].Width = 250;
                dataGridView1.Columns[2].Width = 100;
                dataGridView1.Columns[3].Width = 120;
                dataGridView1.Columns[4].Width = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormHocPhi_Load(sender, e);
        }

        private void txtMaSV_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
