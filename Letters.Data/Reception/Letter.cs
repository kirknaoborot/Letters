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
        /// Идентификатор пользователя если отправка через ГОС УСЛУГИ
        /// </summary>
        [Column("USER_ID")]
        public Guid? UserId { get; set; }
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
        /// Флаг обработки
        /// </summary>
        [Column("IS_PROCESSED")]
        public bool IsProcessed { get; set; }
        /// <summary>
        /// Дата обращения
        /// </summary>
        [Column("Дата создания обращения")]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Флаг отслеживания истории
        /// </summary>
        [Column("IS_HISTORY")]
        public bool IsHistory { get; set; }

        [Column("DOCUMENT_ID")]
        public Guid? DocumentId { get; set; }

        public ICollection<Attach> Attaches { get; set; }
    }
}
