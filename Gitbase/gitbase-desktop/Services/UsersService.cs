using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using gitbase_desktop.Data;
using gitbase_desktop.Models;

namespace gitbase_desktop.Services {
    public class UsersService : AuthorizedService {
        private const string CONTROLLER_URL = SERVICE_URL + "/Users";
        public UsersService(ApplicationContext context) : base(context) { }
        public async Task<bool> Authorize(string authname, string password) {
            var url = $"{CONTROLLER_URL}/auth/authorize";
            
            var bodyData = new { Authname = authname, Password = password };
            var bodyJson = JsonSerializer.Serialize(bodyData);
            var jsonContent = new StringContent(bodyJson);
            
            var httpMessage = new HttpRequestMessage(HttpMethod.Post, url);
            httpMessage.Content = jsonContent;
            httpMessage.Headers.Add("Accept", "application/json");
            var response = await http.SendAsync(httpMessage);
            if (response.IsSuccessStatusCode) { 
                var responseText = await response.Content.ReadAsStringAsync();
                var token = JsonSerializer.Deserialize<TokenResponse>(responseText);
                context.AuthorizationToken =  new Token {
                    Self = token.Token,
                    UserId = token.User.Id
                };
                context.CurrentUser = token.User;
                return true;
            } else {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception(errorText);
            }
        }
        public class TokenResponse {
            public string Token { get; set; } = String.Empty;
            public User   User  { get; set; } = null;
        }
    }
}
