using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace BaiTapNhom
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }


        
        Modify modify = new Modify();
        SinhVien sinhVien;

        private void setTenCot()
        {
            dataGridView1.Columns[1].HeaderText = "Mã sinh viên";
            dataGridView1.Columns[2].HeaderText = "Họ và tên";
            dataGridView1.Columns[3].HeaderText = "Mã lớp";
            dataGridView1.Columns[4].HeaderText = "Mã khoa";
            dataGridView1.Columns[5].HeaderText = "Ngày sinh";
            dataGridView1.Columns[6].HeaderText = "Giới tính";
            dataGridView1.Columns[7].HeaderText = "Địa chỉ";
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            modify = new Modify();
            try
            {
                dataGridView1.DataSource = modify.getAllSinhVien();
                dataGridView1.Columns[0].Width = 35;
                dataGridView1.Columns[1].Width = 105;
                dataGridView1.Columns[2].Width = 150;
                dataGridView1.Columns[3].Width = 75;
                dataGridView1.Columns[4].Width = 80;
                dataGridView1.Columns[5].Width = 100;
                dataGridView1.Columns[6].Width = 80;
                dataGridView1.Columns[7].Width = 155;
                setTenCot();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi");
            }
        }


        private void getValues()
        {
            string masv = txtMaSV.Text;
            string tensv = txtHoTen.Text;
            string diachi = txtDiaChi.Text;
            string gioitinh = (radNam.Checked ? radNam.Text : radNu.Text);
            DateTime ngaysinh = dtpNgaySinh.Value;
            string makhoa = txtMaKhoa.Text;
            string malop = txtMaLop.Text;
            sinhVien = new SinhVien(masv, tensv, malop, makhoa, gioitinh, ngaysinh, diachi);
        }

        private void btnThem_Click(object sender, EventArgs e)
         {
             try
             {
                 getValues();
                 if (txtMaSV.Text != "" && modify.insert(sinhVien))
                 {
                     dataGridView1.DataSource = modify.getAllSinhVien();
                 }
                 else
                 {
                     MessageBox.Show("Lỗi thêm");
                 }
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Lỗi không thêm được!");
             }
         }
        
        
        private void cobMaMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy item được chọn trong ComboBox
            string selectedSubject = cobMaMon.SelectedItem.ToString();

            // Tách mã môn học từ item (phần trước dấu '-')
            string subjectId = selectedSubject.Split('-')[0].Trim();

            if (subjectId == "None")
            {
                lbNoData.Visible = false;
                FormMain_Load(sender, e);
            }
            else
            {
                try
                {
                    // Gọi phương thức tìm kiếm với mã môn học đã tách được
                    DataTable dataTable = modify.getStudentAttemptsBySubject(subjectId);
                    if (dataTable.Rows.Count == 0)
                    {
                        lbNoData.Text = "* Không có dữ liệu cho môn học này.";
                        lbNoData.Visible = true; // Hiển thị thông báo không có dữ liệu
                        dataGridView1.DataSource = null;
                    }
                    else
                    {
                        lbNoData.Visible = false;
                        dataGridView1.DataSource = dataTable; // Hiển thị kết quả tìm kiếm nếu có
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaSV.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtHoTen.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            txtMaLop.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtMaKhoa.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            txtDiaChi.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            if (txtMaSV.Text == "")
            {
                txtMaSV.Enabled = true;
            }
            else txtMaSV.Enabled = false;

            if (dataGridView1.SelectedRows[0].Cells[6].Value.ToString() == "Nam")
            {
                radNam.Checked = true;
            }
            else if (dataGridView1.SelectedRows[0].Cells[6].Value.ToString() == "Nữ")
            {
                radNu.Checked = true;
            }
            else
            {
                radNam.Checked = false;
                radNu.Checked = false;
            }
            if (dataGridView1.SelectedRows[0].Cells[5].Value != null && DateTime.TryParse(dataGridView1.SelectedRows[0].Cells[5].Value.ToString(), out DateTime ngaySinh))
            {
                dtpNgaySinh.Value = ngaySinh;
            }
            else
            {
                dtpNgaySinh.Value = DateTime.Now;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                getValues();
                if (modify.update(sinhVien))
                {
                    dataGridView1.DataSource = modify.getAllSinhVien();
                }
                else
                {
                    MessageBox.Show("Lỗi sửa");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không sửa được!");
            }
        }

        /*
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string name = txtTimKiem.Text.Trim();
            if(name == "")
            {
                FormMain_Load(sender, e);
            }
            else
            {
                //string query = @"Select * from SinhVien where MaSV like '%"+name+"%'";
                //dataGridView1.DataSource = modify.Search(query);
                string filter = $"MaSV like '%{name}%' OR HoTen like '%{name}%'";

                // Áp dụng bộ lọc vào DataView của DataGridView
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filter;
                //- Ở đây, chúng ta đang tạo một chuỗi bộ lọc mới.
                //- Biến filter này sẽ chứa một chuỗi điều kiện để áp dụng cho dữ liệu trong DataGridView.
                //- Bộ lọc này sử dụng cú pháp của SQL để tìm kiếm dữ liệu theo cả mã sinh viên(MaSV) và tên sinh viên(HoTen).
                //- Toán tử like được sử dụng để so sánh một chuỗi với một mẫu trong SQL.
                //- Trong trường hợp này, %{ name}% đại diện cho một chuỗi có chứa name ở bất kỳ vị trí nào.
                //- Trong bước này, chúng ta đang truy cập DataSource của DataGridView, và chắc chắn rằng nó là một DataTable.
                //- Sau đó, chúng ta lấy DefaultView của DataTable, mà là một DataView chứa dữ liệu được hiển thị trong DataGridView.
                //- Cuối cùng, chúng ta gán chuỗi bộ lọc cho thuộc tính RowFilter của DataView.Điều này sẽ làm cho DataView chỉ hiển thị các dòng trong dữ liệu mà thỏa mãn điều kiện bộ lọc đã xác định.
            }
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            string MaSV = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            if (modify.delete(MaSV))
            {
                dataGridView1.DataSource = modify.getAllSinhVien();
            }
            else
            {
                MessageBox.Show("Lỗi xóa");
            }
        }

        private void UpdateSTT()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["STT"].Value = i + 1;
            }
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            UpdateSTT();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        */
    }
}
