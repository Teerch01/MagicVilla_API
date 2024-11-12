using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.VM;

public class VillaNumberDeleteVM
{
    public VillaNumberUpdateDTO VillaNumber { get; set; } = new();
    [ValidateNever]
    public IEnumerable<SelectListItem> VillaList { get; set; }
}
