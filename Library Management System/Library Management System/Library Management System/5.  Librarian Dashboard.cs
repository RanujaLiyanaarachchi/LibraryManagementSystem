using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 fm5 = new Form5();
            fm5.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form6 fm6 = new Form6();
            fm6.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form8 fm8 = new Form8();
            fm8.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form9 fm9 = new Form9();
            fm9.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form10 fm10 = new Form10();
            fm10.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form11 fm11 = new Form11();
            fm11.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form18 fm18 = new Form18();
            fm18.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form20 fm20 = new Form20();
            fm20.Show();
        }
    }
}
