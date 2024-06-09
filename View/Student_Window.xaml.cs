﻿using System;
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
    /// Логика взаимодействия для Student_Window.xaml
    /// </summary>
    public partial class Student_Window : Window
    {
        private ObservableCollection<Student> _students;
        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set { _students = value; Stud.ItemsSource = value; } 
        }

        public Student_Window()
        {
            InitializeComponent();

            using (var context = new ViroContext())
            {
            Students = new ObservableCollection<Student>(context.Students.ToList());
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Administrator_Window administrator_Window = new Administrator_Window();
            this.Hide();
            administrator_Window.Show();
        }

        private void New_Student(object sender, RoutedEventArgs e)
        {
            New_Student_Window new_Student_Window = new New_Student_Window();
            new_Student_Window.Show();
        }



    }
}
