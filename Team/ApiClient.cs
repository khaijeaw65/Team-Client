using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Team.Models;

namespace Team
{
    class ApiClient
    {
        private HttpClient apiClient;

        public ApiClient()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            String api = "http://localhost:8000";

            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<UserModel>> GetUserData()
        {

            using (HttpResponseMessage response = await apiClient.GetAsync("/user/get"))
            {
                if (response.IsSuccessStatusCode)
                {

                    var resp = await response.Content.ReadAsAsync<List<UserModel>>();

                    return resp;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<UserModel> Login(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("/user/login", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var resp = await response.Content.ReadAsAsync<UserModel>();
                    return resp;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<UserModel> GetLoginUserInfo(string id)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<String, String>("userID", id)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("/user/getById", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var resp = await response.Content.ReadAsAsync<UserModel>();
                    return resp;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<String> Register(String username, String password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("/user/add", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var resp = await response.Content.ReadAsStringAsync();
                    return resp;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<TeamModel>> GetTeam()
        {
            using (HttpResponseMessage response = await apiClient.GetAsync("/team/get"))
            {
                if (response.IsSuccessStatusCode)
                {

                    var resp = await response.Content.ReadAsAsync<List<TeamModel>>();

                    return resp;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<String> CreateTeam(String name, String key)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<String, String>("teamName", name),
                new KeyValuePair<String, String>("teamKey", key)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("/team/create", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var resp = await response.Content.ReadAsStringAsync();
                    return resp;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<TopicModel>> GetTopic(String teamID)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("teamID", teamID)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync($"topic/get/", data))
            {
                if (response.IsSuccessStatusCode)
                {

                    var resp = await response.Content.ReadAsAsync<List<TopicModel>>();

                    return resp;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<String> CreateTopic(String name, String teamID, String date)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<String, String>("topicName", name),
                new KeyValuePair<String, String>("postDate", date),
                new KeyValuePair<String, String>("teamID", teamID)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("/topic/create", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var resp = await response.Content.ReadAsStringAsync();
                    return resp;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<CommentModel>> GetComment(String topicID)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("topicID", topicID)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync($"comment/get/", data))
            {
                if (response.IsSuccessStatusCode)
                {

                    var resp = await response.Content.ReadAsAsync<List<CommentModel>>();

                    if (resp == null)
                    {
                        throw new NullReferenceException();
                    }else
                    {
                        return resp;
                    }
                    
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<String> CreateComment(String topicID, String date, String detail)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<String, String>("topicID", topicID),
                new KeyValuePair<String, String>("detail", detail),
                new KeyValuePair<String, String>("date", date)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("/comment/create", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var resp = await response.Content.ReadAsStringAsync();
                    return resp;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
