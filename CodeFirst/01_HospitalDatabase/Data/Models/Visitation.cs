using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_HospitalDatabase.Data.Models
{
    /*
     *  	VisitationId
        	Date
        	Comments (up to 250 characters, unicode)
        	Patient

     */
    public class Visitation
    {
        public int VisitationId { get; set; }

        public DateTime Date { get; set; }

        [MaxLength(DataValidations.VisitationsComments)]
        public string Comments { get; set; }

        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }

        public Patient Patient { get; set; }


    }
}
