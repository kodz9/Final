using Final.Models;
using Final.Service;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddSwaggerGen(c =>
{
    // 配置 Swagger 支持 Bearer Token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your Bearer token"
    });

    // 配置 Swagger UI 显示 Bearer Token 输入框
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

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
    if (path == "/api/User/login" || path == "/api/User/logout"||path == "/api/User/register")
    {
        // 对于登录和登出，注册请求，直接继续处理
        await next();
        return;
    }

    // 从请求头的 Authorization 中获取 Token
    string token = ctx.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

    // 如果 token 不存在，返回 Unauthorized
    if (string.IsNullOrEmpty(token))
    {
        ctx.Response.StatusCode = 401; // Unauthorized
        await ctx.Response.WriteAsync("Unauthorized");
        return;
    }

    // 通过 Token 获取用户
    var user = UserService.Instance.GetUserByToken(token);
    if (user == null)
    {
        ctx.Response.StatusCode = 401; // Unauthorized
        await ctx.Response.WriteAsync("Unauthorized");
        return;
    }

    // 设置用户信息到请求的特性中，供后续中间件或控制器使用
    ctx.Features.Set<User>(user);
    await next();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
