using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Data;
using ResumeBuilder.Models;
using ResumeBuilder.Models.Extensions;
using ResumeBuilder.Models.ViewModels;

namespace ResumeBuilder.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;

        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            if (!_context.PersonalInfo.Any() || await _context.PersonalInfo.FindAsync(User.GetId()) == null)
            {
                return RedirectToAction("Create");
            }
            return RedirectToAction("Details");
        }

        #region PERSONAL INFO

        // GET: User/Create
        public IActionResult Create()
        {
            var personalInfo = new VMPersonalInfo();
            personalInfo.Email = User.Identity.Name;
            return View(personalInfo);
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonalInfoForm(int actionId)
        {
            var personalInfo = await _context.PersonalInfo.FindAsync(User.GetId());
            ViewData["Context"] = "User";
            ViewData["Action"] = (FormActions)actionId;
            if (personalInfo != null)
                return PartialView("/Views/Shared/VMPersonalInfo.cshtml", personalInfo.ConvertToViewModel());
            else return PartialView("/Views/Shared/VMPersonalInfo.cshtml", new PersonalInfo());
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePersonalInfo(VMPersonalInfo vmPersonalInfo)
        {
            var personalInfo = await _context.PersonalInfo.FindAsync(User.GetId());
            if (await TryUpdateModelAsync(
                personalInfo,
                "",
                i => i.FirstName, i => i.LastName, i => i.Address, i => i.PhoneNumber,
                i => i.Address, i => i.PostalCode, i => i.LinkedInURL, i => i.WebsiteURL, i => i.GitHubAccount))
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
            }
            return Ok(PartialView("/Views/Shared/VMPersonalInfo.cshtml", personalInfo));
        }

        #endregion

        #region PROFILE ENTRIES

        [HttpGet]
        public async Task<IActionResult> GetProfileEntry(string id)
        {
            var profileEntry = await _context.ProfileEntry.FindAsync(id);
            if (profileEntry == null)
                return NotFound();
            
            return PartialView("/Views/Shared/DisplayTemplates/VMProfileEntry.cshtml", profileEntry.ConvertToViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetProfileEntries(EntryCategory category)
        {
            var profileEntries = await _context.ProfileEntry
                .AsNoTracking()
                .Where(m => m.UserId == User.GetId() && m.Category == category)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();

            if (profileEntries == null)
                return NoContent();

            List<VMProfileEntry> vMProfileEntries = new();
            for (int i = 0; i < profileEntries.Count; i++)
                vMProfileEntries.Add(profileEntries[i].ConvertToViewModel());            

            return PartialView("/Views/Shared/VMProfileEntries.cshtml", vMProfileEntries);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfileEntryForm(string? id, EntryCategory? category, FormActions? actionId)
        {
            ProfileEntry? profileEntry = null;
            if (id != null)
            {
                profileEntry = await _context.ProfileEntry.FindAsync(id);
            }
            if (profileEntry == null)
            {
                profileEntry = new ProfileEntry();
                profileEntry.Id = Guid.NewGuid().ToString();
                profileEntry.Category = category?? EntryCategory.WorkExperience;
            }

            ViewData["Action"] = actionId ?? FormActions.Edit;
            return PartialView("/Views/Shared/EditorTemplates/VMProfileEntry.cshtml", profileEntry.ConvertToViewModel());
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProfileEntry(VMProfileEntry vmProfileEntry)
        {
            if (ModelState.IsValid)
            {
                var profileEntry = vmProfileEntry.ConvertToEntity();
                profileEntry.UserId = User.GetId();
                _context.Add(profileEntry);
                await _context.SaveChangesAsync();
                return Ok(profileEntry);
            }            
            //return PartialView("/Views/Shared/VMProfileEntry.cshtml", vmProfileEntry);
            return CreatedAtAction("AddProfileEntry",vmProfileEntry);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfileEntry(VMProfileEntry vmProfileEntry)
        {
            var profileEntry = await _context.ProfileEntry.FindAsync(vmProfileEntry.Id);
            if (await TryUpdateModelAsync(
                profileEntry,
                "",
                i => i.Title, i => i.Organization, i => i.Location, i => i.StartDate,
                i => i.EndDate, i => i.IsCurrent, i => i.Details, i => i.Category))
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
            }
            return Ok(vmProfileEntry);
        }

        // POST: User/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteProfileEntry(string id)
        {
            var profileEntry = await _context.ProfileEntry.FindAsync(id);
            if (profileEntry != null && profileEntry.UserId.Equals(User.GetId()))
            {
                _context.ProfileEntry.Remove(profileEntry);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest();
        }

        #endregion
        //// POST: User/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(VMPersonalInfo vmPersonalInfo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        vmPersonalInfo.Email = User.Identity.Name;
        //        var personalInfo = vmPersonalInfo.ConvertToEntity();
        //        personalInfo.UserId = User.GetId();
        //        _context.Add(personalInfo);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(vmPersonalInfo);
        //}

        //// GET: User/Edit/5
        //public async Task<IActionResult> EditPersonalInfo(int? id)
        //{
        //    PersonalInfo? userInfo = await _context.PersonalInfo
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(x => x.UserId.Equals(User.GetId()));
        //    if (userInfo == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(userInfo);
        //}

        //// POST: User/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditProfileInfo([Bind("FirstName,LastName,PhoneNumber,Address,LinkedInProfile,WebSiteURL,GitHubAccount")] PersonalInfo personalInfo)
        //{
        //    var userInfoToUpdate = await _context.PersonalInfo.FindAsync(User.GetId().ToInt());
        //    if (await TryUpdateModelAsync(
        //        userInfoToUpdate,
        //        "",
        //        i => i.FirstName, i => i.LastName, i => i.PhoneNumber, i => i.Address,
        //        i => i.LinkedInURL, i => i.WebsiteURL, i => i.GitHubAccount))
        //    {
        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateException /* ex */)
        //        {
        //            //Log the error (uncomment ex variable name and write a log.)
        //            ModelState.AddModelError("", "Unable to save changes. " +
        //                "Try again, and if the problem persists, " +
        //                "see your system administrator.");
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}

        #region PROFESSIONAL INFO

        // GET: User/Create
        //public IActionResult AddProfileEntry(EntryCategory category)
        //{
        //    EntryCategory entryCategory = category;
        //    UpdateProfileEntryViewData(entryCategory);

        //    //ViewData
        //    ViewData["Title"] = "Add";
        //    ViewData["Action"] = "AddProfileEntry";

        //    ProfileEntry profileEntry = new ProfileEntry();
        //    profileEntry.Category = category;
        //    //ViewData["RouteValues"] = new RouteValueDictionary { { "action", "AddProfileEntry" }, { "category", category } };            
        //    return PartialView("ProfileEntryEditor", profileEntry);
        //}

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddProfileEntry([Bind("Title,Category,Organization,Location,StartDate,EndDate,IsCurrent,Details")] ProfileEntry profileEntry)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        profileEntry.UserId = User.GetId();
        //        _context.Add(profileEntry);
        //        await _context.SaveChangesAsync();
        //        //return Redirect(HttpContext.Request.Path);
        //        return Ok(profileEntry); //RedirectToAction(nameof(Index));
        //    }
        //    return PartialView("ProfileEntryEditor", new ProfileEntry());
        //}

        // GET: User/Edit/5
        //public async Task<IActionResult> EditProfileEntry(int? id)
        //{
        //    ProfileEntry? profileEntry = await _context.ProfileEntry.FindAsync(id);
        //    if (profileEntry == null || !profileEntry.UserId.Equals(User.GetId()))
        //    {
        //        return NotFound();
        //    }
        //    UpdateProfileEntryViewData(profileEntry.Category);

        //    //ViewData
        //    ViewData["Title"] = "Edit";
        //    ViewData["Action"] = "EditProfileEntry";

        //    return View("ProfileEntryEditor", profileEntry);
        //}

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditProfileEntry(int id, [Bind("Title,Organization,Location,StartDate,EndDate,IsCurrent,Details")] ProfileEntry profileEntry)
        //{
        //    var profileInfoToUpdate = await _context.ProfileEntry.FindAsync(id);
        //    if (await TryUpdateModelAsync(
        //        profileInfoToUpdate,
        //        "",
        //        i => i.Title, i => i.Organization, i => i.StartDate, i => i.EndDate,
        //        i => i.IsCurrent, i => i.Details))
        //    {
        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateException /* ex */)
        //        {
        //            //Log the error (uncomment ex variable name and write a log.)
        //            ModelState.AddModelError("", "Unable to save changes. " +
        //                "Try again, and if the problem persists, " +
        //                "see your system administrator.");
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}

        //private void UpdateProfileEntryViewData(EntryCategory entryCategory)
        //{
        //    ViewData["EntryCategory"] = (int)entryCategory;
        //    switch (entryCategory)
        //    {
        //        case EntryCategory.Education:
        //            ViewData["EntryCategoryText"] = "Education";
        //            ViewData["Organization"] = "Institution";
        //            break;
        //        case EntryCategory.WorkExperience:
        //            ViewData["EntryCategoryText"] = "Experience";
        //            ViewData["Organization"] = "Organization";
        //            break;
        //        case EntryCategory.Project:
        //            ViewData["EntryCategoryText"] = "Project";
        //            ViewData["Organization"] = "Project";
        //            break;
        //        case EntryCategory.Other:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        #endregion                

        public IActionResult Details()
        {
            return View();
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.PersonalInfo
                .FirstOrDefaultAsync(m => m.UserId.Equals(id));
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

        private bool UserInfoExists(string id)
        {
            return _context.PersonalInfo.Any(e => e.UserId.Equals(id));
        }
    }
}
