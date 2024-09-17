using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_LTWebNC.Models.EF
{
    [Table("tblUser")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int UserRoleID { get; set; }
        public string VerifyKey { get; set; }
        [ForeignKey("UserRoleID")]
        public Role Role { get; set; }

        [NotMapped]
        public string RePassWord { get; set; }
    }
}
