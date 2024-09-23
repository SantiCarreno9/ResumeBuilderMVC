using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ResumeBuilder.Data;
using ResumeBuilder.Models;
using ResumeBuilder.Models.Extensions;
using ResumeBuilder.Models.ViewModels;
using System.Text.Json;

namespace ResumeBuilder.Controllers
{
    [Authorize]
    public class ResumeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResumeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Resume
        public async Task<IActionResult> Index()
        {
            var resumes = await _context.Resumes.Where(x => x.UserId.Equals(User.GetId())).ToListAsync();
            var vmResumes = new List<VMResume>();
            for (int i = 0; i < resumes.Count; i++)
                vmResumes.Add(resumes[i].ConvertToViewModel());

            return View(vmResumes);
        }

        // GET: Resume/Create        
        public async Task<IActionResult> Create()
        {
            string userId = User.GetId();
            var vmResume = new VMResume();

            vmResume.Id = Guid.NewGuid().ToString();
            vmResume.ResumeInfo = new();
            vmResume.PersonalInfo = await _context.PersonalInfo
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId.Equals(userId));
            vmResume.ProfileEntries = await _context.ProfileEntry
                .AsNoTracking()
                .Where(x => x.UserId.Equals(userId))
                .ToListAsync();

            var resume = vmResume.ConvertToEntity();
            resume.UserId = userId;
            _context.Add(resume);
            await _context.SaveChangesAsync();

            return View("ResumeEditor", vmResume);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var resume = await _context.Resumes.FindAsync(id);

            if (resume == null || !resume.UserId.Equals(User.GetId()))
                return NotFound();

