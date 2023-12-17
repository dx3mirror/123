using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrifinProgram
{
    public partial class UserControl3 : UserControl
    {
        private byte[] avatarBytes;
        public UserControl3()
        {
            InitializeComponent();
            LoadGrid();
        }
        private void LoadGrid()
        {
            try
            {
                string statusFilter = bunifuCheckBox1.Checked ? "yes" : "no";

                string query = $@"select ID, Category as Категория, del,avatar from Category WHERE del = '{statusFilter}'";

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
                        database.InsertCategory(bunifuTextBox1.Text, avatarBytes);
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
                    database.UpdateeCategoria((int)dataGridView.CurrentRow.Cells[0].Value,bunifuTextBox1.Text);
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
                    database.DeleteCategoria((int)dataGridView.CurrentRow.Cells[0].Value);
                    LoadGrid();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuCheckBox1_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            LoadGrid();
        }
    }
}
