using System.ComponentModel.DataAnnotations;

namespace Letters.Rest.Input
{
    public class FormInputModel
    {
        /// <summary>
        /// Идентификатор приемной
        /// </summary>
        [Required]
        public Guid ReceptionId { get; set; }
    }
}
