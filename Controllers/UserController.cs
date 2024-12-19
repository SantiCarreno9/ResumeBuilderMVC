using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ResumeBuilder.Models;
using ResumeBuilder.Models.Extensions;
using ResumeBuilder.Models.ViewModels;
using ResumeBuilder.Repositories.Contracts;
using System;

namespace ResumeBuilder.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            if (await _repository.GetPersonalInfo(User.GetId()) == null)
            {
                PersonalInfo personalInfo = new PersonalInfo();
                personalInfo.Email = User.Identity.Name;
                personalInfo.UserId = User.GetId();
                await _repository.AddPersonalInfo(personalInfo);
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
            var personalInfo = await _repository.GetPersonalInfo(User.GetId());
            if (personalInfo != null)
                return Ok(personalInfo);
            return NotFound();
        }

        [ActionName("PersonalInfoView")]
        [HttpGet]
        public async Task<IActionResult> GetPersonalInfoView()
        {
            var personalInfo = await _repository.GetPersonalInfo(User.GetId());

            ViewData["Context"] = "User";
            if (personalInfo == null)
                return PartialView("/Views/Shared/DisplayTemplates/VMPersonalInfo.cshtml", new PersonalInfo());

            if (personalInfo.AdditionalContactInfo != null)
                ViewData["Contacts"] = JsonConvert.DeserializeObject<List<AdditionalContact>>(personalInfo.AdditionalContactInfo);
            return PartialView("/Views/Shared/DisplayTemplates/VMPersonalInfo.cshtml", personalInfo.ConvertToViewModel());
        }

        [ActionName("PersonalInfoForm")]
        [HttpGet]
        public async Task<IActionResult> GetPersonalInfoForm()
        {
            var personalInfo = await _repository.GetPersonalInfo(User.GetId());
            ViewData["Context"] = "User";
            if (personalInfo != null)
                return PartialView("/Views/Shared/EditorTemplates/VMPersonalInfo.cshtml", personalInfo.ConvertToViewModel());
            else return PartialView("/Views/Shared/EditorTemplates/VMPersonalInfo.cshtml", new PersonalInfo());
        }

        [ActionName("PersonalInfo")]
        [HttpPut]
        public async Task<IActionResult> UpdatePersonalInfo(VMPersonalInfo vmPersonalInfo)
        {
            var personalInfo = await _repository.UpdatePersonalInfo(User.GetId(), vmPersonalInfo.ConvertToEntity());
            if (personalInfo != null)
                return Ok(personalInfo);

            return NotFound();
        }

        #endregion

        #region PROFILE ENTRIES

        [ActionName("ProfileEntryView")]
        [HttpGet]
        public async Task<IActionResult> GetProfileEntryView(string id)
        {
            var profileEntry = await _repository.GetProfileEntry(User.GetId(), id);
            if (profileEntry == null)
                return NotFound();
            ViewData["Context"] = "User";
            return PartialView("/Views/Shared/DisplayTemplates/VMProfileEntry.cshtml", profileEntry.ConvertToViewModel());
        }

        [ActionName("ProfileEntriesView")]
        [HttpGet]
        public async Task<IActionResult> GetProfileEntriesView(EntryCategory category)
        {
            var profileEntries = await _repository.GetProfileEntriesByCategory(User.GetId(), category);

            if (profileEntries == null)
                return NoContent();

            ViewData["Context"] = "User";
            List<VMProfileEntry> vMProfileEntries = new();
            foreach (ProfileEntry entry in profileEntries)
                vMProfileEntries.Add(entry.ConvertToViewModel());

            return PartialView("/Views/Shared/VMProfileEntries.cshtml", vMProfileEntries);
        }

        [ActionName("ProfileEntryForm")]
        [HttpGet]
        public async Task<IActionResult> GetProfileEntryForm(string? id, EntryCategory? category)
        {
            ProfileEntry? profileEntry = null;
            if (id != null)
            {
                profileEntry = await _repository.GetProfileEntry(User.GetId(), id);
            }
            if (profileEntry == null)
            {
                profileEntry = new ProfileEntry();
                profileEntry.Id = Guid.NewGuid().ToString();
                profileEntry.Category = category ?? EntryCategory.WorkExperience;
            }

            ViewData["Context"] = "User";
            return PartialView("/Views/Shared/EditorTemplates/VMProfileEntry.cshtml", profileEntry.ConvertToViewModel());
        }

        [ActionName("ProfileEntry")]
        [HttpPost]
        public async Task<IActionResult> UpdateOrAddProfileEntry(VMProfileEntry vmProfileEntry)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(vmProfileEntry.Id))
                {
                    var existingProfileEntry = await _repository.UpdateProfileEntry(User.GetId(), vmProfileEntry.Id, vmProfileEntry.ConvertToEntity());
                    if (existingProfileEntry != null)
                        return Ok(existingProfileEntry);
                }

                var profileEntry = await _repository.AddProfileEntry(vmProfileEntry.ConvertToEntity());
                if (profileEntry != null)
                    return CreatedAtAction("ProfileEntry", profileEntry);

            }

            return BadRequest();
        }

        // POST: User/Delete/5
        [ActionName("ProfileEntry")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProfileEntry(string id)
        {
            await _repository.DeleteProfileEntry(User.GetId(), id);
            return NoContent();
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

            await _repository.DeletePersonalInfo(User.GetId());
            return NoContent();
        }

        // POST: User/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var userInfo = await _context.PersonalInfo.FindAsync(id);
        //    if (userInfo != null)
        //    {
        //        _context.PersonalInfo.Remove(userInfo);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserInfoExists(string id)
        //{
        //    return _context.PersonalInfo.Any(e => e.UserId.Equals(id));
        //}
    }
}
