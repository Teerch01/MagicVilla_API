using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MagicVilla_Web.Controllers
{
    public class HomeController(IVillaService villaService, IMapper mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            IEnumerable<VillaDTO> villas = [];

            var response = await villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villas = JsonConvert.DeserializeObject<IEnumerable<VillaDTO>>(Convert.ToString(response.Result));
            }

            return View(villas);
        }
    }
}
