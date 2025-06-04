using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleProject.Data;
using SampleProject.Models;
using SampleProject.DTOs;
using System.Security.Cryptography;
using System.Text;
using Sampleproject.DTO;

namespace SampleProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EduSyncContext _context;

        public UsersController(EduSyncContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new GetUserDTO
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDTO>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            var dto = new GetUserDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(dto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<GetUserDTO>> Login([FromBody] LoginDTO loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var hashedInputPassword = HashPassword(loginDto.Password);
            if (user.PasswordHash != hashedInputPassword)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var getDto = new GetUserDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(getDto);
        }

        // POST: api/Users
        [HttpPost("register")]
        public async Task<ActionResult<GetUserDTO>> PostUser(CreateUserDTO dto)
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Role = dto.Role,
                PasswordHash = HashPassword(dto.Password)
            };

            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserId))
                    return Conflict();
                else
                    throw;
            }

            var getDto = new GetUserDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, getDto);
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, PutUserDTO dto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Role = dto.Role;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = HashPassword(dto.Password);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }



        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
