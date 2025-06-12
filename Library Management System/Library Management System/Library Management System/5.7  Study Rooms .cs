using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class Form18 : Form
    {
        private string connectionString = "Data Source=DESKTOP-9LH3V47\\SQLEXPRESS01;Initial Catalog=Library_Management_System;Integrated Security=True;";
        private int selectedRoomId = -1;
        public Form18()
        {
            InitializeComponent();
            LoadRoomData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string roomNumber = txtRoomNumber.Text;
            string floor = comboBox1.SelectedItem?.ToString();
            string roomSpace = txtRoomSpace.Text;

            if (string.IsNullOrWhiteSpace(roomNumber) || string.IsNullOrWhiteSpace(floor) || string.IsNullOrWhiteSpace(roomSpace))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string checkQuery = "SELECT COUNT(*) FROM Room WHERE RoomNumber = @RoomNumber";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@RoomNumber", roomNumber);
                    int roomCount = (int)checkCommand.ExecuteScalar();

                    if (roomCount > 0)
                    {
                        MessageBox.Show("Room number already exists. Please enter a different room number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string insertQuery = "INSERT INTO Room (RoomNumber, Floor, RoomSpace) VALUES (@RoomNumber, @Floor, @RoomSpace)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@RoomNumber", roomNumber);
                    insertCommand.Parameters.AddWithValue("@Floor", floor);
                    insertCommand.Parameters.AddWithValue("@RoomSpace", roomSpace);
                    insertCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Room details saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadRoomData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRoomData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Room";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading room data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtRoomNumber.Text = "";
            txtRoomSpace.Text = "";
            comboBox1.SelectedIndex = -1;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedRoomId = Convert.ToInt32(row.Cells["RoomNumber"].Value);

                comboBox3.Text = row.Cells["Available"].Value.ToString();
                comboBox2.Text = row.Cells["Floor"].Value.ToString();
                txtRoomNumber2.Text = row.Cells["RoomNumber"].Value.ToString();
                txtRoomSpace2.Text = row.Cells["RoomSpace"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedRoomId == -1)
            {
                MessageBox.Show("Please select a room to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string available = comboBox3.Text;
            string floor = comboBox2.Text;
            string roomNumber = txtRoomNumber2.Text;
            string roomSpace = txtRoomSpace2.Text;

            if (string.IsNullOrWhiteSpace(available) || string.IsNullOrWhiteSpace(floor) || string.IsNullOrWhiteSpace(roomNumber) || string.IsNullOrWhiteSpace(roomSpace))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!roomNumber.Equals(selectedRoomId.ToString()))
            {
                MessageBox.Show("You cannot update the room number directly. Please delete the existing room and add a new one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE Room SET Available = @Available, Floor = @Floor, RoomSpace = @RoomSpace WHERE RoomNumber = @RoomNumber";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@Available", available);
                    updateCommand.Parameters.AddWithValue("@Floor", floor);
                    updateCommand.Parameters.AddWithValue("@RoomSpace", roomSpace);
                    updateCommand.Parameters.AddWithValue("@RoomNumber", selectedRoomId);
                    updateCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Room details updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                comboBox3.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                txtRoomNumber2.Clear();
                txtRoomSpace2.Clear();
                LoadRoomData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedRoomId == -1)
            {
                MessageBox.Show("Please select a room to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Room WHERE RoomNumber = @RoomNumber";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@RoomNumber", selectedRoomId);
                    deleteCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Room deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadRoomData();
                comboBox3.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                txtRoomNumber2.Clear();
                txtRoomSpace2.Clear();
                selectedRoomId = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtRoomNumber2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
    

