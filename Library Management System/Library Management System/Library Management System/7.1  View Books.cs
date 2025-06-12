using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class Form14 : Form
    {
        string connectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";

        public Form14()
        {
            InitializeComponent();
            DisplayBookDetails();
        }

        private void DisplayBookDetails()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ISBN, BookName AS 'Book Name', Author, Publication, Price, Quantity AS 'Available Quantity' FROM Add_Books";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable bookTable = new DataTable();
                        adapter.Fill(bookTable);
                        dataGridView1.DataSource = bookTable;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Searchbox_TextChanged(object sender, EventArgs e)
        {
            if (Searchbox.Text != "")
            {
                Image image = Image.FromFile(@"C:\Users\ranuj\OneDrive\Desktop\Projects\Computing Project\Pictures\Member Search.gif");
                pictureBox1.Image = image;


                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "select * from Add_Books where BookName like '" + Searchbox.Text + "%'";
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                DA.Fill(DS);

                dataGridView1.DataSource = DS.Tables[0];
            }

            else
            {
                Image image = Image.FromFile(@"C:\Users\ranuj\OneDrive\Desktop\Projects\Computing Project\Pictures\view books.gif");
                pictureBox1.Image = image;

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "select * from Add_Books where BookName like '" + Searchbox.Text + "%'";
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                DA.Fill(DS);

                dataGridView1.DataSource = DS.Tables[0];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Searchbox.Clear();
            DisplayBookDetails();
        }

    }
}
