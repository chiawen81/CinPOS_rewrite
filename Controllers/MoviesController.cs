// ===== 引用命名空間 =====
using Microsoft.AspNetCore.Mvc;           // 使用 [ApiController]、ControllerBase、IActionResult 等 MVC 核心
using Microsoft.EntityFrameworkCore;      // 使用 .Include()、.ToListAsync() 等 EF Core 擴充方法
using CinPOS_rewrite.Data;                // 使用 AppDbContext（你的資料庫連線）
using CinPOS_rewrite.DTOs.Movie;          // 引用 Dtos：MovieListItemDto、MovieDetailDto
using CinPOS_rewrite.DTOs.Common;         // 引用 Dtos：ApiResponse<T>（統一回應格式）
using CinPOS_rewrite.Enums;               // 引用 Enums：MovieStatus、MovieRate
using CinPOS_rewrite.Constants;           // 引用 Constants：MovieStatusNames、MovieRateNames

// ===== 宣告這個類別的命名空間 =====
namespace CinPOS_rewrite.Controllers;

[ApiController]                           // 告訴框架這是 API Controller，自動處理 ModelState 驗證、400 回應
[Route("api/movies")]                     // 此 Controller 的基礎路徑
public class MoviesController : ControllerBase      // 繼承 ControllerBase（不含 View，純 API 用）
{
    private readonly AppDbContext _context;         // 宣告私有欄位，readonly = 只在建構子賦值

    public MoviesController(AppDbContext context)   // 建構子 DI，ASP.NET Core 自動注入
    {
        _context = context;                         // 儲存注入的 DbContext 供 Action 使用
    }

    // ═══════════════════════════════════════════
    //  GET /api/movies
    // ═══════════════════════════════════════════
    [HttpGet]                                       // 對應 HTTP GET，路徑 = 基礎路徑 api/movies
    public async Task<IActionResult> GetList(       // async = 非同步，IActionResult = 可回傳任何 HTTP 回應
        [FromQuery] int? status,                    // 上映狀態
        [FromQuery] string? title,                  // 電影名稱
        [FromQuery] DateTime? searchDateS,          // 上映日期起
        [FromQuery] DateTime? searchDateE)          // 上映日期迄
    /**NOTE：[FromQuery]
     * 
     * 從網址參數取得篩選條件，使用 [FromQuery] 明確指定來源（網址參數範例：?status=1&title=abc），可為 null
     */
    {
        var query = _context.Movies
            .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
            .Include(m => m.MovieProvideVersions).ThenInclude(mp => mp.ProvideVersion)
            .AsQueryable();
        /**NOTE：
         * 1. Include()、ThenInclude()：
         *      Include 載入 Navigation Property MovieGenres，ThenInclude 後再深入載入 Genre
         * 2. AsQueryable()：
         *      保持 IQueryable，後續 Where 才能組合成單一 SQL
         */

        // 篩選條件
        if (status.HasValue)
            query = query.Where(m => m.Status == (MovieStatus)status.Value);        // 原始資料 int 與 Enum 比對，SQL WHERE Status = @status

        if (!string.IsNullOrEmpty(title))
            query = query.Where(m => m.Title.Contains(title));                      // SQL LIKE '%title%'

        if (searchDateS.HasValue)
            query = query.Where(m => m.ReleaseDate >= searchDateS.Value);

        if (searchDateE.HasValue)
            query = query.Where(m => m.ReleaseDate <= searchDateE.Value);

        var movies = await query.ToListAsync();                                     // 此時才真正送出 SQL，非同步等待結果

        // ===== 將 Entity 轉為 DTO =====
        var data = movies.Select(m => new MovieListItemDto
        {
            Id = m.MovieId,
            Title = m.Title,
            StatusName = MovieStatusNames.Names[m.Status],      // Enum 轉 中文名稱（用字典對應）
            GenreName = m.MovieGenres.Select(mg => mg.Genre.GenreName).ToList(),    // 多對多，取出名稱清單
            Runtime = m.Runtime,
            Rate = (int)m.Rate,                                 // Enum 轉 int
            RateName = MovieRateNames.Names[m.Rate],            // Enum 轉 中文名稱（用字典對應）
            ReleaseDate = m.ReleaseDate,
            ProvideVersionName = m.MovieProvideVersions.Select(mp => mp.ProvideVersion.ProvideVersionName).ToList()
        }).ToList();

        return Ok(ApiResponse<List<MovieListItemDto>>.Success(
            data,
            data.Count > 0 ? "成功查詢電影列表!" : "沒有符合條件的資料!"
        ));
        /**NOTE
         * 三元運算子：有資料/無資料給不同訊息，但都是 200 OK
         */
    }



    // ═══════════════════════════════════════════
    //  GET /api/movies/{id}
    // ═══════════════════════════════════════════
    [HttpGet("{id}")]                               // 路徑參數，對應 api/movies/abc123
    public async Task<IActionResult> GetById(string id)
    {
        var movie = await _context.Movies
            .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
            .Include(m => m.MovieProvideVersions).ThenInclude(mp => mp.ProvideVersion)
            .Include(m => m.MovieCasts)                 // Detail 才需要演員，List 不需要 → 只在這裡 Include
            .FirstOrDefaultAsync(m => m.MovieId == id); // 找第一筆符合的，找不到回傳 null

        if (movie == null)
            return NotFound(ApiResponse<object>.Fail("查無此電影!"));    // 404 Not Found

        var data = new MovieDetailDto                   // Detail DTO 比 List DTO 多更多欄位
        {
            Id = movie.MovieId,
            Title = movie.Title,
            EnTitle = movie.EnTitle,
            // 電影類型（Genre）同時給 ID 和 Name：ID 供前端表單預選，Name 供顯示
            Genre = movie.MovieGenres.Select(mg => mg.GenreId).ToList(),                // 給前端代碼清單
            GenreName = movie.MovieGenres.Select(mg => mg.Genre.GenreName).ToList(),    // 給前端顯示用的名稱
            Runtime = movie.Runtime,
            ProvideVersion = movie.MovieProvideVersions.Select(mp => mp.ProvideVersionId).ToList(),
            ProvideVersionName = movie.MovieProvideVersions.Select(mp => mp.ProvideVersion.ProvideVersionName).ToList(),
            // 電影分級（Rate）同時給 ID 和 Name：ID 供前端表單預選，Name 供顯示
            Rate = (int)movie.Rate,
            RateName = MovieRateNames.Names[movie.Rate],
            Director = movie.Director,
            Cast = movie.MovieCasts.Select(mc => mc.CastName).ToList(),
            Description = movie.Description,
            Status = (int)movie.Status,
            StatusName = MovieStatusNames.Names[movie.Status],
            ReleaseDate = movie.ReleaseDate,
            TrailerLink = movie.TrailerLink,
            Distributor = movie.Distributor,
            PosterUrl = movie.PosterUrl
        };

        return Ok(ApiResponse<MovieDetailDto>.Success(data, "成功查詢電影資訊!"));
    }
}