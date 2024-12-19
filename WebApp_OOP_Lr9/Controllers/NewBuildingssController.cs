using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_OOP_Lr9.DataBase;
using WebApp_OOP_Lr9.Servises;

namespace WebApp_OOP_Lr9.Controllers
{
    public class NewBuildingssController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly INewBuildingsService _NewBuildingsService;

        public NewBuildingssController(INewBuildingsService service)
        {
            _NewBuildingsService = service;
        }
   
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var buildings = await _NewBuildingsService.GetAllNewBuildingAsync();
            return View(buildings);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var newBuilding = await _NewBuildingsService.GetNewBuildingAsync(id);
            if (newBuilding == null)
            {
                return NotFound();
            }

            return View(newBuilding);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Caption, string Address)
        {
            if (ModelState.IsValid)
            {
                var result = await _NewBuildingsService.CreateNewBuildingAsync(Caption, Address);

                if (result)
                {
                    return RedirectToAction("Create");
                }

                ModelState.AddModelError(string.Empty, "Не вдалося додати новий запис");
            }

            return View();  
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var newBuilding = await _NewBuildingsService.GetNewBuildingAsync(id);
            if (newBuilding == null)
            {
                return NotFound();
            }
            return View(newBuilding);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string caption, string address)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _NewBuildingsService.UpdateNewBuildingAsync(id, caption, address);
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var newBuilding = await _NewBuildingsService.GetNewBuildingAsync(id);
            if (newBuilding == null)
            {
                return NotFound();
            }
            return View(newBuilding);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _NewBuildingsService.DeleteNewBuildingAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
