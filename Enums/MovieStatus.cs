namespace CinPOS_rewrite.Enums;

public enum MovieStatus
{
    // -1 = 已下檔(下線)
    Offline = -1,

    // 0 = 尚未發佈(籌備中)
    Draft = 0,

    // 1 = 已發佈(上映中)
    Published = 1
}