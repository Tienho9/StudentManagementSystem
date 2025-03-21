using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace BaiTapNhom
{
    public partial class FormAward : Form
    {
        public FormAward()
        {
            InitializeComponent();
        }

        Modify modify;
        Award award;

        private void FormAward_Load(object sender, EventArgs e)
        {
            modify = new Modify();
            try
            {
                dataGridView1.DataSource = modify.getAward();
                dataGridView1.Columns[0].Width = 55;
                dataGridView1.Columns[1].Width = 140;
                dataGridView1.Columns[2].Width = 140;
                dataGridView1.Columns[3].Width = 140;
                dataGridView1.Columns[4].Width = 100;
                setTenCot();
            }
            catch
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void setTenCot()
        {
            dataGridView1.Columns[1].HeaderText = "Mã khen thưởng";
            dataGridView1.Columns[2].HeaderText = "Mã sinh viên";
            dataGridView1.Columns[3].HeaderText = "Ngày khen thưởng";
            dataGridView1.Columns[4].HeaderText = "Loại khen thưởng";
            dataGridView1.Columns[5].HeaderText = "Lý do khen thưởng";

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaKT.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtMSV.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            txtLydo.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            if (txtMaKT.Text == "")
            {
                txtMaKT.Enabled = true;
            }
            else txtMaKT.Enabled = false;

            if (dataGridView1.SelectedRows[0].Cells[4].Value.ToString() != "")
            {
                cobLoaiKT.SelectedItem = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            }
            else
            {
                cobLoaiKT.SelectedIndex = -1;
            }
            if (dataGridView1.SelectedRows[0].Cells[3].Value != null && DateTime.TryParse(dataGridView1.SelectedRows[0].Cells[3].Value.ToString(), out DateTime ngaySinh))
            {
                dtpNgayKhenThuong.Value = ngaySinh;
            }
            else
            {
                dtpNgayKhenThuong.Value = DateTime.Now;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            DateTime startDate = dtpNgayBD.Value;
            DateTime endDate = dtpNgayKT.Value;

            try
            {
                dataGridView1.DataSource = modify.GetAwardByDate(startDate, endDate);

                // Cập nhật tiêu đề cột nếu cần thiết
                dataGridView1.Columns[1].HeaderText = "Mã khen thưởng";
                dataGridView1.Columns[2].HeaderText = "Mã sinh viên";
                dataGridView1.Columns[3].HeaderText = "Ngày khen thưởng";
                dataGridView1.Columns[4].HeaderText = "Loại khen thưởng";
                dataGridView1.Columns[5].HeaderText = "Lý do khen thưởng";
                dataGridView1.Columns[6].HeaderText = "Tổng số khen thưởng";
                dataGridView1.Columns[6].Width = 80;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAward_Load(sender, e);
        }

        private void getValues()
        {
            string makt = txtMaKT.Text;
            string masv = txtMSV.Text;
            DateTime ngaykt = dtpNgayKhenThuong.Value;
            string loaikt = cobLoaiKT.Text;
            string lydo = txtLydo.Text;
            award = new Award(makt, masv, ngaykt, loaikt, lydo);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                getValues();
                if (modify.insertAward(award))
                {
                    dataGridView1.DataSource = modify.getAward(); // hiển thị dữ liệu vào datagridview
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
                getValues(); // ctdt đã có giá trị
                if (modify.updateAward(award))
                {
                    dataGridView1.DataSource = modify.getAward();
                }
                else
                {
                    MessageBox.Show("Lỗi.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }




        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count != 1)
            {
                string MaAward = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                if (modify.deleteAward(MaAward))
                {
                    dataGridView1.DataSource = modify.getAward();
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

    }
}
