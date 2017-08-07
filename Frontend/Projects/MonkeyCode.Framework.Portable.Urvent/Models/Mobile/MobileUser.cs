using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace MonkeyCode.Framework.Portable.Urvent.Models.Mobile
{
    [Table(nameof(User))]
    public class MobileUser : MobileEntity
    {
        [PrimaryKey]
        public string Guid { get; set; }

        public string EMail { get; set; }

        public string AccessToken { get; set; }

        public DateTime AccessTokenReceived { get; set; }

        public DateTime Birthday { get; set; }

        public DateTime LastLogin { get; set; }

        public int Gender { get; set; }

        public string FacebookAccessToken { get; set; }

        public DateTime FacebookAccessTokenReceived { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]
        public List<MobileEvent> UserEvents { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public double MaxDistance { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]
        public List<MobileCategory> ExcludedCategories { get; set; }

        public int AttractedTo { get; set; }

        public int EventDateRange { get; set; }
    }
}
