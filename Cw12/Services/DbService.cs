using Cw12.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw12.Services
{
    public class DbService : IDbService
    {
        private s18923Context context;

        public DbService(s18923Context context)
        {
            this.context = context;
        }

        public void DeletePatient(Patient patient)
        {
            var result = context.Patient.FirstOrDefault(e => e.IdPatient == patient.IdPatient);
            //var result = context.Patient.Include(x => x.Prescription).FirstOrDefault(e => e.IdPatient == patient.IdPatient);
            //var pre = context.Prescription.Where(s => s.IdPatient == result.IdPatient).ToList();      
            context.Patient.Remove(result);
            ////context.Prescription.Remove(pre);
            context.SaveChanges();
        }

        public void AddPatient(Patient patient)
        {
            context.Add(patient);
            context.SaveChanges();
        }

        public Patient GetPatientDetails(Patient patient)
        {
            //var xxxx = context
            //    .Patient
            //    .Where(e => e.IdPatient == patient.IdPatient)
            //    .Select(x => new 
            //    {
            //        x.LastName,
            //        x.IdPatient,
            //        Recepty = x.Prescription.Select(s => new { s.IdPrescription, s.IdDoctorNavigation.LastName, s.IdDoctorNavigation.FirstName })
            //    })
            //    .ToList();

            var result = context.Patient.Include(x => x.Prescription).FirstOrDefault(e => e.IdPatient == patient.IdPatient);                                 
            return result;
        }

        public List<Patient> GetPatients()
        {
            return context.Patient.ToList();
        }
    }
}
