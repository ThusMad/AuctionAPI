using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM_BusinessLogicLayer.DataTransferObject
{
    public class TokenDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
