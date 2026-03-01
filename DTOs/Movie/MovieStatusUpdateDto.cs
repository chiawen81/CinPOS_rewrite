using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CinPOS_rewrite.DTOs.Movie;

/**
 * PATCH /api/movies/{id}/status
 */

public class MovieStatusUpdateDto
{
    [Required] 
    [Description("上映狀態 (-1:已下檔 0:籌備中 1:上映中)")]
    [DefaultValue(1)]
    public int Status { get; set; }
}