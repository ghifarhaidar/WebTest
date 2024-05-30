using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using WebTest.VWModels.Ingredient;
using Microsoft.EntityFrameworkCore;
namespace WebTest.Controllers
{
    public class IngredientsController : BaseController
    {
        public IngredientsController(PharmacyContext context) : base(context)
        {
        }
        override
         public async Task<IActionResult> Index()
        {
            return _context.Categories != null ?
                View(await _context.Ingredients.ToListAsync()) :
                Problem("Entity set 'PharmacyContext.Categories'  is null.");
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name" , "Description")] CreateIngredientRequest ingredient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Ingredient()
                {
                    Name = ingredient.Name,
                    Description = ingredient.Description,
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }

    }
}
