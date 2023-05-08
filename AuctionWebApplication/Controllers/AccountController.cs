using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuctionWebApplication.ViewModel;
using AuctionWebApplication.Models;

namespace AuctionWebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserIdent> _userManager;
        private readonly SignInManager<UserIdent> _signInManager;
        private readonly DbauctionContext _context;
    public AccountController(UserManager<UserIdent> userManager, SignInManager<UserIdent> signInManager, DbauctionContext dbcontext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = dbcontext;
    }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
				UserIdent user = new UserIdent { Email = model.Email, UserName = model.Email};
                User usr = new User { UserId =user.Id, Name = model.Name, Balance=10000, Freeze=0};
                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    _context.Add(usr);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Auctions");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("index", "Auctions");
                }
            }
            else
            {
                ModelState.AddModelError(" ", "Неправильний логін чи(та) пароль");
            }
           
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Auctions");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Auctions");
        }
    }
}