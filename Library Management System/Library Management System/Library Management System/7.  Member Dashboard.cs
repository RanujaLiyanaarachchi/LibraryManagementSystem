using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class Form12 : Form
    {
        private string username;
        private string password;
        private object txtPass;
        private static string connectionString;
        private object reader;

        public static string LoggedInUsername { get; private set; }
        public static string LoggedInUserID { get; private set; }
        public static string LoggedInUserName { get; internal set; }

        public Form12(string username, string password)
        {
            InitializeComponent();
            LoggedInUsername = username;
            LoggedInUserID = password;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form14 fm14 = new Form14();
            fm14.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form15 fm15 = new Form15();
            fm15.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form16 fm16 = new Form16();
            fm16.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form17 fm17 = new Form17();
            fm17.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form19 fm19 = new Form19();
            fm19.Show();
        }
    }
}
