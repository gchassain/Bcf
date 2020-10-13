using System;
using System.ComponentModel.DataAnnotations;
using static Bcf.Models.Enums;

namespace Bcf.ViewModels
{
    public class DetailsPlayerViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public int Number { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "Né le : {0:dd-MM-yyyy}")]
        public DateTime BirthDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.} kg")]
        public double Weight { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00} m")]
        public double Height { get; set; }

        public PlayerPositionsEnum Position { get; set; }

        public string ProfilePicture { get; set; }

        [Display(Name = "Equipe")]
        public string NameOfTeam { get; set; }
    }
}
