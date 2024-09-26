using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BufferGame
{
    public class Description
    {
        public Guid ID { get; set; }
        public int Dataset { get; set; }
        public List<HistoricalProperty> HistoricalProperties { get; set; }

        public Description()
        {
            HistoricalProperties = new List<HistoricalProperty>();

        }
    }
}
