using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IWorldRepository _repository;

        public AppController(IMailService service, IWorldRepository repository)
        {
            _mailService = service;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Trips()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            var email = Startup.Configuration["AppSettings:SiteEmailAddress"];

            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("", "No admin e-mail found");
            }

            var result = _mailService.SendMail(
                email,
                email,
                $"Contact message from {model.Name} ({model.Email})",
                model.Message);

            if (result)
            {
                ModelState.Clear();
                ViewBag.Message = "Mail was sent. Thank you!";
            }

            return View();
        }
    }
}
