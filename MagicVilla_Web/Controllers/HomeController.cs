using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class HomeController(IVillaService villaService, IMapper mapper) : Controller
    {
        public async Task<IActionResult> Index()
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
    }
}
