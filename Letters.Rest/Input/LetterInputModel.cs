using System.ComponentModel.DataAnnotations;

namespace Letters.Rest.Input
{
    public class LetterInputModel
    {
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// Текст обращения
        /// </summary>
        [Required]
        public string Text { get; set; }
        /// <summary>
        /// В адрес кого направлено обращение
        /// </summary>
        [Required]
        public string Recipient { get; set; }
        /// <summary>
        /// Значение капчи для повторной проверки
        /// </summary>
        [Required]
        public string CaptchaValue { get; set; }
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Социальный статус
        /// </summary>
        public string SocialStatus { get; set; }
        /// <summary>
        /// Имя заявителя
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия заявителя
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Отчество заявителя
        /// </summary>
        public string MiddleName { get; set; }
        /// <summary>
        /// Адрес обращения
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Приложеный файл
        /// </summary>
        public IFormFile File { get; set; }
    }
}
