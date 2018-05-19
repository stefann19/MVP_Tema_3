using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MVP_Tema_3
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext() : base(ConfigurationManager.ConnectionStrings["MVP3Entities"].ConnectionString)
        {

        }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentIntervention> AppointmentInterventions { get; set; }
        public DbSet<Intervention> Interventions { get; set; }
        public DbSet<InterventionPricesBeforeDate_Result> InterventionPricesBeforeDateResults { get; set; }
        public DbSet<Login_Result> LoginResults { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Price> Prices { get; set; }

        public DbSet<SortPatientsByInterventionsNumber_Result> SortPatientsByInterventionsNumberResults { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }
    }
}
