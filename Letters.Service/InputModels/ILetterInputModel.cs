using Microsoft.AspNetCore.Http;

namespace Letters.Service.InputModels
{
    public interface ILetterInputModel
    {
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Текст обращения
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// В адрес кого направлено обращение
        /// </summary>
        public string Recipient { get; set; }
        /// <summary>
        /// Значение капчи для повторной проверки
        /// </summary>
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
        public IFormFileCollection File { get; set; }
    }
}
