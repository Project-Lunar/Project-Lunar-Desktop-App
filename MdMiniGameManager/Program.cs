using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectLunarUI
{
    static class Program
    {
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        private static Mutex instanceMutex;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /// Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;

            bool isNewMutex = false;
            instanceMutex = new Mutex(true, "Project-Lunar", out isNewMutex);
            if (isNewMutex)
            {
                try
                {
                    Application.Run(new frmMain());
                }
                catch (Exception ex)
                {
                    HandleGeneralException(ex);
                }
            }
            else
            {
                frmMain mainForm = new frmMain();
                string title = mainForm.Text;
                IntPtr handle = FindWindow(null, title);

                if (handle == IntPtr.Zero)
                {
                    return;
                }

                SetForegroundWindow(handle);

                return;
            }
        }

        private static void HandleGeneralException(Exception ex)
        {
            SwingMessageBox.Show("An unexpected error occurred. If this error persists, please contact the developers at ModMyClassic. The application will exit now.",
                                 "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            File.WriteAllText($@"{Application.StartupPath}\lunar_data\logs\error-{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.txt", ex.ToString());

            Application.Exit();
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleGeneralException(e.Exception);
        }
    }
}
