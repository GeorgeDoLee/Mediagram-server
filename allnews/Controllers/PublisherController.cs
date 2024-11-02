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
        private static readonly string[] ValidPositions = { "left", "center", "right" };

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
        public IActionResult AddPublisher(AddPublisherDto addPublisherDto)
        {
            try
            {
                if(!ValidPositions.Contains(addPublisherDto.Position)){
                    throw new ArgumentException("Invalid position");
                }

                var publisherEntity = new Publisher()
                {
                    Name = addPublisherDto.Name,
                    Logo = addPublisherDto.Logo,
                    Position = addPublisherDto.Position,
                    TitleClass = addPublisherDto.TitleClass,
                    ArticleClass = addPublisherDto.ArticleClass,
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
        public IActionResult UpdatePublisher(Guid id, UpdatePublisherDto updatePublisherDto)
        {
            try
            {
                var publisher = dbContext.Publishers.Find(id);

                if (publisher == null)
                {
                    return NotFound();
                }

                publisher.Name = updatePublisherDto.Name;
                publisher.Logo = updatePublisherDto.Logo;
                publisher.Position = updatePublisherDto.Position;
                publisher.TitleClass = updatePublisherDto.TitleClass;
                publisher.ArticleClass = updatePublisherDto.ArticleClass;
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

                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
