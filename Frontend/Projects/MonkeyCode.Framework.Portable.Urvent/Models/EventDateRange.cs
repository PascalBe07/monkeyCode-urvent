using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode.Framework.Portable.Urvent.Models
{
    public class EventDateRange
    {
        public EventDateRange()
        {
            
        }

        /// <summary>
        /// ToDo Primary key
        /// Values: Today/Week/Weekend/All
        /// </summary>
        public int Id { get; set; }

        public string Description { get; set; }
    }
}
