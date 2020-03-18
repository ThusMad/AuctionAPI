using System;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Auction> Auctions { get; }
        IRepository<Bid> Bids { get; }
        IRepository<User> Users { get; }
        void Save();
    }
}