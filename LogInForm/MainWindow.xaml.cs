using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ApiClient;

namespace LogInForm
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Action<string, string> OnCorrectDataProvided;

        public MainWindow()
        {
            InitializeComponent();
            OnCorrectDataProvided = (s, s1) => { return; };
            ErrorLabel.Visibility = Visibility.Hidden;
        }


        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }



        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void InputField_OnGotFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text != (string)textBox.Tag) return;
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }

            if (sender is PasswordBox passBox)
            {
                if (passBox.Password != (string)passBox.Tag) return;
                passBox.Password = "";
                passBox.Foreground = Brushes.Black;
            }
           

        }

        private void InputField_OnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {

            if (sender is TextBox textBox)
            {
                if (textBox.Text.Length != 0) return;
                textBox.Foreground = Brushes.DarkGray;
                textBox.Text = (string)textBox.Tag;
            }

            if (sender is PasswordBox passBox)
            {
                if (passBox.Password.Length != 0) return;
                passBox.Foreground = Brushes.DarkGray;
                passBox.Password = (string)passBox.Tag;
            }

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this), this);
            DragMove();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (Client.IsAuthDataCorrect(EmailInput.Text, PasswordInput.Password))
            {
                //todo send data to model
                ErrorLabel.Visibility = Visibility.Hidden;
                Close();
            }
            else
            {
                ErrorLabel.Visibility = Visibility.Visible;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            OnCorrectDataProvided(EmailInput.Text, PasswordInput.Password);
        }
    }
}
