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
using System.Runtime.Serialization;

namespace InsulaWindowsForms
{
    /*
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
    */
    [Serializable()]
    [DataContract]
    //[Table("Patient")]
    public class Patient
    {
        /*
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientId { get; set; }*/
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Insulin { get; set; }
        [DataMember]
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
        [DataMember]
        public double Weight { get; set; }
        [DataMember]
        public double Growth { get; set; }
        [DataMember]
        public List<Fact> Facts { get; set; }
    }

    [Serializable()]
    [DataContract]
    //[Table("Fact")]
    public class Fact
    {
        /*
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FactId { get; set; }*/
        [DataMember]
        public DateTime DateTime { get; set; }
        [DataMember]
        public double XE { get; set; }
        [DataMember]
        public double Dose { get; set; }
        [DataMember]
        public double Before { get; set; }
        [DataMember]
        public double After { get; set; }
        [DataMember]
        public double InsXE { get; set; }
        [DataMember]
        public double InsGlu { get; set; }
        [DataMember]
        public double Time { get; set; }
        [NotMapped]
        public double Coef { get; set; }
        /*[DataMember]
        public virtual Patient Patient { get; set; }*/

        public Fact()
        {

        }

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
