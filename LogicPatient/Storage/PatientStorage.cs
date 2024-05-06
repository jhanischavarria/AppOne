using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
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

        public Patient WritePatientStorage(Patient patient)
        {
            try
            {
                string patientFilePath = _configuration.GetConnectionString("FileSettings");
                using (StreamWriter sw = File.AppendText(patientFilePath))
                {
                    sw.WriteLine($"{patient.Name}, {patient.LastName}, {patient.CI}, {patient.BloodGroup}");
                }
                return patient;
            }
            catch (Exception ex)
            {
                Log.Error($"Error al escribir en el archivo de pacientes: {ex.Message}");
                throw; // Propaga la excepción para que sea manejada en otro lugar si es necesario
            }
        }

        public List<Patient> ReadPatientsFromFile()
        {
            try
            {
                List<Patient> patients = new List<Patient>();
                string patientFilePath = _configuration.GetSection("FileSettings")["PatientFilePath"];
                using (StreamReader sr = new StreamReader(patientFilePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Patient patient = ConvertLineToPatient(line);
                        patients.Add(patient);
                    }
                }
                return patients;
            }
            catch (Exception ex)
            {
                Log.Error($"Error al leer el archivo de pacientes: {ex.Message}");
                throw; // Propaga la excepción para que sea manejada en otro lugar si es necesario
            }
        }

        public Patient GetPatientByCI(string ci)
        {
            try
            {
                List<Patient> patients = ReadPatientsFromFile();
                return patients.Find(p => p.CI == ci);
            }
            catch (Exception ex)
            {
                Log.Error($"Error al buscar paciente por CI: {ex.Message}");
                throw; // Propaga la excepción para que sea manejada en otro lugar si es necesario
            }
        }

        public Patient UpdatePatientInStorage(string ci, Patient updatedPatient)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error($"Error al actualizar paciente en el almacenamiento: {ex.Message}");
                throw; // Propaga la excepción para que sea manejada en otro lugar si es necesario
            }
        }

        public Patient DeletePatientFromStorage(string ci)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error($"Error al eliminar paciente del almacenamiento: {ex.Message}");
                throw; // Propaga la excepción para que sea manejada en otro lugar si es necesario
            }
        }

        private void RewritePatientsToFile(List<Patient> patients)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error($"Error al reescribir el archivo de pacientes: {ex.Message}");
                throw; // Propaga la excepción para que sea manejada en otro lugar si es necesario
            }
        }

        private Patient ConvertLineToPatient(string line)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error($"Error al convertir la línea en un objeto Patient: {ex.Message}");
                throw; // Propaga la excepción para que sea manejada en otro lugar si es necesario
            }
        }
    }
}
