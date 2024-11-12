using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.VM;

public class VillaNumberCreateVM
{
    public VillaNumberCreateDTO VillaNumber { get; set; } = new();
    [ValidateNever]
    public IEnumerable<SelectListItem> VillaList { get; set; }
}
