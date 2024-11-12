using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers;

[Route("api/villaNumberApi")]
[ApiController]
public class VillaNumberAPIController(IUnitOfWork unit, IMapper mapper) : ControllerBase
{
    protected APIResponse response = new();

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillaNumbers()
    {
        try
        {
            //logger.Log("Getting all villas", "error");
            var villaNumber = await unit.VillaNumberRepository.GetAllAsync(includeProperties: "Villa");
            response.Result = mapper.Map<IEnumerable<VillaNumberDTO>>(villaNumber);
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessage = [ex.ToString()];
        }
        return response;
    }

    [HttpGet("{id:int}", Name = "GetVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                //logger.Log($"get vill error with id {id}", "error");
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                return BadRequest(response);
            }
            var villaNumber = await unit.VillaNumberRepository.GetAsync(u => u.VillaNo == id, tracked: false);
            if (villaNumber == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                return NotFound(response);
            }

            response.Result = mapper.Map<VillaNumberDTO>(villaNumber);
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessage = [ex.ToString()];
        }
        return response;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
    {
        try
        {
            if (await unit.VillaNumberRepository.GetAsync(u => u.VillaNo == createDTO.VillaNo) != null)
            {
                ModelState.AddModelError("ErrorMessages", "Villa Number Already Exists");
                return BadRequest(ModelState);
            }

            if (await unit.VillaRepository.GetAsync(u => u.Id == createDTO.VillaID) == null)
            {
                ModelState.AddModelError("ErrorMessages", "Villa ID Is invalid");
                return BadRequest(ModelState);
            }
            //logger.Log("Creating new villa", "error");
            if (createDTO == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Result = BadRequest(response);
                response.IsSuccess = false;
                return BadRequest(response);
            }

            var villaNumber = mapper.Map<VillaNumber>(createDTO);

            await unit.VillaNumberRepository.CreateAsync(villaNumber);

            response.Result = mapper.Map<VillaNumberDTO>(villaNumber);
            response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVilla", new { id = villaNumber.VillaNo }, response);
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessage = [ex.ToString()];
        }
        return response;

    }

    [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                return BadRequest(response);
            }

            var villaNumber = await unit.VillaNumberRepository.GetAsync(u => u.VillaNo == id, tracked: false);
            if (villaNumber == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }

            await unit.VillaNumberRepository.RemoveAsync(villaNumber);

            response.StatusCode = HttpStatusCode.NoContent;
            response.IsSuccess = true;

            return Ok(response);
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessage = [ex.ToString()];
        }
        return response;
    }

    [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO UpdateNumberDTO)
    {
        try
        {
            if (UpdateNumberDTO == null || id != UpdateNumberDTO.VillaNo)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                return BadRequest(response);
            }

            if (await unit.VillaRepository.GetAsync(u => u.Id == UpdateNumberDTO.VillaID) == null)
            {
                ModelState.AddModelError("ErrorMessages", "Villa ID Is invalid");
                return BadRequest(ModelState);
            }

            var villaNumber = await unit.VillaNumberRepository.GetAsync(u => u.VillaNo == id, tracked: false);
            if (villaNumber == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }

            var updatedVillaNumber = mapper.Map<VillaNumber>(UpdateNumberDTO);

            await unit.VillaNumberRepository.UpdateAsync(updatedVillaNumber);

            response.StatusCode = HttpStatusCode.NoContent;
            response.IsSuccess = true;

            return Ok(response);

        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessage = [ex.ToString()];
        }
        return response;
    }

    [HttpPatch("{id:int}", Name = "UpdatePartialVillaNumber")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            return BadRequest();
        }

        var villa = await unit.VillaNumberRepository.GetAsync(u => u.VillaNo == id, tracked: false);
        if (villa == null)
        {
            return NotFound();
        }
        var updateDTO = mapper.Map<VillaNumberUpdateDTO>(villa);

        if (villa == null)
        {
            return BadRequest();
        }

        patchDTO.ApplyTo(updateDTO, ModelState);

        var patchedVilla = mapper.Map<VillaNumber>(updateDTO);

        await unit.VillaNumberRepository.UpdateAsync(patchedVilla);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }

}
