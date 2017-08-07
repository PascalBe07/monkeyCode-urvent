using System;
using System.Collections.Generic;

namespace MonkeyCode.Framework.Portable.Urvent.Models
{
    public class User : Entity
    {
        public User()
        {

        }

        /// <summary>
        /// ToDo Primary key
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// Access token for Urvent API
        /// ToDo Nullable (because created on server)
        /// </summary>
        public virtual UrventUserData UrventUserData { get; set; }

        /// <summary>
        /// ToDo Nullable, maybe age range might be better?
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// ToDo Not null, default value
        /// </summary>
        public DateTime LastLogin { get; set; }


        /// <summary>
        /// ToDo Not null, default value unknown
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// ToDo Nullable until Facebook login is implemented
        /// </summary>
        public virtual FacebookUserData FacebookUserData { get; set; }

        /// <summary>
        /// ToDo Nullable
        /// </summary>
        public virtual ICollection<UserEvent> UserEvents { get; set; }
    }
}
