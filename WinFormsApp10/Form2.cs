using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp10
{
    public partial class Form2 : Form
    {
        Report rep = new Report();
        public Form2()
        {
            InitializeComponent();
            rep.Start(listBox1,listBox2);
            CopyFix.SetReport(rep);
            timer1.Interval = 10000;
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rep.Save();
            this.Close();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            rep.ShowInfo();
        }
    }
}
