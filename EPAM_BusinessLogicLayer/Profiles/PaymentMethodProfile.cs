using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPAM_BusinessLogicLayer.DTO;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_BusinessLogicLayer.Profiles
{
    class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            CreateMap<PaymentMethod, PaymentMethodDTO>();
            CreateMap<PaymentMethodDTO, PaymentMethod>();
        }
    }
}
