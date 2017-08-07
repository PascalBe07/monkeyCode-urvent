namespace MonkeyCode.Framework.Portable.Urvent.Models
{
    public class Location : Entity
    {
        public Location()
        {

        }

        /// <summary>
        /// ToDo Primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ToDo What happens if longitude/latitude are the same but another attribute is not the same? 
        /// Do we might need another primary key? But how should it be created
        /// Todo Primary key
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// Todo Primary key
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// ToDo Nullable
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// ToDo Nullable
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// ToDo Nullable
        /// </summary>
        public string ZipCode { get; set; }
    }
}
