using Newtonsoft.Json;

namespace ResumeBuilder.Models.ViewModels
{
    public class VMResumeBasicInfo : ResumeBasicInfo
    {
        public Skills? SkillsObject
        {
            get
            {
                if (Skills== null)
                    return null;
                return JsonConvert.DeserializeObject<Skills>(Skills);
            }
        }
    }
}
