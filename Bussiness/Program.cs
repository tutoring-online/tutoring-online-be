using DataAccess.Repository;
using DataAccess.Repository.MySqlRepository;
using tutoring_online_be.Services;
using tutoring_online_be.Services.V1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dependency Injection
//Subject
builder.Services.AddSingleton<ISubjectService, SubjectServiceV1>();
builder.Services.AddSingleton<ISubjectDao, SubjectDao>();
//Lesson
builder.Services.AddSingleton<ILessonService, LessonServiceV1>();
builder.Services.AddSingleton<ILessonDao, LessonDao>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
