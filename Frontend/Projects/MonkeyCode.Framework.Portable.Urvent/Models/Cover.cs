using System.Globalization;

namespace MonkeyCode.Framework.Portable.Urvent.Models
{
    //ToDo add a default cover to the database that can be used if no cover image is available,
    // set primary key to "Default"?
    public class Cover : Entity
    {
        public Cover()
        {
            
        }

        /// <summary>
        /// ToDo Primary Key
        /// </summary>
        public string Url { get; set; }


        /// <summary>
        /// ToDo Not null, default value(no image available)
        /// </summary>
        public byte[] ThumbnailLarge { get; set; }

        /// <summary>
        /// ToDo Not null, default value(no image available)
        /// </summary>
        public byte[] ThumbnailMedium { get; set; }

        /// <summary>
        /// ToDo Not null, default value(no image available)
        /// </summary>
        public byte[] ThumbnailSmall { get; set; }
    }
}
