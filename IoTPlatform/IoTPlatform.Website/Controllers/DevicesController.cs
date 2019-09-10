using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IoTPlatform.Data.Device;
using IoTPlatform.Data.EntityFramework;
using System.Security.Claims;
using SmartBreadcrumbs.Attributes;

namespace IoTPlatform.Website.Controllers
{
    public class DevicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DevicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Devices
        [Breadcrumb("Devices")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Devices.Include(d => d.Owner).Where(d => d.OwnerId == User.FindFirstValue(ClaimTypes.NameIdentifier) || d.IsPublic);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Devices/Details/5
        [Breadcrumb("ViewData.Title")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.Nodes)
                .Include(d => d.Units)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (device == null || (device.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !device.IsPublic))
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: Devices/Create
        [Breadcrumb("ViewData.Title")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("ViewData.Title")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IsPublic")] Device device)
        {
            device.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ModelState.Clear();
            TryValidateModel(device);

            if (ModelState.IsValid)
            {
                _context.Add(device);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(device);
        }

        // GET: Devices/Edit/5
        [Breadcrumb("ViewData.Title")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices.FindAsync(id);
            if (device == null || device.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }
            
            return View(device);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("ViewData.Title")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OwnerId,Name,Description,IsPublic")] Device device)
        {
            if (id != device.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.Id))
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
            return View(device);
        }

        // GET: Devices/Delete/5
        [Breadcrumb("ViewData.Title")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (device == null || device.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Breadcrumb("ViewData.Title")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }
    }
}
