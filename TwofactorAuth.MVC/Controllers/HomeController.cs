using Google.Authenticator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TwofactorAuth.MVC.Models;

namespace TwofactorAuth.MVC.Controllers
{
    public class HomeController : Controller
    {
        private const string _secretKey = "[secretKey]";
        private string _username;
        private string _AuthKey;

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginRequest login)
        {
            if(login.Username == "luizgbs" && login.Password == "123")
            {
                ViewBag.Status = true;
                _username = login.Username;

                TwoFactorAuthenticator auth = new TwoFactorAuthenticator();
                _AuthKey = login.Username + _secretKey;
                var setupInfo = auth.GenerateSetupCode("Two Factor", login.Username, _AuthKey, false, 3);
                ViewBag.QrSrc = setupInfo.QrCodeSetupImageUrl;
                ViewBag.Key = setupInfo.ManualEntryKey;

            }
            else
            {
                ViewBag.Status = false;
                ViewBag.Message = "Usuário e/ou senha inválidos";
            }
            return View();
        }

        [HttpPost]
        public ActionResult TwoFactor(string passKey)
        {
            TwoFactorAuthenticator auth = new TwoFactorAuthenticator();
            if (auth.ValidateTwoFactorPIN(_AuthKey, passKey))
                ViewBag.Message = "Logado";
            else
                ViewBag.Message = "Código inválido";

            ViewBag.Status = true;
            return RedirectToAction("Login");
        }
    }
}
