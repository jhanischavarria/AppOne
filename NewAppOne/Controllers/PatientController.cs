using Microsoft.AspNetCore.Mvc;
using UPB.LogicPatient.Manager;
using UPB.LogicPatient.Models;
using UPB.LogicPatient.Storage;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UPB.NewAppOne.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
       
        private PatientManager _patientManager;

        public PatientController(PatientManager patientManager)
        {
            _patientManager = patientManager;
        }

        // HTTP GET - Get all Patients
        [HttpGet]
        public List<Patient> GetPatients()
        {
            return _patientManager.GetAllPatients();
        }

        //  HTTP GET - Get Patient by CI
        [HttpGet("{ci}")]
        public Patient? GetPatientByCI(string ci)
        {
            return _patientManager.GetPatientByCI(ci);
        }

        [HttpPost]
        // HTTP POST - Create Patient
        public Patient CreatePatient([FromBody] Patient patient)
        {
            return _patientManager.CreatePatient(patient);
        }

        // HTTP PUT - Update Patient
        [HttpPut("{ci}")]
        public Patient UpdatePatient(string ci, [FromBody] Patient updatedPatient)
        {
           return _patientManager.UptadePatient(ci, updatedPatient.Name, updatedPatient.LastName);
        }

        // HTTP DELETE - Delete Patient
        [HttpDelete("{ci}")]
        public Patient DeletePatient(string ci)
        {
            return _patientManager.DeletePatient(ci);
        }
    }
}
