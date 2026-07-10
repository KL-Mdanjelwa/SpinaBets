using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpinaBets.Models;
using SpinaBets.ViewModels;
using System.Diagnostics;

namespace SpinaBets.Controllers
{
   
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Customer")]
        public IActionResult Customer()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Contact()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string phoneNumber = "27683566154"; 

            string message =
                $"Name: {model.Name}%0A" +
                $"Email: {model.Email}%0A" +
                $"Subject: {model.Subject}%0A%0A" +
                $"{model.Message}";

            return Redirect($"https://wa.me/{phoneNumber}?text={message}");
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
