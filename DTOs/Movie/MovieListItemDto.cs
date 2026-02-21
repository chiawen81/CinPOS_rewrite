using System.ComponentModel.DataAnnotations;

namespace CinPOS_rewrite.DTOs.Movie;
/**
 * ¹ïÀ³ GET /api/movies ªº data[]
 */

public class MovieListItemDto
{
    public string Id { get; set; } = null!;

    public string StatusName { get; set; } = null!;

    public string Title { get; set; } = null!;

    public List<string> GenreName { get; set; } = null!;

    public int Runtime { get; set; }

    public int Rate { get; set; }

    public string RateName { get; set; } = null!;

    public DateTime ReleaseDate { get; set; }

    public List<string> ProvideVersionName { get; set; } = null!;
}