using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_LTWebNC.Models.EF
{
    [Table("tblLike")]
    public class Like
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LikeID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        [ForeignKey("PostID")]
        public Post Post { get; set; }
        public User? User { get; set; }

    }
}
