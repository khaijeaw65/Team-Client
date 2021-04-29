using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Team.View;

namespace Team
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        private String username;
        private String password;

        private ApiClient client = new ApiClient();

        public String Username
        {
            get { return username; }
            set { username = value; }
        }

        public String Password
        {
            get { return password; }
            set { password = value; }
        }

        public Login()
        {
            InitializeComponent();
        }

        private void UserName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UsernameBox.Text.Equals("Username"))
                UsernameBox.Text = "";
        }

        private void UserName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(UsernameBox.Text))
                UsernameBox.Text = "Username";
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password.Equals("Password"))
                PasswordBox.Password = "";
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(PasswordBox.Password))
                PasswordBox.Password = "Password";
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            this.DragMove();
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UsernameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Username = UsernameBox.Text;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = PasswordBox.Password;
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await client.Login(Username, Password);

                var userInfo = await client.GetLoginUserInfo(result.id);
                
                if (result != null)
                {
                    ErrorMessage.Text = "";
                    new EnterKeyView().Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Bad Request"))
                {
                    ErrorMessage.Text = "Wrong username or password";
                }
            }
        }
        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            new RegisterView().Show();
            this.Close();
        }
    }
}
