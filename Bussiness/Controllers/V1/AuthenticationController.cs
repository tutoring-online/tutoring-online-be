using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Anotar.NLog;
using DataAccess.Entities.Student;
using DataAccess.Entities.Token;
using DataAccess.Models.Admin;
using DataAccess.Models.Authentication;
using DataAccess.Models.Student;
using DataAccess.Models.Tutor;
using DataAccess.Utils;
using FirebaseAdmin.Auth;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog.Fluent;
using tutoring_online_be.Controllers.Utils;
using tutoring_online_be.Security.Attribute;
using tutoring_online_be.Services;
using Claim = System.Security.Claims.Claim;

namespace tutoring_online_be.Controllers.V1;

[ApiController]
[Route("/api/v1/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly AppSetting appSetting;
    private IAuthenticationService authenticationService;
    private IAdminService adminService;
    private IStudentService studentService;
    private ITutorService tutorService;
    public AuthenticationController(
        IOptionsMonitor<AppSetting> optionsMonitor,
        IAuthenticationService authenticationService,
        IAdminService adminService,
        IStudentService studentService,
        ITutorService tutorService
        )
    {
        appSetting = optionsMonitor.CurrentValue;
        this.authenticationService = authenticationService;
        this.adminService = adminService;
        this.tutorService = tutorService;
        this.studentService = studentService;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public IActionResult Login(AuthenticationRequestModel model)
    {
        try
        {
            LogTo.Info($"\nReceived request with token : {model.Token}");
            LogTo.Info("\nCall Firebase to verify token");

            var decodedToken = FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(model.Token).Result;
            var uid = decodedToken.Uid;

            LogTo.Info($"\nReceived response with token id: {uid}");

            var currentUtcDate = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            if (decodedToken.ExpirationTimeSeconds < currentUtcDate)
            {
                LogTo.Info($"\nAccess Token: {model.Token} is expired");
                return Unauthorized(new AuthenticationResponseModel()
                {
                    ResultCode = (int?)ResultCode.AccessTokenExpired,
                    ResultMessage = ResultCode.AccessTokenExpired.ToString()
                });
            }

            var userRecord = FirebaseAuth.DefaultInstance.GetUserAsync(uid).Result;
            var email = userRecord.Email;
            var name = userRecord.DisplayName;
            var phoneNumber = userRecord.PhoneNumber;
            var avatarUrl = userRecord.PhotoUrl;
            var claims = userRecord.CustomClaims;
            LogTo.Info($"\nUser email : {email}" +
                       $"\nnName : {name}" +
                       $"\nPhoneNumber : {phoneNumber}" +
                       $"\nAvatarUrl : {avatarUrl}" +
                       $"\nUuid: {uid}");

            if (!claims.Any() || !claims.ContainsKey(Constants.role.ToString()))
            {
                //Case role not found
                LogTo.Info("User role not found - do find user in Admin or Tutor and update user role");
                AdminDto? adminDto = adminService.GetAdminByEmail(email);
                TutorDto? tutorDto = tutorService.GetTutorByEmail(email);

                if ((adminDto is null && tutorDto is null)
                    || (adminDto is not null && tutorDto is not null))
                {
                    return Unauthorized(
                        new AuthenticationResponseModel()
                        {
                            ResultCode = (int)ResultCode.InvalidUser,
                            ResultMessage = ResultCode.InvalidUser.ToString()
                        }
                    );
                }

                if (adminDto is not null)
                {
                    var newClaims = claims.ToDictionary(pair => pair.Key, pair => pair.Value);
                    newClaims.Add("role", Role.Admin.ToString().ToLower());
                    FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, newClaims);
                    AdminDto adminTmp = new AdminDto
                    {
                        Name = name,
                        Uid = uid,
                        AvatarURL = avatarUrl,
                        Phone = phoneNumber
                    };
                    adminService.UpdateAdmin(adminTmp.AsEntity(), adminDto.Id);
                    adminDto = adminService.GetAdminByEmail(email);

                    return Ok(
                        new AuthenticationResponseModel()
                        {
                            ResultCode = (int)ResultCode.Success,
                            ResultMessage = ResultCode.Success.ToString(),
                            Data = adminDto,
                            Role = Role.Admin.ToString().ToLower()
                        }
                    );
                }

                if (tutorDto is not null)
                {
                    var newClaims = claims.ToDictionary(pair => pair.Key, pair => pair.Value);
                    newClaims.Add("role", Role.Tutor.ToString().ToLower());
                    FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, newClaims);
                    TutorDto tutorTmp = new TutorDto
                    {
                        Name = name,
                        Uid = uid,
                        AvatarURL = avatarUrl,
                        Phone = phoneNumber
                    };
                    tutorService.UpdateTutor(tutorTmp.AsEntity(), tutorDto.Id);
                    tutorDto = tutorService.GetTutorByEmail(email);
                    
                    return Ok(
                        new AuthenticationResponseModel()
                        {
                            ResultCode = (int)ResultCode.Success,
                            ResultMessage = ResultCode.Success.ToString(),
                            Data = tutorDto,
                            Role = Role.Tutor.ToString().ToLower()
                        }
                    );
                }

            }
            else if (claims.Any() && claims.ContainsKey(Constants.role.ToString()))
            {
                //case have role -> return information
                var role = claims.GetValueOrDefault(Constants.role.ToString(), "").ToString();
                object? userData = null;
                switch (role)
                {
                    case "student":
                    {
                        var students = studentService.GetStudentByFirebaseUid(uid);
                        if (students.Any())
                        {
                            userData = students.ElementAt(0);
                        }
                        break;
                    }
                    case "tutor":
                    {
                        var tutors = tutorService.GetTutorByFirebaseUid(uid);
                        if (tutors.Any())
                        {
                            userData = tutors.ElementAt(0);
                        }
                        break;
                    }
                    case "admin":
                    {
                        var admins = adminService.GetAdminByFirebaseUid(uid);
                        if (admins.Any())
                        {
                            userData = admins.ElementAt(0);
                        }
                        break;
                    }
                }

                if (userData is null)
                {
                    return Unauthorized(
                        new AuthenticationResponseModel()
                        {
                            ResultCode = (int)ResultCode.UserRoleNotFound,
                            ResultMessage = ResultCode.UserRoleNotFound.ToString()
                        }
                    );
                }

                return Ok(
                    new AuthenticationResponseModel()
                    {
                        ResultCode = (int)ResultCode.Success,
                        ResultMessage = ResultCode.Success.ToString(),
                        Data = userData,
                        Role = role
                    }
                );
            }
        }
        catch (AggregateException e)
        {
            LogTo.Info(e.ToString);
            return Unauthorized(new AuthenticationResponseModel
            {
                ResultCode = (int?)ResultCode.InvalidToken,
                ResultMessage = ResultCode.InvalidToken.ToString(),
            });
        }
        
        return Unauthorized(
            new AuthenticationResponseModel()
            {
                ResultCode = (int)ResultCode.InvalidUser,
                ResultMessage = ResultCode.InvalidUser.ToString()
            }
        );    
    }

    [HttpPost]
    [Route("signup")]
    [AllowAnonymous]
    public IActionResult SignUp(AuthenticationRequestModel model)
    {
        try
        {
            LogTo.Info($"\nReceived request with token : {model.Token}");

            LogTo.Info("\nCall Firebase to verify token");
            var decodedToken = FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(model.Token).Result;
            var uid = decodedToken.Uid;
            LogTo.Info($"\nReceived response with token id: {uid}");

            var userRecord = FirebaseAuth.DefaultInstance.GetUserAsync(uid).Result;
            var email = userRecord.Email;
            var name = userRecord.DisplayName;
            var phoneNumber = userRecord.PhoneNumber;
            var avatarUrl = userRecord.PhotoUrl;
            var claims = userRecord.CustomClaims;
            LogTo.Info($"\nUser email : {email}" +
                       $"\nnName : {name}" +
                       $"\nPhoneNumber : {phoneNumber}" +
                       $"\nAvatarUrl : {avatarUrl}" +
                       $"\nUuid: {uid}");

            var students = studentService.GetStudentByFirebaseUid(uid);
            var tutors = tutorService.GetTutorByFirebaseUid(uid);
            var admins = adminService.GetAdminByFirebaseUid(uid);

            if (students.Any() || admins.Any() || tutors.Any())
            {
                return BadRequest(
                    new AuthenticationResponseModel
                    {
                        ResultCode = (int) ResultCode.UserAlreadySignup,
                        ResultMessage = ResultCode.UserAlreadySignup.ToString()
                    }
                );
            }

            var role = "student"; //default role

            int result = studentService.CreateStudentByFirebaseToken(decodedToken);
            StudentDto studentDto;
            if (result > 0)
            {
                var claimsTmp = new Dictionary<string, object>()
                {
                    { Constants.role.ToString(), Role.Student.ToString().ToLower() },
                };
                FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, claimsTmp);
                
                studentDto = studentService.GetStudentByFirebaseUid(uid).ElementAt(0);
            }
            else
            {
                LogTo.Error("Student signup error");
                return Ok(new AuthenticationResponseModel
                {
                    ResultCode = (int?)ResultCode.SystemError,
                    ResultMessage = ResultCode.SystemError.ToString(),
                });
            }

            return Ok(
                new AuthenticationResponseModel()
                {
                    ResultCode = (int)ResultCode.Success,
                    ResultMessage = ResultCode.Success.ToString(),
                    Data = studentDto,
                    Role = role
                });

        }
        catch (AggregateException e)
        {
            LogTo.Info(e.ToString);
            return Unauthorized(new AuthenticationResponseModel
            {
                ResultCode = (int?)ResultCode.InvalidToken,
                ResultMessage = ResultCode.InvalidToken.ToString(),
            });
        }
        
        return Unauthorized(
            new AuthenticationResponseModel()
            {
                ResultCode = (int)ResultCode.InvalidUser,
                ResultMessage = ResultCode.InvalidUser.ToString()
            }
        );    
    }
    

    // [HttpPost]
    // [AllowAnonymous]
    // public IActionResult GetAuthentication(AuthenticationRequestModel model)
    // {
    // 
    //     try
    //     {
    //         LogTo.Info($"\nReceived request with token : {model.Token}");
    //
    //         LogTo.Info("\nCall Firebase to verify token");
    //         var decodedToken = FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(model.Token).Result;
    //         var uid = decodedToken.Uid;
    //         LogTo.Info($"\nReceived response with token id: {uid}");
    //         
    //         var role = "student"; //default role
    //         var isSignUp = false;
    //         var currentUtcDate = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    //         if (decodedToken.ExpirationTimeSeconds < currentUtcDate)
    //         {
    //             LogTo.Info($"\nAccess Token: {model.Token} is expired");
    //             return Ok(new AuthenticationResponseModel()
    //             {
    //                 ResultCode = (int?)ResultCode.AccessTokenExpired,
    //                 ResultMessage = ResultCode.AccessTokenExpired.ToString()
    //             });
    //         }
    //
    //         //fetch user from db by uid
    //         var admins = adminService.GetAdminByFirebaseUid(uid);
    //         var tutors = tutorService.GetTutorByFirebaseUid(uid);
    //         var students = studentService.GetStudentByFirebaseUid(uid);
    //
    //         if (
    //             (admins.Any() && tutors.Any() && students.Any())
    //             || (admins.Any() && tutors.Any())
    //             || (admins.Any() && students.Any())
    //             || (tutors.Any() && students.Any())
    //         )
    //         {
    //             LogTo.Error($"\nTwo or more table contain user account : {uid} ");
    //             return Ok(new AuthenticationResponseModel
    //             {
    //                 ResultCode = (int?)ResultCode.SystemError,
    //                 ResultMessage = ResultCode.SystemError.ToString(),
    //             });
    //         }
    //
    //         object user = null;
    //         string type = AuthenticationType.login.ToString();
    //         var result = 0;
    //         if (!admins.Any() && !tutors.Any() && !students.Any())
    //         {
    //             LogTo.Info($"\nAccount not found : {uid} ");
    //
    //
    //             if (model.role is not null)
    //             {
    //                 role = model.role.ToLower();
    //             }
    //             LogTo.Info($"\nCreate new account with role {role}");
    //
    //             switch (role)
    //             {
    //                 case "student":
    //                 {
    //                     result = studentService.CreateStudentByFirebaseToken(decodedToken);
    //                     if (result > 0)
    //                     {
    //                         var claims = new Dictionary<string, object>()
    //                         {
    //                             { Constants.role.ToString(), Role.Student.ToString().ToLower() },
    //                         };
    //                         FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, claims);
    //                         
    //                         user = studentService.GetStudentByFirebaseUid(uid);
    //                         type = AuthenticationType.signup.ToString();
    //                     }
    //                     else
    //                     {
    //                         LogTo.Error("Student signup error");
    //                         return Ok(new AuthenticationResponseModel
    //                         {
    //                             ResultCode = (int?)ResultCode.SystemError,
    //                             ResultMessage = ResultCode.SystemError.ToString(),
    //                         });
    //                     }
    //                     break;
    //                 }
    //                 case "tutor":
    //                 {
    //                     result = tutorService.CreateTutorByFirebaseToken(decodedToken);
    //                     if (result > 0)
    //                     {
    //                         var claims = new Dictionary<string, object>()
    //                         {
    //                             { Constants.role.ToString(), Role.Tutor.ToString().ToLower() },
    //                         };
    //                         FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, claims);
    //                         
    //                         user = tutorService.GetTutorByFirebaseUid(uid);
    //                         type = AuthenticationType.signup.ToString();
    //                     }
    //                     else
    //                     {
    //                         LogTo.Error("Tutor signup error");
    //                         return Ok(new AuthenticationResponseModel
    //                         {
    //                             ResultCode = (int?)ResultCode.SystemError,
    //                             ResultMessage = ResultCode.SystemError.ToString(),
    //                         });
    //                     }
    //
    //                     break;
    //                 }
    //                 case "admin":
    //                 {
    //
    //                     result = adminService.CreateAdminByFirebaseToken(decodedToken);
    //                     if (result > 0)
    //                     {
    //                         var claims = new Dictionary<string, object>()
    //                         {
    //                             { Constants.role.ToString(), Role.Admin.ToString().ToLower() },
    //                         };
    //                         FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, claims);
    //                         
    //                         user = adminService.GetAdminByFirebaseUid(uid);
    //                         type = AuthenticationType.signup.ToString();
    //                     }
    //                     else
    //                     {
    //                         LogTo.Error("Admin signup error");
    //                         return Ok(new AuthenticationResponseModel
    //                         {
    //                             ResultCode = (int?)ResultCode.SystemError,
    //                             ResultMessage = ResultCode.SystemError.ToString(),
    //                         });
    //                     }
    //                     break;
    //                 }
    //                 default:
    //                 {
    //                     LogTo.Error($"\nUser Role not found : {uid} ");
    //                     return Ok(new AuthenticationResponseModel
    //                     {
    //                         ResultCode = (int?)ResultCode.UserRoleNotFound,
    //                         ResultMessage = ResultCode.UserRoleNotFound.ToString(),
    //                     });
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             object value;
    //             decodedToken.Claims.TryGetValue("role", out value);
    //
    //             role = (string) value;
    //             switch (role)
    //             {
    //                 case "admin":
    //                     user = adminService.GetAdminByFirebaseUid(uid);
    //                     break;
    //                 case "tutor":
    //                     user = tutorService.GetTutorByFirebaseUid(uid);
    //                     break;
    //                 case "student":
    //                     user = studentService.GetStudentByFirebaseUid(uid);
    //                     break;
    //             }
    //         }
    //         
    //
    //         return Ok(new AuthenticationResponseModel
    //         {
    //             ResultCode = (int?)ResultCode.Success,
    //             ResultMessage = ResultCode.Success.ToString(),
    //             Data = user,
    //             Type = type,
    //             Role = role
    //         });
    //     }
    //     catch (AggregateException e)
    //     {
    //         LogTo.Info(e.ToString);
    //         return Ok(new AuthenticationResponseModel
    //         {
    //             ResultCode = (int?)ResultCode.InvalidToken,
    //             ResultMessage = ResultCode.InvalidToken.ToString(),
    //         });
    //     }
    // }

    // [HttpPost]
    // [Route("renew-token")]
    // public IActionResult RenewToken(TokenModel model)
    // {
    //     var jwtTokenHandler = new JwtSecurityTokenHandler();
    //     var secretKeyBytes = Encoding.UTF8.GetBytes(appSetting.SecretKey);
    //     var tokenValidateParam = new TokenValidationParameters
    //     {
    //         ValidateIssuer = false,
    //         ValidateAudience = false,
    //
    //         ValidateIssuerSigningKey = true,
    //         IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
    //
    //         ClockSkew = TimeSpan.Zero,
    //
    //         ValidateLifetime = false
    //     };
    //
    //     try
    //     {
    //         var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);
    //
    //         if (validatedToken is JwtSecurityToken jwtSecurityToken)
    //         {
    //             var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
    //             
    //             if (!result)
    //             {
    //                 LogTo.Info($"\nAccess Token {model.AccessToken} is invalid");
    //                 return Ok(new AuthenticationResponseModel()
    //                 {
    //                     ResultCode = (int?)ResultCode.InvalidToken,
    //                     ResultMessage = ResultCode.InvalidToken.ToString()
    //                 });
    //             }
    //         }
    //
    //         var utcExpireDate =
    //             long.Parse(tokenInVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
    //         var currentUtcDate = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    //         if (utcExpireDate > currentUtcDate)
    //         {
    //             LogTo.Info($"\nAccess Token: {model.AccessToken} is not expired");
    //             return Ok(new AuthenticationResponseModel()
    //             {
    //                 ResultCode = (int?)ResultCode.AccessTokenNotExpired,
    //                 ResultMessage = ResultCode.AccessTokenNotExpired.ToString()
    //             });
    //         }
    //
    //         var storedToken = authenticationService.FindByToken(model.RefreshToken);
    //         if (storedToken is null)
    //         {
    //             LogTo.Info($"\nRefresh Token: {model.RefreshToken} is not exist.");
    //             return Ok(new AuthenticationResponseModel()
    //             {
    //                 ResultCode = (int?)ResultCode.RefreshTokenNotExist,
    //                 ResultMessage = ResultCode.RefreshTokenNotExist.ToString()
    //             });
    //         }
    //
    //         if (storedToken.IsUsed.Value)
    //         {
    //             LogTo.Info($"\nRefresh Token: {model.RefreshToken} is used.");
    //             return Ok(new AuthenticationResponseModel()
    //             {
    //                 ResultCode = (int?)ResultCode.RefreshTokenHasBeenUsed,
    //                 ResultMessage = ResultCode.RefreshTokenHasBeenUsed.ToString()
    //             });
    //         }
    //
    //
    //         if (storedToken.IsRevoked.Value)
    //         {
    //             LogTo.Info($"\nRefresh Token: {model.RefreshToken} is revoked.");
    //             return Ok(new AuthenticationResponseModel()
    //             {
    //                 ResultCode = (int?)ResultCode.RefreshTokenHasBeenRevoked,
    //                 ResultMessage = ResultCode.RefreshTokenHasBeenRevoked.ToString()
    //             });
    //         }
    //
    //         var accessTokenId = tokenInVerification.Claims
    //             .FirstOrDefault(token => token.Type.Equals(JwtRegisteredClaimNames.Jti)).Value;
    //
    //         if (!storedToken.JwtId.Equals(accessTokenId))
    //         {
    //             LogTo.Info($"\nToken: {model.RefreshToken}, {model.AccessToken} does not match.");
    //             return Ok(new AuthenticationResponseModel()
    //             {
    //                 ResultCode = (int?)ResultCode.TokenNotMatch,
    //                 ResultMessage = ResultCode.TokenNotMatch.ToString()
    //             });
    //         }
    //
    //         storedToken.IsUsed = true;
    //         storedToken.IsRevoked = true;
    //         
    //         authenticationService.UpdateToken(storedToken.AsEntity());
    //         
    //         var user = new AuthenticationModel()
    //         {
    //             Id = "1",
    //             Name = "abc",
    //             Email = "abc@gmail.com",
    //             Roles = new []
    //             {
    //                 "Admin"
    //             }
    //         };
    //         var token = GenerateToken(user);
    //         
    //         return Ok(new AuthenticationResponseModel
    //         {
    //             ResultCode = (int?)ResultCode.Success,
    //             ResultMessage = ResultCode.Success.ToString(),
    //             Data = token
    //         });
    //
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(new AuthenticationResponseModel
    //         {
    //             ResultCode = (int?)ResultCode.SystemError,
    //             ResultMessage = ResultCode.SystemError.ToString(),
    //         });
    //     }
    // }
    //
    // private TokenModel GenerateToken(AuthenticationModel model)
    // {
    //     var jwtTokenHandler = new JwtSecurityTokenHandler();
    //     var secretKeyBytes = Encoding.UTF8.GetBytes(appSetting.SecretKey);
    //
    //     var id = model.Id;
    //     var name = model.Name;
    //     var email = model.Email;
    //     var jwtId = Guid.NewGuid().ToString();
    //     var roles = model.Roles;
    //     var tokenExpires = appSetting.AccessTokenExpired;
    //
    //     LogTo.Info($"\nGenerated access token, refresh token for user: \n" +
    //                $"Id : {id} \n" +
    //                $"Name : {name} \n" +
    //                $"Email : {email} \n" +
    //                $"TokenId : {jwtId} \n" +
    //                $"Roles : {string.Join("," ,roles.ToList())} \n" +
    //                $"Token expired in {tokenExpires} minutes");
    //
    //     var claims = new List<Claim>();
    //     claims.Add(new Claim(ClaimTypes.Name, name));
    //     claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
    //     claims.Add(new Claim(JwtRegisteredClaimNames.Sub, email));
    //     claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jwtId));
    //     claims.Add(new Claim("Id", id));
    //
    //     foreach (var role in roles)
    //     {
    //         claims.Add(new Claim("Roles", role));
    //     }
    //     
    //     var expires = DateTime.UtcNow.AddMinutes(tokenExpires);
    //     var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),
    //         SecurityAlgorithms.HmacSha512Signature);
    //     var subject = new ClaimsIdentity();
    //     subject.AddClaims(claims);
    //
    //     var tokenDescription = new SecurityTokenDescriptor
    //     {
    //         Subject = subject,
    //         Expires = expires,
    //         SigningCredentials = signingCredentials
    //     };
    //
    //     var token = jwtTokenHandler.CreateToken(tokenDescription);
    //     var accessToken = jwtTokenHandler.WriteToken(token);
    //     var refreshToken = GenerateRefreshToken();
    //     
    //     LogTo.Info($"\nAccess Token: {accessToken}" +
    //                $"\nRefresh Token: {refreshToken}" +
    //                $"\nThen insert Refresh Token into DB with expired date is {appSetting.RefreshTokenExpired}");
    //     
    //     var refreshTokenEntity = new RefreshToken
    //     {
    //         JwtId = token.Id,
    //         UserId = model.Id,
    //         Token = refreshToken,
    //         IsUsed = false,
    //         IsRevoked = false,
    //         IssuedAt = DateTime.UtcNow,
    //         ExpiredAt = DateTime.UtcNow.AddDays(appSetting.RefreshTokenExpired),
    //         UserRole = string.Join("," ,roles.ToList())
    //     };
    //     
    //     authenticationService.InsertRefreshToken(refreshTokenEntity);
    //     
    //     return new TokenModel
    //     {
    //         AccessToken = accessToken,
    //         RefreshToken = refreshToken
    //     };
    // }
    //
    // private string GenerateRefreshToken()
    // {
    //     var random = new byte[32];
    //     using (var rng = RandomNumberGenerator.Create())
    //     {
    //         rng.GetBytes(random);
    //
    //         return Convert.ToBase64String(random);
    //     }
    // }
    //
    // private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
    // {
    //     var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    //     dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
    //
    //     return dateTimeInterval;
    // }
}