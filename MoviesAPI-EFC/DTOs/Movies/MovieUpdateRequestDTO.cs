﻿using MoviesAPI_EFC.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.DTOs.Movies
{
    public class MovieUpdateRequestDTO
    {
        [Required]
        public string Title { get; set; }
        public bool OnTheaters { get; set; }
        public DateTime Date { get; set; }
        [ValidateImage(size: 4)]
        public IFormFile Poster { get; set; }
    }
}