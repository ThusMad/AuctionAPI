using AutoMapper;
using EPAM_BusinessLogicLayer.DataTransferObjects;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_BusinessLogicLayer.Profiles
{
    class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            CreateMap<PaymentMethod, PaymentMethodDTO>();
            CreateMap<PaymentMethod, DefaultPaymentMethod>()
                .ForMember(a=> a.UserId, opt => opt.MapFrom(b => b.UserId))
                .ForMember(a => a.PaymentMethodId, opt => opt.MapFrom(b => b.Id));
            CreateMap<PaymentMethodDTO, PaymentMethod>();
        }
    }
}
