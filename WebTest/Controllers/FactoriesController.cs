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
                Problem("Entity set 'PharmacyContext.Factories'  is null.");
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
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var factory = await _context.Factories
                            .FirstOrDefaultAsync(f => f.Id == id);
            if (factory is null)
            {
                return NotFound();
            }
            var filteredMedicines = _context.Medicines
                                    .Where(i => i.FactoryId == id)
                                    .Include(e => e.Category)
                                    .Include(e => e.ActiveSubstance)
                                    .ToList();

            var medicines = new VWModels.Factory.ShowMedicinesResponse();

            medicines.FactoryId = (int)id;
            medicines.Name = factory.Name;
            medicines.Medicines = filteredMedicines
                                    .Where(i => i.FactoryId == id)
                                    .Select(i => new MedicineInfo()
                                    {
                                        Id = i.Id,
                                        Name = i.Name,
                                        TradeName = i.TradeName,
                                        InStock = i.InStock,
                                        ActiveSubstance = i.ActiveSubstance.Name,
                                        Category = i.Category.Name,
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

            var factory = await _context.Factories
                            .FirstOrDefaultAsync(f => f.Id == id);
            if (factory is null)
            {
                return NotFound();
            }

            return View(factory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (_context.Factories is null)
            {
                return Problem("Entity set 'PharmacyContext.Factories  is null.");
            }
            var factory = await _context.Factories.FindAsync(id);
            if (factory is not null)
            {
                _context.Factories.Remove(factory);
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

            var factory = await _context.Factories.FindAsync(id);
            if (factory is null)
            {
                return NotFound();
            }
            var factoryModel = new EditFactoryRequest()
            {
                Name = factory.Name,
            };

            return View(factoryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, [Bind("Name")] EditFactoryRequest factory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(new Factory()
                    {
                        Id = id,
                        Name = factory.Name,
                    });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                return RedirectToAction(nameof(Index));
            }
            return View(factory);
        }
    }
}
