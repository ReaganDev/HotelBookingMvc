using hotel_booking_services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using hotel_booking_model.ViewModels;
using hotel_booking_mvc.Helpers;
using Microsoft.AspNetCore.Authorization;
using hotel_booking_mvc.CustomAuthorization;
using hotel_booking_model.Dtos.AuthenticationDtos;
using System;

namespace hotel_booking_mvc.Controllers.Manager
{
    [CustomAuthenticationFilter( "Manager" )]
    public class ManagerController : Controller
    {
        private readonly IManagerService _managerService;
        private readonly IHotelService _hotelService;
        private readonly IAuthenticationService _managerAuth;



        public ManagerController(IHotelService hotelService, IManagerService managerService,IAuthenticationService managerAuth)

        {
            _managerAuth = managerAuth;
            _hotelService = hotelService;
            _managerService = managerService;
        }


        public async Task<IActionResult> DashboardAsync()
        {
            var loggedinUser = HttpContext.Session.GetString("User");
            var user = JsonConvert.DeserializeObject<AuthenticatedDto>(loggedinUser);
            var result = await _managerService.ShowManagerDashboard(user.Id);
            return View(result);
        }


        public async Task<IActionResult> HotelAsync()
        {

            var loggedinUser = HttpContext.Session.GetString("User");
            var user = JsonConvert.DeserializeObject<AuthenticatedDto>(loggedinUser);
            var paginationResponse = await _hotelService.GetAllHotelForManagerAsync(user.Id);
            return View(paginationResponse);
        }


        public IActionResult Bookings()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Transactions(int pageNumber, int pageSize)
        {

            var loggedinUser = HttpContext.Session.GetString("User");
            var user = JsonConvert.DeserializeObject<AuthenticatedDto>(loggedinUser);


            var managerTransactionsList = await _managerService.GetAllManagerTransactionsAsync(user.Id, pageSize, pageNumber);
            return View(managerTransactionsList);
        }


        [HttpPost]
        public async Task<IActionResult> Transactions(int pageNumber, int pageSize, string searchQuery)
        {

            var loggedinUser = HttpContext.Session.GetString("User");
            var user = JsonConvert.DeserializeObject<AuthenticatedDto>(loggedinUser);

            var managerTransactionsList = await _managerService.GetAllManagerTransactionsAsync(user.Id, pageSize, pageNumber, searchQuery);
            return View(managerTransactionsList);
        }


        public async Task<IActionResult> HotelRooms(string roomTypeId)
        {
            var result = await _hotelService.GetRoomTypeDetails(roomTypeId);
            return View(result);
        }

        public async Task<IActionResult> HotelDetails(string hotelId)
        {
            var singleHotel = await _hotelService.GetHotelById(hotelId);
            var customers = await _hotelService.GetHotelCustomersAsync(hotelId);
            ViewData["GetHotel"] = singleHotel;
            ViewData["Customers"] = customers;
            return View();
        }


        public IActionResult Account()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Account(UserDto userDto)
        {
            return View();
        }


        [HttpGet]
        public IActionResult AddHotel()
        {
            var hotel = new AddHotelViewModel();
            return View(hotel);
        }


        [HttpPost]
        public async Task<IActionResult> AddHotel(AddHotelViewModel model)
        {
            var user = HttpContext.Session.GetString("User");
            var loggedInUser = JsonConvert.DeserializeObject<AuthenticatedDto>(user);

            if (ModelState.IsValid)
            {
                var result = await _hotelService.AddHotelAsync(model);
                if (result.Succeeded)
                {
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_HotelList", await _hotelService.GetAllHotelForManagerAsync(loggedInUser.Id)) });
                }
                ViewBag.Error = result.Message;
                return BadRequest();
            }

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddHotel", model) });

        }


        [AllowAnonymous]
        public IActionResult RegisterManager()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(UpdatePasswordDto obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var response = _managerAuth.UpdatePassword(obj);
                ViewBag.Data = response.Result;
                return View();
            }
            catch (Exception)
            {
                TempData["error"] = "Oops something bad happened try again!";
                return View();
            }
        }
    }

    
}
