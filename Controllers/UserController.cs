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
                PersonalInfo personalInfo= new PersonalInfo();
                personalInfo.Email = User.Identity.Name;
                personalInfo.UserId = User.GetId();
                _context.Add(personalInfo);
                await _context.SaveChangesAsync();                
            }
            return View();
        }

        #region PERSONAL INFO

        // GET: User/Create
        public IActionResult Create()
        {
            var personalInfo = new VMPersonalInfo();
            personalInfo.Email = User.Identity.Name;
            return View(personalInfo);
        }

        [ActionName("PersonalInfo")]
        [HttpGet]
        public async Task<IActionResult> GetPersonalInfo()
        {
            var personalInfo = await _context.PersonalInfo.FindAsync(User.GetId());            
            if (personalInfo != null)
                return Ok(personalInfo.ConvertToViewModel());
            return NotFound();
        }

        [ActionName("PersonalInfoForm")]
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

        [ActionName("PersonalInfo")]
        [HttpPut]
        public async Task<IActionResult> UpdatePersonalInfo(VMPersonalInfo vmPersonalInfo)
        {
            var personalInfo = await _context.PersonalInfo.FindAsync(User.GetId());
            if (await TryUpdateModelAsync(
                personalInfo,
                "",
                i => i.FirstName, i => i.LastName, i => i.Address, i => i.PhoneNumber,
                i => i.Address, i => i.AdditionalContactInfo))
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

        [ActionName("ProfileEntryView")]
        [HttpGet]
        public async Task<IActionResult> GetProfileEntryView(string id)
        {
            var profileEntry = await _context.ProfileEntry.FindAsync(id);
            if (profileEntry == null)
                return NotFound();

            return PartialView("/Views/Shared/DisplayTemplates/VMProfileEntry.cshtml", profileEntry.ConvertToViewModel());
        }

        [ActionName("ProfileEntriesView")]
        [HttpGet]
        public async Task<IActionResult> GetProfileEntriesView(EntryCategory category)
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

        [ActionName("ProfileEntryForm")]
        [HttpGet]
        public async Task<IActionResult> GetProfileEntryForm(string? id, EntryCategory? category, FormActions? actionId)
        {
            ProfileEntry? profileEntry = null;
            if (id != null)
            {
                profileEntry = await _context.ProfileEntry.FindAsync(id);
                if (profileEntry == null)
                    return NotFound();
                if (!profileEntry.UserId.Equals(User.GetId()))
                    return BadRequest();
            }
            if (profileEntry == null)
            {
                profileEntry = new ProfileEntry();
                profileEntry.Id = Guid.NewGuid().ToString();
                profileEntry.Category = category ?? EntryCategory.WorkExperience;
            }

            ViewData["Action"] = actionId ?? FormActions.Edit;
            return PartialView("/Views/Shared/EditorTemplates/VMProfileEntry.cshtml", profileEntry.ConvertToViewModel());
        }

        [ActionName("ProfileEntry")]
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
            return CreatedAtAction("AddProfileEntry", vmProfileEntry);
        }

        [ActionName("ProfileEntry")]
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
        [ActionName("ProfileEntry")]
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
