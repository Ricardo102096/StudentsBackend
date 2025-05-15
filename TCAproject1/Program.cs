using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TCAproject1.Controllers.Requests;
using TCAproject1.Data;
using TCAproject1.Middleware;
using Microsoft.OpenApi.Models;

AppDomain.CurrentDomain.SetData("DataDirectory", AppContext.BaseDirectory);
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API v1 (Default)", Version = "v1" });
    options.SwaggerDoc("v2", new() { Title = "API v2", Version = "v2" });

    options.DocInclusionPredicate((docName, apiDesc) =>
{
    // Para V1: rutas que no contienen versión explícita (como api/student)
    if (docName == "v1" && !apiDesc.RelativePath.Contains("v2"))
        return true;

    // Para V2: rutas que empiezan con api/v2/
    if (docName == "v2" && apiDesc.RelativePath.StartsWith("api/v2"))
        return true;

    return false;
});
});
builder.Services.AddCors();
builder.Services.AddScoped<IValidator<NewStudent>, NewStudentValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
    });
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
