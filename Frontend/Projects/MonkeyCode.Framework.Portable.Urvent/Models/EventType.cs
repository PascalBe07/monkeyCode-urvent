namespace MonkeyCode.Framework.Portable.Urvent.Models
{
    public class EventType : Entity
    {
        public EventType()
        {
            
        }

        /// <summary>
        /// ToDo Primary key
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// ToDo Not Null
        /// Values: Facebook, Eventbrite
        /// </summary>
        public string Type { get; set; }

    }
}
