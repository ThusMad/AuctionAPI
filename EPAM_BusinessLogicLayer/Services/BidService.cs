using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPAM_BusinessLogicLayer.DataTransferObjects;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;

namespace EPAM_BusinessLogicLayer.Services
{
    public class BidService : IBidService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BidService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void PlaceBid(BidDTO bid)
        {
            throw new NotImplementedException();
        }
    }
}
