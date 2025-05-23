﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ResumeBuilder.Models;
using ResumeBuilder.Models.Extensions;
using ResumeBuilder.Models.ViewModels;
using ResumeBuilder.Repositories.Contracts;

namespace ResumeBuilder.Controllers
{
    [Authorize]
    public class ResumeController : Controller
    {
        private readonly IResumeRepository _resumeRepository;
        private readonly IUserRepository _userRepository;

        public ResumeController(IResumeRepository repository, IUserRepository userRepository)
        {
            _resumeRepository = repository;
            _userRepository = userRepository;
        }

        // GET: Resume
        public async Task<IActionResult> Index()
        {
            var resumes = await _resumeRepository.GetResumes(User.GetId());
            var vmResumes = new List<VMResume>();
            if (resumes != null)
            {
                foreach (var resume in resumes)
                    vmResumes.Add(resume.ConvertToViewModel());
            }

            return View(vmResumes);
        }

        // GET: Resume/Create        
        public async Task<IActionResult> Create()
        {
            string userId = User.GetId();
            var vmResume = new VMResume();

            vmResume.Id = Guid.NewGuid().ToString();
            vmResume.ResumeInfo = new();
            var personalInfo = await _userRepository.GetPersonalInfo(User.GetId());
            if (personalInfo != null)
                vmResume.PersonalInfo = personalInfo.ConvertToViewModel();

            var resume = vmResume.ConvertToEntity();
            resume.UserId = userId;
            resume.OrderedCategories = JsonConvert.SerializeObject(Enum.GetNames(typeof(EntryCategory)));
            await _resumeRepository.AddResume(resume);

            return View("ResumeEditor", vmResume);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var resume = await _resumeRepository.GetResume(User.GetId(), id);

            if (resume == null)
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

            var resume = await _resumeRepository.GetResume(User.GetId(), resumeId);
            if (resume == null)
            {
                return NotFound();
            }

            var vmResume = resume.ConvertToViewModel();
            
            ViewData["Template"] = "/Views/Resume/Templates/Template" + templateId + ".cshtml";

            return PartialView("/Views/Resume/Templates/Template" + templateId + ".cshtml", vmResume);
        }

        #region RESUME

        [ActionName("ResumeInfo")]
        [HttpGet]
        public async Task<IActionResult> GetResumeInfo(string resumeId)
        {
            var resume = await _resumeRepository.GetResume(User.GetId(), resumeId);
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
            var updatedResume = await _resumeRepository.UpdateResume(User.GetId(), resumeId, resume);
            if (updatedResume == null)
                NotFound();
            return Ok(updatedResume);
        }

        #endregion

        #region RESUME NAME

        [ActionName("ResumeName")]
        [HttpGet]
        public async Task<IActionResult> GetResumeName(string resumeId)
        {
            var resumeName = await _resumeRepository.GetResumeName(User.GetId(), resumeId);
            if (resumeName == null)
                return NotFound();

            return Ok(resumeName);
        }

        [ActionName("ResumeName")]
        [HttpPut]
        public async Task<IActionResult> UpdateResumeName(string resumeId, string newName)
        {
            var resume = await _resumeRepository.UpdateResumeName(User.GetId(), resumeId, newName);
            if (resume == null)
                NotFound();

            return Ok(resume);
        }

        #endregion

        #region BASIC INFO

        [ActionName("ResumeBasicInfo")]
        [HttpGet]
        public async Task<IActionResult> GetResumeBasicInfo(string resumeId)
        {
            var basicInfo = await _resumeRepository.GetResumeBasicInfo(User.GetId(), resumeId);
            if (basicInfo == null)
                return NotFound();

            return Ok(basicInfo);
        }

        [ActionName("ResumeBasicInfoView")]
        [HttpGet]
        public async Task<IActionResult> GetResumeBasicInfoView(string resumeId)
        {
            var basicInfo = await _resumeRepository.GetResumeBasicInfo(User.GetId(), resumeId);

            ViewData["ResumeId"] = resumeId;
            var vmBasicInfo = basicInfo?.ConvertToViewModel();
            if (vmBasicInfo != null)
                return PartialView("/Views/Shared/DisplayTemplates/ResumeBasicInfo.cshtml", vmBasicInfo);
            else return PartialView("/Views/Shared/DisplayTemplates/ResumeBasicInfo.cshtml", new VMResumeBasicInfo());
        }

