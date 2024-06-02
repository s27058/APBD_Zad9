using EfCodeFirst.Models;
using WebApplication1.DTOs;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public interface IDoctorService
{
    public Task<List<DoctorDTO>> GetDoctorsAsync();
    public Task<bool> AddDoctorAsync(DoctorDTO doctorDTO);
    public Task<bool> EditDoctorAsync(DoctorDTO doctorDTO);
    public Task<bool> DeleteDoctorAsync(int IdDoctor);
    public Task<Prescription?> GetPrescriptionsByDependenciesAsync(GetPrescriptionDTO getPrescriptionDto);
}

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    public DoctorService(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }
    public async Task<List<DoctorDTO>> GetDoctorsAsync()
    {
        var doctors = await _doctorRepository.GetDoctorsAsync();
        var doctorDTOs = new List<DoctorDTO>();
        doctors.ForEach(d =>
        {
            doctorDTOs.Add(new DoctorDTO()
            {
                IdDoctor = d.IdDoctor,
                Email = d.Email,
                FirstName = d.FirstName,
                LastName = d.LastName
            });
        });
        return doctorDTOs;
    }

    public async Task<bool> AddDoctorAsync(DoctorDTO doctorDTO)
    {
        if (await _doctorRepository.DoctorExistsByIdAsync(doctorDTO.IdDoctor))
        {
            return false;
        }
        var doctor = new Doctor()
        {
            IdDoctor = doctorDTO.IdDoctor,
            Email = doctorDTO.Email,
            FirstName = doctorDTO.FirstName,
            LastName = doctorDTO.LastName
        };
        await _doctorRepository.AddDoctorAsync(doctor);
        return true;
    }
    public async Task<bool> EditDoctorAsync(DoctorDTO doctorDTO)
    {
        if (!await _doctorRepository.DoctorExistsByIdAsync(doctorDTO.IdDoctor))
        {
            return false;
        }
        var doctor = new Doctor()
        {
            IdDoctor = doctorDTO.IdDoctor,
            Email = doctorDTO.Email,
            FirstName = doctorDTO.FirstName,
            LastName = doctorDTO.LastName
        };
        await _doctorRepository.EditDoctorAsync(doctor);
        return true;
    }
    public async Task<bool> DeleteDoctorAsync(int IdDoctor)
    {
        if (!await _doctorRepository.DoctorExistsByIdAsync(IdDoctor))
        {
            return false;
        }
        await _doctorRepository.DeleteDoctorAsync(IdDoctor);
        return true;
    }

    public async Task<Prescription?> GetPrescriptionsByDependenciesAsync(GetPrescriptionDTO getPrescriptionDto)
    {
        return await _doctorRepository.GetPrescriptionsByDependenciesAsync(getPrescriptionDto);
    }
}