using AutoMapper;
using Services.DataTransferObjects.Objects;
using EPAM_DataAccessLayer.Entities;

namespace Services.DataTransferObjects.Profiles
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
