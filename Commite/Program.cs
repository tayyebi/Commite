using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Commite
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += (sender, args) => MyError(sender, args.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => MyError(sender, args.ExceptionObject as Exception);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void MyError(object sender, Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
}
