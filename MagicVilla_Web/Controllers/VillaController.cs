using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaController(IVillaService villaService, IMapper mapper) : Controller
{
    public async Task<IActionResult> IndexVilla()
    {
        IEnumerable<VillaDTO> villas = [];

        var response = await villaService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            villas = JsonConvert.DeserializeObject<IEnumerable<VillaDTO>>(Convert.ToString(response.Result));
        }

        return View(villas);
    }

    public IActionResult CreateVilla()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVilla(VillaCreateDTO villa)
    {
        if (ModelState.IsValid)
        {
            var response = await villaService.CreateAsync<APIResponse>(villa);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa created Successfully";
                return RedirectToAction(nameof(IndexVilla));
            }
        }
        TempData["error"] = "Error creating Villa";
        return View(villa);
    }

    public async Task<IActionResult> UpdateVilla(int villaId)
    {
        var request = await villaService.GetAsync<APIResponse>(villaId);
        if (request != null && request.IsSuccess)
        {
            var villa = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(request.Result));
            return View(mapper.Map<VillaUpdateDTO>(villa));
        }
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVilla(VillaUpdateDTO villa)
    {
        if (ModelState.IsValid)
        {
            var response = await villaService.UpdateAsync<APIResponse>(villa);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Updated Successfully";
                return RedirectToAction(nameof(IndexVilla));
            }
        }
        TempData["error"] = "Error Updating Villa";
        return View(villa);
    }

    public async Task<IActionResult> DeleteVilla(int villaId)
    {
        var request = await villaService.GetAsync<APIResponse>(villaId);
        if (request != null && request.IsSuccess)
        {
            var villa = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(request.Result));
            return View(villa);
        }
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVilla(VillaDTO villa)
    {

        var response = await villaService.DeleteAsync<APIResponse>(villa.Id);
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Villa Deleted Successfully";
            return RedirectToAction(nameof(IndexVilla));
        }
        TempData["error"] = "Error Deleting Villa";
        return View(villa);
    }
}
