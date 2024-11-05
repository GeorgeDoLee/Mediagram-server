using allnews.Data;
using allnews.Models.DTOs;
using allnews.Models.Entities;
using allnews.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace allnews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ArticleScraper articleScraper;

        public ArticleController(ApplicationDbContext context)
        {
            dbContext = context;
            articleScraper = new ArticleScraper();
        }

        [HttpGet]
        public IActionResult GetAllArticles()
        {
            try
            {
                var articles = dbContext.Articles.ToList();
                return Ok(articles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetArticleById(Guid id)
        {
            try
            {
                var article = dbContext.Articles.Find(id);
                if (article == null)
                {
                    return NotFound();
                }
                return Ok(article);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteArticle(Guid id)
        {
            try
            {
                var article = dbContext.Articles.Find(id);
                if (article == null)
                {
                    return NotFound();
                }

                dbContext.Articles.Remove(article);
                dbContext.SaveChanges();
                return Ok(new { message = "სტატია წაიშალა წარმატებით" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
