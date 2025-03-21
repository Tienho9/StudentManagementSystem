using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BaiTapNhom
{
    internal class Connections
    {
        private static string stringConnection = @"Data Source= TIENN\SQLEXPRESS; Initial Catalog = Student_Management_System;Integrated Security=True";
        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(stringConnection);
        }
    }
}
