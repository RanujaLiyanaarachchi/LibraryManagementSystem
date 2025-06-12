using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class Form20 : Form
    {
        private string connectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";

        public Form20()
        {
            InitializeComponent();
            txtBookNumber.KeyPress += TxtBookNumber_KeyPress;
            panel2.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
   
            string memberId = Searchbox.Text.Trim();

            txtBookName.Clear();
            txtAuthor.Clear();
            txtPublication.Clear();
            txtISBN.Clear();
            txtIdate.Clear();
            txtIssuedTimes.Clear();
            txtReturnedTimes.Clear();
            txtBookNumber.Clear();

            if (string.IsNullOrWhiteSpace(memberId))
            {
                MessageBox.Show("Please enter a member ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string memberQuery = "SELECT * FROM Add_Members WHERE ID = @MemberId";
                    SqlCommand memberCommand = new SqlCommand(memberQuery, connection);
                    memberCommand.Parameters.AddWithValue("@MemberId", memberId);

                    SqlDataReader memberReader = memberCommand.ExecuteReader();

                    if (memberReader.Read())
                    {
                        txtUniID.Text = memberId;
                        txtMemberName.Text = memberReader["Name"].ToString();
                        panel2.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("No member found for the provided ID.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtUniID.Clear();
                        txtMemberName.Clear();
                        panel2.Visible = false;
                        Searchbox.Clear();

                    }

                    memberReader.Close();

                    string query = "SELECT IR.BookNumber, AB.ISBN, AB.BookName, AB.Author, AB.Publication, IR.Book_Issue_Date, IR.Book_Return_Date " +
                                   "FROM I_and_R_Books IR " +
                                   "INNER JOIN Add_Books AB ON IR.BookNumber = AB.BookNumber " +
                                   "WHERE IR.ID = @MemberId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MemberId", memberId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dataTable;
                    }
                    else
                    {
                        MessageBox.Show("No records found for the member ID.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter a valid ID Number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }


        private void TxtBookNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string bookNumber = txtBookNumber.Text.Trim();

                if (!string.IsNullOrWhiteSpace(bookNumber))
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            string query = "SELECT * FROM Add_Books WHERE BookNumber = @BookNumber";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@BookNumber", bookNumber);

                            SqlDataReader reader = command.ExecuteReader();

                            if (reader.Read())
                            {
                                txtBookName.Text = reader["BookName"].ToString();
                                txtAuthor.Text = reader["Author"].ToString();
                                txtPublication.Text = reader["Publication"].ToString();
                                txtISBN.Text = reader["ISBN"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No book found for the provided Book Number.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtBookName.Clear();
                                txtBookNumber.Clear();
                                txtAuthor.Clear();
                                txtPublication.Clear();
                                txtISBN.Clear();
                                txtIdate.Clear();
                                txtIssuedTimes.Clear();
                                txtReturnedTimes.Clear();
                            }

                            reader.Close();

                            // Query to fetch issued times by the specific member
                            string issuedQuery = "SELECT COUNT(*) FROM I_and_R_Books WHERE BookNumber = @BookNumber AND ID = @MemberId";
                            SqlCommand issuedCommand = new SqlCommand(issuedQuery, connection);
                            issuedCommand.Parameters.AddWithValue("@BookNumber", bookNumber);
                            issuedCommand.Parameters.AddWithValue("@MemberId", txtUniID.Text.Trim()); // Assuming txtUniID holds the member ID

                            int issuedTimes = (int)issuedCommand.ExecuteScalar();
                            txtIssuedTimes.Text = issuedTimes.ToString();

                            // Query to fetch returned times by the specific member
                            string returnedQuery = "SELECT COUNT(*) FROM I_and_R_Books WHERE BookNumber = @BookNumber AND ID = @MemberId AND Book_Return_Date IS NOT NULL";
                            SqlCommand returnedCommand = new SqlCommand(returnedQuery, connection);
                            returnedCommand.Parameters.AddWithValue("@BookNumber", bookNumber);
                            returnedCommand.Parameters.AddWithValue("@MemberId", txtUniID.Text.Trim()); // Assuming txtUniID holds the member ID

                            int returnedTimes = (int)returnedCommand.ExecuteScalar();
                            txtReturnedTimes.Text = returnedTimes.ToString();

                            // Fetch last issuance and return dates for the book
                            string dateQuery = "SELECT TOP 1 Book_Issue_Date, Book_Return_Date FROM I_and_R_Books WHERE BookNumber = @BookNumber AND ID = @MemberId ORDER BY Num DESC";
                            SqlCommand dateCommand = new SqlCommand(dateQuery, connection);
                            dateCommand.Parameters.AddWithValue("@BookNumber", bookNumber);
                            dateCommand.Parameters.AddWithValue("@MemberId", txtUniID.Text.Trim()); // Assuming txtUniID holds the member ID

                            SqlDataReader dateReader = dateCommand.ExecuteReader();

                            if (dateReader.Read())
                            {
                                txtIdate.Text = dateReader["Book_Issue_Date"].ToString();
                            }
                            else
                            {
                                txtIdate.Clear();
                                txtIssuedTimes.Clear();
                                txtReturnedTimes.Clear();
                                txtBookNumber.Clear();
                            }

                            dateReader.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please enter a valid Book Number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        txtBookNumber.Clear();
                    }
                }
                else
                {
                    txtBookName.Clear();
                    txtBookNumber.Clear();
                    txtAuthor.Clear();
                    txtPublication.Clear();
                    txtISBN.Clear();
                    txtIdate.Clear();
                    txtIssuedTimes.Clear();
                    txtReturnedTimes.Clear();

                }
            }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBookName.Clear();
            txtAuthor.Clear();
            txtPublication.Clear();
            txtISBN.Clear();
            txtIdate.Clear();
            txtIssuedTimes.Clear();
            txtReturnedTimes.Clear();
            txtBookNumber.Clear();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            Searchbox.Clear();
            txtUniID.Clear();
            txtMemberName.Clear();
            txtBookNumber.Clear();
            txtBookName.Clear();
            txtAuthor.Clear();
            txtPublication.Clear();
            txtISBN.Clear();
            txtIdate.Clear();
            txtIssuedTimes.Clear();
            txtReturnedTimes.Clear();
            dataGridView1.DataSource = null;
        }

        private void txtTimes_TextChanged(object sender, EventArgs e)
        {

        }

    }
}