using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class Form17 : Form
    {
        private static string connectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";

        public Form17()
        {
            InitializeComponent();
            FillMemberDetails();
        }

        private void FillMemberDetails()
        {
            string query = "SELECT * FROM Add_Members WHERE Password = @Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Password", Form12.LoggedInUserID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        txtName.Text = reader["Name"].ToString();
                        txtID.Text = reader["ID"].ToString();
                        txtPosition.Text = reader["Position"].ToString();
                        txtFaculty.Text = reader["Faculty"].ToString();
                        txtDepartment.Text = reader["Department"].ToString();
                        txtBatch.Text = reader["Batch"].ToString();
                        txtContact.Text = reader["Contact"].ToString();
                        txtMail.Text = reader["Mail"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSingOut_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
