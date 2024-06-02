using EfCodeFirst.Models;

namespace WebApplication1.DTOs;

public class GetPrescriptionDTO
{
    public string DoctorFirstName { get; set; }
    public string DoctorLastName { get; set; }
    public string PatientFirstName { get; set; }
    public string PatientLastName { get; set; }
    public List<string> Medicaments { get; set; }
}