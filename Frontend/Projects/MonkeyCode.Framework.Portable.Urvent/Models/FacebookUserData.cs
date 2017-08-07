using System;

namespace MonkeyCode.Framework.Portable.Urvent.Models
{
    public class FacebookUserData : Entity
    {
        public FacebookUserData()
        {

        }

        /// <summary>
        /// ToDo Primary key
        /// </summary>
        public string AccessToken { get; set; }


        /// <summary>
        /// ToDo Not Null
        /// </summary>
        public DateTime AccessTokenReceived { get; set; }

    }
}
