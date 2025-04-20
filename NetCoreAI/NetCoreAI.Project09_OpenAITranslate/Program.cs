using Newtonsoft.Json;
using System.Text;

class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Lütfen çevirmek istediğiniz cümleyi giriniz:");
        string inputText = Console.ReadLine();

        string apiKey = "secret key";

        string translatedText = await TranslateTextToEnglish(inputText, apiKey);

        if (!string.IsNullOrEmpty(translatedText))
        {
            Console.WriteLine($"Çevirisi: {translatedText}");
        }
        else
        {
            Console.WriteLine("Çeviri yapılamadı.");
        }
    }

    private static async Task<string> TranslateTextToEnglish(string text, string apiKey)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new{role = "system",content = "You are a helpful translator."}, // modelin ne yapacağını belirten mesaj
                    new{role = "user",content = $"Please translate this text to English: {text}"}, // kullanıcının girdiği metin
                }
            };

            string jsonBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage responseMessage = await client.PostAsync("https://api.openai.com/v1/chat/completions", content); // API'ye POST isteği gönderme
                string responseString = await responseMessage.Content.ReadAsStringAsync();

                dynamic responseJson = JsonConvert.DeserializeObject(responseString); // API'den gelen yanıtı JSON'dan nesneye çevirme
                string translation = responseJson.choices[0].message.content; // çevirinin bulunduğu kısmı alıp geri döndürme

                return translation;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }
}