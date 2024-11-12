using allnews.Data;
using allnews.Models.DTOs;
using allnews.Models.Entities;
using allnews.Services;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult GetAllArticles(bool? isBlindSpot = null, int pageNumber = 1, int pageSize = 10, bool? sortByTrending = null)
        {
            try
            {
                var skipCount = (pageNumber - 1) * pageSize;

                var articlesQuery = dbContext.Articles
                    .Where(a => !isBlindSpot.HasValue || a.IsBlindSpot == isBlindSpot.Value);

                if (sortByTrending.HasValue && sortByTrending.Value)
                {
                    articlesQuery = articlesQuery.OrderByDescending(a => a.TrendingScore);
                }
                else
                {
                    articlesQuery = articlesQuery.OrderByDescending(a => a.UploadDate);
                }

                var totalArticles = articlesQuery.Count();

                var articles = articlesQuery
                    .Skip(skipCount)
                    .Take(pageSize)
                    .Select(a => new ArticleSummaryDto
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Photo = a.Photo,
                        OppCoverage = a.OppCoverage,
                        CenterCoverage = a.CenterCoverage,
                        GovCoverage = a.GovCoverage,
                        SubArticleCount = a.SubArticleCount,
                        IsBlindSpot = a.IsBlindSpot,
                        CategoryName = a.Category.Name
                    })
                    .ToList();

                bool hasNextPage = (skipCount + pageSize) < totalArticles;

                var result = new
                {
                    Articles = articles,
                    HasNextPage = hasNextPage
                };

                return Ok(result);
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
                    .Include(a => a.Category)
                    .Include(a => a.SubArticles)
                        .ThenInclude(sa => sa.Publisher)
                    .FirstOrDefaultAsync();

                if (article == null)
                {
                    return NotFound();
                }

                article.Category.TrendingScore++;
                article.TrendingScore++;

                dbContext.Articles.Update(article);
                dbContext.Categories.Update(article.Category);
                await dbContext.SaveChangesAsync();

                return Ok(article);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
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
        [Authorize]
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
                        Console.WriteLine($"Publisher with ID {publisherId} not found.");
                        continue;
                    }

                    ScrapedArticle scrapedArticle;
                    try
                    {
                        scrapedArticle = await articleScraper.ScrapeArticle(url, publisher.ArticleClass);
                        if (scrapedArticle == null || string.IsNullOrEmpty(scrapedArticle.Title))
                        {
                            Console.WriteLine($"Failed to scrape article from URL: {url}");
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error scraping {url}: {ex.Message}");
                        continue;
                    }

                    subArticles.Add(new SubArticle
                    {
                        Url = url,
                        Title = scrapedArticle.Title,
                        PublisherId = publisherId,
                        ArticleId = articleId
                    });

                    switch (publisher.Position.ToLowerInvariant())
                    {
                        case "opp": oppCount++; break;
                        case "center": centerCount++; break;
                        case "gov": govCount++; break;
                    }
                }

                var (oppCoverage, centerCoverage, govCoverage) = CoverageCalculator.CalculateCoverage(oppCount, centerCount, govCount);
                bool isBlindSpot = oppCoverage > 70 || govCoverage > 70;

                var article = new Article
                {
                    Id = articleId,
                    Title = dto.Title,
                    Photo = dto.Photo,
                    OppCoverage = oppCoverage,
                    CenterCoverage = centerCoverage,
                    GovCoverage = govCoverage,
                    SubArticleCount = subArticles.Count,
                    CategoryId = dto.CategoryId,
                    IsBlindSpot = isBlindSpot
                };

                dbContext.Articles.Add(article);
                dbContext.SubArticles.AddRange(subArticles);
                await dbContext.SaveChangesAsync();

                return Ok(article);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
