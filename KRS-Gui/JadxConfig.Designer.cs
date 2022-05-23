
using System.IO;

namespace KRS_Gui
{
    partial class JadxConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.jadxPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.callstackPath = new System.Windows.Forms.TextBox();
            this.resultPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.warning = new System.Windows.Forms.Label();
            
            this.SuspendLayout();
            // 
            // jadxPath
            // 
            this.jadxPath.Location = new System.Drawing.Point(130, 12);
            this.jadxPath.Name = "jadxPath";
            this.jadxPath.Size = new System.Drawing.Size(327, 27);
            this.jadxPath.TabIndex = 0;
            string path = "";
            if (File.Exists("./jadxPath.txt"))
            {
                StreamReader reader = new StreamReader(new FileStream("./jadxPath.txt", FileMode.Open));
                path = reader.ReadLine().Replace("\n", "");
                reader.Close();
            }
            jadxPath.Text = path;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Jadx root path:";
            // 
            // callstackPath
            // 
            this.callstackPath.Location = new System.Drawing.Point(130, 59);
            this.callstackPath.Name = "callstackPath";
            this.callstackPath.Size = new System.Drawing.Size(327, 27);
            this.callstackPath.TabIndex = 2;
            this.callstackPath.Text = "./callstack.json";
            // 
            // resultPath
            // 
            this.resultPath.Location = new System.Drawing.Point(130, 108);
            this.resultPath.Name = "resultPath";
            this.resultPath.Size = new System.Drawing.Size(327, 27);
            this.resultPath.TabIndex = 3;
            this.resultPath.Text = "./result.json";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Callstack path:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Result path:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 141);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(445, 29);
            this.button1.TabIndex = 6;
            this.button1.Text = "JADX Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // warning
            // 
            this.warning.AutoSize = true;
            this.warning.ForeColor = System.Drawing.Color.Red;
            this.warning.Location = new System.Drawing.Point(12, 177);
            this.warning.Name = "warning";
            this.warning.Size = new System.Drawing.Size(101, 20);
            this.warning.TabIndex = 7;
            this.warning.Text = "Inject JADX!!";
            // 
            // JadxConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 206);
            this.Controls.Add(this.warning);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.resultPath);
            this.Controls.Add(this.callstackPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.jadxPath);
            this.MaximumSize = new System.Drawing.Size(487, 353);
            this.Name = "JadxConfig";
            this.Text = "JadxConfig";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox jadxPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox callstackPath;
        private System.Windows.Forms.TextBox resultPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label warning;
    }

}