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
            
            writter.OnDataSentBufferNew += buffer.AddDataNew;
            writter.OnDataSentHistoricalDataNew += historicalData.StoreDataFromWritterNew;
            buffer.OnBatchReadyNew += historicalData.StoreDataFromBufferNew;
            historicalData.OnHistoricalDataReady += reader.ReadData;
            reader.OnHistoricalDataRequested += historicalData.GetDataValues;

            Console.WriteLine("enter to exit | 1-8 to read data from historical data by code | s to send data from writter directly to historical data\n");

            string input;
            do
            {
                input = Console.ReadLine();
                if (input.Equals("s"))
                {
                    writter.SendDataToHistoricalDataNew();
                }
                else if (int.TryParse(input, out int userInput) && userInput >= 1 && userInput <= 8)
                {
                    reader.RequestData(userInput);
                }
            } while (!string.IsNullOrWhiteSpace(input));
        }
    }
}
