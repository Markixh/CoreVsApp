namespace CoreVsApp.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebApplication _app;

        /// <summary>
        ///  Middleware-компонент должен иметь конструктор, принимающий RequestDelegate
        /// </summary>
        public LoggingMiddleware(RequestDelegate next, WebApplication app)
        {
            _next = next;
            _app = app;
        }

        /// <summary>
        ///  Записываем Лог в консоль
        /// </summary>
        public void LogConsole(HttpContext context) 
        {
            // Строка для публикации в лог
            string logMessage = $"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}";

            // Для логирования данных о запросе используем свойста объекта HttpContext
            Console.WriteLine(logMessage);
        }

        /// <summary>
        ///  Записываем Лог в файл
        /// </summary>
        public async Task LogFileAsync(HttpContext context)
        {
            // Строка для публикации в лог
            string logMessage = $"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}";

            // Путь до лога (опять-таки, используем свойства IWebHostEnvironment)
            string logFilePath = Path.Combine(_app.Environment.ContentRootPath, "Logs", "RequestLog.txt");

            // Используем асинхронную запись в файл
            await File.AppendAllTextAsync(logFilePath, logMessage + Environment.NewLine);
        }

        /// <summary>
        ///  Необходимо реализовать метод Invoke  или InvokeAsync
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {            
            LogConsole(context);
            await LogFileAsync(context);

            // Передача запроса далее по конвейеру
            await _next.Invoke(context);
        }
    }
}
