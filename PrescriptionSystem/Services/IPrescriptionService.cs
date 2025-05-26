using Microsoft.AspNetCore.Mvc;
using PrescriptionSystem.DTOs;
using PrescriptionSystem.Models;

namespace PrescriptionSystem.Services;

public interface IPrescriptionService
{
    Task<Prescription> addPrescription(PrescriptionDTO prescriptionDto, int doctorId);
    Task<PatientDescriptionDTO?> getPatientPrescription(int patientID);
}