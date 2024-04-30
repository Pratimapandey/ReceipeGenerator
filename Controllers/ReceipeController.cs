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
        [HttpPost("create")]
        public IActionResult CreateRecipe([FromBody] Receipe recipeRequest)
        {
            try
            {
                // Create the recipe using the service
                var newRecipe = _recipeService.CreateRecipe(recipeRequest);
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
        public IActionResult GetIngredientsForRecipeTitle([FromQuery] string recipeTitle, [FromQuery] int servings)
        {
            if (string.IsNullOrEmpty(recipeTitle))
            {
                return BadRequest("Recipe title is required.");
            }

            try
            {
                var ingredients = _recipeService.GetIngredientsForRecipeTitle(recipeTitle, servings);
                return Ok(ingredients);
            }
            catch (Exception ex)
            {
                return NotFound($"Recipe with title '{recipeTitle}' not found. Error: {ex.Message}");
            }
        }


        [HttpPost("festival")]
        public IActionResult CreateFestival([FromBody] FestivalViewModel festivalData)
        {
            try
            {
                var result = _recipeService.CreateFestival(festivalData);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create festival: {ex.Message}");
            }
        }

        [HttpGet("festival/{festivalName}")]
        public IActionResult GetRecipesForFestival(string festivalName)
        {
            var festivalData = _recipeService.GetRecipesForFestival(festivalName);

            if (festivalData == null)
            {
                return NotFound($"Festival with name '{festivalName}' not found.");
            }

            return Ok(festivalData);
        }


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