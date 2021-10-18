﻿using hotel_booking_model;
using hotel_booking_services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using hotel_booking_model.AuthModels;
using System.IdentityModel.Tokens.Jwt;

namespace hotel_booking_mvc.Controllers.Auth
{
    public class AuthController : Controller
    {

        public AuthController()
        {

        }


        private readonly IAuthRepository _auth;


        public AuthController(IAuthRepository auth)
        {
            _auth = auth;
        }

        public IActionResult Login()
        {
            return View();
        }





        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            var handler = new JwtSecurityTokenHandler();


            if (ModelState.IsValid)
            {
                try
                {
                    var result = _auth.Login(loginModel);

                    HttpContext.Session.SetString("user", JsonConvert.SerializeObject(result));
                    JwtSecurityToken decodedValue = handler.ReadJwtToken(result.Data[' ']);
                    var Claim = decodedValue.Claims.ElementAt(2);
                    if (Claim.Value == "Manager")
                    {
                        return RedirectToAction("Dashboard", "Manager");
                    }
                    return RedirectToAction("Dashboard", "Admin");
                }
                catch (Exception)
                {
                    TempData["error"] = "Oops something bad happened try again!";
                    return View();
                }
            }
            return View();
        }




        public IActionResult Signup()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Signup(SignupModel signupmodel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                var result = _auth.Signup(signupmodel);
                //  result.EnsureSuccessStatusCode();
                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                TempData["error"] = "Oops something bad happened try again!";
                return View();
            }
        }







        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult ConfirmEmail()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }


    }
}
