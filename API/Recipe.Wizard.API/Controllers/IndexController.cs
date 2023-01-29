using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.Wizard.Logic.Services.Interfaces;
using Recipe.Wizard.Logic.ViewModels;

namespace Recipe.Wizard.API.Controllers
{
    [Route("v1")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public IndexController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(RecipeGetResponse), 200)]
        public async Task<IActionResult> Index()
        {
            return Ok(await _recipeService.GetAsync());
        }

        [HttpPost("import")]
        [ProducesResponseType(typeof(RecipeImportResponse), 200)]
        public async Task<IActionResult> ImportAsync([FromForm] IFormFileCollection files)
        {
            return Ok(await _recipeService.ImportAsync(files));
        }

        [HttpGet("export")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportAsync()
        {
            var fileResult = await _recipeService.ExportAsync();

            if (fileResult == null)
                return BadRequest("Failed to generate export.");

            return fileResult;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(RecipeDeleteResponse), 200)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            return Ok(await _recipeService.DeleteAsync(id));
        }
    }
}