using System.Diagnostics;
using Aeolian.Models;
using Aeolian.Services;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aeolian.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;

        public HomeController(ILogger<HomeController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Route("home")]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User model)
        {
            try
            {
                Console.WriteLine($"Attempting to authenticate user: {model.username}");

                User userCheck = _userService.GetUser(UserService.USER_TYPE.GET, UserService.GET_TYPE.USERNAME, model.username);

                if (userCheck.username == null)
                {
                    Console.WriteLine($"No use under the username {model.username}.");
                    return View(nameof(Login));
                }

                if (!Argon2.Verify(userCheck.password, model.password))
                {
                    Console.WriteLine($"Failed to authenticate user: {model.username}");
                    return View(nameof(Login));
                }

                Console.WriteLine($"Successfully authenticated user: {model.username}");
                return View("home");
            }
            catch (Exception ex)
            {
                //Maybe replace this with a logger.
                Console.WriteLine(ex.Message);
                return View(nameof(Login));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
