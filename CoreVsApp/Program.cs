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

            //��������� ��������� ��� ����������� �������� � �������������� ������ Use.
            app.Use(async (context, next) =>
            {
                // ��� ����������� ������ � ������� ���������� �������� ������� HttpContext
                Console.WriteLine($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
                await next.Invoke();
            });

            app.MapGet("/", () => $"Welcome to the {app.Environment.ApplicationName}!");

            app.Use(async (context, next) =>
            {
                // ������ ��� ���������� � ���
                string logMessage = $"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}{Environment.NewLine}";

                // ���� �� ���� (�����-����, ���������� �������� IWebHostEnvironment)
                string logFilePath = Path.Combine(app.Environment.ContentRootPath, "Logs", "RequestLog.txt");

                // ���������� ����������� ������ � ����
                await File.AppendAllTextAsync(logFilePath, logMessage);

                await next.Invoke();
            });

            app.Map("/config", Config);

            app.Map("/about", About);

            app.Run();
        }

        /// <summary>
        ///  ���������� ��� �������� Config
        /// </summary>
        private static void Config(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("App running configuration");
            });
        }

        /// <summary>
        ///  ���������� ��� �������� About
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