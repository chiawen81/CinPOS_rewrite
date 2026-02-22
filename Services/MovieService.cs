// ── MovieService.cs ─────────────────────────────────────────────────
/**
 * ╔══════════════════════════════════════════════════════════╗
 * ║  MovieService — Movie 業務邏輯層的實作                                                                            ║
 * ╠══════════════════════════════════════════════════════════╣
 * ║  職責：呼叫 Repository 取得原始 Entity，並將其組裝成 DTO 回傳                                                      ║
 * ║  ✗ 不知道 HTTP 存在（沒有 Request / Response）                                                                    ║
 * ║  ✗ 不直接操作 DB（那是 Repository 的事）                                                                          ║
 * ╚══════════════════════════════════════════════════════════╝
 */

// ── 引用命名空間 ──────────────────────────────────────────────────
using CinPOS_rewrite.DTOs.Movie;       // MovieListItemDto、MovieDetailDto（回傳的 DTO 型別）
using CinPOS_rewrite.Constants;        // MovieStatusNames、MovieRateNames（Enum → 中文名稱的字典）
using CinPOS_rewrite.Enums;            // MovieStatus、MovieRate（Enum 型別，用於型別轉換）
using CinPOS_rewrite.Repositories;     // IMovieRepository（資料存取介面）
using CinPOS_rewrite.Models;           // MovieCast 等 Entity

// ── 宣告命名空間 ──────────────────────────────────────────────────
namespace CinPOS_rewrite.Services;

public class MovieService : IMovieService
{
    // ── 依賴注入 ──────────────────────────────────────────────────
    // 依賴 Interface 而非實作類別 → 方便單元測試時替換假資料（Mock）
    private readonly IMovieRepository _repo;
    public MovieService(IMovieRepository repo) => _repo = repo;

    // =======================================================================================
    //     GetListAsync：查詢多筆電影，回傳 MovieListItemDto 清單
    // =======================================================================================
    public async Task<List<MovieListItemDto>> GetListAsync(MovieStatus? status, string? title, DateTime? dateS, DateTime? dateE)
    {
        // 呼叫 Repository，帶入所有篩選條件（null = 不套用該條件）
        var movies = await _repo.GetListAsync(status, title, dateS, dateE);

        // 將每筆 Entity 轉換為列表用 DTO
        return movies.Select(m => new MovieListItemDto
        {
            Id = m.MovieId,
            Title = m.Title,
            StatusName = MovieStatusNames.Names[m.Status],                                     // Enum 轉為 中文名稱（字典查找）
            GenreName = m.MovieGenres.Select(mg => mg.Genre.GenreName).ToList(),               // 多對多關聯 轉為 名稱清單（供前端顯示）
            Runtime = m.Runtime,
            Rate = (int)m.Rate,                                                                // Enum 轉為 int（不轉的話 JSON 會是字串如 "PG"）
            RateName = MovieRateNames.Names[m.Rate],                                           // Enum 轉為 中文名稱（字典查找）
            ReleaseDate = m.ReleaseDate,
            ProvideVersionName = m.MovieProvideVersions
                                  .Select(mp => mp.ProvideVersion.ProvideVersionName).ToList() // 多對多關聯 轉為 名稱清單（供前端顯示）
        }).ToList();
    }
    /**NOTE
     * Rate 不轉 int 的話，回傳給前端的 JSON 會是像 "NowPlaying" 的字串，而不是 1
     */


    // =======================================================================================
    //     GetByIdAsync：查詢單筆電影，回傳 MovieDetailDto（含完整欄位）
    // =======================================================================================
    public async Task<MovieDetailDto?> GetByIdAsync(string id)
    {
        // 呼叫 Repository 取得原始 Entity（含所有關聯資料）
        var movie = await _repo.GetByIdAsync(id);

        // 找不到時直接回 null，讓 Controller 決定要回 404 還是其他 HTTP 狀態碼
        if (movie == null) return null;

        // 將 Entity 組裝成詳情用 DTO
        return new MovieDetailDto
        {
            Id = movie.MovieId,
            Title = movie.Title,
            EnTitle = movie.EnTitle,
            Genre = movie.MovieGenres.Select(mg => mg.GenreId).ToList(),               // 多對多關聯 轉為 ID 清單（供前端預選）
            GenreName = movie.MovieGenres.Select(mg => mg.Genre.GenreName).ToList(),   // 多對多關聯 轉為 名稱清單（供前端顯示）
            Runtime = movie.Runtime,
            ProvideVersion = movie.MovieProvideVersions.Select(mp => mp.ProvideVersionId).ToList(),                      // 多對多關聯 轉為 ID 清單（供前端預選）
            ProvideVersionName = movie.MovieProvideVersions.Select(mp => mp.ProvideVersion.ProvideVersionName).ToList(), // 多對多關聯 轉為 名稱清單（供前端顯示）
            Rate = (int)movie.Rate,                                                    // Enum 轉為 int（不轉的話 JSON 會是字串如 "PG"）
            RateName = MovieRateNames.Names[movie.Rate],                               // Enum 轉為 中文名稱（字典查找）
            Director = movie.Director,
            Cast = movie.MovieCasts.Select(mc => mc.CastName).ToList(),                // 一對多關聯 轉為 名稱清單
            Description = movie.Description,
            Status = (int)movie.Status,                                                // Enum 轉為 int（不轉的話 JSON 會是字串如 "NowPlaying"）
            StatusName = MovieStatusNames.Names[movie.Status],                         // Enum 轉為 中文名稱（字典查找）
            ReleaseDate = movie.ReleaseDate,
            TrailerLink = movie.TrailerLink,
            Distributor = movie.Distributor,
            PosterUrl = movie.PosterUrl
        };
        /**NOTE
         * Rate、Status 不轉 int 的話，回傳給前端的 JSON 會是像 "NowPlaying" 的字串，而不是 1
         */
    }


    // =======================================================================================
    //     CreateAsync：新增單筆電影，回傳 MovieDetailDto（含完整欄位）
    // =======================================================================================
    public async Task<MovieDetailDto> CreateAsync(MovieCreateDto dto)
    {
        // 將 DTO 組裝成 Entity
        var movie = new Movie
        {
            MovieId = Guid.NewGuid().ToString(),
            Title = dto.Title,
            EnTitle = dto.EnTitle,
            Runtime = dto.Runtime,
            Rate = (MovieRate)dto.Rate,             // int 轉 Enum
            Director = dto.Director,
            Description = dto.Description,
            Status = (MovieStatus)dto.Status,         // int 轉 Enum
            ReleaseDate = dto.ReleaseDate,
            TrailerLink = dto.TrailerLink,
            Distributor = dto.Distributor,
            PosterUrl = dto.PosterUrl,

            // 多對多：只建中間表記錄，不需要載入完整關聯 Entity
            MovieGenres = dto.Genre
                .Select(gId => new MovieGenre { GenreId = gId })
                .ToList(),

            MovieProvideVersions = dto.ProvideVersion
                .Select(pvId => new MovieProvideVersion { ProvideVersionId = pvId })
                .ToList(),

            // 一對多：直接建立 Cast Entity
            MovieCasts = (dto.Cast ?? [])
                .Select(name => new MovieCast { CastName = name })
                .ToList()
        };

        // 存入資料庫
        await _repo.CreateAsync(movie);

        // 重新查詢以取得完整關聯資料（Genre名稱、ProvideVersion名稱等）
        // 直接呼叫已實作的 GetByIdAsync，避免重複組裝 DTO 邏輯
        return (await GetByIdAsync(movie.MovieId))!;
    }

}
