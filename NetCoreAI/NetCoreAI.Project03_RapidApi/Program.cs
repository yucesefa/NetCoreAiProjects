using NetCoreAI.Project03_RapidApi.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;

var client = new HttpClient();
List<ApiSeriesViewModel> apiSeriesViewModel = new List<ApiSeriesViewModel>();
var request = new HttpRequestMessage
{
    Method = HttpMethod.Get,
    RequestUri = new Uri("https://imdb-top-100-movies.p.rapidapi.com/series/"),
    Headers =
    {
        { "x-rapidapi-key", "08d4297573mshf4f79e4627fbc46p18f620jsnf86ecf342d7e" },
        { "x-rapidapi-host", "imdb-top-100-movies.p.rapidapi.com" },
    },
};
using (var response = await client.SendAsync(request))
{
    response.EnsureSuccessStatusCode();
    var body = await response.Content.ReadAsStringAsync();
    apiSeriesViewModel = JsonConvert.DeserializeObject<List<ApiSeriesViewModel>>(body);
    foreach (var item in apiSeriesViewModel)
    {
        Console.WriteLine(item.rank+"-"+item.title+"-Film Puanı:"+item.rating+"-Yapım Yılı:"+item.year);
    }
}
Console.ReadLine();