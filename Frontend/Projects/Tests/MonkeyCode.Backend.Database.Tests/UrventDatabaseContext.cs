using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using MonkeyCode.Framework.Portable.Urvent.Models;
using MySql.Data.Entity;

namespace MonkeyCode.Backend.Database.Tests
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class UrventDatabaseContext : DatabaseContext
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Cover> Cover { get; set; }
        public DbSet<EventType> EventType { get; set; }
        public DbSet<EventDateRange> EventDateRange { get; set; }
        public DbSet<Setting> Setting{ get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserEventStatus> UserEventStatus { get; set; }
        public DbSet<UserEvent> UserEvent { get; set; }
        

        public UrventDatabaseContext() : base("urvent")
        {
            System.Data.Entity.Database.SetInitializer(new DropCreateDatabaseAlways<UrventDatabaseContext>());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure domain classes using Fluent API here
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Category>().HasKey<int>(s => s.Id);
            modelBuilder.Entity<Category>()
                .Property(p => p.Name)
                .IsRequired();


            modelBuilder.Entity<Event>().HasKey(s => new {s.Id, s.EventTypeId});

            modelBuilder.Entity<Event>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<Event>()
                .Property(p => p.Description)
                .IsRequired();

            modelBuilder.Entity<Event>()
                .Property(p => p.Price)
                .IsOptional();
            
            modelBuilder.Entity<Event>()
                    .Property(p => p.AttendingCount)
                    .IsRequired();

            modelBuilder.Entity<Event>()
                    .Property(p => p.AttendingCountFemale)
                    .IsRequired();

            modelBuilder.Entity<Event>()
                    .Property(p => p.AttendingCountMale)
                    .IsRequired();

            modelBuilder.Entity<Event>()
                    .Property(p => p.Priority)
                    .IsRequired();

            modelBuilder.Entity<Event>()
                    .Property(p => p.StartDateTime)
                    .IsRequired();

            modelBuilder.Entity<Event>()
                    .Property(p => p.EndDateTime)
                    .IsRequired();
            

            

            /*
             * The property 'EventType' cannot be used as a key property on the entity 
             * 'MonkeyCode.Framework.Portable.Urvent.Models.UserEvent' 
             * because the property type is not a valid key type. 
             * Only scalar types, string and byte[] are supported key types.
             */

            modelBuilder.Entity<UserEvent>()
                .HasKey(c => new { c.EventId, c.EventTypeId, c.UserEMail });

            modelBuilder.Entity<User>()
                .HasMany(c => c.UserEvents)
                .WithRequired()
                .HasForeignKey(c => c.UserEMail);

            modelBuilder.Entity<Event>()
                .HasMany(c => c.UserEvents)
                .WithRequired()
                .HasForeignKey(c => new { c.EventId, c.EventTypeId });

            modelBuilder.Entity<Cover>().HasKey(s => new { s.Url});
            modelBuilder.Entity<Cover>().Property(e => e.Url).HasMaxLength(512);

            modelBuilder.Entity<Location>().HasKey(s => new { s.Id, s.Longitude, s.Latitude });

            modelBuilder.Entity<User>().HasKey(s => new { s.EMail });
        }



    }
}
