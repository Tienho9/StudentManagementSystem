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
using System.IO;

namespace BaiTapNhom
{
    public partial class FormQLDiem : Form
    {
        public FormQLDiem()
        {
            InitializeComponent();
        }
        Modify modify;
        /*
        
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            txtMaSV.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtTenSV.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            txtMaMH.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtTenMH.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
        }

        private string TinhDiemChu(float diemTrungBinh)
        {
            if (diemTrungBinh >= 8.5f)
                return "A";
            else if (diemTrungBinh >= 7.0f && diemTrungBinh <= 8.4f)
                return "B";
            else if (diemTrungBinh >= 5.5f && diemTrungBinh <= 6.9f)
                return "C";
            else if (diemTrungBinh >= 4.0f && diemTrungBinh <= 5.4f)
                return "D";
            else
                return "F";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            modify = new Modify();
            string maSV = txtMaSV.Text;
            string maMH = txtMaMH.Text;
            float diemQT;
            float diemThi;

            // Kiểm tra đầu vào hợp lệ
            if (!float.TryParse(txtDQT.Text, out diemQT) || !float.TryParse(txtDiemThi.Text, out diemThi))
            {
                MessageBox.Show("Điểm phải là số.");
                return;
            }

            float diemTrungBinh = (0.5f * diemQT + 0.5f * diemThi);
            string diemChu = TinhDiemChu(diemTrungBinh);
            // Gọi phương thức cập nhật cơ sở dữ liệu
            if (modify.UpdateDiem(maSV, maMH, diemQT, diemThi, diemTrungBinh, diemChu))
            {
                dataGridView1.DataSource = modify.getAllSinhVien1();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại.");
            }
        }

        private void txtTimKiem_TextChanged_1(object sender, EventArgs e)
        {
            string name = txtTimKiem.Text.Trim();
            if (name == "")
            {
                dataGridView1.DataSource = modify.getDKHP();
            }
            else
            {

                string filter = $"MaSV like '%{name}%' OR MaMH like '%{name}%' OR TenMH like '%{name}%'";

                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filter;
                UpdateSTT();
            }
        }
        */
        private void FormQLDiem_Load(object sender, EventArgs e)
        {
            modify = new Modify();
            try
            {
                //modify.InsertDiem(); // Cập nhật dữ liệu trong bảng Diem
                dataGridView1.DataSource = modify.getAllSinhVien1(); // Tải lại dữ liệu mới

                // Cấu hình hiển thị cho các cột
                dataGridView1.Columns[0].Width = 50;
                dataGridView1.Columns[1].Width = 115;
                dataGridView1.Columns[2].Width = 100;
                dataGridView1.Columns[3].Width = 85;
                dataGridView1.Columns[4].Width = 70;
                dataGridView1.Columns[5].Width = 70;
                dataGridView1.Columns[6].Width = 65;
                dataGridView1.Columns[7].Width = 65;
                dataGridView1.Columns[8].Width = 75;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void txtStudentID_TextChanged(object sender, EventArgs e)
        {
            string stuID = txtStudentID.Text.Trim();
            if (stuID == "")
            {
                FormQLDiem_Load(sender, e);
            }
            else
            {

                string filter = $"StudentID like '%{stuID}%'";
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filter;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaSV.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtDQT.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtDiemThi.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            txtTongDiem.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            txtDiemChu.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            txtSoLanThi.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            txtStatus.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() != "")
            {
                cobMamonhoc.SelectedItem = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            }
            else
            {
                cobMamonhoc.SelectedIndex = -1;
            }
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            string studentID = txtMaSV.Text;
            string subjectID = cobMamonhoc.SelectedItem?.ToString();
            float continuousAssessmentScore;
            float finalExamScore;


            if (!float.TryParse(txtDQT.Text, out continuousAssessmentScore))
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng.");
                return;
            }
            continuousAssessmentScore = (float)Math.Round(continuousAssessmentScore, 2);
            if (!float.TryParse(txtDiemThi.Text, out finalExamScore))
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng.");
                return;
            }
            finalExamScore = (float)Math.Round(finalExamScore, 2);
            modify = new Modify();
            bool isUpdated = modify.UpdateScore(studentID, subjectID, continuousAssessmentScore, finalExamScore);

            if (isUpdated)
            {
                MessageBox.Show("Cập nhật điểm thành công.");
                dataGridView1.DataSource = modify.getAllSinhVien1();
            }
            else
            {
                MessageBox.Show("Lỗi. Kiểm tra lại mã sinh viên hoặc mã môn học.");
            }

        }

        private void btnClassify_Click(object sender, EventArgs e)
        {
            string studentID = txtStudentID.Text;

            if (string.IsNullOrEmpty(studentID))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên.");
                return;
            }
            modify = new Modify();
            // Call the GetClassification method from the Classify class
            string classification = modify.GetClassification(studentID);
            txtClassification.Text = classification;
            string classifiGPA = modify.GetClassifiGPA(studentID);
            txtClassifyGPA.Text = classifiGPA;
        }
    }
}
