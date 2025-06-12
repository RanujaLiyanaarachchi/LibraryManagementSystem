using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            txtID.TextChanged += txtID_TextChanged;
            txtName.TextChanged += txtName_TextChanged;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtID.Clear();
            txtPosition.Clear();
            txtFaculty.Clear();
            txtDepartment.Clear();
            txtBatch.Clear();
            txtContact.Clear();
            txtMail.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtID.Text != "" && txtPosition.Text != "" && txtFaculty.Text != "" && txtDepartment.Text != "" && txtBatch.Text != "" && txtContact.Text != "" && txtMail.Text != "" && txtUsername.Text != "" && txtPassword.Text != "")
            {
                try
                {
                    string Name = txtName.Text;
                    string ID = txtID.Text;
                    string Position = txtPosition.Text;
                    string Faculty = txtFaculty.Text;
                    string Department = txtDepartment.Text;
                    string Batch = txtBatch.Text;
                    string Contact = txtContact.Text;
                    string Mail = txtMail.Text;
                    string Username = txtUsername.Text;
                    string Password = txtPassword.Text;

                    // Validate email
                    if (!IsValidEmail(txtMail.Text))
                    {
                        MessageBox.Show("Invalid email address, please enter a valid email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Validate integers
                    if (!IsValidInteger(txtID.Text) || !IsValidInteger(txtBatch.Text) || !IsValidInteger(txtContact.Text))
                    {
                        MessageBox.Show("Invalid data. Check ID, Batch, and Contact should be correct.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Insert data into database
                    using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;"))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("INSERT INTO Add_Members (Name, ID, Position, Faculty, Department, Batch, Contact, Mail, Username, Password) VALUES (@Name, @ID, @Position, @Faculty, @Department, @Batch, @Contact, @Mail, @Username, @Password)", con);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@Position", Position);
                        cmd.Parameters.AddWithValue("@Faculty", Faculty);
                        cmd.Parameters.AddWithValue("@Department", Department);
                        cmd.Parameters.AddWithValue("@Batch", Batch);
                        cmd.Parameters.AddWithValue("@Contact", Contact);
                        cmd.Parameters.AddWithValue("@Mail", Mail);
                        cmd.Parameters.AddWithValue("@Username", Username);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Congratulations, you have entered data successfully.", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        SqlCommand cmdUsers = new SqlCommand("INSERT INTO Users (ID, username, password) VALUES (@ID, @Username, @Password)", con);
                        cmdUsers.Parameters.AddWithValue("@ID", ID);
                        cmdUsers.Parameters.AddWithValue("@Username", Username);
                        cmdUsers.Parameters.AddWithValue("@Password", Password);
                        cmdUsers.ExecuteNonQuery();

                        txtName.Clear();
                        txtID.Clear();
                        txtPosition.Clear();
                        txtFaculty.Clear();
                        txtDepartment.Clear();
                        txtBatch.Clear();
                        txtContact.Clear();
                        txtMail.Clear();
                        txtUsername.Clear();
                        txtPassword.Clear();
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("The member with the provided ID number already exists.", " ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Blank spaces are not allowed, please fill in all fields.", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };


        }

        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsValidInteger(string value)
        {
            return int.TryParse(value, out _) || double.TryParse(value, out _);
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {
                txtPassword.Text = txtID.Text;        
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string fullName = txtName.Text.Trim();
            string firstName = "";

            string[] nameParts = fullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length > 0)
            {
                firstName = nameParts[0];
            }

            txtUsername.Text = firstName.ToLower();
        }
    }
}
