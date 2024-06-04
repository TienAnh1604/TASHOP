using Newtonsoft.Json;

namespace FreshMarketMVC.Areas.API.Request
{
    public class ProductRequest
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Discription")]
        public string Discription { get; set; }
        [JsonProperty("Quantity")]
        public string Quantity { get; set; }
        [JsonProperty("Price")]
        public string Price { get; set; }
        [JsonProperty("CategoryId")]
        public string CategoryId { get; set; }
        [JsonProperty("BrandId")]
        public string BrandId { get; set; }
        [JsonProperty("UnitId")]
        public string UnitId { get; set; }
    }
}
