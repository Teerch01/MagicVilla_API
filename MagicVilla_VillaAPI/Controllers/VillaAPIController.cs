using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers;

[Route("api/villaApi")]
[ApiController]
public class VillaAPIController(IUnitOfWork unit, IMapper mapper) : ControllerBase
{
    protected APIResponse response = new();

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillas()
    {
        try
        {
            //logger.Log("Getting all villas", "error");
            var Villas = await unit.VillaRepository.GetAllAsync();
            response.Result = mapper.Map<IEnumerable<VillaDTO>>(Villas);
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

    [HttpGet("{id:int}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                //logger.Log($"get vill error with id {id}", "error");
                response.StatusCode=HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                return BadRequest(response);
            }
            var villa = await unit.VillaRepository.GetAsync(u => u.Id == id, tracked: false);
            if (villa == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                return NotFound(response);
            }

            response.Result = mapper.Map<VillaDTO>(villa);
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
    public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO)
    {
        try
        {

            //logger.Log("Creating new villa", "error");
            if (createDTO == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Result = BadRequest(response);
                response.IsSuccess = false;
                return BadRequest(response);
            }

            var villa = mapper.Map<Villa>(createDTO);

            await unit.VillaRepository.CreateAsync(villa);

            response.Result = mapper.Map<VillaDTO>(villa);
            response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVilla", new { id = villa.Id }, response);
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessage = [ex.ToString()];
        }
        return response;

    }

    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                return BadRequest(response);
            }

            var villa = await unit.VillaRepository.GetAsync(u => u.Id == id, tracked: false);
            if (villa == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }

            await unit.VillaRepository.RemoveAsync(villa);

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

    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO UpdateDTO)
    {
        try
        {
            if (UpdateDTO == null || id != UpdateDTO.Id)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                return BadRequest(response);
            }

            var villa = await unit.VillaRepository.GetAsync(u => u.Id == id, tracked: false);
            if (villa == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }

            var updatedVilla = mapper.Map<Villa>(UpdateDTO);

            await unit.VillaRepository.UpdateAsync(updatedVilla);

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

    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            return BadRequest();
        }

        var villa = await unit.VillaRepository.GetAsync(u => u.Id == id, tracked: false);
        if (villa == null)
        {
            return NotFound();
        }
        var updateDTO = mapper.Map<VillaUpdateDTO>(villa);

        if (villa == null)
        {
            return BadRequest();
        }

        patchDTO.ApplyTo(updateDTO, ModelState);

        var patchedVilla = mapper.Map<Villa>(updateDTO);

        await unit.VillaRepository.UpdateAsync(patchedVilla);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }

}
