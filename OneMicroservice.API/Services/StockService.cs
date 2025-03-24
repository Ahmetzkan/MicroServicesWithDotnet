using OneMicroservice.API.Models;
using System.Text.Json;

namespace OneMicroservice.API.Services
{
    public class StockService(HttpClient client)
    {
        public async Task<int> GetStockCount()
        {
            var response = await client.GetAsync("/api/Stock/GetStockCount");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<GetStockCountResponse>();

                return content.Count;
            }

            return 0;
        }
    }
}