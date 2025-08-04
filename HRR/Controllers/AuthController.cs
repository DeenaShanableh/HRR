using HRR.DTOs.Auth;
using HRR.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private HrDbContext _dbContext;

        public AuthController(HrDbContext dbContext)//Constructer
        {
            _dbContext = dbContext;
           

        }
        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto) 
        {
            //admin , Admin ,ADMIN == Admin
            var user = _dbContext.Users.FirstOrDefault(x => x.UserName.ToUpper() == loginDto.UserName.ToUpper());

            if(user == null)
            {
                return BadRequest("Invalid UserName");
            }
           // True --> Matching Passwords
           // !True --> False
           // False --> Not Matching Password
           // !False --> True
            if(!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.HashedPassword)) 
            {

                return BadRequest("Invalidb UserName Pr Password");
            }
            var token = GenerateJwtToken(user);
            return Ok(token);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>();//user Info
            //Key --> Value
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            
            // HR , Manager , Developer , Admin --> Roles
            if (user.IsAdmin) 
            {
                claims.Add(new Claim(ClaimTypes.Role,"Admin"));
            }
            else
            {
                var employee = _dbContext.Employees.Include(x => x.Lookup).FirstOrDefault(x => x.UserId== user.Id);
                claims.Add(new Claim(ClaimTypes.Role, employee.Lookup.Name));
            }

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("WHAFWEI#!@S!!112312WQEQW@RWQEQW432"));
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var tokenSettings = new JwtSecurityToken(
                claims: claims, // User Info
                expires: DateTime.Now.AddDays(1), // When Does The Token Expire
                signingCredentials : creds // Encryption Settings
                );
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenSettings);

           return token;
        }
    }
}
//eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTc1NDI2MTkwM30.cq8X_KzgcAQxtcKDicvJim84qF90KuhfgEkFARgSPxQ