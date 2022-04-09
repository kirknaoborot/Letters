using System.ComponentModel.DataAnnotations.Schema;

namespace Letters.Data.Reception
{
    [Table("RECIPENT")]
    public class Recipient
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        [Column("ID")]
        public Guid Id { get; set; }
        /// <summary>
        /// Идентификатор приемной
        /// </summary>
        [Column("RECEPTION_ID")]
        public Guid ReceptionId { get; set; }
        /// <summary>
        /// Название получателя
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }
    }
}
