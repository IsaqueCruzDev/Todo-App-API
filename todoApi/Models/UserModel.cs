using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace todoApi.Models {

    [Index(nameof(Email), IsUnique = true)]
    public class UserModel {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name {get; set;} = "";

        [Required]
        [MaxLength(255)]
        public string Email {get; set;} = "";

        [Required]   
        public string Password {get; set;} = "";

    }
}
