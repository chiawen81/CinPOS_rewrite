using System.ComponentModel.DataAnnotations;

namespace CinPOS_rewrite.Models;

public class ProvideVersion
{
    [Key]
    public int ProvideVersionId { get; set; }

    public string ProvideVersionName { get; set; } = null!;

    // Navigation：反向查「哪些電影屬於這個版本」
    public ICollection<MovieProvideVersion> MovieProvideVersions { get; set; } = new List<MovieProvideVersion>();
}