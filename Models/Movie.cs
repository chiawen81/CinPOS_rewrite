using System.ComponentModel.DataAnnotations;
namespace CinPOS_rewrite.Models;
using CinPOS_rewrite.Enums;

public class Movie
{
    [Key]
    // <主鍵> 電影編號
    public string MovieId { get; set; } = null!;

    // 標題
    public string Title { get; set; } = null!;

    // 英文標題
    public string? EnTitle { get; set; }

    // 類型- Navigation
    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    /**NOTE
     * 用途：讓程式能從「一部電影」走到「它的所有 genre」
     * 沒有這些，就無法用 EF Core 的 Include() 語法，一次撈出關聯資料
     */

    // 片長
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "片長必須大於 0")]
    public int Runtime { get; set; }

    // 提供版本- Navigation
    public ICollection<MovieProvideVersion> MovieProvideVersions { get; set; } = new List<MovieProvideVersion>();

    // 分級代碼- Enum
    public MovieRate Rate { get; set; }

    // 導演
    public string? Director { get; set; }

    // 演員- Navigation   
    public ICollection<MovieCast> MovieCasts { get; set; } = new List<MovieCast>();

    // 電影簡介
    public string? Description { get; set; }

    // 上映狀態代碼- Enum
    public MovieStatus Status { get; set; }

    // 上映日期
    public DateTime ReleaseDate { get; set; }

    // 預告片連結
    public string? TrailerLink { get; set; }

    // 發行商
    public string? Distributor { get; set; }

    // 海報連結
    public string? PosterUrl { get; set; }

    // 建立時間
    public DateTime? CreatedAt { get; set; }

    // 更新時間
    public DateTime? UpdatedAt { get; set; }
}

/**NOTE:
 *  1. string：
 *     必填欄位 → string + = null!
 *     選填欄位 → string? (不加 = null!)
 *  2. 其它的值型別
 *   (1) 不用 = null!
 *       int、DateTime 等值型別的預設值已經是 0 或 DateTime.MinValue,不需要 = null!
 *   (2) 值型別可 null → 使用 ? (如 DateTime?)
 */