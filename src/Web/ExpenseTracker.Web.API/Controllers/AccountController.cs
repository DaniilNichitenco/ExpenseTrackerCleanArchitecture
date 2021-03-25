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
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace ExpenseTracker.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly ILogger<AccountController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IDiscoveryCache _discoveryCache;
        private readonly IConfiguration _configuration;

        public AccountController(ILogger<AccountController> logger, HttpClient httpClient,
            IDiscoveryCache discoveryCache, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _discoveryCache = discoveryCache;
            _configuration = configuration;
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

            var tokenRespone = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = endPointDiscovery.TokenEndpoint,
                ClientId = _configuration["Self:Id"],
                ClientSecret = _configuration["Self:Secret"],
                Scope = "app.expensetracker.api.full openid profile offline_access",
                GrantType = GrantTypes.Password,
                UserName = userSignInDto.UserName,
                Password = userSignInDto.Password
            });

            return Ok(new { access_token = tokenRespone.AccessToken, refresh_token = tokenRespone.RefreshToken, expires_in = tokenRespone.ExpiresIn });
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpDto userSignUpDto)
        {
            var result = await _httpClient.PostAsync($"{_configuration["ApiResourceBaseUrls:AuthServer"]}/identity/users/create", JsonContent.Create(userSignUpDto));
            var content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
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

            var tokenRespone = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Method = HttpMethod.Post,
                Address = endPointDiscovery.TokenEndpoint,
                ClientId = _configuration["Self:Id"],
                ClientSecret = _configuration["Self:Secret"],
                Scope = "app.expensetracker.api.full openid profile",
                RefreshToken = refreshToken
            });

            return Ok(new { access_token = tokenRespone.AccessToken, refresh_token = tokenRespone.RefreshToken, expires_in = tokenRespone.ExpiresIn });
        }
    }
}
