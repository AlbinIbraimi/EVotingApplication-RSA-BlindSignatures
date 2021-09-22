using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVotingApplication.Models.Domain
{
    public class Candidate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PoliticalParty { get; set; }
        public int votes { get; set; }

    }
}
