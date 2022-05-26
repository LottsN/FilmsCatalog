using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models
{
    public class CreateFilmViewModel
    {
        [Required(ErrorMessage = "Необходимо заполнить название фильма")]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить год выхода фильма")]
        [Display(Name = "Год выхода фильма")]
        [Range(1895, Int32.MaxValue, ErrorMessage = "Введите корректный год выхода фильма")]
        public int ReleaseYear { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить страну выхода фильма")]
        [Display(Name = "Страна выхода фильма")]
        [MaxLength(127, ErrorMessage = "Превышена максимальная длина строки для названия страны")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать жанры")]
        [Display(Name = "Жанры")]
        public List<string> Genres { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить слоган фильма")]
        [Display(Name = "Слоган")]
        public string Slogan { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить имя режиссера")]
        [Display(Name = "Режиссер")]
        [MaxLength(127, ErrorMessage = "Превышена максимальная длина строки для имени режиссера")]
        public string Director { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить бюджет фильма ($USD)")]
        [Display(Name = "Бюджет фильма")]
        [Range(0, 1000000000, ErrorMessage = "Введите корректный бюджет фильма")]
        public int Budget { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить сборы фильма ($USD)")]
        [Display(Name = "Сборы фильма")]
        [Range(0, 5000000000, ErrorMessage = "Введите корректные сборы фильма")]
        public int Fees { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить возрастное ограничение (от 0 до 21 года)")]
        [Display(Name = "Возрастное ограничение")]
        [Range(0, 21, ErrorMessage = "Введите корректное возрастное ограничение")]
        public int AgeLimit { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить продолжительность фильма")]
        [Display(Name = "Продолжительность (в минутах)")]
        [Range(0, 3000, ErrorMessage = "Введите корректную продолжительность фильма")]
        public int Time { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить описание фильма")]
        [Display(Name = "Описание")]
        [StringLength(1000, MinimumLength = 150, ErrorMessage = "Maximum 1000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Необходимо загрузить постер")]
        [Display(Name = "Постер")]
        public IFormFile File { get; set; }
    }
}
