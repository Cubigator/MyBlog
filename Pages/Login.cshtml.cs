using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;
using MyBlog.Services;
using System.Security.Claims;

namespace MyBlog.Pages
{
    public class LoginModel : PageModel
    {
        private List<User> _users = null!;
        private readonly UsersRepository _usersRepository = null!;
        private readonly EncryptorService _encryptor = null!;

        public LoginModel(UsersRepository usersRepository, EncryptorService encryptor)
        {
            _usersRepository = usersRepository;
            _encryptor = encryptor;
        }
        public void OnGet()
        {
            
        }

        public async Task<ActionResult> OnPost(LoginUserModel model)
        {
            _users = (await _usersRepository.GelAllAsync()).ToList();

            var user = _users
                .FirstOrDefault(user => user.Email == model.Email &&
                user.Password == _encryptor.HashPassword(model.Password, user.Salt.ToString()));

            if (user is null)
                return Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Status.ToString())
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Redirect("/");
        }
    }

    public class LoginUserModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
