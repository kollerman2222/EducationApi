using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Custom_DataAnnotation
{
    public class MaxFileSize : ValidationAttribute
    {
        private readonly int _maxSize;

        public MaxFileSize(int maxSize)
        {
            _maxSize = maxSize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            var file = value as IFormFile;

            if (file != null)
            {
                if(file.Length > _maxSize * 1024 * 1024)
                {
                    return new ValidationResult("Max file size is  "+_maxSize+"MB");
                }               
            }
            return ValidationResult.Success;

        }

    }
}
