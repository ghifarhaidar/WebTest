using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using WebTest.VWModels.Patient;
using Microsoft.EntityFrameworkCore;

namespace WebTest.Controllers
{
    public class PatientsController : BaseController
    {
        public PatientsController(PharmacyContext context) : base(context)
        {
        }
        override
         public async Task<IActionResult> Index()
        {
            if (_context.Patients is null)
                return Problem("Entity set 'PharmacyContext.Patients'  is null.");
            var patients = await _context.Patients.ToListAsync();
            return View(patients);
        }


        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePatientRequest patien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Patient()
                {
                    FirstName = patien.FirstName,
                    LastName = patien.LastName,  
                    Address = patien.Address,
                    PhoneNumber = patien.PhoneNumber,
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(patien);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            var patientModel = new EditPatientRequest
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber,
            };

            return View(patientModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPatientRequest patien)
        {
            

            if (ModelState.IsValid)
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient is null)
                {
                    return NotFound();
                }
                _context.Update(new Patient()
                {
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Address = patient.Address,
                    PhoneNumber = patient.PhoneNumber,
                });

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(patien);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient is null)
            {
                return NotFound();
            }

            return View(patient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Patients is null)
            {
                return Problem("Entity set 'PharmacyContext.Patients'  is null.");
            }
            var patient = await _context.Patients.FindAsync(id);
            if (patient is not null)
            {
                _context.Patients.Remove(patient);
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

            var patient = await _context.Patients
                .FindAsync(id);
            if (patient is null)
            {
                return NotFound();
            }
            var filteredDescriptions = _context.Descriptions
                .Where(d=> d.PatientId == id);

            var descriptions = new VWModels.Patient.ShowDescriptionsResponse();

            descriptions.PatientId = (int)id;
            descriptions.Name = patient.FirstName + " " + patient.LastName;
            descriptions.Descriptions = filteredDescriptions.Select(i => new DescriptionInfo()
            {
                Id = i.Id,
                Name = patient.FirstName + " " + patient.LastName,
                PatientId = i.Id,
                Description1 = i.Description1,
            }).ToList();
            ViewBag.Descriptions = descriptions;

            return View(patient);
        }
    }
}
