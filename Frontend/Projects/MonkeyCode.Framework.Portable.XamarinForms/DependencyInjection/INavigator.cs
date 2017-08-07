using System;
using System.Threading.Tasks;
using MonkeyCode.Framework.Portable.ViewModel;

namespace MonkeyCode.Framework.Portable.XamarinForms.DependencyInjection
{
    public interface INavigator
    {
        Task<IViewModel> PopAsync();

        Task<IViewModel> PopModalAsync();

        Task PopToRootAsync();

        Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;

        void SetSync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;
    }
}