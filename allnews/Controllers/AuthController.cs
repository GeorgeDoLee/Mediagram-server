using allnews.Data;
using allnews.Models.DTOs;
using allnews.Models.Entities;
using allnews.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace allnews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly PasswordHasher<Admin> _passwordHasher;

        public AuthController(ApplicationDbContext context)
        {
            dbContext = context;
            _passwordHasher = new PasswordHasher<Admin>();
        }

        [HttpPost("register")]
        public IActionResult Register(AdminDto dto)
        {
            try
            {
                var existingAdmin = dbContext.Admins.FirstOrDefault(a => a.Username == dto.Username);

                if (existingAdmin != null)
                {
                    return BadRequest("Username already taken");
                }

                var adminEntity = new Admin()
                {
                    Username = dto.Username,
                    PasswordHash = _passwordHasher.HashPassword(null, dto.Password)
                };

                dbContext.Admins.Add(adminEntity);
                dbContext.SaveChanges();


                return Ok(new { message = "Admin created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login(AdminDto dto)
        {
            try
            {
                var admin = dbContext.Admins.FirstOrDefault(a => a.Username == dto.Username);

                if (admin == null)
                {
                    return Unauthorized("Invalid credentials");
                }

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, dto.Password);

                if (passwordVerificationResult == PasswordVerificationResult.Failed)
                {
                    return Unauthorized("Invalid credentials");
                }

                var token = TokenGenerator.Generate(admin);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
