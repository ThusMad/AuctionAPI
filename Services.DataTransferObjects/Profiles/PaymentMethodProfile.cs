using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using AutoMapper;
using Services.DataTransferObjects.Objects;
using EPAM_DataAccessLayer.Entities;

namespace Services.DataTransferObjects.Profiles
{
    class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            CreateMap<PaymentMethod, PaymentMethodDTO>()
                .AfterMap(HideSensitive);
            CreateMap<PaymentMethod, DefaultPaymentMethod>()
                .ForMember(a=> a.UserId, opt => opt.MapFrom(b => b.UserId))
                .ForMember(a => a.PaymentMethodId, opt => opt.MapFrom(b => b.Id));
            CreateMap<PaymentMethodDTO, PaymentMethod>();
        }

        private static void HideSensitive(PaymentMethod auctionDto, PaymentMethodDTO auction)
        {
            var cardNumber = new StringBuilder(auctionDto.CardNumber);
            for (var i = 4; i < 12; i++)
            {
                cardNumber[i] = '*';
            }
            auctionDto.CardNumber = cardNumber.ToString();
        }
    }
}
