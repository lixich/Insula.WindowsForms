using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InsulaWindowsForms
{
    public partial class NewDose : Form
    {
        List<Fact> lst = new List<Fact>();

        public NewDose(Patient p)
        {
            InitializeComponent();
            lst = p.Facts;
            tbTime.Text = DateTime.Now.TimeOfDay.ToString();
            numCurrentGlucose.Value = 6;
            cmbNumberNeighbors.Text = "5";
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            try
            {
                double XE = (double)numXE.Value;
                double before = (double)numCurrentGlucose.Value;
                double after = (double)numPlanGlucose.Value;
                TimeSpan time = TimeSpan.Parse(tbTime.Text);
                /*
                double d1 = (XE - lst.Select(x => x.XE).Min()) / (lst.Select(x => x.XE).Max() - lst.Select(x => x.XE).Min());
                double d2 = (before - lst.Select(x => x.Before).Min()) / (lst.Select(x => x.Before).Max() - lst.Select(x => x.Before).Min()));
                double d3 = (after - lst.Select(x => x.After).Min()) / (lst.Select(x => x.After).Max() - lst.Select(x => x.After).Min()));
                double d4 = (time.TotalMinutes) / (24 * 60);
                 * */
                foreach (Fact item in lst)
                {
                    
                    double d1 = (Math.Abs(item.XE - XE)/(lst.Select(x => x.XE).Max() - lst.Select(x => x.XE).Min()));
                    double d2 = (Math.Abs(item.Before - before) / (lst.Select(x => x.Before).Max() - lst.Select(x => x.Before).Min()));
                    double d3 = (Math.Abs(item.After - after) / (lst.Select(x => x.After).Max() - lst.Select(x => x.After).Min()));
                    double d4 = (Math.Min(Math.Abs((item.DateTime.Value.TimeOfDay - time).TotalMinutes), 24 * 60 - Math.Abs((item.DateTime.Value.TimeOfDay - time).TotalMinutes))) / (24 * 60);
                    item.Coef = Math.Sqrt(d1*d1 + d2*d2 + d3*d3 + d4*d4);
                }
                lst = lst.OrderBy(x => x.Coef).ToList();
                double dose = 0;
                double accuracy = 0;
                int numNeighbors = int.Parse(cmbNumberNeighbors.Text);
                for (int i = 0; i < numNeighbors; i++)
                {
                    dose += lst[i].Dose;
                    accuracy += lst[i].Coef;
                }
                dose /= numNeighbors;
                accuracy /= numNeighbors;
                tbDoseKNN.Text = ((int)dose).ToString();
                tbAccuracy.Text = Math.Round(accuracy, 2).ToString();
                
                dataGridView1.Rows.Clear();
                dataGridView1.ColumnCount = 6;
                dataGridView1.ColumnHeadersVisible = true;

                dataGridView1.Columns[0].Name = "Time";
                dataGridView1.Columns[1].Name = "XE";
                dataGridView1.Columns[2].Name = "Glucose before meal";
                dataGridView1.Columns[3].Name = "Glucose after meal";
                dataGridView1.Columns[4].Name = "Dose";
                dataGridView1.Columns[5].Name = "Euclidean distance";

                foreach (Fact f in lst)
                   dataGridView1.Rows.Add(new string[] { f.DateTime.Value.TimeOfDay.ToString(), f.XE.ToString(), f.Before.ToString(), f.After.ToString(), f.Dose.ToString(), Math.Round(f.Coef,2).ToString() });
                for (int i = 0; i < numNeighbors; i++)
                {
                   dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                }



                //----------------
                List<Fact> goodFacts = lst.Where(x => Math.Abs(x.After - x.Before) <= 1.5).ToList();
                List<Fact> badFacts = lst.Where(x => Math.Abs(x.After - x.Before) >= 1).ToList();

                foreach (var item in goodFacts)
                    if (item.XE != 0)
                        item.InsXE = Math.Round(item.Dose / item.XE, 4);

                goodFacts = goodFacts.Where(x => x.InsXE > 0).ToList();

                double[] xdataXE = goodFacts.Select(x => x.Time).ToArray();
                double[] ydataXE = goodFacts.Select(x => x.InsXE).ToArray();

                double omega = Math.PI / 12;
                Func<double, double> harmonicCombinationXE = Fit.LinearCombinationFunc(xdataXE, ydataXE,
                x => 1,
                x => SpecialFunctions.GeneralHarmonic(100, Math.Sin(omega * x)),
                x => SpecialFunctions.GeneralHarmonic(100, Math.Cos(omega * x)));
                double goodnessHarmonicCombinationXE = GoodnessOfFit.RSquared(xdataXE.Select(x => harmonicCombinationXE(x)), ydataXE);

                foreach (var item in badFacts)
                {
                    //if (item.Before > item.After)
                    if (item.Before - item.After != 0)
                    {
                        item.InsGlu = Math.Round((item.Dose - (harmonicCombinationXE(item.Time) * item.XE)) / (item.Before - item.After), 4);
                    }
                }
                badFacts = badFacts.Where(x => x.InsGlu > 0).ToList();

                double[] xdataGlucose = badFacts.Select(x => x.Time).ToArray();
                double[] ydataGlucose = badFacts.Select(x => x.InsGlu).ToArray();

                Func<double, double> harmonicCombinationGlucose = Fit.LinearCombinationFunc(
                xdataGlucose,
                ydataGlucose,
                x => 1,
                x => SpecialFunctions.GeneralHarmonic(100, Math.Sin(omega * x)),
                x => SpecialFunctions.GeneralHarmonic(100, Math.Cos(omega * x)));
                double goodnessHarmonicCombinationGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => harmonicCombinationGlucose(x)), ydataGlucose);

                double example = harmonicCombinationXE(time.TotalMinutes/60) * XE + (before - 6) * harmonicCombinationGlucose(time.TotalMinutes / 60);
                tbDose.Text = ((int)example).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
