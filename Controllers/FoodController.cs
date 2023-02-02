using DotNetCoreApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        public static readonly List<Food> Foods = new List<Food>()
        {
            new Food() { Id = "1", Name = "Pizza Pepperoni", Price = 10, ImageUrl = "assets/food-1.jpg", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged." },
            new Food() { Id = "2", Name = "Meatball", Price = 20, ImageUrl = "assets/food-2.jpg", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged." },
            new Food() { Id = "3", Name = "Hamburger", Price = 5, ImageUrl = "assets/food-3.jpg", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged." },
            new Food() { Id = "4", Name = "Fried Potatoes", Price = 2, ImageUrl = "assets/food-4.jpg", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged." },
            new Food() { Id = "5", Name = "Chicken Soup", Price = 11, ImageUrl = "assets/food-5.jpg", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged." },
            new Food() { Id = "6", Name = "Vegetables Pizza", Price = 9, ImageUrl = "assets/food-6.jpg", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged." },
        };
        // GET: api/<FoodController>
        [HttpGet("foods")]
        public ActionResult GetFoods()
        {
            return Ok(Foods);
        }

        // GET api/<FoodController>/foods/getFoodByFoodId/5
        [HttpGet("foods/getFoodByFoodId/{foodId}")]
        public ActionResult GetFood(string foodId)
        {
            Food? food = Foods.FirstOrDefault(food => food.Id == foodId);
            if (food == null)
            {
                return NotFound(string.Format("We cannot found food with id {0}", foodId));
            }

            return Ok(food);
        }
  
        // POST api/<FoodController>/foods/search
        [HttpGet("foods/search")]
        public ActionResult Search([FromQuery] FoodSearchCriteria criteria)
        {
            return Ok(Foods.Where(food => food.Name.Contains(criteria.Keyword)));
        }
    }
}
