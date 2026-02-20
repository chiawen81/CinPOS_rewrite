/*————————————————————————————————————————
§【檔案角色】
  - AppDbContext.cs 是整個系統「與資料庫互動的入口點」
  - 所有資料表的存取，都必須透過這個類別進行

§【它在做什麼】
  - 繼承 DbContext，交由 EF Core 管理資料庫操作
  - 負責：
   1. 管理資料庫連線
   2. 將 C# 操作轉換成 SQL 指令
   3. 提供對資料庫的 CRUD 操作（新增 / 查詢 / 更新 / 刪除）
   4. 追蹤資料變更，決定何時寫入資料庫

§【DbSet<T> 的意義】
  - 每一個 DbSet<T> 代表資料庫中的「一張表」
  - T 是對應的 Entity 類別

§【以 User 為例】
  - DbSet<User> Users;
    表示：
     1. 資料庫中有一張 Users 資料表
     2. 每一筆資料會對應到一個 User 物件
     3. EF Core 會自動處理 Users 表與 User 類別之間的對應關係

§【總結一句話】
  - AppDbContext = 資料庫的「總管」
  - DbSet = 一張資料表的「程式入口」
————————————————————————————————————————*/

using Microsoft.EntityFrameworkCore;
using CinPOS_rewrite.Models;

namespace CinPOS_rewrite.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    // 使用者
    public DbSet<User> Users { get; set; }

    // 票券類型
    public DbSet<TicketType> TicketTypes { get; set; }

    // 選項
    public DbSet<Option> Options { get; set; }

    // 電影票類型
    public DbSet<Ticket> Tickets { get; set; }

    // 電影資訊
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<MovieProvideVersion> MovieProvideVersions { get; set; }
    public DbSet<ProvideVersion> ProvideVersions { get; set; }
    public DbSet<MovieCast> MovieCasts { get; set; }

}
