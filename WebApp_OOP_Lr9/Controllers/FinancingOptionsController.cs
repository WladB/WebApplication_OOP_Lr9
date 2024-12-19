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
    public class FinancingOptionsController : Controller
    {
        private readonly IFinancingOptionsService _financingOptionsService;
      
        public FinancingOptionsController(IFinancingOptionsService service)
        {
            _financingOptionsService = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int buildId)
        {
            var fOptions = await _financingOptionsService.GetAllFinancingOptionsAsync(buildId);
            ViewData["NewbuildingId"] = buildId;
            return View(fOptions);
        }

        [HttpGet]
        public IActionResult Create(int buildId)
        {
            var model = new FinancingOption
            {
                NewBuildingId = buildId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FinancingOption model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _financingOptionsService.CreateFinancingOptionAsync(
                        model.Caption,
                        model.Description,
                        model.NewBuildingId
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
            var fOption = await _financingOptionsService.GetFinancingOptionAsync(id);

            if (fOption == null)
            {
                return NotFound();
            }

            return View(fOption);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FinancingOption model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _financingOptionsService.UpdateFinancingOptionAsync(
                        model.Id,
                        model.Caption,
                        model.Description,
                        model.NewBuildingId
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
            var fOption = await _financingOptionsService.GetFinancingOptionAsync(id);
            if (fOption == null)
            {
                return NotFound();
            }

            return View(fOption);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var fOption = await _financingOptionsService.GetFinancingOptionAsync(id);
                if (fOption == null)
                {
                    return NotFound();
                }

                await _financingOptionsService.DeleteFinancingOptionAsync(id);

                return RedirectToAction("Index", new { buildId = fOption.NewBuildingId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
