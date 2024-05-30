using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
namespace WebTest.Controllers
{
    public class DescriptionsController : BaseController
    {
        public DescriptionsController(PharmacyContext context) : base(context)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
