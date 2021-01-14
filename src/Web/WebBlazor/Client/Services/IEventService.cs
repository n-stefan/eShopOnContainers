using System;
using System.Threading.Tasks;

namespace WebBlazor.Client.Services
{
    public interface IEventService
    {
        event Func<string, Task> BasketItemAdded;

        void OnBasketItemAdded(string userId);
    }
}
