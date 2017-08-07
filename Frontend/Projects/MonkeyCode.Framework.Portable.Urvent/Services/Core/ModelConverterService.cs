using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MonkeyCode.Framework.Portable.Urvent.Models;
using MonkeyCode.Framework.Portable.Urvent.Models.Mobile;

namespace MonkeyCode.Framework.Portable.Urvent.Services.Core
{
    public class ModelConverterService : IModelConverterService
    {
        private readonly IMapper autoMapper;

        public ModelConverterService()
        { 
            //create AutoMapper mappings
            MapperConfiguration mapperConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<Category, MobileCategory>()
                    .ForMember(t => t.EventId, opt => opt.Ignore())
                    .ForMember(t => t.UserGuid, opt => opt.Ignore());
                cfg.CreateMap<MobileCategory, Category>();
                cfg.CreateMap<MobileCategory, User>()
                    .ForMember(x => x.UrventUserData, opt => opt.ResolveUsing(model => new UrventUserData() { Guid = model.UserGuid }))
                    .ForMember(x => x.Gender, opt => opt.Ignore())
                    .ForMember(x => x.Birthday, opt => opt.Ignore())
                    .ForMember(x => x.EMail, opt => opt.Ignore())
                    .ForMember(x => x.LastLogin, opt => opt.Ignore())
                    .ForMember(x => x.FacebookUserData, opt => opt.Ignore())
                    .ForMember(x => x.UserEvents, opt => opt.Ignore());
                cfg.CreateMap<User, MobileCategory>()
                    .ForMember(d => d.UserGuid, a => a.MapFrom(s => s.UrventUserData.Guid))
                    .ForMember(d => d.Id, opt => opt.Ignore())
                    .ForMember(d => d.EventId, opt => opt.Ignore())
                    .ForMember(d => d.Name, opt => opt.Ignore());
                cfg.CreateMap<Event, MobileCategory>()
                    .ForMember(d => d.UserGuid, opt => opt.Ignore())
                    .ForMember(d => d.Id, opt => opt.Ignore())
                    .ForMember(d => d.EventId, a => a.MapFrom(s => s.Id))
                    .ForMember(d => d.Name, opt => opt.Ignore());
                cfg.CreateMap<Event, MobileEvent>()
                    //.ForMember(d => d.Id, a => a.MapFrom(s => s.Id))
                    .ForMember(d => d.EventType, a => a.MapFrom(s => s.EventType.Id))
                    .ForMember(d => d.UserGuid, opt => opt.Ignore())
                    .ForMember(d => d.Rating, opt => opt.Ignore())
                    .ForMember(d => d.UserEventStatus, opt => opt.Ignore())
                    .ForMember(d => d.Categories, a => a.MapFrom(s => s.Categories))
                    .ForMember(d => d.CoverUrl, a => a.MapFrom(s => s.Cover.Url))
                    .ForMember(d => d.User, opt => opt.Ignore())
                    .ForMember(d => d.ThumbnailSmallBlob, opt => opt.Ignore())
                    .ForMember(d => d.ThumbnailMediumBlob, opt => opt.Ignore())
                    .ForMember(d => d.ThumbnailLargeBlob, opt => opt.Ignore())
                    .ForMember(d => d.ThumbnailSmall, a => a.MapFrom(s => s.Cover.ThumbnailSmall))
                    .ForMember(d => d.ThumbnailMedium, a => a.MapFrom(s => s.Cover.ThumbnailMedium))
                    .ForMember(d => d.ThumbnailLarge, a => a.MapFrom(s => s.Cover.ThumbnailLarge))
                    //.ForMember(d => d.Name, a => a.MapFrom(s => s.Name))
                    .ForMember(d => d.LocationId, a => a.MapFrom(s => s.Location.Id))
                    .ForMember(d => d.Longitude, a => a.MapFrom(s => s.Location.Longitude))
                    .ForMember(d => d.Latitude, a => a.MapFrom(s => s.Location.Latitude))
                    .ForMember(d => d.City, a => a.MapFrom(s => s.Location.City))
                    .ForMember(d => d.Street, a => a.MapFrom(s => s.Location.Street))
                    .ForMember(d => d.ZipCode, a => a.MapFrom(s => s.Location.ZipCode));
                cfg.CreateMap<MobileEvent, Event>()
                    .ForMember(d => d.Id, a => a.MapFrom(s => s.Id))
                    .ForMember(d => d.EventTypeId, a => a.MapFrom(s => s.EventType))
                    .ForMember(d => d.UserEvents, opt => opt.Ignore())
                    .ForMember(d => d.EventType, opt => opt.ResolveUsing(model => new EventType() { Id = model.EventType }))
                    .ForMember(d => d.Cover, opt => opt.ResolveUsing(model => new Cover() { Url = model.CoverUrl, ThumbnailSmall = model.ThumbnailSmall, ThumbnailMedium = model.ThumbnailMedium, ThumbnailLarge = model.ThumbnailLarge }))
                    .ForMember(d => d.Location, opt => opt.ResolveUsing(model => new Location() { Id = model.LocationId, Longitude = model.Longitude, Latitude = model.Latitude, City = model.City, Street = model.Street, ZipCode = model.ZipCode }))
                    .ForMember(d => d.Categories, a => a.MapFrom(s => s.Categories))
                    .ForMember(d => d.Name, a => a.MapFrom(s => s.Name))
                    .ForMember(d => d.Description, a => a.MapFrom(s => s.Description))
                    .ForMember(d => d.Price, a => a.MapFrom(s => s.Price))
                    .ForMember(d => d.AttendingCount, a => a.MapFrom(s => s.AttendingCount))
                    .ForMember(d => d.AttendingCountMale, a => a.MapFrom(s => s.AttendingCountMale))
                    .ForMember(d => d.AttendingCountFemale, a => a.MapFrom(s => s.AttendingCountFemale))
                    .ForMember(d => d.StartDateTime, a => a.MapFrom(s => s.StartDateTime))
                    .ForMember(d => d.EndDateTime, a => a.MapFrom(s => s.EndDateTime))
                    .ForMember(d => d.Priority, a => a.MapFrom(s => s.Priority));

        });

