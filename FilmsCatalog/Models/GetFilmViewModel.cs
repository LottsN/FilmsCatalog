using System;
using System.Collections.Generic;

namespace FilmsCatalog.Models
{
    public class GetFilmViewModel
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
        public string Slogan { get; set; }
        public List<Genre> Genres { get; set; }
        public string? Poster { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }
        public int Time { get; set; }
        public string Director { get; set; }
        public int Budget { get; set; }
        public int Fees { get; set; }
        public int AgeLimit { get; set; }
        public string Description { get; set; }
        public bool IsCreator { get; set; }
    }
}
