using Bunifu.UI.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GrifinProgram
{
    class Database : IDisposable
    {
        
        private const string ConnectionString = "Data Source=62.78.81.19;Initial Catalog=GrifinProgram;User ID=stud2;Password=123456789";
        private DataTable studentDataTable;
        private SqlConnection connection;
        public static string Username;
        public static byte[] HashAvatar;
        public void InsertTest(string pam1, int pam2, int pam3, byte[] pam4)
        {
            string insertQuery = "INSERT INTO Test (Name,Predmet,Category, Avatar) VALUES (@Name,@Predmet,@Catagory, @Avatar)";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Добавление параметров с данными
                    command.Parameters.AddWithValue("@Name", pam1);
                    command.Parameters.AddWithValue("@Predmet", pam2);
                    command.Parameters.AddWithValue("@Catagory", pam3);
                    command.Parameters.AddWithValue("@Avatar", pam4);

                    // Выполнение SQL-запроса
                    command.ExecuteNonQuery();

                    MessageBox.Show("Запись успешно добавлена в таблицу Predmet.");
                }
            }
        }
        public void DeleteResult(int pam1)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string updateQuery = $@"UPDATE ResultTest SET del = @del WHERE ID = @ResultTest ";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Test", "yes");
                    command.Parameters.AddWithValue("@ResultTest", pam1);
                    

                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdateResult(int pam1,int pam2, string pam3, int pam4)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string updateQuery = $@"UPDATE ResultTest SET Test = @Test,Student = @Student, Bal = @Bal WHERE ID = @ResultTest ";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Test", pam1);
                    command.Parameters.AddWithValue("@Student", pam2);
                    command.Parameters.AddWithValue("@Bal", pam3);
                    command.Parameters.AddWithValue("@ResultTest", pam4);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void InsertResult(int test, int student, string bal)
        {
            string insertQuery = "INSERT INTO ResultTest (Test,Student,Bal) VALUES (@Test,@Student,@Bal)";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Добавление параметров с данными
                    command.Parameters.AddWithValue("@Test", test);
                    command.Parameters.AddWithValue("@Student", student);
                    command.Parameters.AddWithValue("@Bal", bal);
                 

                    // Выполнение SQL-запроса
                    command.ExecuteNonQuery();

                    MessageBox.Show("Запись успешно добавлена в таблицу Predmet.");
                }
            }
        }
        public void InsertCategory(string pam1, byte[] pam2)
        {
            string insertQuery = "INSERT INTO Category (Category, Avatar) VALUES (@Predmet, @Avatar)";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Добавление параметров с данными
                    command.Parameters.AddWithValue("@Predmet", pam1);
                    command.Parameters.AddWithValue("@Avatar", pam2);

                    // Выполнение SQL-запроса
                    command.ExecuteNonQuery();

                    MessageBox.Show("Запись успешно добавлена в таблицу Predmet.");
                }
            }
        }
        public void UpdateeCategoria(int studentId, string pam1)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string updateQuery = $@"UPDATE Category SET del = 'yes', Category = @Cat WHERE ID = @PredmetID ";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Cat", pam1);
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteCategoria(int studentId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string updateQuery = $@"UPDATE Category SET del = 'yes' WHERE ID = @PredmetID ";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeletePredmetData(int studentId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string updateQuery = $@"UPDATE Predmet SET del = 'yes' WHERE ID = @PredmetID ";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdatePredmetData(int studentId, string newFirstName)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string updateQuery = $@"UPDATE Predmet SET predmet = @Predmet WHERE ID = @PredmetID ";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", newFirstName);
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void InsertPredmet(string predmetName, byte[] avatarBytes)
        {
            string insertQuery = "INSERT INTO Predmet (Predmet, Avatar) VALUES (@Predmet, @Avatar)";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Добавление параметров с данными
                    command.Parameters.AddWithValue("@Predmet", predmetName);
                    command.Parameters.AddWithValue("@Avatar", avatarBytes);

                    // Выполнение SQL-запроса
                    command.ExecuteNonQuery();

                    MessageBox.Show("Запись успешно добавлена в таблицу Predmet.");
                }
            }
        }
    
        public void DisplayAvatarInPictureBox(byte[] avatarBytes, PictureBox pictureBox)
        {
            try
            {
                // Проверка, что массив байтов не пуст
                if (avatarBytes == null || avatarBytes.Length == 0)
                {
                    // Можно установить изображение по умолчанию или выполнить другие действия
                    pictureBox.Image = null;
                    return;
                }

                // Создание MemoryStream из массива байтов
                using (MemoryStream memoryStream = new MemoryStream(avatarBytes))
                {
                    // Загрузка изображения из MemoryStream
                    Image avatarImage = Image.FromStream(memoryStream);

                    // Установка изображения в PictureBox
                    pictureBox.Image = avatarImage;
                  
                }
            }
            catch (Exception ex)
            {
                // Обработка исключения, если не удалось преобразовать байты в изображение
                MessageBox.Show("Ошибка при отображении изображения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool Authenticate(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT Login, Avatar FROM Account WHERE Login = @Login AND Password = @Password";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Аутентификация успешна, сохраняем Login и Avatar
                            Database.Username = reader.GetString(0); // Первый столбец - Login
                            Database.HashAvatar = (byte[])reader.GetValue(1); // Второй столбец - Avatar

                            return true;
                        }
                    }

                    return false;
                }
            }
        }
        public void InsertStudent(string txtFirstName, string txtLastName, string txtPatanomic, byte[] avatar, string txtKlass)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlInsert = "INSERT INTO Student (FirstName, LastName, Patanomic, Avatar, del, Klass) " +
                                   "VALUES (@FirstName, @LastName, @Patanomic, @Avatar, @Del, @Klass)";

                using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", txtFirstName);
                    command.Parameters.AddWithValue("@LastName", txtLastName);
                    command.Parameters.AddWithValue("@Patanomic", txtPatanomic);
                    command.Parameters.AddWithValue("@Avatar", avatar ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Del", "no");
                    command.Parameters.AddWithValue("@Klass", txtKlass);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Студент добавлен успешно!");
                }
            }
        }
        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }
        public static DataTable Query(string sql)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sa = new SqlDataAdapter(sql, ConnectionString);
            sa.Fill(dt);
            return dt;
        }

        public void InsertAccount(string login,string password, byte[] avatar, int access)
        {
            string insertQuery = "INSERT INTO Account (Login, Password, Avatar, Acces) VALUES (@Login, @Password, @Avatar, @Acces)";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Добавление параметров с данными
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Avatar", avatar);
                    command.Parameters.AddWithValue("@Acces", access);

                    // Выполнение SQL-запроса
                    command.ExecuteNonQuery();

                    Console.WriteLine("Запись успешно добавлена в таблицу Account.");
                }
            }
        }
        public void UpdateStudentData(int studentId, string newFirstName, string newLastName, string newPatanomic,  string newKlass)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE Student " +
                                     "SET FirstName = @FirstName, " +
                                     "    LastName = @LastName, " +
                                     "    Patanomic = @Patanomic, " +
                                     "    Klass = @Klass " +
                                     "WHERE ID = @StudentId";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", newFirstName);
                    command.Parameters.AddWithValue("@LastName", newLastName);
                    command.Parameters.AddWithValue("@Patanomic", newPatanomic);
                    command.Parameters.AddWithValue("@Klass", newKlass);
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdateTest(string pam1, int pam2 , int pam3 , int pam4)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE Test SET Name = @Name, predmet = @Predmet, category = @category WHERE ID = @PredmetID";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", pam1);
                    command.Parameters.AddWithValue("@Predmet", pam2);
                    command.Parameters.AddWithValue("@category", pam3);
                    command.Parameters.AddWithValue("@StudentId", pam4);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void LoadComboCategory(System.Windows.Forms.ComboBox comboBox)
        {
            string selectQuery = "SELECT TOP (1000) [ID], [Category] FROM [GrifinProgram].[dbo].[Category] WHERE [del] = 'no'";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    // Создание DataTable для хранения результатов запроса
                    DataTable dataTable = new DataTable();

                    // Заполнение DataTable данными из базы данных
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }

                    // Привязка данных к ComboBox
                    comboBox.DisplayMember = "Category"; // Отображаемое поле
                    comboBox.ValueMember = "ID"; // Значение

                    comboBox.DataSource = dataTable;

                }
            }
        }

        public void LoadComboPredmet(System.Windows.Forms.ComboBox comboBox)
        {
            string selectQuery = "SELECT TOP (1000) [ID], [Predmet] FROM [GrifinProgram].[dbo].[Predmet] WHERE [del] = 'no'";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    // Создание DataTable для хранения результатов запроса
                    DataTable dataTable = new DataTable();

                    // Заполнение DataTable данными из базы данных
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }

                    // Привязка данных к ComboBox
                    comboBox.DisplayMember = "Predmet"; // Отображаемое поле
                    comboBox.ValueMember = "ID"; // Значение

                    comboBox.DataSource = dataTable;

                }
            }
        }


        public void DeleteTest(int testID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string updateQuery = $@"UPDATE Test SET del = 'yes' WHERE ID = @TestID";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@TestID", testID);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteStudentData(int studentId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string updateQuery = $@"UPDATE Student SET del = 'yes' WHERE ID = @StudentId";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    command.ExecuteNonQuery();
                }
            }
        }


    }
}
