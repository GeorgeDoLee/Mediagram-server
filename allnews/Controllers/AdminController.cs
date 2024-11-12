using allnews.Data;
using allnews.Models.DTOs;
using allnews.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace allnews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public AdminController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        public IActionResult GetAllAdmins()
        {
            try
            {
                var admins = dbContext.Admins.ToList();
                return Ok(admins);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateAdmin(Guid id, AdminDto dto)
        {
            try
            {
                var existingAdmin = dbContext.Admins.Find(id);

                if (existingAdmin == null)
                {
                    return NotFound();
                }

                existingAdmin.Username = dto.Username;
                existingAdmin.PasswordHash = dto.PasswordHash;

                dbContext.SaveChanges();

                return Ok(existingAdmin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteAdmin(Guid id)
        {
            try
            {
                var admin = dbContext.Admins.Find(id);

                if (admin == null)
                {
                    return NotFound();
                }

                dbContext.Admins.Remove(admin);
                dbContext.SaveChanges();

                return Ok(new { message = "Admin deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
