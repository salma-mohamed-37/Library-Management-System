using backend.Dtos.Account;
using backend.Interfaces;
using backend.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using backend.Dtos.Responses;
using static System.Runtime.InteropServices.JavaScript.JSType;


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
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if(! ModelState.IsValid)
            {
                var errors = ModelState
                .Where(e => e.Value!.Errors.Count > 0)
                .SelectMany(e => e.Value!.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
                var response = new APIResponse<object>(400, string.Join("\n", errors) , null);
                return BadRequest(response);
            }

            var is_user_exists = await _userManager.FindByNameAsync(registerDto.Username);
            if (is_user_exists is not null)
            {
                return BadRequest(new APIResponse<object>(400, "Username already exists", null));
            }


            var appUser = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.Fullname,
                DateOfBirth = registerDto.DateOfBirth,
                Type = StaticUserRoles.USER.ToString()
            };

            var is_succeeded = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (!is_succeeded.Succeeded)
            {
                var error = "User creation failed because: ";

                foreach (var e in is_succeeded.Errors)
                {
                    error += "\n";
                    error += e.Description;
                }
                return BadRequest(new APIResponse<object>(400, error, null));
            }
                await _userManager.AddToRoleAsync(appUser, StaticUserRoles.USER);

            return Ok(new APIResponse<object>(200,"User created successfully",null));
        }

        [Authorize(Roles ="ADMIN")]
        [HttpPost("registerLibrarian")]
        public async Task<IActionResult> RegisterAsLibrarian([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                .Where(e => e.Value!.Errors.Count > 0)
                .SelectMany(e => e.Value!.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
                var response = new APIResponse<object>(400, string.Join("\n", errors), null);
                return BadRequest(response);
            }

            var is_user_exists = await _userManager.FindByNameAsync(registerDto.Username);
            if (is_user_exists is not null)
            {
                return BadRequest(new APIResponse<object>(400, "Username already exists", null));
            }


            var appUser = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.Fullname,
                DateOfBirth = registerDto.DateOfBirth,
                Type = StaticUserRoles.lIBRARIAN.ToString()
            };

            var is_succeeded = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (!is_succeeded.Succeeded)
            {
                var error = "User creation failed because: ";

                foreach (var e in is_succeeded.Errors)
                {
                    error += "\n";
                    error += e.Description;
                }
                return BadRequest(new APIResponse<object>(400, error, null));
            }
            await _userManager.AddToRoleAsync(appUser, StaticUserRoles.lIBRARIAN);

            return Ok(new APIResponse<object>(200,"Librarian created successfully",null));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            { 
                return Unauthorized(new APIResponse<object>(400, "Invalid credentials", null));
            }

            var is_password_correct = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!is_password_correct)
            {
                return Unauthorized(new APIResponse<object>(400, "Invalid credentials", null));
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

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(jwt);
            var expiryDate = jwtSecurityToken.ValidTo;
            var expiryDateLocal = expiryDate.ToLocalTime();

            var res = new LoggedInUserDto 
            {
                Id = user.Id,
                Role = userRoles[0],
                Token = jwt,
                ExpiryDate = expiryDateLocal
            };

            return Ok(new APIResponse<object>(200,"", res));
        }

        [HttpPost("checkForAdmin")]
        public async Task<IActionResult>AddAdminIfNotExists()
        {
            var admin = new ApplicationUser
            {
                UserName = "admin111",
                Email = "admin@gmail.com",
                FullName = "admin",
                Type = StaticUserRoles.ADMIN.ToString()
            };

            var is_admin_exists = await _userManager.FindByNameAsync(admin.UserName);

            if (is_admin_exists is not null)
            {
                return BadRequest(new APIResponse<object>(400,"Admin already exists", null));
            }
            else
            {
                var is_succeeded = await _userManager.CreateAsync(admin, "admin111");
                if (is_succeeded.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, StaticUserRoles.ADMIN);
                    return Ok(new APIResponse<object>(200,"Admin created successfully",null));

                }
                else
                {
                    var error = "User creation failed because: ";

                    foreach (var e in is_succeeded.Errors)
                    {
                        error += "\n";
                        error += e.Description;
                    }
                    return BadRequest(new APIResponse<object>(400, error, null));
                }

            }
            
        }
    }
}
