using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace WebAppDemoRazorPages.Controllers
{
    public class ToolsController : Controller
    {
        
        [Route("/")]
        [HttpPost]
        public IActionResult SetLanguage(string culture,string returnUrl) // 20200301
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1)});
            return LocalRedirect(returnUrl);
        }
    }
}
