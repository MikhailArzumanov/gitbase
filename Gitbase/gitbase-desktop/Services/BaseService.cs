using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace gitbase_desktop.Services {
    public class BaseService {
        protected const string SERVICE_URL = "https://gitbase.ru/api";
        protected HttpClient http = new HttpClient();

        protected async Task<string> Get(string url) {
            using (var response = await http.GetAsync(url)) {
                if (response.StatusCode != HttpStatusCode.OK) {
                    var resp = await response.Content.ReadAsStringAsync();
                    return null;
                }
                else {
                    response.EnsureSuccessStatusCode();
                    var resp = await response.Content.ReadAsStringAsync();
                    return resp;
                }
            }
        }
    }
}
