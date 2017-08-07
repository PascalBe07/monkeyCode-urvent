using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode.Framework.Portable.Urvent.Models
{
    public class Setting : Entity
    {
        public Setting()
        {

        }

        /// <summary>
        /// ToDo Primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ToDo Not null, default value = 0
        /// </summary>
        public double MaxDistance { get; set; }

        /// <summary>
        /// ToDo Nullable
        /// </summary>
        public virtual ICollection<Category> ExcludedCategories { get; set; }

        /// <summary>
        /// ToDo Not null, default complementary gender
        /// </summary>
        public virtual Gender AttractedTo { get; set; }

        /// <summary>
        /// ToDo Nullable
        /// </summary>
        public virtual EventDateRange EventDateRange { get; set; }

        //ToDo Check if TimeZone will be necessary
        //http://stackoverflow.com/questions/246498/creating-a-datetime-in-a-specific-time-zone-in-c-sharp-fx-3-5
        //public TimeZoneInfo TimeZone { get; set; }

    }
}
