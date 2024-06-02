using EfCodeFirst.EfConfigurations;
using EfCodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCodeFirst.Context;

public class AppDbContext : DbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }


    public AppDbContext() {}

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder
    //         .UseSqlServer(
    //             "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True")
    //         .LogTo(Console.WriteLine, LogLevel.Information);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DoctorEfConfiguration());
        modelBuilder.ApplyConfiguration(new MedicamentEfConfiguration());
        modelBuilder.ApplyConfiguration(new PatientEfConfiguration());
        modelBuilder.ApplyConfiguration(new PrescriptionEfConfiguration());
        modelBuilder.ApplyConfiguration(new PrescriptionMedicamentEfConfiguration());
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>().HasData(new {IdDoctor=1, FirstName="Jan", LastName="Kowalski", Email="l"});
        modelBuilder.Entity<Patient>().HasData(new {IdPatient=1, FirstName="Tomasz", LastName="Nowak", Birthdate=new DateTime(1999, 12, 1)});
        modelBuilder.Entity<Prescription>().HasData(new {IdPrescription=1, Date=new DateTime(2021, 12, 1), DueDate=new DateTime(2023, 12, 1), IdPatient=1, IdDoctor=1});
        modelBuilder.Entity<Medicament>().HasData(new {IdMedicament=1, Name="Paracetamol", Description="Lek", Type="Przeciwbolowy"});
        modelBuilder.Entity<PrescriptionMedicament>().HasData(new {IdMedicament=1, IdPrescription=1, Dose=10, Details="Nie przedawkowac"});
    }
}