using System.ComponentModel.DataAnnotations.Schema;

namespace Letters.Data.Reception
{
    [Table("ATTACH")]
    public class Attach
    {
        [Column("ID")]
        public long Id { get; set; }

        [Column("LETTER_ID")]
        public Guid LetterId { get; set; }

        [Column("CONTENT")]
        public byte[] Content { get; set; }

        [Column("FILE_NAME")]
        public string FileName { get; set; }

        public Letter Letter { get; set; } 
    }
}
