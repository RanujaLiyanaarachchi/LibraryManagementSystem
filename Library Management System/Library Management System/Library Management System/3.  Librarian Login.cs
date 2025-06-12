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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 fm2 = new Form2();
            fm2.Show();
            this.Hide();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-9LH3V47\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;");

            string query = "SELECT COUNT(*) FROM Librarian_Login WHERE username= @username COLLATE Latin1_General_CS_AS AND password= @password COLLATE Latin1_General_CS_AS";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", txtUser.Text);
            cmd.Parameters.AddWithValue("@password", txtPass.Text);
            con.Open();
            int count = (int)cmd.ExecuteScalar();
            con.Close();

            if (count > 0)
            {
                Form4 fm4 = new Form4();
                fm4.Show();
                this.Hide();
            }
            else
            {
                Form7 form7 = new Form7();
                form7.Show();
            }




        }
    }
}
