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

            app.MapGet("/", () => $"Welcome to the {app.Environment.ApplicationName}!");

            app.MapGet("/config", async context =>
            {
                await context.Response.WriteAsync($"App name: {app.Environment.ApplicationName}. App running configuration: {app.Environment.EnvironmentName}");
            });

            app.Run();
        }
    }
}