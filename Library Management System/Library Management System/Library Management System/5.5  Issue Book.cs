using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class Form10 : Form
    {
        private SqlConnection con;

        public Form10()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            con = new SqlConnection();
            con.ConnectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";
            con.Open();

            txtBookNumber.TextChanged += txtBookNumber_TextChanged;
            button1.Click += button1_Click;
        }

        private void txtBookNumber_TextChanged(object sender, EventArgs e)
        {
            string query = "SELECT BookName FROM Add_Books WHERE BookNumber = @BookNumber";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@BookNumber", txtBookNumber.Text);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                txtBookName.Text = reader["BookName"].ToString();
            }
            else
            {
                txtBookName.Clear();
            }
            reader.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (!int.TryParse(Searchbox.Text, out int memberId))
            {
                MessageBox.Show("Please enter a valid ID Number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "SELECT * FROM Add_Members WHERE ID = @ID";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ID", Searchbox.Text);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                txtName.Text = reader["Name"].ToString();
                txtPosition.Text = reader["Position"].ToString();
                txtFaculty.Text = reader["Faculty"].ToString();
                txtDepartment.Text = reader["Department"].ToString();
                txtBatch.Text = reader["Batch"].ToString();
            }
            else
            {
                txtName.Clear();
                txtPosition.Clear();
                txtFaculty.Clear();
                txtDepartment.Clear();
                txtBatch.Clear();

                MessageBox.Show("No member found with the entered ID Number.", " ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            reader.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtPosition.Clear();
            txtFaculty.Clear();
            txtDepartment.Clear();
            txtBatch.Clear();
            txtBookNumber.Clear();
            txtBookName.Clear();
            Searchbox.Clear();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPosition.Text) || string.IsNullOrWhiteSpace(txtFaculty.Text) ||
                string.IsNullOrWhiteSpace(txtDepartment.Text) || string.IsNullOrWhiteSpace(txtBatch.Text) || string.IsNullOrWhiteSpace(txtBookNumber.Text) ||
                string.IsNullOrWhiteSpace(txtBookName.Text))
            {
                MessageBox.Show("Please fill all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string countQuery = "SELECT COUNT(*) FROM I_and_R_Books WHERE ID = @ID AND Book_Return_Date IS NULL";
            SqlCommand countCmd = new SqlCommand(countQuery, con);
            countCmd.Parameters.AddWithValue("@ID", Searchbox.Text);
            int issuedBooksCount = (int)countCmd.ExecuteScalar();

            if (issuedBooksCount >= 2)
            {
                MessageBox.Show("This member has already issued two books. Please return a book before issuing another one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string checkBookQuery = "SELECT Quantity FROM Add_Books WHERE BookNumber = @BookNumber";
            SqlCommand checkBookCmd = new SqlCommand(checkBookQuery, con);
            checkBookCmd.Parameters.AddWithValue("@BookNumber", txtBookNumber.Text);
            object result = checkBookCmd.ExecuteScalar();
            int availableQuantity = 0;

            if (result != null && result != DBNull.Value)
            {
                if (int.TryParse(result.ToString(), out int quantity))
                {
                    availableQuantity = quantity;
                }
                else
                {
                    MessageBox.Show("Error: Unable to convert quantity to integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("This book is out of stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (availableQuantity <= 0)
            {
                MessageBox.Show("This book is out of stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string checkIssuedQuery = "SELECT COUNT(*) FROM I_and_R_Books WHERE ID = @ID AND BookNumber = @BookNumber AND Book_Return_Date IS NULL";
            SqlCommand checkIssuedCmd = new SqlCommand(checkIssuedQuery, con);
            checkIssuedCmd.Parameters.AddWithValue("@ID", Searchbox.Text);
            checkIssuedCmd.Parameters.AddWithValue("@BookNumber", txtBookNumber.Text);
            int existingIssuedCount = (int)checkIssuedCmd.ExecuteScalar();

            if (existingIssuedCount > 0)
            {
                MessageBox.Show("This book is already issued to the member.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string insertQuery = "INSERT INTO I_and_R_Books (ID, Name, Position, Faculty, Department, Batch, BookNumber, BookName, Book_Issue_Date, Book_Return_Date) " +
                                 "VALUES (@ID, @Name, @Position, @Faculty, @Department, @Batch, @BookNumber, @BookName, @Book_Issue_Date, NULL)";
            SqlCommand insertCmd = new SqlCommand(insertQuery, con);
            insertCmd.Parameters.AddWithValue("@ID", Searchbox.Text);
            insertCmd.Parameters.AddWithValue("@Name", txtName.Text);
            insertCmd.Parameters.AddWithValue("@Position", txtPosition.Text);
            insertCmd.Parameters.AddWithValue("@Faculty", txtFaculty.Text);
            insertCmd.Parameters.AddWithValue("@Department", txtDepartment.Text);
            insertCmd.Parameters.AddWithValue("@Batch", txtBatch.Text);
            insertCmd.Parameters.AddWithValue("@BookNumber", txtBookNumber.Text);
            insertCmd.Parameters.AddWithValue("@BookName", txtBookName.Text);
            insertCmd.Parameters.AddWithValue("@Book_Issue_Date", DateTime.Now.ToString());

            string updateQuery = "UPDATE Add_Books SET Quantity = Quantity - 1 WHERE BookNumber = @BookNumber";
            SqlCommand updateCmd = new SqlCommand(updateQuery, con);
            updateCmd.Parameters.AddWithValue("@BookNumber", txtBookNumber.Text);

            SqlTransaction transaction = null;

            try
            {
                transaction = con.BeginTransaction();

                insertCmd.Transaction = transaction;
                int rowsAffected = insertCmd.ExecuteNonQuery();

                updateCmd.Transaction = transaction;
                int updatedRows = updateCmd.ExecuteNonQuery();

                if (rowsAffected > 0 && updatedRows > 0)
                {
                    transaction.Commit();
                    MessageBox.Show("Book issued successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    transaction.Rollback();
                    MessageBox.Show("Failed to issue book.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
