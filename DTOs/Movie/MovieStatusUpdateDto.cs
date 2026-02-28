using System.ComponentModel.DataAnnotations;

namespace CinPOS_rewrite.DTOs.Movie;

/**
 * PUT /api/movies/status
 */

public class MovieStatusUpdateDto
{
    [Required] 
    public int Status { get; set; }
}