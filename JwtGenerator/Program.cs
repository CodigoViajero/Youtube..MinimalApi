using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var jwtHandler = new JwtSecurityTokenHandler();
var privateKey = Encoding.UTF8.GetBytes("My secret key - No usar de esta manera en ambiente productivo");

var credentials = new SigningCredentials(
    new SymmetricSecurityKey(privateKey),
    SecurityAlgorithms.HmacSha256
);

var tokenDescriptor = new SecurityTokenDescriptor
{
    SigningCredentials = credentials,
    Expires = DateTime.UtcNow.AddHours(1),
    Audience = "Product",
    Issuer = "http://localhost:5000",
    Subject = new ClaimsIdentity(
        new []
        {
            new Claim("Id", Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, "viajero"),
            new Claim(JwtRegisteredClaimNames.Email, "viajero.ejemplo.com"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        }
    )
};

var token = jwtHandler.CreateToken(tokenDescriptor);

Console.WriteLine("Token => ");
Console.WriteLine(jwtHandler.WriteToken(token));
