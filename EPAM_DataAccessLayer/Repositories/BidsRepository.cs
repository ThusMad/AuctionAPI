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
    class BidsRepository : IRepository<Bid>
    {
        private AuctionContext db;

        public BidsRepository(AuctionContext context)
        {
            this.db = context;
        }

        public void Create(Bid item)
        {
            db.Bids.Add(item);
        }

        public void Delete(int id)
        {
            Bid bid = db.Bids.Find(id);
            if (bid != null)
                db.Bids.Remove(bid);
        }

        public IEnumerable<Bid> Find(Func<Bid, bool> predicate)
        {
            return db.Bids.Where(predicate).ToList();
        }

        public Bid Get(int id)
        {
            return db.Bids.Find(id);
        }

        public IEnumerable<Bid> GetAll()
        {
            return db.Bids;
        }

        public void Update(Bid item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
