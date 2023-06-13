using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;
using MyBlog.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MyBlog.Pages
{
    public class LoginModel : PageModel
    {
        private List<User> _users = null!;
        private readonly UsersRepository _usersRepository = null!;
        private readonly EncryptorService _encryptor = null!;

        [BindProperty]
        public LoginUserModel InputModel { get; set; } = null!;
        public LoginModel(UsersRepository usersRepository, EncryptorService encryptor)
        {
            _usersRepository = usersRepository;
            _encryptor = encryptor;
        }
        public void OnGet()
        {
            
        }

        public async Task<ActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                _users = (await _usersRepository.GelAllAsync()).ToList();

                var user = _users
                    .FirstOrDefault(user => user.Email == InputModel.Email &&
                    user.Password == _encryptor.HashPassword(InputModel.Password, user.Salt.ToString()));

                //TODO
                if (user is null)
                {
                    ModelState.TryAddModelError("Email", "Неверный email и/или пароль");
                    return Page();
                }

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Status.ToString())
            };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Redirect("/");
            }
            return Page();
        }
    }

    public class LoginUserModel
    {
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [EmailAddress(ErrorMessage = "Некорректный формат")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string Password { get; set; } = null!;
    }
}
