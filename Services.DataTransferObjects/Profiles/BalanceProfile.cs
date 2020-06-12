using AutoMapper;
using EPAM_DataAccessLayer.Entities;
using Services.DataTransferObjects.Objects;

namespace Services.DataTransferObjects.Profiles
{
    public class BalanceProfile : Profile
    {
        public BalanceProfile()
        {
            CreateMap<Balance, BalanceDTO>();

            CreateMap<BalanceTransaction, BalanceTransactionDTO>();
        }
    }
}