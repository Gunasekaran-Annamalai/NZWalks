using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /*
         * Here, we used UserManager<IdentityUser> as dependency
         * It is a class from Microsoft.AspNetCore.Identity;
         * We can use this class to interact with the Auth database which is automatically created from IdentityDbContext
         * It is the inbuilt class which helps us to interact with "Users" table in Auth database
         */
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }
        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Username
            };
            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDTO.Password);

            // Checking whether the user has been added successfully
            if (identityResult.Succeeded)
            {
                if (registerRequestDTO.Roles is not null && registerRequestDTO.Roles.Any())
                {
                    /* 
                 * Add roles to this User
                 * the "registerRequestDTO.Roles" is an array because,
                 * ".AddToRolesAsync(identity_user, [])" takes an identity user as 1st parameter and takes an IEnumerable as 2nd parameter
                 */
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! Please login");
                    }
                }
            }

            return BadRequest("Something went wrong");
        }

        // POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var identityUser = await _userManager.FindByEmailAsync(loginRequestDTO.Username);

            if (identityUser is not null)
            {
                // here, we are passing in the identity user and password and checking it
                // .CheckPasswordAsync() will return a boolean result
                var checkPasswordResult = await _userManager.CheckPasswordAsync(identityUser, loginRequestDTO.Password);

                if (checkPasswordResult)
                {
                    // Get roles for user
                    // .GetRolesAsync(identityUser) -> this will get the role from UserRole table
                    // by matching user id
                    var roles = await _userManager.GetRolesAsync(identityUser);

                    if (roles != null)
                    {
                        // Create Token
                        var jwtToken = _tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken,
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("Username or Password Incorrect");
        }
    }
}
