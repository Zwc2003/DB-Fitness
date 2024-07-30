using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.BLL
{
    public class Qwen_VL
    {
        private static readonly HttpClient client =new();
        

        

        public static async Task<string> CallQWen(string prompt, string imgUrl)
        {

            var requestBody = new
            {
                model = "qwen-vl-plus",
                input = new
                {
                    messages = new[]
                        {
                        new
                        {
                            role = "user",
                            content = new object[]
                            {
                                new { image = imgUrl },
                                new { text = prompt }
                            }
                        }
                    }
                },
                parameters = new { }
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = content
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            try
            {
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);

                // 解析返回的JSON字符串
                var jsonResponse = JObject.Parse(responseString);
                var contentResult = jsonResponse["output"]["choices"][0]["message"]["content"][0]["text"].ToString();
                return contentResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"请求失败: {ex.Message}");
                return null;
            }
        }





    }
}
