using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models
{
    [Index(nameof(NormalizedTitle), IsUnique = true)]
    public class Film
    {
        [Key]
        public int FilmId { get; set; }
        public string Title { get; set; }
        public string Slogan { get; set; }
        public string NormalizedTitle { get; set; }
        public string? Poster { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }
        public int Time { get; set; }
        public string Director { get; set; }
        public int Budget { get; set; }
        public int Fees { get; set; }
        public int AgeLimit { get; set; } 
        public string Description { get; set; }
        public Guid? CreatorId { get; set; }
    }
}
