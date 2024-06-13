using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VIRO_APP.Models;


namespace VIRO_APP.View
{
    /// <summary>
    /// Логика взаимодействия для Education_Window.xaml
    /// </summary>
    public partial class Education_Window : Window
    {
        private ObservableCollection<StudentCourse> _studentcourses;
        public ObservableCollection<StudentCourse> Studentcourses
        {
            get { return _studentcourses; }
            set { _studentcourses = value; StCo.ItemsSource = _studentcourses; }
        }
        public Education_Window()
        {
            InitializeComponent();
            using (var context = new ViroContext())
            {
                Studentcourses = new ObservableCollection<StudentCourse>(context.StudentCourses
                    .Include(s => s.Student)
                    .Include(c => c.Course)
                    .ToList());
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Administrator_Window administrator_Window = new Administrator_Window();
            this.Hide();
            administrator_Window.Show();
        }

        private void New_Education(object sender, RoutedEventArgs e)
        {
            New_Education_Window new_Education_Window = new New_Education_Window(null);
            new_Education_Window.Show();
        }

        private void change(object sender, RoutedEventArgs e)
        {
            New_Education_Window edit_Education = new New_Education_Window((sender as Button).DataContext as StudentCourse );
            //edit_Education.DataChanged += UpdateDataGrid;
            edit_Education.ShowDialog();
        }

        private void UpdateDataGrid(object sender, DataGridRowEventArgs e)
        {
            using(var context = new ViroContext())
            {
                Studentcourses = new ObservableCollection<StudentCourse>(context.StudentCourses.ToList());
            }
        }



        private void delete(object sender, RoutedEventArgs e)
        {
            var studentCoursefordelete = StCo.SelectedItems.Cast<StudentCourse>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить {studentCoursefordelete.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new ViroContext())
                    {
                        context.StudentCourses.RemoveRange(studentCoursefordelete);
                        context.SaveChanges();
                        MessageBox.Show("Данные удалены");
                        Studentcourses = new ObservableCollection<StudentCourse>(context.StudentCourses.ToList());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void export(object sender, RoutedEventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Установка контекста лицензии

            using (var context = new ViroContext())
            {
                var allCourses = context.Courses.ToList();

                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Курсы");

                    // Set headers
                    worksheet.Cells[1, 1].Value = "Название курса";
                    worksheet.Cells[1, 2].Value = "Количество предполагаемых обучающихся";
                    worksheet.Cells[1, 3].Value = "Дата начала";
                    worksheet.Cells[1, 4].Value = "Дата окончания";
                    worksheet.Cells[1, 5].Value = "Студенты на курсе";
                    for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
                    {
                        worksheet.Column(i).Width = 40;
                    }

                    int row = 2;
                    foreach (var course in allCourses)
                    {
                        var studentsOnCourse = context.StudentCourses
                            .Where(sc => sc.CourseId == course.CourseId)
                            .Select(sc => sc.Student)
                            .ToList();

                        // Set course details
                        worksheet.Cells[row, 1].Value = course.Name;
                        worksheet.Cells[row, 2].Value = course.NumberOfPeople;
                        worksheet.Cells[row, 3].Value = course.StartDate.ToShortDateString();
                        worksheet.Cells[row, 4].Value = course.FinishDate.ToShortDateString();

                        // Set student details
                        int studentRow = row + 1;
                        foreach (var student in studentsOnCourse)
                        {
                            worksheet.Cells[studentRow, 5].Value = student.FirstName + " " + student.LastName;
                            studentRow++;
                        }

                        // Set student count
                        worksheet.Cells[studentRow, 5].Value = $"Количество студентов: {studentsOnCourse.Count}";

                        // Move to the next course row
                        row = studentRow + 1;
                    }

                    // Save the Excel file
                    var saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        excelPackage.SaveAs(excelFile);
                    }
                }
            }
        }


    }
}
