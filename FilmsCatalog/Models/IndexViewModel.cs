using System.Collections.Generic;

namespace FilmsCatalog.Models
{
    public class IndexViewModel
    {
        public IEnumerable<GetFilmViewModel> Films { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
