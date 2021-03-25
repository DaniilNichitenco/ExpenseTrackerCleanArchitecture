using ExpenseTracker.Core.Domain.Auth;
using ExpenseTracker.Core.Domain.UserDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.IdentityServer.Controllers
{
    [Route("identity/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpDto userSignUpDto)
        {
            var user = new User
            {
                UserName = userSignUpDto.UserName,
                Email = userSignUpDto.Email,
                IsActive = true
            };

            return Ok(new { user.UserName, userSignUpDto.Password });

            var result = await _userManager.CreateAsync(user, userSignUpDto.Password);

            if(result == IdentityResult.Success)
            {
                await _userManager.AddClaimAsync(user, new Claim("sub", user.Id.ToString()));
                await _userManager.AddToRoleAsync(user, "user");

                return Ok(new { user.UserName, userSignUpDto.Password });
            }

            return BadRequest(result.Errors);
        }
    }
}
