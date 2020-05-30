using EPAM_BusinessLogicLayer.DataTransferObjects;

namespace EPAM_BusinessLogicLayer.Services.Interfaces
{
    public interface IBidService
    {
        void PlaceBid(BidDTO bid);
    }
}