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
    public partial class AddTab : Form
    {
        Form1 form1;
        public AddTab(Form1 f)
        {
            InitializeComponent();
            form1 = f;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                {
                    form1.set_text(t.Text);

                    var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    path += @"\files";

                    string[] directories = Directory.GetDirectories(path);
                    Directory.CreateDirectory(path + @"\" + Convert.ToString(directories.Length + 2));
                    path += @"\" + Convert.ToString(directories.Length + 2);

                    File.AppendAllText(path + @"\description.txt", richTextBox1.Text);
                    File.AppendAllText(path + @"\name.txt", t.Text);
                    File.AppendAllText(path + @"\content.txt", string.Empty);

                    this.Hide();
                }
            }
            catch { }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
