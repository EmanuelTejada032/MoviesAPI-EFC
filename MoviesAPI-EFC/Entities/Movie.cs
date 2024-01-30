﻿using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool OnTheaters { get; set; }
        public DateTime Date { get; set; }
        public string Poster { get; set; }
    }
}