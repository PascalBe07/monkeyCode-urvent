using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autofac;
using MonkeyCode.Framework.Portable.ViewModel;
using Xamarin.Forms;

namespace MonkeyCode.Framework.Portable.XamarinForms.DependencyInjection.Core
{
    internal class ViewFactory : IViewFactory
    {
        private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();
        private readonly IComponentContext _componentContext;

        public ViewFactory(IComponentContext componentContext)
        {
            this._componentContext = componentContext;
        }

        public void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Page
        {
            this._map[typeof(TViewModel)] = typeof(TView);
        }

        public Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null) where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            return this.Resolve(out viewModel, setStateAction);
        }

        public Page Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            viewModel = this._componentContext.Resolve<TViewModel>();

            var viewType = this._map[typeof(TViewModel)];
            var view = this._componentContext.Resolve(viewType) as Page;

            if (setStateAction != null)
                viewModel.SetState(setStateAction);

            Debug.Assert(view != null, "view != null");
            view.BindingContext = viewModel;
            return view;
        }

        public Page Resolve<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var viewType = this._map[typeof(TViewModel)];
            var view = this._componentContext.Resolve(viewType) as Page;
            Debug.Assert(view != null, "view != null");
            view.BindingContext = viewModel;
            return view;
        }
    }
}