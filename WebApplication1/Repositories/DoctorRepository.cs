using EfCodeFirst.Context;
using EfCodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication1.DTOs;

namespace WebApplication1.Repositories;

public interface IDoctorRepository
{
    public Task<List<Doctor>> GetDoctorsAsync();
    public Task AddDoctorAsync(Doctor doctor);
    public Task<bool> DoctorExistsByIdAsync(int IdDoctor);
    public Task EditDoctorAsync(Doctor doctor);
    public Task DeleteDoctorAsync(int IdDoctor);
    public Task<Prescription?> GetPrescriptionsByDependenciesAsync(GetPrescriptionDTO getPrescriptionDto);
}

public class DoctorRepository : IDoctorRepository
{
    private readonly AppDbContext _context;

    public DoctorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Doctor>> GetDoctorsAsync()
    {
        return await _context.Doctors.AsQueryable().ToListAsync();
    }
    
    public async Task AddDoctorAsync(Doctor doctor)
    { 
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> DoctorExistsByIdAsync(int IdDoctor)
    {
        return await _context.Doctors.AnyAsync(d => d.IdDoctor == IdDoctor);
    }

    public async Task EditDoctorAsync(Doctor doctor)
    {
        _context.Doctors.Attach(doctor);
        _context.Entry(doctor).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task DeleteDoctorAsync(int IdDoctor)
    {
        var doctor = new Doctor()
        {
            IdDoctor = IdDoctor
        };
        _context.Doctors.Attach(doctor);
        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync();
    }

    public async Task<Prescription?> GetPrescriptionsByDependenciesAsync(GetPrescriptionDTO getPrescriptionDto)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(
            d => d.FirstName == getPrescriptionDto.DoctorFirstName && d.LastName == getPrescriptionDto.DoctorLastName);
        if (doctor == null) return null;
        var patient = await _context.Patients.FirstOrDefaultAsync(
            d => d.FirstName == getPrescriptionDto.PatientFirstName && d.LastName == getPrescriptionDto.PatientLastName);
        if (patient == null) return null;
        
        var medicamentIds = new List<int>();
        await _context.Medicaments.ForEachAsync(m =>
        {
            if (getPrescriptionDto.Medicaments.Contains(m.Name)) medicamentIds.Add(m.IdMedicament);
        });
        var prescriptionIds = _context.Prescriptions
            .Where(p => p.IdDoctor == doctor.IdDoctor && p.IdPatient == patient.IdPatient)
            .Select(p => p.IdPrescription).AsEnumerable();
        var meds = _context.PrescriptionMedicaments
            .Where(pm => prescriptionIds.Contains(pm.IdPrescription))
            .GroupBy(
                pm => pm.IdPrescription,
                pm => pm.IdMedicament,
                (IdPrescription, meds) => new
                {
                    Key = IdPrescription,
                    Value = meds
                }
            )
            .AsEnumerable()
            .Single(g => g.Value.Equals(medicamentIds));
        return meds == null ? null : _context.Prescriptions.FindAsync(meds.Key).Result;
    }
}