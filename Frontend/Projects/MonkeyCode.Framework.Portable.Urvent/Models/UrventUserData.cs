using System;

namespace MonkeyCode.Framework.Portable.Urvent.Models
{
    public class UrventUserData : Entity
    {
        public UrventUserData()
        {

        }


        /// <summary>
        /// ToDo Primary key 
        /// </summary>
        public string Guid { get; set; }


        /// <summary>
        /// ToDo Nullable (created on server)
        /// </summary>
        public string AccessToken { get; set; }


        /// <summary>
        /// ToDo Nullable (created on server)
        /// </summary>
        public DateTime AccessTokenReceived { get; set; }



    }
}
