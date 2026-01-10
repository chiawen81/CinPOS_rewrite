using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinPOS_rewrite.Data;
using CinPOS_rewrite.Models;

namespace CinPOS_rewrite.Controllers;

[ApiController]
[Route("users")] // 定義路由前綴為 /users
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    // 把之前註冊好的 AppDbContext 注入進來

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // =========================  真正的 API 本體  =========================
    // 取得所有使用者的 API
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync(); // 從資料庫取得所有使用者資料，等同於 SQL 的 SELECT * FROM Users
        return Ok(users);
    }


    // 建立新使用者的 API
    //// <原寫法>
    //[HttpPost]
    //public async Task<ActionResult<User>> CreateUser(User user)
    //{
    //    _context.Users.Add(user);
    //    await _context.SaveChangesAsync();

    //    return CreatedAtAction(
    //        nameof(GetUsers),
    //        new { id = user.Id },
    //        user
    //    );
    //}

    // <Dtos寫法>
    using CinPOS_rewrite.Dtos; // ← 新增這行

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(CreateUserDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetUsers),
            new { id = user.Id },
            user
        );
    }


    // ======================================================================
}
