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
using AutoMapper;
using backend.Handlers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

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
        private readonly IMapper _mapper;
        private readonly ImageHandler _imageHandler;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IBorrowedRepository _borrowedRepository;
        public AccountController(IUserRepository userRepository, ImageHandler imageHandler, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ITokenService tokenService, IMapper mapper, IValidator<RegisterDto> registerValidator, IBorrowedRepository borrowedRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _imageHandler = imageHandler;
            _mapper = mapper;
            _userRepository = userRepository;
            _registerValidator = registerValidator;
            _borrowedRepository = borrowedRepository;
        }

        [HttpPost("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.lIBRARIAN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));

            return Ok();
        }

        [Authorize(Roles ="lIBRARIAN")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
        {
            registerDto.Type = StaticUserRoles.USER.ToString();

            if (! ModelState.IsValid)
            {
                var errors = ModelState
                .Where(e => e.Value!.Errors.Count > 0)
                .SelectMany(e => e.Value!.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
                var response = new APIResponse<object>(400, string.Join("\n", errors) , null);
                return BadRequest(response);
            }

            var validationResult = await _registerValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                var fluentErrors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = new APIResponse<object>(400, string.Join("\n", fluentErrors), null);
                return BadRequest(response);
            }

            var appUser = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.Fullname,
                DateOfBirth = registerDto.DateOfBirth,
                Type = registerDto.Type,
                Gender = registerDto.Gender,
                City = registerDto.City,
                PhoneNumber =registerDto.PhoneNumber,
                ImagePath = $"{Guid.NewGuid()}_{registerDto.ImageFile.FileName}"

            };

            var is_succeeded = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (!is_succeeded.Succeeded)
            {
                var errors = is_succeeded.Errors
                            .Select(e => e.Description)
                            .ToList();

                var errorMessage = "User creation failed because:\n" + string.Join("\n", errors);
                return BadRequest(new APIResponse<object>(400, errorMessage, null));
            }

            await _userManager.AddToRoleAsync(appUser, StaticUserRoles.USER);
            await _imageHandler.SaveImageFile(registerDto.ImageFile, appUser.ImagePath, "Users");

            return Ok(new APIResponse<object>(200,"User created successfully",null));
        }

        [Authorize(Roles ="ADMIN")]
        [HttpPost("registerLibrarian")]
        public async Task<IActionResult> RegisterAsLibrarian([FromForm] RegisterDto registerDto)
        {
            registerDto.Type = StaticUserRoles.lIBRARIAN.ToString();
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

            var validationResult = await _registerValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                var fluentErrors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = new APIResponse<object>(400, string.Join("\n", fluentErrors), null);
                return BadRequest(response);
            }

            var appUser = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.Fullname,
                DateOfBirth = registerDto.DateOfBirth,
                Type = registerDto.Type,
                Gender = registerDto.Gender,
                City = registerDto.City,
                PhoneNumber = registerDto.PhoneNumber,
                ImagePath = $"{Guid.NewGuid()}_{registerDto.ImageFile.FileName}"

            };
            

            var is_succeeded = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (!is_succeeded.Succeeded)
            {
                var errors = is_succeeded.Errors
                            .Select(e => e.Description)
                            .ToList();

                var errorMessage = "User creation failed because:\n" + string.Join("\n", errors);
                return BadRequest(new APIResponse<object>(400, errorMessage, null));
            }

            await _userManager.AddToRoleAsync(appUser, StaticUserRoles.lIBRARIAN);
            await _imageHandler.SaveImageFile(registerDto.ImageFile, appUser.ImagePath, "Users");

            return Ok(new APIResponse<object>(200,"Librarian created successfully",null));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
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

            var user = await _userManager.Users
                        .Where(u => u.Email == loginDto.Email && !u.IsDeleted)
                        .FirstOrDefaultAsync();

            if (user == null)
            { 
                return BadRequest(new APIResponse<object>(400, "Invalid credentials", null));
            }

            var is_password_correct = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!is_password_correct)
            {
                return BadRequest(new APIResponse<object>(400, "Invalid credentials", null));
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
                ExpiryDate = expiryDateLocal,
                ImagePath = Path.Combine("StaticFiles", "Images", "Users", user.ImagePath)
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
                Type = StaticUserRoles.ADMIN.ToString(),
                Gender = "Male",
                City= "Giza",
                PhoneNumber = "12345678",
                ImagePath="admin.jpeg",
                DateOfBirth= new DateTime(1990, 1, 1)
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
                    var errors = is_succeeded.Errors
                           .Select(e => e.Description)
                           .ToList();

                    var errorMessage = "User creation failed because:\n" + string.Join("\n", errors);
                    return BadRequest(new APIResponse<object>(400, errorMessage, null));
                }

            }
            
        }

        [HttpPut()]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] UpdateUserDto dto, [FromQuery] string userId = null)
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

            if (userId ==null)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            else
            {
                var operatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = await _userRepository.GetUserRole(userId);
                var operatorRole =await  _userRepository.GetUserRole(operatorId!);
                if (! CanUpdate(userRole, operatorRole))
                {
                    return BadRequest(new APIResponse<object>(400,"You are not authorized to  update this account.", null));
                }
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted == true)
            {
                return BadRequest(new APIResponse<object>(400, "This user doesn't exist.", null));
            }

            user.FullName = dto.Fullname;
            user.DateOfBirth = dto.DateOfBirth;
            user.Gender = dto.Gender;
            user.PhoneNumber = dto.PhoneNumber;
            user.City = dto.City;

            if (dto.ImageFile != null)
            {
                 _imageHandler.DeleteImage(user.ImagePath, "Users");
                user.ImagePath = $"{Guid.NewGuid()}_{dto.ImageFile.FileName}";
                await _imageHandler.SaveImageFile(dto.ImageFile, user.ImagePath, "Users");
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new APIResponse<object>(200, "Account updated successfully", null));
            }
            else
            {
                var errors = result.Errors
                            .Select(e => e.Description)
                            .ToList();

                var errorMessage = "User creation failed because:\n" + string.Join("\n", errors);
                return BadRequest(new APIResponse<object>(400, errorMessage, null));
            }

        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteAccount([FromQuery] string userId=null)
        {
            if (userId == null)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            else
            {
                var operatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = await _userRepository.GetUserRole(userId);
                var operatorRole = await _userRepository.GetUserRole(operatorId!);
                if (!CanUpdate(userRole, operatorRole))
                {
                    return BadRequest(new APIResponse<object>(400, "You are not authorized to  delete this account.", null));
                }
            }

            if(await _borrowedRepository.IsCurrentlyBorrower(userId))
            {
                return BadRequest(new APIResponse<object>(400, "This user is currently borrowing a book.", null));
            }

            var user = await _userManager.FindByIdAsync(userId);

            user.IsDeleted = true;
            user.DeletedAt = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new APIResponse<object>(200, "Account deleted successfully", null));
            }
            else
            {
                var errors = result.Errors
                            .Select(e => e.Description)
                            .ToList();

                var errorMessage = "User creation failed because:\n" + string.Join("\n", errors);
                return BadRequest(new APIResponse<object>(400, errorMessage, null));
            }



        }



        private bool CanUpdate(string userRole, string operatorRole)
        {
            if (userRole == StaticUserRoles.USER.ToString() && operatorRole == StaticUserRoles.lIBRARIAN.ToString())
                return true;
            else if (userRole == StaticUserRoles.lIBRARIAN.ToString() && operatorRole == StaticUserRoles.ADMIN.ToString())
                return true;
            else
                return false;
        }

        [Authorize]
        [HttpGet("info")]
        public async Task<ActionResult<APIResponse<UserDto>>> GetAccountInfo([FromQuery] string userId = null)
        {
            if (userId == null)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var user = await _userManager.FindByIdAsync(userId!);
            if (user == null)
            {
                return BadRequest(new APIResponse<object>(400, "The user is not found.",null));
            }
            var userDto = _mapper.Map<UserDto>(user);

            return Ok(new APIResponse<UserDto>(200, "", userDto));
        }


        [Authorize]

        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                var vErrors = ModelState
                .Where(e => e.Value!.Errors.Count > 0)
                .SelectMany(e => e.Value!.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
                var response = new APIResponse<object>(400, string.Join("\n", vErrors), null);
                return BadRequest(response);
            }

            if (dto.UserId == null)
            {
                dto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            else
            {
                var operatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = await _userRepository.GetUserRole(dto.UserId);
                var operatorRole = await _userRepository.GetUserRole(operatorId!);
                if (!CanUpdate(userRole, operatorRole))
                {
                    return BadRequest(new APIResponse<object>(400, "You are not authorized to  change password update this account.", null));
                }
            }

            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return BadRequest(new APIResponse<object>(404, "This account doesn't exist.", null));
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new APIResponse<object>(200, "Password changed successfully.", null));
            }
            var errors = result.Errors
                           .Select(e => e.Description)
                           .ToList();

            var errorMessage = "User creation failed because:\n" + string.Join("\n", errors);
            return BadRequest(new APIResponse<object>(400, errorMessage, null));


        }
    }
}
