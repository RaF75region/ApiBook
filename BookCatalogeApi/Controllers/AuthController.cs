using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCatalogeApi.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookCatalogeApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("Auth")]
        [AllowAnonymous]
        public async Task<ResultLogon> Auth([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var userData = await _userManager.FindByNameAsync(user.UserName);
                Microsoft.AspNetCore.Identity.SignInResult signInResult  = await _signInManager.CheckPasswordSignInAsync(userData, user.Password, false);
                if (signInResult.Succeeded)
                {
                    var role = await _userManager.GetRolesAsync(userData);
                    Claim[] claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Name,userData.UserName),
                        new Claim(ClaimTypes.Role,role[0]),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    };

                    JwtSecurityToken token = new JwtSecurityToken(
                        audience: AuthOptions.AUDIENCE,
                        issuer: AuthOptions.ISSUER,
                        expires: DateTime.Now.AddHours(1),
                        claims: claims,
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                        );
                    return new ResultLogon() { Name = userData.UserName, JwtToken = new JwtSecurityTokenHandler().WriteToken(token), Success = true, Message = "Logon success" };
                }
                return new ResultLogon() { Name = userData.UserName, JwtToken = String.Empty, Success = false, Message = "Logon error" };
            }
            return new ResultLogon() { Name = String.Empty, JwtToken = String.Empty, Success = false, Message = "Date incorrect" };

        }
        
        [HttpPost]
        [Route("CreateUser")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> CreateUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                IdentityUser identityUser = new IdentityUser()
                {
                    UserName = user.UserName,
                };
                IdentityResult identityResult = await _userManager.CreateAsync(identityUser,user.Password);
                if (identityResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(identityUser, user.RoleUser.Name);
                    return new HttpResponseMessage(HttpStatusCode.Accepted);
                }                 
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [Route("CreateRole")]
        [Authorize(Roles ="admin")]
        public async Task<HttpResponseMessage> CreateRole([FromBody] Role role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = role.Name,
                };
                IdentityResult identityResult = await _roleManager.CreateAsync(identityRole);
                if (identityResult.Succeeded)
                    return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}

