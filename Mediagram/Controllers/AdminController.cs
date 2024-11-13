using Mediagram.Data;
using Mediagram.Models.DTOs;
using Mediagram.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mediagram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly PasswordHasher<Admin> _passwordHasher;

        public AdminController(ApplicationDbContext context)
        {
            dbContext = context;
            _passwordHasher = new PasswordHasher<Admin>();
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
                existingAdmin.PasswordHash = _passwordHasher.HashPassword(null, dto.Password);

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
