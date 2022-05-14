using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class SettingForm : Form
    {
        public Form1 f;
        public int ind;
        public string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public SettingForm(Form1 f1, int n)
        {
            InitializeComponent();
            f = f1;
            ind = n;
            colorDialog1.FullOpen = true;
            colorDialog1.Color = this.BackColor;
            path += @"\files" + @"\" + Convert.ToInt32(ind);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            f.set_color_of_tp(colorDialog1.Color, ind);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                f.rename(textBox1.Text, ind);
                File.WriteAllText(path + @"\name.txt", textBox1.Text);
            }

            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string p = path;
            p = p.Substring(0, p.LastIndexOf(@"\"));
            string[] dirs = Directory.GetDirectories(p);
            Directory.Delete(dirs[ind - 2], true);
            for (int i = ind - 2 + 1; i < dirs.Length; i++)
            {
                Directory.Move(dirs[i], dirs[i - 1]);
            }

            MessageBox.Show("Чтобы продолжить, перезагрузите приложение!", "Удаление плейлиста", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Hide();
        }
    }
}
