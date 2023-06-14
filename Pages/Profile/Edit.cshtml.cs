using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;
using MyBlog.Pages.ViewModels;
using System.Security.Claims;

namespace MyBlog.Pages.Profile
{
    [Authorize]
    public class EditModel : PageModel
    {
        [BindProperty]
        public UserViewModel UserModel { get; set; } = null!;
        private readonly UsersRepository _usersRepository = null!;

        public EditModel(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public async Task<ActionResult> OnGet()
        {
            string userEmail = HttpContext.User.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.Email)!.Value;

            var user = await _usersRepository.GetByEmailAsync(userEmail);
            if (user is null)
                return Redirect("/");
            UserModel = new UserViewModel()
            {
                Id = user.Id,
                Status = user.Status,
                Email = user.Email,
                Name = user.Name,
            };

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            string userEmail = HttpContext.User.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.Email)!.Value;

            var user = await _usersRepository.GetByEmailAsync(userEmail);

            if(user is null)
                return Redirect("/");
            
            user.Name = UserModel.Name;
            await _usersRepository.UpdateAsync(user);
            return Redirect("/Profile");
        }
    }
}
