using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPAM_BusinessLogicLayer.DataTransferObject;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using EPAM_DataAccessLayer.Interfaces;

namespace EPAM_BusinessLogicLayer.Services
{
    public class BidService : IBidService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

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
