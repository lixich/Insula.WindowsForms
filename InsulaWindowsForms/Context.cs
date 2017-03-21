using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.ModelConfiguration;

namespace InsulaWindowsForms
{
    class Context : DbContext
    {
        public Context()
        { }

        public DbSet<Patient> PatientSet { set; get; }
        public DbSet<Fact> FactSet { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // To remove the requests to the Migration History table
            Database.SetInitializer<Context>(null);
            // To remove the plural names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //For versions of EF before 6.0, uncomment the following line to remove calls to EdmTable, a metadata table
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
        }
    }
    
    [Table("Patient")]
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string Insulin { get; set; }
        public DateTime DateOfBirth { get; set; }
  
        [NotMapped]
        public int Age
        {
            get
            {
                try
                {
                    TimeSpan t = DateTime.Now - (DateTime)DateOfBirth;
                    return (int)Math.Round(t.TotalDays / 365, 0);
                }
                catch (Exception)
                {
                    return 0;
                }
          
            }
        }
        public double Weight { get; set; }
        public double Growth { get; set; }
        public virtual List<Fact> Facts { get; set; }
    }

    public class PatientMap : EntityTypeConfiguration<Patient>
    {
        public PatientMap()
        {
            this.ToTable("Patient");
            this.HasKey(x => x.PatientId);
            this.Property(x => x.Name);
        }
    }

        [Table("Fact")]
    public class Fact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FactId { get; set; }
        public DateTime DateTime { get; set; }
        public double XE { get; set; }
        public double Dose { get; set; }
        public double Before { get; set; }
        public double After { get; set; }
        public double InsXE { get; set; }
        public double InsGlu { get; set; }
        public double Time { get; set; }
        [NotMapped]
        public double Coef { get; set; }
        public virtual Patient Patient { get; set; }

        public Fact(DateTime _DateTime, double _Dose, double _XE, double _Before, double _After)
        {
            this.DateTime = _DateTime;
            this.Time = _DateTime.TimeOfDay.TotalMinutes / 60;
            this.XE = _XE;
            this.Dose = _Dose;
            this.Before = _Before;
            this.After = _After;
            //if (Math.Abs(_After - _Before) <= 1)
            //    this.InsXE = _Insulin / _XE;
            //else
            this.InsXE = 0;
            this.InsGlu = 0;
        }

        public Fact(DateTime _DateTime, double _Dose, double _XE, double _Before)
        {
            this.DateTime = _DateTime;
            this.Time = _DateTime.TimeOfDay.TotalMinutes / 60;
            this.XE = _XE;
            this.Dose = _Dose;
            this.Before = _Before;
            this.After = 0;
            this.InsXE = 0;
            this.InsGlu = 0;
        }
    }
    public class FactMap : EntityTypeConfiguration<Fact>
    {
        public FactMap()
        {
            this.ToTable("Fact");
            this.HasKey(x => x.FactId);
            this.Property(x => x.DateTime);
        }
    }

}
