using System.Text.Json.Serialization;

namespace Recipe.Wizard.Repository.ViewModels
{
    public class StoreItemModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.MinValue;

        [JsonPropertyName("ingredients")]
        public List<string> Ingredients { get; set; } = new();

        [JsonPropertyName("steps")]
        public List<string> Steps { get; set; } = new();
    }
}