using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Show : Form
    {
        string file;
        public Show(string p)
        {
            file = p;
            InitializeComponent();
        }

        private void Show_Load(object sender, EventArgs e)
        {
            try
            {
                string type = get_type(file);
                if (type == "mp4")
                {
                    axWindowsMediaPlayer1.Visible = true;
                    axWindowsMediaPlayer1.Dock = DockStyle.Fill;
                    axWindowsMediaPlayer1.URL = file;
                }
                else
                {
                    this.Padding = Padding.Empty;
                    this.Size = System.Drawing.Image.FromFile(file).Size;
                    this.MaximumSize = System.Drawing.Image.FromFile(file).Size;
                    this.MinimumSize = System.Drawing.Image.FromFile(file).Size;
                    PictureBox p = new PictureBox();
                    p.Padding = Padding.Empty;
                    p.Location = new Point(0, 0);
                    p.BorderStyle = BorderStyle.None;
                    p.Image = Image.FromFile(file);
                    p.Size = this.Size;

                    Controls.Add(p);
                }
            }
            catch { }
        }
        private string get_type(string p)
        {
            int ind = p.LastIndexOf('.');
            string s = p.Substring(ind + 1);
            return s;
        }

        private void Show_FormClosed(object sender, FormClosedEventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }
    }
}
