using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Directory_Tree_Viewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void makeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Directory Tree Files | *.drt";
            result = sfd.ShowDialog();

            if (result != DialogResult.OK)
            {
                return;
            }

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C cd " + fbd.SelectedPath + " & tree /f /a > " + sfd.FileName;
            process.StartInfo = startInfo;
            process.Start();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Directory Tree Files | *.drt";
            DialogResult result = ofd.ShowDialog();

            if (result != DialogResult.OK)
            {
                return;
            }

            treeView1.Nodes.Clear();

            StreamReader sr = new StreamReader(ofd.FileName, Encoding.Default);

            sr.ReadLine();
            sr.ReadLine();
            sr.ReadLine();

            List<TreeNode> levels = new List<TreeNode>();
            levels.Add(new TreeNode(ofd.FileName));

            int level = 0, linecount = 3;
            bool isfolder;

            do
            {
                linecount++;
                isfolder = false;

                string input = sr.ReadLine();

                input = input.Replace('|', ' ');

                int spaces = 0;

                while (spaces < input.Length && input[spaces] == ' ')
                {
                    spaces++;
                }

                level = spaces / 4;
                
                if (input.Length > spaces && (input[spaces] == '+' || input[spaces] == '\\') && input.Substring(spaces + 1, 3) == "---")
                {
                    level++;
                    isfolder = true;
                }

                input = input.Substring(level * 4);

                while (level < levels.Count)
                {
                    levels[levels.Count - 2].Nodes.Add(levels[levels.Count - 1]);
                    levels.RemoveAt(levels.Count - 1);
                }

                if (isfolder)
                {
                    levels.Add(new TreeNode("[_] " + input));
                }

                else
                {
                    if (input != "")
                    {
                        levels[levels.Count - 1].Nodes.Add(input);
                    }
                }

            } while (!sr.EndOfStream);

            while (1 < levels.Count)
            {
                levels[levels.Count - 2].Nodes.Add(levels[levels.Count - 1]);
                levels.RemoveAt(levels.Count - 1);
            }

            treeView1.Nodes.Add(levels[0]);

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();

            about.ShowDialog();
        }
    }
}