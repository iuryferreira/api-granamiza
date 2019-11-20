using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIGranamiza.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace APIGranamiza.Utils
{

    public static class Token
    {
        public static string GerarToken(Usuario usuario)
        {
            var chaveJson = Encoding.ASCII.GetBytes(Startup.StaticConfiguration["chaveJson"]);
            var claims = new ClaimsIdentity( new Claim[]{
                    new Claim(ClaimTypes.Name, usuario.Email)
            });
            var tempoExpiracao = DateTime.UtcNow.AddHours(2);
            var tokenHandler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey chave = GerarChaveSimetrica();
            SigningCredentials credenciais = CriarCredenciais(chave);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor{
                Subject = claims,
                Expires = tempoExpiracao,
                SigningCredentials = credenciais
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private static SigningCredentials CriarCredenciais(SymmetricSecurityKey chave)
        {
            return new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
        }

        private static SymmetricSecurityKey GerarChaveSimetrica()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.StaticConfiguration.GetValue("chaveJson", "")));
        }
    }
}