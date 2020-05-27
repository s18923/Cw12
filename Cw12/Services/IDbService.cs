using Cw12.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw12.Services
{
    public interface IDbService
    {
        public List<Patient> GetPatients();

        public Patient GetPatientDetails(Patient patient);

        public void AddPatient(Patient patient);

        public void DeletePatient(Patient patient);
    }
}
