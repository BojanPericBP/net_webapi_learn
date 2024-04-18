using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController(UserManager<AppUser> userManager) : ControllerBase()
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerInput)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appUser = new AppUser
            {
                UserName = registerInput.Username,
                Email = registerInput.Email,
            };

            var createdUser = await userManager.CreateAsync(appUser, registerInput.Password!);


            if (createdUser.Succeeded)
            {
                var role = new Collection<string> { "User", };

                var roleResult = await userManager.AddToRolesAsync(appUser, role);

                if (roleResult.Succeeded)
                    return Ok("User created");

                else
                    return StatusCode(500, roleResult.Errors);
            }
            else
                return StatusCode(500, createdUser.Errors);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}