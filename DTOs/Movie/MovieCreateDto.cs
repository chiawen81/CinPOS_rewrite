using System.ComponentModel.DataAnnotations;

namespace CinPOS_rewrite.DTOs.Movie;

/**
 * POST request body
 */

public class MovieCreateDto
{
    [Required] 
    public string Title { get; set; } = null!;

    public string? EnTitle { get; set; }

    [Required, MinLength(1)] 
    public List<int> Genre { get; set; } = null!;

    [Required] 
    public int Runtime { get; set; }
    
    [Required, MinLength(1)] 
    public List<int> ProvideVersion { get; set; } = null!;

    [Required] 
    public int Rate { get; set; }
    
    public string? Director { get; set; }
    
    public List<string>? Cast { get; set; }
    
    public string? Description { get; set; }
    
    [Required] 
    public int Status { get; set; }

    [Required] 
    public DateTime ReleaseDate { get; set; }
    
    public string? TrailerLink { get; set; }
    
    public string? Distributor { get; set; }
    
    public string? PosterUrl { get; set; }
}