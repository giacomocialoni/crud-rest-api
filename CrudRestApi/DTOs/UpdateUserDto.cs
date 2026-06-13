using System.ComponentModel.DataAnnotations;

namespace CrudRestApi.DTOs;

public class UpdateUserDto
{
    [Required(ErrorMessage = "Name is required")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
    [MaxLength(100, ErrorMessage = "Name must be at most 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    [MaxLength(200, ErrorMessage = "Email must be at most 200 characters")]
    public string Email { get; set; } = string.Empty;
}