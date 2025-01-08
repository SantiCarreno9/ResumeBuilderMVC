using Newtonsoft.Json;

namespace ResumeBuilder.Models.ViewModels
{
    public class VMPersonalInfo : PersonalInfo
    {
        public List<AdditionalContact>? Contacts
        {
            get
            {
                if (AdditionalContactInfo == null)
                    return null;
                return JsonConvert.DeserializeObject<List<AdditionalContact>>(AdditionalContactInfo);
            }
        }
    }
}
