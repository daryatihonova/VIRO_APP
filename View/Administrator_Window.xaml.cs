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

namespace VIRO_APP.View
{
    /// <summary>
    /// Логика взаимодействия для Administrator_Window.xaml
    /// </summary>
    public partial class Administrator_Window : Window
    {
        public Administrator_Window()
        {
            InitializeComponent();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Hide();
            mainWindow.Show();
        }

        private void Kurs(object sender, RoutedEventArgs e)
        {
            Kurs_Window kurs_Window = new Kurs_Window();
            this.Hide();
            kurs_Window.Show();
        }

        private void Student(object sender, RoutedEventArgs e)
        {
            Student_Window student_Window = new Student_Window();
            this.Hide(); 
            student_Window.Show();
        }

        private void Education(object sender, RoutedEventArgs e)
        {
            Education_Window education_Window = new Education_Window();
            this.Hide();
            education_Window.Show();
        }
    }
}
