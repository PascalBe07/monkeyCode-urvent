using System;
using System.Threading.Tasks;
using MonkeyCode.Framework.Portable.ViewModel;
using Xamarin.Forms;

namespace MonkeyCode.Framework.Portable.XamarinForms.DependencyInjection.Core
{
    internal class Navigator : INavigator
    {
        private readonly Lazy<INavigation> _navigation;
        private readonly IViewFactory _viewFactory;
        private readonly IMasterNavigation _masterNavigation;

        public Navigator(Lazy<INavigation> navigation, IViewFactory viewFactory, IMasterNavigation masterNavigation)
        {
            this._navigation = navigation;
            this._viewFactory = viewFactory;
            this._masterNavigation = masterNavigation;
        }

        private INavigation Navigation => this._navigation.Value;

        public async Task<IViewModel> PopAsync()
        {
            var view = await this.Navigation.PopAsync();
            return view.BindingContext as IViewModel;
        }

        public async Task<IViewModel> PopModalAsync()
        {
            var view = await this.Navigation.PopAsync();
            return view.BindingContext as IViewModel;
        }

        public async Task PopToRootAsync()
        {
            await this.Navigation.PopToRootAsync();
        }

        public async Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = this._viewFactory.Resolve(out viewModel, setStateAction);
            await this.Navigation.PushAsync(view);
            return viewModel;
        }

        public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var view = this._viewFactory.Resolve(viewModel);
            await this.Navigation.PushAsync(view);
            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = this._viewFactory.Resolve(out viewModel, setStateAction);
            await this.Navigation.PushModalAsync(view);
            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var view = this._viewFactory.Resolve(viewModel);
            await this.Navigation.PushModalAsync(view);
            return viewModel;
        }

        public void SetSync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = this._viewFactory.Resolve(out viewModel, setStateAction);
            this._masterNavigation.SetDetail(view);
        }
    }
}