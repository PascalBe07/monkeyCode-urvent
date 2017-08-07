using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace MonkeyCode.Framework.Portable.Urvent.Models.Mobile
{
    [Table(nameof(Category))]
    public class MobileCategory : MobileEntity
    {
        //ToDo AutoIncrement is only temporary until own categories are defined
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey(typeof(MobileEvent))]
        public string EventId { get; set; }

        [ForeignKey(typeof(MobileUser))]
        public string UserGuid { get; set; }
    }
}
