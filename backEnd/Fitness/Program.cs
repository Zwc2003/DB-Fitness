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

//��� IMemoryCache ����
builder.Services.AddMemoryCache();
//ע�� SignalR ����
builder.Services.AddSignalR();
//ע�� VerificationHelper ����
builder.Services.AddSingleton<VerificationHelper>();
//���� CORS��������Դ����:��Ϊ SignalR ��Ҫ֧�ֿ�������
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod();
              // .AllowCredentials(); // ��������ƾ����֧��SignalR
    });
});

// ע�� DatabasePingService
builder.Services.AddHostedService<DatabasePingService>();

var app = builder.Build();

// ʹ�� CORS �м��
app.UseCors();
// ʹ�� SignalR �м��
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chatHub");//��һ�д��뽫 ChatHub ��ӳ�䵽 /chathub ·����
    //���ͻ������ӵ� http://yourdomain/chatHub ʱ�������� SignalR Hub �������ӡ�
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

//��������ú�˿�������Э��
app.UseCors(builder =>
{
    builder.AllowAnyHeader()
           .AllowAnyOrigin()
           .AllowAnyMethod();
});

app.MapControllers();

app.Run();
