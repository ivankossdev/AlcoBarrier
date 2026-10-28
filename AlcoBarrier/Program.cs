using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlcoBarrier
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ДОБАВЛЕНО (этап 5.6): глобальный перехват исключений.
            // UI-поток: исключения показываем MessageBox'ом и пишем в Console,
            // чтобы не терять их молча.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) =>
            {
                Console.WriteLine($"UI thread exception: {e.Exception}");
                MessageBox.Show(e.Exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            // Фоновые потоки: только Console, MessageBox из чужого потока опасен.
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Console.WriteLine($"Unhandled exception: {e.ExceptionObject}");
            };

            // ДОБАВЛЕНО (этап 5.6): конструктор MainForm (InitClass) выполняется ДО
            // Application.Run и не попадает под ThreadException. Ловим явно.
            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal startup error: {ex}");
                MessageBox.Show(ex.ToString(), "Ошибка запуска", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}