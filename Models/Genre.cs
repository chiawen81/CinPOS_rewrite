using System.ComponentModel.DataAnnotations;

namespace CinPOS_rewrite.Models;

public class Genre
{
    [Key]
    public int GenreId { get; set; }  // PK，對應原本的 GenreCode

    public string GenreName { get; set; } = null!;

    // Navigation：反向查「哪些電影屬於這個類型」
    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
}