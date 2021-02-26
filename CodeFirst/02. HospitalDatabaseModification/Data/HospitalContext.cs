using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext: DbContext
    {

        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientMedicament> Prescriptions { get; set; }
        public DbSet<Visitation> Visitations { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        protected  override void OnConfiguring(DbContextOptionsBuilder builder)
        {

            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(DataSettings.DefaultConnection);
            }
        }
        
        protected  override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Patient>()
                .Property(p => p.Email)
                .IsUnicode(false);

            builder.Entity<PatientMedicament>()
                .HasKey(k => new { k.PatientId, k.MedicamentId });

        }


    }
}
