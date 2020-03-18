using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using EPAM_DataAccessLayer.EF;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Interfaces;
using EPAM_DataAccessLayer.Repositories;

namespace EPAM_DataAccessLayer.UnitOfWork
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly AuctionContext _db;

        private BidsRepository? _bidRepository;
        private AuctionsRepository? _auctionRepository;
        private UsersRepository? _userRepository;

        public EFUnitOfWork()
        {
           
            //var connection = ConfigurationManager.ConnectionStrings["EPAM_Auction"].ConnectionString;
            //Debug.WriteLine(connection);
            _db = new AuctionContext(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Projects\EPAM_Auction\EPAM_DataAccessLayer\App_Data\EPAM_Auction.mdf;Integrated Security=True");
        }
        public IRepository<Bid> Bids
        {
            get
            {
                if (_bidRepository == null)
                    _bidRepository = new BidsRepository(_db);
                return _bidRepository;
            }
        }

        public IRepository<Auction> Auctions
        {
            get
            {
                if (_auctionRepository == null)
                    _auctionRepository = new AuctionsRepository(_db);
                return _auctionRepository;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UsersRepository(_db);
                return _userRepository;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
