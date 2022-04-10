using Letters.Rest.Validate;
using System.ComponentModel.DataAnnotations;

namespace Letters.Rest.Input
{
    public class ValidateFormInputModel
    {
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Не валидный адрес электронной почты")]
        public string Email { get; set; }
        /// <summary>
        /// Подтверждение адреса электронной почты
        /// </summary>
        [Required]
        [Compare(nameof(Email), ErrorMessage = "Адреса электронной почты не совпадают")]
        public string ConfirmEmail { get; set; }
        /// <summary>
        /// В адрес кого направлено обращение
        /// </summary>
        [Required]
        public string Recipient { get; set; }
        /// <summary>
        /// Текст обращения
        /// </summary>
        [Required]
        [StringLength(5000, ErrorMessage = "Не корректно передан текст обращения",MinimumLength = 5)]
        public string Text { get; set; }
        /// <summary>
        /// Идентификатор капчи
        /// </summary>
        [Required]
        public Guid CaptchaId { get; set; }
        /// <summary>
        /// Значение капчи
        /// </summary>
        [Required]
        public string CaptchaValue { get; set; }
        /// <summary>
        /// Вложенный файл
        /// </summary>
        [DataType(DataType.Upload)]
        [MaxFileSize(20 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".txt", ".doc", ".docx", ".rtf", ".xls", ".xlsx", ".pps", ".ppt", ".odt", ".ods", ".odp", ".pub", ".pdf", ".jpg", ".jpeg", ".bmp", ".png", ".tif", ".gif", ".pcx", ".mp3", ".wma", ".avi", ".mp4", ".mkv", ".wmv", ".mov", ".flv", ".zip", ".rar" })]
        public IFormFile File { get; set; }
    }
}
