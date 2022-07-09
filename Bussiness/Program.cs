using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Text;
using System.Text.Json.Serialization;
using DataAccess.Entities.Student;
using DataAccess.Repository;
using DataAccess.Repository.MySqlRepository;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog;
using tutoring_online_be.Controllers.Utils;
using tutoring_online_be.Security;
using tutoring_online_be.Security.Filter;
using tutoring_online_be.Services;
using tutoring_online_be.Services.V1;
using tutoring_online_be.Utils;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

//Null handler in response
// builder.Services.AddControllers().AddJsonOptions(options =>
// {
//     options.JsonSerializerOptions.IgnoreNullValues = true;
// });

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
builder.Services.AddTransient<PaymentServiceV1>();
builder.Services.AddTransient<PaymentServiceV1>();
builder.Services.AddTransient<IPaymentService.ServiceResolver>(serviceProvider => key =>
{
    switch (key)
    {
        case "payment-v1":
            return serviceProvider.GetService<PaymentServiceV1>();
        default:
            throw new KeyNotFoundException(); 
    }
});
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

//Tutor-Subject
builder.Services.AddSingleton<ITutorSubjectService, TutorSubjectServiceV1>();
builder.Services.AddSingleton<ITutorSubjectDao, TutorSubjectDao>();

//Student
builder.Services.AddSingleton<IStudentService, StudentServiceV1>();
builder.Services.AddSingleton<IStudentDao, StudentDao>();

//Category
builder.Services.AddSingleton<ICategoryService, CategoryServiceV1>();
builder.Services.AddSingleton<ICategoryDao, CategoryDao>();


// Configure app setting
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));

// Clear cache timezone
System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();

// Security Configuration
// Middleware 
builder.Services.AddTransient<RequestResponseHandlerMiddleware>();
builder.Services.AddTransient<ExceptionMiddleware>();
builder.Services.AddTransient<OptionsMiddleware>();


//Add filter
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ValidateModelAttribute>();
});


//Firebase configuration
string? json;
var filename = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

if (filename != null)
{
    json = System.IO.File.ReadAllText(filename);
}
else
{
    json =
        "{\n  \"type\": \"service_account\",\n  \"project_id\": \"tutoring-online-e3711\",\n  \"private_key_id\": \"8f3451a800324b137da0aeb8f6119f875b459654\",\n  \"private_key\": \"-----BEGIN PRIVATE KEY-----\\nMIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQDwEN8azJSuDi59\\nQn6XnQRaWMIRTRzQXsPhQmvbH2B7WFw8vqzp2CR4MB4FH7ZAdBvIdEW67fRYkSlf\\nDRQm2oQIVEX5Mfad0RmO1Y/0eF0av1L3Mhm9VmOOEFCCd2XjICpEKPkSOMsoDDfz\\nkNsd8Yz9FPY6Ju99HI2hqyHijiLqlA5sfKSrrx4Y6xRzcx4H5+z+naNNRz5o3y/R\\nykBfPmfWmohmsFYKDlsAhx2U6zqUxKWqyCmaLZsTGkU4lEGEIoWv1xGiaJAsHCCI\\nTk30Rx++LC0ZoE666PD9Oa4I6iA+fksY5dCsA1dTXetVaMbO5yIganBCUUmMvKzU\\nU3ChOd59AgMBAAECggEAAsQ1jT8CMrQxZ/ST+7NiG2mytKPhWZ+6TpfqCdFZ4jwt\\n7+4s8jXUHWWLGZB4fTyn106oEoJvZACea+5Osru+AJYGNgqIOD1ylNCRTsvzYUNM\\n5AxFnh3+VIW6PlR26Ci6mFMe6dj6sxTDgqmgLYUdCPRPkS0raipzS+xTEmmbuSAx\\n2WYPF0DgnFE4yaX5GlciQptyOjBmbdb2Ohj1lKC/jDndnCsRI0gC5eELuyeya2lg\\nkf2qH86UAUObsLglBdPgUEW7WpB24zrqDhMqQCv8yPMp4fnIK4yIc7rvzPgHjsiI\\n+MqtLcubjX9cQwsAaUFZGV4l+L8FfoPWAVkrGBGx8QKBgQD/SZSzn1uAfhkjzxQT\\nzrZdUFJdAzMbtBv+cAfhsz0ef7531gS2s8E9zbJVULfHMY9lawNSFuyykpBD/Gk5\\n6g4WWsUawkW/oAbl2p0ZCVCXdY3LN8av82hSeREAO+ASq0SBcwttUIohdfjCCFvE\\nebEQi/tF4X/+9GAA/L35QEa1BQKBgQDwvGn0s6Z6fHJQjZ44FBqZtOVI+H9hOf/k\\nbkjF48cMFjUJ2eupQ9KvURSMaotja0n3E3wEPFcNIjSJurDjKBDX3q1LIfU9RoW7\\nhZqKFeqLWwxjj4g/pcCPorQ3cI7MG/wCLRFLwx3oxmPXPb0W1J9iRi/RKCJ8YXXH\\nb8TlQ3A9GQKBgAELP/xsDme8HEY1NpPOKJjBF1UiCjd2yRaFRsL5hKp3Q0QiL+q/\\nWW7zRGNs7RN3dGqpwV24kkc4qjZc9eEyv9P/kwbE/JwH/385IaNUkmvMI0RNehaG\\nHEsaC6PAmu34nVMaMVXFGouAe//vINDw3nR+3gwvG+LjBPF8FxrJ1IAxAoGAHlOe\\nBXWcQ1HqFLvCcs4Vi8d+GvMzGMx1sBE0mblYGe2yQMtzJJ+mqu9L52SEqsGZT8bk\\nmKQBU2Y7uB4MqpEhjhA/RHfCrTV1I2pxTXP1WBjgNqqeP2ZiG7YjfdhwJMZhuOR0\\niVbLLcfQTA07BOVELt0oqPClZ4XfkIBEDZ2xRgECgYAgQNVDa38YTEmAQyNfmh7r\\nk0ttrUDnAUVbqtKsIqsu2YWdThyoVJvp1xuW6ggF2ckvB1tJgbCQEHMxbgAsSN2B\\nqkXWitO5ofcp/0XqPQZ/K9uOvcUJJeZD8GSKx9KNjqwLP45hS9Fne85STIsnI0gb\\nSoxx2UUbsFa33zcnLzS4jg==\\n-----END PRIVATE KEY-----\\n\",\n  \"client_email\": \"firebase-adminsdk-oevte@tutoring-online-e3711.iam.gserviceaccount.com\",\n  \"client_id\": \"106700587188435552093\",\n  \"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\",\n  \"token_uri\": \"https://oauth2.googleapis.com/token\",\n  \"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\",\n  \"client_x509_cert_url\": \"https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-oevte%40tutoring-online-e3711.iam.gserviceaccount.com\"\n}";
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
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<OptionsMiddleware>();

app.UseCors();
    
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
