using System.ComponentModel.DataAnnotations;

namespace Bcf.Models
{
    public class Enums
    {
        public enum PlayerPositionsEnum
        {
            [Display(Name = "Meneur")]
            POINT_GUARD = 1,
            [Display(Name = "Arrière")]
            SHOOTING_GUARD = 2,
            [Display(Name = "Ailier")]
            SMALL_FORWARD = 3,
            [Display(Name = "Ailier fort")]
            POWER_FORWARD = 4,
            [Display(Name = "Pivot")]
            CENTER = 5
        }
    }
}
