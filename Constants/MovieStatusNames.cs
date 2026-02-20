using CinPOS_rewrite.Enums;

namespace CinPOS_rewrite.Constants;

public static class MovieStatusNames
{
    public static readonly Dictionary<MovieStatus, string> Names = new()
    {
        { MovieStatus.Offline,   "已下檔" },
        { MovieStatus.Draft,     "尚未發佈" },
        { MovieStatus.Published, "上映中" }
    };
}