using System.Collections.Generic;
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
                UpdateExistingBufferProperty(code, dataValue);
            }
            else if (DatasetExists(code))
            {
                CreateNewBufferPropertyInDataset(code, dataValue);
            }
            else
            {
                CreateNewCollectionDescription(code, dataValue);
            }
        }

        private void CreateNewCollectionDescription(Code code, int dataValue)
        {
            var collectionDescription = new CollectionDescription();
            if (DatasetCodes.TryGetValue(code, out int value)) 
            {
                collectionDescription.Dataset = value;
            } 
            else 
            {
                Logger.Log($"Code not valid\n");
                return; 
            }
            collectionDescription.ID = Guid.NewGuid();
            collectionDescription.BufferPropertyCollection.Add(new BufferProperty(code, dataValue));

            ValuesNew.Add(collectionDescription);
            Logger.Log($"Added value: {dataValue} , code: {code}");
        }

        private void CreateNewBufferPropertyInDataset(Code code, int dataValue)
        {
            foreach (var cd in ValuesNew)
            {
                if (DatasetCodes.TryGetValue(code, out var datasetCodeValue) && cd.Dataset == datasetCodeValue)
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

        private void UpdateExistingBufferProperty(Code code, int dataValue)
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
                if (DatasetCodes.TryGetValue(code, out var datasetCodeValue) && collectionDescription.Dataset == datasetCodeValue)
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
