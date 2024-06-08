using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Data;
using ResumeBuilder.Models;
using System.Security.Claims;

namespace ResumeBuilder.Controllers
{
    public class AccessController : Controller
    {
        private readonly ResumeBuilderContext _context;
        private const string accessMessage = "AccessMessage";

        public AccessController(ResumeBuilderContext context)
        {
            _context = context;
        }

        // GET: Access/Create
        public IActionResult Register()
        {
            return View();
        }

        // POST: Access/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Email,Password,ConfirmPassword")]Account userToRegister)
        {
            if (!userToRegister.Password.Equals(userToRegister.ConfirmPassword))
            {
                ViewData[accessMessage] = "Passwords don't match";
                return View();
            }

            if (ModelState.IsValid)
            {                
                _context.Add(userToRegister);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Resume");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email,Password")]Account modelLogin)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == modelLogin.Email);
            if (user != null)
            {
                if (user.Password.Equals(modelLogin.Password))
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim("OtherProperties","Example Role")
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = modelLogin.KeepLoggedIn
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);

                    ViewData["UserId"] = user.Id;
                    return RedirectToAction("Index", "Resume");
                }
                else ViewData[accessMessage] = "Password Incorrect";
            }
            else ViewData[accessMessage] = "User not Found";

            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }
                
        // GET: Access/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Accounts.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Access/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password")] Account user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Access/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Access/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Accounts.FindAsync(id);
            if (user != null)
            {
                _context.Accounts.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}
