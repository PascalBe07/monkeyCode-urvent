using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace MonkeyCode.Framework.Portable.Urvent.Models.Mobile
{
    [Table(nameof(Event))]
    public class MobileEvent : MobileEntity
    {
        [PrimaryKey]
        public string Id { get; set; }

        [Indexed]
        public int EventType { get; set; }


        [ForeignKey(typeof(MobileUser))]
        public string UserGuid { get; set; }

        [ManyToOne]
        public MobileUser User { get; set; }

        public int Rating { get; set; }

        public int UserEventStatus { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]
        public List<MobileCategory> Categories { get; set; }

        public string CoverUrl { get; set; }

        [SQLiteNetExtensions.Attributes.TextBlob("ThumbnailLargeBlob")]
        public byte[] ThumbnailLarge { get; set; }

        [SQLiteNetExtensions.Attributes.TextBlob("ThumbnailMediumBlob")]
        public byte[] ThumbnailMedium { get; set; }

        [SQLiteNetExtensions.Attributes.TextBlob("ThumbnailSmallBlob")]
        public byte[] ThumbnailSmall { get; set; }

        public string ThumbnailLargeBlob { get; set; }
        public string ThumbnailMediumBlob { get; set; }
        public string ThumbnailSmallBlob { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int AttendingCount { get; set; }

        public int AttendingCountMale { get; set; }

        public int AttendingCountFemale { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int Priority { get; set; }

        public int LocationId { get; set; }

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string ZipCode { get; set; }
    }
}
