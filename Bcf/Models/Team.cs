using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bcf.Models
{
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez saisir un nom d'équipe"), StringLength(50), Display(Name = "Equipe")]
        public string NameOfTeam { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}
