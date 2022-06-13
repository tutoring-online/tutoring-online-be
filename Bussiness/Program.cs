using System.Configuration;
using System.Text;
using DataAccess.Entities.Student;
using DataAccess.Repository;
using DataAccess.Repository.MySqlRepository;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog;
using tutoring_online_be.Controllers.Utils;
using tutoring_online_be.Security.Filter;
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

//Authentication
builder.Services.AddSingleton<IAuthenticationService, AuthenticationServiceV1>();

//RefreshToken
builder.Services.AddSingleton<IRefreshTokenDao, RefreshTokenDao>();

//Admin
builder.Services.AddSingleton<IAdminService, AdminServiceV1>();
builder.Services.AddSingleton<IAdminDao, AdminDao>();

//Tutor
builder.Services.AddSingleton<ITutorService, TutorServiceV1>();
builder.Services.AddSingleton<ITutorDao, TutorDao>();

//Student
builder.Services.AddSingleton<IStudentService, StudentServiceV1>();
builder.Services.AddSingleton<IStudentDao, StudentDao>();


// Configure app setting
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));

// Clear cache timezone
System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();

// Security Configuration
// Middleware 
builder.Services.AddTransient<RequestResponseHandlerMiddleware>();
builder.Services.AddTransient<OptionsMiddleware>();

//Firebase configuration
string? json;
var filename = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

if (filename != null)
{
    json = System.IO.File.ReadAllText(filename);
}
else
{
    json = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS_STRING");
    if (json == null)
    {
        throw new Exception(
            "GOOGLE_APPLICATION_CREDENTIALS_STRING environment variable with JSON is not set");
    }
}

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromJson(json),
});

// // Jwt Configuration
// var secretKey = builder.Configuration["AppSettings:SecretKey"];
// var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
//
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(opt =>
//     {
//         opt.TokenValidationParameters = new TokenValidationParameters
//         {
//             //Self generate token
//             ValidateIssuer = false,
//             ValidateAudience = false,
//
//             //Sign token
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
//
//             ClockSkew = TimeSpan.Zero
//         };
//     });

// Build app
var app = builder.Build();

//Setup logger
NLog.Common.InternalLogger.LogLevel = NLog.LogLevel.Trace;
NLog.Common.InternalLogger.LogToConsole = true;
NLog.Common.InternalLogger.LogFile = "nlog.txt"; 
Logger logger = LogManager.GetLogger("Logger");
logger.Info("Program started");

//Config swagger
app.UseSwagger();
app.UseSwaggerUI();
    
// Configure the HTTP request pipeline.    
app.UseMiddleware<RequestResponseHandlerMiddleware>();
app.UseMiddleware<OptionsMiddleware>();

    
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
