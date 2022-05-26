using System;

namespace FilmsCatalog.Models
{
    public class DeleteFilmViewModel
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
        public Guid? CreatorId { get; set; }
    }
}
