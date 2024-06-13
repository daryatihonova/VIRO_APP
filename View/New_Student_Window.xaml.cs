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
    /// Логика взаимодействия для New_Student_Window.xaml
    /// </summary>
    public partial class New_Student_Window : Window
    {
        private Student _currentStudent = new Student();
        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler DataChanged;

        public New_Student_Window(Student selectedStudent)
        {
            InitializeComponent();
            if (selectedStudent != null)
                _currentStudent = selectedStudent;
            DataContext = _currentStudent;
            //PassText.LostFocus += PasswordBox_LostFocus;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameText.Text) ||
                string.IsNullOrEmpty(FamText.Text)||
                string.IsNullOrEmpty(OtchText.Text) ||
                string.IsNullOrEmpty(BirthText.Text) ||
                string.IsNullOrEmpty(ObrText.Text) ||
                string.IsNullOrEmpty(DolText.Text) ||
                string.IsNullOrEmpty(LogText.Text) ||
                 string.IsNullOrEmpty(PassText.Text))
            { 
                MessageBox.Show("Необходимо заполнить все поля", "Ошибка");
                return;
            }

            using (var context = new ViroContext())
            {
                if (_currentStudent.StudentId == 0)
                {
                    Student newStudent = new Student
                    {
                        FirstName = NameText.Text,
                        LastName = FamText.Text,
                        Patronymic = OtchText.Text,
                        Birthday = DateTime.Parse(BirthText.Text),
                        Education = ObrText.Text,
                        Post = DolText.Text,
                        Login = LogText.Text,
                        Password = PassText.Text
                    };
                    context.Students.Add(newStudent);
                }
                else
                {
                    var existingStudent = context.Students.Find(_currentStudent.StudentId);
                    if (existingStudent != null)
                    {
                        existingStudent.FirstName = NameText.Text;
                        existingStudent.LastName = FamText.Text;
                        existingStudent.Patronymic = OtchText.Text;
                        existingStudent.Birthday = DateTime.Parse(BirthText.Text);
                        existingStudent.Education = ObrText.Text;
                        existingStudent.Post = DolText.Text;
                        existingStudent.Login = LogText.Text;
                        existingStudent.Password = PassText.Text;
                    }
                }
                context.SaveChanges();
                DataContext = _currentStudent;
                DataChanged?.Invoke(this, EventArgs.Empty);

            }
            MessageBox.Show("Информация сохранена", "Внимание");
        }


        //private void PasswordBox_LostFocus(object sender, EventArgs e)
        //{
        //    string password = PassText.Text;
        //    if (password.Length < 6)
        //    {
        //        MessageBox.Show("Пароль должен содержать минимум 6 символов", "Ошибка");
        //    }
        //    else if (password.Length > 10)
        //    {
        //        MessageBox.Show("Пароль должен содержать менее 10 символов", "Ошибка");
        //    }
        //    else if (password.Any(char.IsUpper))
        //    {
        //        MessageBox.Show("Пароль должен содержать минимум 1 прописную букву", "Ошибка");
        //    }
        //    else if (password.Any(char.IsDigit))
        //    {
        //        MessageBox.Show("Пароль должен содержать минимум 1 цифру", "Ошибка");
        //    }
        //    else if (password.Any(c => "!@#$%^".Contains(c)))
        //    {
        //        MessageBox.Show("Пароль должен содержать минимум один из следующих символов: ! @ # $ % ^.", "Ошибка");
        //    }
        //}

    }
}
