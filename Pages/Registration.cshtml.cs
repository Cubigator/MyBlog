using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;
using MyBlog.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MyBlog.Pages
{
    public class RegistrationModel : PageModel
    {
        private readonly UsersRepository _usersRepository;
        private readonly EncryptorService _encryptor;

        [BindProperty]
        public RegistrationUserModel InputModel { get; set; }
        public RegistrationModel(UsersRepository usersRepository, EncryptorService encryptor)
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
                if (await _usersRepository.GetByEmailAsync(InputModel.Email) != null)
                {
                    ModelState.TryAddModelError("Email", "Пользователь уже существует");
                    return Page();
                }

                User user = new User()
                {
                    Email = InputModel.Email,
                    Name = InputModel.Name,
                    Salt = Guid.NewGuid(),
                    Status = UserStatus.User
                };

                user.Password = _encryptor.HashPassword(InputModel.Password, user.Salt.ToString());

                await _usersRepository.AddAsync(user);

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

    public class RegistrationUserModel
    {
        public string? Name { get; set; }
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [EmailAddress(ErrorMessage = "Неверный формат")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#$%^&*-]).{10,}$",
            ErrorMessage = "Пароль слишком простой")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string RepeatPassword { get; set; } = null!;
    }
}
