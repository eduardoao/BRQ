using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebPocApi.Configurations;
using WebPocApi.DataAccess;
using WebPocApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace WebPocApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
            [AllowAnonymous]
            [HttpPost]
            public object Post(
                [FromBody]Usuario usuario,
                [FromServices]UsuarioDataAccess usersDAO,
                [FromServices]SigningConfigurations signingConfigurations,
                [FromServices]TokenConfigurations tokenConfigurations)
            {
                bool credenciaisValidas = false;
                if (usuario != null && !String.IsNullOrWhiteSpace(usuario.UsuarioID))
                {
                    var usuarioBase = usersDAO.LocalizarUsuario(usuario.UsuarioID);
                    credenciaisValidas = (usuarioBase != null &&
                        usuario.UsuarioID == usuarioBase.UsuarioID &&
                        usuario.AccessKey == usuarioBase.AccessKey);
                }

                if (credenciaisValidas)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(usuario.UsuarioID, "Login"),
                        new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UsuarioID)
                        }
                    );

                    var dataCriacao = DateTime.Now;
                    var dataExpiracao = dataCriacao + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                    {
                        Issuer = tokenConfigurations.Issuer,
                        Audience = tokenConfigurations.Audience,
                        SigningCredentials = signingConfigurations.SigningCredentials,
                        Subject = identity,
                        NotBefore = dataCriacao,
                        Expires = dataExpiracao
                    });
                    var token = handler.WriteToken(securityToken);

                    return new
                    {
                        authenticated = true,
                        created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                        expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                        accessToken = token,
                        message = "OK"
                    };
                }
                else
                {
                    return new
                    {
                        authenticated = false,
                        message = "Falha ao autenticar a solicitação!"
                    };
                }
            }
        }
    }
