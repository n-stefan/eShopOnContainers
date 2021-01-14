using System;
using System.Threading.Tasks;

namespace WebBlazor.Client.Services
{
    public class EventService : IEventService
    {
        public event Func<string, Task> BasketItemAdded;

        public void OnBasketItemAdded(string userId) =>
            BasketItemAdded?.Invoke(userId);
    }
}
