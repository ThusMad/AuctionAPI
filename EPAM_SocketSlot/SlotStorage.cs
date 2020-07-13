using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using EPAM_SocketSlot.Interfaces;

namespace EPAM_SocketSlot
{
    public class SlotStorage : ISlotStorage
    {
        private readonly ConcurrentDictionary<Guid, ISocketSlot> _slots;

        public SlotStorage()
        {
            _slots = new ConcurrentDictionary<Guid, ISocketSlot>();
        }

        public async Task NotifySlotsAsync(Guid slotId, string data)
        {
            if (_slots.ContainsKey(slotId))
            {
                _slots[slotId].NotifyAllSubscribers(data);
            }
        }

        public async Task AddSubscriber(Guid slotId, WebSocket webSocket, TaskCompletionSource<object> socketFinishedTcs)
        {
            ISocketSlot slot;

            if (!_slots.ContainsKey(slotId))
            {
                slot = new SocketSlot();
                while (!_slots.TryAdd(slotId, slot))
                {
                    Thread.Sleep(5);
                }
            }
            else
            {
                while (!_slots.TryGetValue(slotId, out slot))
                {
                    Thread.Sleep(5);
                }
            }

            slot.AddSubscriber(webSocket, socketFinishedTcs);
        }

        public ISocketSlot? GetSlot(Guid slotId)
        {
            if (!_slots.ContainsKey(slotId))
            {
                return null;
            }

            ISocketSlot slot;
            while (!_slots.TryGetValue(slotId, out slot))
            {
                Thread.Sleep(5);
            }

            return slot;

        }
    }
}