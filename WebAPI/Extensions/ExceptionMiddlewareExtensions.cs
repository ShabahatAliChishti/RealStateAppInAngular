using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using WebAPI.Middlewares;

namespace WebAPI.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app,
        IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleWare>();
        }
        public static void ConfigureBuiltinExceptionHandler(this IApplicationBuilder app,
       IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                //builtin middleware for exception handling

                app.UseExceptionHandler(
                    options =>
                    {
                        options.Run(
                            async context =>
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                var ex = context.Features.Get<IExceptionHandlerFeature>();

                                if (ex != null)
                                {
                                    await context.Response.WriteAsync(ex.Error.Message);
                                }
                            }
                            );


                    });
            }
        }
    }
}
