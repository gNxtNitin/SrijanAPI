using AuthLibrary;
using AuthLibrary.Interface;
//using DatabaseManager;
using CustomerManagementLibrary;
using DatabaseManager;
using EmailService.Library;
using ErrorAndExceptionHandling.Library;
using MenuManagementLib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MobilePortalManagementLibrary.Implementation;
using MobilePortalManagementLibrary.Interface;
using NotificationPreferenceLib;
using NotificationPreferenceLib.Interface;
using NotificationSenderLib;
using PasswordManagementLibrary;
using RoleManagementLibrary;
using Services.Implementation;
using Services.Interfaces;
using SRIJANWEBAPI;
using SRIJANWEBAPI.Models;
using UserManagementLibrary;
using UserManagementLibrary.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["jwt:Issuer"],
        ValidAudience = builder.Configuration["jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"]))
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

DALOR.dbConnStr = builder.Configuration.GetConnectionString("DefaultConnection");
ApiAuditSettings.EnableAudit = builder.Configuration.GetValue<bool>("ApiAudit:EnableAudit");
//builder.Services.AddScoped<IAuthorizationHandler, FeatureAccessRequirementHandler>();

builder.Services.AddScoped<IUserAuthService, UserAuthService>();//Services
builder.Services.AddScoped<IAuthService, AuthService>();//AuthLibrary
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordManagementService, PasswordManagementService>();
builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
builder.Services.AddScoped<IUserManagerService, UserManagerService>();
builder.Services.AddScoped<IEmailSerivce, EmailSerivce>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IMenuManagementService, MenuManagementService>();
builder.Services.AddScoped<IRolesManagementService, RolesManagementService>();
builder.Services.AddScoped<IRoleManagerService, RoleManagerService>();

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationPreferenceService, NotifcationPreferenceService>();
builder.Services.AddScoped<INotificationSenderService, NotificationSenderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerManagementService, CustomerManagementService>();
builder.Services.AddScoped<IAdminManagementService,AdminManagementService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IDAManagementService, DAManagementService>();
builder.Services.AddScoped<IVisitsManagementService, VisitsManagementService>();
builder.Services.AddScoped<IPunchingManagementService, PunchingManagementService>();
builder.Services.AddScoped<IDAService, DAService>();
builder.Services.AddScoped<IVisitService, VisitService>();
builder.Services.AddScoped<IPunchingService, PunchingService>();
builder.Services.AddScoped<IApiAuditManagement, ApiAuditManagement>();
builder.Services.AddScoped<IApiAuditService, ApiAuditService>();


builder.Services.AddScoped<IErrorLoggingService>(sp =>
    new ErrorLoggingService("logs/error.log"));

builder.Services.Configure<FileSettings>(
    builder.Configuration.GetSection("FileSettings"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
