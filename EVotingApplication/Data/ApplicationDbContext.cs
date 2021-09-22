using EVotingApplication.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVotingApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<VotersIdentity> Voters { get; set; }
        public DbSet<VotersCredentials> VotersCredentials { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
