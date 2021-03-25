using ExpenseTracker.Core.Domain.Auth;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Server.Validation
{
    public class ExpenseTrackerResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public ExpenseTrackerResourceOwnerPasswordValidator(SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(context.UserName);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, context.Password, false, false);
                    //check if password match - remember to hash password if stored as hash in db
                    if (result.Succeeded)
                    {
                        //set the result
                        context.Result = new GrantValidationResult(
                        subject: user.Id.ToString(),
                        authenticationMethod: "custom",
                        claims: ProfileService.GetUserClaims(user));

                        return;
                    }

                    if (!user.EmailConfirmed)
                    {
                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Email is not confirmed");
                    }
                    else
                    {
                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect password");
                    }
                    return;
                }
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "User does not exist.");
                return;
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
                Console.WriteLine(ex.Message);
            }

        }
    }
}
