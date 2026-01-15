using System.ComponentModel.DataAnnotations;
// Q：為什麼引入 System.ComponentModel.DataAnnotations 呢？
// A：
//   1. 因為用了 [Key] 
//   2. 這個套件是用來提供資料註解（Data Annotations）的功能

namespace CinPOS_rewrite.Models;

public class TicketType
{
    [Key]
    public int TicketTypeId { get; set; }

    public string TicketTypeName { get; set; } = null!;
    // § 補充：null! 
    //   表示這個屬性不會是 null，但在建構時暫時允許 null，開發者告訴編譯器在使用前一定會有值。

    public decimal Price { get; set; }
}
