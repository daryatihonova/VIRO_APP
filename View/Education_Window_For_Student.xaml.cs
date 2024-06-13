
using Microsoft.Win32;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VIRO_APP.Models;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace VIRO_APP.View
{
    public partial class Education_Window_For_Student : Window
    {
        private int _studentId;
        private ViroContext _context;

        public Education_Window_For_Student(int studentId)
        {
            InitializeComponent();
            _studentId = studentId;
            _context = new ViroContext();
            LoadStudentData();
        }

        private void LoadStudentData()
        {
            // Получаем данные о студенте из базы данных
            var student = _context.Students.FirstOrDefault(s => s.StudentId == _studentId);

            if (student != null)
            {
                // Выводим ФИО студента в TextBlock
                TextBlock studentTextBlock = this.FindName("StudentTextBlock") as TextBlock;
                if (studentTextBlock != null)
                {
                    studentTextBlock.Text = $"Студент: {student.FullName}";
                }

                // Загружаем данные о курсах студента в DataGrid
                var courses = _context.StudentCourses
                    .Where(sc => sc.StudentId == _studentId)
                    .Select(sc => new
                    {
                        CourseName = sc.Course.Name,
                        Progress = sc.Progress.HasValue && sc.Progress.Value ? "Пройден" : "Не пройден"
                    })
                    .ToList();

                // Находим DataGrid по имени и устанавливаем источник данных
                DataGrid dataGrid = this.FindName("StudentDataGrid") as DataGrid;
                if (dataGrid != null)
                {
                    dataGrid.ItemsSource = courses;
                }
            }
            else
            {
                MessageBox.Show("Ошибка: Студент не найден.", "Ошибка");
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void DownloadCertificate(object sender, RoutedEventArgs e)
        {

            // Получаем данные о курсе и студенте
            var courseName = CourseName.GetCellContent(StudentDataGrid.SelectedItem) as TextBlock;
            var studentName = StudentTextBlock.Text;
            var finishDate = Course.FinishDate; // Получаем дату окончания курса
            var courseSupervisor = Course.CourseSupervisor; // Получаем проводившего курс

            // Создаем диалоговое окно для сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word документ|*.docx";
            saveFileDialog.FileName = "Сертификат" + "_" + DateTime.Now.ToString("ddMMyyyy") + ".docx";

            if (saveFileDialog.ShowDialog() == true)
            {
                // Создаем документ Word
                using (DocX document = DocX.Create(saveFileDialog.FileName))
                {
                    // Устанавливаем горизонтальное выравнивание страницы
                    document.MarginLeft = 85f;
                    document.MarginRight = 85f;

                    // Добавляем текст "Сертификат о повышении квалификации" посередине большим шрифтом
                    var titleFormat = new Formatting();
                    titleFormat.Size = 24;
                    titleFormat.Bold = true;

                    // Вставляем текст и устанавливаем выравнивание по центру
                    var titleParagraph = document.InsertParagraph("Сертификат о повышении квалификации", false, titleFormat);
                    titleParagraph.Alignment = Alignment.center;

                    // Увеличиваем размер шрифта для остального текста
                    var textFormat = new Formatting();
                    textFormat.Size = 16;

                    document.InsertParagraph(studentName, false, textFormat);
                    document.InsertParagraph("Курс: " + courseName.Text, false, textFormat);
                    document.InsertParagraph("Курс успешно пройден", false, textFormat);
                    document.InsertParagraph("Дата окончания курса: " + finishDate.ToShortDateString(), false, textFormat);
                    document.InsertParagraph("Проводивший курс: " + courseSupervisor, false, textFormat);

                    // Сохраняем документ
                    document.Save();
                }

                MessageBox.Show("Сертификат успешно скачан.");
            }
        }

    }
}