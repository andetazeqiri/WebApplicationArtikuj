using Microsoft.AspNetCore.Mvc;
using ArtikujManager.Web.Models;
using ArtikujManager.Web.Services;

namespace ArtikujManager.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiService _apiService;

        public AuthController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            
            if (HttpContext.Session.GetString("Token") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var loginData = new
                {
                    email = model.Email,
                    password = model.Password
                };

                var response = await _apiService.PostAsync<AuthResponse>("/api/auth/login", loginData);

                if (response != null && response.Success)
                {
                    
                    HttpContext.Session.SetString("Token", response.Token ?? "");
                    HttpContext.Session.SetString("UserEmail", response.User?.Email ?? "");
                    HttpContext.Session.SetString("UserName", response.User?.FullName ?? "");
                    HttpContext.Session.SetString("UserRole", response.User?.Role ?? "");

                    TempData["SuccessMessage"] = "You have logged in successfully!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", response?.Message ?? "An error occurred during login.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ka ndodhur një gabim gjate komunikimit me serverin.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            
            if (HttpContext.Session.GetString("Token") != null)
            {
                return RedirectToAction("Index", "Home");
            }

           
            try
            {
                var roles = await _apiService.GetAsync<string[]>("/api/auth/roles");
                ViewBag.Roles = roles ?? new string[] { "Operator" };
            }
            catch
            {
                ViewBag.Roles = new string[] { "Operator" };
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
           
            try
            {
                var roles = await _apiService.GetAsync<string[]>("/api/auth/roles");
                ViewBag.Roles = roles ?? new string[] { "Operator" };
            }
            catch
            {
                ViewBag.Roles = new string[] { "Operator" };
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var registerData = new
                {
                    email = model.Email,
                    password = model.Password,
                    confirmPassword = model.ConfirmPassword,
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    role = model.Role
                };

                var response = await _apiService.PostAsync<AuthResponse>("/api/auth/register", registerData);

                if (response != null && response.Success)
                {
                                        HttpContext.Session.SetString("UserToken", response.Token);
                    HttpContext.Session.SetString("UserEmail", response.User?.Email ?? "");
                    HttpContext.Session.SetString("UserName", response.User?.FullName ?? "");
                    HttpContext.Session.SetString("UserRole", response.User?.Role ?? "");

                    TempData["SuccessMessage"] = "Registration completed successfully! You are logged in automatically.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", response?.Message ?? "Ka ndodhur një gabim gjatë regjistrimit.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ka ndodhur një gabim gjatë komunikimit me serverin.");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            
            TempData["SuccessMessage"] = "Jeni shkëputur me sukses!";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}