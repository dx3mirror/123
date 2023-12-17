using Bunifu.UI.WinForms;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace GrifinProgram
{
    public partial class Form1 : Form
    {
        private string connectionString;
        private string textToDraw = "Авторизируйтесь для продолжения работы";
        private int textXPosition;
        private Font textFont = new Font("Franklin Gothic Book", 14, FontStyle.Bold);
        private Color backgroundColor = Color.FromArgb(255, 255, 255);
        
        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            InitializePanel();
            Check();

        }
        public bool Authenticate(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT COUNT(*) FROM Account WHERE Login = @Login AND Password = @Password";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    int result = (int)command.ExecuteScalar();
                    return result > 0;
                }
            }
        }
        private void InitializePanel()
        {
            bunifuPanel2.Paint += Panel1_Paint;
            textXPosition = bunifuPanel2.Width; // Начальная позиция текста справа
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            textXPosition -= 5; // Изменение позиции текста (скорость движения)
            if (textXPosition + TextRenderer.MeasureText(textToDraw, Font).Width < 0)
            {
                textXPosition = bunifuPanel2.Width; // Перезапуск анимации, когда текст выходит за границу слева
            }
            bunifuPanel2.Invalidate(); // Запрос на перерисовку панели
        }
        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 50; // Интервал обновления (в миллисекундах)
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(backgroundColor); // Установка цвета фона
            

            // Отрисовка текста
            using (SolidBrush brush = new SolidBrush(Color.Blue))
            {
                e.Graphics.DrawString(textToDraw, textFont, brush, new Point(textXPosition, bunifuPanel2.Height / 2));
            }
        }
        
       

        private void timer1_Tick(object sender, EventArgs e)
        {
           
        }

        private void bunifuTextBox2_OnIconRightClick(object sender, EventArgs e)
        {
            bunifuTextBox2.UseSystemPasswordChar = false;
        }

        private void bunifuTextBox2_OnIconLeftClick(object sender, EventArgs e)
        {
            bunifuTextBox2.UseSystemPasswordChar = false;
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {
                using (Database database = new Database())
                {
                    // Аутентификация пользователя
                    bool isAuthenticated = database.Authenticate(bunifuTextBox1.Text, bunifuTextBox2.Text);

                    if (isAuthenticated)
                    {
                        // Аутентификация успешна, открываем Form2
                        Form2 form2 = new Form2();
                        form2.Show();
                        this.Hide(); // Скрываем текущую форму (если нужно)

                    }
                    else
                    {
                        // Аутентификация неуспешна, выводим сообщение
                        MessageBox.Show("Неверный логин или пароль", "Ошибка аутентификации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений, если необходимо
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Check()
        {
            bunifuTextBox2.UseSystemPasswordChar = false;
        }

        private void bunifuCheckBox1_CheckedChanged(object sender, BunifuCheckBox.CheckedChangedEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bunifuTextBox2.UseSystemPasswordChar = false;
        }
    }
}
