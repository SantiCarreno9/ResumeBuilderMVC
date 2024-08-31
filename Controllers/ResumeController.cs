using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: Resume/Details/5
        //public async Task<IActionResult> Details(int? id, int? templateId)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var resume = await _context.Resumes
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (resume == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["Template"] = "/Views/Resume/Templates/Template" + 1 + ".cshtml";
        //    //var vmResume = ConvertToVMResume(resume);
        //    return View(vmResume);
        //}

        public async Task<IActionResult> GetResumePreview(string id, int templateId = 1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (resume == null)
            {
                return NotFound();
            }

            ViewData["Template"] = "/Views/Resume/Templates/Template" + templateId + ".cshtml";
            return View(resume.ConvertToViewModel());
        }

        #region PERSONAL INFO

        [HttpGet]
        public async Task<IActionResult> GetPersonalInfoForm(string resumeId, int actionId)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);

            if (resume == null)
                return NotFound();

            var personalInfo = resume.ConvertToViewModel().PersonalInfo;
            ViewData["Context"] = "Resume";
            ViewData["Action"] = (FormActions)actionId;
            ViewData["ResumeId"] = resumeId;
            if (personalInfo != null)
                return PartialView("/Views/Shared/VMPersonalInfo.cshtml", personalInfo.ConvertToViewModel());
            else return PartialView("/Views/Shared/VMPersonalInfo.cshtml", new PersonalInfo());
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePersonalInfo(string resumeId, VMPersonalInfo vmPersonalInfo)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);

            if (resume == null || !resume.UserId.Equals(User.GetId()) || vmPersonalInfo==null)
                return BadRequest();

            resume.PersonalInfo = JsonSerializer.Serialize(vmPersonalInfo.ConvertToEntity());
            await _context.SaveChangesAsync();

            return Ok(PartialView("/Views/Shared/VMPersonalInfo.cshtml", vmPersonalInfo));
        }

        #endregion

        // POST: Resume/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost, ActionName("Create")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreatePost([Bind("ResumeName,JobTitle,Description,Skills,PersonalInfo,ProfessionalRecord")] VMResume vmResume)
        //{
        //    //var resume = ConvertToResume(vmResume);
        //    resume.AccountId = User.GetId().ToInt();
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(resume);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(vmResume);
        //}

        // GET: Resume/Edit/5
        //[ActionName("ResumeEditor")]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var resume = await _context.Resumes.FindAsync(id);
        //    if (resume == null)
        //    {
        //        return NotFound();
        //    }
        //    //VMResume vmResume = ConvertToVMResume(resume);
        //    return View("ResumeEditor", vmResume);
        //}

        // POST: Resume/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, BasicResumeInfo? resumeInfo, PersonalInfo? personalInfo, List<ProfileEntry>? profileEntries, ProfileEntry? record)
        {
            var resume = await _context.Resumes.FindAsync(id);
            if (resume == null)
                return NotFound();

            if (!resume.UserId.Equals(User.GetId()))
                return NotFound();

            var vmResume = resume.ConvertToViewModel();
            if (resumeInfo != null)
                vmResume.ResumeInfo = resumeInfo;

            if (personalInfo != null)
                vmResume.PersonalInfo = personalInfo;

            if (profileEntries != null)
                vmResume.ProfileEntries = profileEntries;

            if (record != null && vmResume.ProfileEntries != null)
            {
                int index = vmResume.ProfileEntries.FindIndex(pr => pr.Id == record.Id);
                if (index >= 0)
                    vmResume.ProfileEntries[index] = record;
            }

            var tempResume = vmResume.ConvertToEntity();
            resume.ResumeInfo = tempResume.ResumeInfo;
            resume.PersonalInfo = tempResume.PersonalInfo;
            resume.ProfileEntries = tempResume.ProfileEntries;
            await _context.SaveChangesAsync();
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(vmResume);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!ResumeExists(vmResume.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //var vmResume = ConvertToVMResume(resume);
            return View(vmResume);
        }

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
