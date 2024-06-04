using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using WebTest.VWModels.Description;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Create()
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var description = await _context.Descriptions.FindAsync(id);
            if (description is null)
            {
                return NotFound();
            }
            var descriptionModel = new EditDescriptionRequest()
            {
                Id = (int)id,
                Name = description.Name,
                Description1 = description.Description1,
                PatientId = description.PatientId,
            };

            return View(descriptionModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("Name", "PatientId", "Description1")] EditDescriptionRequest description)
        {
            if (ModelState.IsValid)
            {
                _context.Update(new Description()
                {
                    Id = id,
                    Name = description.Name,
                    PatientId = description.PatientId,
                    Description1 = description.Description1,
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Patients", new { id = description.PatientId });
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
                            .Include(i=>i.Patient)
                            .FirstOrDefaultAsync(i => i.Id == id);
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
            medicines.Name = description.Name;
            medicines.Description1 = description.Description1;
            medicines.Medicines = filteredMedicines
                                    .Select(i => new MedicineInfo()
                                    {
                                        Id = i.Id,
                                        Name = i.Medicine.Name,
                                        TradeName = i.Medicine.TradeName,
                                        InStock = i.Medicine.InStock,
                                        Dose = i.Medicine.Dose,
                                        Factory = i.Medicine.Factory.Name,
                                        Count = i.Count,
                                    }).ToList();

            ViewBag.Description = description;
            return View(medicines);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var description = await _context.Descriptions
                            .Include(i => i.Patient)
                            .FirstOrDefaultAsync(i => i.Id == id);
            if (description is null)
            {
                return NotFound();
            }

            return View(description);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (_context.Descriptions is null)
            {
                return Problem("Entity set 'PharmacyContext.Descriptions  is null.");
            }
            var description = await _context.Descriptions
                            .Include(i => i.Patient)
                            .FirstOrDefaultAsync(i => i.Id == id);
            if (description is not null)
            {
                _context.Descriptions.Remove(description);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> EditMedicines(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Description? description = await _context.Descriptions.FindAsync(id);
            if (description is null)
            {
                return NotFound();
            }
            var filteredMedicines = _context.DescriptionMedicines
                                    .Where(i => i.DescriptionId == id)
                                    .Include(e => e.Medicine)
                                    .Include(e=>e.Medicine.Factory)
                                    .ToList();

            var medicines = new VWModels.DescriptionMedicine.ShowMedicinesResponse();

            medicines.DescriptionId = (int)id;
            medicines.Medicines = filteredMedicines.Select(i => new VWModels.Description.MedicineInfo()
            {
                Id = i.Medicine.Id,
                Name = i.Medicine.Name,
                Dose = i.Medicine.Dose,
                TradeName = i.Medicine.TradeName,
                Count = i.Count,
                Factory = i.Medicine.Factory.Name,
                InStock = i.Medicine.InStock,
            }).ToList();
            ViewBag.Medicines = new SelectList(_context.Medicines, "Id", "Name");

            return View(medicines);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMedicine([FromRoute] int id, [Bind("MedicineId", "Count")] VWModels.DescriptionMedicine.AddMedicineToDescriptionRequest medicine )
        {

            if (ModelState.IsValid)
            {
                _context.Add(new DescriptionMedicine()
                {
                    DescriptionId = id,
                    MedicineId = medicine.MedicineId,
                    Count = medicine.Count,
                });

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EditMedicines), new { id = id });
            }
            return RedirectToAction(nameof(EditMedicines), new { id = id });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMedicine(int medicineId, int descriptionId)
        {

            if (ModelState.IsValid)
            {
                var descriptionmedicine = await _context.DescriptionMedicines
                            .FirstOrDefaultAsync(m => m.MedicineId == medicineId && m.DescriptionId == descriptionId);
                if (descriptionmedicine is not null)
                {
                    _context.DescriptionMedicines.Remove(descriptionmedicine);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EditMedicines), new { id = descriptionId });
            }

            return RedirectToAction(nameof(EditMedicines), new { id = descriptionId });

        }
        public async Task<IActionResult> EditMedicine(int medicineId, int descriptionId)
        {
            var descriptionmedicine = await _context.DescriptionMedicines
                                    .Where(m => m.MedicineId == medicineId && m.DescriptionId == descriptionId)
                                    .Select(i => new VWModels.DescriptionMedicine.EditMedicineinDescriptionRequest()
                                    {
                                        DescriptionMedicineId = i.Id,
                                        DescriptionId = descriptionId,
                                        MedicineId = medicineId,
                                        Medicine = new VWModels.DescriptionMedicine.MedicineInfo()
                                        {
                                            Count = i.Count,
                                            
                                        }
                                    }).ToListAsync();

            Console.WriteLine(descriptionmedicine.Count);
            if (descriptionmedicine is null || descriptionmedicine.Count == 0)
            {
                return NotFound();
            }



            return View(descriptionmedicine[0]);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMedicine(VWModels.DescriptionMedicine.EditMedicineinDescriptionRequest model)
        {

            if (ModelState.IsValid)
            {

                var descriptionmedicine = new DescriptionMedicine
                {
                    Id = model.DescriptionMedicineId,
                    DescriptionId = model.DescriptionId,
                    MedicineId = model.MedicineId,
                    Count = model.Medicine.Count,
                };
                if (descriptionmedicine is not null)
                {
                    _context.Update(descriptionmedicine);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EditMedicines), new { id = model.DescriptionId });
            }

            return RedirectToAction(nameof(EditMedicine), new { medicineId = model.MedicineId, descriptionId = model.DescriptionId });

        }
    }
}
