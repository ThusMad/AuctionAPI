using System;
using System.Threading.Tasks;
using EPAM_SocketSlot.Interfaces;

namespace EPAM_SocketSlot
{
    public class SlotStorage : ISlotStorage
    {
        public SlotStorage()
        {

        }

        public Task NotifySlotsAsync(Guid slotId, string data)
        {
            throw new NotImplementedException();
        }

        public Task AddSlot(Guid slotId, ISocketSlot slot)
        {
            throw new NotImplementedException();
        }

        public ISocketSlot? GetSlot(Guid slotId)
        {
            throw new NotImplementedException();
        }
    }
}