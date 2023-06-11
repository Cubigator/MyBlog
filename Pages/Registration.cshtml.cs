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
        public RegistrationModel(UsersRepository usersRepository, EncryptorService encryptor)
        {
            _usersRepository = usersRepository;
            _encryptor = encryptor;
        }
        public void OnGet()
        {
        }

        public async Task<ActionResult> OnPost(RegistrationUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _usersRepository.GetByEmailAsync(model.Email) != null) 
                    return Page();

                if(model.RepeatPassword != model.Password)
                    return Page();

                User user = new User()
                {
                    Email = model.Email,
                    Name = model.Name,
                    Salt = Guid.NewGuid(),
                    Status = UserStatus.User
                };

                user.Password = _encryptor.HashPassword(model.Password, user.Salt.ToString());

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
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string RepeatPassword { get; set; } = null!;
    }
}
