﻿using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.DTOs.MovieTheater
{
    public class MovieTheaterCreateReqDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }
    }
}
