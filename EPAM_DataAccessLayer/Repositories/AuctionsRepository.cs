using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using EPAM_DataAccessLayer.EF;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Interfaces;

namespace EPAM_DataAccessLayer.Repositories
{
    class AuctionsRepository : IRepository<Auction>
    {
        private AuctionContext db;

        public AuctionsRepository(AuctionContext context)
        {
            this.db = context;
        }

        public IEnumerable<Auction> GetAll()
        {
            return db.Auctions;
        }

        public Auction Get(int id)
        {
            return db.Auctions.Find(id);
        }

        public IEnumerable<Auction> Find(Func<Auction, bool> predicate)
        {
            return db.Auctions.Where(predicate).ToList();
        }

        public void Create(Auction item)
        {
            db.Auctions.Add(item);
        }

        public void Update(Auction item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Auction auction = db.Auctions.Find(id);
            if (auction != null)
                db.Auctions.Remove(auction);
        }
    }
}
