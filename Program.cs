
using System;
using System.Runtime.CompilerServices;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Xml.Linq;
using System.Text.Json.Serialization;

namespace TestConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync("https://raw.githubusercontent.com/Serempre/test-json/main/list1.json");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();

                        List<Test> testList = JsonSerializer.Deserialize<List<Test>>(responseContent);

                        if (testList != null)
                        {
                            testList.Sort((t1, t2) => t2.Kpi.Speed - t1.Kpi.Speed);

                            foreach (var test in testList)
                            {
                                Console.WriteLine(test.Name.ToString() + " speed = " + test.Kpi.Speed.ToString());
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public class Test
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("kpi")]
            public Kpi Kpi { get; set; }

            public Test (int id, string name, Kpi kpi)
            {
                Id = id;
                Name = name;
                Kpi = kpi;
            }
        }

        public class Kpi
        {
            [JsonPropertyName("speed")]
            public int Speed { get; set; }
            [JsonPropertyName("level")]
            public int Level { get; set; }

            public Kpi(int speed, int level)
            {
                Speed = speed;
                Level = level;
            }
        }
    }
}