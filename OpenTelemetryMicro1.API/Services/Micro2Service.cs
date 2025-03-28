namespace OpenTelemetryMicro1.API.Services
{
    public class Micro2Service(HttpClient client)
    {
        public async Task<string> GetMicro2Data()
        {
            var response = await client.PostAsJsonAsync<Product>("https://localhost:7295/api/products",
                new Product(10, "Pencil 1", 20));
            return await response.Content.ReadAsStringAsync();
        }
    }
}