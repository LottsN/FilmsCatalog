using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace FilmsCatalog.Models
{
    public class EditFilmViewModel : CreateFilmViewModel
    {
        public int FilmId { get; set; }
        public string CurrentFilePath { get; set; } = "";
        public Guid? CreatorId { get; set; }
        new public IFormFile File { get; set; }
    }
}
