using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.Wizard.Logic.ViewModels;

namespace Recipe.Wizard.Logic.Services.Interfaces
{
    public interface IRecipeService
    {
        Task<RecipeGetResponse> GetAsync();

        Task<RecipeImportResponse> ImportAsync(IFormFileCollection file);

        Task<FileStreamResult?> ExportAsync();

        Task<RecipeDeleteResponse> DeleteAsync(string id);
    }
}