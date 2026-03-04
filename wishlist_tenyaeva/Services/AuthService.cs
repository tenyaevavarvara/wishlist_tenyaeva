using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using wishlist_tenyaeva.Data;
using wishlist_tenyaeva.DTO;
using wishlist_tenyaeva.Interfaces;
using wishlist_tenyaeva.Models;

namespace wishlist_tenyaeva.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Проверяем, не занят ли email
            if (await UserExistsAsync(registerDto.Email))
                throw new InvalidOperationException("Пользователь с таким email уже существует");

            // Создаем нового пользователя
            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password), // Хешируем пароль
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Генерируем токен
            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // Ищем пользователя по email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Неверный email или пароль");

            // Проверяем пароль
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Неверный email или пароль");

            // Генерируем токен
            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(7);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}