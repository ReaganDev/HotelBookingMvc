﻿using hotel_booking_model;
using hotel_booking_model.ViewModels;
using hotel_booking_services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_mvc.Controllers.Admin
{
	public class AdminController : Controller
	{
		private readonly IHotelService _hotelService;
		private readonly IAdminService _adminService;

		public AdminController(IHotelService hotelService, IAdminService adminService)
		{
			_hotelService = hotelService;
			_adminService = adminService;
		}
		public async Task<IActionResult> Dashboard()
		{
			var result = await _adminService.ShowAdminDashboard();
			return View(result);
		}
		public async Task<IActionResult> HotelAsync(int pageNumber)
		{
			var hotelList = await _hotelService.GetAllHotelAsync(pageNumber);
			return View(hotelList);
		}     


		// Manager Listing Controller
		public IActionResult Manager()
		{
			return View();  
		}

		public IActionResult Transactions()
		{
			return View();  
		}


		public IActionResult HotelRooms()
		{
			return View();
		}
		public IActionResult AllManagers()
		{
			return View();
		}


		public IActionResult AllUsers()
		{
			return View();
		}
		public async Task<IActionResult> HotelDetails(string hotelId)
		{
			var singleHotel = await _hotelService.GetHotelById(hotelId);
			ViewData["GetHotel"] = singleHotel;
			return View();
		}

	}
}
