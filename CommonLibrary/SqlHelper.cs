using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class SqlHelper
    {
        public static DataSet ExecuteSqlCommand(string connectionString, string command)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                DataSet data = new DataSet();
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(command, connectionString);
                da.Fill(data);
                conn.Close();
                return data;
            }
        }

        public static string ExcuteObjectQuery(string connectionString, string sqlQuery)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    object result = cmd.ExecuteScalar();
                    conn.Close();
                    return result == null ? null : result.ToString();
                }
            }
        }

        public static DataTable ExcuteDataQuery(string connectionString, string sqlQuery)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable result=new DataTable();
                    adp.Fill(result);
                }
            }
            return null;
        }
    }
}