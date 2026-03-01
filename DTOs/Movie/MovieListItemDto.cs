using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CinPOS_rewrite.DTOs.Movie;
/**
 * 對應 GET /api/movies 的 data[]
 */

public class MovieListItemDto
{
    [Description("電影ID")]
    [DefaultValue("M001")]
    public string Id { get; set; } = null!;

    [Description("上映狀態名稱")]
    [DefaultValue("上映中")]
    public string StatusName { get; set; } = null!;

    [Description("電影名稱")]
    [DefaultValue("關於我和鬼變成家人的那件事")]
    public string Title { get; set; } = null!;

    [Description("電影類型名稱清單")]
    public List<string> GenreName { get; set; } = null!;

    [Description("片長(分鐘)")]
    [DefaultValue(134)]
    public int Runtime { get; set; }

    [Description("分級制度 (0:普遍級 6:保護級 12:輔12級 15:輔15級 18:限制級)")]
    [DefaultValue(0)]
    public int Rate { get; set; }

    [Description("分級名稱")]
    [DefaultValue("普遍級")]
    public string RateName { get; set; } = null!;

    [Description("上映日期")]
    public DateTime ReleaseDate { get; set; }

    [Description("放映版本名稱清單")]
    public List<string> ProvideVersionName { get; set; } = null!;
}