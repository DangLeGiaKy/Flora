using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using test.GUI;

namespace test
{
    internal static class Program

    {
        public static string server = "";
        public static string database = "";
        public static string uid = "";
        public static string password = "";
        public static string authen = "windows";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());

        }
    }
}
