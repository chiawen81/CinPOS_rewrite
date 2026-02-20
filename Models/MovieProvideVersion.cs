using System.ComponentModel.DataAnnotations;

namespace CinPOS_rewrite.Models;

public class MovieProvideVersion
{
    [Key]
    public int Id { get; set; }

    // ========== Foreign Key ==========
    public string MovieId { get; set; } = null!;
    public int ProvideVersionId { get; set; }

    // ========== Navigation Property ==========
    public Movie Movie { get; set; } = null!;
    public ProvideVersion ProvideVersion { get; set; } = null!;
}
