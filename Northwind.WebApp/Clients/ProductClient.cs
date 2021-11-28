using Northwind.WebApp.Common;
using Northwind.WebApp.Entities;
using Northwind.WebApp.Entities.Dtos;
using Northwind.WebApp.Utilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Northwind.WebApp.Clients
{
    public class ProductClient
    {
        private readonly HttpClient _client;
        public ProductClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(CommonInfo.BaseAddress);
        }

        public async Task<Result<List<Product>>> GetProducts()
        {
            UserLoginDto loginInfo = new UserLoginDto();
            loginInfo.Email = "test@email.com";
            loginInfo.Password = "123456";
            var dataAsString = JsonConvert.SerializeObject(loginInfo);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync("Auth/login", content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AccessToken>(responseData);
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + result.Token);
                var responseProduct = await _client.GetAsync("/Products");
                if (responseProduct.IsSuccessStatusCode)
                {
                    var responseProductData = await responseProduct.Content.ReadAsStringAsync();
                    var resultProduct = JsonConvert.DeserializeObject<List<Product>>(responseProductData);
                    if (responseProductData.Any())
                        return new Result<List<Product>>(true, "Kayit Bulundu", resultProduct.ToList());
                    
                }
                return new Result<List<Product>>(false, "Kayit bulunamadi");
            }
            return new Result<List<Product>>(false, "Kayit bulunamadi");
        }


    }
}