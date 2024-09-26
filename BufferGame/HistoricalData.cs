using static BufferGame.GlobalData;

namespace BufferGame
{
    public class HistoricalData
    {
        public Logger Logger { get; set; }
        public List<int> Values { get; set; }
        public List<Description> ListDescription { get; set; }
        public event Action<List<HistoricalProperty>> OnHistoricalDataReady;

        public HistoricalData(Logger logger)
        {
            Logger = logger;
            Values = new List<int>();
            ListDescription = new List<Description>();
        }


        public void StoreDataFromBufferNew(CollectionDescription collectionDescription)
        {
            // provera da li je odgovarajuc dataset sa kodovima
            if (ValidateData(collectionDescription) == false)
            {
                Logger.Log($"Dataset and code not valid\n");
                return;
            }
            foreach (var description in ListDescription)
            {
                // ako vec postoji ovaj dataset
                if(description.Dataset == collectionDescription.Dataset)
                {
                    var descProps = description.HistoricalProperties.ToArray();
                    var buffProps = collectionDescription.BufferPropertyCollection.ToArray();

                    // provera novih i istorijski vec upisanih da bi se proverilo da li podatak postoji i da li je van deadbound-a
                    foreach (var buffP in buffProps)
                    {
                        var dP = descProps.LastOrDefault(x => x.Code == buffP.Code);
                        if (dP != null)
                        {
                            // provera da li upisati novopristigle vrednosti
                            if (((dP.Code == buffP.Code) && (Math.Abs(buffP.BufferValue - dP.HistoricalValue) >= 0.03 * dP.HistoricalValue)) || buffP.Code == GlobalData.Code.CODE_DIGITAL)
                            {
                                // ovde cuvati u bazu
                                Logger.Log($"Storing historical data -> \n ID \t\t\t\t Dataset |  code \t|  value \n {description.ID} {description.Dataset} | {buffP.Code} | {buffP.BufferValue}");
                                description.HistoricalProperties.Add(new HistoricalProperty(buffP.Code, buffP.BufferValue));
                            }
                        }
                        else
                        {
                            // ovde cuvati u bazu, ovo je slucaj kad se direktno upisao jedan u historical data i onda drugi kod nece imati s cim da poredi svoju staru vrednost
                            Logger.Log($"Storing historical data -> \n ID \t\t\t\t Dataset |  code \t|  value \n {description.ID} {description.Dataset} | {buffP.Code} | {buffP.BufferValue}");
                            description.HistoricalProperties.Add(new HistoricalProperty(buffP.Code, buffP.BufferValue));
                        }
                    }
                    
                    return;
                }
            }
            // prvi put nailazi ovaj dataset
            Description desc = new Description();
            desc.ID = Guid.NewGuid();
            desc.Dataset = collectionDescription.Dataset;
            Logger.Log($"Storing historical data -> \n ID \t\t\t\t Dataset |  code \t|  value \t|  code \t|  value \n {desc.ID} {desc.Dataset} | {collectionDescription.BufferPropertyCollection[0].Code} | {collectionDescription.BufferPropertyCollection[0].BufferValue} \t|{collectionDescription.BufferPropertyCollection[1].Code} | {collectionDescription.BufferPropertyCollection[1].BufferValue}");
            foreach (var bp in collectionDescription.BufferPropertyCollection)
            {
                // ovde cuvati u bazu
                desc.HistoricalProperties.Add(new HistoricalProperty(bp.Code, bp.BufferValue));
            }
            ListDescription.Add(desc);
        }

        public bool ValidateData(CollectionDescription collectionDescription) 
        {
            foreach(var bf in collectionDescription.BufferPropertyCollection)
            {
                if (GlobalData.DatasetCodes[bf.Code] != collectionDescription.Dataset)
                {
                    return false;
                }
            }
            return true;
        }


        public void StoreDataFromWritterNew(Code code, int dataValue)
        {
            // sad u bazu upisati value
            if (DatasetExists(code))
            {
                foreach (var description in ListDescription)
                {
                    if (description.Dataset == DatasetCodes[code])
                    {
                        var dP = description.HistoricalProperties.LastOrDefault(x => x.Code == code);
                        if (dP != null)
                        {
                            // provera da li upisati novopristiglu vrednost
                            if (((dP.Code == code) && (Math.Abs(dataValue - dP.HistoricalValue) >= 0.03 * dP.HistoricalValue)) || code == GlobalData.Code.CODE_DIGITAL)
                            {
                                // ovde cuvati u bazu
                                Logger.Log($"Storing historical data -> \n ID \t\t\t\t Dataset |  code \t|  value \n {description.ID} {description.Dataset} | {code} | {dataValue}");
                                description.HistoricalProperties.Add(new HistoricalProperty(code, dataValue));
                            }
                        }
                        return;
                    }
                }
            }

            // prvi put nailazi ovaj dataset
            Description desc = new Description();
            desc.ID = Guid.NewGuid();
            desc.Dataset = DatasetCodes[code];
            Logger.Log($"Storing historical data -> \n ID \t\t\t\t Dataset |  code \t|  value \t \n {desc.ID} {desc.Dataset} | {code} | {dataValue}");
            
            // ovde cuvati u bazu
            desc.HistoricalProperties.Add(new HistoricalProperty(code, dataValue));
            ListDescription.Add(desc);
        }

        public bool DatasetExists(Code code)
        {
            foreach (var description in ListDescription)
            {
                if (description.Dataset == DatasetCodes[code])
                {
                    return true;
                }
            }
            return false;
        }

        public void GetDataValues(int codeValue) 
        {
            var dataset = DatasetCodes[(Code)(codeValue - 1)];
            var description = ListDescription.Find(x => x.Dataset == dataset);
            if (description != null)
            {
                var res = description.HistoricalProperties.FindAll(x => x.Code == (Code)(codeValue-1));
                
                OnHistoricalDataReady?.Invoke(res);
            }
        }
    }
}
