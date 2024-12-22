using Final.Models;
using Final.Service;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WebContext>(opt =>
{
    opt.UseSqlite("Data Source = D:\\database.db");
});


var app = builder.Build();

// 确保数据库在应用启动时被创建和更新
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WebContext>();
    dbContext.Database.Migrate(); // 应用所有未应用的迁移
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.Use(async (ctx, next) =>
{
    // 检查请求路径
    var path = ctx.Request.Path.Value;
    if (path == "/api/User/login" || path == "/api/User/logout")
    {
        // 对于登录和登出请求，直接继续处理
        await next();
        return;
    }

    // 对其他请求进行token验证
    string token = ctx.Request.Query["token"].ToString();
    var user = UserService.Instance.GetUserByToken(token);
    if (user == null)
    {
        ctx.Response.StatusCode = 401; // Unauthorized
        await ctx.Response.WriteAsync("Unauthorized");
        return;
    }

    ctx.Features.Set<User>(user);
    await next();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
