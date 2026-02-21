using System.ComponentModel.DataAnnotations;

namespace CinPOS_rewrite.DTOs.Movie;

/**
 * ¹ïÀ³ GET /api/movies/{id} ªº data
 */

public class MovieDetailDto
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? EnTitle { get; set; }

    public List<int> Genre { get; set; } = null!;

    public List<string> GenreName { get; set; } = null!;

    public int Runtime { get; set; }

    public List<int> ProvideVersion { get; set; } = null!;

    public List<string> ProvideVersionName { get; set; } = null!;

    public int Rate { get; set; }

    public string RateName { get; set; } = null!;

    public string? Director { get; set; }

    public List<string> Cast { get; set; } = null!;

    public string? Description { get; set; }

    public int Status { get; set; }

    public string StatusName { get; set; } = null!;

    public DateTime ReleaseDate { get; set; }

    public string? TrailerLink { get; set; }

    public string? Distributor { get; set; }

    public string? PosterUrl { get; set; }
}