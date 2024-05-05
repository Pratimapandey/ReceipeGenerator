using Microsoft.AspNetCore.Mvc;
using ReceipeGenerator.Data;
using ReceipeGenerator.Model;
using ReceipeGenerator.Services.Implementation;
using ReceipeGenerator.Services.Interface;
using ReceipeGenerator.ViewModel;
using System;
using Microsoft.EntityFrameworkCore;

namespace ReceipeGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FestivalController : ControllerBase
    {
        private readonly FestivalService _festivalService;
        private readonly ReceipeDbContext _context;

        public FestivalController(FestivalService festivalService, ReceipeDbContext context)
        {
            _festivalService = festivalService;
            _context = context;
        }

        [HttpGet("all-festivals")]
        public IActionResult GetAllFestivals()
        {
            try
            {
                var festivals = _context.Festivals.Include(f => f.Recipes).ThenInclude(r => r.Categories).ThenInclude(c => c.Ingredients).ToList();
                return Ok(festivals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving festivals: {ex.Message}");
            }
        }


        [HttpPost("create-festival")]
        public IActionResult CreateFestival([FromBody] CreateFestivalRequest request)
        {
            try
            {
                var result = _festivalService.CreateFestival(request);
                if (result)
                {
                    return Ok("Festival created successfully.");
                }
                else
                {
                    return BadRequest("Failed to create festival.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }


        [HttpGet("recipes-festival")]
        public IActionResult GetRecipesAndIngredientsByFestivalName([FromQuery] string festivalName, [FromQuery] int servings)
        {
            try
            {
                var festivalViewModel = _festivalService.GetRecipesAndIngredientsByFestivalName(festivalName, servings);
                if (festivalViewModel != null)
                {
                    return Ok(festivalViewModel);
                }
                else
                {
                    return NotFound($"Festival with name '{festivalName}' not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }


    }
}