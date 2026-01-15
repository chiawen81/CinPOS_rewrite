/* § Models/User.cs 用途：定義「一筆資料長什麼樣子」
     1. 定義 User 類別，包含用户的基本属性，如 Id、Name 和 Email。
     2. 為資料庫中的 Users 表提供數據結構。
*/

using System.ComponentModel.DataAnnotations;
namespace CinPOS_rewrite.Models;
// § 程式碼解釋：
//   namespace 是用來宣告「我這個檔案屬於哪裡」，讓其它檔案可以用 using 的方式引用它

public class User
{
    [Key]
    public string UserId { get; set; } = null!; 
    // 對應原本的 staffId，前台帳號格式 AXXXX、後台帳號格式 BXXXX

    public string Name { get; set; } = "";
    // § 程式碼解釋：
    //  1. 這是一個「可以被讀取（get）」、也可以被修改（set）」的屬性（property）
    //  2. 預設值是空字串

    public string? Password { get; set; }

    public string? ResetKey { get; set; }

    public string? Role { get; set; }   
    // staff or manager

    public int? Status { get; set; }

    public DateTime? CreatedAt { get; set; }
}


/* ————————————————————————————————————
   § 程式碼解釋：
     get = 讀取屬性值
     set = 設定屬性值
     public Xxx { get; set; } 是 EF Core 標準寫法
    ————————————————————————————————————
 */

