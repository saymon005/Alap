using Newtonsoft.Json;
using System.Text;

namespace Alap.Services
{
    public class TavilyService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public TavilyService(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<string> GetBotResponse(string query)
        {
            var apiKey = _config["Tavily:ApiKey"];
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.tavily.com/search");
            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            var body = new { query = query };
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(json);

            string answer = result?.answer;
            if (string.IsNullOrEmpty(answer) && result?.results != null && result.results.Count > 0)
            {
                answer = result.results[0].content;
            }

            return answer ?? "Sorry, I couldn't find a good answer.";
        }

    }

}
