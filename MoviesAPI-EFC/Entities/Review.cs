using Microsoft.AspNetCore.Identity;
using MoviesAPI_EFC.DTOs.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.Entities
{
    public class Review : IId
    {
        public int Id { get; set; }
        public string Text { get; set; }
        [Range(1,5)]
        public int Points { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
