using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Data;
using ResumeBuilder.Models;

namespace ResumeBuilder.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ResumeBuilderContext _context;

        public UserController(ResumeBuilderContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            if (!_context.PersonalInfo.Any() || await _context.PersonalInfo.FindAsync(User.GetId().ToInt()) == null)
            {
                return RedirectToAction("Create");
            }
            return RedirectToAction("Details");
        }

        #region BASIC INFO

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,PhoneNumber,Address,LinkedInProfile,WebSiteURL,GitHubAccount")] PersonalInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                userInfo.ProfileInfoId = int.Parse(User.GetId());
                _context.Add(userInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> EditProfileInfo(int? id)
        {
            PersonalInfo? userInfo = await _context.PersonalInfo
                .Include(i => i.ProfileInfo)
                .ThenInclude(i => i.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProfileInfoId == User.GetId().ToInt());
            if (userInfo == null)
            {
                return NotFound();
            }
            return View(userInfo);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfileInfo([Bind("FirstName,LastName,PhoneNumber,Address,LinkedInProfile,WebSiteURL,GitHubAccount")] PersonalInfo personalInfo)
        {
            var userInfoToUpdate = await _context.PersonalInfo.FindAsync(User.GetId().ToInt());
            if (await TryUpdateModelAsync(
                userInfoToUpdate,
                "",
                i => i.FirstName, i => i.LastName, i => i.PhoneNumber, i => i.Address,
                i => i.LinkedInURL, i => i.WebSiteURL, i => i.GitHubAccount))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        #endregion

        #region PROFESSIONAL INFO

        // GET: User/Create
        public IActionResult AddProfileEntry(int category)
        {
            EntryCategory entryCategory = (EntryCategory)category;
            UpdateProfileEntryViewData(entryCategory);
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProfileEntry(int category, [Bind("Title,Organization,Location,StartDate,EndDate,IsCurrent,Details")] ProfileEntry profileEntry)
        {
            if (ModelState.IsValid)
            {
                profileEntry.ProfileInfoId = User.GetId().ToInt();
                profileEntry.Category = (EntryCategory)category;
                _context.Add(profileEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profileEntry);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> EditProfileEntry(int? id)
        {
            ProfileEntry? profileEntry = await _context.ProfileEntry.FindAsync(id);
            if (profileEntry == null || profileEntry.ProfileInfoId != User.GetId().ToInt())
            {
                return NotFound();
            }
            UpdateProfileEntryViewData(profileEntry.Category);
            return View(profileEntry);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfileEntry(int id, [Bind("Title,Organization,Location,StartDate,EndDate,IsCurrent,Details")] ProfileEntry profileEntry)
        {
            var profileInfoToUpdate = await _context.ProfileEntry.FindAsync(id);
            if (await TryUpdateModelAsync(
                profileInfoToUpdate,
                "",
                i => i.Title, i => i.Organization, i => i.StartDate, i => i.EndDate,
                i => i.IsCurrent, i => i.Details))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private void UpdateProfileEntryViewData(EntryCategory entryCategory)
        {
            ViewData["EntryCategory"] = (int)entryCategory;
            switch (entryCategory)
            {
                case EntryCategory.Education:
                    ViewData["EntryCategoryText"] = "Education";
                    ViewData["Organization"] = "Institution";
                    break;
                case EntryCategory.WorkExperience:
                    ViewData["EntryCategoryText"] = "Experience";
                    ViewData["Organization"] = "Organization";
                    break;
                case EntryCategory.Project:
                    ViewData["EntryCategoryText"] = "Project";
                    break;
                case EntryCategory.Other:
                    break;
                default:
                    break;
            }
        }

        #endregion

        // GET: User/Details/5
        public async Task<IActionResult> Details()
        {
            if (User.GetId() == null)
            {
                return NotFound();
            }

            var userInfo = await _context.ProfileInfo
                .Include(i => i.PersonalInfo)
                .Include(i => i.ProfessionalRecord)
                .Include(i => i.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AccountId == User.GetId().ToInt());
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.PersonalInfo
                .FirstOrDefaultAsync(m => m.ProfileInfoId == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userInfo = await _context.PersonalInfo.FindAsync(id);
            if (userInfo != null)
            {
                _context.PersonalInfo.Remove(userInfo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProfileEntry(int id)
        {
            var profileEntry = await _context.ProfileEntry.FindAsync(id);
            if (profileEntry != null)
            {
                _context.ProfileEntry.Remove(profileEntry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInfoExists(int id)
        {
            return _context.PersonalInfo.Any(e => e.ProfileInfoId == id);
        }
    }
}
