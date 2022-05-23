using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

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
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello");
            //AllocConsole();

            //JObject json = new JObject();
            

            //Console.WriteLine("Hello");
            //List<Msg> msgs = new List<Msg>();
            //for (int i = 0; i < 10; i++)
            //{
            //    Msg m = new Msg();
            //    m.init();
            //    m.key = i + "!!";
            //    msgs.Add(m);
            //}
            //Console.WriteLine(json.ToString());
            //Console.ReadKey();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            gui = new GuiForm();
            Application.Run(gui);
        }
    }

    public class Msg
    {
        public string key { set; get; }
        public object comments;
        public void init() 
        {
            comments = new List<object>();
            for (int i = 0; i < 10; i++)
            {
                ((List<object>)comments).Add("List<" + i + ">");
            }
        }
    }
}
