using KlazzRelationShipFinder.KRSFinder;
using KlazzRelationShipFinder.KRSFinder.Base;
using KlazzRelationShipFinder.KRSFinder.LogPrinter;
using KlazzRelationShipFinder.KRSFinder.MessageSaver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KRS_Gui
{

    public partial class GuiForm
    {
        //Key:Package
        private Dictionary<string, TreeNode> nodeSaver = new Dictionary<string, TreeNode>();

        void node_Click(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.BeginUpdate();
            if (e.Node.Nodes.Count == 0 && e.Node.ImageIndex == LISTIMAGE_KLAZZ)
            {
                List<Var> vars = RelationSaver.relations[e.Node.Name];
                //若节点为Klazz节点
                for (int i = 0; i < vars.Count; i++)
                {
                    TreeNode node = new TreeNode();
                    node.Text = vars[i].var_name;
                    node.Tag = i;
                    node.ImageIndex = LISTIMAGE_OBJ;
                    node.SelectedImageIndex = LISTIMAGE_OBJ;
                    e.Node.Nodes.Add(node);
                }
            }
            else if (e.Node.ImageIndex == LISTIMAGE_OBJ)
            {
                listView1.Items.Clear();
                List<Var> vars = RelationSaver.relations[e.Node.Parent.Name];
                string sourceName = SourceNameSaver.getSourceName(e.Node.Parent.Name);
                if (sourceName != null)
                {
                    ListViewItem item = new ListViewItem("本成员的隶属类的原类名为" + sourceName);
                    listView1.Items.Add(item);
                }
                Var v = vars[(int)e.Node.Tag];
                foreach (string comment in v.comments)
                {
                    ListViewItem item = new ListViewItem(comment);
                    listView1.Items.Add(item);
                }
            }
            treeView1.EndUpdate();
        }

        void button2_Click(object sender, EventArgs e)
        {
            JadxConfig config = new JadxConfig();
            config.Show();
        }

        void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            new Thread(() =>
            {
                RelationSaver.Clear();
                string path = textBox1.Text;
                new SmaliHandler(path).analyseSmaliFiles();
                Invoke(new Action(() => listKlazzesInTreeView()));
            }).Start();
        }

        public void AppendText(string text)
        {
            Invoke(new Action(() =>
            {
                richTextBox1.Text = (text);
                richTextBox1.ScrollToCaret();
            }));
        }

        public void ShowText(string text)
        {
            Invoke(new Action(() =>
            {
                richTextBox2.Text = (text);
                //richTextBox1.ScrollToCaret();
            }));
        }

        private void listKlazzesInTreeView()
        {
            //开始更新treeView
            treeView1.BeginUpdate();
            nodeSaver.Clear();
            treeView1.Nodes.Clear();

            Dictionary<string, List<Var>> relations = RelationSaver.relations;
            string Json = RelationSaver.convertRelationToJson();
            Log.log(Json);
            StreamWriter writer = new StreamWriter("./result.json", false, Encoding.UTF8);
            writer.AutoFlush = true;
            writer.Write(Json);
            writer.Close();
            writer = null;

            Json = RelationSaver.convertCallStackToJson();
            //Log.log(Json);
            writer = new StreamWriter("./callstack.json", false, Encoding.UTF8);
            writer.AutoFlush = true;
            writer.Write(Json);
            writer.Close();

            //取出klazz列表
            foreach (string klazz in relations.Keys)
            {
                string package = getParentPackage(klazz);
                string klazz_name = getKlazz(klazz);
                handleNodes(package, klazz_name);
            }
            treeView1.EndUpdate();

            button1.Enabled = true;
        }

        /// <summary>
        /// 根据提供的package与class_name保存treeView的子节点
        /// </summary>
        /// <param name="package"></param>
        /// <param name="klazz_name"></param>
        private void handleNodes(string package, string klazz_name)
        {
            TreeNode package_node = nodeSaver.GetValueOrDefault(package, null);
            //若nodeSaver成员中未保存当前package的node
            if (package_node == null && !package.Equals(""))
            {
                string key = "";
                foreach (string s in package.Split("/"))
                {

                    TreeNode sub_node = nodeSaver.GetValueOrDefault(key + s, null);
                    if (sub_node == null)
                    {
                        //package的根目录未创建node的情况下
                        if (key.Equals(""))
                        {
                            sub_node = new TreeNode();
                            sub_node.Text = s;
                            sub_node.ImageIndex = LISTIMAGE_PACKAGE;
                            treeView1.Nodes.Add(sub_node);
                            nodeSaver[s] = sub_node;
                            key = s + "/";
                            package_node = sub_node;
                            continue;
                        }

                        //若未创建node非根节点,则为此节点添加子节点
                        string p_package = getParentPackage(key);
                        sub_node = nodeSaver[p_package];
                        TreeNode s_node = new TreeNode();
                        s_node.Text = s;
                        s_node.ImageIndex = LISTIMAGE_PACKAGE;
                        sub_node.Nodes.Add(s_node);
                        nodeSaver[key + s] = s_node;
                        key = key + s + "/";
                        package_node = s_node;
                        continue;
                    }

                    key = key + s + "/";
                    package_node = sub_node;
                }
            }

            if (package.Equals(""))
            {
                TreeNode klazz = new TreeNode();
                klazz.Text = klazz_name;
                klazz.Name = klazz_name;
                klazz.ImageIndex = LISTIMAGE_KLAZZ;
                klazz.SelectedImageIndex = LISTIMAGE_KLAZZ;
                treeView1.Nodes.Add(klazz);
            }
            else
            {
                TreeNode klazz = new TreeNode();
                klazz.Text = klazz_name;
                klazz.Name = package + "/" + klazz_name;
                klazz.ImageIndex = LISTIMAGE_KLAZZ;
                klazz.SelectedImageIndex = LISTIMAGE_KLAZZ;
                package_node.Nodes.Add(klazz);
            }
        }

        /// <summary>
        /// 获取class的所在package
        /// </summary>
        /// <param name="klazz">A/B/C</param>
        /// <returns>A/B</returns>
        private string getParentPackage(string klazz)
        {
            int length = klazz.Contains("/") ? klazz.LastIndexOf("/") : 0;
            return klazz.Substring(0, length);
        }

        /// <summary>
        /// 获取class名
        /// </summary>
        /// <param name="klazz">A/B/C</param>
        /// <returns>C</returns>
        private string getKlazz(string klazz)
        {
            if (klazz.Contains("/"))
            {
                string[] ks = klazz.Split("/");
                return ks[ks.Length - 1];
            }
            return klazz;
        }

    }
}
