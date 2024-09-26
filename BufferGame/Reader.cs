namespace BufferGame
{
    public class Reader
    {
        public Logger Logger {  get; set; }
        public event Action<int> OnHistoricalDataRequested;

        public Reader(Logger logger)
        {
            Logger = logger;
        }

        public void ReadData(List<HistoricalProperty> dataValues) 
        {
            foreach (var value in dataValues)
            {
                Logger.Log($"Read\n \t code: {value.Code} \t value: {value.HistoricalValue}");
            }
        }

        public void RequestData(int codeValue)
        {
            OnHistoricalDataRequested?.Invoke(codeValue);
        }
    }
}
