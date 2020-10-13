using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Bcf.ViewModels
{
    public class IndexViewModel
    {
        public List<PlayerViewModel> PlayersVM { get; set; }
        public SelectList Teams { get; set; }
        public string PlayerTeam { get; set; }
        public string SearchString { get; set; }
    }
}
