using System;
using System.Collections.Generic;
using System.Text;
using EPAM_SocketSlot.Interfaces;

namespace EPAM_SocketSlot
{
    class SlotFactory : ISlotFactory
    {
        public ISocketSlot CreateInstance()
        {
            ISocketSlot slot = new SocketSlot();

            return slot;
        }
    }
}
