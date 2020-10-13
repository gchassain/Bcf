using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Bcf.Models.Enums;

namespace Bcf.Models
{
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez saisir un prénom"), StringLength(30)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez saisir un nom"), StringLength(30)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez choisir un poste")]
        public PlayerPositionsEnum Position { get; set; }

        [Range(typeof(int), "00", "99", ErrorMessage = "Saississez un numéro valide")]
        public int Number { get; set; }

        [Range(10, 250, ErrorMessage = "Saississez une taille valide")]
        public double Height { get; set; }

        [Range(10, 250, ErrorMessage = "Saississez une poids valide")]
        public double Weight { get; set; }

        [StringLength(30)]
        public string NickName { get; set; }

        [Required(ErrorMessage = "Veuillez saisir la date de naissance")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public string ProfilePicture { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public int TeamId { get; set; }

        public virtual Team Team { get; set; }
    }
}
