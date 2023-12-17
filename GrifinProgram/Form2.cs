using Bunifu.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrifinProgram
{
    public partial class Form2 : Form
    {
       
        public Form2()
        {
            InitializeComponent();
            LoadUser();
           

        }
       
        private void LoadUser()
        {
            try
            {

                // Ensure that 'database' is properly initialized before using it
                using (Database database = new Database())
                {  // Create an instance of the Database class
                    database.DisplayAvatarInPictureBox(Database.HashAvatar,bunifuPictureBox1);
                    bunifuLabel1.Text = Database.Username;


                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            UserControl1 userControl1 = new UserControl1();
            bunifuPanel2.Controls.Clear();
            userControl1.Dock = DockStyle.Fill;
            bunifuPanel2.Controls.Add(userControl1);
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            UserControl2 userControl1 = new UserControl2();
            bunifuPanel2.Controls.Clear();
            userControl1.Dock = DockStyle.Fill;
            bunifuPanel2.Controls.Add(userControl1);
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            UserControl3 userControl1 = new UserControl3();
            bunifuPanel2.Controls.Clear();
            userControl1.Dock = DockStyle.Fill;
            bunifuPanel2.Controls.Add(userControl1);
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            UserControl4 userControl1 = new UserControl4();
            bunifuPanel2.Controls.Clear();
            userControl1.Dock = DockStyle.Fill;
            bunifuPanel2.Controls.Add(userControl1);
        }

        private void bunifuButton6_Click(object sender, EventArgs e)
        {
            UserControl5 userControl1 = new UserControl5();
            bunifuPanel2.Controls.Clear();
            userControl1.Dock = DockStyle.Fill;
            bunifuPanel2.Controls.Add(userControl1);
        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            UserControl6 userControl1 = new UserControl6();
            bunifuPanel2.Controls.Clear();
            userControl1.Dock = DockStyle.Fill;
            bunifuPanel2.Controls.Add(userControl1);
        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
