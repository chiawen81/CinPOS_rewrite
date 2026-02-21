using CinPOS_rewrite.Models;

namespace CinPOS_rewrite.Data;

public static class SeedMockDataProvideVersions
{
    public static void DbInitialize(AppDbContext context)
    {
        // 確保資料庫已建立
        context.Database.EnsureCreated();

        // ProvideVersion 主檔
        if (!context.ProvideVersions.Any())
        {
            context.ProvideVersions.AddRange(
                new ProvideVersion { ProvideVersionName = "2D" },
                new ProvideVersion { ProvideVersionName = "3D" },
                new ProvideVersion { ProvideVersionName = "IMAX" },
                new ProvideVersion { ProvideVersionName = "4DX" }
            );
            context.SaveChanges();
            Console.WriteLine("[SEED] ProvideVersion 資料初始化完成");
        }
    }
}