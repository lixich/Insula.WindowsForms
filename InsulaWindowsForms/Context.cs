using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.ComponentModel;
using SQLite.CodeFirst;

namespace InsulaWindowsForms
{
    /*
    class Context : DbContext
    {
        public Context() : base("DbConnection")
        { }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<Context>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
        
        public DbSet<Patient> PatientSet { get; set; }
        public DbSet<Fact> FactSet { get; set; }

    }*/

    class Context : DbContext
    {
        public List<Patient> PatientSet = new List<Patient>();
        public List<Fact> FactSet = new List<Fact>();
    }
    [Table("Patient")]
    public class Patient
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string Insulin { get; set; }
        public DateTime? DateOfBirth { get; set; }
  
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
    
    //[Table("Fact")]
    public class Fact
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FactId { get; set; }
        public DateTime? DateTime { get; set; }
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

}
