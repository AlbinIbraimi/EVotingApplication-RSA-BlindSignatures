using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace EVotingApplication.Models.DTO
{
    public class RequestForSigningDto
    {
        public string vote { get; set; }

        //Credentials
        public string username { get; set; }
        public string password { get; set; }
        public string id { get; set; }
    }
}
