using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_LTWebNC.Models.EF
{
    [Table("tblComment")]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }
        public int UserID { get; set; }
        public int PostID { get; set; }
        public string ContentCmt { get; set; }
        public DateTime CreateOn { get; set; }
        [ForeignKey("PostID")]
        public Post Post { get; set; }
        public User? User { get; set; }

    }
}
