using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Team.Models;

namespace Team.View
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window
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

        public RegisterView()
        {
            InitializeComponent();
        }

        //UI Behavior
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            this.DragMove();
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
            new Login().Show();
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

        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            this.DragMove();
        }

        //End UI Behavior

        private async void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UserEnterUsername() && UserEnterPassword())
            {
                ErrorMessage.Text = "";

                if (await UserExist())
                {
                    ErrorMessage.Text = "Username exists";
                }
                else
                {
                    Register();
                }   
            }
            else
            {
                ErrorMessage.Text = "Username must contain at least 1 character and 1 number";
            }
        }

        private async void Register()
        {
            await client.Register(Username, Password);
            ErrorMessage.Text = "";
            MessageBox.Show($"Welcome {Username}");
            new Login().Show();
            this.Close();
        }

        private async Task<bool> UserExist()
        {
            bool exists = false;

            try
            {
                var result = await client.GetUserData();

                foreach (UserModel user in result)
                {
                    if (user.Username.Equals(Username))
                    {
                        exists = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Bad Request"))
                {
                    ErrorMessage.Text = ex.Message;
                }
            }

            return exists;
        }

        private bool UserEnterUsername()
        {
            bool enter = false;

            if (Username.Length > 0)
                enter = true;

            return enter;
        }

        private bool UserEnterPassword()
        {
            bool enter = false;

            if (Password.Length > 0)
                enter = true;

            return enter;
        }
    }
}
