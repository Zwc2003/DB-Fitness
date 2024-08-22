// 创建Web应用程序生成器，并传入命令行参数
using FitnessWebAPI.BLL;
using FitnessWebAPI.BLL.Core;
using FitnessWebAPI.BLL.Interfaces;
using FitnessWebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

//注册 SignalR 服务
builder.Services.AddSignalR();

// 向依赖注入容器添加控制器服务
builder.Services.AddControllers();

// 添加端点API探查器，用于生成API描述信息
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// 添加Swagger生成器服务，用于生成API文档
builder.Services.AddSwaggerGen();

// 添加 IMemoryCache 服务
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IUserBLL, UserBLL>();
builder.Services.AddSingleton<ICourseBLL, CourseBLL>();
builder.Services.AddSingleton<IPostBLL, PostBLL>();
builder.Services.AddSingleton<ICommentBLL, CommentBLL>();
// 注册 VerificationHelper 服务
builder.Services.AddSingleton<VerificationHelper>();

//配置 CORS（跨域资源共享）:通常在前后端分离的项目中使用，因为 SignalR 需要支持跨域请求。
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// 构建Web应用程序实例
var app = builder.Build();

// 使用 CORS 中间件
app.UseCors();

/*// 如果你想手动设置端口，可以使用以下代码
app.Urls.Add("http://localhost:8080");*/

// 使用 SignalR 中间件
app.UseRouting();


/*//这一行代码将 ChatHub 类映射到 /chathub 路径。
//当客户端连接到 http://yourdomain/chathub 时，它会与 SignalR Hub 建立连接。
app.UseEndpoints(endpoints =>
{
    // 配置 SignalR Hub
    endpoints.MapHub<ChatHub>("/chatHub");
});*/


// 配置HTTP请求管道

// 如果应用程序在开发环境中运行
if (app.Environment.IsDevelopment())
{
    // 启用Swagger中间件，生成Swagger文档
    app.UseSwagger();
    // 启用Swagger UI，用于查看和测试API
    app.UseSwaggerUI();
}

// 启用HTTPS重定向中间件，将HTTP请求重定向到HTTPS
app.UseHttpsRedirection();

// 启用授权中间件，确保只有经过授权的请求才能访问受保护的资源
/*app.UseAuthorization();*/

// 将控制器路由映射到终结点
app.MapControllers();

// 启动应用程序，开始处理传入的HTTP请求
app.Run();

