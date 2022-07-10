using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestWebApp.Entities;
using RestWebApp.Entities.Models;

namespace RestWebApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly RepositoryContext _context;
    
    public TokenController(IConfiguration configuration, RepositoryContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpGet]
    private async Task<User> Get(string name, string password)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Name == name && u.Password == password);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] User user)
    {
        var us = await Get(user.Name, user.Password);

        if (us == null)
            Unauthorized();
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: creds);
        
        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }
}