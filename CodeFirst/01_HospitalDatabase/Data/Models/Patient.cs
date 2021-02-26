using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P01_HospitalDatabase.Data.Models
{
    /*
     */
    public class Patient
    {

        public Patient()
        {
            this.Diagnoses = new HashSet<Diagnose>();
            this.Prescriptions = new HashSet<PatientMedicament>();
            this.Visitations = new HashSet<Visitation>();
        }
        [Key]
        public int PatientId { get; set; }

        
        [MaxLength(DataValidations.PatientFistName)]
        public string FirstName { get; set; }

        
        [MaxLength(DataValidations.PatientLastName)]
        public string LastName { get; set; }

        [MaxLength(DataValidations.PatientAddress)]
        public string Address { get; set; }

        [MaxLength(DataValidations.PatientEmail)]
        public string Email { get; set; }

        public bool HasInsurance { get; set; }

        public virtual ICollection<PatientMedicament> Prescriptions { get; set; }
        public virtual ICollection<Visitation> Visitations { get; set; }
        public virtual ICollection<Diagnose> Diagnoses { get; set; }
    }
}
