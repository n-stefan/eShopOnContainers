using System;
using System.Threading.Tasks;

namespace WebBlazor.Client.Services
{
    public interface IEventService
    {
        event Func<string, Task> BasketItemAdded;

        event Action OrderCreated;

        void OnBasketItemAdded(string userId);

        void OnOrderCreated();
    }
}