            mapperConfig.AssertConfigurationIsValid();

            this.autoMapper = mapperConfig.CreateMapper();
        }

        public MobileEntity Convert(Entity item)
        {
            bool coversionResult = false;
            MobileEntity mobEntity = null;
            
            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (item is User)
            {
                User user = ((User)item);
                MobileUser mobUser;
                coversionResult = this.Convert(user, out mobUser);
                mobEntity = mobUser;
            }
            else if(item is Event)
            {
                Event @event = ((Event)item);
                MobileEvent mobEvent;
                coversionResult = this.Convert(@event, out mobEvent);
                mobEntity = mobEvent;
            }
            /*
            else if (item is Category)
            {
                Category category = ((Category)item);
                MobileCategory mobCategory;
                coversionResult = Convert(category, out mobCategory);
                mobEntity = mobCategory;
            }
            */
            else
            {
                throw new InvalidOperationException();
            }

            if (!coversionResult || mobEntity == null)
            {
                throw new InvalidOperationException("Could not convert from Entity to MobileEntity");
            }

            return mobEntity;
        }

        public Entity Convert(MobileEntity item)
        {
            bool coversionResult = false;
            Entity entity = null;

            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (item is MobileUser)
            {
                MobileUser mobUser = ((MobileUser)item);
                User user;
                coversionResult = this.Convert(mobUser, out user);
                entity = user;
            }
            else if (item is MobileEvent)
            {
                MobileEvent mobEvent = ((MobileEvent)item);
                Event @event;
                coversionResult = this.Convert(mobEvent, out @event);
                entity = @event;
            }
            /*
            else if (item is Category)
            {
                Category category = ((Category)item);
                MobileCategory mobCategory;
                coversionResult = Convert(category, out mobCategory);
                mobEntity = mobCategory;
            }
            */
            else
            {
                throw new InvalidOperationException();
            }

            if (!coversionResult || entity == null)
            {
                throw new InvalidOperationException("Could not convert from MobileEntity to Entity");
            }

            return entity;
        }

        private bool Convert(Event e, out MobileEvent mobEvent)
        {
            try
            {
                mobEvent = this.autoMapper.Map<Event, MobileEvent>(e);
                foreach (var category in mobEvent.Categories)
                {
                    category.EventId = mobEvent.Id;
                }


                mobEvent.UserEventStatus = 0;
                if (e.UserEvents.FirstOrDefault() != null)
                {
                    mobEvent.UserEventStatus = e.UserEvents.FirstOrDefault().Status.Id;
                }

            }
            catch
            {
                mobEvent = null;
                return false;
            }
            
            return true;
        }

        private bool Convert(User user, out MobileUser mobUser)
        {
            try
            {
                mobUser = this.autoMapper.Map<User, MobileUser>(user);
                foreach (var category in mobUser.ExcludedCategories)
                {
                    category.UserGuid = mobUser.Guid;
                }
            }
            catch
            {
                mobUser = null;
                return false;
            }

            return true;
        }

        private bool Convert(MobileEvent mobEvent, out Event e)
        {
            try
            {
                e = this.autoMapper.Map<MobileEvent, Event>(mobEvent);
                e.UserEvents = new List<UserEvent>();

                UserEvent newUserEvent = new UserEvent {Status = new UserEventStatus {Id = mobEvent.UserEventStatus}};
                //ToDo use global status
                switch (mobEvent.UserEventStatus)
                {
                    case 1:
                        newUserEvent.Status.Status = "accepted";
                        break;
                    case 2:
                        newUserEvent.Status.Status = "declined";
                        break;
                    default:
                        newUserEvent.Status.Status = "unknown";
                        break;
                }
                newUserEvent.Rating = (uint)mobEvent.Rating;
                e.UserEvents.Add(newUserEvent);
                
            }
            catch
            {
                e = null;
                return false;
            }

            return true;
        }

        private bool Convert(MobileUser mobUser, out User user)
        {
            try
            {
                user = this.autoMapper.Map<MobileUser, User>(mobUser);
            }
            catch
            {
                user = null;
                return false;
            }

            return true;
        }
    }
}
