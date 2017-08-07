using System;
using System.Collections.Generic;


namespace MonkeyCode.Framework.Portable.Urvent.Models
{
    public class Event : Entity
    {
        public Event()
        {
            Categories = new List<Category>();

        }

        /// <summary>
        /// ToDo Primary key
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// ToDo Primary key
        /// </summary>
        public int EventTypeId { get; set; }

        public virtual EventType EventType { get; set; }

        /// <summary>
        /// ToDo Not null - must contain at least a fallback category?
        /// </summary>
        public virtual ICollection<Category> Categories { get; set; }

        /// <summary>
        /// ToDo Not null
        /// </summary>
        public virtual ICollection<UserEvent> UserEvents { get; set; }

        /// <summary>
        /// ToDo Not null - what happens with events that do not have a location (virtual meetings?)
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// ToDo Not null, set default cover 
        /// </summary>
        public Cover Cover { get; set; }

        /// <summary>
        /// ToDo Not null
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ToDo check howto set default values for ORMs which will be used if attribute is null
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ToDo Nullable
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// ToDo Not null
        /// </summary>
        public int AttendingCount { get; set; }

        /// <summary>
        /// Todo add validation: AttendingCountMale + AttendingCountFemale  >= AttendingCount
        ///ToDo Not null
        /// </summary>
        public int AttendingCountMale { get; set; }

        /// <summary>
        /// ToDo Not null
        /// </summary>
        public int AttendingCountFemale { get; set; }

        /// <summary>
        /// ToDo Not null
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// ToDo Not null
        /// </summary>
        public DateTime EndDateTime { get; set; }
        
        /// <summary>
        /// ToDo Not null
        /// </summary>
        public int Priority { get; set; }

        //ToDo Check if TimeZone will be necessary
        //http://stackoverflow.com/questions/246498/creating-a-datetime-in-a-specific-time-zone-in-c-sharp-fx-3-5
        //public TimeZoneInfo TimeZone { get; set; }

    }
}


///// <summary>
///// ToDo delete this namesapce and classes if services are adapted to new models
///// </summary>
//namespace MonkeyCode.Framework.Portable.Urvent.Models.EventbriteObsolete
//{
//    //[Table(nameof(HtmlText))]
//    public class HtmlText : EntityAutoPrimaryKey
//    {
//        //[PrimaryKey]
//        public string Text { get; set; }

//        //WTF: https://bitbucket.org/twincoders/sqlite-net-extensions/issues/91/sqlitenetsqliteexception-near-where-syntax
//        public string Foo { get; set; }
//    }

//    //[Table(nameof(DateTimeInfo))]
//    public class DateTimeInfo : EntityAutoPrimaryKey
//    {
//        //[PrimaryKey]
//        public DateTime Local { get; set; }

//        //WTF: https://bitbucket.org/twincoders/sqlite-net-extensions/issues/91/sqlitenetsqliteexception-near-where-syntax
//        public string Foo { get; set; }
//    }

//    //[Table(nameof(Logo))]
//    public class Logo : EntityAutoPrimaryKey
//    {
//        //[PrimaryKey]
//        public string Url { get; set; }

//        //WTF: https://bitbucket.org/twincoders/sqlite-net-extensions/issues/91/sqlitenetsqliteexception-near-where-syntax
//        public string Foo { get; set; }
//    }

//    //[Table(nameof(Address))]
//    public class Address : EntityAutoPrimaryKey
//    {
//        //Todo use converter for proper naming
//        //[PrimaryKey]
//        public string Address_1 { get; set; }
//        public string Address_2 { get; set; }
//        public string City { get; set; }
//        public string Region { get; set; }
//        public string Postal_Code { get; set; }
//        public string Country { get; set; }
//        public string Country_Name { get; set; }
//    }

//    //[Table(nameof(Venue))]
//    public class Venue : EntityId
//    {
//        public string Name { get; set; }
//        public string Longitude { get; set; }
//        public string Latitude { get; set; }

//        //[ForeignKey(typeof(Address))]     // Specify the foreign key
//        public int AddressId { get; set; }

//        //[OneToOne(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]
//        public Address Address { get; set; }
//    }

//    //[Table(nameof(Event))]
//    public class Event : EntityId
//    {
//        //[ForeignKey(typeof(HtmlText))]     // Specify the foreign key
//        public int NameId { get; set; }

//        //[OneToOne(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]
//        public HtmlText Name { get; set; }

//        public string Url { get; set; }


//        //[ForeignKey(typeof(DateTimeInfo))]     // Specify the foreign key
//        public int StartId { get; set; }

//        //[OneToOne(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]
//        public DateTimeInfo Start { get; set; }

//        //[ForeignKey(typeof(DateTimeInfo))]     // Specify the foreign key
//        public int EndId { get; set; }

//        //[OneToOne(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]
//        public DateTimeInfo End { get; set; }

//        //[ForeignKey(typeof(HtmlText))]     // Specify the foreign key
//        public int DescriptionId { get; set; }

//        //[OneToOne(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]
//        public HtmlText Description { get; set; }

//        //[ForeignKey(typeof(Logo))]     // Specify the foreign key
//        public int LogoId { get; set; }

//        //[OneToOne(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]
//        public Logo Logo { get; set; }
    
//        //[ForeignKey(typeof(Venue))]     // Specify the foreign key
//        public string VenueId { get; set; }

//        //[ManyToOne(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]
//        public Venue Venue { get; set; }

//    }


//    //[Table(nameof(UserEvent))]
//    public class UserEvent : Event
//    {
//        public bool IsSaved { get; set; }
//    }

//    public class EventSearchResult : Entity
//    {
//        public UserEvent[] Events { get; set; }
//    }
//}