using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    }
}
