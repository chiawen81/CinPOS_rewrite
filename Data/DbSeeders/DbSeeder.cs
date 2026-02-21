using Microsoft.EntityFrameworkCore;
using CinPOS_rewrite.Data;

namespace CinPOS_rewrite.Data.Seeding;

public static class DbSeeder
{
    public static async Task SeedAllAsync(IServiceProvider services, bool clearExisting = false)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        //// 清除舊資料（如果需要的話）
        //if (clearExisting)
        //{
        //    await ClearDataAsync(context);
        //}

        // 依序執行塞假資料
        SeedMockDataGenres.DbInitialize(context);           // 電影類型代碼與中文
        SeedMockDataProvideVersions.DbInitialize(context);  // 提供版本代碼與中文
        await SeedMockDataMovies.SeedAsync(services);       // 電影資料
    }



    // 清除舊資料
    private static async Task ClearDataAsync(AppDbContext context)
    {
        // 注意：要按照外鍵依賴順序刪除，先刪子表再刪主表
        context.MovieCasts.RemoveRange(context.MovieCasts);
        context.MovieGenres.RemoveRange(context.MovieGenres);
        context.MovieProvideVersions.RemoveRange(context.MovieProvideVersions);
        context.Movies.RemoveRange(context.Movies);
        context.Genres.RemoveRange(context.Genres);
        context.ProvideVersions.RemoveRange(context.ProvideVersions);
        
        await context.SaveChangesAsync();
        Console.WriteLine("✅ 舊資料已清除");
    }
}
/**NOTE
 * 1. 刪除資料庫
 *   (1) 執行 ClearDataAsync
 *   (2) 在終端機打指令
 *      dotnet ef database drop --force     # 刪除資料庫
 *      dotnet ef database update           # 重新建立資料庫
 */