using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPB.LogicPatient.Models;

namespace UPB.LogicPatient.Storage
{
    public class PatientStorage
    {
        private readonly IConfiguration _configuration;

        public PatientStorage(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public  Patient WritePatientStorage(Patient patient)
        {
            // Obtener la ubicación del archivo de pacientes desde la configuración
            string patientFilePath = _configuration.GetConnectionString("FileSettings");

            // Inicializar StreamWriter con la ubicación del archivo de pacientes
            using (StreamWriter sw = File.AppendText(patientFilePath))
            {
                // Escribir los datos del paciente en el archivo
                sw.WriteLine($"{patient.Name}, {patient.LastName}, {patient.CI}, {patient.BloodGroup}");
            }
            return patient;
        }
        public List<Patient> ReadPatientsFromFile()
        {
            List<Patient> patients = new List<Patient>();

            // Obtener la ubicación del archivo de pacientes desde la configuración
            string patientFilePath = _configuration.GetSection("FileSettings")["PatientFilePath"];

            // Leer el contenido del archivo línea por línea
            using (StreamReader sr = new StreamReader(patientFilePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Patient patient = ConvertLineToPatient(line);
                    patients.Add(patient); ;
                }
            }

            return patients;
        }
        public Patient GetPatientByCI(string ci)
        {
            List<Patient> patients = ReadPatientsFromFile();
            return patients.Find(p => p.CI == ci);
        }
        public Patient UpdatePatientInStorage(string ci, Patient updatedPatient)
        {
            List<Patient> patients = ReadPatientsFromFile();
            int index = patients.FindIndex(p => p.CI == ci);
            if (index != -1)
            {
                patients[index] = updatedPatient;
                RewritePatientsToFile(patients);
            }
            else
            {
                Console.WriteLine("Patient not found");
            }
            return updatedPatient;
        }
        public Patient DeletePatientFromStorage(string ci)
        {
            List<Patient> patients = ReadPatientsFromFile();
            Patient patientToRemove = patients.Find(p => p.CI == ci);
            if (patientToRemove != null)
            {
                patients.Remove(patientToRemove);
                RewritePatientsToFile(patients);
            }
            else
            {
                Console.WriteLine("Patient not found");
            }
            return patientToRemove;
        }
        private void RewritePatientsToFile(List<Patient> patients)
        {
            string patientFilePath = _configuration.GetConnectionString("FileSettings");

            using (StreamWriter sw = new StreamWriter(patientFilePath))
            {
                foreach (Patient patient in patients)
                {
                    sw.WriteLine($"{patient.Name}, {patient.LastName}, {patient.CI}, {patient.BloodGroup}");
                }
            }
        }

        private Patient ConvertLineToPatient(string line)
        {
            // Separar la línea en sus componentes (nombre, apellido, CI, grupo sanguíneo)
            string[] parts = line.Split(',');

            // Crear un nuevo objeto Patient usando los componentes de la línea
            Patient patient = new Patient
            {
                Name = parts[0].Trim(),
                LastName = parts[1].Trim(),
                CI = parts[2].Trim(),
                BloodGroup = parts[3].Trim()
            };

            return patient;
        }
    }
}