        [ActionName("ResumeBasicInfoForm")]
        [HttpGet]
        public async Task<IActionResult> GetResumeBasicInfoForm(string resumeId)
        {
            var basicInfo = await _resumeRepository.GetResumeBasicInfo(User.GetId(), resumeId);

            ViewData["ResumeId"] = resumeId;
            var vmBasicInfo = basicInfo?.ConvertToViewModel();
            if (vmBasicInfo != null)
                return PartialView("/Views/Shared/EditorTemplates/ResumeBasicInfo.cshtml", vmBasicInfo);
            else return PartialView("/Views/Shared/EditorTemplates/ResumeBasicInfo.cshtml", new VMResumeBasicInfo());
        }

        [ActionName("ResumeBasicInfo")]
        [HttpPut]
        public async Task<IActionResult> UpdateResumeBasicInfo(string resumeId, ResumeBasicInfo resumeInfo)
        {
            var resume = await _resumeRepository.UpdateResumeBasicInfo(User.GetId(), resumeId, resumeInfo);

            if (resume == null)
                return NotFound();

            return Ok(resumeInfo);
        }

        #endregion

        #region PERSONAL INFO

        [ActionName("ResumePersonalInfo")]
        [HttpGet]
        public async Task<IActionResult> GetResumePersonalInfo(string resumeId)
        {
            var personalInfo = await _resumeRepository.GetResumePersonalInfo(User.GetId(), resumeId);
            if (personalInfo == null)
                return NotFound();

            return Ok(personalInfo.ConvertToViewModel());
        }

        [ActionName("PersonalInfoView")]
        [HttpGet]
        public async Task<IActionResult> GetPersonalInfoView(string resumeId)
        {
            var personalInfo = await _resumeRepository.GetResumePersonalInfo(User.GetId(), resumeId);

            ViewData["Context"] = "Resume";
            ViewData["ResumeId"] = resumeId;
            if (personalInfo == null)
                return PartialView("/Views/Shared/DisplayTemplates/VMPersonalInfo.cshtml", new VMPersonalInfo());

            if (personalInfo.AdditionalContactInfo != null)
                ViewData["Contacts"] = JsonConvert.DeserializeObject<List<AdditionalContact>>(personalInfo.AdditionalContactInfo);
            return PartialView("/Views/Shared/DisplayTemplates/VMPersonalInfo.cshtml", personalInfo.ConvertToViewModel());
        }

        [ActionName("PersonalInfoForm")]
        [HttpGet]
        public async Task<IActionResult> GetPersonalInfoForm(string resumeId, int actionId)
        {
            var personalInfo = await _resumeRepository.GetResumePersonalInfo(User.GetId(), resumeId);

            ViewData["Context"] = "Resume";
            ViewData["ResumeId"] = resumeId;
            if (personalInfo != null)
                return PartialView("/Views/Shared/EditorTemplates/VMPersonalInfo.cshtml", personalInfo.ConvertToViewModel());
            else return PartialView("/Views/Shared/EditorTemplates/VMPersonalInfo.cshtml", new VMPersonalInfo());
        }

        [ActionName("PersonalInfo")]
        [HttpPut]
        public async Task<IActionResult> UpdatePersonalInfo(string resumeId, VMPersonalInfo vmPersonalInfo)
        {
            if (vmPersonalInfo == null)
                return NotFound();

            var resume = await _resumeRepository.UpdateResumePersonalInfo(User.GetId(), resumeId, vmPersonalInfo.ConvertToEntity());

            if (resume == null)
                return NotFound();

            return Ok(PartialView("/Views/Shared/VMPersonalInfo.cshtml", vmPersonalInfo));
        }

        #endregion

        #region PROFILE ENTRIES

        [ActionName("ProfileEntry")]
        [HttpGet]
        public async Task<IActionResult> GetProfileEntry(string resumeId, string id)
        {
            var profileEntry = await _resumeRepository.GetResumeProfileEntry(User.GetId(), resumeId, id);
            if (profileEntry == null)
                return NotFound();

            return Ok(profileEntry);
        }

