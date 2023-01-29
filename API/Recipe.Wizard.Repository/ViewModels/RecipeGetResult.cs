namespace Recipe.Wizard.Repository.ViewModels
{
    public class RecipeGetResult
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public List<StoreItemModel> Recipes { get; set; } = new();
    }
}