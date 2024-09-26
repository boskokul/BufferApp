using static BufferGame.GlobalData;

namespace BufferGame
{
    public class Buffer
    {
        public List<int> Values { get; set; }
        public List<CollectionDescription> ValuesNew { get; set; }
        public Logger Logger { get; set; }
        public event Action<CollectionDescription> OnBatchReadyNew;

        public Buffer(Logger logger)
        {
            Values = new List<int>();
            ValuesNew = new List<CollectionDescription>();
            Logger = logger;
        }


        public void AddDataNew(Code code, int dataValue)
        {
            if (CodeExists(code))
            {
                foreach (var cd in ValuesNew)
                {
                    foreach (var bp in cd.BufferPropertyCollection)
                    {
                        if (bp.Code == code)
                        {
                            Logger.Log($"Added value: {dataValue} , code: {code}");
                            bp.BufferValue = dataValue;
                            return;
                        }
                    }
                }
            }
            if (DatasetExists(code))
            {
                foreach (var cd in ValuesNew)
                {
                    if (cd.Dataset == DatasetCodes[code])
                    {
                        Logger.Log($"Added value: {dataValue} , code: {code}");
                        cd.BufferPropertyCollection.Add(new BufferProperty(code, dataValue));
                        if (cd.BufferPropertyCollection.Count == 2) 
                        {
                            SendToHistoricalData(cd);
                            cd.BufferPropertyCollection.Clear();
                        }
                        return;
                    }
                }
            }

            var collectionDescription = new CollectionDescription();
            collectionDescription.Dataset = DatasetCodes[code];
            collectionDescription.ID = Guid.NewGuid();
            collectionDescription.BufferPropertyCollection.Add(new BufferProperty(code, dataValue));

            ValuesNew.Add(collectionDescription);
            Logger.Log($"Added value: {dataValue} , code: {code}");
        }

        public bool CodeExists(Code code)
        {
            foreach(var collectionDescription in ValuesNew) 
            {
                foreach(var bufferProperty in collectionDescription.BufferPropertyCollection)
                {
                    if(bufferProperty.Code == code)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool DatasetExists(Code code)
        {
            foreach (var collectionDescription in ValuesNew)
            {
                if(collectionDescription.Dataset == DatasetCodes[code])
                {
                    return true;
                }
            }
            return false;
        }


        public void SendToHistoricalData(CollectionDescription collectionDescription)
        {
            Logger.Log($"Sending to historical data -> \n ID \t\t\t\t Dataset |  code \t|  value \t|  code \t|  value \n {collectionDescription.ID} {collectionDescription.Dataset} | {collectionDescription.BufferPropertyCollection[0].Code} | {collectionDescription.BufferPropertyCollection[0].BufferValue} \t|{collectionDescription.BufferPropertyCollection[1].Code} | {collectionDescription.BufferPropertyCollection[1].BufferValue}");
            OnBatchReadyNew?.Invoke(collectionDescription);
        }
    }
}
