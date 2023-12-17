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
    public partial class UserControl4 : UserControl
    {
        private byte[] avatarBytes;
        public UserControl4()
        {
            InitializeComponent();
            LoadGrid();
            LoadCombo();
            LoadCombo1();
        }
        private void LoadCombo()
        {
            using (Database db = new Database()) {
                db.LoadComboCategory(comboBox2);
            }
        }
        private void LoadCombo1()
        {
            using (Database db = new Database())
            {
                db.LoadComboPredmet(comboBox1);
            }
        }
        private void LoadGrid()
        {
            try
            {
                string statusFilter = bunifuCheckBox1.Checked ? "yes" : "no";

                string query = $@"SELECT 
                    Test.ID AS TestID,
                    Test.Name AS 'Название теста',
                    Test.del AS TestDel,
                    Category.Category AS Категория,
                    Predmet.Predmet AS Предмет
                FROM 
                    [GrifinProgram].[dbo].[Test] Test
                JOIN 
                    [GrifinProgram].[dbo].[Category] Category ON Test.Category = Category.ID
                JOIN 
                    [GrifinProgram].[dbo].[Predmet] Predmet ON Test.Predmet = Predmet.ID
                 WHERE Test.del = '{statusFilter}'";

                dataGridView.DataSource = Database.Query(query);
                dataGridView.Columns[0].Visible = false;
                dataGridView.Columns[2].Visible = false;
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
                        database.InsertTest(bunifuTextBox1.Text, (int)comboBox1.SelectedValue,(int)comboBox2.SelectedValue,avatarBytes);
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

        private void bunifuCheckBox1_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            LoadGrid();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            try
            {
                
                    // Ensure that 'database' is properly initialized before using it
                    using (Database database = new Database())
                    {  // Create an instance of the Database class
                        database.UpdateTest(bunifuTextBox1.Text, (int)comboBox1.SelectedValue, (int)comboBox2.SelectedValue, (int)dataGridView.CurrentRow.Cells["TestID"].Value);
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
                {
                    int testIDToDelete = (int)dataGridView.CurrentRow.Cells["TestID"].Value;
                    database.DeleteTest(testIDToDelete);
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
