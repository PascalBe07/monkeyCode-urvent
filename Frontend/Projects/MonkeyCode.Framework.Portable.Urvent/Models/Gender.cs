namespace MonkeyCode.Framework.Portable.Urvent.Models
{
    public class Gender : Entity
    {
        public Gender()
        {
            
        }

        /// <summary>
        /// ToDo Primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ToDo Not null, values: unknown/male/female
        /// </summary>
        public string Type { get; set; }
    }
}
