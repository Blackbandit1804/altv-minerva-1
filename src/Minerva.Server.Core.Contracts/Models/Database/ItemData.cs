namespace Minerva.Server.Core.Contracts.Models.Database
{
    public class ItemData
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}