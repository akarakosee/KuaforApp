using KuaforApp.Models.AI;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace KuaforApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public HomeController(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public IActionResult Index()
        {
            // Burada işletme hakkında bilgilendirme metni girebilirsiniz
            ViewData["Info"] = "Bu işletme 2000 yılından beri hizmet vermektedir...";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetHairSuggestions(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                ModelState.AddModelError("", "Lütfen bir fotoğraf yükleyiniz.");
                return RedirectToAction("Index");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            byte[] imageData = await System.IO.File.ReadAllBytesAsync(filePath);
            string base64Image = Convert.ToBase64String(imageData);

            var endpoint = _config["AIService:Endpoint"];
            var apiKey = _config["AIService:ApiKey"];

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var requestModel = new AIRequestModel { ImageBase64 = base64Image };
            var response = await client.PostAsJsonAsync(endpoint, requestModel);

            if (response.IsSuccessStatusCode)
            {
                var aiResult = await response.Content.ReadFromJsonAsync<AIResponseModel>();
                ViewData["SuggestedStyle"] = aiResult?.SuggestedStyle;
                ViewData["SuggestedColor"] = aiResult?.SuggestedColor;
            }
            else
            {
                ViewData["Error"] = "Öneri alınamadı. Lütfen daha sonra tekrar deneyiniz.";
            }

            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
