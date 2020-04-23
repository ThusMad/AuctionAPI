using EPAM_BusinessLogicLayer.DTO;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_BusinessLogicLayer.Services.Interfaces
{
    public interface IBidService
    {
        void PlaceBid(BidDTO bid);
    }
}