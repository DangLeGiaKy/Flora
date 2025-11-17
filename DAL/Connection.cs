using System.Data.SqlClient;

namespace test.DAL
{
    internal class Connection
    {
        public static string strConnection = "";

        // Hàm static để gọi như Database.connect()
        public static SqlConnection connect()
        {
            if (Program.authen == "windows")
            {
                strConnection =
                    "Data Source=" + Program.server +
                    ";Initial Catalog=" + Program.database +
                    ";Integrated Security=True;TrustServerCertificate=True;";
            }
            else
            {
                strConnection =
                    "server=" + Program.server +
                    ";database=" + Program.database +
                    ";uid=" + Program.uid +
                    ";pwd=" + Program.password +
                    ";TrustServerCertificate=True;";
            }

            return new SqlConnection(strConnection);
        }

        // Hàm instance để DAL dùng
        public SqlConnection GetConnection()
        {
            return connect(); // gọi lại hàm static
        }
    }
}
