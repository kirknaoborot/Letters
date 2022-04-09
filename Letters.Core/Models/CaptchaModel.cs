namespace Letters.Core.Models
{
    public class CaptchaModel
    {
        /// <summary>
        /// Идентификатор капчи
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Изображение конвертированный Base64
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// Аудио файл конвертированный Base64
        /// </summary>
        public string Audio { get; set; }
    }
}
