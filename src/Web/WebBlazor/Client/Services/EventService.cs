using System;
using System.Threading.Tasks;

namespace WebBlazor.Client.Services
{
    public class EventService
    {
        public event Func<string, Task> BasketItemAdded;

        public void OnBasketItemAdded(string userId) =>
            BasketItemAdded?.Invoke(userId);
    }
}
