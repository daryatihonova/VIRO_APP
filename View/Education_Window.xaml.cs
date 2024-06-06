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
    /// Логика взаимодействия для Education_Window.xaml
    /// </summary>
    public partial class Education_Window : Window
    {
        public Education_Window()
        {
            InitializeComponent();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Administrator_Window administrator_Window = new Administrator_Window();
            this.Hide();
            administrator_Window.Show();
        }

        private void New_Education(object sender, RoutedEventArgs e)
        {
            New_Education_Window new_Education_Window = new New_Education_Window();
            new_Education_Window.Show();
        }
    }
}
