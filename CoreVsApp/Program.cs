using Microsoft.AspNetCore.Builder;
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

            //Добавляем компонент для логирования запросов с использованием метода Use.
            app.Use(async (context, next) =>
            {
                // Для логирования данных о запросе используем свойства объекта HttpContext
                Console.WriteLine($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
                await next.Invoke();
            });

            app.MapGet("/", () => $"Welcome to the {app.Environment.ApplicationName}!");

            app.Use(async (context, next) =>
            {
                // Строка для публикации в лог
                string logMessage = $"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}{Environment.NewLine}";

                // Путь до лога (опять-таки, используем свойства IWebHostEnvironment)
                string logFilePath = Path.Combine(app.Environment.ContentRootPath, "Logs", "RequestLog.txt");

                // Используем асинхронную запись в файл
                await File.AppendAllTextAsync(logFilePath, logMessage);

                await next.Invoke();
            });

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