using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoClients;
using WebAppDemoMVC.Data;
using WebAppDemoMVC.Models;

namespace WebAppDemoMVC.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrganizationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Organizations (startwith,endwith,contains)
        public async Task<IActionResult> Index(string? filter,DirectionSearch directionSearch,string fieldName="Name")
        {
            if (_context.Organizations != null)
            {               
                var model = new OrganizationIndexViewModel {Filter = filter ?? string.Empty, DirectionSearch = directionSearch,FieldName = fieldName};

                foreach (var field in typeof(Organization).GetProperties())
                {
                    if (field.PropertyType == typeof(string))
                        model.AvaliableFields.Add(field.Name);

                }

                 var query = _context.Organizations.AsQueryable();
                if (filter != null)
                {
                    switch (directionSearch)
                    {
                        case DirectionSearch.StartWith:
                            query = query.Where(s =>  typeof(Organization).GetProperty(fieldName).GetValue(s).ToString().StartsWith(filter));
                            break;
                        case DirectionSearch.EndWith:
                             query = query.Where(s => s.Name.EndsWith(filter));
                            break;
                        case DirectionSearch.Contains:
                            query = query.Where(s => s.Name.Contains(filter));
                            break;

                    }
                }

                model.Organizations = await query.ToListAsync();
                var view = View(model);
                return view;
            }
            return   Problem("Entity set 'ApplicationDbContext.Organizations'  is null.");
                            
        }

        // GET: Organizations/Detail/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }
        // Base address https://localhost:5000
        // GET: Organizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Organization organization)
        {
            //Bind("Id,Name,FullName,Created")] 
            if (ModelState.IsValid)
            {
                if (organization.Created<DateTime.Parse("01.01.2023") || organization.Created>DateTime.Parse("30.01.2023"))
                {
                    ModelState.AddModelError("Created","Data is invalid");
                    return View(organization);
                }
                _context.Add(organization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        // GET: Organizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                return NotFound();
            }
            return View(organization);
        }

        // POST: Organizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FullName")] Organization organization)
        {
            if (id != organization.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        // GET: Organizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Organizations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Organizations'  is null.");
            }
            var organization = await _context.Organizations.FindAsync(id);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(int id)
        {
          return (_context.Organizations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
