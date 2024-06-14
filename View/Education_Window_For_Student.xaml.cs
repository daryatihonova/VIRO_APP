using Microsoft.Win32;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VIRO_APP.Models;
using Xceed.Document.NET;
using Xceed.Words.NET;
using System.Windows.Controls;

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
            var student = _context.Students.FirstOrDefault(s => s.StudentId == _studentId);
            if (student != null)
            {
                TextBlock studentTextBlock = this.FindName("StudentTextBlock") as TextBlock;
                if (studentTextBlock != null)
                {
                    studentTextBlock.Text = $"Студент: {student.FullName}";
                }
                var courses = _context.StudentCourses
                    .Where(sc => sc.StudentId == _studentId)
                    .Select(sc => new
                    {
                        CourseName = sc.Course.Name,
                        Progress = sc.Progress.HasValue && sc.Progress.Value ? "Пройден" : "Не пройден"
                    })
                    .ToList();
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

       
    }
}