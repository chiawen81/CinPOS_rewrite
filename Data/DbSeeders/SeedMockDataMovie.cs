using CinPOS_rewrite.Models;
using CinPOS_rewrite.Data;
using Microsoft.EntityFrameworkCore;
using CinPOS_rewrite.Enums;

public static class SeedMockDataMovies
{
	public static async Task SeedAsync(IServiceProvider services)
	{
		using var scope = services.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

		if (!db.Movies.Any(m => m.MovieId == "M001"))
		{
			var movie = new Movie
			{
				MovieId = "M001",
				Title = "關於我和鬼變成家人的那件事",
				EnTitle = "My Best Friend's Exorcism",
				Runtime = 134,
                Rate = MovieRate.General,											// Enum 處理
                Status = MovieStatus.Published,										// Enum 處理
                ReleaseDate = new DateTime(2023, 2, 10),
				Director = "程偉豪",
				Description = "測試用電影",
				PosterUrl = "https://placeholder.com/poster.jpg",
				CreatedAt = DateTime.Now,
				MovieGenres = new List<MovieGenre>									// Navigation Property 直接塞入關聯資料
				{
					new MovieGenre { GenreId = 3 },
					new MovieGenre { GenreId = 4 }
				},
                MovieProvideVersions = new List<MovieProvideVersion>				// Navigation Property 直接塞入關聯資料
				{
					new MovieProvideVersion { ProvideVersionId = 1 },
					new MovieProvideVersion { ProvideVersionId = 2 }
				},
                MovieCasts = new List<MovieCast>									// Navigation Property 直接塞入關聯資料
				{
					new MovieCast { ActorName = "許光漢" },
					new MovieCast { ActorName = "林柏宏" },
					new MovieCast { ActorName = "王淨" }
				}
			};

			db.Movies.Add(movie);
			await db.SaveChangesAsync();
			Console.WriteLine("[TEST] 電影新增成功");
		}

		var result = await db.Movies
			.Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
            .Include(m => m.MovieProvideVersions).ThenInclude(mpv => mpv.ProvideVersion)
            .Include(m => m.MovieCasts)
			.FirstOrDefaultAsync(m => m.MovieId == "M001");

		if (result != null)
		{
			Console.WriteLine("類型：" + string.Join(", ", result.MovieGenres.Select(mg => mg.Genre.GenreName)));
            Console.WriteLine("版本：" + string.Join(", ", result.MovieProvideVersions.Select(mpv => mpv.ProvideVersion.ProvideVersionName)));
            Console.WriteLine("演員：" + string.Join(", ", result.MovieCasts.Select(c => c.ActorName)));
            Console.WriteLine($"\n🎬 {result.Title} 資料初始化完成\n 測試 Navigation Property 關聯成功");
        }
    }
}