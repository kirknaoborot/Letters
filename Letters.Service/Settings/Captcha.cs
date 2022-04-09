namespace Letters.Service.Settings
{
    public class Captcha
    {
        /// <summary>
        /// Высота изображения
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Ширина изображения
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Кол-во символов для капчи
        /// </summary>
        public int LenghtSymbols { get; set; }
    }
}
