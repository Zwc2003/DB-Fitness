using System;
using Oracle.ManagedDataAccess.Client;
using Fitness.DAL.Core;
using System.Data;
using Fitness.BLL;
using Fitness.BLL.Interfaces;
using Fitness.BLL.Core;
using Fitness.Models;
using Fitness.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add BLL Singleton
builder.Services.AddSingleton<IFoodPlanBLL, FoodPlanBLL>();
builder.Services.AddSingleton<IMealRecordBLL, MealRecordBLL>();
builder.Services.AddSingleton<IFitnessALGuideBLL, FitnessAIGuideBLL>();
builder.Services.AddSingleton<IUserBLL, UserBLL>();
builder.Services.AddSingleton<IVigorTokenBLL, VigorTokenBLL>();
builder.Services.AddSingleton<ICourseBLL, CourseBLL>();
builder.Services.AddSingleton<IPostBLL, PostBLL>();
builder.Services.AddSingleton<ICommentBLL, CommentBLL>();
builder.Services.AddSingleton<IMessageBLL, MessageBLL>();
builder.Services.AddSingleton<IVigorTokenBLL, VigorTokenBLL>();

//添加 IMemoryCache 服务
builder.Services.AddMemoryCache();
//注册 SignalR 服务
builder.Services.AddSignalR();
//注册 VerificationHelper 服务
builder.Services.AddSingleton<VerificationHelper>();
//配置 CORS（跨域资源共享）:因为 SignalR 需要支持跨域请求。
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod();
              // .AllowCredentials(); // 必须允许凭据来支持SignalR
    });
});

// 注册 DatabasePingService
builder.Services.AddHostedService<DatabasePingService>();

var app = builder.Build();

// 使用 CORS 中间件
app.UseCors();
// 使用 SignalR 中间件
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chatHub");//这一行代码将 ChatHub 类映射到 /chathub 路径。
    //当客户端连接到 http://yourdomain/chatHub 时，它会与 SignalR Hub 建立连接。
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

//服务端启用后端跨域请求协议
app.UseCors(builder =>
{
    builder.AllowAnyHeader()
           .AllowAnyOrigin()
           .AllowAnyMethod();
});

app.MapControllers();

app.Run();
