using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using static Bcf.Models.Enums;

namespace Bcf.ViewModels
{
    public class PlayerViewModel
    {
        public int Id { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Zéèêëïîâäàùûôöü-]*$", ErrorMessage = "Saississez un prénom valide")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez saisir un prénom"), Display(Name = "Prénom"), StringLength(30)]
        public string FirstName { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage = "Saississez un nom de famille valide")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez saisir un nom"), Display(Name = "Nom"), StringLength(30)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez choisir la position"), Display(Name = "Poste")]
        public PlayerPositionsEnum Position { get; set; }

        [Range(typeof(int), "00", "99", ErrorMessage = "Saississez un numéro valide")]
        [Display(Name = "Numéro")]
        public int Number { get; set; }

        [Display(Name = "Taille"), Range(10, 250, ErrorMessage = "Saississez une taille valide")]
        public int Height { get; set; }

        [Display(Name = "Poids"), Range(10, 250, ErrorMessage = "Saississez une poids valide")]
        public int Weight { get; set; }

        [Display(Name = "Surnom"), StringLength(30)]
        public string NickName { get; set; }

        [Required(ErrorMessage = "Veuillez saisir la date de naissance"), Display(Name = "Date de naissance")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }

        [Display(Name = "Joueur")]
        public string FullName => $"{FirstName} {LastName}";

        public PlayerViewModel()
        {
                
        }
    }
}