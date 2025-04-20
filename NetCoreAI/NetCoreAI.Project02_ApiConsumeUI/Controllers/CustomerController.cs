using Microsoft.AspNetCore.Mvc;
using NetCoreAI.Project02_ApiConsumeUI.Dtos;
using Newtonsoft.Json;
using System.Text;

namespace NetCoreAI.Project02_ApiConsumeUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> CustomerList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7067/api/Cutomers");
            if (responseMessage.IsSuccessStatusCode)
            {
                var data = await responseMessage.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<ResultCustomerDto>>(data);
                return View(customers);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> CreateCustomer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(createCustomerDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7067/api/Cutomers", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("CustomerList");
            }
            return View();
        }
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync("https://localhost:7067/api/Customers?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {  
                return RedirectToAction("CustomerList");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7067/api/Cutomers/GetCustomer?id="+id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var data = await responseMessage.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<GetByIdCustomerDto>(data);
                return View(customer);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDto updateCustomerDto)
        {
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(updateCustomerDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7067/api/Cutomers", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("CustomerList");
            }
            return View();
        }
    }
}
