using System;

namespace EPAM_DataAccessLayer.Entities.Interfaces.Users
{
    public interface IBalanceHolder
    {
        public string Id { get; set; }
        public Balance Balance { get; set; }
    }
}