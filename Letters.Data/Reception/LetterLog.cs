using System.ComponentModel.DataAnnotations.Schema;

namespace Letters.Data.Reception
{
    public class LetterLog
    {
        [Column("ID")]
        public long Id { get; set; }

        [Column("Letter_ID")]
        public Guid LetterId { get; set; }

        [Column("IS_COMPLETE")]
        public bool IsComplete { get; set; }

        [Column("ERROR_MESSAGE")]
        public string ErrorMessage { get; set; }

        [Column("MODIFY_DATE")]
        public DateTime ModiFyDate { get; set; }
    }
}
