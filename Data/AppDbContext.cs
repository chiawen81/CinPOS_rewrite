/* AppDbContext.cs 代表的意思：整個資料庫的操作入口
 * 以下的程式碼意義：跟EF Core 說這個資料庫裡，有一張表叫 Users，對應的資料型別是 User

 */

using Microsoft.EntityFrameworkCore;
using CinPOS_rewrite.Models;

namespace CinPOS_rewrite.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    // 註冊要被當成資料表的類別，由 EF Core 來管理
    public DbSet<User> Users { get; set; }
    // 意義：DbSet<User> 代表資料庫裡有一張叫 Users 的表，對應的資料型別是 User
}
