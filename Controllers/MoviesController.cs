// ── MoviesController.cs ───────────────────────────────────────────────
/**
 * ╔══════════════════════════════════════════════════════════╗
 * ║  MoviesController — Movie 的 HTTP 入口                                                                            ║
 * ╠══════════════════════════════════════════════════════════╣
 * ║  職責：接收 HTTP Request，呼叫 Service，包裝成統一回應格式後回傳                                                   ║
 * ║  ✓ 決定 HTTP 狀態碼（200 / 404 ...）                                                                              ║
 * ║  ✓ 將 Service 回傳的 DTO 包入 ApiResponse<T>                                                                      ║
 * ║  ✗ 不處理業務邏輯（那是 Service 的事）                                                                            ║
 * ║  ✗ 不直接操作 DB（那是 Repository 的事）                                                                          ║
 * ╚══════════════════════════════════════════════════════════╝
 */

// ── 引用命名空間 ──────────────────────────────────────────────────
using Microsoft.AspNetCore.Mvc;        // [ApiController]、ControllerBase、IActionResult、[HttpGet] 等
using CinPOS_rewrite.DTOs.Movie;       // MovieListItemDto、MovieDetailDto（Service 回傳的 DTO 型別）
using CinPOS_rewrite.DTOs.Common;      // ApiResponse<T>（統一回應格式的包裝器）
using CinPOS_rewrite.Enums;            // MovieStatus（Enum 型別，用於型別轉換）
using CinPOS_rewrite.Services;         // IMovieService（業務邏輯層介面）

// ── 宣告命名空間 ──────────────────────────────────────────────────
namespace CinPOS_rewrite.Controllers;

[ApiController]           // 啟用 API 模式：自動處理 ModelState 驗證、400 Bad Request 回應
[Route("api/movies")]     // 此 Controller 的基礎路由：/api/movies
public class MoviesController : ControllerBase  // 繼承 ControllerBase（純 API 用，不含 MVC View）
{
    // ── 依賴注入 ──────────────────────────────────────────────────
    private readonly IMovieService _service;
    public MoviesController(IMovieService service) => _service = service; // 注入 Service 介面，不依賴具體實作
    

    // ==== GET /api/movies：查詢電影列表 =========================================
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] MovieStatus? status,  // Query String 參數：篩選狀態（ASP.NET Core 會自動解析將 int 轉成 Enum）
        [FromQuery] string? title,        // Query String 參數：模糊搜尋名稱
        [FromQuery] DateTime? searchDateS,// Query String 參數：上映日期起始
        [FromQuery] DateTime? searchDateE)// Query String 參數：上映日期結束
    {
        // 呼叫 Service 取得 DTO 清單（篩選邏輯在 Service / Repository 處理）
        var data = await _service.GetListAsync(status, title, searchDateS, searchDateE);

        // 包裝成統一回應格式後回傳 HTTP 200
        // 有資料 vs 空清單給不同的提示訊息（都是 200，不是 404）
        return Ok(ApiResponse<List<MovieListItemDto>>.Success(
            data,
            data.Count > 0 ? "成功查詢電影列表!" : "沒有符合條件的資料!"
        ));
    }
    


    // ==== GET /api/movies/{id}：查詢單筆電影 ====================================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        // 呼叫 Service 依 ID 查詢（找不到時 Service 回傳 null）
        var data = await _service.GetByIdAsync(id);

        // Service 回傳 null → 包裝成失敗回應後回傳 HTTP 404
        if (data == null)
            return NotFound(ApiResponse<object>.Fail("查無此電影!"));

        // 找到資料 → 包裝成成功回應後回傳 HTTP 200
        return Ok(ApiResponse<MovieDetailDto>.Success(data, "成功查詢電影資訊!"));
    }



    // ==== POST /api/movies：新增電影 ============================================
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MovieCreateDto dto)
    {
        // [ApiController] 會自動驗證 [Required] 等 DataAnnotations，失敗直接回 400
        var data = await _service.CreateAsync(dto);
        return CreatedAtAction(
            nameof(GetById),                          // 指向 GET /api/movies/{id}
            new { id = data.Id },                     // 回傳 Location Header
            ApiResponse<MovieDetailDto>.Success(data, "成功新增電影資訊!")
        );
    }


}