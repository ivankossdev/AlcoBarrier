using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlcoBarrier
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) =>
            {
                // ИЗМЕНЕНО (этап C2): Console.WriteLine -> Logger.Error.
                Logger.Error("UI thread exception", e.Exception);
                MessageBox.Show(e.Exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Logger.Error("Unhandled exception", e.ExceptionObject as Exception);
            };

            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                Logger.Error("Fatal startup error", ex);
                MessageBox.Show(ex.ToString(), "Ошибка запуска", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}