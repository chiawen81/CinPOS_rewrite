/* Models/User.cs 用途：定義「一筆資料長什麼樣子」
   1. 定義 User 類別，包含用户的基本属性，如 Id、Name 和 Email。
   2. 為資料庫中的 Users 表提供數據結構。
   3. 以下程式碼的意思：
        資料庫裡有一張叫 Users 的表，每一列（row）有這些欄位：Id、Name、Email.....etc
      - public int Id { get; set; }：定義一個整數類型的 Id 屬性，作為用户的唯一標識符。
      - public string Name { get; set; } = ""：定義一個字符串類型的 Name 屬性，表示用户的名稱，預設值為空字符串。
      - public string Email { get; set; } = ""：定義一個字符串類型的 Email 屬性，表示用户的電子郵件地址，預設值為空字符串。
*/


namespace CinPOS_rewrite.Models;

public class User
{
    public int Id { get; set; }
    // 這是一個「可以被讀取（get）」、也可以被修改（set）」的屬性（property）

    public string Name { get; set; } = "";
    // 預設值是空字串

    public string Email { get; set; } = "";
}


/* 程式碼意義：
 get = 讀取屬性值
 set = 設定屬性值
 public Xxx { get; set; } 是 EF Core 標準寫法
 */

