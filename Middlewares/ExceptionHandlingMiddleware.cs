// ── ExceptionHandlingMiddleware.cs ───────────────────────────────────────
/**
 * ╔═══════════════════════════════════════════════════════╗
 * ║  ExceptionHandlingMiddleware — 全域例外處理 Middleware                                                      ║
 * ╠═══════════════════════════════════════════════════════╣
 * ║  職責：作為 Pipeline 第一關，統一攔截所有未處理的                                                            ║
 * ║        Exception，包裝成 ApiResponse<T> 格式回傳                                                             ║
 * ║                                                                                                              ║
 * ║  流程：                                                                                                      ║
 * ║  Request → InvokeAsync 放行 → [Controller/Service/Repo]                                                    ║
 * ║                ↑                                                                                            ║
 * ║                ∣ throw Exception（往回拋）                                                                  ║
 * ║                ∣                                                                                            ║   
 * ║            catch 攔截 → HandleExceptionAsync 分類回應                                                       ║
 * ║                                                                                                              ║
 * ║  ✓ 自訂 Exception（NotFoundException 等）→ 對應狀態碼                                                      ║
 * ║  ✓ 未知 Exception → 500 + 記錄 Log                                                                         ║
 * ║  ✗ 不處理業務邏輯（那是 Service 的事）                                                                      ║
 * ╚═══════════════════════════════════════════════════════╝
 */

using System.Text.Json;
using CinPOS_rewrite.DTOs.Common;
using CinPOS_rewrite.Exceptions;

namespace CinPOS_rewrite.Middlewares;

public class ExceptionHandlingMiddleware
{
    // ── 依賴注入 ──────────────────────────────────────────────────
    private readonly RequestDelegate _next;     // Pipeline 中下一個 Middleware 的參考
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }


    // ══════════════════════════════════════════════════════════════
    //  InvokeAsync — Middleware 入口（ASP.NET Core 規定必須有此方法）
    //  職責：放行請求 or 攔截 Exception；若下游拋出 Exception，負責攔截並交給 HandleExceptionAsync
    // ══════════════════════════════════════════════════════════════
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);   // 放行至下一個 Middleware（正常流程不會進 catch）
        }
        catch (Exception ex)
        {
            // Exception 從 Repository/Service/Controller 一路往回拋，在此統一攔截
            _logger.LogError(ex, "未處理的例外：{Message}", ex.Message); // 記錄 Log（排查 500 錯誤的關鍵）
            await HandleExceptionAsync(context, ex);
        }
    }


    // ══════════════════════════════════════════════════════════════
    //  HandleExceptionAsync — 例外分類與回應組裝
    //  職責：依 Exception 型別對應 HTTP 狀態碼，統一包裝成 ApiResponse 回傳
    //  分離原因：單一職責——InvokeAsync 只管「攔不攔」，這裡只管「攔到後怎麼回」
    // ══════════════════════════════════════════════════════════════
    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        // ── 依 Exception 型別決定狀態碼與訊息 ────────────────────
        // 自訂 Exception（有貼標籤）→ 精確對應；其餘一律 500
        var (statusCode, message) = ex switch
        {
            NotFoundException e => (StatusCodes.Status404NotFound, e.Message),
            BadRequestException e => (StatusCodes.Status400BadRequest, e.Message),
            _ => (StatusCodes.Status500InternalServerError, "伺服器發生未知錯誤，請稍後再試")
        };

        // ── 組裝統一回應格式並寫入 Response ──────────────────────
        var response = ApiResponse<object>.Fail(message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase  // camelCase 輸出（符合前端 JS 慣例）
        });

        await context.Response.WriteAsync(json);
    }
}