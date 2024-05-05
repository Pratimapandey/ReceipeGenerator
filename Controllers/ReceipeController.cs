using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceipeGenerator.Data;
using ReceipeGenerator.Model;
using ReceipeGenerator.Services.Implementation;
using ReceipeGenerator.Services.Interface;
using ReceipeGenerator.ViewModel;


namespace ReceipeGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceipeController : ControllerBase
    {
        private readonly IReceipeService _recipeService;
        private readonly ReceipeDbContext _context;

        public ReceipeController(IReceipeService recipeService, ReceipeDbContext context)
        {
            _recipeService = recipeService;
            _context = context;
        }
        [HttpGet("all-recipes")]
        public IActionResult GetAllRecipes()
        {
            try
            {
                // Eagerly load Categories related to each Receipe
                var recipes = _context.Receipes.Include(r => r.Categories).ToList();
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving recipes: {ex.Message}");
            }
        }


        [HttpPost("create-recipe")]
        public IActionResult CreateRecipe([FromBody] CreateRecipeRequest request)
        {
            try
            {
                // Create the recipe using the service
                var newRecipe = _recipeService.CreateRecipe(request);
                return Ok(newRecipe);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet("ingredients")]
        public IActionResult GetIngredientsForRecipeTitle([FromQuery] string recipeTitle, [FromQuery] string categoryName, [FromQuery] int servings)
        {
            if (string.IsNullOrEmpty(recipeTitle) || string.IsNullOrEmpty(categoryName))
            {
                return BadRequest("Recipe title and category name are required.");
            }

            try
            {
                var ingredients = _recipeService.GetIngredientsForRecipeTitle(recipeTitle, categoryName, servings);
                return Ok(ingredients);
            }
            catch (Exception ex)
            {
                return NotFound($"Recipe with title '{recipeTitle}' or category '{categoryName}' not found. Error: {ex.Message}");
            }
        }

        [HttpPut("recipes/{id}")]
        public IActionResult UpdateRecipe(int id, [FromBody] Receipe updatedRecipe)
        {
            try
            {
                updatedRecipe.ReceipeId = id; 
                var updatedRecipeResult = _recipeService.UpdateRecipe(updatedRecipe);
                if (updatedRecipeResult == null)
                {
                    return NotFound($"Recipe with ID '{id}' not found.");
                }
                return Ok(updatedRecipeResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the recipe: {ex.Message}");
            }
        }

        [HttpDelete("recipes/{id}")]
        public IActionResult DeleteRecipe(int id)
        {
            try
            {
                var isDeleted = _recipeService.DeleteRecipe(id);
                if (!isDeleted)
                {
                    return NotFound($"Recipe with ID '{id}' not found.");
                }
                return Ok("Recipe deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the recipe: {ex.Message}");
            }
        }
    }

}
