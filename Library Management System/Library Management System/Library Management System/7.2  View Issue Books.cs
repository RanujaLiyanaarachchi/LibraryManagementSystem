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
    public partial class Form15 : Form
    {
        private static string connectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";

        public Form15()
        {
            InitializeComponent();
        }

        private void Form15_Load(object sender, EventArgs e)
        {
            string query = "SELECT ib.BookNumber, ab.ISBN, ab.BookName, ab.Author, ab.Publication, ib.Book_Issue_Date " +
                           "FROM I_and_R_Books ib " +
                           "INNER JOIN Add_Books ab ON ib.BookNumber = ab.BookNumber " +
                   "INNER JOIN Users u ON ib.ID = u.ID " +
                   "WHERE u.username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("Username", Form12.LoggedInUsername);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
