using Microsoft.AspNetCore.Mvc;
using ReceipeGenerator.Data;
using ReceipeGenerator.Services.Interface;
using ReceipeGenerator.ViewModel;
using Swashbuckle.AspNetCore.Annotations;

namespace ReceipeGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFestivalService _festivalService;
        private readonly IReceipeService _receipeService;
        private readonly ReceipeDbContext _context;

        public HomeController(IFestivalService festivalService, IReceipeService receipeService, ReceipeDbContext context)
        {
            _festivalService = festivalService;
            _receipeService = receipeService;
            _context = context;
        }

        /// <summary>
        /// Retrieve all festivals and recipes.
        /// </summary>
        /// <returns>A combined JSON response containing festivals and recipes.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve all festivals and recipes")]
        public IActionResult Index()
        {
            try
            {
                // Retrieve all festivals and recipes
                var festivals = _festivalService.GetAllFestivals();
                var recipes = _receipeService.GetAllRecipes();

                // Create a combined model to represent the data
                var homeData = new
                {
                    Festivals = festivals,
                    Recipes = recipes
                };

                // Return the combined data as a JSON response
                return Ok(homeData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
