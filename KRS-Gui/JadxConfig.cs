using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace KRS_Gui
{
    public partial class JadxConfig : Form
    {
        public JadxConfig()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string jadx = jadxPath.Text.ToString() + "/lib/jadx-gui-dev.jar";
            if (!File.Exists(jadx))
            {
                warning.Text = "jadx-gui-dev.jar no found in Jadx root path!";
                return;
            }

            StreamWriter writer = new StreamWriter(new FileStream("./jadxPath.txt", FileMode.Create));
            writer.Write(jadxPath.Text.ToString());
            writer.Close();

            string callstack = callstackPath.Text.ToString();
            string result = resultPath.Text.ToString();
            Process process = new Process();
            process.StartInfo.FileName = "java.exe";
            process.StartInfo.Arguments = "-javaagent:./AgentPlug.jar=\"" + result + "|" + callstack + "\" -jar " + jadx;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
        }
    }
}
