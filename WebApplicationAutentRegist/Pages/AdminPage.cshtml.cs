using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlX.XDevAPI.Common;
using WebApplicationAutentRegist.Data;
using static WebApplicationAutentRegist.Pages.AdminPageModel;

namespace WebApplicationAutentRegist.Pages
{
    public class AdminPageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AdminPageModel> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminPageModel(UserManager<ApplicationUser> userManager, ILogger<AdminPageModel> logger, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
        }
        public List<User> Users { get
            {
                List<User> users = new List<User>();
                var listOfAllUsers = _userManager.Users.ToList();

                foreach (var user in listOfAllUsers)
                {
                    users.Add(new User { Id = user.Id, Name = user.Name, Email = user.Email, LastLoginTime = user.LastLoginTime, LastRegistrationTime = user.RegistrationDate, Status = user.Status });
                }
                return users;
            } 
        }   

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostBlockUsersAsync(string[] userIds)
        {
            var currentuser = await GetCurrentUserAsync();
            foreach (var user in userIds)
            {
                var u = _userManager.Users.Where(s => s.Id == user).FirstOrDefault();
                if (u != null)
                {
                    u.Status = true;
                    await _userManager.UpdateAsync(u);
                    await _userManager.UpdateSecurityStampAsync(u);
                }
            }
            if (userIds.Contains(currentuser.Id))
            {
                await _signInManager.SignOutAsync();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostUnBlockUsersAsync(string[] userIds)
        {
           
            foreach (var user in userIds)
            {
                var u = _userManager.Users.Where(s => s.Id == user).FirstOrDefault();
                if (u != null)
                {
                    u.Status = false;
                    await _userManager.UpdateAsync(u);
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteUsersAsync(string[] userIds)
        {
            var currentuser = await GetCurrentUserAsync();
            var us = _userManager.Users.ToList();
            foreach (var user in userIds)
            {
                var u = us.Where(s => s.Id == user).FirstOrDefault();
                if (u != null)
                {
                    await _userManager.DeleteAsync(u);
                }
            }
            foreach (var user in userIds)
            {
                var u = us.Where(s => s.Id == user).FirstOrDefault();
                {
                    await _userManager.UpdateSecurityStampAsync(u);
                }
            }
            if (userIds.Contains(currentuser.Id))
            {
                await _signInManager.SignOutAsync();
            }

            return Page();
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public class User
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime? LastLoginTime { get; set; }
            public DateTime? LastRegistrationTime { get; set; }
            public bool Status { get; set; }
        }
    }
}
