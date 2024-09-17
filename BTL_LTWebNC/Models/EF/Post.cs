using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_LTWebNC.Models.EF
{
    [Table("tblPost")]
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostID { get; set; }
        public string Title {  get; set; }
        public string ContentPost { get; set; }
        public string? Urlimg { get; set; }
        public int UserID { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
        public List<Like>? ListLike { get; set; }
        public List<Comment>? ListComment { get; set; }
        public List<PostGallery>? ListPostGallery { get; set; }

        [NotMapped]
        public bool UserHasLiked { get; set; }
        [NotMapped]
        public int LikeCount { get; set; }
        [NotMapped]
        public IFormFile CoverPhoto { get; set; }
        [NotMapped]
        public IFormFileCollection FormFileCollection { get; set; }
    }
}
