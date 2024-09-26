namespace BufferGame
{
    public class BufferProperty
    {
        public int BufferValue { get; set; }
        public GlobalData.Code Code { get; set; }

        public BufferProperty(GlobalData.Code code, int value) 
        {
            Code = code;
            BufferValue = value;
        }
    }
}
