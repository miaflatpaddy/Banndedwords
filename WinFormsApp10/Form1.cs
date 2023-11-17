namespace WinFormsApp10
{
    
    public partial class Form1 : Form
    {
        Search search = new Search();
        Stopper stop = new Stopper();
        Form2 form;

        public Form1()
        {

            InitializeComponent();
            textBox2.Text = "";
            DriveInfo[] Di = DriveInfo.GetDrives();
            comboBox1.Items.Add("None");
            foreach (var item in Di)
            {
                if (item.IsReady)
                {
                    comboBox1.Items.Add(item.Name);
                }
            }
            comboBox1.SelectedIndex = 0;
            stop.ew = new EventWaitHandle(false, EventResetMode.AutoReset);
            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("Необходимо выбрать место хранения найденных файлов.");
            }
            else if (BannedWords.strnum == 0)
            {
                MessageBox.Show("Необходимо добавить в список ходябы одно запрещённое слово.");
            }
            else
            {

                form = new Form2();
                form.Show();
                if (comboBox1.SelectedIndex == 0)
                {
                    try
                    {
                        Task.Run(() =>
                        search.SearchF(progressBar1, stop), search.token);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    string tmp = comboBox1.SelectedItem.ToString();
                    try
                    {

                        Task.Run(() =>
                        search.SearchF(progressBar1, stop, tmp), search.token);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                timer1.Start();
            }
        }

        private void Timer1_Tick(object? sender, EventArgs e)
        {
            progressBar1.Value = search.progress;
            if(progressBar1.Value == progressBar1.Maximum)
            {
                timer1.Stop();
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            textBox2.Text = fbd.SelectedPath;
            CopyFix.DestinationSet(textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();  
            ofd.ShowDialog();
            BannedWords.listAddres = ofd.FileName;
            BannedWords.Read();
            listBox1.Items.Clear();
            foreach (var item in BannedWords.BannedList)
            {
                listBox1.Items.Add(item);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BannedWords.Save();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            if (listBox1.SelectedIndex != -1)
            {
                if (listBox1.SelectedIndices.Count > 1)
                {
                    foreach (var item in listBox1.SelectedIndices)
                    {
                        BannedWords.Remove(listBox1.SelectedIndex);
                    }
                    listBox1.Items.Clear();
                    foreach (var item in BannedWords.BannedList)
                    {
                        listBox1.Items.Add(item);
                    }
                }
                else
                {
                    BannedWords.Remove(listBox1.SelectedIndex);
                    listBox1.Items.Clear();
                    foreach (var item in BannedWords.BannedList)
                    {
                        listBox1.Items.Add(item);
                    }
                }

            }
            else
            {
                MessageBox.Show("Ниодин элемент не выбран.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BannedWords.Add(textBox1.Text);
            listBox1.Items.Clear();
            foreach (var item in BannedWords.BannedList)
            {
                listBox1.Items.Add(item);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (stop.Pause)
            {
                stop.Pause = false;
                button7.Text = "Пауза";
                stop.ew.Set();
            }
            else if (!stop.Pause)
            {
                stop.Pause = true;
                button7.Text = "Возобновить";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Search.cTS.Cancel();
            timer1.Stop();
            Search.cTS.Dispose();
            Search.cTS = new CancellationTokenSource();
            progressBar1.Value = 0;
            search = new Search();
            form.Dispose();
        }


        //private void button9_Click(object sender, EventArgs e)
        //{
        //}
    }
}