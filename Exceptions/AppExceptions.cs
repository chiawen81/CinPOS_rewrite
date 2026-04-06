// ── AppExceptions.cs ─────────────────────────────────────────────
/**
 * ╔═══════════════════════════════════════════════════════╗
 * ║  AppExceptions — 自訂例外類別                                                                               ║
 * ╠═══════════════════════════════════════════════════════╣
 * ║  職責：定義可預期的業務錯誤，讓 Middleware 能精確對應                                                        ║
 * ║        HTTP 狀態碼                                                                                           ║
 * ║  ✓ Service 層主動 throw 這些 Exception                                                                      ║
 * ║  ✓ Middleware 攔截後依型別回傳對應狀態碼                                                                    ║
 * ║  ✗ 不處理未知錯誤（那些統一歸 500）                                                                         ║
 * ╚═══════════════════════════════════════════════════════╝
 */

namespace CinPOS_rewrite.Exceptions;

// ── 404 查無資料 ────────────────────────────────────────────────
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

// ── 400 業務規則驗證失敗 ────────────────────────────────────────────
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}