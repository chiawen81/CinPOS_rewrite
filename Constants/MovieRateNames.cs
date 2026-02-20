using CinPOS_rewrite.Enums;

namespace CinPOS_rewrite.Constants;

public static class MovieRateNames
{
    public static readonly Dictionary<MovieRate, string> Names = new()
    {
        { MovieRate.General,    "普通級" },
        { MovieRate.Protected,  "保護級" },
        { MovieRate.Guidance12, "輔導十二級" },
        { MovieRate.Guidance15, "輔導十五級" },
        { MovieRate.Restricted, "限制級" }
    };
}