using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_OOP_Lr9.DataBase;
using WebApp_OOP_Lr9.Servises;

namespace WebApp_OOP_Lr9.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly IPropertiesService _propertiesService;

        public PropertiesController(IPropertiesService service)
        {
            _propertiesService = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int buildId)
        {
            var properties = await _propertiesService.GetAllPropertyAsync(buildId);
            ViewData["NewbuildingId"] = buildId;
            return View(properties);
        }

        [HttpGet]
        public IActionResult Create(int buildId)
        {
            var model = new Property
            {
                NewBuildingId = buildId
            };
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed(Property model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _propertiesService.CreatePropertyAsync(
                        model.CountRooms,
                        model.NewBuildingId,
                        model.Area,
                        model.Floor
                    );

                    if (result)
                    {
                        return RedirectToAction("Index", new { buildId = model.NewBuildingId });
                    }

                    ModelState.AddModelError(string.Empty, "Не вдалося додати новий запис");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var property = await _propertiesService.GetPropertyAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(Property model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _propertiesService.UpdatePropertyAsync(
                        model.Id,
                        model.CountRooms,
                        model.NewBuildingId,
                        model.Area,
                        model.Floor
                    );

                    return RedirectToAction("Index", new { buildId = model.NewBuildingId });
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var property = await _propertiesService.GetPropertyAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var property = await _propertiesService.GetPropertyAsync(id);
                if (property == null)
                {
                    return NotFound();
                }
                await _propertiesService.DeletePropertyAsync(id);
                return RedirectToAction("Index", new { buildId = property.NewBuildingId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}