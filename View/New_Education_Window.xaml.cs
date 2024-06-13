using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Логика взаимодействия для New_Education_Window.xaml
    /// </summary>
    public partial class New_Education_Window : Window
    {
        private StudentCourse _currentStudentCourse = new StudentCourse();
        public delegate void DataChangedEvantHandler(object sender, EventArgs e);

        public event DataChangedEvantHandler DataChangedEvant;

        public New_Education_Window(StudentCourse selectedStudentCourse)
        {
            InitializeComponent();

            if (selectedStudentCourse != null)
            {
                _currentStudentCourse = selectedStudentCourse;
                DataContext = _currentStudentCourse;
            }


            using (var context = new ViroContext())
            {
                Students = new ObservableCollection<Student>(context.Students.ToList());
            }
            StudentBox.ItemsSource = Students;
            DataContext = _currentStudentCourse;
            StudentBox.SelectionChanged += StudentBox_SelectionChanged;


            using (var context = new ViroContext())
            {
                Courses = new ObservableCollection<Course>(context.Courses.ToList());
            }
            CourseBox.ItemsSource = Courses;
            DataContext = _currentStudentCourse;
            CourseBox.SelectionChanged += CourseBox_SelectionChanged;
        }

        public ObservableCollection<Student> Students { get;set; }
        public ObservableCollection<Course> Courses { get;set; }


        public void StudentBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedStudent = (Student) StudentBox.SelectedItem;
            if (selectedStudent != null)
            {
                _currentStudentCourse.StudentId = selectedStudent.StudentId;
             }
        }

        private void CourseBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCourse = (Course)CourseBox.SelectedItem;
            if(selectedCourse != null)
            {
                _currentStudentCourse.CourseId = selectedCourse.CourseId;
            }

        }


        private void Save(object sender, EventArgs e)
        {
            using (var context = new ViroContext())
            {
                if (_currentStudentCourse.StudentCourseId == 0)
                {
                    StudentCourse newStudentCourse = new StudentCourse
                    {
                        StudentId = _currentStudentCourse.StudentId,
                        CourseId = _currentStudentCourse.CourseId,
                        Progress = bool.Parse(ProgressBox.Text)

                };
                    context.StudentCourses.Add(newStudentCourse);
                }
                else
                {
                    var existingStudentCourse = context.StudentCourses.Find(_currentStudentCourse.StudentCourseId);
                    if (existingStudentCourse != null)
                    {
                        existingStudentCourse.StudentId = int.Parse(StudentBox.Text);
                        existingStudentCourse.CourseId = int.Parse(CourseBox.Text);
                        existingStudentCourse.Progress = bool.Parse(ProgressBox.Text);
                    }
                }
                context.SaveChanges();
                DataContext = _currentStudentCourse;
                DataChangedEvant?.Invoke(this, EventArgs.Empty);

            }
            MessageBox.Show("Данные успешно сохранены");
        }


        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
