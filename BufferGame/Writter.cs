using System.Timers;

namespace BufferGame
{
    public class Writter
    {
        public System.Timers.Timer Timer {  get; set; }
        public event Action<int> OnDataSentBuffer;
        public event Action<GlobalData.Code, int> OnDataSentBufferNew;
        public event Action<int> OnDataSentHistoricalData;
        public event Action<GlobalData.Code, int> OnDataSentHistoricalDataNew;

        public Writter()
        {
            Timer = new System.Timers.Timer(3000);
            Timer.Elapsed += SendDataToBufferNew;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }

        private void SendDataToBuffer(object sender, ElapsedEventArgs e)
        {
            var value = new Random().Next(1, 100);
            OnDataSentBuffer?.Invoke(value);
        }

        private void SendDataToBufferNew(object sender, ElapsedEventArgs e)
        {
            var value = new Random().Next(1, 100);
            var code = new Random().Next(0, 8);
            var allCodes = Enum.GetValues(typeof(GlobalData.Code)).Cast<GlobalData.Code>().ToArray();
            OnDataSentBufferNew?.Invoke(allCodes[code], value);
        }

        public void SendDataToHistoricalData()
        {
            var value = new Random().Next(1, 100);
            OnDataSentHistoricalData?.Invoke(value);
        }

        public void SendDataToHistoricalDataNew()
        {
            var value = new Random().Next(1, 100);
            var code = new Random().Next(0, 8);
            var allCodes = Enum.GetValues(typeof(GlobalData.Code)).Cast<GlobalData.Code>().ToArray();
            OnDataSentHistoricalDataNew?.Invoke(allCodes[code], value);
        }
    }
}
