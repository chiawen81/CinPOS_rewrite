using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CinPOS_rewrite.DTOs.Common;
/**
 * 統一 response wrapper
 */

public class ApiResponse<T>
{
    [Description("狀態碼 (1:成功 -1:失敗)")]
    [DefaultValue(1)]
    public int Code { get; set; }

    [Description("回應訊息")]
    [DefaultValue("操作成功")]
    public string Message { get; set; } = null!;

    [Description("回應資料")]
    public T? Data { get; set; }

    public static ApiResponse<T> Success(T data, string message) =>
        new() { Code = 1, Message = message, Data = data };

    public static ApiResponse<object> Fail(string message) =>
        new ApiResponse<object> { Code = -1, Message = message };
}