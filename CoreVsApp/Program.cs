namespace CoreVsApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            if (app.Environment.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}