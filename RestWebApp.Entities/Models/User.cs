using System.ComponentModel.DataAnnotations;

namespace RestWebApp.Entities.Models;

public class User
{
    [Key] [Required] public int Id { get; init; }

    [Required] public string Name { get; set; }

    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    public string Password { get; set; }

    [Required] public string Role { get; set; }
}