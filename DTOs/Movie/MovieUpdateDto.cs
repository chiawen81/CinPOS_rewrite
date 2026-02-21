using System.ComponentModel.DataAnnotations;

namespace CinPOS_rewrite.DTOs.Movie;

/**
 * PATCH request body¡A¦h¤@­Ó Id
 */

public class MovieUpdateDto : MovieCreateDto
{
    [Required]
    public string Id { get; set; } = null!;
}