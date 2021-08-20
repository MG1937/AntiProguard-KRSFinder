using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KRS_Gui
{
    public static class Program
    {
        public static GuiForm gui;
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //AllocConsole();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            gui = new GuiForm();
            Application.Run(gui);
        }
    }
}
