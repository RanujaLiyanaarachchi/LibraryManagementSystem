using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class Form11 : Form
    {
        public Form11()
        {
            InitializeComponent();
        }

        private void Form11_Load(object sender, EventArgs e)
        {
            panel2.Visible = false;
            Searchbox.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(Searchbox.Text, out int memberId))
            {
                MessageBox.Show("Please enter a valid ID Number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;"))
            {
                con.Open();
                string query = "SELECT ID, Name, Position, faculty, Department, batch, BookNumber, BookName, Book_Issue_Date, Book_Return_Date FROM I_and_R_Books WHERE ID = @ID AND Book_Return_Date IS NULL";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", Searchbox.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dataTable;
                }
                else
                {
                    MessageBox.Show("Invalid ID Number Or No Book Issued In This ID Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (dataGridView1.Rows[e.RowIndex].Selected)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (row.Cells["BookNumber"].Value != null && int.TryParse(row.Cells["BookNumber"].Value.ToString(), out int bookNumber))
                {
                    panel2.Visible = true;

                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;

                    cmd.CommandText = "select * from Add_Books where BookNumber = @BookNumber";
                    cmd.Parameters.AddWithValue("@BookNumber", bookNumber);
                    SqlDataAdapter DA = new SqlDataAdapter(cmd);
                    DataSet DS = new DataSet();
                    DA.Fill(DS);

                    if (DS.Tables[0].Rows.Count > 0)
                    {

                        txtNumber.Text = bookNumber.ToString();
                        txtISBN.Text = GetBookDetail(bookNumber.ToString(), "ISBN");
                        txtName.Text = GetBookDetail(bookNumber.ToString(), "BookName");
                        txtAuthor.Text = GetBookDetail(bookNumber.ToString(), "Author");
                        txtPublication.Text = GetBookDetail(bookNumber.ToString(), "Publication");
                        txtIssue.Text = row.Cells["Book_Issue_Date"].Value.ToString();
                        txtPrice.Text = GetBookDetail(bookNumber.ToString(), "Price");
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
            else
            {
                MessageBox.Show("Please select the entire row to view book details.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GetBookDetail(string bookNumber, string column)
        {
            string detail = "";
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;"))
            {
                con.Open();
                string query = $"SELECT {column} FROM Add_Books WHERE BookNumber = @BookNumber";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BookNumber", bookNumber);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    detail = reader[column].ToString();
                }
                reader.Close();
            }
            return detail;
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                if (!string.IsNullOrEmpty(selectedRow.Cells["BookNumber"].Value?.ToString()))
                {
                    string bookNumber = selectedRow.Cells["BookNumber"].Value.ToString();

                    using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;"))
                    {
                        con.Open();
                        string updateQuery = "UPDATE I_and_R_Books SET Book_Return_Date = @ReturnDate WHERE ID = @ID AND BookNumber = @BookNumber AND Book_Return_Date IS NULL";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                        updateCmd.Parameters.AddWithValue("@ReturnDate", dateTimePicker2.Value);
                        updateCmd.Parameters.AddWithValue("@ID", Searchbox.Text);
                        updateCmd.Parameters.AddWithValue("@BookNumber", bookNumber);

                        int rowsAffected = updateCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Book returned successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            panel2.Visible = false;

                            UpdateBookQuantity(bookNumber);

                            RefreshDataGridView();
                        }
                        else
                        {
                            MessageBox.Show("Failed to return the book.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a book to return.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to return a book.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateBookQuantity(string bookNumber)
        {
            int currentQuantity = 0;
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;"))
            {
                con.Open();
                string query = "SELECT Quantity FROM Add_Books WHERE BookNumber = @BookNumber";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BookNumber", bookNumber);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    currentQuantity = Convert.ToInt32(reader["Quantity"]);
                }
                reader.Close();
            }

            int updatedQuantity = currentQuantity + 1;
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;"))
            {
                con.Open();
                string updateQuery = "UPDATE Add_Books SET Quantity = @Quantity WHERE BookNumber = @BookNumber";
                SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@Quantity", updatedQuantity);
                updateCmd.Parameters.AddWithValue("@BookNumber", bookNumber);
                updateCmd.ExecuteNonQuery();
            }
        }

        private void RefreshDataGridView()
        {
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;"))
            {
                con.Open();
                string query = "SELECT ID, Name, Position, faculty, Department, batch, BookNumber, BookName, Book_Issue_Date, Book_Return_Date FROM I_and_R_Books WHERE ID = @ID AND Book_Return_Date IS NULL";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", Searchbox.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dataTable;
                }
                else
                {
                    dataGridView1.DataSource = null;
                    Searchbox.Clear();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Searchbox.Clear();
            dataGridView1.DataSource = null;
            Form11_Load(this, null);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
