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
using Team.Models;

namespace Team.View
{
    /// <summary>
    /// Interaction logic for createTeamView.xaml
    /// </summary>
    public partial class CreateTeamView : Window
    {
        private String TeamName { get; set; }
        private String Key { get; set; }

        

        private ApiClient client = new ApiClient();

        public CreateTeamView()
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

        private async void createTeamBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UserEnterTeamName())
            {
                ErrorMessage.Text = "";

                if (await TeamNameExists())
                {
                    ErrorMessage.Text = "Team name exists";
                }else
                {
                    GenerateTeamKey();

                    CreateTeam();
                }
            }
            else
            {
                ErrorMessage.Text = "Please enter team name.";
            }
        }

        private void teamName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TeamName = teamName.Text;
        }

        private void GenerateTeamKey()
        {
            Random rand = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Key =  new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[rand.Next(s.Length)]).ToArray());
        }

        private bool UserEnterTeamName()
        {
            bool enter = false;

            if (teamName.Text.Length > 0)
            {
                enter = true;
            }

            return enter;
        }

        private async Task<bool> TeamNameExists()
        {
            bool exists = false;

            try
            {
                var response = await client.GetTeam();

                foreach(TeamModel team in response){
                    if (team.Name.Equals(TeamName))
                    {
                        exists = true;
                        break;
                    }
                }
            } catch (Exception ex)
            {
                if (ex.Message.Equals("Bad Request"))
                {
                    ErrorMessage.Text = ex.Message;
                }
            }

            return exists;
        }

        private async void CreateTeam()
        {
            await client.CreateTeam(TeamName, Key);
            ErrorMessage.Text = "";
            MessageBox.Show($"Create Team: {TeamName} Enter key is {Key} Please make sure to note this. \n (This programm is still developing)");
            new EnterKeyView().Show();
            this.Close();

        }
    }


}
