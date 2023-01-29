using System.Text.Json.Serialization;

namespace Recipe.Wizard.Logic.ViewModels
{
    public class RecipeExportItem
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.MinValue;

        [JsonPropertyName("ingredients")]
        public string Ingredients { get; set; } = string.Empty;

        [JsonPropertyName("steps")]
        public string Steps { get; set; } = string.Empty;
    }
}