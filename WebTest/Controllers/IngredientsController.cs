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

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients
                            .FirstOrDefaultAsync(i => i.Id == id);
            if (ingredient is null)
            {
                return NotFound();
            }

            return View(ingredient);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients
                            .FirstOrDefaultAsync(i => i.Id == id);
            if (ingredient is null)
            {
                return NotFound();
            }

            return View(ingredient); 
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (_context.Ingredients is null)
            {
                return Problem("Entity set 'PharmacyContext.Medicines'  is null.");
            }
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient is not null)
            {
                _context.Ingredients.Remove(ingredient);
                Console.WriteLine(1);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null )
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient is null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id,[Bind("Name", "Description")] EditIngredientRequest ingredient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(new Ingredient()
                    {
                        Id = id,
                        Name = ingredient.Name,
                        Description = ingredient.Description,
                    });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }
    }
}
