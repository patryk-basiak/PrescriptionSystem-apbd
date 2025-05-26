using PrescriptionSystem.Models;

namespace PrescriptionSystem.DTOs;

public class PatientDescriptionDTO
{
    public PatientDTO PatientDto { get; set; }
    public List<Prescription> PrescriptionDto { get; set; }
}