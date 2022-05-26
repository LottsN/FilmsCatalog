using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmsCatalog.Models
{
    public class FilmsGenres
    {
        public int FilmId { get; set; }
        public int GenreId { get; set; }
    }
}
