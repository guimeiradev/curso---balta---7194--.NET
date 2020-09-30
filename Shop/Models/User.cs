using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models {
    [Table("Usuario")]
    public class User {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Este campo é obrigatório")]
        [MaxLength(20,ErrorMessage ="Este campo deve conter entre 20 e 3 caracteres")]
        [MinLength(3,ErrorMessage ="Este campo deve conter entre 3 e 20 caracteres")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 60 e 3 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        public string Passoword { get; set; }

        public string Role { get; set; }
    }
}
