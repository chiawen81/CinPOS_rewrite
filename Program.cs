/**
 * ╔══════════════════════════════════════════════════════════╗
 * ║  Program.cs — 應用程式進入點                                                                                      ║
 * ╠══════════════════════════════════════════════════════════╣
 * ║  ASP.NET Core 啟動流程分兩階段：                                                                                   ║
 * ║  1. Builder 階段：註冊所有服務（DI 容器）                                                                          ║
 * ║  2. App 階段：設定 HTTP Pipeline（Middleware 順序）                                                                ║
 * ╚══════════════════════════════════════════════════════════╝
 */

// ── 引用命名空間 ──────────────────────────────────────────────────
using Microsoft.EntityFrameworkCore;                // UseSqlServer() 等 EF Core 擴充方法
using CinPOS_rewrite.Data;                          // AppDbContext（DB 連線與 DbSet）
using CinPOS_rewrite.Data.Seeding;                  // DbSeeder（開發用假資料，目前已停用）
using CinPOS_rewrite.Repositories;                  // IMovieRepository、MovieRepository
using CinPOS_rewrite.Services;                      // IMovieService、MovieService


// ======================================================================================================================
//  階段一：Builder — 建立應用程式並註冊所有服務（DI 容器）
// ======================================================================================================================
var builder = WebApplication.CreateBuilder(args);   // 建立 WebApplication 的建造者，載入設定檔（appsettings.json）

// ── 註冊資料庫 ───────────────────────────────────────────────────
// DbContext 是「資料庫的操作入口」，職責：
//   ・管理資料庫連線
//   ・將 LINQ 查詢轉換成 SQL
//   ・追蹤 Entity 的變更狀態（Change Tracking）
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection") // 從 appsettings.json 讀取連線字串
    ));

// ── 註冊 MVC Controllers ──────────────────────────────────────────────
// 掃描所有繼承 ControllerBase 的類別，將其納入路由系統
builder.Services.AddControllers();

// ── 註冊 Repository 與 Service（依賴注入）─────────────────────────────────────
// AddScoped：每次 HTTP Request 建立一個新實例，Request 結束後釋放
// 格式：AddScoped<介面, 實作> → 讓 DI 容器知道「看到 I 要給 實作」
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

// ── 註冊 OpenAPI（Swagger 文件）──────────────────────────────────────────
builder.Services.AddOpenApi();      // 自動產生 API 文件，僅開發環境使用



// ======================================================================================================================
//  階段二：App — 建置應用程式並設定 HTTP Pipeline（Middleware）
//  注意：Middleware 有順序性，順序錯誤會導致功能異常
// ======================================================================================================================
var app = builder.Build();          // 根據上方所有註冊，建置最終的應用程式實例

// ── 開發環境專用 Middleware ────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();               // 開發環境才掛載 OpenAPI 端點（/openapi/v1.json）
}

// ── 正式 Middleware Pipeline ────────────────────────────────────────────
app.UseHttpsRedirection();          // 將 HTTP 請求自動導向 HTTPS
app.UseAuthorization();             // 啟用授權機制（驗證 JWT / Policy 等）
app.MapControllers();               // 將 Controller 的路由對應到 HTTP 端點

// ── 開發工具：資料庫假資料 Seeder（停用中）─────────────────────────────────────
// 用途：重建資料庫時打開此段，可一次塞入完整測試資料
// 使用方式：取消下方註解 → dotnet run → 資料塞完後再次註解
//if (app.Environment.IsDevelopment())
//{
//    await DbSeeder.SeedAllAsync(app.Services, clearExisting: true); // clearExisting: true = 先清除舊資料再塞
//}


app.Run();                          // 啟動應用程式，開始監聽 HTTP 請求