using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonkeyCode.Framework.Portable.Urvent.Models;
using Xamarin.Forms;

namespace MonkeyCode.Framework.Portable.Urvent.Services.Core
{
    public class MockEventService : IEventService
    {
        //public IEnumerable<Event> GetEvents(double latitude, double longtidue, EventDateRange dateRange)
        //{
        //    yield return new Event
        //    {
        //        Id = "Mock_1",
        //        Name = "My awesome event",
        //        StartDateTime = DateTime.Today,
        //        EndDateTime = DateTime.Today.AddHours(10),
        //        Description = "Images are a crucial part of application navigation, useability, and branding. Xamarin.Forms applications need to be able to share images across all platforms, but also potentially display different images on each platform. Platform - specific images are also required for icons and splash screens; these need to be configured on a per-platform basis.",
        //        Cover = new Cover
        //        {
        //            Url = @"https://i.embed.ly/1/display/resize?key=1e6a1a1efdb011df84894040444cdc60&url=http%3A%2F%2Fpbs.twimg.com%2Fmedia%2FBzti-cDIAAAb_6Q.jpg"
        //        }
        //    };
        //    yield return new Event
        //    {
        //        Id = "Mock_2",
        //        Name = "This is the most awesome event",
        //        StartDateTime = DateTime.Now,
        //        EndDateTime = DateTime.Now.AddDays(2),
        //        Description = "This article discusses the software development lifecycle with respect to mobile applications, and discusses some of the considerations required when building mobile projects. For developers wishing to just jump right in and start building, this guide can be skipped and read later for a more complete understanding of mobile development.",
        //        Cover = new Cover
        //        {
        //            Url = @"http://image.slidesharecdn.com/vstsextensions-160404123139/95/visual-studio-team-services-extensions-by-taavi-kosaar-melborp-3-638.jpg?cb=1459773764"
        //        }
        //    };
        //}

        //public IEnumerable<Event> GetEvents(string city)
        //{
        //    return this.GetEvents();
        //}
        public Task<IEnumerable<Event>> GetEvents(double latitude, double longitude, EventDateRange dateRange)
        {
            throw new NotImplementedException();
        }

        public Task<ImageSource> GetCover(string url, ThumbnailSize size)
        {
            throw new NotImplementedException();
        }
    }
}