using Newtonsoft.Json;
using System.Text;

class Program
{
    private static readonly string apiKey = "";
    static async Task Main(string[] args)
    {

        Console.Write("Metni Giriniz : ");
        string input;
        input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Ses dosyası oluşturuluyor....");
            await GenerateSpeech(input);
            Console.WriteLine("Ses dosyası oluşturuldu.");
            System.Diagnostics.Process.Start("explorer.exe","output.mp3");
        }

        static async Task GenerateSpeech(string text)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                var requestBody = new
                {
                    model = "tts-1",
                    input = text,
                    voice = "davinci" // fable, echo,onyx,nova,shimmer
                };

                string json = JsonConvert.SerializeObject(requestBody);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponseMessage = await client.PostAsync("https://api.openai.com/v1/audio/speech", content);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    byte[] audioBytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync("output.mp3", audioBytes);    
                }
                else
                {
                    Console.WriteLine("Hata oluştu : " + httpResponseMessage.ReasonPhrase);
                }
            }
        }
    }
}