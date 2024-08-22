// ����WebӦ�ó����������������������в���
using FitnessWebAPI.BLL;
using FitnessWebAPI.BLL.Core;
using FitnessWebAPI.BLL.Interfaces;
using FitnessWebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

//ע�� SignalR ����
builder.Services.AddSignalR();

// ������ע��������ӿ���������
builder.Services.AddControllers();

// ��Ӷ˵�API̽��������������API������Ϣ
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// ���Swagger������������������API�ĵ�
builder.Services.AddSwaggerGen();

// ��� IMemoryCache ����
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IUserBLL, UserBLL>();
builder.Services.AddSingleton<ICourseBLL, CourseBLL>();
builder.Services.AddSingleton<IPostBLL, PostBLL>();
builder.Services.AddSingleton<ICommentBLL, CommentBLL>();
// ע�� VerificationHelper ����
builder.Services.AddSingleton<VerificationHelper>();

//���� CORS��������Դ����:ͨ����ǰ��˷������Ŀ��ʹ�ã���Ϊ SignalR ��Ҫ֧�ֿ�������
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// ����WebӦ�ó���ʵ��
var app = builder.Build();

// ʹ�� CORS �м��
app.UseCors();

/*// ��������ֶ����ö˿ڣ�����ʹ�����´���
app.Urls.Add("http://localhost:8080");*/

// ʹ�� SignalR �м��
app.UseRouting();


/*//��һ�д��뽫 ChatHub ��ӳ�䵽 /chathub ·����
//���ͻ������ӵ� http://yourdomain/chathub ʱ�������� SignalR Hub �������ӡ�
app.UseEndpoints(endpoints =>
{
    // ���� SignalR Hub
    endpoints.MapHub<ChatHub>("/chatHub");
});*/


// ����HTTP����ܵ�

// ���Ӧ�ó����ڿ�������������
if (app.Environment.IsDevelopment())
{
    // ����Swagger�м��������Swagger�ĵ�
    app.UseSwagger();
    // ����Swagger UI�����ڲ鿴�Ͳ���API
    app.UseSwaggerUI();
}

// ����HTTPS�ض����м������HTTP�����ض���HTTPS
app.UseHttpsRedirection();

// ������Ȩ�м����ȷ��ֻ�о�����Ȩ��������ܷ����ܱ�������Դ
/*app.UseAuthorization();*/

// ��������·��ӳ�䵽�ս��
app.MapControllers();

// ����Ӧ�ó��򣬿�ʼ�������HTTP����
app.Run();

