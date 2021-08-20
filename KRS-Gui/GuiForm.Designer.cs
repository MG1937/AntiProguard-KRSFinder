
using System.Windows.Forms;

namespace KRS_Gui
{
    partial class GuiForm
    {
        public const int LISTIMAGE_PACKAGE = 0;

        public const int LISTIMAGE_KLAZZ = 1;

        public const int LISTIMAGE_OBJ = 2;

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "KRS Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(93, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(695, 23);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "Path";
            // 
            // treeView1
            // 
            this.treeView1.ImageIndex = 0;
            this.treeView1.Location = new System.Drawing.Point(2, 41);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(230, 412);
            this.treeView1.TabIndex = 2;
            treeView1.ImageList = new System.Windows.Forms.ImageList();
            treeView1.ImageList.Images.Add(Properties.Resources.package_obj);
            treeView1.ImageList.Images.Add(Properties.Resources.class_obj);
            treeView1.ImageList.Images.Add(Properties.Resources.field_public_obj);
            treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.node_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.WindowText;
            this.richTextBox1.Font = new System.Drawing.Font("微软雅黑", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.Location = new System.Drawing.Point(238, 307);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(561, 146);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(238, 41);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(561, 260);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            listView1.Columns.Add("Comment", -2, HorizontalAlignment.Left);
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            // 
            // GuiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "GuiForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "KRS Auth:MG193.7";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ListView listView1;
    }
}

