using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaController(IVillaService villaService, IMapper mapper) : Controller
{

    public async Task<IActionResult> IndexVilla()
    {
        IEnumerable<VillaDTO> villas = [];
        var token = HttpContext.Session.GetString(SD.SessionToken);
        var response = await villaService.GetAllAsync<APIResponse>(token);
        if (response != null && response.IsSuccess)
        {
            villas = JsonConvert.DeserializeObject<IEnumerable<VillaDTO>>(Convert.ToString(response.Result));
        }

        return View(villas);
    }

    [Authorize(Roles = "admin")]
    public IActionResult CreateVilla()
    {
        return View();
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVilla(VillaCreateDTO villa)
    {
        if (ModelState.IsValid)
        {
            var token = HttpContext.Session.GetString(SD.SessionToken);
            var response = await villaService.CreateAsync<APIResponse>(villa, token);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa created Successfully";
                return RedirectToAction(nameof(IndexVilla));
            }
        }
        TempData["error"] = "Error creating Villa";
        return View(villa);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateVilla(int villaId)
    {
        var token = HttpContext.Session.GetString(SD.SessionToken);
        var request = await villaService.GetAsync<APIResponse>(villaId, token);
        if (request != null && request.IsSuccess)
        {
            var villa = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(request.Result));
            return View(mapper.Map<VillaUpdateDTO>(villa));
        }
        return NotFound();
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVilla(VillaUpdateDTO villa)
    {
        if (ModelState.IsValid)
        {
            var token = HttpContext.Session.GetString(SD.SessionToken);
            var response = await villaService.UpdateAsync<APIResponse>(villa, token);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Updated Successfully";
                return RedirectToAction(nameof(IndexVilla));
            }
        }
        TempData["error"] = "Error Updating Villa";
        return View(villa);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteVilla(int villaId)
    {
        var token = HttpContext.Session.GetString(SD.SessionToken);
        var request = await villaService.GetAsync<APIResponse>(villaId, token);
        if (request != null && request.IsSuccess)
        {
            var villa = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(request.Result));
            return View(villa);
        }
        return NotFound();
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVilla(VillaDTO villa)
    {
        var token = HttpContext.Session.GetString(SD.SessionToken);
        var response = await villaService.DeleteAsync<APIResponse>(villa.Id, token);
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Villa Deleted Successfully";
            return RedirectToAction(nameof(IndexVilla));
        }
        TempData["error"] = "Error Deleting Villa";
        return View(villa);
    }
}
