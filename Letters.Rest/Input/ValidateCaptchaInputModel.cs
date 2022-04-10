using System.ComponentModel.DataAnnotations;

namespace Letters.Rest.Input
{
    public class ValidateCaptchaInputModel
    {
        /// <summary>
        /// Идентификатор капчи
        /// </summary>
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Значение капчи
        /// </summary>
        [Required]
        public string Value { get; set; }
    }
}
