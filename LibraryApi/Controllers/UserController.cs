using LibraryApi.Context;
using LibraryApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.Identity;
using LibraryApi.Helpers;
using PasswordHasher = LibraryApi.Helpers.PasswordHasher;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
   // [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _appContext;
        public UserController(AppDbContext appDbContext)
        {
            _appContext = appDbContext;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authentication([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            var user = await _appContext.Users
                .FirstOrDefaultAsync(x => x.UserName == userObj.UserName);
            if (user == null)
                return NotFound(new { Message = "User Not Found" });

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return BadRequest(new { Message = "Password is Incorrect" });
            }
            //return Ok(userObj);
            return Ok(new
            {
                Message = "Login Success !"
            });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userobj, PasswordHasher passwordHasher)
        {
            if (userobj == null)
                return BadRequest();

            //check userName
            if (await CheckUserNameExistAsysnc(userobj.UserName))
                return BadRequest(new { message = "User Name already exist !" });

            //check email
            if (await CheckEmailExistAsysnc(userobj.Email))
                return BadRequest(new { message = "This email is already exist !" });

            //check passwordStrength
            var pass = CheckPasswordStreangth(userobj.Password);
            if (!string.IsNullOrEmpty(pass))
                return BadRequest(new { Message = pass.ToString() });


            userobj.Password = PasswordHasher.HashPassword(userobj.Password);
           // userobj.Role = "User";
            //serobj.Token = "";
            await _appContext.Users.AddAsync(userobj);
            await _appContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "User Registered !"
            });
        }

        private Task<bool> CheckUserNameExistAsysnc(String username)
             => _appContext.Users.AnyAsync(x => x.UserName == username);

        private Task<bool> CheckEmailExistAsysnc(String email)
            => _appContext.Users.AnyAsync(x => x.Email == email);

        private string CheckPasswordStreangth(string password)
        {
            StringBuilder sb = new StringBuilder();
            if (password.Length < 8)
                sb.Append("Minimum password length should be 8" + Environment.NewLine);
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]")
                && Regex.IsMatch(password, "[0-9]")))
                sb.Append("password should be Alphanumeric" + Environment.NewLine);
            if (!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,(,),?,//[,\\],},{,+,=,/,_,-,']"))
                sb.Append("password should be contain special character" + Environment.NewLine);
            return sb.ToString();
        }
    }
}
