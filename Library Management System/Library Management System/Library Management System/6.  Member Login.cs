using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class Form13 : Form
    {

        public static string LoggedInPassword { get; private set; }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            LoggedInPassword = txtPass.Text;
            this.Close();
        }

        private string connectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";

        public Form13()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text.Trim();
            string password = txtPass.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Form7 fm7 = new Form7();
                fm7.Show();
                return;
            }

            string query = "SELECT Password FROM Add_Members WHERE Username = @Username COLLATE Latin1_General_CS_AS";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string storedPassword = reader["Password"].ToString();

                        if (password.Equals(storedPassword, StringComparison.Ordinal))
                        {
                            Form12 fm12 = new Form12(username, password);
                            fm12.Show();
                            this.Hide();
                        }
                        else
                        {
                            Form7 fm7 = new Form7();
                            fm7.Show();

                        }
                    }
                    else
                    {
                        Form7 fm7 = new Form7();
                        fm7.Show();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form2 fm2 = new Form2();
            fm2.Show();
            this.Hide();
        }
    }
}

