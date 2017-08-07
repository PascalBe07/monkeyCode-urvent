using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MonkeyCode.Framework.Portable.Urvent.Models
{

    /* ToDo check how many-to-many relationships work using an own entity
    //http://stackoverflow.com/questions/19342908/how-to-create-a-many-to-many-mapping-in-entity-framework
    // releationship context has to be set manually:
    //  
    
       modelBuilder.Entity<UserEvent>()
           .HasKey(c => new { c.EventId, c.EventType, c.UserEMail });

       modelBuilder.Entity<User>()
           .HasMany(c => c.UserEvents)
           .WithRequired()
           .HasForeignKey(c => c.UserEMail);

       modelBuilder.Entity<Event>()
           .HasMany(c => c.UserEvents)
           .WithRequired()
           .HasForeignKey(c => new { c.EventId, c.EventType });
    */

    public class UserEvent : Entity
    {


        //Primary key from User and Event entity:
        public string UserEMail { get; set; }

        public string EventId { get; set; }
        public int EventTypeId { get; set; }

        public virtual EventType EventType { get; set; }

        /// <summary>
        /// ToDo Nullable
        /// </summary>
        public virtual ICollection<User> Users { get; set; }

        /// <summary>
        /// ToDo Nullable
        /// </summary>
        public virtual ICollection<Event> Events { get; set; }

        /// <summary>
        /// ToDo Nullable (what happens in instance with nullable data types?)
        /// </summary>
        public uint Rating { get; set; }

        /// <summary>
        /// ToDo Not null, default values "Unknown"
        /// </summary>
        public virtual UserEventStatus Status { get; set; }
    }
}
