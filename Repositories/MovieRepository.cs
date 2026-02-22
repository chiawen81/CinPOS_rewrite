// ── MovieRepository.cs ───────────────────────────────────────────────
/**
 * ╔══════════════════════════════════════════════════════════╗
 * ║  MovieRepository — Movie 資料存取層的實作                                                                         ║
 * ╠══════════════════════════════════════════════════════════╣
 * ║  職責：透過 EF Core 操作 DB，回傳原始 Entity                                                                       ║
 * ║  ✗ 不處理業務邏輯（那是 Service 的事）                                                                            ║
 * ║  ✗ 不做 DTO 轉換（那是 Service 的事）                                                                             ║
 * ║  ✗ 不知道 HTTP 存在                                                                                               ║
 * ╚══════════════════════════════════════════════════════════╝
 */

// ── 引用命名空間 ──────────────────────────────────────────────────
using Microsoft.EntityFrameworkCore;   // Include()、ThenInclude()、ToListAsync()、FirstOrDefaultAsync()
using CinPOS_rewrite.Data;             // AppDbContext（DB 連線與 DbSet）
using CinPOS_rewrite.Models;           // Movie、Genre、ProvideVersion 等 Entity
using CinPOS_rewrite.Enums;            // MovieStatus（用於篩選條件的型別轉換）

// ── 宣告命名空間 ──────────────────────────────────────────────────
namespace CinPOS_rewrite.Repositories;

public class MovieRepository : IMovieRepository
{
    // ── 依賴注入 ──────────────────────────────────────────────────
    private readonly AppDbContext _context;                             // readonly：只在建構子賦值，防止被意外重新指派
    public MovieRepository(AppDbContext context) => _context = context; // 透過 DI 注入 DbContext

    // =======================================================================================
    //     GetListAsync：依條件查詢多筆電影，回傳原始 Entity 清單
    // =======================================================================================
    public async Task<List<Movie>> GetListAsync(MovieStatus? status, string? title, DateTime? dateS, DateTime? dateE)
    {
        // 建立基礎查詢，預先 JOIN 載入關聯資料（避免後續迴圈造成 N+1 查詢問題）
        // AsQueryable() 保持為 IQueryable，讓後續所有 Where 最終組合成單一 SQL 再送出
        var query = _context.Movies
            .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)                   // 載入 電影 → 電影類型關聯 → 類型
            .Include(m => m.MovieProvideVersions).ThenInclude(mp => mp.ProvideVersion) // 載入 電影 → 放映版本關聯 → 版本
            .AsQueryable();

        // 動態疊加篩選條件（null = 不套用，最終組合成一條 SQL）
        if (status.HasValue)
            query = query.Where(m => m.Status == status.Value);
        if (!string.IsNullOrEmpty(title))
            query = query.Where(m => m.Title.Contains(title));                         // 模糊搜尋（SQL LIKE）
        if (dateS.HasValue)
            query = query.Where(m => m.ReleaseDate >= dateS.Value);                    // 日期範圍：起始
        if (dateE.HasValue)
            query = query.Where(m => m.ReleaseDate <= dateE.Value);                    // 日期範圍：結束

        // 到此才真正送出 SQL，執行查詢並回傳結果
        return await query.ToListAsync();
    }


    // =======================================================================================
    //     GetByIdAsync：依主鍵 ID 查詢單筆電影，回傳原始 Entity
    // =======================================================================================
    // 比 GetListAsync 多 Include MovieCasts（詳情頁需要演員資料，列表頁不需要）
    public async Task<Movie?> GetByIdAsync(string id)
    {
        return await _context.Movies
            .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
            .Include(m => m.MovieProvideVersions).ThenInclude(mp => mp.ProvideVersion)
            .Include(m => m.MovieCasts)                                                // 詳情頁專用：載入演員清單
            .FirstOrDefaultAsync(m => m.MovieId == id);                                // 找不到時回傳 null
    }


    // =======================================================================================
    //     CreateAsync：新增單筆電影至資料庫，回傳原始 Entity
    // =======================================================================================
    public async Task<Movie> CreateAsync(Movie movie)
    {
        _context.Movies.Add(movie);        // 將 Entity 加入 EF Core 的追蹤清單（尚未寫入 DB）
        await _context.SaveChangesAsync(); // 實際執行 INSERT SQL，寫入資料庫
        return movie;                      // 回傳已存入的 Entity（此時 MovieId 已確定）
    }
}