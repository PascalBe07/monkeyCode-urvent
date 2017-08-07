using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MonkeyCode.Framework.Portable.ViewModel
{
    public abstract class ViewModel : IViewModel
    {
        public string Title { get; set; }

        public void SetState<T>(Action<T> action)
            where T : class, IViewModel
        {
            action(this as T);
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
