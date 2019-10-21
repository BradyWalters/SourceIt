using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace SourceIt._classes
{
    class localDB
    {
        private static SqlConnection Get_DB_Connection()
        {
            string connection_string = Properties.Settings.Default.connection_String;
            SqlConnection cn_connection = new SqlConnection(connection_string);
            if (cn_connection.State != ConnectionState.Open) cn_connection.Open();

            return cn_connection;
        }

        public static DataTable GetDataTable(string sqlText)
        {
            SqlConnection cn_connection = Get_DB_Connection();

            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlText, cn_connection);
            adapter.Fill(table);

            Close_DB_connection();

            return table;
        }

        public static void Execute_SQL(string sqlText)
        {
            SqlConnection cn_connection = Get_DB_Connection();

            SqlCommand command = new SqlCommand(sqlText, cn_connection);
            command.ExecuteNonQuery();

            Close_DB_connection();
        }

        public static void Close_DB_connection()
        {
            string connection_string = Properties.Settings.Default.connection_String;

            SqlConnection cn_connection = new SqlConnection(connection_string);

            if (cn_connection.State != ConnectionState.Closed) cn_connection.Close();
        }
    }
}
