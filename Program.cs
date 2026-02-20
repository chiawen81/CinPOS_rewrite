using Microsoft.EntityFrameworkCore;
using CinPOS_rewrite.Data;
using CinPOS_rewrite.Models;

var builder = WebApplication.CreateBuilder(args);

// 註冊 AppDbContext，並設定使用 SQL Server 資料庫
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
// 補充- DbContext
//   角色是「資料庫的操作入口」，負責：
//     1. 管理資料庫連線
//     2. 把 SQL 變成 C# 操作，提供對資料庫的 CRUD 操作
//     3. 提供對資料庫的 CRUD 操作
//     4. 跟踪實體的變更狀態


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ————————————————  塞假資料 ———————————————— 
//// 電影類型代碼與對應中文
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    SeedMockDataGenres.DbInitialize(context);
//}


//// 影廳設備代碼與對應中文
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    SeedMockDataProvideVersions.DbInitialize(context);
//}


//// 電影
//await SeedMockDataMovies.SeedAsync(app.Services);
///**NOTE
// * 用途：測試新增電影並一次撈出它的 Navigation Property（ex:類型、演員）
// */


app.Run();
