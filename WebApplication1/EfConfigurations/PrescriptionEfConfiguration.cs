using EfCodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCodeFirst.EfConfigurations;

public class PrescriptionEfConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.ToTable("Prescription");

        builder.HasKey(p => p.IdPrescription);
        builder.Property(p => p.IdPrescription).ValueGeneratedOnAdd();
        builder.Property(p => p.Date).IsRequired();
        builder.Property(p => p.DueDate).IsRequired();
        
        builder.HasOne<Doctor>(p => p.IdDoctorNavigation)
            .WithMany(d => d.Prescriptions)
            .HasForeignKey(p => p.IdDoctor);
        
        builder.HasOne<Patient>(p => p.IdPatientNavigation)
            .WithMany(p => p.Prescriptions)
            .HasForeignKey(p => p.IdPatient);
    }
}