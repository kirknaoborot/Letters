using System.ComponentModel.DataAnnotations.Schema;

namespace Letters.Data.Reception
{
    [Table("RECEPTION")]
    public class Reception
    {
        /// <summary>
        /// Идентификатор приемной
        /// </summary>
        [Column("ID")]
        public Guid Id { get; set; }
        /// <summary>
        /// Название приемной
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }
    }
}
