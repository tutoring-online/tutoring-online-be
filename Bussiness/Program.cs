using DataAccess.Repository;
using DataAccess.Repository.MySqlRepository;
using NLog;
using tutoring_online_be.Services;
using tutoring_online_be.Services.V1;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

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

//Syllabus
builder.Services.AddSingleton<ISyllabusService, SyllabusServiceV1>();
builder.Services.AddSingleton<ISyllabusDao, SyllabusDao>();

//Payment
builder.Services.AddSingleton<IPaymentService, PaymentServiceV1>();
builder.Services.AddSingleton<IPaymentDao, PaymentDao>();
var app = builder.Build();

//Setup logger
NLog.Common.InternalLogger.LogLevel = NLog.LogLevel.Trace;
NLog.Common.InternalLogger.LogToConsole = true;
NLog.Common.InternalLogger.LogFile = "nlog.txt"; 
Logger logger = LogManager.GetLogger("Logger");
logger.Info("Program started");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
