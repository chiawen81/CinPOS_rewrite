using System.ComponentModel.DataAnnotations;

namespace CinPOS_rewrite.Dtos;

public class CreateUserDto
{
    [Required(ErrorMessage = "Name 為必填")]
    [MinLength(1, ErrorMessage = "Name 不可為空字串")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email 為必填")]
    [EmailAddress(ErrorMessage = "Email 格式不正確")]
    public string Email { get; set; } = string.Empty;
}
