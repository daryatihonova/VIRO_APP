using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для New_Kurs_Window.xaml
    /// </summary>
    public partial class New_Kurs_Window : Window
    {
        private Course _currentCourse = new Course();
        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler DataChanged;
        public New_Kurs_Window(Course selectedCourse)
        {
            InitializeComponent();

            if (selectedCourse != null)
                _currentCourse = selectedCourse;
            HachText.SelectedDate = DateTime.Today;
            KonText.SelectedDate= DateTime.Today;

            DataContext = _currentCourse;
        }


        private void Save(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CatText.Text) ||
                string.IsNullOrEmpty(NameText.Text) ||
                string.IsNullOrEmpty(HourText.Text) ||
                string.IsNullOrEmpty(PeopText.Text) ||
                string.IsNullOrEmpty(HachText.Text) ||
                string.IsNullOrEmpty(KonText.Text) ||
                string.IsNullOrEmpty(PrepText.Text))
            {
                MessageBox.Show("Необходимо заполнить все поля", "Ошибка");
                return;
            }
             using (var context = new ViroContext())
            {
                var existingCourse = context.Courses.FirstOrDefault(c => c.Name == _currentCourse.Name);
                if (existingCourse != null)
                {
                    if (_currentCourse.CourseId == 0)
                    {

                        MessageBox.Show($"Курс с названием {_currentCourse.Name} уже существует", "Ошибка");
                    }
                    else
                    {
                        context.Entry(existingCourse).CurrentValues.SetValues(_currentCourse);
                        context.SaveChanges();
                        MessageBox.Show("Курс успешно изменён");
                    }
                }
                else
                {
                    context.Courses.Add(_currentCourse);
                    context.SaveChanges();
                }
                

             }
           
             DataChanged?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
