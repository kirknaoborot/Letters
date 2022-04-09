namespace Letters.Core.Models
{
    public class FormModel
    {
        /// <summary>
        /// Список в адрес кого адресованно
        /// </summary>
        public IReadOnlyCollection<string> Recipients { get; set; }
        /// <summary>
        /// Обект капчи
        /// </summary>
        public CaptchaModel Captcha { get; set; }
    }
}
