namespace BufferGame
{
    public class Program
    {
        static void Main(string[] args)
        {
            var logger = new Logger("log.txt");
            var writter = new Writter();
            var buffer = new Buffer(logger);
            var historicalData = new HistoricalData(logger);
            var reader = new Reader(logger);
            
            //writter.OnDataSentBuffer += buffer.AddData;
            writter.OnDataSentBufferNew += buffer.AddDataNew;
            //writter.OnDataSentHistoricalData += historicalData.StoreDataFromWritter;
            writter.OnDataSentHistoricalDataNew += historicalData.StoreDataFromWritterNew;
            //buffer.OnBatchReady += historicalData.StoreDataFromBuffer;
            buffer.OnBatchReadyNew += historicalData.StoreDataFromBufferNew;
            historicalData.OnHistoricalDataReady += reader.ReadData;
            reader.OnHistoricalDataRequested += historicalData.GetDataValues;

            Console.WriteLine("enter to exit | 1-8 to read data from historical data by code | s to send data from writter directly to historical data\n");

            int userInput = 0;
            string input = Console.ReadLine();
            while (!string.IsNullOrWhiteSpace(input))
            {
                if (input.Equals("s"))
                {
                    writter.SendDataToHistoricalDataNew();
                }
                if (int.TryParse(input, out userInput))
                {
                    if (userInput >= 1 && userInput <= 8)
                    {
                        reader.RequestData(userInput);
                    }
                }
                input = Console.ReadLine();
            }
        }
    }
}
