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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VIRO_APP.View;

namespace VIRO_APP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            var login = logText.Text;
            var pass = pasText.Password;

            if (login == "admin" && pass == "admin") 
            { 
                Administrator_Window administrator_Window = new Administrator_Window();
                this.Hide();
                administrator_Window.Show();
            }
            else
            {
                Education_Window_For_Student education_Window_For_Student = new Education_Window_For_Student();
                this.Hide();
                education_Window_For_Student.Show();
            }
        }
       
            
           

    }
}
