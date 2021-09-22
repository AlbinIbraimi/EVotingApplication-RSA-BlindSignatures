using EVotingApplication.Data;
using EVotingApplication.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EVotingApplication.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class Keys : ControllerBase
    {
        private RSA keyPari;
        private BigInteger n;
        private BigInteger e;
        private BigInteger d;
        private readonly ApplicationDbContext _context;
        public Keys(ApplicationDbContext context) 
        {
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

        public string getPublicKeyParametars()
        { 
            var KeyDatat = new
            {
                exponent = e.ToString(),
                module = n.ToString()
            };

            return JsonConvert.SerializeObject(KeyDatat);
        }



        [HttpPost]
        [ActionName("SignMessage")]
        public string SignMessage([FromBody]RequestForSigningDto dto)
        {

            //ceck the credentials of the voter;

            try
            {
                var credentials = _context.VotersCredentials.FirstOrDefault(m => m.VoterId.Equals(Guid.Parse(dto.id)));
                if (credentials == null || !credentials.username.Equals(dto.username) || !credentials.password.Equals(dto.password) || credentials.voted==true)
                {
                    return JsonConvert.SerializeObject(false);
                }
                else
                {
                    credentials.voted = true;
                    _context.VotersCredentials.Update(credentials);
                    _context.SaveChanges();
                }
            }
            catch (Exception a)
            {
                return JsonConvert.SerializeObject(false);
            }

            //

            BigInteger message = BigInteger.Parse(dto.vote);
            BigInteger c = BigInteger.ModPow(message, d, n);
            
            //var parametars = keyPari.ExportParameters(true);
            //var key = new RSACryptoServiceProvider();
            //key.ImportParameters(parametars);
            //BigInteger c = new BigInteger(key.Encrypt(message.ToByteArray(), false));

            return JsonConvert.SerializeObject(c);
        }
    }
}
