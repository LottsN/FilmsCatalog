using FilmsCatalog.Models;
using FilmsCatalog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    public class FilmController : Controller
    {
        private readonly IFilmsService _filmsService;
        private readonly IGenresService _genresService;
        public FilmController(IFilmsService filmsService, IGenresService genresService)
        {
            _filmsService = filmsService;
            _genresService = genresService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(int id)
        {
            return View(await _filmsService.GetOneFilm(id, User));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            ViewData["Genres"] = _genresService.GetAllGenres().Result;
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFilmViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _filmsService.Create(model, User);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("CreationFilmErrors", ex.Message);
                }
            }
            ViewData["Genres"] = _genresService.GetAllGenres().Result;
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var film = _filmsService.GetForEdit(id).Result;
                ViewData["Genres"] = _genresService.GetAllGenres().Result;
                if (film.CreatorId == _filmsService.GetUserId(User)) return View(film);
                else { return StatusCode(403, "You have no permissoin to do that"); }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditFilmViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Genres"] = _genresService.GetAllGenres().Result;
                return View(model);
            }
            try
            {
                if (model.CreatorId == _filmsService.GetUserId(User))
                {
                    await _filmsService.Edit(model);
                    return RedirectToAction("Index", "Home");
                }
                else { return StatusCode(403, "You have no permissoin to do that"); }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewData["Genres"] = _genresService.GetAllGenres().Result;
                return View(model);
            }
        }



        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                var film = _filmsService.GetForDelete(id).Result;
                if (film.CreatorId == _filmsService.GetUserId(User)) return View(film);
                else { return StatusCode(403, "You have no permissoin to do that"); }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteFilmViewModel model)
        {
            try
            {
                if (model.CreatorId == _filmsService.GetUserId(User)) 
                {
                    await _filmsService.Delete(model.FilmId);
                    return RedirectToAction("Index", "Home");
                }
                else { return StatusCode(403, "You have no permissoin to do that"); }
            }
            catch
            {
                return RedirectToAction("Index", "Menu");
            } 
        }

    }
}
