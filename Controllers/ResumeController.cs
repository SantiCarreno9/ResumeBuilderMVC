using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using ResumeBuilder.Data;
using ResumeBuilder.Models;
using ResumeBuilder.Models.ViewModels;
using System.Text.Json;

namespace ResumeBuilder.Controllers
{
    public class ResumeController : Controller
    {
        private readonly ResumeBuilderContext _context;

        public ResumeController(ResumeBuilderContext context)
        {
            _context = context;
        }

        // GET: Resume
        public async Task<IActionResult> Index()
        {
            return View(await _context.Resumes.Where(x => x.AccountId == User.GetId().ToInt()).ToListAsync());
        }

        // GET: Resume/Details/5
        public async Task<IActionResult> Details(int? id, int? templateId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resume == null)
            {
                return NotFound();
            }

            ViewData["Template"] = "/Views/Resume/Templates/Template" + 1 + ".cshtml";
            var vmResume = ConvertToVMResume(resume);
            return View(vmResume);
        }

        // GET: Resume/Create
        public async Task<IActionResult> Create(VMResume? resume)
        {
            if (resume == null)
            {
                resume = new VMResume();
                resume.PersonalInfo = await _context.PersonalInfo
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProfileInfoId == User.GetId().ToInt());
            }
            return View(resume);
        }

        // POST: Resume/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([Bind("ResumeName,JobTitle,Description,Skills,PersonalInfo,ProfessionalRecord")] VMResume vmResume)
        {
            var resume = ConvertToResume(vmResume);
            resume.AccountId = User.GetId().ToInt();
            if (ModelState.IsValid)
            {
                _context.Add(resume);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vmResume);
        }

        // GET: Resume/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes.FindAsync(id);
            if (resume == null)
            {
                return NotFound();
            }
            return View(resume);
        }

        // POST: Resume/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResumeName,Description,Skills,PersonalInfo,ProfessionalRecord")] VMResume vmResume)
        {
            //if (id != vmResume.Id)
            //{
            //    return NotFound();
            //}

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
            return View(vmResume);
        }

        // GET: Resume/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        // POST: Resume/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resume = await _context.Resumes.FindAsync(id);
            if (resume != null)
            {
                _context.Resumes.Remove(resume);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private Resume ConvertToResume(VMResume vmResume)
        {
            Resume resume = new Resume
            {
                Description = vmResume.Description,
                Skills = vmResume.Skills,
                ResumeName = vmResume.ResumeName,
                JobTitle = vmResume.JobTitle,
            };
            string personalInfoJSON = JsonSerializer.Serialize(vmResume.PersonalInfo);
            var educationRecord = vmResume.ProfessionalRecord?.Where(x => x.Category == EntryCategory.Education);
            string educationRecordJSON = JsonSerializer.Serialize(educationRecord);
            var experienceRecord = vmResume.ProfessionalRecord?.Where(x => x.Category == EntryCategory.WorkExperience);
            string experienceRecordJSON = JsonSerializer.Serialize(experienceRecord);

            resume.PersonalInfo = personalInfoJSON;
            resume.EducationRecord = educationRecordJSON;
            resume.ExperienceRecord = experienceRecordJSON;
            return resume;
        }

        private VMResume ConvertToVMResume(Resume resume)
        {
            VMResume vmResume = new VMResume
            {
                Description = resume.Description,
                Skills = resume.Skills,
                ResumeName = resume.ResumeName,
                JobTitle = resume.JobTitle,
            };
            var personalInfo = JsonSerializer.Deserialize<PersonalInfo>(resume.PersonalInfo);
            var educationRecord = JsonSerializer.Deserialize<ICollection<ProfileEntry>>(resume.EducationRecord);
            var experienceRecord = JsonSerializer.Deserialize<ICollection<ProfileEntry>>(resume.ExperienceRecord);

            vmResume.PersonalInfo = personalInfo;
            vmResume.ProfessionalRecord = new List<ProfileEntry>();
            if (educationRecord != null)
                vmResume.ProfessionalRecord.AddRange(educationRecord);
            if (experienceRecord != null)
                vmResume.ProfessionalRecord.AddRange(experienceRecord);
            return vmResume;
        }

        private bool ResumeExists(int id)
        {
            return _context.Resumes.Any(e => e.Id == id);
        }
    }
}
