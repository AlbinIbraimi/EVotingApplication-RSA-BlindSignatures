using EVotingApplication.Data;
using EVotingApplication.Models.Domain;
using EVotingApplication.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVotingApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<VotersIdentity> _signInManager;
        private readonly UserManager<VotersIdentity> _userManager;
        private readonly ApplicationDbContext _context;
        public AccountController(SignInManager<VotersIdentity> signInManager, UserManager<VotersIdentity> userManager, ApplicationDbContext context) 
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._context = context;
        }

        public IActionResult Login()
        {
            var dto = new LoginViewDto();
            return View(dto);
        }

        [HttpPost]
        public IActionResult TestAction()
        {
            return new JsonResult("test API");
        }

        [HttpPost]
        public IActionResult Login(LoginViewDto model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var voter = _userManager.FindByNameAsync(model.Embg).Result;
            Startup.LogedInUserId = Guid.Parse(voter.Id);
             
            if (!checkInformations(model,voter))
            {
                ModelState.AddModelError("", "Invalid Credentials");
                return View(model);
            }


            _signInManager.SignInAsync(voter, false).Wait();
           
            return RedirectToAction("Index","Home");
        }

        private bool checkInformations(LoginViewDto model, VotersIdentity voter) 
        {
           

            //check if voter with the model.embg exist
            if (voter == null)
                return false;

                // check if the embg corespond to the name, surname of the voter;
                if (!voter.Name.Equals(model.Name) || !voter.Surname.Equals(model.Surname))
                {
                    return false;
                }

                //check credentials of the voter and the model
                var credentials = _context.VotersCredentials.FirstOrDefault(m => m.VoterId.Equals(voter.Id));
                if(credentials != null)
                {
                    if(! credentials.username.Equals(model.UserName) || ! credentials.password.Equals(model.Password))
                    {
                        return false;
                    }
                }

            return true;           
        }
    }
}
