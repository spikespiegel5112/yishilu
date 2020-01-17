using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NLS.AspNetCore.Controllers;
using NLS.Framework;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NLS.Host.ControllerExtension
{
    //[NLSAuthorize]
    public class TestController : NLSBaseController
    {
        public TestController(NLSEntitesContext context)
        {
            //testServer = IoCManager.Resolve<ITestServer>(); //Reference<TestServer>();
        }

        /// <summary>
        /// 1231231
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("gettoken")]
        public string GetToken()
        {
            var claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("UserId",1.ToString(),ClaimValueTypes.Integer32),
                new Claim("ExpireTime","7200000")
            };
            //var a = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var token = new JwtSecurityToken(
                 issuer: "LvLin",
                 audience: "LvLin",
                 claims: claims,
                 notBefore: DateTime.Now,
                 expires: DateTime.Now.AddDays(1),
                 signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("G0ae*$3c5nmahi4005pa2ea-0fea210c858a")), SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}