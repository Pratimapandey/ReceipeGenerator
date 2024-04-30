using Microsoft.AspNetCore.Mvc;
using ReceipeGenerator.Data;
using ReceipeGenerator.Model;
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


        [HttpGet("all")]
        public IActionResult GetAllRecipes()
        {
            try
            {
                var recipes = _recipeService.GetAllRecipes();
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving recipes: {ex.Message}");
            }
        }

        [HttpPost("create")]
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
                updatedRecipe.ReceipeId = id; // Ensure the ID is set correctly
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






        //[HttpPost("festival")]
        //public IActionResult CreateFestival([FromBody] FestivalViewModel festivalData)
        //{
        //    try
        //    {
        //        var result = _recipeService.CreateFestival(festivalData);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Failed to create festival: {ex.Message}");
        //    }
        //}

        //[HttpGet("festival/{festivalName}")]
        //public IActionResult GetRecipesForFestival(string festivalName)
        //{
        //    var festivalData = _recipeService.GetRecipesForFestival(festivalName);

        //    if (festivalData == null)
        //    {
        //        return NotFound($"Festival with name '{festivalName}' not found.");
        //    }

        //    return Ok(festivalData);
        //}


        //[HttpGet("recipes")]
        //public IActionResult GetRecipesForFestival(string festivalName)
        //{
        //    var recipes = _context.ReceipeFestivals
        //        .Where(rf => rf.Festival.Name == festivalName)
        //        .Select(rf => rf.Receipe)
        //        .ToList();

        //    return Ok(recipes);
        //}
        //[HttpPost("recipes/{festivalName}")]
        //public IActionResult PostRecipesForFestival(string festivalName, [FromBody] List<string> recipeNames)
        //{
        //    if (string.IsNullOrEmpty(festivalName) || recipeNames == null || recipeNames.Count == 0)
        //    {
        //        return BadRequest("Festival name and recipe names are required.");
        //    }

        //    var result = _recipeService.PostRecipesForFestival(festivalName, recipeNames);

        //    if (!result)
        //    {
        //        return BadRequest("Failed to post recipes for the festival.");
        //    }

        //    return Ok("Recipes posted for the festival successfully.");
        //}




    }
}