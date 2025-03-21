using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using BaiTapNhom.OOP;
using System.Diagnostics.Eventing.Reader;

namespace BaiTapNhom
{
    internal class Modify
    {
        public Modify()
        {
        }
        // int a
        SqlCommand sqlCommand; // dùng để truy vấn các câu lệnh insert into, delete, update, ...
        SqlDataReader dataReader; // dùng để đọc dữ liệu trong bảng
        SqlDataAdapter dataAdapter; // truy xuất dữ liệu vào bảng
        // Tài khoản
        public List<TaiKhoan> TaiKhoan(string query) // dùng để check tài khoản
        {
            List<TaiKhoan> taiKhoans = new List<TaiKhoan>();
            using (SqlConnection sqlConnection = Connections.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                dataReader = sqlCommand.ExecuteReader(); // thực thi câu lệnh
                while (dataReader.Read())
                {
                    taiKhoans.Add(new TaiKhoan(dataReader.GetString(0), dataReader.GetString(1)));
                }
                sqlConnection.Close();
            }
            return taiKhoans;
        }

        public void Command(string query) // dùng để insert tài khoản đã đky vào csdl
        {
            using (SqlConnection sqlConnection = Connections.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        // Hiển thị danh sách sv
        public DataTable getAllSinhVien()
        {
            DataTable dataTable = new DataTable();
            string query = "select * from Student";
            using (SqlConnection sqlConnection = Connections.GetSqlConnection())
            {
                sqlConnection.Open();
                dataAdapter = new SqlDataAdapter(query, sqlConnection);
                dataAdapter.Fill(dataTable);

                dataTable.Columns.Add("STT", typeof(int));
                // Điền số thứ tự cho từng hàng
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dataTable.Rows[i]["STT"] = i + 1;
                }

                // Thiết lập thứ tự hiển thị cột
                dataTable.Columns["STT"].SetOrdinal(0);

                sqlConnection.Close();
            }
            return dataTable;
        }

        public DataTable getStudentAttemptsBySubject(string subjectId)
        {
            DataTable dataTable = new DataTable();
            string query = "SELECT * FROM StudentAttemptSummaryView WHERE SubjectID = @subjectId";

            using (SqlConnection sqlConnection = Connections.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@subjectId", subjectId);

                dataAdapter = new SqlDataAdapter(sqlCommand);
                dataAdapter.Fill(dataTable);

                dataTable.Columns.Add("STT", typeof(int));

                // Điền số thứ tự cho từng hàng
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dataTable.Rows[i]["STT"] = i + 1;
                }

                // Thiết lập thứ tự hiển thị cột
                dataTable.Columns["STT"].SetOrdinal(0);

                sqlConnection.Close();
            }
            return dataTable;
        }


        //thêm sinh viên mới
        public bool insert(SinhVien sinhVien)
        {
            SqlConnection sqlConnection = Connections.GetSqlConnection();
            string query = "insert into Student values (@MaSV, @HoTen, @MaLop, @MaKhoa, @NgaySinh, @GioiTinh, @DiaChi)";
            try
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.Add("@MaSV", SqlDbType.VarChar).Value = sinhVien.MaSV;
                sqlCommand.Parameters.Add("@HoTen", SqlDbType.NVarChar).Value = sinhVien.HoTen;
                sqlCommand.Parameters.Add("@NgaySinh", SqlDbType.DateTime).Value = sinhVien.NgaySinh.ToShortDateString();
                sqlCommand.Parameters.Add("@GioiTinh", SqlDbType.NVarChar).Value = sinhVien.GioiTinh;
                sqlCommand.Parameters.Add("@MaLop", SqlDbType.VarChar).Value = sinhVien.MaLop;
                sqlCommand.Parameters.Add("@MaKhoa", SqlDbType.NVarChar).Value = sinhVien.MaKhoa;
                sqlCommand.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = sinhVien.DiaChi;
                sqlCommand.ExecuteNonQuery(); // thực thi câu truy vấn
            }
            catch
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }

        ////public DataTable Search(string query)
        ////{
        ////    DataTable dataTable = new DataTable();
        ////    using (SqlConnection sqlConnection = Connections.GetSqlConnection())
        ////    {
        ////        sqlConnection.Open();
        ////        dataAdapter = new SqlDataAdapter(query, sqlConnection);
        ////        dataAdapter.Fill(dataTable);
        ////        sqlConnection.Close();
        ////    }
        ////    return dataTable;
        ////}


