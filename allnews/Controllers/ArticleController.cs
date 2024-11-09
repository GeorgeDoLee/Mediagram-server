using allnews.Data;
using allnews.Models.DTOs;
using allnews.Models.Entities;
using allnews.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                var articles = dbContext.Articles
                    .Select(a => new ArticleSummaryDto
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Photo = a.Photo,
                        OppCoverage = a.OppCoverage,
                        CenterCoverage = a.CenterCoverage,
                        GovCoverage = a.GovCoverage,
                        SubArticleCount = a.SubArticleCount
                    })
                    .ToList();

                return Ok(articles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetArticleById(Guid id)
        {
            try
            {
                var article = await dbContext.Articles
                    .Where(a => a.Id == id)
                    .Include(a => a.SubArticles)
                        .ThenInclude(sa => sa.Publisher)
                    .FirstOrDefaultAsync();

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
                return Ok(new { message = "Article deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostArticle(AddArticleDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Title) || dto.PublisherUrls == null || !dto.PublisherUrls.Any())
            {
                return BadRequest("Invalid input data.");
            }

            try
            {
                Guid articleId = Guid.NewGuid();
                int oppCount = 0, centerCount = 0, govCount = 0;
                var subArticles = new List<SubArticle>();

                foreach (var (publisherId, url) in dto.PublisherUrls)
                {
                    var publisher = dbContext.Publishers.Find(publisherId);
                    if (publisher == null)
                    {
                        return NotFound($"Publisher with ID {publisherId} not found.");
                    }

                    var scrapedData = await articleScraper.ScrapeArticle(url, publisher.TitleClass, publisher.ArticleClass);
                    if (scrapedData == null || string.IsNullOrEmpty(scrapedData.Title))
                    {
                        Console.WriteLine($"Failed to scrape article from URL: {url}");
                        continue;
                    }

                    var subArticle = new SubArticle
                    {
                        Url = url,
                        Title = scrapedData.Title,
                        PublisherId = publisherId,
                        ArticleId = articleId
                    };

                    subArticles.Add(subArticle);

                    switch (publisher.Position.ToLower())
                    {
                        case "opp":
                            oppCount++;
                            break;
                        case "center":
                            centerCount++;
                            break;
                        case "gov":
                            govCount++;
                            break;
                    }
                }

                var (oppCoverage, centerCoverage, govCoverage) = CoverageCalculator.CalculateCoverage(oppCount, centerCount, govCount);

                var article = new Article
                {
                    Id = articleId,
                    Title = dto.Title,
                    Photo = dto.Photo,
                    OppCoverage = oppCoverage,
                    CenterCoverage = centerCoverage,
                    GovCoverage = govCoverage,
                    SubArticleCount = oppCount + centerCount + govCount,
                    CategoryId = dto.CategoryId
                };

                dbContext.Articles.Add(article);
                dbContext.SubArticles.AddRange(subArticles);

                await dbContext.SaveChangesAsync();

                return Ok(article);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
