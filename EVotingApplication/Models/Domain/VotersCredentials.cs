using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EVotingApplication.Models.Domain
{
    public class VotersCredentials
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid VoterId { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        public bool voted { get; set; }
    }
}
