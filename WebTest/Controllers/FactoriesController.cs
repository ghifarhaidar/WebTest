using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using WebTest.VWModels.Factory;
using Microsoft.EntityFrameworkCore;
namespace WebTest.Controllers
{
    public class FactoriesController : BaseController
    {
        public FactoriesController(PharmacyContext context) : base(context)
        {
        }
        override
        public async Task<IActionResult> Index()
        {
            return _context.Factories != null ?
                View(await _context.Factories.ToListAsync()) :
                Problem("Entity set 'PharmacyContext.Categories'  is null.");
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] CreateFactoryRequest factory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Factory()
                {
                    Name = factory.Name,
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(factory);
        }
    }
}
