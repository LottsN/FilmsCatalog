using FilmsCatalog.Models;
using FilmsCatalog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFilmsService _filmsService;

        public HomeController(ILogger<HomeController> logger, IFilmsService filmsService)
        {
            _logger = logger;
            _filmsService = filmsService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 15)
        { 
            var films = await _filmsService.GetAllFilms(User);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(films.Count, page, pageSize),
                Films = films.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
