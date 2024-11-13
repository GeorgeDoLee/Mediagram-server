using Mediagram.Data;
using Mediagram.Models.DTOs;
using Mediagram.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mediagram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            try
            {
                var categories = dbContext.Categories
                    .OrderByDescending(c => c.TrendingScore)
                    .ToList();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetCategoryById(Guid id)
        {
            try
            {
                var category = dbContext.Categories.Find(id);

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddCategory(CategoryDto categoryDto)
        {
            try
            {
                var categoryEntity = new Category { Name = categoryDto.Name };

                dbContext.Categories.Add(categoryEntity);
                dbContext.SaveChanges();

                return Ok(categoryEntity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public IActionResult UpdateCategory(Guid id, CategoryDto categoryDto)
        {
            try
            {
                var category = dbContext.Categories.Find(id);

                if (category == null)
                {
                    return NotFound();
                }

                category.Name = categoryDto.Name;

                dbContext.SaveChanges();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public IActionResult DeleteCategory(Guid id)
        {
            try
            {
                Console.WriteLine("11 22 33");
                var category = dbContext.Categories.Find(id);

                if (category == null)
                {
                    return NotFound();
                }

                dbContext.Categories.Remove(category);
                dbContext.SaveChanges();

                return Ok(new { message = "Category deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
