using System.ComponentModel.DataAnnotations.Schema;

namespace Letters.Data.Reception
{
    [Table("LETTER")]
    public class Letter
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Column("ID")]
        public Guid Id { get; set; }
        /// <summary>
        /// Электронный адрес почты
        /// </summary>
        [Column("EMAIL")]
        public string Email { get; set; }
        /// <summary>
        /// Телефон
        /// </summary>
        [Column("PHONE")]
        public string Phone { get; set; }
        /// <summary>
        /// ФИО заявителя
        /// </summary>
        [Column("FIO")]
        public string Fio { get; set; }
        /// <summary>
        /// Социальное положение
        /// </summary>
        [Column("SOCIAL_STATUS")]
        public string SocialStatus { get; set; }
        /// <summary>
        /// Адрес обращения
        /// </summary>
        [Column("Address")]
        public string Address { get; set; }
        /// <summary>
        /// В Адрес кого отправлено
        /// </summary>
        [Column("RECIPIENT")]
        public string Recipient { get; set; }
        /// <summary>
        /// Текст обращения
        /// </summary>
        [Column("TEXT")]
        public string Text { get; set; }
        /// <summary>
        /// Вложение
        /// </summary>
        [Column("ATTACH")]
        public byte[]? Attach { get; set; }
        /// <summary>
        /// Флаг обработки
        /// </summary>
        [Column("IS_PROCESSED")]
        public bool IsProcessed { get; set; }
    }
}
