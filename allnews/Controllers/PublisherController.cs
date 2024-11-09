using allnews.Data;
using allnews.Models.DTOs;
using allnews.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace allnews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private static readonly string[] ValidPositions = { "opp", "center", "gov" };

        public PublisherController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        public IActionResult GetAllPublishers()
        {
            try
            {
                var publishers = dbContext.Publishers.ToList();
                return Ok(publishers);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetPublisherById(Guid id)
        {
            try
            {
                var publisher = dbContext.Publishers.Find(id);

                if (publisher == null)
                {
                    return NotFound();
                }

                return Ok(publisher);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddPublisher(PublisherDto publisherDto)
        {
            try
            {
                if(!ValidPositions.Contains(publisherDto.Position)){
                    throw new ArgumentException("Invalid position");
                }

                var publisherEntity = new Publisher()
                {
                    Name = publisherDto.Name,
                    Logo = publisherDto.Logo,
                    Position = publisherDto.Position,
                    TitleClass = publisherDto.TitleClass,
                    ArticleClass = publisherDto.ArticleClass,
                };

                dbContext.Publishers.Add(publisherEntity);
                dbContext.SaveChanges();

                return Ok(publisherEntity);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdatePublisher(Guid id, PublisherDto publisherDto)
        {
            try
            {
                var publisher = dbContext.Publishers.Find(id);

                if (publisher == null)
                {
                    return NotFound();
                }

                publisher.Name = publisherDto.Name;
                publisher.Logo = publisherDto.Logo;
                publisher.Position = publisherDto.Position;
                publisher.TitleClass = publisherDto.TitleClass;
                publisher.ArticleClass = publisherDto.ArticleClass;
                dbContext.SaveChanges();

                return Ok(publisher);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            try
            {
                var publisher = dbContext.Publishers.Find(id);

                if (publisher == null)
                {
                    return NotFound();
                }

                dbContext.Publishers.Remove(publisher);
                dbContext.SaveChanges();

                return Ok(new { message = "Publisher deleted successfully" });
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
