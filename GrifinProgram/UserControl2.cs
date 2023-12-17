using Bunifu.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrifinProgram
{
    public partial class UserControl2 : UserControl
    {
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
        public UserControl2()
        {
            InitializeComponent();
            AnimateForm(this.Handle);
            LoadGrid();
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

                string query = $@"select ID, Predmet as Предмет, del,avatar from Predmet WHERE del = '{statusFilter}'";

                dataGridView.DataSource = Database.Query(query);
                dataGridView.Columns[0].Visible = false;
                dataGridView.Columns[2].Visible = false;
                dataGridView.Columns[3].Visible = false;
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
                        database.InsertPredmet(bunifuTextBox1.Text, avatarBytes);
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

        private void bunifuCheckBox1_CheckedChanged(object sender, BunifuCheckBox.CheckedChangedEventArgs e)
        {
            LoadAvatar();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            try
            {
                using (Database database = new Database())
                {  // Create an instance of the Database class
                    database.UpdatePredmetData((int)dataGridView.CurrentRow.Cells[0].Value, bunifuTextBox1.Text);
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
                using (Database database = new Database())
                {  // Create an instance of the Database class
                    database.DeletePredmetData((int)dataGridView.CurrentRow.Cells[0].Value);
                    LoadGrid();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}

