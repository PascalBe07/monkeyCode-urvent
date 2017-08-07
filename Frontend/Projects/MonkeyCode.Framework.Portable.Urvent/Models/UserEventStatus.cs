using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode.Framework.Portable.Urvent.Models
{
    public class UserEventStatus : Entity
    {
        public UserEventStatus()
        {
            
        }

        /// <summary>
        /// ToDo Primary key
        /// </summary>
        public int Id { get; set; }
        

        /// <summary>
        /// ToDo Not Null
        /// Values: Unknown/Declined/Accepted;
        /// </summary>
        public string Status { get; set; }

    }
}
