using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystemAPI.Data;
using QueueManagementSystemAPI.DTOs;
using QueueManagementSystemAPI.Interfaces;
using QueueManagementSystemAPI.Models;


namespace QueueManagementSystemAPI.Controllers
{
    public class AccountController : BaseApiController
    {

        private readonly UserManager<App1User> _userManager;

        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<App1User> userManager, ITokenService tokenService, ApplicationDbContext context)
        {
            _tokenService = tokenService;
            _userManager = userManager; 
            _context = context;
            
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");


            var user = new App1User
            {
                UserName = registerDto.Username.ToLower(),
                Name = registerDto.Name,
                LastName = registerDto.LastName,
                Authenticated = false
            };

            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            //return new UserDto
            //{
            //    Username = user.UserName,
            //    Token = await _tokenService.CreateToken(user)
            //};
            return Ok(new { Message = "Registration successful, pending admin approval." });

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

            if(user == null) return Unauthorized("Invalid username");

            if (user.Authenticated == false) return Unauthorized("User not approved");

            if(loginDto.SelectedRole == null){
                return BadRequest("no selected role! man");
            }

            if (loginDto.SelectedRole != null)
            {
                if (loginDto.SelectedRole == 0)
                {
                    user.SelectedRole = UserRoleEnum.MEMBER;
                } else if (loginDto.SelectedRole == 1)
                {
                    user.SelectedRole = UserRoleEnum.ADMIN;
                } else user.SelectedRole = UserRoleEnum.HANDLER;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();


            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if(!result) return Unauthorized("Invalid password"); 

            return new UserDto
            {
                Username = user.UserName,
                Token = await  _tokenService.CreateToken(user)
            };
        }


        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());  
        }
    }
}
