using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.DTOs.Security
{
    public class AdminSetRoleDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
