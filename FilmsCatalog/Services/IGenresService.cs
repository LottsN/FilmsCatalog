using FilmsCatalog.Data;
using FilmsCatalog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Services
{
    public interface IGenresService
    {
        Task<List<Genre>> GetAllGenres();
    }

    public class GenreService : IGenresService
    {
        private readonly ApplicationDbContext _context;

        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Genre>> GetAllGenres()
        {
            var genres = _context.Genres.ToList();
            return genres;
        }
    }
}

