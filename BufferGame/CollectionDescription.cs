namespace BufferGame
{
    public class CollectionDescription
    {
        public Guid ID { get; set; }
        public int Dataset { get; set; }
        public List<BufferProperty> BufferPropertyCollection { get; set; }

        public CollectionDescription()
        {
            BufferPropertyCollection = new List<BufferProperty>();
            
        }
    }
}
