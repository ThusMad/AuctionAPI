using AutoMapper;
using EPAM_DataAccessLayer.Entities;
using Services.DataTransferObjects.Objects;

namespace Services.DataTransferObjects.Profiles
{
    public class BalanceTransactionProfile : Profile
    {
        public BalanceTransactionProfile()
        {
            CreateMap<BalanceTransaction, BalanceTransactionDTO>();
        }
    }
}