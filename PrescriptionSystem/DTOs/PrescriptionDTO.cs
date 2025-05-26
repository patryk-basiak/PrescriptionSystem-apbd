using PrescriptionSystem.Models;

namespace PrescriptionSystem.DTOs;

public class PrescriptionDTO
{
    public PatientDTO Patient { get; set; }
    public List<MedicamentsDTO> Medicaments { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}