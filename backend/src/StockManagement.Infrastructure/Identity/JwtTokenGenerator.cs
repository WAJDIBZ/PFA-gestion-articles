using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Domain.Entities;

namespace StockManagement.Infrastructure.Identity;

public class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{
    public string GenererToken(ApplicationUser utilisateur, IList<string> roles)
    {
        var cle = configuration["Jwt:Cle"] ?? throw new InvalidOperationException("Jwt:Cle manquante dans la configuration.");
        var emetteur = configuration["Jwt:Emetteur"];
        var audience = configuration["Jwt:Audience"];
        var dureeMinutes = int.TryParse(configuration["Jwt:DureeMinutes"], out var d) ? d : 480;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, utilisateur.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, utilisateur.Email ?? string.Empty),
            new(ClaimTypes.NameIdentifier, utilisateur.Id.ToString()),
            new(ClaimTypes.Name, utilisateur.NomComplet),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cle)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: emetteur,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(dureeMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
