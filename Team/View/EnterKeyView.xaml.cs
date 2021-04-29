using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Team.Models;

namespace Team.View
{
    /// <summary>
    /// Interaction logic for EnterKeyView.xaml
    /// </summary>
    public partial class EnterKeyView : Window
    {
        private ApiClient client = new ApiClient();

        public TeamModel TeamModel { get; set; }

        public String TeamKey { get; set; }

        public EnterKeyView()
        {
            InitializeComponent();
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

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            this.DragMove();
        }

        private void createTeamBtn_Click(object sender, RoutedEventArgs e)
        {
            new CreateTeamView().Show();
            this.Close();
        }

        private async Task<bool> TeamExists()
        {
            bool exists = false;

            try
            {
                var response = await client.GetTeam();

                foreach (TeamModel team in response)
                {
                    if (team.TeamKey.Equals(TeamKey))
                    {
                        exists = true;
                        TeamModel = team;
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

        private void KeyBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TeamKey = KeyBox.Text;
        }

        private async void enterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (await TeamExists())
            {
                new MainView(TeamModel).Show();
                this.Close();
            }else
            {
                ErrorMessage.Text = "Team not found.";
            }
        }
    }
}
