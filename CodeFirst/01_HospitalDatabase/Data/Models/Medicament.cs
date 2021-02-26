using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P01_HospitalDatabase.Data.Models 
{
    /*
     *  	MedicamentId
        	Name (up to 50 characters, unicode)

     */
    public class Medicament
    {
        public Medicament()
        {
            this.Prescriptions = new HashSet<PatientMedicament>();
        }
        public int MedicamentId { get; set; }

        [MaxLength(DataValidations.MedicamentName)]
        public string Name { get; set; }

        public virtual ICollection<PatientMedicament> Prescriptions { get; set; }

    }
}