        [ActionName("ProfileEntries")]
        [HttpGet]
        public async Task<IActionResult> GetProfileEntries(string resumeId, string category)
        {
            var profileEntries = await _resumeRepository.GetResumeProfileEntriesByCategory(User.GetId(), resumeId, category);

            if (profileEntries == null)
                return NoContent();

            return Ok(profileEntries);
        }

        [ActionName("ProfileEntriesView")]
        [HttpGet]
        public async Task<IActionResult> GetProfileEntriesView(string resumeId, string category)
        {
            var profileEntries = await _resumeRepository.GetResumeProfileEntriesByCategory(User.GetId(), resumeId, category);

            if (profileEntries == null)
                return NoContent();

            ViewData["Context"] = "Resume";
            ViewData["ResumeId"] = resumeId;
            return PartialView("/Views/Shared/ProfileEntries.cshtml", profileEntries.ToList());
        }

        [ActionName("ProfileEntryForm")]
        [HttpGet]
        public async Task<IActionResult> GetProfileEntryForm(string? id, string? resumeId, EntryCategory? category)
        {
            ProfileEntry? profileEntry = null;
            if (id != null && resumeId != null)
            {
                profileEntry = await _resumeRepository.GetResumeProfileEntry(User.GetId(), resumeId, id);
                if (profileEntry == null)
                    return NotFound();
            }
            if (profileEntry == null)
            {
                profileEntry = new ProfileEntry();
                profileEntry.Id = Guid.NewGuid().ToString();
                profileEntry.Category = category ?? EntryCategory.WorkExperience;
            }

            ViewData["Context"] = "Resume";
            ViewData["ResumeId"] = resumeId;
            return PartialView("/Views/Shared/EditorTemplates/ProfileEntry.cshtml", profileEntry);
        }

        [ActionName("LoadProfileEntriesFromProfile")]
        [HttpGet]
        public async Task<IActionResult> LoadProfileEntriesFromProfile(string resumeId, EntryCategory[]? category)
        {
            var profileEntries = await _userRepository.GetProfileEntriesByCategories(User.GetId(), category?? []);
            if (profileEntries == null)
                return NotFound();

            var profileEntry = await _resumeRepository.UpdateResumeProfileEntries(User.GetId(), resumeId, profileEntries.ToList());
            if (profileEntry == null)
                return NotFound();

            return Ok(profileEntries);
        }

        [ActionName("ProfileEntry")]
        [HttpPost]
        public async Task<IActionResult> UpdateOrAddProfileEntry(string resumeId, ProfileEntry vmProfileEntry)
        {
            if (ModelState.IsValid)
            {
                var existingProfileEntry = await _resumeRepository.UpdateResumeProfileEntry(User.GetId(), resumeId, vmProfileEntry);
                if (existingProfileEntry != null)
                    return Ok(existingProfileEntry);
            }
            return BadRequest();
        }

        [ActionName("ProfileEntry")]
        [HttpPut]
        public async Task<IActionResult> UpdateProfileEntry(string resumeId, ProfileEntry vmProfileEntry)
        {
            var profileEntry = await _resumeRepository.UpdateResumeProfileEntry(User.GetId(), resumeId, vmProfileEntry);
            if (profileEntry == null)
                return NotFound();

            return Ok(vmProfileEntry);
        }

        [ActionName("CategoriesOrder")]
        [HttpPut]
        public async Task<IActionResult> UpdateProfileEntryCategoriesOrder(string resumeId, string[] categories)
        {
            var orderedCategories = await _resumeRepository.UpdateCategoriesOrder(User.GetId(), resumeId, categories);
            if (orderedCategories == null)
                return NotFound();
            
            return Ok(orderedCategories);
        }

        // POST: User/Delete/5
        [ActionName("ProfileEntry")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProfileEntry(string resumeId, string id)
        {
            await _resumeRepository.DeleteResumeProfileEntry(User.GetId(), resumeId, id);
            return NoContent();
        }

        #endregion        

        // GET: Resume/Delete/5        
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
                return NotFound();

            await _resumeRepository.DeleteResume(User.GetId(), id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(string? id)
        {
            if (id == null)
                return NotFound();

            await _resumeRepository.DeleteResume(User.GetId(), id);
            return NoContent();
        }

    }
}
