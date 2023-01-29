using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Recipe.Wizard.Repository.Repositories.Interfaces;
using Recipe.Wizard.Repository.ViewModels;

namespace Recipe.Wizard.Repository.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly string _storePath;

        public RecipeRepository(IHostingEnvironment hostingEnvironment)
        {
            _storePath = Path.Combine(hostingEnvironment.WebRootPath, "store.json");
        }

        public async Task<RecipeAddResult> AddAsync(List<StoreItemModel> model)
        {
            try
            {
                // Get full recipe list
                var getResult = await GetAsync();

                if (!getResult.Success)
                    return new() { Message = getResult.Message };

                var data = getResult.Recipes;

                // Add new recipes to orignal list
                data.AddRange(model);

                // Rewrite file
                await UpdateAsync(data);

                return new() { Success = true, Message = "Recipes added." };
            }
            catch (Exception ex)
            {
                return new() { Message = $"Exception: {ex.Message}" };
            }
        }

        public async Task<RecipeDeleteResult> DeleteAsync(string id)
        {
            // Get list of recipes
            var getResult = await GetAsync();

            if (!getResult.Success)
                return new() { Message = getResult.Message };

            var data = getResult.Recipes;

            // Find item to delete
            var item = data
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (item == null)
                return new() { Message = "Recipe not found." };

            // Remove item from list
            bool removed = data.Remove(item);

            if (!removed)
                return new() { Message = "Item not found." };

            // Rewrite file
            await UpdateAsync(data);

            return new()
            {
                Success = true,
                Message = "Recipe deleted."
            };
        }

        public async Task<RecipeGetResult> GetAsync()
        {
            try
            {
                // Read file as string
                string jsonData = await File.ReadAllTextAsync(_storePath);

                if (string.IsNullOrEmpty(jsonData))
                    return new() { Message = "File is empty." };

                // Deserialize into model
                var deserializedData = JsonConvert.DeserializeObject<List<StoreItemModel>>(jsonData);

                if (deserializedData == null)
                    return new() { Message = "No data." };

                return new()
                {
                    Success = true,
                    Message = "Recipes retrieved.",
                    Recipes = deserializedData
                };
            }
            catch (Exception ex)
            {
                return new() { Message = $"Exception: {ex.Message}" };
            }
        }

        private async Task UpdateAsync(List<StoreItemModel> data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            await File.WriteAllTextAsync(_storePath, jsonData);
        }
    }
}