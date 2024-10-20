using Friendbook.Business.Dtos.TokenDtos;
using Friendbook.Business.Dtos.UserDtos;
using Friendbook.Business.Exceptions.LoginRegisterExceptions;
using Friendbook.Business.Services.Interfaces;
using Friendbook.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IConfiguration configuration;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }
        public async Task<TokenResponseDto> Login(UserLoginDto userLoginDto)
        {
            AppUser user = null;
            user = await userManager.FindByEmailAsync(userLoginDto.Email);
            if (user == null)
            {
                throw new NullReferenceException("Invalid Credentials");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, false);
            var roles = await userManager.GetRolesAsync(user);

            List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier,user.Id),
            new Claim(ClaimTypes.Name,user.UserName),
             new Claim("Fullname", user.FullName),
            .. roles.Select(role => new Claim(ClaimTypes.Role, role)),
        ];

            string secretKey = configuration.GetSection("JWT:SecretKey").Value; 
            DateTime expires = DateTime.UtcNow.AddDays(10);

            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken = new(
                signingCredentials: signingCredentials,
                claims: claims,
                audience: configuration.GetSection("JWT:Audience").Value,
                issuer: configuration.GetSection("JWT:Issuer").Value,
                expires: expires,
                notBefore: DateTime.UtcNow
                );

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return new TokenResponseDto(token, expires);
        }

        public async Task Register(UserRegisterDto userRegisterDto)
        {
            if (userRegisterDto.Password != userRegisterDto.ConfirmPassword) throw new PasswordsDoNotMatchException(StatusCodes.Status400BadRequest, "ConfirmPassword", "Passwords do not match");

            Random random = new Random();
            int randomDigits = random.Next(100, 1000);

            string userName = $"{userRegisterDto.FullName.Substring(0,3)}{randomDigits}";
            AppUser appUser = new AppUser()
            {
                Email = userRegisterDto.Email,
                FullName= userRegisterDto.FullName,
                UserName = userName
            };

            var result = await userManager.CreateAsync(appUser, userRegisterDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception();
            }
        }
    }
}
