using System.ComponentModel.DataAnnotations;

namespace CinPOS_rewrite.Models;

public class MovieGenre
{
    [Key]
    public int Id { get; set; }     // 這張表自己的 PK

    // ========== Foreign Key ==========
    // FK:關連到特定的電影
    public string MovieId { get; set; } = null!;
    /**NOTE
     * 指向 Movies 表的 PK，可關聯到特定的電影資料（Movie）
     * 資料庫會用這個建立 FOREIGN KEY 約束
    */

    // FK：關聯到特定的電影類型
    public int GenreId { get; set; }  // FK


    // ========== Navigation Property ==========
    /**NOTE
     * 用途：讓程式能從「一筆 genre 紀錄」走回「它屬於哪部電影」
     * 例：movieGenre.Movie.Title
     */
    public Movie Movie { get; set; } = null!;   // 走到 Movie 主檔，取得電影資料
    public Genre Genre { get; set; } = null!;   // 走到 Genre 主檔，取得代碼對應的名稱
}
