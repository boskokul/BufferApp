namespace BufferGame
{
    public class HistoricalProperty
    {
        public int HistoricalValue { get; set; }
        public GlobalData.Code Code { get; set; }

        public HistoricalProperty(GlobalData.Code code, int value)
        {
            Code = code;
            HistoricalValue = value;
        }
    }
}
