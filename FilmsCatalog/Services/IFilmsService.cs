using FilmsCatalog.Data;
using FilmsCatalog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FilmsCatalog.Services
{
    public interface IFilmsService
    {
        Task Create(CreateFilmViewModel model, ClaimsPrincipal AuthorId);
        Task<List<GetFilmViewModel>> GetAllFilms(ClaimsPrincipal user);
        Task<GetFilmViewModel> GetOneFilm(int id, ClaimsPrincipal user);
        Task<List<string>> GetGenresForEdit(int FilmId);
        Task<EditFilmViewModel> GetForEdit(int id);
        Task Edit(EditFilmViewModel model);
        Task<DeleteFilmViewModel> GetForDelete(int id);
        Task Delete(int id);
        Guid? GetUserId(ClaimsPrincipal user);
    }

    public class FilmsService : IFilmsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private static string[] AllowedExtensions { get; set; } = { "jpg", "jpeg", "png" };

        public FilmsService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task Create(CreateFilmViewModel model, ClaimsPrincipal Author)
        {
            if (model.Genres.Count == 1) throw new Exception($"You have to choose at least one genre");
            var sameFilm = await GetByTitle(model.Title);
            if (sameFilm != null)
            {
                throw new ArgumentException($"Film with same name - {model.Title} already exists");
            }

            var isFileAttached = model.File != null;
            var fileNameWithPath = string.Empty;
            if (isFileAttached)
            {
                var extension = Path.GetExtension(model.File.FileName).Replace(".", "");
                if (!AllowedExtensions.Contains(extension))
                {
                    throw new ArgumentException("Attached file has not supported extension");
                }
                fileNameWithPath = $"files/{Guid.NewGuid()}-{model.File.FileName}";
                using (var fs = new FileStream(Path.Combine(_environment.WebRootPath, fileNameWithPath), FileMode.Create))
                {
                    model.File.CopyTo(fs);
                }
            }       

            var film = new Film
            {
                Title = model.Title,
                NormalizedTitle = NormalizeTitle(model.Title),
                Slogan = model.Slogan,
                Poster = fileNameWithPath,
                Year = model.ReleaseYear,
                Country = model.Country,
                Time = model.Time,
                Director = model.Director,
                Budget = model.Budget,
                Fees = model.Fees,
                AgeLimit = model.AgeLimit,
                Description = model.Description.Trim(),
                CreatorId = GetUserId(Author)
            };
            await _context.Films.AddAsync(film);
            await _context.SaveChangesAsync();

            for (int i = 1; i < model.Genres.Count; i++)
            {
                _context.FilmsGenres.Add(new FilmsGenres
                {
                    FilmId = film.FilmId,
                    GenreId = Int32.Parse(model.Genres[i]),
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<GetFilmViewModel>> GetAllFilms(ClaimsPrincipal user)
        {
            var UserId = GetUserId(user);
            var films = _context.Films.Select(x => new GetFilmViewModel
            {
                FilmId = x.FilmId,
                Title = x.Title,
                Slogan = x.Slogan,
                Poster = x.Poster,
                Year = x.Year,
                Country = x.Country,
                Time = x.Time,
                Director = x.Director,
                Budget = x.Budget,
                Fees = x.Fees,
                AgeLimit = x.AgeLimit,
                Description = string.Concat(x.Description.Substring(0, 120), " ..."),
                IsCreator = (x.CreatorId == UserId) ? true : false
            }).ToList();
            return films;
        }

        public async Task<GetFilmViewModel> GetOneFilm(int id, ClaimsPrincipal? user)
        {
            var UserId = GetUserId(user);
            var film = _context.Films.FirstOrDefault(x => x.FilmId == id);
            if (film == null) throw new Exception($"Film with ID: {id} not found");
            return new GetFilmViewModel
            {
                FilmId = film.FilmId,
                Title = film.Title,
                Slogan = film.Slogan,
                Genres = GetFilmGenres(film.FilmId).Result,
                Poster = film.Poster,
                Year = film.Year,
                Country = film.Country,
                Time = film.Time,
                Director = film.Director,
                Budget = film.Budget,
                Fees = film.Fees,
                AgeLimit = film.AgeLimit,
                Description = film.Description,
                IsCreator = (film.CreatorId == UserId) ? true : false
            };
        }

        public async Task<List<string>> GetGenresForEdit(int FilmId)
        {
            var pairs = _context.FilmsGenres.Where(x => x.FilmId == FilmId).ToArray();
            var genres = new List<string>();
            for (int k = 0; k < pairs.Length; k++)
            {
                var genre = _context.Genres.FirstOrDefault(x => x.GenreId == pairs[k].GenreId);
                genres.Add(genre.GenreId.ToString());
            }
            return genres;
        }

        public async Task<EditFilmViewModel> GetForEdit(int id)
        {
            var film = _context.Films.FirstOrDefault(x => x.FilmId == id);
            if (film == null) throw new KeyNotFoundException($"Film with Id={id} does not found!");
            return new EditFilmViewModel
            {
                FilmId = film.FilmId,
                Title = film.Title,
                Slogan = film.Slogan,
                Genres = GetGenresForEdit(film.FilmId).Result,
                CurrentFilePath = film.Poster,
                ReleaseYear = film.Year,
                Country = film.Country,
                Time = film.Time,
                Director = film.Director,
                Budget = film.Budget,
                Fees = film.Fees,
                AgeLimit = film.AgeLimit,
                Description = film.Description,
                CreatorId = film.CreatorId
            };
        }

        public async Task Edit(EditFilmViewModel model)
        {
            var film = _context.Films.FirstOrDefault(x => x.FilmId == model.FilmId);
            if (film == null) throw new KeyNotFoundException($"Film with Id={model.FilmId} does not found!");
            if (model.Genres.Count == 0) throw new Exception($"You have to choose at least one genre");

            film.Title = model.Title;
            film.NormalizedTitle = NormalizeTitle(model.Title);
            film.Slogan = model.Slogan;
            film.Year = model.ReleaseYear;
            film.Country = model.Country;
            film.Time = model.Time;
            film.Director = model.Director;
            film.Budget = model.Budget;
            film.Fees = model.Fees;
            film.AgeLimit = model.AgeLimit;
            film.Description = model.Description;

            var isFileAttached = model.File != null;
            if (isFileAttached)
            {
                var extension = Path.GetExtension(model.File.FileName).Replace(".", "");
                if (!AllowedExtensions.Contains(extension))
                {
                    throw new ArgumentException("Attached file has not supported extension");
                }
                var fileNameWithPath = $"files/{Guid.NewGuid()}-{model.File.FileName}";
                using (var fs = new FileStream(Path.Combine(_environment.WebRootPath, fileNameWithPath), FileMode.Create))
                {
                    await model.File.CopyToAsync(fs);
                }
                //delete previous poster
                if (File.Exists(film.Poster))
                {
                    File.Delete(film.Poster);
                }
                film.Poster = fileNameWithPath;
            }
            _context.Films.Update(film);

            //delete old genres
            var allPairs = _context.FilmsGenres.Where(x => x.FilmId == film.FilmId);
            _context.RemoveRange(allPairs);

            //add new genres
            for (int i = 0; i < model.Genres.Count; i++)
            {
                try
                {
                    _context.FilmsGenres.Add(new FilmsGenres
                    {
                        FilmId = film.FilmId,
                        GenreId = Int32.Parse(model.Genres[i]),
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<DeleteFilmViewModel> GetForDelete(int id)
        {
            var film = _context.Films.FirstOrDefault(x => x.FilmId == id);
            if (film == null) throw new KeyNotFoundException($"Film with Id={id} does not found!");
            return new DeleteFilmViewModel
            {
                FilmId = film.FilmId,
                Title = film.Title,
                CreatorId = film.CreatorId,
            };
        }
        public async Task Delete(int id)
        {
            var film = _context.Films.FirstOrDefault(x => x.FilmId == id);
            if (File.Exists(film.Poster))
            {
                File.Delete(film.Poster);
            }
            _context.Films.Remove(film);
            await _context.SaveChangesAsync();
        }

        private async Task<List<Genre>> GetFilmGenres(int FilmId)
        {
            var pairs = _context.FilmsGenres.Where(x => x.FilmId == FilmId).ToArray();
            var genres = new List<Genre>();
            for (int k = 0; k < pairs.Length; k++)
            {
                var genre = _context.Genres.FirstOrDefault(x => x.GenreId == pairs[k].GenreId);
                genres.Add(genre);
            }
            return genres;
        }

        private async Task<Film> GetByTitle(string title)
        {
            return _context.Films.FirstOrDefault(x => x.NormalizedTitle == NormalizeTitle(title));
        }

        /*
        private async Task<Genre> GetGenreByName(string name)
        {
            var genre = _context.Genres.FirstOrDefault(x => x.Name == name);
            return genre;
        }
        */
        private string NormalizeTitle(string title)
        {
            return title.ToUpper().Replace(" ", "").Replace("«", "\"").Replace("»", "\"");
        }

        public Guid? GetUserId(ClaimsPrincipal user)
        {
            if (user == null) return null;
            Guid? userId = null;
            var claims = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (claims == null) return null;
            else userId = Guid.Parse(claims.Value);
            return userId;
        }
    }
}
