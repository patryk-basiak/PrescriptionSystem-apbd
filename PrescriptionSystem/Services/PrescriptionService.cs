using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PrescriptionSystem.Data;
using PrescriptionSystem.DTOs;
using PrescriptionSystem.Models;

namespace PrescriptionSystem.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly DatabaseContext _context;
    
    public PrescriptionService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Prescription> addPrescription(PrescriptionDTO prescriptionDto, int doctorId)
    {
        if (prescriptionDto.Date > prescriptionDto.DueDate)
        {
            throw new ArgumentException("DueDate is smaller than date");
        }

        if (prescriptionDto.Medicaments.Count > 10)
        {
            throw new ArgumentException("Medicaments list is larger than 10");
        }

        if (!PatientExists(prescriptionDto.Patient).Result)
        {
            AddPatient(prescriptionDto.Patient);
        }

        foreach (var medicament in prescriptionDto.Medicaments)
        {
            var medi = await _context.Medicaments.FirstOrDefaultAsync(m =>
                m.IdMedicament == medicament.IdMedicament &&
                m.Description == medicament.Description);
            if (medi == null)
            {
                throw new Exception("Medicament in list doesnt exist in database");
            }
        }
        
        var prescription = new Prescription
        {
            Date = prescriptionDto.Date,
            DueDate = prescriptionDto.DueDate,
            IdPatient = prescriptionDto.Patient.IdPatient,
            IdDoctor = doctorId,
            PrescriptionMedicaments =  prescriptionDto.Medicaments.Select(m=>new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose, 
                Details = m.Description
                
            }).ToList()
        };

        _context.Prescriptions.Add(prescription);
        
        await _context.SaveChangesAsync();
        
        return prescription;
        
    }

    public async Task<bool> PatientExists(PatientDTO patient)
    {
        var p =  await _context.Patients.FirstOrDefaultAsync(p => p.IdPatient == patient.IdPatient && p.FirstName == patient.FirstName && p.LastName == patient.LastName && p.Birthdate == patient.Birthdate);
        return p != null;
    }

    public async Task<PatientDescriptionDTO?> getPatientPrescription(int patientID)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == patientID);

        if (patient == null)
            return null;
        var prescriptions = await _context.Prescriptions
            .Where(p => p.IdPatient == patient.IdPatient)
            .Include(p => p.Doctor)
            .Select(p => new Prescription
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                Doctor = new Doctor()
                {
                    IdDoctor = p.Doctor.IdDoctor,
                    FirstName = p.Doctor.FirstName,
                    LastName = p.Doctor.LastName,
                    Email = p.Doctor.Email
                },
                
            })
            .ToListAsync();

        return new PatientDescriptionDTO
        {
            PatientDto = new PatientDTO(){Birthdate = patient.Birthdate, FirstName = patient.FirstName, LastName = patient.LastName, IdPatient = patientID},
            PrescriptionDto = prescriptions
        };
    }
    public async void AddPatient(PatientDTO patientDto)
    {
        _context.Patients.Add(new Patient()
        {
            IdPatient = patientDto.IdPatient,
            FirstName = patientDto.FirstName,
            LastName = patientDto.LastName,
            Birthdate = patientDto.Birthdate,
            Prescriptions = new List<Prescription>()
        });
        await _context.SaveChangesAsync();
    }
}