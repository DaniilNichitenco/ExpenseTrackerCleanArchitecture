using ExpenseTracker.Core.Domain.Auth;
using ExpenseTracker.Core.Domain.Exceptions;
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

            var result = await _userManager.CreateAsync(user, userSignUpDto.Password);

            if(result == IdentityResult.Success)
            {
                await _userManager.AddClaimAsync(user, new Claim("sub", user.Id.ToString()));
                await _userManager.AddToRoleAsync(user, "user");

                return Ok(new { user.UserName, userSignUpDto.Password });
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("email/confirmation/request")]
        public async Task<IActionResult> RequestEmailConfirmation([FromQuery] string email, [FromQuery] string redirectTo)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                throw new BadRequestException("User with this email does not exist");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("EmailConfirmation", new { email = user.Email, token, redirectTo });

            return Ok(confirmationLink);
        }

        [HttpGet("email/confirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token, [FromQuery] string redirectTo)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return Ok(false);
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return RedirectPermanent(redirectTo);
            }

            return Ok(result.Succeeded);
        }
    }
}
