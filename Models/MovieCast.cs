using System.ComponentModel.DataAnnotations;

namespace CinPOS_rewrite.Models;

public class MovieCast
{
    [Key]
    public int Id { get; set; }

    public string MovieId { get; set; } = null!;
    public string CastName { get; set; } = null!;

    // Navigation
    public Movie Movie { get; set; } = null!;
}
