using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVotingApplication.Models.DTO
{
    public class LoginViewDto
    {
        public string Name{ get; set; }
        public string Surname { get; set; }
        public string Embg { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
