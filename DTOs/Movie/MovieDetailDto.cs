using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CinPOS_rewrite.DTOs.Movie;

/**
 * 對應 GET /api/movies/{id} 的 data
 */

public class MovieDetailDto
{
    [Description("電影ID")]
    [DefaultValue("M001")]
    public string Id { get; set; } = null!;

    [Description("電影名稱")]
    [DefaultValue("關於我和鬼變成家人的那件事")]
    public string Title { get; set; } = null!;

    [Description("英文名稱")]
    [DefaultValue("My Best Friend's Exorcism")]
    public string? EnTitle { get; set; }

    [Description("電影類型ID清單")]
    public List<int> Genre { get; set; } = null!;

    [Description("電影類型名稱清單")]
    public List<string> GenreName { get; set; } = null!;

    [Description("片長(分鐘)")]
    [DefaultValue(134)]
    public int Runtime { get; set; }

    [Description("放映版本ID清單")]
    public List<int> ProvideVersion { get; set; } = null!;

    [Description("放映版本名稱清單")]
    public List<string> ProvideVersionName { get; set; } = null!;

    [Description("分級制度 (0:普遍級 6:保護級 12:輔12級 15:輔15級 18:限制級)")]
    [DefaultValue(0)]
    public int Rate { get; set; }

    [Description("分級名稱")]
    [DefaultValue("普遍級")]
    public string RateName { get; set; } = null!;

    [Description("導演")]
    [DefaultValue("程偉豪")]
    public string? Director { get; set; }

    [Description("演員清單")]
    public List<string> Cast { get; set; } = null!;

    [Description("電影簡介")]
    [DefaultValue("測試用電影")]
    public string? Description { get; set; }

    [Description("上映狀態 (-1:已下檔 0:籌備中 1:上映中)")]
    [DefaultValue(1)]
    public int Status { get; set; }

    [Description("上映狀態名稱")]
    [DefaultValue("上映中")]
    public string StatusName { get; set; } = null!;

    [Description("上映日期")]
    public DateTime ReleaseDate { get; set; }

    [Description("預告片連結")]
    [DefaultValue("https://www.youtube.com/watch?v=example")]
    public string? TrailerLink { get; set; }

    [Description("發行商")]
    [DefaultValue("華納兄弟")]
    public string? Distributor { get; set; }

    [Description("海報網址")]
    [DefaultValue("https://placeholder.com/poster.jpg")]
    public string? PosterUrl { get; set; }
}