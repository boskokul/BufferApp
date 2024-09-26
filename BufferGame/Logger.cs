namespace BufferGame
{
    public class Logger
    {
        private readonly string _filePath;

        public Logger(string logFilePath)
        {
            _filePath = logFilePath;
        }

        public void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message}\n");
            File.AppendAllText(_filePath, $"{DateTime.Now}: {message}\n");
        }
    }
}
