using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using WMPLib;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // получаем путь к программе
        public string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            tc.SelectedTab = tb2;
            LinkLabel.Left = (this.ClientSize.Width - LinkLabel.Width) / 2;
            tc.SelectedIndexChanged += Tc_SelectedIndexChanged;
            path += @"\files";
            Directory.CreateDirectory(path); // создаем папку
            start_page();
            show_tc();
        }
        public void start_page()
        {
            Label l = new Label();
            l.Text = "Thank you for using this app";
            l.Location = new Point(0, 100);
            l.Size = new Size(300, 50);
            l.Left = (this.Width - l.Width) / 2;

            tb2.Controls.Add(l);
        }
        private void show_tc()
        {
            try
            {
                // цикл для кажлого элемента списка directories
                string[] directories = Directory.GetDirectories(path);
                foreach (string e in directories)
                {
                    string s = File.ReadAllText(e + @"\name.txt");
                    tc.TabPages.Add(s);

                    

                    // List
                    ListBox l = new ListBox();
                    l.Name = "l" + Convert.ToString(tc.TabPages.Count - 1); // переименовать список
                    l.Size = new Size(553, 220);
                    l.BorderStyle = BorderStyle.FixedSingle;
                    l.Location = new Point(0, 100);
                    l.Left = (this.Width - l.Width) / 2;
                    l.SelectedIndexChanged += L_SelectedIndexChanged;


                    string[] items = File.ReadAllLines(e + @"\content.txt");
                    foreach (string it in items)
                    {
                        l.Items.Add(get_name(it));
                    }

                    // Add Song Button
                    Button AddSong = new Button();
                    AddSong.Name = "AddItem" + Convert.ToString(tc.TabPages.Count - 1); // переименовать кнопку
                    AddSong.Location = new Point(l.Location.X, l.Location.Y + l.Height + 10);
                    AddSong.Text = "Добавить";
                    AddSong.Size = new Size(l.Width / 2 - 10, 50);
                    AddSong.Click += AddSong_Click;

                    // Delete Song Button
                    Button DelSong = new Button();
                    DelSong.Name = "DelItem" + Convert.ToString(tc.TabPages.Count - 1); // переименовать кнопку
                    DelSong.Location = new Point(l.Location.X + AddSong.Width + 10, l.Location.Y + l.Height + 10);
                    DelSong.Text = "Удалить";
                    DelSong.Size = new Size(l.Width / 2, 50);
                    DelSong.Click += DelSong_Click;

                    
                    // Description
                    Label dcr = new Label();
                    dcr.Name = "dcr" + Convert.ToString(tc.TabPages.Count - 1); // переименовать Label
                    dcr.Size = new Size(l.Width - 60, 50);
                    dcr.Location = new Point(l.Location.X, 10);
                    dcr.BorderStyle = BorderStyle.None;
                    
                    string t = File.ReadAllText(e + @"\description.txt");
                    dcr.Text = t;
                    
                    
                    // Settings Button
                    Button SettingsButton = new Button();
                    SettingsButton.Name = "Setteings" + Convert.ToString(tc.TabPages.Count - 1); // переименовать кнопку
                    SettingsButton.Location = new Point(dcr.Location.X + dcr.Width + 10, 10);
                    SettingsButton.Size = new Size(50, 50);
                    SettingsButton.Text = "";
                    SettingsButton.Click += SettingsButton_Click;
                    string p1 = path;
                    p1 = p1.Substring(0, p1.Length - 16);
                    SettingsButton.BackgroundImage = Image.FromFile(p1 + @"\Settings-icon.png");

                    
                    // Average Duration

                    Label avrgd = new Label();
                    string[] pathes = File.ReadAllLines(e + @"\content.txt");
                    Tuple<int, int> tuple = new Tuple<int, int>(0, 0);
                    tuple = average_duration(pathes);

                    // вывести кол-во минут и секунд
                    avrgd.Text = "Средняя продолжительсность : " + Convert.ToString(tuple.Item1) + " м " + Convert.ToString(tuple.Item2) + " с ";
                    avrgd.AutoSize = true;
                    avrgd.Name = "avrgd" + Convert.ToString(tc.TabPages.Count - 1); // переименовать Label
                    avrgd.Location = new Point((l.Width - avrgd.Width) / 2, DelSong.Location.Y + DelSong.Height + 10);

                
                    tc.TabPages[tc.TabPages.Count - 1].Controls.Add(l);
                    tc.TabPages[tc.TabPages.Count - 1].Controls.Add(AddSong);
                    tc.TabPages[tc.TabPages.Count - 1].Controls.Add(DelSong);
                    tc.TabPages[tc.TabPages.Count - 1].Controls.Add(SettingsButton);
                    tc.TabPages[tc.TabPages.Count - 1].Controls.Add(dcr);
                    tc.TabPages[tc.TabPages.Count - 1].Controls.Add(avrgd);
                }
            }
            catch { }
        }
        public int Duration(String file)
        {
            WindowsMediaPlayer wmp = new WindowsMediaPlayerClass();
            IWMPMedia mediainfo = wmp.newMedia(file);
            return Convert.ToInt32(mediainfo.duration); // вернуть продолжительность файла в целом числе
        }
        public Tuple<int, int> average_duration(string[] pathes)
        {
            // если список пустой, то возращай нули
            if (pathes.Length == 0) return new Tuple<int, int>(0, 0); 
            int seconds = 0;
            // узнаем длительность кажлого элемента списка pathes
            foreach (string p in pathes)
                seconds += Duration(p);
            int avrs = 0, m = 0, sec = 0;
            avrs = seconds / pathes.Length;
            m = avrs / 60;
            sec = avrs % 60;
            return new Tuple<int, int>(m, sec);
        }
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            int ind = get_ind(c.Name);
            SettingForm sform = new SettingForm(this, ind); // создать новую форму и передать главную форму и индекс
            sform.ShowDialog(); // показывать созданную форму
        }
        public void set_color_of_tp(Color c, int ind)
        {
            tc.TabPages[ind].BackColor = c;
        }
        private void L_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Control c = (Control)sender;
                int ind = get_ind(c.Name);

                string s = "l" + Convert.ToString(ind);
                ListBox l = get_control(s);
                string[] directories = Directory.GetDirectories(path);
                string path1 = path;
                path1 += @"\" + Convert.ToString(ind);
                string[] items = File.ReadAllLines(path1 + @"\content.txt");
                string p = items[l.SelectedIndex];

                string type = get_type(p);
                // если расширение mp3 или wav, то запускай проигрыватель
                if (type == "mp3" || type == "wav")
                    axWindowsMediaPlayer1.URL = Convert.ToString(items[l.SelectedIndex]);
                else // иначе создавай новую форму
                {
                    Show n = new Show(p); // создать новую форму и передать выбранный файл 
                    n.ShowDialog(); // показать форму
                }
            }
            catch { }
        }
        private string get_type(string p)
        {
            try
            {
                int ind = p.LastIndexOf('.');
                string s = p.Substring(ind + 1);
                return s;
            }
            catch
            {
                return string.Empty;
            }
        }
        private void DelSong_Click(object sender, EventArgs e)
        {
            try
            {
                axWindowsMediaPlayer1.URL = string.Empty;

                Control c = (Control)sender;
                int ind = get_ind(c.Name);

                string s = "l" + Convert.ToString(ind);
                ListBox l = get_control(s);
                int index = l.SelectedIndex;

                if (l.Items.Count != 1) // если кол-во элементов списка не равен 1 
                {
                    if (index != l.Items.Count - 1) // если индекс не равен последнему элементу
                    {
                        l.SelectedIndex++;
                        l.Focus(); // меняем фокус
                    }
                    else
                    {
                        l.SelectedIndex--;
                        l.Focus();
                    }
                }
                else
                {
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                }


                string[] directories = Directory.GetDirectories(path);
                string path1 = path;
                path1 += @"\" + Convert.ToString(ind);
                string[] items = File.ReadAllLines(path1 + @"\content.txt");
                List<string> l1 = new List<string>();

                // цикл с нуля до конца кол-во элементов в списке
                for (int i = 0; i < items.Length; i++)
                {
                    if (i != index) // если i не равен индексу
                    {
                        l1.Add(items[i]);
                    }
                }

                File.WriteAllText(path1 + @"\content.txt", string.Empty);
                // каждый элемент списка l1 добавляем в файл
                foreach (string it in l1)
                {
                    File.AppendAllText(path1 + @"\content.txt", it + "\n");
                }

                l.Items.RemoveAt(index);

                string s1 = "avrgd" + Convert.ToString(ind);
                Label ll = Get_control(s1);
                string[] pathes = File.ReadAllLines(path1 + @"\content.txt");
                Tuple<int, int> tuple = new Tuple<int, int>(0, 0);
                tuple = average_duration(pathes);
                ll.Text = "Средняя продолжительсность : " + Convert.ToString(Convert.ToString(tuple.Item1) + " м " + Convert.ToString(tuple.Item2) + " с ");

            }
            catch { }
        }
        private void AddSong_Click(object sender, EventArgs e)
        {
            try
            {
                Control c = (Control)sender;
                int ind = get_ind(c.Name);

                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    InitialDirectory = @"C:\",
                    Title = "Browse Media Files",

                    CheckFileExists = true,
                    CheckPathExists = true,

                    Filter = "All Media Files|*.wav;*.mp3;*.mp4;*.png;*.jpg;*.jpeg",

                    FilterIndex = 2,
                    RestoreDirectory = true,

                    ReadOnlyChecked = true,
                    ShowReadOnly = true
                };
                if (openFileDialog1.ShowDialog() == DialogResult.OK) // если выборка файла прошла успешна 
                {
                    string s = "l" + Convert.ToString(ind);
                    ListBox l = get_control(s);
                    l.Items.Add(get_name(openFileDialog1.FileName));

                    string path1 = path + @"\" + Convert.ToString(ind);
                    File.AppendAllText(path1 + @"\content.txt", openFileDialog1.FileName + "\n");

                    string s1 = "avrgd" + Convert.ToString(ind);
                    Label l1 = Get_control(s1);
                    string[] pathes = File.ReadAllLines(path1 + @"\content.txt");
                    Tuple<int, int> tuple = new Tuple<int, int>(0, 0);
                    tuple = average_duration(pathes);
                    l1.Text = "Средняя продолжительсность : " + Convert.ToString(Convert.ToString(tuple.Item1) + " м " + Convert.ToString(tuple.Item2) + " с ");

                }
            }
            catch { }
        }
        private int get_ind(string name)
        {
            try
            {
                string s = "";
                // цикл с нуля до конца длины переменной name
                for (int i = 0; i < name.Length; i++)
                {
                    // если i-ый элемент является числом
                    if (Convert.ToChar(name[i]) >= '0' && Convert.ToChar(name[i]) <= '9')
                    {
                        s += name[i];
                    }
                }
                return Convert.ToInt32(s);
            }
            catch 
            {
                return -1;
            }
        }
        public ListBox get_control(string name)
        {
            try
            {
                for (int i = 0; i < tc.TabPages.Count; i++)
                {
                    if (tc.TabPages[i].Controls.ContainsKey(name))
                    {
                        return (ListBox)tc.TabPages[i].Controls[name];
                    }
                }
                return new ListBox();
            }
            catch 
            {
                return new ListBox();
            }
        }
        public Label Get_control(string name)
        {
            try
            {
                // цикл с нуля до конца кол-во вкладок в табконтрол
                for (int i = 0; i < tc.TabPages.Count; i++)
                {
                    // если в i-ым tabpage есть контрол с именем name
                    if (tc.TabPages[i].Controls.ContainsKey(name))
                    {
                        return (Label)tc.TabPages[i].Controls[name];
                    }
                }
                return new Label();
            }
            catch
            {
                return new Label();
            }
        }
        private void Tc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // если пользователь выбрал первую вкладку
                if (tc.SelectedTab == tb1)
                {
                    AddTab tab = new AddTab(this); // создать форму и передать главную форму как аргумент
                    tab.ShowDialog(); // показать созданную форму

                    // List
                    ListBox l = new ListBox();
                    l.Name = "l" + Convert.ToString(tc.TabPages.Count - 1);
                    l.Size = new Size(553, 220);
                    l.BorderStyle = BorderStyle.FixedSingle;
                    l.Location = new Point(0, 100);
                    l.Left = (this.Width - l.Width) / 2;
                    l.SelectedIndexChanged += L_SelectedIndexChanged;


                    // Add Song Button
                    Button AddSong = new Button();
                    AddSong.Name = "AddItem" + Convert.ToString(tc.TabPages.Count - 1);
                    AddSong.Location = new Point(l.Location.X, l.Location.Y + l.Height + 10);
                    AddSong.Text = "Добавить";
                    AddSong.Size = new Size(l.Width / 2 - 10, 50);
                    AddSong.Click += AddSong_Click;

                    // Delete Song Button
                    Button DelSong = new Button();
                    DelSong.Name = "DelItem" + Convert.ToString(tc.TabPages.Count - 1);
                    DelSong.Location = new Point(l.Location.X + AddSong.Width + 10, l.Location.Y + l.Height + 10);
                    DelSong.Text = "Удалить";
                    DelSong.Size = new Size(l.Width / 2, 50);
                    DelSong.Click += DelSong_Click;



                    // Description
                    Label dcr = new Label();
                    dcr.Name = "dcr" + Convert.ToString(tc.TabPages.Count - 1);
                    dcr.Size = new Size(l.Width - 60, 50);
                    dcr.Location = new Point(l.Location.X, 10);
                    dcr.BorderStyle = BorderStyle.None;
                    string pp = path;
                    pp += @"\" + Convert.ToString(tc.TabPages.Count - 1) + @"\description.txt";
                    dcr.Text = File.ReadAllText(pp);


                    // Settings Button
                    Button SettingsButton = new Button();
                    SettingsButton.Name = "Setteings" + Convert.ToString(tc.TabPages.Count - 1);
                    SettingsButton.Location = new Point(dcr.Location.X + dcr.Width + 10, 10);
                    SettingsButton.Size = new Size(50, 50);
                    SettingsButton.Text = "";
                    SettingsButton.Click += SettingsButton_Click;
                    string p1 = path;
                    p1 = p1.Substring(0, p1.Length - 16);
                    SettingsButton.BackgroundImage = Image.FromFile(p1 + @"\Settings-icon.png");

                    // Average Duration
                    Label avrgd = new Label();
                    
                    avrgd.Text = "Средняя продолжительсность : "  + "0 м " + "0 с";
                    avrgd.AutoSize = true;
                    avrgd.Name = "avrgd" + Convert.ToString(tc.TabPages.Count - 1);
                    avrgd.Location = new Point((l.Width - avrgd.Width) / 2, DelSong.Location.Y + DelSong.Height + 10);

                    // добавляем компоненты в табконтрол
                    tc.TabPages[tc.TabPages.Count - 1].Controls.Add(l);
                    tc.TabPages[tc.TabPages.Count - 1].Controls.Add(AddSong);
                    tc.TabPages[tc.TabPages.Count - 1].Controls.Add(DelSong);
                    tc.TabPages[tc.TabPages.Count - 1].Controls.Add(SettingsButton);
                    tc.TabPages[tc.TabPages.Count - 1].Controls.Add(dcr);
                    tc.TabPages[tc.TabPages.Count - 1].Controls.Add(avrgd);
                }
            }
            catch { }
        }
        public void set_text(string s) => tc.TabPages.Add(s); // добавляем вкладку
        private string get_name(string s)
        {
            try
            {
                int i = s.LastIndexOf(@"\"); // ищем последний "\" в строке
                s = s.Substring(i + 1); // получаем отрезок строки с i + 1 до конца
                s = s.Substring(0, s.Length - 4); // получаем отрезок строки с начало до 4 элемента с конца
                return s;
            }
            catch
            {
                return string.Empty;
            }
        }
        public void rename(string name, int ind)
        {
            tc.TabPages[ind].Text = name;
        }
        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // открыть сайт
                System.Diagnostics.Process.Start("http://github.com/kokeyev");
            }
            catch { }
        }
    }
}
