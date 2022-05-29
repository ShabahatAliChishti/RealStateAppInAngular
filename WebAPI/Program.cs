using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using WebAPI.Data;
using WebAPI.Extensions;
using WebAPI.Helpers;
using WebAPI.Interfaces;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);
////change environment variable prefix when using shared server
//builder.Host.ConfigureHostConfiguration(configHost=>
//{
//    configHost.AddEnvironmentVariables(prefix: "HSPA_");
//}
//);
//for creating password in environment variable and use it here for connection string

//var connectionstringbuilder = new SqlConnectionStringBuilder(
//    builder.Configuration.GetConnectionString("Default"));
////DB PASSWORD is a key of password stored in environment variable
//connectionstringbuilder.Password = builder.Configuration.GetSection("DBPassword").Value;
////now connectionstring is ready
//var connectionString = connectionstringbuilder.ConnectionString;

    


var secretKey = builder.Configuration.GetSection("AppSettings:Key").Value;
var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//we can replace value of usesqlserver with connectionString
builder.Services.AddDbContext<DataContext >(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = key
    };
});
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//else
//{
//    //builtin middleware for exception handling

//    app.UseExceptionHandler(
//        options =>
//        {
//            options.Run(
//                async context =>
//                {
//                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                    var ex = context.Features.Get<IExceptionHandlerFeature>();

//                    if (ex != null)
//                    {
//                        await context.Response.WriteAsync(ex.Error.Message);
//                    }
//                }
//                );


//        });
//        }

//inbuilt middleware
//app.ConfigureExceptionHandler(app.Environment);
//app.ConfigureBuiltinExceptionHandler(app.Environment);

//custom middleware
//app.UseMiddleware<ExceptionMiddleWare>();

app.ConfigureExceptionHandler(app.Environment);

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
