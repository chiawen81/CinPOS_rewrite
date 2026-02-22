/**
 * ╔══════════════════════════════════════════════════════════╗
 * ║  IMovieRepository — Movie 資料存取層的合約（Interface）                                                           ║
 * ╠══════════════════════════════════════════════════════════╣
 * ║  職責：定義 Service 層與資料存取層之間的「規格書」                                                                 ║
 * ║  ・只宣告方法簽章，不含任何實作                                                                                    ║
 * ║  ・讓 Service 層依賴「介面」而非 EF Core 實作（依賴反轉原則）                                                      ║
 * ║  ・實際實作位於 MovieRepository.cs                                                                                 ║
 * ╠══════════════════════════════════════════════════════════╣
 * ║  Repository 層核心原則：                                                                                           ║
 * ║  ✓ 只管 DB 存取（撈資料、寫資料）                                                                                 ║
 * ║  ✓ 回傳原始 Entity，不轉換成 DTO                                                                                  ║
 * ║  ✗ 不處理業務規則（那是 Service 層的事）                                                                          ║
 * ║  ✗ 不知道 HTTP 存在（那是 Controller 層的事）                                                                     ║
 * ╚══════════════════════════════════════════════════════════╝
 */

// ── 引用命名空間 ──────────────────────────────────────────────────
using CinPOS_rewrite.Models;   // Movie、Genre、ProvideVersion 等 Entity
using CinPOS_rewrite.Enums;    // MovieStatus、MovieRate 等列舉型別

// ── 宣告命名空間 ──────────────────────────────────────────────────
namespace CinPOS_rewrite.Repositories;

public interface IMovieRepository
{
    // ── 查詢多筆 ──────────────────────────────────────────────────
    // [目的] 依條件篩選電影清單，所有參數皆為可選（null = 不套用該條件）
    // [回傳] List<Movie>：原始 Entity 清單，空清單時回傳 []，不回傳 null
    Task<List<Movie>> GetListAsync(
        MovieStatus? status,       // 篩選上映狀態（對應 MovieStatus Enum）
        string? title,             // 模糊搜尋電影名稱
        DateTime? dateS,           // 上映日期範圍：起始日
        DateTime? dateE            // 上映日期範圍：結束日
    );

    // ── 查詢單筆 ──────────────────────────────────────────────────
    // [目的] 依主鍵 ID 取得單一電影
    // [回傳] Movie?：找到回傳 Entity；找不到回傳 null（由呼叫方決定如何處理 404）
    Task<Movie?> GetByIdAsync(string id);
}