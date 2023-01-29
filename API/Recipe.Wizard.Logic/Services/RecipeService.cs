using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.Wizard.Logic.Services.Interfaces;
using Recipe.Wizard.Logic.ViewModels;
using Recipe.Wizard.Repository.Repositories.Interfaces;
using Recipe.Wizard.Repository.ViewModels;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;

namespace Recipe.Wizard.Logic.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;

        public RecipeService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeDeleteResponse> DeleteAsync(string id)
        {
            // Call repo method for deleting
            var deleteResult = await _recipeRepository.DeleteAsync(id);

            // Forward response
            return new()
            {
                Success = deleteResult.Success,
                Message = deleteResult.Message
            };
        }

        public async Task<FileStreamResult?> ExportAsync()
        {
            // Get list of recipes
            var result = await _recipeRepository.GetAsync();

            if (!result.Success)
                return null;

            // CSV Writer config
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };

            byte[]? bytes;

            // Cast list to model for writing to file
            var writeData = result.Recipes
                .Select(x => new RecipeExportItem
                {
                    Title = x.Title,
                    Timestamp = x.Timestamp,
                    Ingredients = string.Join(';', x.Ingredients),
                    Steps = string.Join(';', x.Steps),
                })
                .ToList();

            // Hold file in memory
            using (var memoryStream = new MemoryStream())
            {
                // Writer
                using (var streamWriter = new StreamWriter(memoryStream))

                // CSV Util package
                using (var csvWriter = new CsvWriter(streamWriter, config))
                {
                    // Write list to memory file
                    csvWriter.WriteRecords(writeData);
                }

                // Get bytes of memory file
                bytes = memoryStream.ToArray();
            }

            // Return file object for api
            return new FileStreamResult(new MemoryStream(bytes), "text/csv");
        }

        public async Task<RecipeGetResponse> GetAsync()
        {
            // Get full list of recipes
            var result = await _recipeRepository.GetAsync();

            if (!result.Success)
                return new() { Message = result.Message };

            // Cast repository result to response model
            return new()
            {
                Success = true,
                Message = result.Message,

                Recipes = result.Recipes.Select(x => new RecipeGetResponseData
                {
                    Id = x.Id,
                    Ingredients = x.Ingredients,
                    Steps = x.Steps,
                    Timestamp = x.Timestamp,
                    Title = x.Title
                }).ToList()
            };
        }

        public async Task<RecipeImportResponse> ImportAsync(IFormFileCollection files)
        {
            if (files.Count == 0)
                return new() { Message = "No files found." };

            List<StoreItemModel> data = new();

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];

                using (StreamReader sr = new(file.OpenReadStream()))
                {
                    string? currentLine;

                    // Skip first line
                    currentLine = sr.ReadLine();

                    // Loop through each line
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        // Get current line as string[]
                        string[] splitLine = currentLine.Split(",");

                        data.Add(new()
                        {
                            Id = Guid.NewGuid().ToString().ToLower().Replace("-", ""),
                            Title = splitLine[0],
                            Timestamp = DateTime.Parse(splitLine[1]),
                            Ingredients = splitLine[2].Split(";").ToList(),
                            Steps = splitLine[3].Split(";").ToList(),
                        });
                    }
                }
            }

            // call repo to add new items
            var addResult = await _recipeRepository.AddAsync(data);

            if (!addResult.Success)
                return new() { Message = addResult.Message };

            return new()
            {
                Success = true,
                Message = "Import successful."
            };
        }
    }
}