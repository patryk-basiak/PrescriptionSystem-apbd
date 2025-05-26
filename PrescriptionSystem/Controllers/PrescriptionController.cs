using Microsoft.AspNetCore.Mvc;
using PrescriptionSystem.DTOs;
using PrescriptionSystem.Services;

namespace PrescriptionSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;
    public PrescriptionController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }
        
    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionDTO prescriptionDto, int doctor)
    {
        try
        {
            var result = await _prescriptionService.addPrescription(prescriptionDto, doctor);
            return CreatedAtAction(nameof(AddPrescription), new { id = result.IdPrescription}, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPresctiptionUser(int id)
    {
        var x = _prescriptionService.getPatientPrescription(id);
        if (x.Result == null)
        {
            return BadRequest("Patient with that id doesnt exist");
        }
        return Ok(x.Result);
        
    }
}