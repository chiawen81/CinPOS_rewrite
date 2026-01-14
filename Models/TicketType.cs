using System.ComponentModel.DataAnnotations;

namespace CinPOS_rewrite.Models;

public class TicketType
{
    [Key]
    public string Type { get; set; } = null!;
    // 補充：null! 
    // 表示這個屬性不會是 null，但在建構時暫時允許 null，開發者告訴編譯器在使用前一定會有值。

    public decimal Price { get; set; }
}
