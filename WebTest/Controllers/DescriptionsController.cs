using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using WebTest.VWModels.Description;
namespace WebTest.Controllers
{
    public class DescriptionsController : BaseController
    {
        public DescriptionsController(PharmacyContext context) : base(context)
        {
        }
        override
        public async Task<IActionResult> Index()
        {
            return _context.Descriptions != null ?
                View(await _context.Descriptions
                .Include(d => d.Patient)
                .ToListAsync()) :
                Problem("Entity set 'PharmacyContext.Descriptions'  is null.");
        }
        public IActionResult Create(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromRoute]int id, [Bind("Name", "Description1")] CreateDescriptionRequest description)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Description()
                {
                    Name = description.Name,
                    PatientId = id,
                    Description1 = description.Description1,
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("Details" , "Patients",new {id = id});
            }
            return View(description);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var description = await _context.Descriptions
                            .FindAsync(id);
            if (description is null)
            {
                return NotFound();
            }
            var filteredMedicines = _context.DescriptionMedicines
                                    .Where(i => i.DescriptionId == id)
                                    .Include(e => e.Medicine)
                                    .Include(e => e.Medicine.Factory)
                                    .Include(e => e.Medicine.Category)
                                    .Include(e => e.Medicine.ActiveSubstance)

                                    .ToList();

            var medicines = new VWModels.Description.ShowMedicinesResponse();

            medicines.DescriptionId = (int)id;
            medicines.Name = description .Name;
            medicines.Medicines = filteredMedicines
                                    .Select(i => new MedicineInfo()
                                    {
                                        Id = i.Id,
                                        Name = i.Medicine.Name,
                                        TradeName = i.Medicine.TradeName,
                                        InStock = i.Medicine.InStock,
                                        ActiveSubstance = i.Medicine.ActiveSubstance.Name,
                                        Category = i.Medicine.Category.Name,
                                        Dose = i.Medicine.Dose,
                                        Description = i.Medicine.Description,
                                        Factory = i.Medicine.Factory.Name,
                                    }).ToList();

            ViewBag.Medicines = medicines;
            return View(medicines);
        }
    }
}
