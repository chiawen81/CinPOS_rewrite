using System.ComponentModel.DataAnnotations;

namespace CinPOS_rewrite.DTOs.Common;
/**
 * ²Î¤@ response wrapper
 */

public class ApiResponse<T>
{
    public int Code { get; set; }

    public string Message { get; set; } = null!;

    public T? Data { get; set; }

    public static ApiResponse<T> Success(T data, string message) =>
        new() { Code = 1, Message = message, Data = data };

    public static ApiResponse<object> Fail(string message) =>
        new ApiResponse<object> { Code = -1, Message = message };
}