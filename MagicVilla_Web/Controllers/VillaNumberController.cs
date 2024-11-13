using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaNumberController(IVillaNumberService villaNumberService, IVillaService villaService, IMapper mapper) : Controller
{
    public async Task<IActionResult> IndexVillaNumber()
    {
        IEnumerable<VillaNumberDTO> villas = [];
        var token = HttpContext.Session.GetString(SD.SessionToken);
        var response = await villaNumberService.GetAllAsync<APIResponse>(token);
        if (response != null && response.IsSuccess)
        {
            villas = JsonConvert.DeserializeObject<IEnumerable<VillaNumberDTO>>(Convert.ToString(response.Result));
        }

        return View(villas);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateVillaNumber()
    {
        var token = HttpContext.Session.GetString(SD.SessionToken);
        VillaNumberCreateVM villaNumberVM = new();
        var response = await villaService.GetAllAsync<APIResponse>(token);
        if (response != null && response.IsSuccess)
        {
            villaNumberVM.VillaList = JsonConvert.DeserializeObject<IEnumerable<VillaDTO>>(Convert.ToString(response.Result)).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }
        return View(villaNumberVM);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNumber([Bind(Prefix = "VillaNumber")] VillaNumberCreateDTO villa)
    {
        var token = HttpContext.Session.GetString(SD.SessionToken);
        if (ModelState.IsValid)
        {
            var response = await villaNumberService.CreateAsync<APIResponse>(villa, token);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Number created Successfully";
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            else
            {
                if (response.ErrorMessages.Count() > 0)
                {
                    TempData["error"] = "Error creating Villa Number";
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                }
            }
        }
        VillaNumberCreateVM villaNumberVM = new();
        var res = await villaService.GetAllAsync<APIResponse>(token);
        if (res != null && res.IsSuccess)
        {
            villaNumberVM.VillaList = JsonConvert.DeserializeObject<IEnumerable<VillaDTO>>(Convert.ToString(res.Result)).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }
        return View(villaNumberVM);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateVillaNumber(int villaNo)
    {
        var token = HttpContext.Session.GetString(SD.SessionToken);
        VillaNumberUpdateVM villaNumberVM = new();
        var villaResponse = await villaService.GetAllAsync<APIResponse>(token);
        var villaNumberResponse = await villaNumberService.GetAsync<APIResponse>(villaNo, token);
        if ((villaResponse != null && villaResponse.IsSuccess) && (villaNumberResponse != null && villaNumberResponse.IsSuccess))
        {
            villaNumberVM.VillaList = JsonConvert.DeserializeObject<IEnumerable<VillaDTO>>(Convert.ToString(villaResponse.Result)).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            villaNumberVM.VillaNumber = JsonConvert.DeserializeObject<VillaNumberUpdateDTO>(Convert.ToString(villaNumberResponse.Result));
        }
        return View(villaNumberVM);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVillaNumber([Bind(Prefix = "VillaNumber")] VillaNumberUpdateDTO villa)
    {
        var token = HttpContext.Session.GetString(SD.SessionToken);
        if (ModelState.IsValid)
        {
            var response = await villaNumberService.UpdateAsync<APIResponse>(villa, token);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Number Updated Successfully";
                return RedirectToAction(nameof(IndexVillaNumber));
            }
        }

        VillaNumberUpdateVM villaNumberVM = new();
        var villaResponse = await villaService.GetAllAsync<APIResponse>(token);
        var villaNumberResponse = await villaNumberService.GetAsync<APIResponse>(villa.VillaNo, token);
        if ((villaResponse != null && villaResponse.IsSuccess) && (villaNumberResponse != null && villaNumberResponse.IsSuccess))
        {
            villaNumberVM.VillaList = JsonConvert.DeserializeObject<IEnumerable<VillaDTO>>(Convert.ToString(villaResponse.Result)).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            villaNumberVM.VillaNumber = JsonConvert.DeserializeObject<VillaNumberUpdateDTO>(Convert.ToString(villaNumberResponse.Result));
        }

        TempData["error"] = "Error Updating Villa";
        return View(villaNumberVM);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteVillaNumber(int villaNo)
    {
        var token = HttpContext.Session.GetString(SD.SessionToken);
        VillaNumberDeleteVM villaNumberVM = new();
        var villaResponse = await villaService.GetAllAsync<APIResponse>(token);
        var villaNumberResponse = await villaNumberService.GetAsync<APIResponse>(villaNo, token);
        if ((villaResponse != null && villaResponse.IsSuccess) && (villaNumberResponse != null && villaNumberResponse.IsSuccess))
        {
            villaNumberVM.VillaList = JsonConvert.DeserializeObject<IEnumerable<VillaDTO>>(Convert.ToString(villaResponse.Result)).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            villaNumberVM.VillaNumber = JsonConvert.DeserializeObject<VillaNumberUpdateDTO>(Convert.ToString(villaNumberResponse.Result));
        }
        return View(villaNumberVM);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVillaNumber([Bind(Prefix = "VillaNumber")] VillaNumberUpdateDTO villa)
    {
        var token = HttpContext.Session.GetString(SD.SessionToken);

        var response = await villaNumberService.DeleteAsync<APIResponse>(villa.VillaNo, token);
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Villa Number Deleted Successfully";
            return RedirectToAction(nameof(IndexVillaNumber));
        }
        TempData["error"] = "Error Deleting Villa";
        return View(villa);
    }
}
