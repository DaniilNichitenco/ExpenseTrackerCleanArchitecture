using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Domain.Mails;
using ExpenseTracker.Core.Domain.UserDtos;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static IdentityModel.OidcConstants;

namespace ExpenseTracker.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly ILogger<AccountController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IDiscoveryCache _discoveryCache;
        private readonly IConfiguration _configuration;
        private readonly ISendingManager _sendingManager;
        private readonly ITagReplacer _tagReplacer;

        public AccountController(ILogger<AccountController> logger, HttpClient httpClient,
            IDiscoveryCache discoveryCache, IConfiguration configuration, ISendingManager sendingManager, ITagReplacer tagReplacer)
        {
            _logger = logger;
            _httpClient = httpClient;
            _discoveryCache = discoveryCache;
            _configuration = configuration;
            _sendingManager = sendingManager;
            _tagReplacer = tagReplacer;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserSignInDto userSignInDto)
        {

            var endPointDiscovery = await _discoveryCache.GetAsync();

            if (endPointDiscovery.IsError)
            {
                _logger.Log(LogLevel.Error, $"ErrorType: {endPointDiscovery.ErrorType} Error: {endPointDiscovery.Error}");
                throw new Exception("Something went wrong while connecting to the AuthServer Token Endpoint.");
            }

            var tokenResponse = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = endPointDiscovery.TokenEndpoint,
                ClientId = _configuration["Self:Id"],
                ClientSecret = _configuration["Self:Secret"],
                Scope = "app.expensetracker.api.full openid profile offline_access",
                GrantType = GrantTypes.Password,
                UserName = userSignInDto.UserName,
                Password = userSignInDto.Password
            });

            return Ok(new { access_token = tokenResponse.AccessToken, refresh_token = tokenResponse.RefreshToken, expires_in = tokenResponse.ExpiresIn });
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(UserSignInDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpDto userSignUpDto)
        {
            var result = await _httpClient.PostAsync($"{_configuration["ApiResourceBaseUrls:AuthServer"]}/identity/users/create", JsonContent.Create(userSignUpDto));
            var content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                var link = Url.Action("RequestEmailConfirmation", new { userSignUpDto.Email, userSignUpDto.UserName });
                await _httpClient.GetAsync($"https://localhost:5002{link}");
                return Ok(JsonConvert.DeserializeObject<UserSignInDto>(content));
            }
            else
            {
                return BadRequest(new { errors = JsonConvert.DeserializeObject<IEnumerable<IdentityError>>(content) });
            }
        }

        [AllowAnonymous]
        [HttpGet("refresh/{refreshToken}")]
        public async Task<IActionResult> Refresh([FromRoute] string refreshToken)
        {
            var endPointDiscovery = await _discoveryCache.GetAsync();

            if (endPointDiscovery.IsError)
            {
                _logger.Log(LogLevel.Error, $"ErrorType: {endPointDiscovery.ErrorType} Error: {endPointDiscovery.Error}");
                throw new Exception("Something went wrong while connecting to the AuthServer Token Endpoint.");
            }

            var tokenResponse = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Method = HttpMethod.Post,
                Address = endPointDiscovery.TokenEndpoint,
                ClientId = _configuration["Self:Id"],
                ClientSecret = _configuration["Self:Secret"],
                Scope = "app.expensetracker.api.full openid profile",
                RefreshToken = refreshToken
            });

            return Ok(new { access_token = tokenResponse.AccessToken, refresh_token = tokenResponse.RefreshToken, expires_in = tokenResponse.ExpiresIn });
        }

        [AllowAnonymous]
        [HttpGet("email/confirmation")]
        public async Task<IActionResult> RequestEmailConfirmation([FromQuery] string email, [FromQuery] string username)
        {
            var result = await _httpClient
                .GetAsync(
                    $"{_configuration["ApiResourceBaseUrls:AuthServer"]}/identity/users/email/confirmation/request?email={email}&redirectTo=https://localhost:5002/swagger/");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var content = await result.Content.ReadAsStringAsync();
                try
                {
                    var tagsText = new Dictionary<string, string>
                    {
                        { "heading", "Email confirmation" },
                        { "subheading", $"{username.First().ToString().ToUpper() + username.Substring(1).ToLower()}," },
                        { "body", $"To get started, confirm your email address by clicking the link: https://localhost:5001{content}" }
                    };

                    var text = EmailConfirmation.Message;
                    var body = _tagReplacer.ReplaceTags(tagsText, text);


                    if (_sendingManager.SendMessage("Email confirmation", body,
                        new List<MailAddress> {new MailAddress(email)}))
                    {
                        return Ok();
                    }
                }
                catch(Exception exception)
                {
                    _logger.LogError(exception.Message);
                    return BadRequest();
                }
            }

            return BadRequest();
        }

    }
}
