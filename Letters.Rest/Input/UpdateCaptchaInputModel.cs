using System.ComponentModel.DataAnnotations;

namespace Letters.Rest.Input
{
    public class UpdateCaptchaInputModel
    {
        /// <summary>
        /// Идентификатор капчи
        /// </summary>
        [Required]
        public Guid Key { get; set; }
    }
}