            return View("ResumeEditor", resume.ConvertToViewModel());
        }

        [ActionName("ResumePreview")]
        [HttpGet]
        public async Task<IActionResult> GetResumePreview(string resumeId, int templateId = 1)
        {
            if (resumeId == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id.Equals(resumeId));
            if (resume == null)
            {
                return NotFound();
            }

            var vmResume = resume.ConvertToViewModel();

            ViewData["Template"] = "/Views/Resume/Templates/Template" + templateId + ".cshtml";
            IEnumerable<AdditionalContactResume>? additionalContactInfo = null;
            if (vmResume.PersonalInfo != null && vmResume.PersonalInfo.AdditionalContactInfo != null)            
                additionalContactInfo = JsonConvert.DeserializeObject<IEnumerable<AdditionalContactResume>>(vmResume.PersonalInfo.AdditionalContactInfo);

            ViewData["AdditionalContactInfo"] = additionalContactInfo;
            return PartialView("/Views/Resume/Templates/Template" + templateId + ".cshtml", vmResume);
        }

        #region RESUME

        [ActionName("ResumeInfo")]
        [HttpGet]
        public async Task<IActionResult> GetResumeInfo(string resumeId)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null)
                return NotFound();

            if (!resume.UserId.Equals(User.GetId()))
                return BadRequest();

            return Ok(resume);
        }

        [ActionName("ResumeInfo")]
        [HttpPut]
        public async Task<IActionResult> UpdateResumeInfo(string resumeId, Resume resume)
        {
            var originalResume = await _context.Resumes.FindAsync(resumeId);

            if (originalResume == null || !originalResume.UserId.Equals(User.GetId()) || resume == null)
                return BadRequest();

            if (await TryUpdateModelAsync(
                originalResume,
                "",
                i => i.ResumeInfo, i => i.PersonalInfo, i => i.ProfileEntries))
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
            return Ok(resume);
        }

        #endregion

        #region RESUME NAME

        [ActionName("ResumeName")]
        [HttpGet]
        public async Task<IActionResult> GetResumeName(string resumeId)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null)
                return NotFound();

            if (!resume.UserId.Equals(User.GetId()))
                return BadRequest();


            return Ok(resume.Name);
        }

        [ActionName("ResumeName")]
        [HttpPut]
        public async Task<IActionResult> UpdateResumeName(string resumeId, string newName)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);

            if (resume == null || !resume.UserId.Equals(User.GetId()) || string.IsNullOrEmpty(newName))
                return BadRequest();

            resume.Name = newName;
            await _context.SaveChangesAsync();

            return Ok(resume);
        }

        #endregion

        #region BASIC RESUME INFO

        [ActionName("BasicResumeInfo")]
        [HttpGet]
        public async Task<IActionResult> GetBasicResumeInfo(string resumeId)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null)
                return NotFound();

            if (!resume.UserId.Equals(User.GetId()))
                return BadRequest();

            var resumeInfo = resume.ConvertToViewModel().ResumeInfo;
            if (resumeInfo != null)
                return Ok(resume.ConvertToViewModel().ResumeInfo);
            else return NoContent();
        }

        [ActionName("BasicResumeInfoForm")]
        [HttpGet]
        public async Task<IActionResult> GetBasicResumeInfoForm(string resumeId, int actionId)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);

            if (resume == null)
                return NotFound();

            if (!resume.UserId.Equals(User.GetId()))
                return BadRequest();

            var resumeInfo = resume.ConvertToViewModel().ResumeInfo;
            ViewData["Context"] = "Resume";
            ViewData["Action"] = (FormActions)actionId;
            ViewData["ResumeId"] = resumeId;
            if (resumeInfo != null)
                return PartialView("/Views/Shared/BasicResumeInfo.cshtml", resumeInfo);
            else return PartialView("/Views/Shared/BasicResumeInfo.cshtml", new BasicResumeInfo());
        }

        [ActionName("BasicResumeInfo")]
        [HttpPut]
        public async Task<IActionResult> UpdateBasicResumeInfo(string resumeId, BasicResumeInfo resumeInfo)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);

            if (resume == null || !resume.UserId.Equals(User.GetId()) || resumeInfo == null)
                return BadRequest();

            resume.ResumeInfo = JsonConvert.SerializeObject(resumeInfo);
            await _context.SaveChangesAsync();

            return Ok(resumeInfo);
        }

        #endregion

        #region PERSONAL INFO

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
        public async Task<IActionResult> GetPersonalInfoForm(string resumeId, int actionId)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);

            if (resume == null)
                return NotFound();

            if (!resume.UserId.Equals(User.GetId()))
                return BadRequest();

            var personalInfo = resume.ConvertToViewModel().PersonalInfo;
            ViewData["Context"] = "Resume";
            ViewData["Action"] = (FormActions)actionId;
            ViewData["ResumeId"] = resumeId;
            if (personalInfo != null)
                return PartialView("/Views/Shared/VMPersonalInfo.cshtml", personalInfo.ConvertToViewModel());
            else return PartialView("/Views/Shared/VMPersonalInfo.cshtml", new PersonalInfo());
        }

        [ActionName("PersonalInfo")]
        [HttpPut]
        public async Task<IActionResult> UpdatePersonalInfo(string resumeId, VMPersonalInfo vmPersonalInfo)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);

            if (resume == null || !resume.UserId.Equals(User.GetId()) || vmPersonalInfo == null)
                return BadRequest();

            resume.PersonalInfo = JsonConvert.SerializeObject(vmPersonalInfo.ConvertToEntity());
            await _context.SaveChangesAsync();

            return Ok(PartialView("/Views/Shared/VMPersonalInfo.cshtml", vmPersonalInfo));
        }

        #endregion

        #region PROFILE ENTRIES

        [ActionName("ProfileEntry")]
        [HttpGet]
        public async Task<IActionResult> GetProfileEntry(string resumeId, string id)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null)
                return NotFound();

            if (!resume.UserId.Equals(User.GetId()))
                return BadRequest();

            var vmResume = resume.ConvertToViewModel();
            if (vmResume == null || vmResume.ProfileEntries == null)
                return BadRequest();

            var profileEntry = vmResume.ProfileEntries.FirstOrDefault(pe => pe.Id.Equals(id));
            if (profileEntry == null)
                return NotFound();

            return PartialView("/Views/Shared/DisplayTemplates/VMProfileEntry.cshtml", profileEntry.ConvertToViewModel());
        }

        [ActionName("ProfileEntries")]
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

        [ActionName("ProfileEntryForm")]
        [HttpGet]
        public async Task<IActionResult> GetProfileEntryForm(string? id, string? resumeId, EntryCategory? category, FormActions? actionId)
        {
            ProfileEntry? profileEntry = null;
            if (id != null)
            {
                if (resumeId == null)
                    return BadRequest();

                var resume = await _context.Resumes.FindAsync(resumeId);
                if (resume == null)
                    return NotFound();

                var vmResume = resume.ConvertToViewModel();
                profileEntry = vmResume.ProfileEntries.FirstOrDefault(pe => pe.Id.Equals(id));
                //profileEntry = await _context.Resumes.FindAsync(resumeId);
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
            //return PartialView("/Views/Shared/VMProfileEntry.cshtml", vmProfileEntry);
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

        // GET: Resume/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        // POST: Resume/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var resume = await _context.Resumes.FindAsync(id);
            if (resume != null)
            {
                _context.Resumes.Remove(resume);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
