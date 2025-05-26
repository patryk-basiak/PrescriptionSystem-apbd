using Microsoft.EntityFrameworkCore;
using PrescriptionSystem.Models;

namespace PrescriptionSystem.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("Doctor");
            entity.HasKey(e => e.IdDoctor);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
        });
        
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("Patient");
            entity.HasKey(e => e.IdPatient);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Birthdate).HasColumnType("date");
        });
        
        modelBuilder.Entity<Medicament>(entity =>
        {
            entity.ToTable("Medicament");
            entity.HasKey(e => e.IdMedicament);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(100);
        });
        
        modelBuilder.Entity<PrescriptionMedicament>(entity =>
        {
            entity.ToTable("Prescription_Medicament");
            entity.HasKey(e => new { e.IdMedicament, e.IdPrescription });

            entity.Property(e => e.Details).HasMaxLength(100);

            entity.HasOne(pm => pm.Medicament)
                  .WithMany(m => m.PrescriptionMedicaments)
                  .HasForeignKey(pm => pm.IdMedicament);

            entity.HasOne(pm => pm.Prescription)
                  .WithMany(p => p.PrescriptionMedicaments)
                  .HasForeignKey(pm => pm.IdPrescription);
        });
        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>()
        {
            new Doctor() { IdDoctor = 1, Email = "JohnPork@gmail.com", FirstName = "John", LastName = "Pork" },
            new Doctor() { IdDoctor = 2, Email = "Edd123Blacha@gmail.com", FirstName = "Edmund", LastName = "Blacha" },
            new Doctor() { IdDoctor = 3, Email = "MaciGejwatowski@gmail.com", FirstName = "Maciej", LastName = "Gerwatowski" }
        });

        modelBuilder.Entity<Patient>().HasData(new List<Patient>()
        {
            new Patient()
                { IdPatient = 1, FirstName = "Bartosz", LastName = "Bialy", Birthdate = new DateTime(2003, 12, 17) },
            new Patient()
                { IdPatient = 2, FirstName = "Patryk", LastName = "Basiak", Birthdate = new DateTime(2003, 10, 10) }
        });

        modelBuilder.Entity<Medicament>().HasData(new List<Medicament>()
        {
            new Medicament()
                { IdMedicament = 1, Name = "Paracetamol", Description = "Paracetamol", Type = "Antybiotyczne" },
            new Medicament()
                { IdMedicament = 2, Name = "Ibuprofen", Description = "Ibuprofen", Type = "Antybiotyczne" },
        });

        modelBuilder.Entity<Prescription>().HasData(new List<Prescription>()
        {
            new Prescription() { IdPrescription = 1, Date = new DateTime(2022, 01, 01), DueDate = new DateTime(2022, 02, 02), IdPatient = 1, IdDoctor = 1 },
            new Prescription() { IdPrescription = 2, Date = new DateTime(2022, 03, 11), DueDate = new DateTime(2022, 04, 07), IdPatient = 2, IdDoctor = 1 },
            new Prescription() { IdPrescription = 3, Date = new DateTime(2022, 4, 30), DueDate = new DateTime(2022, 06, 12), IdPatient = 2, IdDoctor = 3}
        });

        modelBuilder.Entity<PrescriptionMedicament>().HasData(new List<PrescriptionMedicament>()
        {
            new PrescriptionMedicament() {IdMedicament = 1, IdPrescription = 1,Dose=4,Details = "lek jakis"},
            new PrescriptionMedicament() {IdMedicament = 2, IdPrescription = 1,Dose=2,Details = "lek jakis 2" },
            new PrescriptionMedicament() {IdMedicament = 1, IdPrescription = 2,Dose=1,Details = "lek jakis 3" },
            new PrescriptionMedicament() {IdMedicament = 2, IdPrescription = 3,Dose=7,Details = "lek jakis 4" },
        });
    }
}