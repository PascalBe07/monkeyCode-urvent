using System.Collections.Generic;
using Xamarin.Forms;

namespace MonkeyCode.Framework.Portable.XamarinForms.Controls
{
    public interface IMultiItemPage<TItem>
    {
        //TItem CurrentItem { get; }

        TItem[] ItemsSource { get; set; }

        DataTemplate ItemTemplate { set; }

        //void GoToNextItem();
    }
}
