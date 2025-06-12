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
using System.Xml.Linq;

namespace Library_Management_System
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            txtNumber.Clear();
            txtISBN.Clear();
            txtName.Clear();
            txtAuthor.Clear();
            txtPublication.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtNumber.Text != "" && txtISBN.Text != "" && txtName.Text != "" && txtAuthor.Text != "" && txtPublication.Text != "" && txtPrice.Text != "" && txtQuantity.Text != "")
            {
                try
                {
                    string BookNumber = txtNumber.Text;
                    string ISBN = txtISBN.Text;
                    string BookName = txtName.Text;
                    string Author = txtAuthor.Text;
                    string Publication = txtPublication.Text;
                    string Date = dateTimePicker1.Text;
                    string Price = txtPrice.Text;
                    Int64 Quantity = Int64.Parse(txtQuantity.Text);

                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;

                    con.Open();

                    cmd.CommandText = "SELECT COUNT(*) FROM Add_Books WHERE BookNumber = @BookNumber";
                    cmd.Parameters.AddWithValue("@BookNumber", BookNumber);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Sorry,This book number already exists. Try a new one", " ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        con.Close();
                        return;
                    }

                    cmd.CommandText = "insert into Add_Books (BookNumber,ISBN,BookName,Author,Publication,Date,Price,Quantity) values (@BookNumber, @ISBN, @BookName, @Author, @Publication, @Date, @Price, @Quantity)";
                    cmd.Parameters.AddWithValue("@ISBN", ISBN);
                    cmd.Parameters.AddWithValue("@BookName", BookName);
                    cmd.Parameters.AddWithValue("@Author", Author);
                    cmd.Parameters.AddWithValue("@Publication", Publication);
                    cmd.Parameters.AddWithValue("@Date", Date);
                    cmd.Parameters.AddWithValue("@Price", Price);
                    cmd.Parameters.AddWithValue("@Quantity", Quantity);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Congratulations, you have entered data successfully.", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtNumber.Clear();
                    txtISBN.Clear();
                    txtName.Clear();
                    txtAuthor.Clear();
                    txtPublication.Clear();
                    txtPrice.Clear();
                    txtQuantity.Clear();
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter a valid number for Quantity.", " ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (SqlException)
                {
                    MessageBox.Show("Please enter a valid number for Book Number.", " ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Blank spaces are not allowed, please fill in all fields.", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
