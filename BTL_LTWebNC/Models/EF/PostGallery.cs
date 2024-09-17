using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_LTWebNC.Models.EF
{
    [Table("tblGalleryImg")]
    public class PostGallery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GalleryID { get; set; }
        public int PostID { get; set; }
        public string FileNameImg { get; set; }
        public string URLImgGallery { get; set; }

        [ForeignKey("PostID")]
        public Post Post { get; set; }
    }
}
