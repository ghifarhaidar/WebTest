using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using WebTest.VWModels.Medicine;
using WebTest.VWModels.MedicineIngredient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebTest.VWModels;

namespace WebTest.Controllers
{
    public class MedicinesController : BaseController
    {
        public MedicinesController(PharmacyContext context) : base(context)
        {
        }
        override
        public async Task<IActionResult> Index()
        {
            if(_context.Medicines is null)
                return Problem("Entity set 'PharmacyContext.Categories'  is null.");
            var medicines = await _context.Medicines
                            .Include(e => e.Factory)
                            .Include(e => e.Category)
                            .Include(e => e.ActiveSubstance)
                            .ToListAsync();
            return View(medicines);
        }
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.ActiveSubstanceId = new SelectList(_context.Ingredients, "Id", "Name");
            ViewBag.FactoryId = new SelectList(_context.Factories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Description, CategoryId, Dose, ActiveSubstanceId, InStock, FactoryId, TradeName")] CreateMedicineRequest medicine)
        {
            if (ModelState.IsValid)
            {
                var newMedicine = new Medicine()
                {
                    Name = medicine.Name,
                    Description = medicine.Description,
                    CategoryId = medicine.CategoryId,
                    Dose = medicine.Dose,
                    ActiveSubstanceId = medicine.ActiveSubstanceId,
                    InStock = medicine.InStock,
                    FactoryId = medicine.FactoryId,
                    TradeName = medicine.TradeName,
                };
                _context.Add(newMedicine);
                _context.Add(new MedicineIngredient()
                {
                    MedicineId = newMedicine.Id,
                    IngredientId = newMedicine.ActiveSubstanceId,
                    Ratio = 1,
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", medicine.CategoryId);
            ViewBag.ActiveSubstanceId = new SelectList(_context.Ingredients, "Id", "Name", medicine.ActiveSubstanceId);
            ViewBag.FactoryId = new SelectList(_context.Factories, "Id", "Name", medicine.FactoryId);

            return View(medicine);
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Medicines == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.ActiveSubstanceId = new SelectList(_context.Ingredients, "Id", "Name");
            ViewBag.FactoryId = new SelectList(_context.Factories, "Id", "Name");
            return View(medicine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, [Bind("Name, Description, CategoryId, Dose, ActiveSubstanceId, InStock, FactoryId, TradeName")] EditMedicineRequest medicine)
        {



            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(new Medicine()
                    {
                        Id = id,
                        Name = medicine.Name,
                        Description = medicine.Description,
                        CategoryId = medicine.CategoryId,
                        Dose = medicine.Dose,
                        ActiveSubstanceId = medicine.ActiveSubstanceId,
                        InStock = medicine.InStock,
                        FactoryId = medicine.FactoryId,
                        TradeName  = medicine.TradeName,
                    });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                return RedirectToAction(nameof(Index));
            }


            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", medicine.CategoryId);
            ViewBag.ActiveSubstanceId = new SelectList(_context.Ingredients, "Id", "Name", medicine.ActiveSubstanceId);
            ViewBag.FactoryId = new SelectList(_context.Factories, "Id", "Name", medicine.FactoryId);
            return View(medicine);
        }
        public async Task<IActionResult> EditIngredients(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Medicine? medicine = await _context.Medicines.FindAsync(id);
            if (medicine is null)
            {
                return NotFound();
            }
            var filteredIngredients =  _context.MedicineIngredients
                                    .Where(i => i.MedicineId == id)
                                    .Include(e => e.Ingredient)
                                    .ToList();

            var ingredients = new VWModels.MedicineIngredient.ShowIngredientsResponse();
            
            ingredients.MedicineId = (int)id;
            ingredients.Ingredients = filteredIngredients.Select(i => new IngredientInfo()
            {
                Id = i.Ingredient.Id,
                Name = i.Ingredient.Name,
                Ratio = i.Ratio
            }).ToList();

            ViewBag.FilteredIngredients = new SelectList(filteredIngredients, "Id", "Name");
            ViewBag.Ingredients = new SelectList(_context.Ingredients, "Id", "Name");

            return View(ingredients);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddIngredient([FromRoute]int id,[Bind("IngredientId","Ratio")] AddIngredientToMedicineRequest ingredientToMedicine)
        {

            if (ModelState.IsValid)
            {
                _context.Add(new MedicineIngredient()
                {
                    IngredientId = ingredientToMedicine.IngredientId,
                    MedicineId = id,
                    Ratio = ingredientToMedicine.Ratio,
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EditIngredients), new { id = id });
            }

            return RedirectToAction(nameof(EditIngredients), new { id = id });

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines
                            .Include(e => e.Factory)
                            .Include(e => e.Category)
                            .Include(e => e.ActiveSubstance)
                            .FirstOrDefaultAsync(m => m.Id == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Medicines is null)
            {
                return Problem("Entity set 'PharmacyContext.Medicines'  is null.");
            }
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine is not null)
            {
                _context.Medicines.Remove(medicine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines
                            .Include(e => e.Factory)
                            .Include(e => e.Category)
                            .Include(e => e.ActiveSubstance)
                            .FirstOrDefaultAsync(m => m.Id == id);
            if (medicine is null)
            {
                return NotFound();
            }

            return View(medicine);
        }
    }
}
