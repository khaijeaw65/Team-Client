using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Team.Helper;
using Team.Models;

namespace Team.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public TeamModel TeamModel { get; set; }

        private List<TopicModel> modelList = new List<TopicModel>();

        private List<CommentModel> commentList;

        private readonly ApiClient apiClient = new ApiClient();

        public MainView(TeamModel teamModel)
        {
            InitializeComponent();
            
            TeamModel = teamModel;
            TeamID.Text = TeamModel.ID;
            TeamName.Text = TeamModel.Name;
            teamKey.Text = TeamModel.TeamKey;
            GetTopicFromApi();
            InitComboBox();
        }

        public MainView()
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

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            this.Close();
        }

        private void addTopicBtn_Click(object sender, RoutedEventArgs e)
        {

            if (CheckTopicInput())
            {
                if (TopicExists())
                {

                    ErrorMessage.Text = "Topic name exists please enter new topic name.";
                }
                else
                {
                    CreateTopic();
                }
            }
            else
            {
                ErrorMessage.Text = "Please enter topic name.";
            }
        }

        private void TopicName_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TopicName.SelectedValue != null)
            {
                try
                {
                    int id = int.Parse(TopicName.SelectedValue.ToString());

                    if (id != 0)
                    {
                        TopicModel topic = modelList.Find(x => x.ID == id);

                        TopicIdBox.Text = topic.ID.ToString();
                        TopicNameBox.Text = topic.name;
                        TopicDateBox.Text = topic.createAt.ToString();

                        CommentIdBox.Text = "";
                        CommentDetailBox.Text = "";
                        CommentCreateDateBox.Text = "";

                        GetCommentFromApi();
                    }
                    else
                    {
                        TopicIdBox.Text = "";
                        TopicNameBox.Text = "";
                        TopicDateBox.Text = "";
                    }
                }
                catch (NullReferenceException)
                {
                    InitComboBox();
                }
            } else
            {
                GetTopicFromApi();
                AddItemToComboBox();
            }

            
        }

        private void TopicCommentCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {

                int id = int.Parse(TopicCommentCombo.SelectedValue.ToString());

                if (id != 0)
                {
                    CommentModel comment = commentList.Find(x => x.ID == id);

                    if (comment != null)
                    {
                        CommentIdBox.Text = comment.ID.ToString();
                        CommentDetailBox.Text = comment.Detail;
                        CommentCreateDateBox.Text = comment.CreateAt.ToString();

                        commentList = new List<CommentModel>();
                    }
                }
                else
                {
                    CommentIdBox.Text = "";
                    CommentDetailBox.Text = "";
                    CommentCreateDateBox.Text = "";
                }
            }
            catch (NullReferenceException)
            {
                CommentIdBox.Text = "";
                CommentDetailBox.Text = "";
                CommentCreateDateBox.Text = "";
            }
        }

        private void AddCommentBtn_Click(object sender, RoutedEventArgs e)
        {

            if (UserSelectedTopic())
            {
                if (UserEnterComment())
                {
                    CreateComment();
                }
                else
                {
                    MessageBox.Show("Please enter comment.");
                }
            }
            else
            {
                MessageBox.Show("Please select topic.");
            }


        }

        private void InitComboBox()
        {
            Dictionary<int, String> comboBoxinitValue = new Dictionary<int, String>();

            comboBoxinitValue.Add(0, "เลือกหัวข้อ");

            TopicName.ItemsSource = comboBoxinitValue;
            TopicName.SelectedValuePath = "Key";
            TopicName.DisplayMemberPath = "Value";
            TopicName.SelectedIndex = 0;
        }

        private async void GetTopicFromApi()
        {
            modelList = new List<TopicModel>();

            TopicName.ItemsSource = null;

            try
            {
                var response = await apiClient.GetTopic(TeamModel.ID);

                foreach (TopicModel topic in response)
                {
                    modelList.Add(topic);
                }

                AddItemToComboBox();
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Bad Request"))
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async void GetCommentFromApi()
        {
            String topicID = TopicIdBox.Text;

            commentList = new List<CommentModel>();

            TopicCommentCombo.ItemsSource = null;

            try
            {
                var response = await apiClient.GetComment(topicID);

                foreach (CommentModel comment in response)
                {
                    commentList.Add(comment);
                }

                AddCommentToComboBox();
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Bad Request"))
                {
                    TopicCommentCombo.ItemsSource = null;

                    Dictionary<String, String> list = new Dictionary<String, String>();

                    list.Add("0", "เลือก comment");

                    TopicCommentCombo.ItemsSource = list;
                    TopicCommentCombo.DisplayMemberPath = "Value";
                    TopicCommentCombo.SelectedValuePath = "Key";
                    TopicCommentCombo.SelectedIndex = 0;
                }
            }
        }

        private void AddItemToComboBox()
        {
            Dictionary<String, String> list = new Dictionary<String, String>();

            list.Add("0", "เลือกหัวข้อ");

            foreach (TopicModel topic in modelList)
            {
                list.Add(topic.ID.ToString(), topic.name);
            }

            TopicName.ItemsSource = list;
            TopicName.DisplayMemberPath = "Value";
            TopicName.SelectedValuePath = "Key";
            TopicName.SelectedIndex = 0;
        }

        private void AddCommentToComboBox()
        {
            TopicCommentCombo.ItemsSource = null;

            Dictionary<String, String> list = new Dictionary<String, String>();

            list.Add("0", "เลือก comment");

            foreach (CommentModel comment in commentList){
                list.Add(comment.ID.ToString(), comment.Detail);
            }

            TopicCommentCombo.ItemsSource = list;
            TopicCommentCombo.DisplayMemberPath = "Value";
            TopicCommentCombo.SelectedValuePath = "Key";
            TopicCommentCombo.SelectedIndex = 0;

        }

        private async void CreateTopic()
        {
            String topicName = newTopicNamebox.Text;
            String date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            await apiClient.CreateTopic(topicName, TeamModel.ID, date);
            ErrorMessage.Text = "";
            MessageBox.Show($"Create Topic: {topicName} \nDate: {date}");

            modelList = new List<TopicModel>();
            InitComboBox();
            GetTopicFromApi();

            TopicName.SelectedIndex = 0;
            AddItemToComboBox();

        }

        private async void CreateComment()
        {
            String newComment = newCommentBox.Text;
            String date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            String topicID = TopicName.SelectedValue.ToString();

            await apiClient.CreateComment(topicID, date, newComment);
            MessageBox.Show($"New Comment Added");

            GetCommentFromApi();
        }

        private bool CheckTopicInput()
        {
            bool check = false;

            String newTopicName = newTopicNamebox.Text;

            if (newTopicName.Length > 0)
            {
                check = true;
            }

            return check;
        }

        private bool TopicExists()
        {
            bool exists = false;
            String newTopicName = newTopicNamebox.Text;

            foreach (TopicModel model in modelList)
            {
                if (newTopicName.Equals(model.name))
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }

        private bool UserEnterComment()
        {
            bool enter = false;


            if (newCommentBox.Text.Length > 0)
                enter = true;


            return enter;
        }

        private bool UserSelectedTopic()
        {
            bool selected = false;

            if (!TopicName.SelectedValue.ToString().Equals("0"))
            {
                selected = true; ;
            }

            return selected;
        }

        
    }
}