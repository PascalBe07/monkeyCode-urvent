using MonkeyCode.Framework.Portable.Urvent.Models;
using MonkeyCode.Framework.Portable.Urvent.Models.Mobile;

namespace MonkeyCode.Framework.Portable.Urvent.Services
{
    public interface IModelConverterService
    {
        MobileEntity Convert(Entity item);

        Entity Convert(MobileEntity mobEntity);
    }
}
