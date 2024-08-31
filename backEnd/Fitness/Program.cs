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
builder.Services.AddSingleton<IVigorTokenBLL, VigorTokenBLL>(); // �������ظ��ģ�������Ҫȥ��

// ��� IMemoryCache ����
builder.Services.AddMemoryCache();

// ע�� SignalR ����
builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    //�����ÿ��Դ����ͻ��˶��󣬷���ֻ�ܴ��ַ���
    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
});
;

// ע�� VerificationHelper ����
builder.Services.AddSingleton<VerificationHelper>();

// ���� CORS��������Դ����: ��Ϊ SignalR ��Ҫ֧�ֿ�������
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // ��������ƾ����֧��SignalR
    });
});

// ע�� DatabasePingService
builder.Services.AddHostedService<DatabasePingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var webSocketOptions = new WebSocketOptions()
{
    KeepAliveInterval = TimeSpan.FromSeconds(120),
};

// ���������
/*webSocketOptions.AllowedOrigins.Add("https://app.example.com");
webSocketOptions.AllowedOrigins.Add("https://www.app.example.com");
*/
// ��������������
webSocketOptions.AllowedOrigins.Add("http://localhost:5173");

app.UseWebSockets(webSocketOptions);

// �����м��˳��
app.UseRouting();      // �ȶ���·��

app.UseCors();         // ʹ��ȫ��CORS����

// ����������֤��Ӧ����������� app.UseAuthentication();

app.UseAuthorization(); // ��Ȩ�м��

// ���� SignalR ·��
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chathub"); // ӳ�� SignalR hub ��ָ��·��
    endpoints.MapControllers();            // ӳ�������
});

// ����Ӧ��
app.Run();
