using Microsoft.AspNetCore.Mvc;
using ArtikujManager.Web.Models;
using ArtikujManager.Web.Services;

namespace ArtikujManager.Web.Controllers
{
    public class ArtikujtController : Controller
    {
        private readonly IApiService _apiService;

        public ArtikujtController(IApiService apiService)
        {
            _apiService = apiService;
        }

        private bool IsAuthenticated => !string.IsNullOrEmpty(HttpContext.Session.GetString("Token"));
        private string? GetToken() => HttpContext.Session.GetString("Token");
        private string? GetUserRole() => HttpContext.Session.GetString("UserRole");
        private bool IsAdmin => GetUserRole() == "Admin";
        private bool CanAccessArticles => IsAuthenticated && (GetUserRole() == "Admin" || GetUserRole() == "Operator");

        public async Task<IActionResult> Index(string? search)
        {
            if (!CanAccessArticles)
            {
                TempData["ErrorMessage"] = "You must be logged in to access articles.";
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var token = GetToken();
                var queryParams = new List<string>();
                
                if (!string.IsNullOrEmpty(search))
                    queryParams.Add($"search={Uri.EscapeDataString(search)}");

                var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
                
                var artikujt = await _apiService.GetAsync<List<ArtikujViewModel>>($"/api/artikujt{queryString}", token);

                var viewModel = new ArtikujListViewModel
                {
                    Artikujt = artikujt ?? new List<ArtikujViewModel>(),
                    SearchTerm = search,
                    CurrentUserRole = GetUserRole()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ka ndodhur një gabim gjatë marrjes së artikujve.";
                return View(new ArtikujListViewModel { CurrentUserRole = GetUserRole() });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!CanAccessArticles)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var token = GetToken();
                var artikuj = await _apiService.GetAsync<ArtikujViewModel>($"/api/artikujt/{id}", token);
                
                if (artikuj == null)
                {
                    TempData["ErrorMessage"] = "Artikulli nuk u gjet.";
                    return RedirectToAction("Index");
                }

                return View(artikuj);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ka ndodhur një gabim gjatë marrjes së detajeve të artikullit.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!CanAccessArticles)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateArtikujViewModel model)
        {
            if (!CanAccessArticles)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var token = GetToken();
                var createData = new
                {
                    Emri = model.Emri,
                    Cmimi = model.Cmimi,
                    Njesia = model.Njesia,
                    Barkodi = model.Barkodi,
                    DataSkadences = model.DataSkadences,
                    Lloj = model.Lloj,
                    KaTvsh = model.KaTvsh,
                    Tipi = model.Tipi
                };

                var result = await _apiService.PostAsync<ArtikujViewModel>("/api/artikujt", createData, token);

                if (result != null)
                {
                    TempData["SuccessMessage"] = "Artikulli u shtua me sukses!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Ka ndodhur një gabim gjatë shtimit të artikullit.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ka ndodhur nje gabim gjate komunikimit me serverin.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin)
            {
                TempData["ErrorMessage"] = "Vetem administratoret mund te perditesojne artikujt.";
                return RedirectToAction("Index");
            }

            try
            {
                var token = GetToken();
                var artikuj = await _apiService.GetAsync<ArtikujViewModel>($"/api/artikujt/{id}", token);

                if (artikuj == null)
                {
                    TempData["ErrorMessage"] = "Artikulli nuk u gjet.";
                    return RedirectToAction("Index");
                }

                var editModel = new CreateArtikujViewModel
                {
                    Emri = artikuj.Emri,
                    Cmimi = artikuj.Cmimi,
                    Njesia = artikuj.Njesia,
                    Barkodi = artikuj.Barkodi,
                    DataSkadences = artikuj.DataSkadences,
                    Lloj = artikuj.Lloj,
                    KaTvsh = artikuj.KaTvsh,
                    Tipi = artikuj.Tipi
                };

                return View(editModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ka ndodhur nje gabim gjate marrjes se detajeve te artikullit.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateArtikujViewModel model)
        {
            if (!IsAdmin)
            {
                TempData["ErrorMessage"] = "Vetem administratoret mund te perditesojne artikujt.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var token = GetToken();
                var updateData = new
                {
                    Emri = model.Emri,
                    Cmimi = model.Cmimi,
                    Njesia = model.Njesia,
                    Barkodi = model.Barkodi,
                    DataSkadences = model.DataSkadences,
                    Lloj = model.Lloj,
                    KaTvsh = model.KaTvsh,
                    Tipi = model.Tipi
                };

                var result = await _apiService.PutAsync<object>($"/api/artikujt/{id}", updateData, token);

                TempData["SuccessMessage"] = "Artikulli u përditësua me sukses!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ka ndodhur një gabim gjatë përditësimit të artikullit.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin)
            {
                TempData["ErrorMessage"] = "Vetëm administratorët mund të fshijnë artikujt.";
                return RedirectToAction("Index");
            }

            try
            {
                var token = GetToken();
                var success = await _apiService.DeleteAsync($"/api/artikujt/{id}", token);

                if (success)
                {
                    TempData["SuccessMessage"] = "Artikulli u fshi me sukses!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ka ndodhur nje gabim gjate fshirjes se artikullit.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ka ndodhur nje gabim gjate komunikimit me serverin.";
            }

            return RedirectToAction("Index");
        }
    }
}