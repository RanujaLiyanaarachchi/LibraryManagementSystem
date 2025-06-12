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
    public partial class Form7 : Form
    {
        private Form2 form2;
        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // Close or hide all forms except the current one
                if (form != this)
                {
                    form.Hide(); // or form.Hide() if you want to hide instead of close
                }
                this.Hide();
            }
        }

        private void Form7_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();

            foreach (Form form in Application.OpenForms)
            {
                if (form != this)
                {
                    form.Hide();
                }
            }

            // Open Form2
            if (form2 == null || form2.IsDisposed)
            {
                form2 = new Form2();
            }
            form2.Show();
        }
    }
}
