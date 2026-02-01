using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IAuthService authService,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }
        [HttpPost("login")]  //api\Account/login
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) 
                return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user,model.Password,false);
            if (result.Succeeded is false) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDTO()
            {
                DisplayName=user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user,_userManager)
            });
        }
        [HttpPost("Register")]   //api\Account\Register
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
        {
            if(CheckEmailExists(model.Email).Result.Value)  //synchrounous
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] {"This email is already exist"} });
            var user = new AppUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            //if (result.Succeeded is false) return BadRequest(new ApiResponse(400));
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new UserDTO() {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser() //user who send this current request
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)

            });
        }
        [HttpGet("Address")]  //api/Account/Address
        [Authorize]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
           
            var user = await _userManager.FindUserWithAddressAsync(User);
            var address = _mapper.Map<AddressDTO>(user.Address);
            return Ok(address);
        }
        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO updatedAdderess)
        {
            var address = _mapper.Map<AddressDTO, Address>(updatedAdderess);
            var user = await _userManager.FindUserWithAddressAsync(User);
            address.Id = user.Address.Id;
            user.Address = address;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(updatedAdderess);

        }
        [HttpGet("emailexists")]  //api/Account/emailexists?email=Rofy@test.com
        [Authorize]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
            //true //exists
            //false //doesn't exist
        }
    }
}
