using EVotingApplication.Data;
using EVotingApplication.Models.Domain;
using EVotingApplication.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EVotingApplication.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class Count : ControllerBase
    {
        private RSA keyPari;
        private BigInteger n;
        private BigInteger e;
        private BigInteger d;
        private readonly ApplicationDbContext _context;
        public Count(ApplicationDbContext context) {
            this._context = context;

            string keyPariPath = @"C:\Users\Albin\Desktop\Informaciska_Bezbednost_FinalProekt\EVotingApplication\EVotingApplication\Controllers\Keys\keypair.pem";
            string keyPariString = "";


            using (StreamReader file = new StreamReader(keyPariPath))
            {
                keyPariString = file.ReadToEnd();

            }

            this.keyPari = RSA.Create();
            keyPari.ImportFromPem(keyPariString);

            var parametars = keyPari.ExportParameters(true);

            BigInteger e = new BigInteger(parametars.Exponent);
            BigInteger n = new BigInteger(parametars.Modulus);
            BigInteger p = new BigInteger(parametars.P);
            BigInteger q = new BigInteger(parametars.Q);
            BigInteger d = new BigInteger(parametars.D);

            BigInteger phi = (p - 1) * (q - 1);

            //transform negative module to positiv
            if (n < 0)
            {
                n = n % phi;
                if (n < 0)
                {
                    n += phi;
                }
            }

            //transform negative private exponent D to positive
            if (d < 0)
            {
                d = d % phi;
                if (d < 0)
                {
                    d += phi;
                }
            }

            this.n = n;
            this.e = e;
            this.d = d;
        }

        [HttpPost]
        public void verify_count([FromBody]CandidatDto dto)
        {
            
            BigInteger vote = BigInteger.Parse(dto.vote_signed);
            //var parametars = keyPari.ExportParameters(true);
            //var key = new RSACryptoServiceProvider();
            //key.ImportParameters(parametars);

            BigInteger decrypt = BigInteger.ModPow(vote, e,n);

            string vote_id = Convert.ToBase64String(decrypt.ToByteArray());

            //find the candidate;
            try
            {
                var candidat = _context.Candidates.FirstOrDefault(m => m.Id.Equals(vote_id));
                if (candidat == null)
                    return;
                candidat.votes++;
                _context.Candidates.Update(candidat);
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                return;
            }


        }
    }
}
