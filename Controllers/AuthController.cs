using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MultiTenantDbContext.Data.Admin;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultiTenantDbContext.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AdminDbContext _adminDbContext;

    public AuthController(AdminDbContext adminDbContext)
    {
        _adminDbContext = adminDbContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromQuery] string user)
    {
        var userInDb = await _adminDbContext.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == user);
        if (userInDb == null)
        {
            return Unauthorized();
        }
        var token = GenerateJwtToken(userInDb.Name, userInDb.CustomerId);
        return Ok(new { token });
    }

    private string GenerateJwtToken(string userName, Guid customerId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var customerIdClaim = new Claim("CustomerId", customerId.ToString());
        var userNameClaim = new Claim(ClaimTypes.Name, userName);

        var token = new JwtSecurityToken(
            issuer: "your_issuer",
            audience: "your_audience",
            claims: new List<Claim> { userNameClaim, customerIdClaim },
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}