        //// cập nhật sinh viên
        public bool update(SinhVien sinhVien)
        {
            SqlConnection sqlConnection = Connections.GetSqlConnection();
            string query = @"update Student set StudentName = @HoTen, DateOfBirth = @NgaySinh, Gender = @GioiTinh, ClassID = @MaLop, DepartmentID = @MaKhoa, Hometown = @DiaChi where StudentID = @MaSV";
            try
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.Add("@MaSV", SqlDbType.VarChar).Value = sinhVien.MaSV;
                sqlCommand.Parameters.Add("@HoTen", SqlDbType.NVarChar).Value = sinhVien.HoTen;
                sqlCommand.Parameters.Add("@NgaySinh", SqlDbType.DateTime).Value = sinhVien.NgaySinh.ToShortDateString();
                sqlCommand.Parameters.Add("@GioiTinh", SqlDbType.NVarChar).Value = sinhVien.GioiTinh;
                sqlCommand.Parameters.Add("@MaLop", SqlDbType.VarChar).Value = sinhVien.MaLop;
                sqlCommand.Parameters.Add("@MaKhoa", SqlDbType.NVarChar).Value = sinhVien.MaKhoa;
                sqlCommand.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = sinhVien.DiaChi;
                sqlCommand.ExecuteNonQuery(); // thực thi câu truy vấn
            }
            catch
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }

        //// xóa sinh viên
        //public bool delete(string MaSV)
        //{
        //    SqlConnection sqlConnection = Connections.GetSqlConnection();
        //    string query = @"delete SinhVien where MaSV = @MaSV";
        //    try
        //    {
        //        sqlConnection.Open();
        //        sqlCommand = new SqlCommand(query, sqlConnection);
        //        sqlCommand.Parameters.Add("@MaSV", SqlDbType.VarChar).Value = MaSV;
        //        sqlCommand.ExecuteNonQuery(); // thực thi câu truy vấn
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    finally
        //    {
        //        sqlConnection.Close();
        //    }
        //    return true;
        //}

        //// chương trình đào tạo
        //// hiển thị chương trình đào tạo

        public DataTable getAward()
        {
            DataTable dataTable = new DataTable();
            string query = @"select * from Award";
            using (SqlConnection sqlConnection = Connections.GetSqlConnection())
            {
                sqlConnection.Open();
                dataAdapter = new SqlDataAdapter(query, sqlConnection);
                dataAdapter.Fill(dataTable);
                dataTable.Columns.Add("STT", typeof(int));

                // Điền số thứ tự cho từng hàng
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dataTable.Rows[i]["STT"] = i + 1;
                }

                // Thiết lập thứ tự hiển thị cột
                dataTable.Columns["STT"].SetOrdinal(0);
                sqlConnection.Close();
            }
            return dataTable;
        }

        public DataTable GetAwardByDate(DateTime startDate, DateTime endDate)
        {
            DataTable dataTable = new DataTable();
            string query = "GetAwardByDate"; // Tên của Stored Procedure

            using (SqlConnection sqlConnection = Connections.GetSqlConnection())
            {
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    sqlConnection.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(dataTable);
                    dataTable.Columns.Add("STT", typeof(int));

                    // Điền số thứ tự cho từng hàng
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i]["STT"] = i + 1;
                    }

                    // Thiết lập thứ tự hiển thị cột
                    dataTable.Columns["STT"].SetOrdinal(0);
                    sqlConnection.Close();
                }
            }

            return dataTable;
        }

        public bool insertAward(Award award)
        {
            SqlConnection sqlConnection = Connections.GetSqlConnection();
            string query = "insert into Award values (@AwardID, @StudentID, @DecisionDate, @AwardAmount, @Reason)";
            try
            {
                sqlConnection.Open(); // mở kết nối
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.Add("@AwardID", SqlDbType.VarChar).Value = award.MaKT;
                sqlCommand.Parameters.Add("@StudentID", SqlDbType.NVarChar).Value = award.MaSV;
                sqlCommand.Parameters.Add("@DecisionDate", SqlDbType.DateTime).Value = award.NgayKT.ToShortDateString();
                sqlCommand.Parameters.Add("@AwardAmount", SqlDbType.NVarChar).Value = award.LoaiKT;
                sqlCommand.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = award.LyDo;
                sqlCommand.ExecuteNonQuery();
            }
            catch
            {
                return false; 
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }

        public bool updateAward(Award award)
        {
            SqlConnection sqlConnection = Connections.GetSqlConnection();
            string query = @"update Award set StudentID = @StudentID, DecisionDate = @DecisionDate, AwardAmount = @AwardAmount, Reason = @Reason where AwardID = @AwardID";
            try
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.Add("@AwardID", SqlDbType.VarChar).Value = award.MaKT;
                sqlCommand.Parameters.Add("@StudentID", SqlDbType.NVarChar).Value = award.MaSV;
                sqlCommand.Parameters.Add("@DecisionDate", SqlDbType.DateTime).Value = award.NgayKT.ToShortDateString();
                sqlCommand.Parameters.Add("@AwardAmount", SqlDbType.NVarChar).Value = award.LoaiKT;
                sqlCommand.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = award.LyDo;
                sqlCommand.ExecuteNonQuery(); // thực thi câu truy vấn
            }
            catch
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }

        public bool deleteAward(string MaAward)
        {
            SqlConnection sqlConnection = Connections.GetSqlConnection();
            string query = @"delete Award where AwardID = @MaAward";
            try
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.Add("@MaAward", SqlDbType.VarChar).Value = MaAward;
                sqlCommand.ExecuteNonQuery(); // thực thi câu truy vấn
            }
            catch
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }


        public string GetClassification(string studentID)
        {
            string classification = "Classification not found.";

            using (SqlConnection sqlConnection = Connections.GetSqlConnection())
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT dbo.ClassifyStudentByTotalScore(@StudentID)";
                    //string query1 = "SELECT dbo.fnCalculateGPA(@StudentID)";
                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@StudentID", studentID);
                        var result = command.ExecuteScalar();
                        classification = result?.ToString() ?? classification;
                    }
                }
                catch (Exception ex)
                {
                    classification = "Error: " + ex.Message;
                }
            }
            return classification;
        }

        public string GetClassifiGPA(string studentID)
        {
            string classification = "Classification not found.";

            using (SqlConnection sqlConnection = Connections.GetSqlConnection())
            {
                try
                {
                    sqlConnection.Open();
                    //string query = "SELECT dbo.ClassifyStudentByTotalScore(@StudentID)";
                    string query = "SELECT dbo.fnCalculateGPA(@StudentID)";
                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@StudentID", studentID);
                        var result = command.ExecuteScalar();
                        classification = result?.ToString() ?? classification;
                    }
                }
                catch (Exception ex)
                {
                    classification = "Error: " + ex.Message;
                }
            }

            return classification;
        }

        public DataTable getAllSinhVien1()
        {
            DataTable dataTable = new DataTable();
            string query = "select * from Score";
            using (SqlConnection sqlConnection = Connections.GetSqlConnection())
            {
                sqlConnection.Open();
                dataAdapter = new SqlDataAdapter(query, sqlConnection);
                dataAdapter.Fill(dataTable);

                dataTable.Columns.Add("STT", typeof(int));
                // Điền số thứ tự cho từng hàng
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dataTable.Rows[i]["STT"] = i + 1;
                }

                // Thiết lập thứ tự hiển thị cột
                dataTable.Columns["STT"].SetOrdinal(0);

                sqlConnection.Close();
            }
            return dataTable;
        }

        public bool UpdateScore(string studentID, string subjectID, float continuousAssessmentScore, float finalExamScore)
        {
            bool isUpdated = false;
            using (SqlConnection sqlConnection = Connections.GetSqlConnection())
            {
                try
                {
                    sqlConnection.Open();
                    string query = "UPDATE Score SET ContinuousAssessmentScore = @DQT, FinalExamScore = @DiemThi WHERE StudentID = @StudentID AND SubjectID = @SubjectID";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@StudentID", studentID);
                        command.Parameters.AddWithValue("@SubjectID", subjectID);
                        command.Parameters.AddWithValue("@DQT", continuousAssessmentScore);
                        command.Parameters.AddWithValue("@DiemThi", finalExamScore);
                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            return isUpdated;
        }

        public DataTable getAllSubject()
        {
            DataTable dataTable = new DataTable();
            string query = "select * from Subject";
            using (SqlConnection sqlConnection = Connections.GetSqlConnection())
            {
                sqlConnection.Open();
                dataAdapter = new SqlDataAdapter(query, sqlConnection);
                dataAdapter.Fill(dataTable);

                dataTable.Columns.Add("STT", typeof(int));
                // Điền số thứ tự cho từng hàng
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dataTable.Rows[i]["STT"] = i + 1;
                }

                // Thiết lập thứ tự hiển thị cột
                dataTable.Columns["STT"].SetOrdinal(0);

                sqlConnection.Close();
            }
            return dataTable;
        }

        public bool insertSubject(Subject subject)
        {
            SqlConnection sqlConnection = Connections.GetSqlConnection();
            string query = "insert into Subject values (@MaMH, @TenMH, @SoTC, @HocKy, @MoTa)";
            try
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.Add("@MaMH", SqlDbType.VarChar).Value = subject.SubjectID; // @MaMH = 123 (get MaMH)
                sqlCommand.Parameters.Add("@TenMH", SqlDbType.NVarChar).Value = subject.SubjectName;
                sqlCommand.Parameters.Add("@SoTC", SqlDbType.Int).Value = subject.Credits;
                sqlCommand.Parameters.Add("@HocKy", SqlDbType.Int).Value = subject.Semester;
                sqlCommand.Parameters.Add("@MoTa", SqlDbType.NVarChar).Value = subject.Description;
                sqlCommand.ExecuteNonQuery(); 
            }
            catch
            {
                return false; 
            }
            finally
            {
                sqlConnection.Close(); 
            }
            return true;
        }


        public bool updateSubject(Subject subject)
        {
            SqlConnection sqlConnection = Connections.GetSqlConnection();
            string query = @"update Subject set SubjectName = @TenMH, Credits = @SoTC, Semester = @HocKy, SubjectDescription = @MoTa where SubjectID = @MaMH";
            try
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.Add("@MaMH", SqlDbType.VarChar).Value = subject.SubjectID;
                sqlCommand.Parameters.Add("@TenMH", SqlDbType.NVarChar).Value = subject.SubjectName;
                sqlCommand.Parameters.Add("@SoTC", SqlDbType.Int).Value = subject.Credits;
                sqlCommand.Parameters.Add("@HocKy", SqlDbType.Int).Value = subject.Semester;
                sqlCommand.Parameters.Add("@MoTa", SqlDbType.NVarChar).Value = subject.Description;
                sqlCommand.ExecuteNonQuery(); // thực thi câu truy vấn
            }
            catch
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }

        public bool deleteSubject(string MaMH)
        {
            SqlConnection sqlConnection = Connections.GetSqlConnection();
            string query = @"delete Subject where SubjectID = @MaMH";
            try
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.Add("@MaMH", SqlDbType.VarChar).Value = MaMH;
                sqlCommand.ExecuteNonQuery(); // thực thi câu truy vấn
            }
            catch
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }

        public DataTable getTuitionFee()
        {
            DataTable dataTable = new DataTable();
            string query = "select * from TuitionFee";
            using (SqlConnection sqlConnection = Connections.GetSqlConnection())
            {
                sqlConnection.Open();
                dataAdapter = new SqlDataAdapter(query, sqlConnection);
                dataAdapter.Fill(dataTable);

                dataTable.Columns.Add("STT", typeof(int));
                // Điền số thứ tự cho từng hàng
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dataTable.Rows[i]["STT"] = i + 1;
                }

                // Thiết lập thứ tự hiển thị cột
                dataTable.Columns["STT"].SetOrdinal(0);

                sqlConnection.Close();
            }
            return dataTable;
        }

        public DataTable getStudentTuitionDetail(string studentID, int semester)
        {
            DataTable dataTable = new DataTable();
            string query = "SELECT * FROM TTSV(@StudentID, @Semester)";
            using (SqlConnection sqlConnection = Connections.GetSqlConnection())
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentID);
                    command.Parameters.AddWithValue("@Semester", semester);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(dataTable);
                }
                sqlConnection.Close();
            }
            return dataTable;
        }

        //// thêm chương trình đào tạo

        //// thêm mới chương trình đạo tạo


        //}


        //// cập nhật chương trình đào tạo


        //// xóa chương trinh đào tạo



        //// tìm kiếm chương trinhd đào tạo
        ////public DataTable SearchCTDT(string query)
        ////{
        ////    DataTable dataTable = new DataTable();
        ////    using (SqlConnection sqlConnection = Connections.GetSqlConnection())
        ////    {
        ////        sqlConnection.Open();
        ////        dataAdapter = new SqlDataAdapter(query, sqlConnection);
        ////        dataAdapter.Fill(dataTable);
        ////        sqlConnection.Close();
        ////    }
        ////    return dataTable;
        ////}


        //// Đăng ký học phần
        //// hiển thị danh sách sinh viên
        //public DataTable getSinhVien()
        //{
        //    DataTable dataTable = new DataTable();
        //    string query = @"select MaSV, HoTen from SinhVien";
        //    using (SqlConnection sqlConnection = Connections.GetSqlConnection())
        //    {
        //        sqlConnection.Open();
        //        dataAdapter = new SqlDataAdapter(query, sqlConnection);
        //        dataAdapter.Fill(dataTable);

        //        dataTable.Columns.Add("STT", typeof(int));

        //        // Điền số thứ tự cho từng hàng
        //        for (int i = 0; i < dataTable.Rows.Count; i++)
        //        {
        //            dataTable.Rows[i]["STT"] = i + 1;
        //        }

        //        // Thiết lập thứ tự hiển thị cột
        //        dataTable.Columns["STT"].SetOrdinal(0);
        //        sqlConnection.Close();
        //    }
        //    return dataTable;
        //}

        //// hiển thị danh sách các môn học
        //public DataTable getHocPhan(int hocKy)
        //{
        //    DataTable dataTable = new DataTable();
        //    string query = @"select MaMH, TenMH, HocKy from CtrinhDT where HocKy = @HocKy";
        //    using (SqlConnection sqlConnection = Connections.GetSqlConnection())
        //    {
        //        sqlConnection.Open();
        //        sqlCommand = new SqlCommand(query, sqlConnection);
        //        sqlCommand.Parameters.AddWithValue("@HocKy", hocKy);
        //        dataAdapter = new SqlDataAdapter(sqlCommand);
        //        dataAdapter.Fill(dataTable);

        //        dataTable.Columns.Add("STT", typeof(int));

        //        // Điền số thứ tự cho từng hàng
        //        for (int i = 0; i < dataTable.Rows.Count; i++)
        //        {
        //            dataTable.Rows[i]["STT"] = i + 1;
        //        }

        //        // Thiết lập thứ tự hiển thị cột
        //        dataTable.Columns["STT"].SetOrdinal(0);
        //        sqlConnection.Close();
        //    }
        //    return dataTable;
        //}


        //// hiển thị danh sách đã đăng ký học phần
        //public DataTable getDKHP()
        //{
        //    DataTable dataTable = new DataTable();
        //    string query = @"select * from DKHP";
        //    using (SqlConnection sqlConnection = Connections.GetSqlConnection())
        //    {
        //        sqlConnection.Open();
        //        dataAdapter = new SqlDataAdapter(query, sqlConnection);
        //        dataAdapter.Fill(dataTable);

        //        dataTable.Columns.Add("STT", typeof(int));

        //        // Điền số thứ tự cho từng hàng
        //        for (int i = 0; i < dataTable.Rows.Count; i++)
        //        {
        //            dataTable.Rows[i]["STT"] = i + 1;
        //        }

        //        // Thiết lập thứ tự hiển thị cột
        //        dataTable.Columns["STT"].SetOrdinal(0);
        //        sqlConnection.Close();
        //    }
        //    return dataTable;
        //}


        //// đăng ký học phần
        //public bool insertDKHP(string maSV, string hoTen, string maMH, string tenMH, int hocKy)
        //{
        //    SqlConnection sqlConnection = Connections.GetSqlConnection();
        //    string query = @"insert into DKHP values ('" + maSV + "', N'" + hoTen + "', '" + maMH + "', N'" + tenMH + "', '" + hocKy + "')";
        //    try
        //    {
        //        sqlConnection.Open();
        //        sqlCommand = new SqlCommand(query, sqlConnection);
        //        sqlCommand.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Sinh viên " + hoTen + " đã đăng ký môn " + tenMH + "");
        //        return false;
        //    }
        //    finally
        //    {
        //        sqlConnection.Close();
        //    }
        //    return true;
        //}


        //// hủy đăng ký học phần
        //public bool deleteDKHP(string MaSV, string MaMH)
        //{
        //    SqlConnection sqlConnection = Connections.GetSqlConnection();
        //    string query = @"delete DKHP where MaSV = @MaSV and MaMH = @MaMH";
        //    try
        //    {
        //        sqlConnection.Open();
        //        sqlCommand = new SqlCommand(query, sqlConnection);
        //        sqlCommand.Parameters.Add("@MaSV", SqlDbType.VarChar).Value = MaSV;
        //        sqlCommand.Parameters.Add("@MaMH", SqlDbType.VarChar).Value = MaMH;
        //        sqlCommand.ExecuteNonQuery(); // thực thi câu truy vấn

        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    finally
        //    {
        //        sqlConnection.Close();
        //    }
        //    return true;
        //}

        //// Quản lý điểm

        //public bool InsertDiem()
        //{
        //    string insertQuery = @"INSERT INTO Diem (MaSV, HoTen, MaMH, TenMH)
        //                        SELECT MaSV, TenSV, MaMH, TenMH
        //                        FROM DKHP
        //                        WHERE NOT EXISTS (SELECT 1 FROM Diem
        //                             WHERE Diem.MaSV = DKHP.MaSV AND Diem.MaMH = DKHP.MaMH);";

        //    using (SqlConnection sqlConnection = Connections.GetSqlConnection())
        //    {
        //        try
        //        {
        //            sqlConnection.Open();
        //            using (SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection))
        //            {
        //                insertCommand.ExecuteNonQuery(); // Thực thi câu lệnh chèn
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Xử lý lỗi tại đây (ghi log, hiển thị thông báo lỗi, v.v.)
        //            MessageBox.Show("Lỗi: " + ex.Message);
        //            return false;
        //        }
        //        finally
        //        {
        //            sqlConnection.Close();
        //        }
        //    }
        //    return true;
        //}

        //public bool DeleteDataDiem()
        //{
        //    string insertQuery = @"DELETE FROM Diem 
        //        WHERE NOT EXISTS (
        //            SELECT 1 
        //            FROM DKHP 
        //            WHERE Diem.MaSV = DKHP.MaSV AND Diem.MaMH = DKHP.MaMH
        //        );";

        //    using (SqlConnection sqlConnection = Connections.GetSqlConnection())
        //    {
        //        try
        //        {
        //            sqlConnection.Open();
        //            using (SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection))
        //            {
        //                insertCommand.ExecuteNonQuery(); // Thực thi câu lệnh chèn
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Xử lý lỗi tại đây (ghi log, hiển thị thông báo lỗi, v.v.)
        //            MessageBox.Show("Lỗi: " + ex.Message);
        //            return false;
        //        }
        //        finally
        //        {
        //            sqlConnection.Close();
        //        }
        //    }
        //    return true;
        //}


        //public DataTable getAllSinhVien1()
        //{
        //    DataTable dataTable = new DataTable();
        //    string query = "select * from Diem";
        //    using (SqlConnection sqlConnection = Connections.GetSqlConnection())
        //    {
        //        sqlConnection.Open();
        //        dataAdapter = new SqlDataAdapter(query, sqlConnection);
        //        dataAdapter.Fill(dataTable);

        //        dataTable.Columns.Add("STT", typeof(int));

        //        // Điền số thứ tự cho từng hàng
        //        for (int i = 0; i < dataTable.Rows.Count; i++)
        //        {
        //            dataTable.Rows[i]["STT"] = i + 1;
        //        }

        //        // Thiết lập thứ tự hiển thị cột
        //        dataTable.Columns["STT"].SetOrdinal(0);



        //        sqlConnection.Close();
        //    }
        //    return dataTable;
        //}

        //public bool UpdateDiem(string maSV, string maMH, float diemQT, float diemThi, float diemTB, string diemChu)
        //{
        //    string query = @"UPDATE Diem
        //             SET DiemQT = @DiemQT, DiemThi = @DiemThi, DiemTB = @DiemTB, DiemChu = @DiemChu
        //             WHERE MaSV = @MaSV AND MaMH = @MaMH";

        //    using (SqlConnection sqlConnection = Connections.GetSqlConnection())
        //    {
        //        try
        //        {
        //            sqlConnection.Open();
        //            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        //            {
        //                sqlCommand.Parameters.Add("@MaMH", SqlDbType.VarChar).Value = maMH;
        //                sqlCommand.Parameters.Add("@MaSV", SqlDbType.VarChar).Value = maSV;
        //                sqlCommand.Parameters.Add("@DiemQT", SqlDbType.Float).Value = diemQT;
        //                sqlCommand.Parameters.Add("@DiemThi", SqlDbType.Float).Value = diemThi;
        //                sqlCommand.Parameters.Add("@DiemTB", SqlDbType.Float).Value = diemTB;
        //                sqlCommand.Parameters.Add("@DiemChu", SqlDbType.Char).Value = diemChu;

        //                sqlCommand.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Xử lý lỗi tại đây (ghi log, hiển thị thông báo lỗi, v.v.)
        //            MessageBox.Show("Lỗi: " + ex.Message);
        //            return false;
        //        }
        //        finally
        //        {
        //            sqlConnection.Close();
        //        }
        //    }
        //    return true;
        //}
    }
}
