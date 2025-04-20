
using System.Text;
using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        var apiKey = "secret key";
        Console.WriteLine("Lütfen Sorunuzu Yazınız:");

        var prompt = Console.ReadLine();
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var requestBody = new
        {
            model="gpt-3.5-turbo",
            messages = new[]
            {
                new {role="system", content="You are a helpful assistant."},
                new {role="user", content=prompt}
            },
            max_tokens = 100
        };

        var json=JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions",content);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<JsonElement>(responseString);
                var answer = result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString(); // seçimler içeirsindeki ilk mesajı getir 
                Console.WriteLine("Open AI'nin Cevabı:");
                Console.WriteLine(answer);
            }
            else
            {
                Console.WriteLine($"Hata Oluştu: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Bir Hata Oluştu: {ex.Message}");
        }
    }
}

