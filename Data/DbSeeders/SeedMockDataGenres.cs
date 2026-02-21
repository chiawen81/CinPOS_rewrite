using CinPOS_rewrite.Models;

namespace CinPOS_rewrite.Data;

public static class SeedMockDataGenres
{
    public static void DbInitialize(AppDbContext context)
    {
        // 確保資料庫已建立
        context.Database.EnsureCreated();

        // Genre 主檔
        if (!context.Genres.Any())
        {
            context.Genres.AddRange(
                new Genre { GenreName = "動作" },
                new Genre { GenreName = "冒險" },
                new Genre { GenreName = "喜劇" },
                new Genre { GenreName = "劇情" },
                new Genre { GenreName = "恐怖" },
                new Genre { GenreName = "科幻" },
                new Genre { GenreName = "浪漫愛情" },
                new Genre { GenreName = "動畫" },
                new Genre { GenreName = "紀錄片" },
                new Genre { GenreName = "音樂" },
                new Genre { GenreName = "懸疑" },
                new Genre { GenreName = "驚悚" },
                new Genre { GenreName = "犯罪" }
            );
            context.SaveChanges();
            Console.WriteLine("[SEED] Genre 資料初始化完成");
        }
    }
}