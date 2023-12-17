using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace GrifinProgram
{
    public partial class UserControl5 : UserControl
    {
        private const string connectionString = "Data Source=62.78.81.19;Initial Catalog=GrifinProgram;User ID=stud2;Password=123456789";
        public UserControl5()
        {
            InitializeComponent();
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            string query = $@"SELECT 
                    Test.ID AS TestID,
                    Test.Name AS 'Название теста',
                    Category.Category AS Категория,
                    Predmet.Predmet AS Предмет
                FROM 
                    [GrifinProgram].[dbo].[Test] Test
                JOIN 
                    [GrifinProgram].[dbo].[Category] Category ON Test.Category = Category.ID
                JOIN 
                    [GrifinProgram].[dbo].[Predmet] Predmet ON Test.Predmet = Predmet.ID";
                    OpenExcelWithData(connectionString, query);
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            
            string query = $@"SELECT 
            ID AS 'Идентификатор', 
            FirstName AS 'Имя', 
            LastName AS 'Фамилия', 
            Patanomic AS 'Отчество', 
            Klass AS 'Класс' 
        FROM Student ";

            OpenExcelWithData(connectionString, query);
        }
        static void OpenExcelWithData(string connectionString, string query)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        System.Data.DataTable dataTable = new System.Data.DataTable();
                        adapter.Fill(dataTable);

                        Workbook workbook = excelApp.Workbooks.Add();
                        Worksheet worksheet = (Worksheet)workbook.Worksheets[1];

                        // Добавление надписи по центру
                        int centerColumn = dataTable.Columns.Count / 2 + 1;
                        Range titleRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, dataTable.Columns.Count]];
                        titleRange.Merge();
                        titleRange.Value = "Отчет";
                        titleRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        titleRange.Font.Size = 16;

                        // Запись заголовков столбцов
                        for (int j = 0; j < dataTable.Columns.Count; j++)
                        {
                            worksheet.Cells[2, j + 1] = dataTable.Columns[j].ColumnName;
                        }

                        // Заполнение ячеек данными из DataTable
                        for (int i = 0; i < Math.Min(dataTable.Rows.Count, 200); i++)
                        {
                            for (int j = 0; j < dataTable.Columns.Count; j++)
                            {
                                worksheet.Cells[i + 3, j + 1] = dataTable.Rows[i][j].ToString();
                            }
                        }

                        // Применение форматирования и стилей
                        FormatTable(worksheet, Math.Min(dataTable.Rows.Count, 200), dataTable.Columns.Count);
                    }
                }

                // Отображение Excel
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        static void FormatTable(Worksheet worksheet, int rowCount, int columnCount)
        {
            // Применение стилей и форматирования
            Range headerRange = worksheet.Range["A1", ConvertToLetter(columnCount) + "1"];
            headerRange.Interior.Color = XlRgbColor.rgbGreen; // Цвет фона для заголовков
            headerRange.Font.Color = XlRgbColor.rgbWhite; // Цвет текста для заголовков
            headerRange.Font.Bold = true; // Жирный шрифт для заголовков

            Range dataRange = worksheet.Range["A2", ConvertToLetter(columnCount) + $"{rowCount + 2}"];
            dataRange.Interior.Color = XlRgbColor.rgbLightGreen; // Цвет фона для данных

            // Автоширина столбцов
            worksheet.Columns.AutoFit();

            // Добавление разделительных линий
            worksheet.UsedRange.Borders.LineStyle = XlLineStyle.xlContinuous;
            worksheet.UsedRange.Borders.Color = XlRgbColor.rgbBlack;

            // Условное форматирование для столбца "Статус удаления"
            FormatConditionalFormatting(worksheet, rowCount, columnCount);
        }

        static void FormatConditionalFormatting(Worksheet worksheet, int rowCount, int columnCount)
        {
            Range statusColumn = worksheet.Range["F2", ConvertToLetter(columnCount) + $"{rowCount + 2}"];

            // Условное форматирование для столбца "Статус удаления"
            FormatCondition formatCondition = (FormatCondition)statusColumn.FormatConditions.Add(XlFormatConditionType.xlCellValue, XlFormatConditionOperator.xlEqual, "'yes'");
            formatCondition.Interior.Color = XlRgbColor.rgbRed; // Цвет фона для "yes"
            formatCondition.Font.Color = XlRgbColor.rgbWhite; // Цвет текста для "yes"
        }

        // Вспомогательная функция для конвертации номера столбца в буквенное обозначение
        private static string ConvertToLetter(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            string query = $@"select ID, Category as Категория from Category";
            OpenExcelWithData(connectionString, query);

        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            string query = $@"select ID, Predmet as Предмет from Predmet";
            OpenExcelWithData(connectionString, query);
        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            string query = $@"SELECT RT.ID AS ResultTestID,
                                                   RT.Bal,Concat(
                                                   S.FirstName, ' ',
                                                   S.LastName, ' ',
                                                   S.Patanomic) as FIO,
                                                   S.Klass,
                                                   T.Name AS TestName
                                            FROM [GrifinProgram].[dbo].[ResultTest] RT
                                            JOIN [GrifinProgram].[dbo].[Student] S ON RT.Student = S.ID
                                            JOIN [GrifinProgram].[dbo].[Test] T ON RT.Test = T.ID";
            OpenExcelWithData(connectionString, query);
        }
    }
}
