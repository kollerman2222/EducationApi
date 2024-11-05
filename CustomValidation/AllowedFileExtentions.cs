using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FgssrApi.Custom_DataAnnotation
{
    public class AllowedFileExtentions : ValidationAttribute
    {
        private readonly string _extentions;

        public AllowedFileExtentions(string extentions)
        {
            _extentions = extentions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
          
            var file = value as IFormFile;

            if (file != null)
            {
                var getExtention = Path.GetExtension(file.FileName);
                var isAllowed = _extentions.Split(",").Contains(getExtention, StringComparer.OrdinalIgnoreCase);

                if (!isAllowed)
                {
                    return new ValidationResult("Only JPG , PNG , JPEG , PDF are allowed");
                }

            }
            return ValidationResult.Success;
            
        }
    }
}
