using Newtonsoft.Json;

namespace DependabotTest.Library;

public class Product
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }
}
