using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.Validations
{
    public class ValidateImage: ValidationAttribute
    {

        private byte _maxFileSizeInMbs;

        public ValidateImage(byte size)
        {
            _maxFileSizeInMbs = size;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile file = value as IFormFile;

            if((file.Length / 1000 / 1000) > _maxFileSizeInMbs)
            {
                return new ValidationResult($"File can't exceed {_maxFileSizeInMbs} mb");
            }


            if (!new List<string> { "image/jpeg", "image/jpg", "image/png" }.Contains(file.ContentType))
            {
                return new ValidationResult("Image field is not valid");
            }

            return ValidationResult.Success;
        }
    }
}
