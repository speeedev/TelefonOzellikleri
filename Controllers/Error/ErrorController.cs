using Microsoft.AspNetCore.Mvc;
using TelefonOzellikleri.Helpers;

namespace TelefonOzellikleri.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode?}")]
        public IActionResult Index(int? statusCode)
        {
            var code = statusCode ?? 500;

            var (title, message) = code switch
            {
                404 => ("Sayfa Bulunamadı", "Aradığınız sayfa mevcut değil veya kaldırılmış olabilir."),
                403 => ("Erişim Engellendi", "Bu sayfaya erişim izniniz bulunmamaktadır."),
                500 => ("Sunucu Hatası", "Bir şeyler ters gitti. Lütfen daha sonra tekrar deneyin."),
                503 => ("Hizmet Kullanılamıyor", "Sunucu şu anda bakımda. Lütfen daha sonra tekrar deneyin."),
                _ => ("Hata", "Beklenmeyen bir hata oluştu.")
            };

            ViewData["Title"] = SeoHelper.TruncateTitle($"{title} | TelefonOzellikleri.Net");
            ViewData["StatusCode"] = code;
            ViewData["ErrorMessage"] = message;
            ViewData["Robots"] = "noindex, nofollow";

            Response.StatusCode = code;

            return View();
        }
    }
}
