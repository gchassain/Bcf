﻿using System.ComponentModel.DataAnnotations;
using static Bcf.Models.Enums;

namespace Bcf.ViewModels
{
    public class IndexPlayerViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Joueur")]
        public string FullName { get; set; }
        [Display(Name = "Numéro")]
        public int Number { get; set; }
        [Display(Name = "Poste")]
        public PlayerPositionsEnum Position { get; set; }
        [Display(Name = "Taille")]
        public double Height { get; set; }
        [Display(Name = "Poids")]
        [DisplayFormat(DataFormatString = "{0:0.} kg")]
        public double Weight { get; set; }
        public string ProfilePicture { get; set; }
    }
}
