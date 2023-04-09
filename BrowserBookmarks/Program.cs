using BrowerBookmariks.Model;
using BrowerBookmariks.Model.Entitys;
using BrowerBookmariks.Model.Services;
using BrowerBookmariks.Services.BookTop;
using BrowerBookmariks.Services.Classifications;
using BrowerBookmariks.Services.Menu;
using BrowerBookmariks.Services.NewBookMark;
using BrowserBookmarks.Filters;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);


//��ֹ����ֵ�����ظ�
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
//ע��ͳһ��ʽ����ֵ������
var mvcBuilder = builder.Services.AddControllersWithViews(
    options => { options.Filters.Add<ResponseWrapperFilter>(); }
);
//���ݿ�����
builder.Services.AddDbContext<MyDbContext>(opt =>
{
    //string connStr = builder.Configuration.GetSection("ConnStr").Value;
    string connStr = "Server=101.43.25.210;Port=3306;Database=BookMark; User=root;Password=zyplj1314999;";
    opt.UseMySql(connStr, new MySqlServerVersion(new Version(5, 7, 40)));
});
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddControllersWithViews();
//���ú�˶˿�
builder.WebHost.UseUrls("http://*:9031");
//����
//��ӿ������
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", 
        opt => opt.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("http://localhost:9030/"));
});
//�����ע��
builder.Services.AddTransient<IBookmarks, Bookmarks>();
builder.Services.AddTransient<IBookTopService, BookTopService>();
builder.Services.AddTransient<IClassifications, Classifications>();
builder.Services.AddTransient<IMenusService, MenusService>();
builder.Services.AddTransient<INewBookmarkService, NewBookmarkService>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//��������
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
