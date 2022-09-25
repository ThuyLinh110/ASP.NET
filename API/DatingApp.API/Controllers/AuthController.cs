using DatingApp.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Data;
using System.Security.Cryptography;
using DatingApp.API.Data.Entities;
using System.Text;

namespace DatingApp.API.Controllers
{

    public class AuthController : BaseController
    {

        
            private readonly DataContext _context;

            public AuthController(DataContext context)
            {
                _context = context;
            }

        
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthUserDto authUserDto)
        {
            authUserDto.Username = authUserDto.Username.ToLower();
            if (_context.AppUsers.Any(u => u.Username == authUserDto.Username))
            {
                return BadRequest("user is already existed"); 
            }
            using var hmac = new HMACSHA512();
            var passwordBytes = Encoding.UTF8.GetBytes(authUserDto.Password);
            var newUser = new User {
                Username = authUserDto.Username,
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(passwordBytes),
            };

            _context.AppUsers.Add(newUser);
            _context.SaveChanges();
            return Ok(newUser.Username);

        }

        [HttpPost("login")]
        public void Login([FromBody] string value)
        {

        }


    }
}
