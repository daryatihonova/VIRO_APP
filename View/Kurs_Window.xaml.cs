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
    /// Логика взаимодействия для Kurs_Window.xaml
    /// </summary>
    public partial class Kurs_Window : Window
    {
        private ObservableCollection<Course> _courses;
        public ObservableCollection<Course> Courses 
        { 
            get { return _courses; }
            set { _courses = value; Kurs.ItemsSource = value; }
        }
        public Kurs_Window()
        {
            InitializeComponent();

            using (var context = new ViroContext())
            {
                Courses = new ObservableCollection<Course>(context.Courses.ToList());
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Administrator_Window administrator_Window = new Administrator_Window();
            this.Hide();
            administrator_Window.Show();
        }

        private void New_Kurs(object sender, RoutedEventArgs e)
        {
            New_Kurs_Window new_Kurs_Window = new New_Kurs_Window(null);
            new_Kurs_Window.Show();
        }

        private void Edit_Kurs(object sender, EventArgs e)
        {
            New_Kurs_Window edit_Kurs_Window = new New_Kurs_Window((sender as Button).DataContext as Course);
            edit_Kurs_Window.DataChanged += UpdateDataGrid;
            edit_Kurs_Window.ShowDialog();
        }

        private void UpdateDataGrid(object sender, EventArgs e)
        {
            using (var context = new ViroContext())
            {
                Courses = new ObservableCollection<Course>(context.Courses.ToList());
            }
        }

        private void Delete_Kurs(object sender, EventArgs e)
        {
            var coursesForDelete = Kurs.SelectedItems.Cast<Course>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {coursesForDelete.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new ViroContext())
                    {
                        context.Courses.RemoveRange(coursesForDelete);
                        context.SaveChanges();
                        MessageBox.Show("Данные удалены");
                        Courses = new ObservableCollection<Course>(context.Courses.ToList());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

        }

        private void Click_Kurs_Search(object sender, RoutedEventArgs e)
        {
            string kurs_Search = Kurs_Search.Text;
            if (string.IsNullOrEmpty(kurs_Search))
            {
                Courses = new ObservableCollection<Course>(_courses);
            }
            else
            {
                using (var context = new ViroContext())
                {
                    var courses = context.Courses.Where(c => c.Name == kurs_Search).ToList();
                if (courses.Count == 0)
                    {
                        MessageBox.Show("Такого курса не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        Courses = new ObservableCollection<Course>(courses);
                    }
                
                }
            }
        }



    }
}
