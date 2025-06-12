using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            panel2.Visible = false;

            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandText = "select * from Add_Books";
            SqlDataAdapter DA = new SqlDataAdapter(cmd);
            DataSet DS = new DataSet();
            DA.Fill(DS);

            dataGridView1.DataSource = DS.Tables[0];
        }

        int BookNumber;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (row.Cells["BookNumber"].Value != null && int.TryParse(row.Cells["BookNumber"].Value.ToString(), out BookNumber))
                {
                    panel2.Visible = true;

                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;

                    cmd.CommandText = "select * from Add_Books where BookNumber = @BookNumber";
                    cmd.Parameters.AddWithValue("@BookNumber", BookNumber);
                    SqlDataAdapter DA = new SqlDataAdapter(cmd);
                    DataSet DS = new DataSet();
                    DA.Fill(DS);

                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        txtNumber.Text = DS.Tables[0].Rows[0][0].ToString();
                        txtISBN.Text = DS.Tables[0].Rows[0][1].ToString();
                        txtName.Text = DS.Tables[0].Rows[0][2].ToString();
                        txtAuthor.Text = DS.Tables[0].Rows[0][3].ToString();
                        txtPublication.Text = DS.Tables[0].Rows[0][4].ToString();
                        txtDate.Text = DS.Tables[0].Rows[0][5].ToString();
                        txtPrice.Text = DS.Tables[0].Rows[0][6].ToString();
                        txtQuantity.Text = DS.Tables[0].Rows[0][7].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Sorry, the Book you were looking for was not found.");
                    }
                }
                else
                {
                    MessageBox.Show("You have selected an invalid cell value or an empty cell.");
                }
            }
        }
    
        private void Searchbox_TextChanged(object sender, EventArgs e)
        {
            if (Searchbox.Text != "")
            {
                Image image = Image.FromFile(@"C:\Users\ranuj\OneDrive\Desktop\Projects\Computing Project\Pictures\Member Search.gif");
                pictureBox1.Image = image;


                panel2.Visible = false;

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
            Form9_Load(this, null);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string BookNumber = txtNumber.Text;
            string ISBN = txtISBN.Text;
            string BookName = txtName.Text;
            string Author = txtAuthor.Text;
            string Publication = txtPublication.Text;
            string Date = dateTimePicker1.Text;
            string Price = txtPrice.Text;
            string Quantity = txtQuantity.Text;

            if (BookNumber != dataGridView1.CurrentRow.Cells["BookNumber"].Value.ToString())
            {
                MessageBox.Show("Sorry, Cannot change the Book number.", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("The data you entered will be updated. please verify?", " ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";

                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;

                    cmd.CommandText = "UPDATE Add_Books SET ISBN = @ISBN, BookName = @BookName, Author = @Author, Publication = @Publication, Date = @Date, Price = @Price, Quantity = @Quantity WHERE BookNumber = @BookNumber";

                    cmd.Parameters.AddWithValue("@BookNumber", BookNumber);
                    cmd.Parameters.AddWithValue("@ISBN", ISBN);
                    cmd.Parameters.AddWithValue("@BookName", BookName);
                    cmd.Parameters.AddWithValue("@Author", Author);
                    cmd.Parameters.AddWithValue("@Publication", Publication);
                    cmd.Parameters.AddWithValue("@Date", Date);
                    cmd.Parameters.AddWithValue("@Price", Price);
                    cmd.Parameters.AddWithValue("@Quantity", Quantity);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Book information has been successfully updated.");
                    }
                    else
                    {
                        MessageBox.Show("Sorry, No Books found with the provided Book Name.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }

                Form9_Load(this, null);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("All data here will be permanently deleted. Do you confirm?", " ", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                string BookNumber = txtNumber.Text;

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";

                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;

                    // Delete related records in I_and_R_Books table first
                    cmd.CommandText = "DELETE FROM I_and_R_Books WHERE BookNumber = @BookNumber";
                    cmd.Parameters.AddWithValue("@BookNumber", BookNumber);
                    cmd.ExecuteNonQuery();

                    // Proceed with deleting the book from Add_Books table
                    cmd.CommandText = "DELETE FROM Add_Books WHERE BookNumber = @BookNumber";
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Congratulations, you have successfully deleted the Book record.");
                        // Additional actions after successful deletion
                    }
                    else
                    {
                        MessageBox.Show("Sorry, No Books found with the provided Book Number.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
                Form9_Load(this, null);
            }
        }
    }
}
