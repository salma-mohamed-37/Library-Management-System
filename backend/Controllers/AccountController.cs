using backend.Dtos.Account;
using backend.Interfaces;
using backend.Models;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpPost("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.lIBRARIAN));
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if(! ModelState.IsValid)
            {
                return BadRequest();
            }

            var is_user_exists = await _userManager.FindByNameAsync(registerDto.Username);
            if (is_user_exists is not null)
            {
                return BadRequest("Username already exists");
            }


            var appUser = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.Fullname,
                DateOfBirth = registerDto.DateOfBirth
            };

            var is_succeeded = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (!is_succeeded.Succeeded)
            {
                var error = "User creation failed because: ";

                foreach (var e in is_succeeded.Errors)
                {
                    error += " ";
                    error += e.Description;
                }
                return BadRequest(error);
            }
            if (registerDto.Type == "librarian")
                await _userManager.AddToRoleAsync(appUser, StaticUserRoles.lIBRARIAN);
            else
                await _userManager.AddToRoleAsync(appUser, StaticUserRoles.USER);

            return Ok("User created successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var is_password_correct = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!is_password_correct)
            {
                return Unauthorized("Invalid credentials");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim ("JWTID", Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var r in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, r));
            }

            var jwt = _tokenService.GenerateNewJsonWebToken(claims);

            return Ok(jwt);
        }
    }
}
