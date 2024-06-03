using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using WebTest.VWModels.Category;
using Microsoft.EntityFrameworkCore;
namespace WebTest.Controllers
{
    public class CategoriesController : BaseController
    {
        public CategoriesController(PharmacyContext context) : base(context)
        {
        }
        override
        public async Task<IActionResult> Index()
        {
            return _context.Categories != null ?
                View(await _context.Categories.ToListAsync()) :
                Problem("Entity set 'PharmacyContext.Categories'  is null.");
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] CreateCategoryRequest category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Category()
                {
                    Name = category.Name,
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                            .FirstOrDefaultAsync(f => f.Id == id);
            if (category is null)
            {
                return NotFound();
            }
            var filteredMedicines = _context.Medicines
                                    .Where(i => i.CategoryId == id)
                                    .Include(e => e.Category)
                                    .Include(e => e.ActiveSubstance)
                                    .ToList();

            var medicines = new VWModels.Category.ShowMedicinesResponse();

            medicines.FactoryId = (int)id;
            medicines.Name = category.Name;
            medicines.Medicines = filteredMedicines
                                    .Where(i => i.CategoryId == id)
                                    .Select(i => new MedicineInfo()
                                    {
                                        Id = i.Id,
                                        Name = i.Name,
                                        TradeName = i.TradeName,
                                        InStock = i.InStock,
                                        ActiveSubstance = i.ActiveSubstance.Name,
                                        Factory = i.Category.Name,
                                        Dose = i.Dose,

                                    }).ToList();

            ViewBag.Medicines = medicines;
            return View(medicines);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                            .FirstOrDefaultAsync(f => f.Id == id);
            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (_context.Categories is null)
            {
                return Problem("Entity set 'PharmacyContext.Categories  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category is not null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            var categoryModel = new EditCategoryRequest()
            {
                Name = category.Name,
            };

            return View(categoryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, [Bind("Name")] EditCategoryRequest category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(new Category()
                    {
                        Id = id,
                        Name = category.Name,
                    });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
    }
}
