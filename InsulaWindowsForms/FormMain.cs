using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace InsulaWindowsForms
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            using (Context db = new Context())
            {
                db.Database.CreateIfNotExists();
                Patient p = new Patient();
                /*p.DateOfBirth = DateTime.Now.ToString();
                p.Growth = 0;
                p.Weight = 0;*/
                p.Name = "Name2";
                p.PatientId = 1;
                db.PatientSet.Add(p);
                /*
                List<Fact> facts = new List<Fact>();
                #region//Дневник 2
                //Add(Время(Год, Месяц, День, Час, Минута, Секунда), Инсулин, ХЕ, До, После);
                //facts.Add(new Fact(new DateTime(2016, 17, 11, 9, 0, 0), 4, 4, 13.6, После);
                //facts.Add(new Fact(new DateTime(2016, 17, 11, 14, 0, 0), 6, 6, До, 3.7);
                facts.Add(new Fact(new DateTime(2016, 11, 17, 19, 0, 0), 10, 9, 3.7, 11.6));

                facts.Add(new Fact(new DateTime(2016, 11, 18, 7, 30, 0), 4, 5, 11.9, 9.8));
                facts.Add(new Fact(new DateTime(2016, 11, 18, 13, 10, 0), 7, 7, 9.8, 9.5));
                facts.Add(new Fact(new DateTime(2016, 11, 18, 19, 0, 0), 12, 10, 9.5, 14.2));
                facts.Add(new Fact(new DateTime(2016, 11, 18, 23, 0, 0), 2, 0, 14.2, 12.2));

                facts.Add(new Fact(new DateTime(2016, 11, 19, 11, 0, 0), 6, 6, 12.2, 10.7));
                //facts.Add(new Fact(new DateTime(2016, 19, 11, 15, 0, 0), 16, 16, 10.7, После);
                //facts.Add(new Fact(new DateTime(2016, 19, 11, 18, 0, 0), Инсулин, ХЕ, До, После);


                facts.Add(new Fact(new DateTime(2016, 11, 20, 15, 0, 0), 6, 6, 6.4, 9.2));
                facts.Add(new Fact(new DateTime(2016, 11, 20, 19, 0, 0), 4, 3, 9.2, 16.2));
                facts.Add(new Fact(new DateTime(2016, 11, 20, 23, 0, 0), 2, 0, 16.2, 7.3));


                facts.Add(new Fact(new DateTime(2016, 11, 21, 12, 30, 0), 6, 6, 7.3, 16.1));
                facts.Add(new Fact(new DateTime(2016, 11, 21, 19, 0, 0), 14, 10, 16.1, 13.5));
                facts.Add(new Fact(new DateTime(2016, 11, 21, 23, 0, 0), 2, 0, 13.5, 8.3));

                facts.Add(new Fact(new DateTime(2016, 11, 22, 19, 0, 0), 14, 10, 6.2, 10.7));

                facts.Add(new Fact(new DateTime(2016, 11, 23, 7, 30, 0), 7, 6, 11.6, 13.5));
                facts.Add(new Fact(new DateTime(2016, 11, 23, 14, 0, 0), 10, 9, 13.5, 18.2));
                facts.Add(new Fact(new DateTime(2016, 11, 23, 18, 0, 0), 16, 11, 18.2, 15.9));
                facts.Add(new Fact(new DateTime(2016, 11, 23, 23, 0, 0), 6, 2, 15.9, 12.0));

                facts.Add(new Fact(new DateTime(2016, 11, 24, 19, 30, 0), 16, 12, 8.5, 8.7));
                facts.Add(new Fact(new DateTime(2016, 11, 24, 22, 0, 0), 6, 4, 8.7, 9.7));

                facts.Add(new Fact(new DateTime(2016, 11, 25, 19, 30, 0), 16, 13, 5.3, 7.9));
                facts.Add(new Fact(new DateTime(2016, 11, 25, 23, 59, 0), 10, 6, 7.9, 13.4));

                facts.Add(new Fact(new DateTime(2016, 11, 26, 11, 30, 0), 6, 7, 8.6, 15.3));
                facts.Add(new Fact(new DateTime(2016, 11, 26, 14, 30, 0), 10, 8, 15.3, 9.7));
                facts.Add(new Fact(new DateTime(2016, 11, 26, 18, 30, 0), 14, 10, 9.7, 13.8));
                facts.Add(new Fact(new DateTime(2016, 11, 26, 23, 30, 0), 8, 4, 13.8, 8.4));

                facts.Add(new Fact(new DateTime(2016, 11, 27, 13, 0, 0), 6, 4, 8.4, 4.4));
                facts.Add(new Fact(new DateTime(2016, 11, 27, 16, 0, 0), 16, 14, 4.4, 14.1));

                facts.Add(new Fact(new DateTime(2016, 11, 28, 12, 30, 0), 8, 7, 7.7, 6.6));
                facts.Add(new Fact(new DateTime(2016, 11, 28, 18, 30, 0), 16, 12, 6.6, 8.8));
                facts.Add(new Fact(new DateTime(2016, 11, 29, 0, 30, 0), 12, 7, 8.8, 7.8));

                facts.Add(new Fact(new DateTime(2016, 11, 29, 7, 30, 0), 7, 5, 7.2, 7.1));
                facts.Add(new Fact(new DateTime(2016, 11, 29, 15, 0, 0), 16, 12, 7.1, 7.0));
                facts.Add(new Fact(new DateTime(2016, 11, 29, 19, 30, 0), 16, 10, 7.0, 3.5));
                facts.Add(new Fact(new DateTime(2016, 11, 29, 23, 0, 0), 0, 5, 3.5, 16.5));

                facts.Add(new Fact(new DateTime(2016, 11, 30, 18, 0, 0), 22, 14, 10.3, 16.3));
                facts.Add(new Fact(new DateTime(2016, 11, 30, 23, 0, 0), 6, 0, 16.3, 5.6));

                facts.Add(new Fact(new DateTime(2016, 12, 1, 8, 30, 0), 6, 5, 5.6, 8.9));

                facts.Add(new Fact(new DateTime(2016, 12, 3, 9, 30, 0), 8, 6, 12.9, 12.8));
                facts.Add(new Fact(new DateTime(2016, 12, 3, 13, 0, 0), 18, 13, 12.8, 3.5));
                facts.Add(new Fact(new DateTime(2016, 12, 3, 16, 0, 0), 5, 5, 3.5, 7.3));

                facts.Add(new Fact(new DateTime(2016, 12, 5, 8, 0, 0), 8, 5, 9.9, 6.4));
                facts.Add(new Fact(new DateTime(2016, 12, 5, 12, 0, 0), 9, 9, 6.4, 9.9));
                facts.Add(new Fact(new DateTime(2016, 12, 5, 18, 0, 0), 10, 7, 9.9, 11.2));

                facts.Add(new Fact(new DateTime(2016, 12, 6, 8, 0, 0), 12, 5, 14.8, 3.5));
                facts.Add(new Fact(new DateTime(2016, 12, 6, 13, 0, 0), 8, 6, 3.5, 10.7));
                facts.Add(new Fact(new DateTime(2016, 12, 6, 21, 0, 0), 14, 10, 10.7, 12.5));

                facts.Add(new Fact(new DateTime(2016, 12, 7, 8, 0, 0), 10, 5, 12.4, 12.4));
                facts.Add(new Fact(new DateTime(2016, 12, 7, 13, 0, 0), 6, 7, 4.0, 4.0));
                facts.Add(new Fact(new DateTime(2016, 12, 7, 18, 0, 0), 16, 11, 12.4, 19.7));
                facts.Add(new Fact(new DateTime(2016, 12, 7, 23, 0, 0), 10, 3, 19.7, 12.6));

                facts.Add(new Fact(new DateTime(2016, 12, 8, 16, 0, 0), 12, 12, 17.8, 18.6));
                facts.Add(new Fact(new DateTime(2016, 12, 8, 23, 0, 0), 6, 3, 18.6, 17.6));

                facts.Add(new Fact(new DateTime(2016, 12, 10, 13, 30, 0), 8, 6, 13.5, 17.8));
                facts.Add(new Fact(new DateTime(2016, 12, 10, 17, 20, 0), 10, 6, 17.8, 13.0));
                facts.Add(new Fact(new DateTime(2016, 12, 10, 19, 15, 0), 8, 6, 13.0, 12.2));

                facts.Add(new Fact(new DateTime(2016, 12, 11, 13, 0, 0), 9, 5, 11.9, 7.8));
                facts.Add(new Fact(new DateTime(2016, 12, 11, 15, 30, 0), 13, 10, 7.8, 18.4));
                facts.Add(new Fact(new DateTime(2016, 12, 11, 21, 30, 0), 8, 2, 19.4, 6.0));

                facts.Add(new Fact(new DateTime(2016, 12, 12, 18, 30, 0), 24, 16, 14.8, 18.9));
                facts.Add(new Fact(new DateTime(2016, 12, 13, 2, 0, 0), 8, 0, 18.9, 9.1));

                facts.Add(new Fact(new DateTime(2016, 12, 13, 18, 0, 0), 18, 12, 10.7, 5.3));
                facts.Add(new Fact(new DateTime(2016, 12, 13, 23, 30, 0), 8, 5, 5.3, 16.3));
                facts.Add(new Fact(new DateTime(2016, 12, 14, 3, 40, 0), 8, 0, 18.4, 5.9));

                facts.Add(new Fact(new DateTime(2016, 12, 14, 7, 45, 0), 6, 5, 5.9, 10.5));
                facts.Add(new Fact(new DateTime(2016, 12, 14, 13, 0, 0), 10, 7, 10.5, 14.9));
                facts.Add(new Fact(new DateTime(2016, 12, 14, 23, 40, 0), 16, 14, 14.9, 13.9));

                facts.Add(new Fact(new DateTime(2016, 12, 15, 9, 30, 0), 10, 5, 17.9, 8.7));
                facts.Add(new Fact(new DateTime(2016, 12, 15, 14, 0, 0), 8, 7, 8.7, 8.0));

                #endregion
                */
                db.SaveChanges();
                comboBoxPatient.Items.Add(db.PatientSet.First().Name);

            }
        }
    }
}
