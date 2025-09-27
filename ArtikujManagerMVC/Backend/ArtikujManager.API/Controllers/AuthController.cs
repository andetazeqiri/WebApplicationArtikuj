using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ArtikujManager.API.DTOs;
using ArtikujManager.API.Models;
using ArtikujManager.API.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ArtikujManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid registration data.",
                });
            }

            if (registerDto.Role != "Operator")
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Public registration is only allowed for Operator role.",
                });
            }

            
            if (!await _roleManager.RoleExistsAsync(registerDto.Role))
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid role specified.",
                });
            }

            
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "User with this email already exists.",
                });
            }

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                EmailConfirmed = true // Skip email confirmation for demo
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                });
            }

            await _userManager.AddToRoleAsync(user, registerDto.Role);

            var token = await _jwtTokenService.GenerateTokenAsync(user);

            return Ok(new AuthResponseDto
            {
                Success = true,
                Message = "Registration successful.",
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = registerDto.Role
                }
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid login data.",
                });
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !user.IsActive)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password.",
                });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password.",
                });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            var token = await _jwtTokenService.GenerateTokenAsync(user);

            return Ok(new AuthResponseDto
            {
                Success = true,
                Message = "Login successful.",
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = role
                }
            });
        }

        [HttpGet("roles")]
        public ActionResult<IEnumerable<string>> GetRoles()
        {
            return Ok(new[] { "Operator" });
        }
    }
}