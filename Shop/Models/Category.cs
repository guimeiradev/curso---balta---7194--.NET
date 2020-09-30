using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models {
    [Table("Categorie")] // Caso queira mudar o nome da tabela use o Data annotation table e coloque o nome da tabela 
    public class Category {
        [Key] // Todas as anotações colocadas em cima das classes são chamadas de "Data annotation"
        public int Id { get; set; }
        [Required(ErrorMessage ="Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage ="Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage ="Este campo deve conter entre 3 e 60 caracteres")]
        public string Title { get; set; }
    }
}
