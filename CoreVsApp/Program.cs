using CoreVsApp.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.CompilerServices;

namespace CoreVsApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            if (app.Environment.IsStaging() || app.Environment.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            // Поддержка статических файлов
            app.UseStaticFiles();

            //Добавляем компонент для логирования запросов с использованием метода Use.
            app.UseMiddleware<LoggingMiddleware>(app);

            app.MapGet("/", () => $"Welcome to the {app.Environment.ApplicationName}!");
                        
            app.Map("/config", Config);

            app.Map("/about", About);

            app.Run();
        }

        /// <summary>
        ///  Обработчик для страницы Config
        /// </summary>
        private static void Config(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("App running configuration");
            });
        }

        /// <summary>
        ///  Обработчик для страницы About
        /// </summary>
        private static void About(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("ASP.Net Core tutorial project");
            });
        }
    }
}