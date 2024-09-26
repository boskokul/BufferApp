

namespace BufferGame
{
    public static class GlobalData
    {
        public enum Code { CODE_ANALOG, CODE_DIGITAL, CODE_CUSTOM, CODE_LIMITSET, CODE_SINGLENODE, CODE_MILTIPLENODE, CODE_PRODUCER, CODE_DESTINATION }

        public static Dictionary<Code, int> DatasetCodes = new Dictionary<Code, int>();

        static GlobalData()
        {
            DatasetCodes[Code.CODE_ANALOG] = 1;
            DatasetCodes[Code.CODE_DIGITAL] = 1;
            DatasetCodes[Code.CODE_CUSTOM] = 2;
            DatasetCodes[Code.CODE_LIMITSET] = 2;
            DatasetCodes[Code.CODE_SINGLENODE] = 3;
            DatasetCodes[Code.CODE_MILTIPLENODE] = 3;
            DatasetCodes[Code.CODE_PRODUCER] = 4;
            DatasetCodes[Code.CODE_DESTINATION] = 4;

        }
    }
}
