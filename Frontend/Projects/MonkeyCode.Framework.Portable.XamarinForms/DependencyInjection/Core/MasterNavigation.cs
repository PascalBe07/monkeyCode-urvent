using System;
using Xamarin.Forms;

namespace MonkeyCode.Framework.Portable.XamarinForms.DependencyInjection.Core
{
    internal class MasterNavigation : IMasterNavigation
    {
        private readonly Action<Page> _setDetail;

        public MasterNavigation(Action<Page> setDetail)
        {
            this._setDetail = setDetail;
        }

        public void SetDetail(Page page)
        {
            var pageToSet = page.Parent as NavigationPage ?? new NavigationPage(page);
            this._setDetail(pageToSet);
        }
    }
}