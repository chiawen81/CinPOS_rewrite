// ── IMovieService.cs ────────────────────────────────────────────────
/**
 * ╔══════════════════════════════════════════════════════════╗
 * ║  IMovieService — Movie 業務邏輯層的合約（Interface）                                                              ║
 * ╠══════════════════════════════════════════════════════════╣
 * ║  職責：定義 Controller 與業務邏輯層之間的「規格書」                                                                ║
 * ║  ・只宣告方法簽章，不含任何實作                                                                                    ║
 * ║  ・讓 Controller 依賴「介面」而非具體實作（依賴反轉原則）                                                          ║
 * ║  ・實際實作位於 MovieService.cs                                                                                    ║
 * ╠══════════════════════════════════════════════════════════╣
 * ║  Service 層核心原則：                                                                                              ║
 * ║  ✓ 呼叫 Repository 拿原始 Entity                                                                                  ║
 * ║  ✓ 將 Entity 組裝成 DTO 後回傳                                                                                    ║
 * ║  ✗ 不知道 HTTP 存在（沒有 Request / Response）                                                                    ║
 * ╚══════════════════════════════════════════════════════════╝
 */

// ── 引用命名空間 ──────────────────────────────────────────────────
using CinPOS_rewrite.DTOs.Movie;   // MovieListItemDto、MovieDetailDto（方法的輸出型別）
using CinPOS_rewrite.Enums;        // MovieStatus 等列舉型別

// ── 宣告命名空間 ──────────────────────────────────────────────────
namespace CinPOS_rewrite.Services;

public interface IMovieService
{
    // ── 查詢多筆 ──────────────────────────────────────────────────
    // [目的] 依條件篩選電影清單，回傳已組裝好的 DTO（Controller 不需要知道資料怎麼來的）
    // [回傳] 空清單時回傳 []，不回傳 null
    Task<List<MovieListItemDto>> GetListAsync(
        MovieStatus? status,       // 篩選上映狀態
        string? title,             // 模糊搜尋電影名稱
        DateTime? dateS,           // 上映日期範圍：起始日
        DateTime? dateE            // 上映日期範圍：結束日
    );

    
    // ── 查詢單筆 ──────────────────────────────────────────────────
    // [目的] 依主鍵 ID 取得單一電影的完整資訊
    // [回傳] MovieDetailDto?：找到回傳 DTO；找不到回傳 null，由 Controller 決定要回 404 還是其他處理
    Task<MovieDetailDto?> GetByIdAsync(string id);


    // ── 新增單筆 ──────────────────────────────────────────────────
    // [目的] 接收前端傳入的 DTO，建立電影資料後回傳完整的電影詳情
    // [回傳] MovieDetailDto：新增成功後的完整電影資訊
    Task<MovieDetailDto> CreateAsync(MovieCreateDto dto);


    // ── 更新單筆 ──────────────────────────────────────────────────
    // [目的] 接收 PATCH DTO，更新後回傳完整 MovieDetailDto
    // [回傳] null = 找不到電影
    Task<MovieDetailDto?> UpdateAsync(string id, MovieUpdateDto dto);


    // ── 刪除單筆 ──────────────────────────────────────────────────
    // [目的] 依主鍵 ID 刪除電影
    // [回傳] true = 刪除成功；false = 找不到該電影（由 Controller 決定要回 404）
    Task<bool> DeleteAsync(string id);


    // ── 更新上映狀態 ──────────────────────────────────────────────
    // [目的] 只異動電影的上映狀態
    // [回傳] true = 更新成功；false = 找不到該電影
    Task<bool> UpdateStatusAsync(string id, int status);

}