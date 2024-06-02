using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controller;


[Route("/api/")]
[ApiController]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }
    [Route("doctors")]
    [HttpGet]
    public async Task<IActionResult> GetDoctorsAsync()
    {
        var doctors = await _doctorService.GetDoctorsAsync();
        return Ok(doctors);
    }
    [Route("doctors")]
    [HttpPost]
    public async Task<IActionResult> AddDoctorAsync([FromBody] DoctorDTO doctorDto)
    {
        var doctors = await _doctorService.AddDoctorAsync(doctorDto);
        return doctors ? Created() : Conflict();
    }
    [Route("doctors")]
    [HttpPut]
    public async Task<IActionResult> EditDoctorAsync([FromBody] DoctorDTO doctorDto)
    {
        var doctors = await _doctorService.EditDoctorAsync(doctorDto);
        return doctors ? NoContent() : NotFound();
    }
    [Route("doctors/{IdDoctor}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteDoctorAsync([FromRoute] int IdDoctor)
    {
        var doctors = await _doctorService.DeleteDoctorAsync(IdDoctor);
        return doctors ? NoContent() : NotFound();
    }
    // [Route("prescriptions")]
    // [HttpPost]
    // public async Task<IActionResult> GetPrescriptionsByDependenciesAsync([FromBody] GetPrescriptionDTO getPrescriptionDto)
    // {
    //     var prescription = await _doctorService.GetPrescriptionsByDependenciesAsync(getPrescriptionDto);
    //     return prescription == null ? NotFound() : Ok(prescription);
    // }
}