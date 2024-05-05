using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UPB.LogicPatient.Models;

namespace UPB.LogicPatient.Manager
{
    public class PatientManager
    {
        private List<Patient> _patients;
        public PatientManager() 
        {
            _patients = new List<Patient>();
            _patients.Add(new Patient()
            {
                Name= "Juan",
                LastName = "Pérez",
                CI = "12345678"
            });

      
            _patients.Add(new Patient()
            {
                Name = "Maria",
                LastName = "Gonzalez",
                CI = "87654321"
            });
        }
        //Method to get all patients
        public List<Patient> GetAllPatients()
        {
            return _patients;
        }

        //Method to get a patient by CI
        public Patient? GetPatientByCI(string ci)
        {
            Patient? foundStudent = _patients.Find(p => p.CI == ci);
            return foundStudent;
        }
        //Method to create patient
        public Patient CreatePatient(Patient patientToCreate)
        {
            Patient createdPatient = new Patient()
            {
                Name = patientToCreate.Name,
                LastName = patientToCreate.LastName,
                CI = patientToCreate.CI
            };
            _patients.Add(createdPatient); 
            return createdPatient;
        }

        //Method to upade patient
        public Patient? UptadePatient(string ci, string name, string lastName)
        {
            var patientUptade = _patients.Find(p => p.CI == ci);
            if ( patientUptade != null)
            {
                patientUptade.Name = name;
                patientUptade.LastName = lastName;
            }
            else
            {
                Console.WriteLine("Patient not found");
            }
            return patientUptade;
        }
        //Method to delete patient
        public Patient? DeletePatient(string ci)
        {
            var patientDelete = _patients.Find(p => p.CI == ci);
            if (patientDelete != null)
            {
                _patients.Remove(patientDelete);
            }
            else
            {
                Console.WriteLine("Patient not found");
            }
            return patientDelete;
        }






    }
}
