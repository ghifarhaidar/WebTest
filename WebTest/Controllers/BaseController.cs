using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
namespace WebTest.Controllers
{
    public class BaseController : Controller
    {
        protected readonly PharmacyContext _context;
        public BaseController(PharmacyContext context)
        {
            _context = context;
        }
        public virtual async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
