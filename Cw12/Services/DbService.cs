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
            using var tran = context.Database.BeginTransaction();
            var read = from p in context.Patient
                       join pre in context.Prescription on p.IdPatient equals pre.IdPatient into gpre
                       from pprree in gpre.DefaultIfEmpty()
                       join pm in context.PrescriptionMedicament on pprree.IdPrescription equals pm.IdPrescription into gpm
                       from ppmm in gpm.DefaultIfEmpty()
                       where p.IdPatient == patient.IdPatient
                       select new { p.IdPatient, pprree.IdPrescription, ppmm.IdMedicament };

            var result = read.ToList();

            var firstDelete = read.Where(x => x.IdMedicament != 0 && x.IdPrescription != 0).Select(x => new { x.IdPrescription, x.IdMedicament });
            foreach (var item in firstDelete)
            {
                var e = new PrescriptionMedicament()
                {
                    IdPrescription = item.IdPrescription,
                    IdMedicament = item.IdMedicament
                };
                context.PrescriptionMedicament.Attach(e);
                context.PrescriptionMedicament.Remove(e);
            }

            context.SaveChanges();

            var secondDelete = read.Where(x => x.IdPrescription != 0).Select(x => new { x.IdPrescription }).Distinct();
            foreach (var item in secondDelete)
            {
                var e = new Prescription()
                {
                    IdPrescription = item.IdPrescription
                };
                context.Prescription.Attach(e);
                context.Prescription.Remove(e);
            }

            context.SaveChanges();

            var thirdDelete = read.Where(x => x.IdPatient == patient.IdPatient).Select(x => new { x.IdPatient }).Distinct();
            foreach (var item in thirdDelete)
            {
                var e = new Patient()
                {
                    IdPatient = item.IdPatient
                };
                context.Patient.Attach(e);
                context.Patient.Remove(e);
            }

            context.SaveChanges();

            tran.Commit();
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
