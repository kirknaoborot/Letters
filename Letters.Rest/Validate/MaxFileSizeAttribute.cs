using System.ComponentModel.DataAnnotations;

namespace Letters.Rest.Validate
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            long lenght = 0;

            if (value is IFormCollection files)
            {
                foreach (IFormFile file in files.Files)
                {
                    if (file.Length > _maxFileSize)
                    {
                        return new ValidationResult(GetErrorMessage($"Превышен максимальный размер файла в { _maxFileSize} 20МБ."));
                    }

                    lenght += file.Length;

                    if (lenght > _maxFileSize)
                        return new ValidationResult(GetErrorMessage($"Превышен суммарный размер файлов в { _maxFileSize} 20МБ."));
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage(string message)
        {
            return $"Превышен максимальный размер файла/ов в { _maxFileSize} 20МБ.";
        }
    }
}
