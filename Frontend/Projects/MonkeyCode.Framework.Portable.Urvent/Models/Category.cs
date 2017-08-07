using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode.Framework.Portable.Urvent.Models
{
    public class Category : Entity
    {
        public Category()
        {
            
        }

        /// <summary>
        /// ToDo Primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ToDo Not null
        /// ToDo How do define our categories? Furthermore we need a mapping mechanism as well if we only allow a subset of all possible categories in other APIs.
        /// </summary>
        public string Name { get; set; }
    }
}
