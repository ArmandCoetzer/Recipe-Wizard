using Recipe.Wizard.Repository.ViewModels;

namespace Recipe.Wizard.Repository.Repositories.Interfaces
{
    public interface IRecipeRepository
    {
        Task<RecipeGetResult> GetAsync();

        Task<RecipeDeleteResult> DeleteAsync(string id);

        Task<RecipeAddResult> AddAsync(List<StoreItemModel> model);
    }
}