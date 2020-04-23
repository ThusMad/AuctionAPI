using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AutoMapper;
using EPAM_BusinessLogicLayer.DTO;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Interfaces;

namespace EPAM_BusinessLogicLayer.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void DeletePaymentMethod(Guid id)
        {
            var paymentRepository = _unitOfWork.Repository<PaymentMethod>();

            var paymentMethod = paymentRepository.GetById(id);
            if (paymentMethod != null)
            {
                paymentRepository.Delete(paymentMethod);
                _unitOfWork.Commit();
            }
        }

        public IEnumerable<PaymentMethodDTO>? GetPaymentMethods(Guid userId)
        {
            var paymentRepository = _unitOfWork.Repository<PaymentMethod>();
            var methods = paymentRepository.Find(a => a.UserId == userId.ToString());

            if (methods != null)
            {
                return _mapper.Map<IEnumerable<PaymentMethod>, IEnumerable<PaymentMethodDTO>>(methods);
            }

            return null;
        }

        public void AddPaymentMethod(Guid userId, PaymentMethodDTO paymentMethod)
        {
            var method = _mapper.Map<PaymentMethodDTO, PaymentMethod>(paymentMethod);
            
            var userRepository = _unitOfWork.Repository<ApplicationUser>();
            var user = userRepository.GetById(userId);

            method.UserId = user.Id;
            method.User = user;

            var paymentRepository = _unitOfWork.Repository<PaymentMethod>();

            paymentRepository.Insert(method);
            _unitOfWork.Commit();
        }
    }
}
