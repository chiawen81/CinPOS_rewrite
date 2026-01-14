using System.ComponentModel.DataAnnotations;

namespace CinPOS_rewrite.Models;

public class Option
{
    [Key]
    public string UniqueId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;
}
