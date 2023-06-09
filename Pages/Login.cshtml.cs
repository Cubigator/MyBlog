using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;
using System.Security.Claims;

namespace MyBlog.Pages
{
    public class LoginModel : PageModel
    {
        private List<User> _users = null!;
        private UsersRepository _usersRepository = null!;

        public LoginModel(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public void OnGet()
        {
            
        }

        public async Task<ActionResult> OnPost(LoginUserModel model)
        {
            _users = (await _usersRepository.GelAllAsync()).ToList();

            var user = _users.FirstOrDefault(user => user.Email == model.Email && user.Password == model.Password);

            if (user is null)
                return Unauthorized();

            var claims = new List<Claim> { new Claim(ClaimTypes.Email, user.Email) };
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
