using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ResumeBuilder.ModelBinder
{
    public class VMResumeModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;

            return Task.CompletedTask;
        }
    }
}
