using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UPB.LogicPatient.Models;
using Microsoft.Extensions.Configuration;
using UPB.LogicPatient.Storage;

namespace UPB.LogicPatient.Manager
{
    public class PatientManager
    {
        private List<Patient> _patients;
        private readonly PatientStorage _patientStorage;
        public PatientManager(PatientStorage patientStorage) 
        {
            _patientStorage = patientStorage;
           
        }
        //Method to get all patients
        public List<Patient> GetAllPatients()
        {
            return _patientStorage.ReadPatientsFromFile();
        }

        //Method to get a patient by CI
        public Patient? GetPatientByCI(string ci)
        {
            return _patientStorage.GetPatientByCI(ci);
        }
        //Method to create patient
        public Patient CreatePatient(Patient patientToCreate)
        {
            return _patientStorage.WritePatientStorage(patientToCreate);
            
        }

        //Method to upade patient
        public Patient? UptadePatient(string ci, string name, string lastName)
        {
            Patient updatedPatient = new Patient()
            {
                CI = ci,
                Name = name,
                LastName = lastName
            };
            return _patientStorage.UpdatePatientInStorage(ci, updatedPatient);

        }
        //Method to delete patient
        public Patient? DeletePatient(string ci)
        {
            return _patientStorage.DeletePatientFromStorage(ci);
        }






    }
}
