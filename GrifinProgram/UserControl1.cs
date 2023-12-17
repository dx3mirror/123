using Bunifu.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrifinProgram
{
    public partial class UserControl1 : UserControl
    {
        private const string ConnectionString = "Data Source=62.78.81.19;Initial Catalog=GrifinProgram;User ID=stud2;Password=123456789"; 
        private byte[] avatarBytes;
        private DataTable studentDataTable;
        private const int AW_ACTIVATE = 0x20000;
        private const int AW_BLEND = 0x80000;
        private const int AW_CENTER = 0x00000010;
        private const int AW_HIDE = 0x10000;
        private const int AW_HOR_POSITIVE = 0x00000001;
        private const int AW_HOR_NEGATIVE = 0x00000002;
        private const int AW_SLIDE = 0x40000;

        [DllImport("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        [Obsolete]
        public UserControl1()
        {
            InitializeComponent();
            ConfigureBunifuDataGridView(dataGridView);
            LoadGrid();
            AnimateForm(this.Handle);
        }
        private async void AnimateForm(IntPtr handle)
        {
            // Задержка перед анимацией (просто для демонстрации)
            await Task.Delay(500);

            // Запустить анимацию для формы
            AnimateWindow(handle, 500, AW_ACTIVATE | AW_BLEND | AW_CENTER | AW_SLIDE);
        }

        private void LoadGrid()
        {
            try
            {
                string statusFilter = bunifuCheckBox1.Checked ? "yes" : "no";

                string query = $@"SELECT 
                ID AS 'Идентификатор', 
                FirstName AS 'Имя', 
                LastName AS 'Фамилия', 
                Patanomic AS 'Отчество', 
                Avatar, 
                del AS 'Статус удаления', 
                Klass AS 'Класс' 
            FROM Student WHERE del = '{statusFilter}'";

                dataGridView.DataSource = Database.Query(query);
                dataGridView.Columns[5].Visible = false;
                dataGridView.Columns[4].Visible = false;
                dataGridView.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
       
        
        private void LoadAvatar()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.gif;*.png)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Загрузка изображения и конвертация в массив байтов
                    using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        avatarBytes = new byte[fs.Length];
                        fs.Read(avatarBytes, 0, Convert.ToInt32(fs.Length));
                    }

                    // Опционально: Отображение выбранного изображения
                    bunifuPictureBox1.Image = Image.FromStream(new MemoryStream(avatarBytes));
                }
            }
        }
        

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {
                LoadAvatar();

                if (avatarBytes != null)
                {
                    // Ensure that 'database' is properly initialized before using it
                    using (Database database = new Database())
                    {  // Create an instance of the Database class
                        database.InsertStudent(bunifuTextBox1.Text, bunifuTextBox2.Text, bunifuTextBox3.Text, avatarBytes, bunifuTextBox4.Text);
                        LoadGrid();
                    }
                }
                else
                {
                    MessageBox.Show("Avatar is null. Please load an avatar before inserting the student.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            try
            {

                // Ensure that 'database' is properly initialized before using it
                using (Database database = new Database())
                {  // Create an instance of the Database class
                    database.UpdateStudentData((int)dataGridView.CurrentRow.Cells[0].Value, bunifuTextBox1.Text, bunifuTextBox2.Text, bunifuTextBox3.Text, bunifuTextBox4.Text);
                    LoadGrid();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            try
            {

                // Ensure that 'database' is properly initialized before using it
                using (Database database = new Database())
                {  // Create an instance of the Database class
                    database.DeleteStudentData((int)dataGridView.CurrentRow.Cells[0].Value);
                    LoadGrid();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ConfigureBunifuDataGridView(BunifuDataGridView dataGridView)
        {
            // Задание шрифта для заголовков столбцов
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Franklin Gothic Book", 12, FontStyle.Bold);

            // Задание шрифта для ячеек
            dataGridView.DefaultCellStyle.Font = new Font("Franklin Gothic Book", 11);

            // Задание цвета фона для заголовков столбцов
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(48, 79, 254);

            // Задание цвета фона для четных строк
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            // Задание цвета фона для выделенных ячеек
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 193, 7);
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Задание цвета текста для заголовков столбцов
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            // Задание цвета текста для ячеек
            dataGridView.DefaultCellStyle.ForeColor = Color.Black;

             

            // Другие настройки по вашему усмотрению
        }

        private void bunifuCheckBox1_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            LoadGrid();
        }
    }
}
