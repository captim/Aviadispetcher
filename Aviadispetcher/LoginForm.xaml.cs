using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Aviadispetcher
{
    /// <summary>
    /// Логика взаимодействия для LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        private void AuthCheck()
        {
            if (MainWindow.logedUser.LogCheck(logTextBox.Text, passwordTextBox.Text) == 2)
            {
                Application.Current.MainWindow.Show();
                Close();
            }
            else
            {
                MainWindow.ErrorShow(new Exception(), "Ввведіть правильні дані авторизації.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            AuthCheck();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                AuthCheck();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }
    }
}
