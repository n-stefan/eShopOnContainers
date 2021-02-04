using System;
using System.Threading.Tasks;

namespace WebBlazor.Client.Services
{
    public class EventService : IEventService
    {
        public event Func<string, Task> BasketItemAdded;

        public event Action OrderCreated;

        public void OnBasketItemAdded(string userId) =>
            BasketItemAdded?.Invoke(userId);

        public void OnOrderCreated() =>
            OrderCreated?.Invoke();
    }
}
