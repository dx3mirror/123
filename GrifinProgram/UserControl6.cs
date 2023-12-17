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

namespace GrifinProgram
{
    public partial class UserControl6 : UserControl
    {
        const string connectionString = "Data Source=62.78.81.19;Initial Catalog=GrifinProgram;User ID=stud2;Password=123456789";
        public UserControl6()
        {
            InitializeComponent();
            LoadDataIntoComboBox();
            LoadTestDataIntoComboBox();
            LoadGrid();
        }
        private void LoadGrid()
        {
            dataGridView.DataSource = Database.Query($@"SELECT RT.ID AS ResultTestID,
                                                   RT.Bal,Concat(
                                                   S.FirstName, ' ',
                                                   S.LastName, ' ',
                                                   S.Patanomic) as FIO,
                                                   S.Klass,
                                                   T.Name AS TestName
                                            FROM [GrifinProgram].[dbo].[ResultTest] RT
                                            JOIN [GrifinProgram].[dbo].[Student] S ON RT.Student = S.ID
                                            JOIN [GrifinProgram].[dbo].[Test] T ON RT.Test = T.ID");
            dataGridView.Columns[0].Visible = false;
        }
        private void LoadTestDataIntoComboBox()
        {
            string connectionString = "Data Source=62.78.81.19;Initial Catalog=GrifinProgram;User ID=stud2;Password=123456789";
            string query = @"SELECT TOP (1000) [ID]
                          ,[Name]
                          ,[Predmet]
                          ,[Category]
                          ,[del]
                          ,[Avatar]
                   FROM [GrifinProgram].[dbo].[Test]";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Настройка ComboBox
                        comboBox1.DataSource = dataTable;
                        comboBox1.DisplayMember = "Name"; // Отображаем [Name]
                        comboBox1.ValueMember = "ID"; // Используем [ID] в качестве ValueMember
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
        private void LoadDataIntoComboBox()
        {
            
            string query = @"SELECT TOP (1000) [ID]
                          ,[FirstName]
                          ,[LastName]
                          ,[Patanomic]
                          ,[Avatar]
                          ,[del]
                          ,[Klass]
                   FROM [GrifinProgram].[dbo].[Student] where del = 'no '";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Объединение столбцов [FirstName], [LastName], [Patanomic] в одну строку
                        dataTable.Columns.Add("FullName", typeof(string), "FirstName + ' ' + LastName + ' ' + Patanomic");

                        // Настройка ComboBox
                        comboBox2.DataSource = dataTable;
                        comboBox2.DisplayMember = "FullName";
                        comboBox2.ValueMember = "ID";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            using (Database database = new Database())
            {
                database.InsertResult((int)comboBox1.SelectedValue,(int)comboBox2.SelectedValue,bunifuTextBox1.Text);
                LoadGrid();
            }
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            using(Database database = new Database())
            {
                database.UpdateResult((int)comboBox1.SelectedValue,
                    (int)comboBox2.SelectedValue,
                    bunifuTextBox1.Text,
                    (int)dataGridView.CurrentRow.Cells["ResultTest.ID"].Value);
                LoadGrid();
            }
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            using( Database database = new Database())
            {
                database.DeleteResult((int)dataGridView.CurrentRow.Cells["ResultTest.ID"].Value);
                LoadGrid();
            }
        }
    }
}
