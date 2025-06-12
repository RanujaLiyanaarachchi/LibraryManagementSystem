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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Library_Management_System
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            panel2.Visible = false;

            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandText = "select * from Add_Members";
            SqlDataAdapter DA = new SqlDataAdapter(cmd);
            DataSet DS = new DataSet();
            DA.Fill(DS);

            dataGridView1.DataSource = DS.Tables[0];
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

                cmd.CommandText = "select * from Add_Members where ID like '"+Searchbox.Text+"%'";
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                DA.Fill(DS);

                dataGridView1.DataSource = DS.Tables[0];
            }

            else
            {
                Image image = Image.FromFile(@"C:\Users\ranuj\OneDrive\Desktop\Projects\Computing Project\Pictures\Members Search.gif");
                pictureBox1.Image = image;

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "select * from Add_Members where ID like '" + Searchbox.Text + "%'";
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                DA.Fill(DS);

                dataGridView1.DataSource = DS.Tables[0];
            }
 
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Searchbox.Clear();
            Form6_Load(this, null);
        }

        int ID;


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (row.Cells["ID"].Value != null && int.TryParse(row.Cells["ID"].Value.ToString(), out ID))
                {
                    panel2.Visible = true;

                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;

                    cmd.CommandText = "select * from Add_Members where ID = @ID";
                    cmd.Parameters.AddWithValue("@ID", ID);
                    SqlDataAdapter DA = new SqlDataAdapter(cmd);
                    DataSet DS = new DataSet();
                    DA.Fill(DS);

                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        txtName.Text = DS.Tables[0].Rows[0][0].ToString();
                        txtID.Text = DS.Tables[0].Rows[0][1].ToString();
                        txtPosition.Text = DS.Tables[0].Rows[0][2].ToString();
                        txtFaculty.Text = DS.Tables[0].Rows[0][3].ToString();
                        txtDepartment.Text = DS.Tables[0].Rows[0][4].ToString();
                        txtBatch.Text = DS.Tables[0].Rows[0][5].ToString();
                        txtContact.Text = DS.Tables[0].Rows[0][6].ToString();
                        txtMail.Text = DS.Tables[0].Rows[0][7].ToString();
                        txtUser.Text = DS.Tables[0].Rows[0][8].ToString();
                        txtpass.Text = DS.Tables[0].Rows[0][9].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Sorry, the member you were looking for was not found.");
                    }
                }
                else
                {
                    MessageBox.Show("You have selected an invalid cell value or an empty cell.");
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string Name = txtName.Text;
            string ID = txtID.Text;
            string Position = txtPosition.Text;
            string Faculty = txtFaculty.Text;
            string Department = txtDepartment.Text;
            string Batch = txtBatch.Text;
            string Contact = txtContact.Text;
            string Mail = txtMail.Text;
            string Username = txtUser.Text;
            string Password = txtpass.Text;

            if (ID != dataGridView1.CurrentRow.Cells["ID"].Value.ToString())
            {
                MessageBox.Show("Sorry Cannot change the university ID card number.", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    cmd.CommandText = "UPDATE Add_Members SET Name = @Name, Position = @Position, Faculty = @Faculty, Department = @Department, Batch = @Batch, Contact = @Contact, Mail = @Mail, Username = @Username, Password = @Password WHERE ID = @ID";

                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Position", Position);
                    cmd.Parameters.AddWithValue("@Faculty", Faculty);
                    cmd.Parameters.AddWithValue("@Department", Department);
                    cmd.Parameters.AddWithValue("@Batch", Batch);
                    cmd.Parameters.AddWithValue("@Contact", Contact);
                    cmd.Parameters.AddWithValue("@Mail", Mail);
                    cmd.Parameters.AddWithValue("@Username", Username);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Parameters.AddWithValue("@ID", ID);



                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        cmd.CommandText = "UPDATE Users SET username = @Username, password = @Password WHERE ID = @ID";
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Membership information has been successfully updated.");
                    }
                    else
                    {
                        MessageBox.Show("Sorry, No member found with the provided university ID.");
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

                Form6_Load(this, null);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string memberID = txtID.Text;

            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                // Check if there are any issued books associated with the member
                cmd.CommandText = "SELECT COUNT(*) FROM I_and_R_Books WHERE ID = @MemberID AND Book_Return_Date IS NULL";
                cmd.Parameters.AddWithValue("@MemberID", memberID);
                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    // If there are issued books that have not been returned, prompt the user to return them first
                    MessageBox.Show("Please return all issued books before deleting the member.");
                }
                else
                {
                    // If there are no issued books pending return, proceed with deleting the member and related data from all tables
                    cmd.CommandText = "DELETE FROM I_and_R_Books WHERE ID = @ID;" +
                                       "DELETE FROM Users WHERE ID = @ID;" +
                                       "DELETE FROM Add_Members WHERE ID = @ID;";
                                     
                    cmd.Parameters.Clear(); // Clear parameters before reusing the command
                    cmd.Parameters.AddWithValue("@ID", memberID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Member and related data deleted successfully.");
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
            Form6_Load(this, null);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
