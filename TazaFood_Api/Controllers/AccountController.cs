using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TazaFood.Core.Models.Identity;
using TazaFood.Core.Services;
using TazaFood_Api.Dtos;
using TazaFood_Api.Extensions;

namespace TazaFood_Api.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager ,
            ILogger<AccountController> logger , ITokenServices tokenServices , IMapper mapper )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenServices = tokenServices;
            _mapper = mapper;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(loginDto.Email);

                    if (user == null)
                    {
                        // No account with this email
                        return Unauthorized("There Is No Account With This Email");
                    }

                    var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                    if (!result.Succeeded)
                    {
                        // Invalid password
                        return Unauthorized();
                    }
                    // Login successful
                    return Ok(new UserDto
                    {
                        DisplayName = user.DisplayName,
                        Email = user.Email,
                        Token = await _tokenServices.CreateTokenAsync(user,_userManager)
                    });
                }
                // Invalid data
                return BadRequest("Invalid Data");
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError($"An unexpected error occurred during login: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    // Check if the user already exists
                    if (IsEmailExist(registerDto.Email).Result.Value)
                    {
                        // User with this email already exists
                        return BadRequest("There is already an account with this email.");
                    }
                    var user = new AppUser()
                    {
                        DisplayName = registerDto.FirstName + " " + registerDto.LastName,
                        Email = registerDto.Email,
                        UserName = registerDto.Email.Split("@")[0],
                        PhoneNumber = registerDto.PhoneNumber
                    };
                    var result = await _userManager.CreateAsync(user, registerDto.Password);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                        return BadRequest(ModelState);
                    }
                    return Ok(new UserDto
                    {
                        DisplayName = user.DisplayName,
                        Email = user.Email,
                        Token = await _tokenServices.CreateTokenAsync(user,_userManager)
                    });
                }
                // Invalid data
                return BadRequest("Invalid Data");
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError($"An unexpected error occurred during user registration: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            return Ok(new UserDto() 
            { 
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user,_userManager)
            });
        }

        [Authorize]
        [HttpGet("GetUserAddress")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var address =  _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(address);            
        }

        [Authorize]
        [HttpPut("UpdateUserAddress")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddress)
        {
            var address = _mapper.Map<AddressDto, Address>(updatedAddress);
            var user = await _userManager.FindUserWithAddressAsync(User);
            address.Id = user.Address.Id;
            user.Address = address;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest("Something went Wrong While Updating the Address");
            return Ok(updatedAddress);
        }

        [HttpGet("IsEmailExist")]
        public async Task<ActionResult<bool>> IsEmailExist(string Email)
        {
            return await _userManager.FindByEmailAsync(Email) is not null;
        }
    }
}
