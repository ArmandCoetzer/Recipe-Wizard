using Recipe.Wizard.Repository.ViewModels;
using System.Text.Json.Serialization;

namespace Recipe.Wizard.Logic.ViewModels
{
    public class RecipeGetResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; } = false;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("recipes")]
        public List<RecipeGetResponseData> Recipes { get; set; } = new();
    }

    public class RecipeGetResponseData
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