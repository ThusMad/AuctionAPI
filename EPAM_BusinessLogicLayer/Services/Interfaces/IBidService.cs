using EPAM_BusinessLogicLayer.DataTransferObjects;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_BusinessLogicLayer.Services.Interfaces
{
    public interface IBidService
    {
        void PlaceBid(BidDTO bid);
    }
}