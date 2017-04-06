using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using System.Numerics;
using System.Windows.Forms.DataVisualization.Charting;
using MathNet.Numerics.LinearRegression;

namespace InsulaWindowsForms
{
    public partial class FormStatistics : Form
    {
        public Patient currentPatient = new Patient();
        public List<Fact> lstFacts = new List<Fact>();

        public void UpdateDataGridView()
        {
            Patient p = currentPatient;
            dataGridViewAll.DataSource = null;
            dataGridViewAll.Rows.Clear();
            dataGridViewAll.ColumnCount = 9;
            dataGridViewAll.ColumnHeadersVisible = true;

            dataGridViewAll.Columns[0].Name = "Date";
            dataGridViewAll.Columns[1].Name = "TimeOfDay";
            dataGridViewAll.Columns[2].Name = "XE";
            dataGridViewAll.Columns[3].Name = "Glucose before meal";
            dataGridViewAll.Columns[4].Name = "Glucose after meal";
            dataGridViewAll.Columns[5].Name = "Dose";
            dataGridViewAll.Columns[6].Name = "Dose for 1 XE";
            dataGridViewAll.Columns[7].Name = "Dose to lower glucose by 1 mmol/L";
            dataGridViewAll.Columns[8].Name = "Comment";

            foreach (Fact f in p.Facts.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) <= trackBarGlucose.Value && x.XE <= trackBarXE.Value).ToList())
                if ((f.After == 0) || (f.After == 0))
                {
                    dataGridViewAll.Rows.Add(new string[] { f.DateTime.ToString().Substring(0, 10), f.DateTime.TimeOfDay.ToString(), f.XE.ToString(), f.Before.ToString(), f.After.ToString(), f.Dose.ToString(), f.InsXE != 0 ? f.InsXE.ToString() : "-", f.InsGlu != 0 ? f.InsGlu.ToString() : "-", "Error" });
                }
                else
                {
                    if ((f.After > 8) || (f.After < 4))
                        dataGridViewAll.Rows.Add(new string[] { f.DateTime.ToString().Substring(0, 10), f.DateTime.TimeOfDay.ToString(), f.XE.ToString(), f.Before.ToString(), f.After.ToString(), f.Dose.ToString(), f.InsXE != 0 ? f.InsXE.ToString() : "-", f.InsGlu != 0 ? f.InsGlu.ToString() : "-", "Bad level of glucose" });
                    else
                        dataGridViewAll.Rows.Add(new string[] { f.DateTime.ToString().Substring(0, 10), f.DateTime.TimeOfDay.ToString(), f.XE.ToString(), f.Before.ToString(), f.After.ToString(), f.Dose.ToString(), f.InsXE != 0 ? f.InsXE.ToString() : "-", f.InsGlu != 0 ? f.InsGlu.ToString() : "-", "" });
                }
            foreach (DataGridViewRow item in dataGridViewAll.Rows)
            {
                if (double.Parse(item.Cells[3].Value.ToString()) < 4)
                    item.Cells[3].Style.ForeColor = Color.Blue;
                if (double.Parse(item.Cells[3].Value.ToString()) < 4)
                    item.Cells[4].Style.ForeColor = Color.Blue;
                if (double.Parse(item.Cells[3].Value.ToString()) > 8)
                    item.Cells[3].Style.ForeColor = Color.Red;
                if (double.Parse(item.Cells[4].Value.ToString()) > 8)
                    item.Cells[4].Style.ForeColor = Color.Red;
            }
        }

        public FormStatistics(Patient p)
        {
            InitializeComponent();
            currentPatient = p;
            lstFacts = p.Facts;
            UpdateDataGridView();
            comboBoxGlucose.SelectedIndex = 0;
            comboBoxXE.SelectedIndex = 0;
            trackBarGlucose.Maximum = (int)Math.Ceiling(lstFacts.Select(x => x.Before).Max());
            trackBarXE.Maximum = (int)Math.Ceiling(lstFacts.Select(x => x.XE).Max());
            trackBarGlucose.Minimum = (int)Math.Ceiling(lstFacts.Select(x => x.Before).Min());
            trackBarXE.Minimum = (int)Math.Ceiling(lstFacts.Select(x => x.XE).Min());
        }

        private void comboBoxXE_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Fact> facts = lstFacts.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) <= trackBarGlucose.Value && x.XE <= trackBarXE.Value).ToList();
            try
            {
                chart2D.Legends[0].Title = "Data";
                switch (comboBoxXE.SelectedItem.ToString())
                {
                    case "Dose for 1 XE(Time)":
                        {
                            List<Fact> goodFacts = facts.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) < (double)numericUpDownCoefForGoodFacts.Value).ToList();
                            List<Fact> badFacts = facts.Where(x => Math.Abs(x.After - x.Before) >= 1).ToList();

                            foreach (var item in goodFacts)
                                if (item.XE != 0)
                                    item.InsXE = Math.Round(item.Dose / item.XE, 4);

                            goodFacts = goodFacts.Where(x => x.InsXE > 0).ToList();

                            double[] xdataXE = goodFacts.Select(x => x.Time).ToArray();
                            double[] ydataXE = goodFacts.Select(x => x.InsXE).ToArray();
                            double[] zdataXE = goodFacts.Select(x => x.XE).ToArray();

                            Func<double, double> lineXE = Fit.LineFunc(xdataXE, ydataXE);
                            double goodnessLineXE = GoodnessOfFit.RSquared(xdataXE.Select(x => lineXE(x)), ydataXE);

                            Func<double, double> polynomOrder2XE = Fit.PolynomialFunc(xdataXE, ydataXE, 2); // polynomial of order 2
                            double goodnessPolynomOrder2XE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder2XE(x)), ydataXE);

                            Func<double, double> polynomOrder3XE = Fit.PolynomialFunc(xdataXE, ydataXE, 3); // polynomial of order 3
                            double goodnessPolynomOrder3XE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder3XE(x)), ydataXE);

                            double omega = Math.PI / 12;
                            Func<double, double> sinusoidalCombinationXE = Fit.LinearCombinationFunc(xdataXE, ydataXE,
                            x => 1.0,
                            x => Math.Sin(omega * x),
                            x => Math.Cos(omega * x));
                            double goodnessSinusoidalCombinationXE = GoodnessOfFit.RSquared(xdataXE.Select(x => sinusoidalCombinationXE(x)), ydataXE);

                            Func<double, double> harmonicCombinationXE = Fit.LinearCombinationFunc(xdataXE, ydataXE,
                            x => 1,
                            x => SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * x)),
                            x => SpecialFunctions.GeneralHarmonic(10, Math.Cos(omega * x)));
                            double goodnessHarmonicCombinationXE = GoodnessOfFit.RSquared(xdataXE.Select(x => harmonicCombinationXE(x)), ydataXE);

                            double exampleLXE = lineXE(13.0);
                            double exampleP2XE = polynomOrder2XE(13.0);
                            double exampleP3XE = polynomOrder3XE(13.0);
                            double exampleLCXE = sinusoidalCombinationXE(13.0);
                            double exampleNCXE = harmonicCombinationXE(13.0);
                            chart2D.Series.Clear();
                            chart2D.Series.Add("line");
                            chart2D.Series.Add("polynom");
                            chart2D.Series.Add("sinusoidal");
                            chart2D.Series.Add("harmonic");
                            chart2D.Series.Add("data");
                            chart2D.ChartAreas[0].AxisX.Minimum = 0;
                            chart2D.ChartAreas[0].AxisX.Maximum = 24;
                            chart2D.ChartAreas[0].AxisX.Interval = 2;
                            chart2D.ChartAreas[0].AxisY.Minimum = 0;
                            chart2D.ChartAreas[0].AxisY.Maximum = (ydataXE.Max() > 10) ? 10 : Math.Ceiling(ydataXE.Max());
                            chart2D.ChartAreas[0].AxisY.Interval = 0.5;
                            chart2D.Series["data"].ChartType = SeriesChartType.Point;
                            chart2D.Series["data"].Color = Color.Black;
                            chart2D.Series["line"].ChartType = SeriesChartType.Line;
                            chart2D.Series["polynom"].ChartType = SeriesChartType.Spline;
                            chart2D.Series["sinusoidal"].ChartType = SeriesChartType.Spline;
                            chart2D.Series["harmonic"].ChartType = SeriesChartType.Spline;
                            chart2D.Series["data"].MarkerSize = 7;
                            chart2D.Series["line"].BorderWidth = 4;
                            chart2D.Series["polynom"].BorderWidth = 4;
                            chart2D.Series["sinusoidal"].BorderWidth = 4;
                            chart2D.Series["harmonic"].BorderWidth = 4;
                            chart2D.ChartAreas[0].AxisX.Title = "Time";
                            chart2D.ChartAreas[0].AxisY.Title = "Insulin dose for 1 XE";
                            for (int i = 0; i < 25; i++)
                            {
                                if (Math.Abs(lineXE(i)) < 10000)
                                    chart2D.Series["line"].Points.AddXY(i, Math.Round(lineXE(i), 2));
                                if (Math.Abs(polynomOrder3XE(i)) < 10000)
                                    chart2D.Series["polynom"].Points.AddXY(i, Math.Round(polynomOrder3XE(i), 2));
                                if (Math.Abs(sinusoidalCombinationXE(i)) < 10000)
                                    chart2D.Series["sinusoidal"].Points.AddXY(i, Math.Round(sinusoidalCombinationXE(i), 2));
                                if (Math.Abs(harmonicCombinationXE(i)) < 10000)
                                    chart2D.Series["harmonic"].Points.AddXY(i, Math.Round(harmonicCombinationXE(i), 2));

                            }
                            for (int i = 0; i < xdataXE.Length; i++)
                            {
                                chart2D.Series["data"].Points.AddXY(xdataXE[i], ydataXE[i]);
                            }
                            chart2D.Legends[0].Title = "Function (Determination)";
                            chart2D.Series["line"].Name += " (" + Math.Round(goodnessLineXE, 2).ToString() + ")";
                            chart2D.Series["polynom"].Name += " (" + Math.Round(goodnessPolynomOrder3XE, 2).ToString() + ")";
                            chart2D.Series["sinusoidal"].Name += " (" + Math.Round(goodnessSinusoidalCombinationXE, 2).ToString() + ")";
                            chart2D.Series["harmonic"].Name += " (" + Math.Round(goodnessHarmonicCombinationXE, 2).ToString() + ")";
                            IEnumerable<double> a = (IEnumerable<double>)chart2D.Series["data"].Points.SelectMany(x => x.YValues.Select(y => x.XValue)).ToArray<double>();
                            IEnumerable<double> b = (IEnumerable<double>)chart2D.Series["data"].Points.SelectMany(x => x.YValues).ToArray<double>();
                            double correl = Correlation.Pearson(a, b);
                            chart2D.Series["data"].Name += (" Correlation(" + Math.Round(correl, 2).ToString() + ")");
                        }
                        break;
                    case "Dose to lower glucose by 1 mmol/L(Time)":
                        {
                            List<Fact> goodFacts = facts.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) < (double)numericUpDownCoefForGoodFacts.Value).ToList();
                            List<Fact> badFacts = facts.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) >= 1).ToList();

                            foreach (var item in goodFacts)
                                if (item.XE != 0)
                                    item.InsXE = Math.Round(item.Dose / item.XE, 4);

                            goodFacts = goodFacts.Where(x => x.InsXE > 0).ToList();

                            double[] xdataXE = goodFacts.Select(x => x.Time).ToArray();
                            double[] ydataXE = goodFacts.Select(x => x.InsXE).ToArray();
                            double[] zdataXE = goodFacts.Select(x => x.XE).ToArray();

                            Func<double, double> lineXE = Fit.LineFunc(xdataXE, ydataXE);
                            double goodnessLineXE = GoodnessOfFit.RSquared(xdataXE.Select(x => lineXE(x)), ydataXE);

                            Func<double, double> polynomOrder2XE = Fit.PolynomialFunc(xdataXE, ydataXE, 2); // polynomial of order 2
                            double goodnessPolynomOrder2XE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder2XE(x)), ydataXE);

                            Func<double, double> polynomOrder3XE = Fit.PolynomialFunc(xdataXE, ydataXE, 3); // polynomial of order 3
                            double goodnessPolynomOrder3XE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder3XE(x)), ydataXE);

                            double omega = Math.PI / 12;
                            Func<double, double> sinusoidalCombinationXE = Fit.LinearCombinationFunc(xdataXE, ydataXE,
                            x => 1.0,
                            x => Math.Sin(omega * x),
                            x => Math.Cos(omega * x));
                            double goodnessSinusoidalCombinationXE = GoodnessOfFit.RSquared(xdataXE.Select(x => sinusoidalCombinationXE(x)), ydataXE);

                            Func<double, double> harmonicCombinationXE = Fit.LinearCombinationFunc(xdataXE, ydataXE,
                            x => 1,
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Sin(omega * x)),
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Cos(omega * x)));
                            double goodnessHarmonicCombinationXE = GoodnessOfFit.RSquared(xdataXE.Select(x => harmonicCombinationXE(x)), ydataXE);

                            double exampleLXE = lineXE(13.0);
                            double exampleP2XE = polynomOrder2XE(13.0);
                            double exampleP3XE = polynomOrder3XE(13.0);
                            double exampleLCXE = sinusoidalCombinationXE(13.0);
                            double exampleNCXE = harmonicCombinationXE(13.0);

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
                            double[] zdataGlucose = badFacts.Select(x => x.XE).ToArray();

                            Func<double, double> lineGlucose = Fit.LineFunc(xdataGlucose, ydataGlucose);
                            double goodnessLineGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => lineGlucose(x)), ydataGlucose);

                            Func<double, double> polynomOrder2Glucose = Fit.PolynomialFunc(xdataGlucose, ydataGlucose, 2); // polynomial of order 2
                            double goodnessPolynomOrder2Glucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => polynomOrder2Glucose(x)), ydataGlucose);

                            Func<double, double> polynomOrder3Glucose = Fit.PolynomialFunc(xdataGlucose, ydataGlucose, 3); // polynomial of order 3
                            double goodnessPolynomOrder3Glucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => polynomOrder3Glucose(x)), ydataGlucose);

                            Func<double, double> sinusoidalCombinationGlucose = Fit.LinearCombinationFunc(
                            xdataGlucose,
                            ydataGlucose,
                            x => 1.0,
                            x => Math.Sin(omega * x),
                            x => Math.Cos(omega * x));
                            double goodnessSinusoidalCombinationGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => sinusoidalCombinationGlucose(x)), ydataGlucose);

                            Func<double, double> harmonicCombinationGlucose = Fit.LinearCombinationFunc(
                            xdataGlucose,
                            ydataGlucose,
                            x => 1,
                            x => SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * x)),
                            x => SpecialFunctions.GeneralHarmonic(10, Math.Cos(omega * x)));
                            double goodnessHarmonicCombinationGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => harmonicCombinationGlucose(x)), ydataGlucose);

                            //пример
                            double exampleLGlucose = lineGlucose(13.0);
                            double exampleP2Glucose = polynomOrder2Glucose(13.0);
                            double exampleP3Glucose = polynomOrder3Glucose(13.0);
                            double exampleLCGlucose = sinusoidalCombinationGlucose(13.0);
                            double exampleHCGlucose = harmonicCombinationGlucose(13.0);

                            double nowGlu = 9.9;
                            double nowXE = 4;
                            double nowTime = 13;
                            //double nowInsulin = ?;

                            double exampleLFull = lineXE(nowTime) * nowXE + (nowGlu - 6) * lineGlucose(nowTime);
                            double exampleP2Full = polynomOrder2XE(nowTime) * nowXE + (nowGlu - 6) * polynomOrder2Glucose(nowTime);
                            double exampleP3Full = polynomOrder3XE(nowTime) * nowXE + (nowGlu - 6) * polynomOrder3Glucose(nowTime);
                            double exampleLCFull = sinusoidalCombinationXE(nowTime) * nowXE + (nowGlu - 6) * sinusoidalCombinationGlucose(nowTime);
                            double exampleHCFull = harmonicCombinationXE(nowTime) * nowXE + (nowGlu - 6) * harmonicCombinationGlucose(nowTime);

                            chart2D.Series.Clear();
                            chart2D.Series.Add("line");
                            chart2D.Series.Add("polynom");
                            chart2D.Series.Add("sinusoidal");
                            chart2D.Series.Add("harmonic");
                            chart2D.Series.Add("data");
                            chart2D.ChartAreas[0].AxisX.Minimum = 0;
                            chart2D.ChartAreas[0].AxisX.Maximum = 24;
                            chart2D.ChartAreas[0].AxisX.Interval = 2;
                            chart2D.ChartAreas[0].AxisY.Minimum = 0;
                            chart2D.ChartAreas[0].AxisY.Maximum = (ydataGlucose.Max() > 10) ? 10 : Math.Ceiling(ydataGlucose.Max());
                            chart2D.ChartAreas[0].AxisY.Interval = 0.5;
                            chart2D.Series["data"].ChartType = SeriesChartType.Point;
                            chart2D.Series["data"].Color = Color.Black;
                            chart2D.Series["line"].ChartType = SeriesChartType.Line;
                            chart2D.Series["polynom"].ChartType = SeriesChartType.Spline;
                            chart2D.Series["sinusoidal"].ChartType = SeriesChartType.Spline;
                            chart2D.Series["harmonic"].ChartType = SeriesChartType.Spline;
                            chart2D.Series["data"].MarkerSize = 7;
                            chart2D.Series["line"].BorderWidth = 4;
                            chart2D.Series["polynom"].BorderWidth = 4;
                            chart2D.Series["sinusoidal"].BorderWidth = 4;
                            chart2D.Series["harmonic"].BorderWidth = 4;
                            chart2D.ChartAreas[0].AxisX.Title = "Time";
                            chart2D.ChartAreas[0].AxisY.Title = "Insulin dose for 1 XE";
                            for (int i = 0; i < 25; i++)
                            {
                                if (Math.Abs(lineGlucose(i)) < 10000)
                                    chart2D.Series["line"].Points.AddXY(i, Math.Round(lineGlucose(i), 2));
                                if (Math.Abs(polynomOrder3Glucose(i)) < 10000)
                                    chart2D.Series["polynom"].Points.AddXY(i, Math.Round(polynomOrder3Glucose(i), 2));
                                if (Math.Abs(sinusoidalCombinationGlucose(i)) < 10000)
                                    chart2D.Series["sinusoidal"].Points.AddXY(i, Math.Round(sinusoidalCombinationGlucose(i), 2));
                                if (Math.Abs(harmonicCombinationGlucose(i)) < 10000)
                                    chart2D.Series["harmonic"].Points.AddXY(i, Math.Round(harmonicCombinationGlucose(i), 2));

                            }
                            for (int i = 0; i < xdataGlucose.Length; i++)
                            {
                                chart2D.Series["data"].Points.AddXY(xdataGlucose[i], ydataGlucose[i]);
                            }
                            chart2D.Legends[0].Title = "Function (Determination)";
                            chart2D.Series["line"].Name += " (" + Math.Round(goodnessLineGlucose, 2).ToString() + ")";
                            chart2D.Series["polynom"].Name += " (" + Math.Round(goodnessPolynomOrder3Glucose, 2).ToString() + ")";
                            chart2D.Series["sinusoidal"].Name += " (" + Math.Round(goodnessSinusoidalCombinationGlucose, 2).ToString() + ")";
                            chart2D.Series["harmonic"].Name += " (" + Math.Round(goodnessHarmonicCombinationGlucose, 2).ToString() + ")";
                            IEnumerable<double> a = (IEnumerable<double>)chart2D.Series["data"].Points.SelectMany(x => x.YValues.Select(y => x.XValue)).ToArray<double>();
                            IEnumerable<double> b = (IEnumerable<double>)chart2D.Series["data"].Points.SelectMany(x => x.YValues).ToArray<double>();
                            double correl = Correlation.Pearson(a, b);
                            chart2D.Series["data"].Name += (" Correlation(" + Math.Round(correl, 2).ToString() + ")");
                        }
                        break;
                    case "//Dose for XE(XE)":
                        {
                            List<Fact> goodFacts = facts.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) < (double)numericUpDownCoefForGoodFacts.Value).ToList();
                            List<Fact> badFacts = facts.Where(x => Math.Abs(x.After - x.Before) >= 1).ToList();

                            foreach (var item in goodFacts)
                                if (item.XE != 0)
                                    item.InsXE = Math.Round(item.Dose / item.XE, 4);

                            goodFacts = goodFacts.Where(x => x.InsXE > 0).ToList();

                            double[] xdataGood = goodFacts.Select(x => x.XE).ToArray();
                            double[] ydataGood = goodFacts.Select(x => x.Dose).ToArray();
                            double[] zdataGood = goodFacts.Select(x => x.Time).ToArray();

                            Func<double, double> lineXE = Fit.LineFunc(xdataGood, ydataGood);
                            double goodnessLineXE = GoodnessOfFit.RSquared(xdataGood.Select(x => lineXE(x)), ydataGood);

                            Func<double, double> polynomOrder2XE = Fit.PolynomialFunc(xdataGood, ydataGood, 2); // polynomial of order 2
                            double goodnessPolynomOrder2XE = GoodnessOfFit.RSquared(xdataGood.Select(x => polynomOrder2XE(x)), ydataGood);

                            Func<double, double> polynomOrder3XE = Fit.PolynomialFunc(xdataGood, ydataGood, 3); // polynomial of order 3
                            double goodnessPolynomOrder3XE = GoodnessOfFit.RSquared(xdataGood.Select(x => polynomOrder3XE(x)), ydataGood);

                            double omega = Math.PI / 12;
                            Func<double, double> sinusoidalCombinationXE = Fit.LinearCombinationFunc(xdataGood, ydataGood,
                            x => 1.0,
                            x => Math.Sin(omega * x),
                            x => Math.Cos(omega * x));
                            double goodnessSinusoidalCombinationXE = GoodnessOfFit.RSquared(xdataGood.Select(x => sinusoidalCombinationXE(x)), ydataGood);

                            Func<double, double> harmonicCombinationXE = Fit.LinearCombinationFunc(xdataGood, ydataGood,
                            x => 1,
                            x => SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * x)),
                            x => SpecialFunctions.GeneralHarmonic(10, Math.Cos(omega * x)));
                            double goodnessHarmonicCombinationXE = GoodnessOfFit.RSquared(xdataGood.Select(x => harmonicCombinationXE(x)), ydataGood);

                            double exampleLXE = lineXE(5);
                            double exampleP2XE = polynomOrder2XE(5);
                            double exampleP3XE = polynomOrder3XE(5);
                            double exampleLCXE = sinusoidalCombinationXE(5);
                            double exampleNCXE = harmonicCombinationXE(5);
                            chart2D.Series.Clear();
                            chart2D.Series.Add("line");
                            chart2D.Series.Add("polynom");
                            chart2D.Series.Add("sinusoidal");
                            chart2D.Series.Add("harmonic");
                            chart2D.Series.Add("data");
                            chart2D.ChartAreas[0].AxisX.Minimum = 0;
                            chart2D.ChartAreas[0].AxisX.Maximum = Math.Ceiling(xdataGood.Max());
                            chart2D.ChartAreas[0].AxisX.Interval = double.NaN;
                            chart2D.ChartAreas[0].AxisY.Minimum = 0;
                            chart2D.ChartAreas[0].AxisY.Maximum = Math.Ceiling(ydataGood.Max());
                            chart2D.ChartAreas[0].AxisY.Interval = double.NaN;
                            chart2D.Series["data"].ChartType = SeriesChartType.Point;
                            chart2D.Series["data"].Color = Color.Black;
                            chart2D.Series["line"].ChartType = SeriesChartType.Line;
                            chart2D.Series["polynom"].ChartType = SeriesChartType.Spline;
                            chart2D.Series["sinusoidal"].ChartType = SeriesChartType.Spline;
                            chart2D.Series["harmonic"].ChartType = SeriesChartType.Spline;
                            chart2D.Series["data"].MarkerSize = 7;
                            chart2D.Series["line"].BorderWidth = 4;
                            chart2D.Series["polynom"].BorderWidth = 4;
                            chart2D.Series["sinusoidal"].BorderWidth = 4;
                            chart2D.Series["harmonic"].BorderWidth = 4;
                            chart2D.ChartAreas[0].AxisX.Title = "XE";
                            chart2D.ChartAreas[0].AxisY.Title = "Insulin dose";
                            for (int i = 0; i < (int)xdataGood.Max() + 1; i++)
                            {
                                if (Math.Abs(lineXE(i)) < 10000)
                                    chart2D.Series["line"].Points.AddXY(i, Math.Round(lineXE(i), 2));
                                if (Math.Abs(polynomOrder3XE(i)) < 10000)
                                    chart2D.Series["polynom"].Points.AddXY(i, Math.Round(polynomOrder3XE(i), 2));
                                if (Math.Abs(sinusoidalCombinationXE(i)) < 10000)
                                    chart2D.Series["sinusoidal"].Points.AddXY(i, Math.Round(sinusoidalCombinationXE(i), 2));
                                if (Math.Abs(harmonicCombinationXE(i)) < 10000)
                                    chart2D.Series["harmonic"].Points.AddXY(i, Math.Round(harmonicCombinationXE(i), 2));

                            }
                            for (int i = 0; i < xdataGood.Length; i++)
                            {
                                chart2D.Series["data"].Points.AddXY(xdataGood[i], ydataGood[i]);
                            }
                            chart2D.Legends[0].Title = "Function (Determination)";
                            chart2D.Series["line"].Name += " (" + Math.Round(goodnessLineXE, 2).ToString() + ")";
                            chart2D.Series["polynom"].Name += " (" + Math.Round(goodnessPolynomOrder3XE, 2).ToString() + ")";
                            chart2D.Series["sinusoidal"].Name += " (" + Math.Round(goodnessSinusoidalCombinationXE, 2).ToString() + ")";
                            chart2D.Series["harmonic"].Name += " (" + Math.Round(goodnessHarmonicCombinationXE, 2).ToString() + ")";
                            IEnumerable<double> a = (IEnumerable<double>)chart2D.Series["data"].Points.SelectMany(x => x.YValues.Select(y => x.XValue)).ToArray<double>();
                            IEnumerable<double> b = (IEnumerable<double>)chart2D.Series["data"].Points.SelectMany(x => x.YValues).ToArray<double>();
                            double correl = Correlation.Pearson(a, b);
                            chart2D.Series["data"].Name += (" Correlation(" + Math.Round(correl, 2).ToString() + ")");
                        }
                        break;
                    case "//Dose to lower glucose(Glucose)":
                        {
                            List<Fact> goodFacts = facts.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) < (double)numericUpDownCoefForGoodFacts.Value).ToList();
                            List<Fact> badFacts = facts.Where(x => Math.Abs(x.After - x.Before) >= 1).ToList();

                            foreach (var item in goodFacts)
                                if (item.XE != 0)
                                    item.InsXE = Math.Round(item.Dose / item.XE, 4);

                            goodFacts = goodFacts.Where(x => x.InsXE > 0).ToList();

                            double[] xdataGood = goodFacts.Select(x => x.XE).ToArray();
                            double[] ydataGood = goodFacts.Select(x => x.Dose).ToArray();
                            double[] zdataGood = goodFacts.Select(x => x.XE).ToArray();

                            Func<double, double> lineXE = Fit.LineFunc(xdataGood, ydataGood);
                            double goodnessLineXE = GoodnessOfFit.RSquared(xdataGood.Select(x => lineXE(x)), ydataGood);

                            Func<double, double> polynomOrder2XE = Fit.PolynomialFunc(xdataGood, ydataGood, 2); // polynomial of order 2
                            double goodnessPolynomOrder2XE = GoodnessOfFit.RSquared(xdataGood.Select(x => polynomOrder2XE(x)), ydataGood);

                            Func<double, double> polynomOrder3XE = Fit.PolynomialFunc(xdataGood, ydataGood, 3); // polynomial of order 3
                            double goodnessPolynomOrder3XE = GoodnessOfFit.RSquared(xdataGood.Select(x => polynomOrder3XE(x)), ydataGood);

                            double omega = Math.PI / 12;
                            Func<double, double> sinusoidalCombinationXE = Fit.LinearCombinationFunc(xdataGood, ydataGood,
                            x => 1.0,
                            x => Math.Sin(omega * x),
                            x => Math.Cos(omega * x));
                            double goodnessSinusoidalCombinationXE = GoodnessOfFit.RSquared(xdataGood.Select(x => sinusoidalCombinationXE(x)), ydataGood);

                            Func<double, double> harmonicCombinationXE = Fit.LinearCombinationFunc(xdataGood, ydataGood,
                            x => 1,
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Sin(omega * x)),
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Cos(omega * x)));
                            double goodnessHarmonicCombinationXE = GoodnessOfFit.RSquared(xdataGood.Select(x => harmonicCombinationXE(x)), ydataGood);

                            double exampleLXE = lineXE(5);
                            double exampleP2XE = polynomOrder2XE(5);
                            double exampleP3XE = polynomOrder3XE(5);
                            double exampleLCXE = sinusoidalCombinationXE(5);
                            double exampleNCXE = harmonicCombinationXE(5);

                            foreach (var item in badFacts)
                            {
                                //if (item.Before > item.After)
                                if (item.Before - item.After != 0)
                                {
                                    item.InsGlu = Math.Round(item.Dose - lineXE(item.XE), 4);
                                    if (item.Before - item.After < 0)
                                        item.InsGlu *= -1;
                                }
                                else item.InsGlu = 0;
                            }
                            badFacts = badFacts.Where(x => x.DifferenceInGlucose.HasValue && x.InsGlu > 0).ToList();

                            double[] xdataGlucose = badFacts.Select(x => Math.Abs(x.DifferenceInGlucose.Value)).ToArray();
                            double[] ydataGlucose = badFacts.Select(x => x.InsGlu).ToArray();
                            double[] zdataGlucose = badFacts.Select(x => x.Time).ToArray();

                            Func<double, double> lineGlucose = Fit.LineFunc(xdataGlucose, ydataGlucose);
                            double goodnessLineGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => lineGlucose(x)), ydataGlucose);

                            Func<double, double> polynomOrder2Glucose = Fit.PolynomialFunc(xdataGlucose, ydataGlucose, 2); // polynomial of order 2
                            double goodnessPolynomOrder2Glucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => polynomOrder2Glucose(x)), ydataGlucose);

                            Func<double, double> polynomOrder3Glucose = Fit.PolynomialFunc(xdataGlucose, ydataGlucose, 3); // polynomial of order 3
                            double goodnessPolynomOrder3Glucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => polynomOrder3Glucose(x)), ydataGlucose);

                            Func<double, double> sinusoidalCombinationGlucose = Fit.LinearCombinationFunc(
                            xdataGlucose,
                            ydataGlucose,
                            x => 1.0,
                            x => Math.Sin(omega * x),
                            x => Math.Cos(omega * x));
                            double goodnessSinusoidalCombinationGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => sinusoidalCombinationGlucose(x)), ydataGlucose);

                            Func<double, double> harmonicCombinationGlucose = Fit.LinearCombinationFunc(
                            xdataGlucose,
                            ydataGlucose,
                            x => 1,
                            x => SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * x)),
                            x => SpecialFunctions.GeneralHarmonic(10, Math.Cos(omega * x)));
                            double goodnessHarmonicCombinationGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => harmonicCombinationGlucose(x)), ydataGlucose);

                            //пример
                            double exampleLGlucose = lineGlucose(4);
                            double exampleP2Glucose = polynomOrder2Glucose(4);
                            double exampleP3Glucose = polynomOrder3Glucose(4);
                            double exampleLCGlucose = sinusoidalCombinationGlucose(4);
                            double exampleHCGlucose = harmonicCombinationGlucose(4);

                            double nowGlu = 9.9;
                            double nowXE = 4;
                            double nowTime = 13;
                            //double nowInsulin = ?;

                            double exampleLFull = lineXE(nowXE) + lineGlucose(nowGlu - 6);
                            double exampleP2Full = polynomOrder2XE(nowXE) + polynomOrder2Glucose(nowGlu - 6);
                            double exampleP3Full = polynomOrder3XE(nowXE) + polynomOrder3Glucose(nowGlu - 6);
                            double exampleLCFull = sinusoidalCombinationXE(nowXE) + sinusoidalCombinationGlucose(nowGlu - 6);
                            double exampleHCFull = harmonicCombinationXE(nowXE) + harmonicCombinationGlucose(nowGlu - 6);

                            chart2D.Series.Clear();
                            chart2D.Series.Add("line");
                            chart2D.Series.Add("polynom");
                            chart2D.Series.Add("sinusoidal");
                            chart2D.Series.Add("harmonic");
                            chart2D.Series.Add("data");
                            chart2D.ChartAreas[0].AxisX.Minimum = 0;
                            chart2D.ChartAreas[0].AxisX.Maximum = Math.Ceiling(xdataGlucose.Max());
                            chart2D.ChartAreas[0].AxisX.Interval = double.NaN;
                            chart2D.ChartAreas[0].AxisY.Minimum = 0;
                            chart2D.ChartAreas[0].AxisY.Maximum = Math.Ceiling(ydataGlucose.Max());
                            chart2D.ChartAreas[0].AxisY.Interval = double.NaN;
                            chart2D.Series["data"].ChartType = SeriesChartType.Point;
                            chart2D.Series["data"].Color = Color.Black;
                            chart2D.Series["line"].ChartType = SeriesChartType.Line;
                            chart2D.Series["polynom"].ChartType = SeriesChartType.Spline;
                            chart2D.Series["sinusoidal"].ChartType = SeriesChartType.Spline;
                            chart2D.Series["harmonic"].ChartType = SeriesChartType.Spline;
                            chart2D.Series["data"].MarkerSize = 7;
                            chart2D.Series["line"].BorderWidth = 4;
                            chart2D.Series["polynom"].BorderWidth = 4;
                            chart2D.Series["sinusoidal"].BorderWidth = 4;
                            chart2D.Series["harmonic"].BorderWidth = 4;
                            chart2D.ChartAreas[0].AxisX.Title = "Glucose";
                            chart2D.ChartAreas[0].AxisY.Title = "Insulin dose";
                            for (int i = 0; i < 25; i++)
                            {
                                if (Math.Abs(lineGlucose(i)) < 10000)
                                    chart2D.Series["line"].Points.AddXY(i, Math.Round(lineGlucose(i), 2));
                                if (Math.Abs(polynomOrder3Glucose(i)) < 10000)
                                    chart2D.Series["polynom"].Points.AddXY(i, Math.Round(polynomOrder3Glucose(i), 2));
                                if (Math.Abs(sinusoidalCombinationGlucose(i)) < 10000)
                                    chart2D.Series["sinusoidal"].Points.AddXY(i, Math.Round(sinusoidalCombinationGlucose(i), 2));
                                if (Math.Abs(harmonicCombinationGlucose(i)) < 10000)
                                    chart2D.Series["harmonic"].Points.AddXY(i, Math.Round(harmonicCombinationGlucose(i), 2));

                            }
                            for (int i = 0; i < xdataGlucose.Length; i++)
                            {
                                chart2D.Series["data"].Points.AddXY(xdataGlucose[i], ydataGlucose[i]);
                            }
                            chart2D.Legends[0].Title = "Function (Determination)";
                            chart2D.Series["line"].Name += " (" + Math.Round(goodnessLineGlucose, 2).ToString() + ")";
                            chart2D.Series["polynom"].Name += " (" + Math.Round(goodnessPolynomOrder3Glucose, 2).ToString() + ")";
                            chart2D.Series["sinusoidal"].Name += " (" + Math.Round(goodnessSinusoidalCombinationGlucose, 2).ToString() + ")";
                            chart2D.Series["harmonic"].Name += " (" + Math.Round(goodnessHarmonicCombinationGlucose, 2).ToString() + ")";
                            IEnumerable<double> a = (IEnumerable<double>)chart2D.Series["data"].Points.SelectMany(x => x.YValues.Select(y => x.XValue)).ToArray<double>();
                            IEnumerable<double> b = (IEnumerable<double>)chart2D.Series["data"].Points.SelectMany(x => x.YValues).ToArray<double>();
                            double correl = Correlation.Pearson(a, b);
                            chart2D.Series["data"].Name += (" Correlation(" + Math.Round(correl, 2).ToString() + ")");
                        }
                        break;
                    case "My XE(Time)":
                        {
                            List<Fact> f = new List<Fact>();
                            f = facts.Where(x => x.Before <= trackBarGlucose.Value && x.XE <= trackBarXE.Value).ToList();
                            chart2D.Series.Clear();
                            chart2D.Series.Add("meal");
                            chart2D.ChartAreas[0].AxisX.Minimum = 0;
                            chart2D.ChartAreas[0].AxisX.Maximum = 24;
                            chart2D.ChartAreas[0].AxisX.Interval = 2;
                            chart2D.ChartAreas[0].AxisY.Minimum = 0;
                            chart2D.ChartAreas[0].AxisY.Maximum = 20;
                            chart2D.ChartAreas[0].AxisY.Interval = double.NaN;
                            chart2D.ChartAreas[0].AxisX.Title = "Time";
                            chart2D.ChartAreas[0].AxisY.Title = "XE";
                            chart2D.Series["meal"].ChartType = SeriesChartType.Point;
                            for (int i = 0; i < f.Count; i++)
                            {
                                if (f[i].XE != 0)
                                    chart2D.Series["meal"].Points.AddXY(f[i].Time, f[i].XE);
                            }
                            IEnumerable<double> a = (IEnumerable<double>)chart2D.Series[0].Points.SelectMany(x => x.YValues.Select(y => x.XValue)).ToArray<double>();
                            IEnumerable<double> b = (IEnumerable<double>)chart2D.Series[0].Points.SelectMany(x => x.YValues).ToArray<double>();
                            double correl = Correlation.Pearson(a, b);
                            chart2D.Series[0].Name = "Correlation " + Math.Round(correl, 2).ToString();
                        }
                        break;
                    case "My glucose(Time)":
                        {
                            List<Fact> f = new List<Fact>();
                            f = facts;
                            chart2D.Series.Clear();
                            chart2D.Series.Add("Before meal");
                            chart2D.ChartAreas[0].AxisX.Minimum = 0;
                            chart2D.ChartAreas[0].AxisX.Maximum = 24;
                            chart2D.ChartAreas[0].AxisX.Interval = 2;
                            chart2D.ChartAreas[0].AxisY.Minimum = 0;
                            chart2D.ChartAreas[0].AxisY.Maximum = 20;
                            chart2D.ChartAreas[0].AxisY.Interval = double.NaN;
                            chart2D.ChartAreas[0].AxisX.Title = "Time";
                            chart2D.ChartAreas[0].AxisY.Title = "Glucose";
                            chart2D.Series["Before meal"].ChartType = SeriesChartType.Point;
                            for (int i = 0; i < f.Count; i++)
                            {
                                if (f[i].Before != 0)
                                    chart2D.Series["Before meal"].Points.AddXY(f[i].Time, f[i].Before);
                            }
                            IEnumerable<double> a = (IEnumerable<double>)chart2D.Series[0].Points.SelectMany(x => x.YValues.Select(y => x.XValue)).ToArray<double>();
                            IEnumerable<double> b = (IEnumerable<double>)chart2D.Series[0].Points.SelectMany(x => x.YValues).ToArray<double>();
                            double correl = Correlation.Pearson(a, b);
                            chart2D.Series[0].Name = "Correlation " + Math.Round(correl, 2).ToString();
                        }
                        break;
                    case "My dose(XE)":
                        {
                            List<Fact> f = new List<Fact>();
                            f = facts;
                            chart2D.Series.Clear();
                            chart2D.Series.Add("For meal");
                            //chart2D.Legends[0].Name = "";
                            chart2D.ChartAreas[0].AxisX.Maximum = double.NaN;
                            chart2D.ChartAreas[0].AxisY.Maximum = double.NaN;
                            chart2D.ChartAreas[0].AxisX.Interval = double.NaN;
                            chart2D.ChartAreas[0].AxisY.Interval = double.NaN;
                            chart2D.ChartAreas[0].AxisX.Title = "XE";
                            chart2D.ChartAreas[0].AxisY.Title = "Dose";
                            chart2D.Series["For meal"].ChartType = SeriesChartType.Point;
                            for (int i = 0; i < f.Count; i++)
                            {
                                chart2D.Series["For meal"].Points.AddXY(f[i].XE, f[i].Dose);
                            }
                            IEnumerable<double> a = (IEnumerable<double>)chart2D.Series[0].Points.SelectMany(x => x.YValues.Select(y => x.XValue)).ToArray<double>();
                            IEnumerable<double> b = (IEnumerable<double>)chart2D.Series[0].Points.SelectMany(x => x.YValues).ToArray<double>();
                            double correl = Correlation.Pearson(a, b);
                            chart2D.Series[0].Name = "Correlation " + Math.Round(correl, 2).ToString();
                        }
                        break;
                    case "My dose(Time)":
                        {
                            List<Fact> f = new List<Fact>();
                            f = facts;
                            chart2D.Series.Clear();
                            chart2D.Series.Add("For meal");
                            chart2D.ChartAreas[0].AxisX.Maximum = double.NaN;
                            chart2D.ChartAreas[0].AxisY.Maximum = double.NaN;
                            chart2D.ChartAreas[0].AxisX.Interval = double.NaN;
                            chart2D.ChartAreas[0].AxisY.Interval = double.NaN;
                            chart2D.ChartAreas[0].AxisX.Minimum = 0;
                            chart2D.ChartAreas[0].AxisX.Maximum = 24;
                            chart2D.ChartAreas[0].AxisX.Interval = 2;
                            chart2D.ChartAreas[0].AxisX.Title = "Time";
                            chart2D.ChartAreas[0].AxisY.Title = "Dose";
                            chart2D.Series["For meal"].ChartType = SeriesChartType.Point;
                            for (int i = 0; i < f.Count; i++)
                            {
                                chart2D.Series["For meal"].Points.AddXY(f[i].Time, f[i].Dose);
                            }
                            IEnumerable<double> a = (IEnumerable<double>)chart2D.Series[0].Points.SelectMany(x => x.YValues.Select(y => x.XValue)).ToArray<double>();
                            IEnumerable<double> b = (IEnumerable<double>)chart2D.Series[0].Points.SelectMany(x => x.YValues).ToArray<double>();
                            double correl = Correlation.Pearson(a, b);
                            chart2D.Series[0].Name = "Correlation " + Math.Round(correl, 2).ToString();
                        }
                        break;
                    case "My dose(DifferenceInGlucose)":
                        {
                            List<Fact> f = new List<Fact>();
                            f = facts;
                            chart2D.Series.Clear();
                            chart2D.Series.Add("For meal");
                            chart2D.ChartAreas[0].AxisX.Maximum = double.NaN;
                            chart2D.ChartAreas[0].AxisY.Maximum = double.NaN;
                            chart2D.ChartAreas[0].AxisX.Interval = double.NaN;
                            chart2D.ChartAreas[0].AxisY.Interval = double.NaN;
                            chart2D.ChartAreas[0].AxisX.Minimum = double.NaN;
                            chart2D.ChartAreas[0].AxisY.Minimum = double.NaN;
                            chart2D.ChartAreas[0].AxisX.Title = "DifferenceInGlucose";
                            chart2D.ChartAreas[0].AxisY.Title = "Dose";
                            chart2D.Series["For meal"].ChartType = SeriesChartType.Point;
                            for (int i = 0; i < f.Count; i++)
                            {
                                //if ((f[i].DifferenceInGlucose.HasValue) && (f[i].DifferenceInGlucose <= 0))
                                // chart2D.Series["For meal"].Points.AddXY(Math.Abs(f[i].DifferenceInGlucose.Value), f[i].Dose);
                                //if ((f[i].DifferenceInGlucose.HasValue) && (f[i].DifferenceInGlucose >= 0))
                                if (f[i].DifferenceInGlucose.HasValue)
                                    chart2D.Series["For meal"].Points.AddXY(f[i].DifferenceInGlucose.Value, f[i].Dose);
                            }
                            IEnumerable<double> a = (IEnumerable<double>)chart2D.Series[0].Points.SelectMany(x => x.YValues.Select(y => x.XValue)).ToArray<double>();
                            IEnumerable<double> b = (IEnumerable<double>)chart2D.Series[0].Points.SelectMany(x => x.YValues).ToArray<double>();
                            double correl = Correlation.Pearson(a, b);
                            chart2D.Series[0].Name = "Correlation " + Math.Round(correl, 2).ToString();
                        }
                        break;
                    case "My dose(GlucoseBeforeMeal)":
                        {
                            List<Fact> f = new List<Fact>();
                            f = facts;
                            chart2D.Series.Clear();
                            chart2D.Series.Add("For meal");
                            chart2D.ChartAreas[0].AxisX.Maximum = double.NaN;
                            chart2D.ChartAreas[0].AxisY.Maximum = double.NaN;
                            chart2D.ChartAreas[0].AxisX.Interval = double.NaN;
                            chart2D.ChartAreas[0].AxisY.Interval = double.NaN;
                            chart2D.ChartAreas[0].AxisX.Title = "GlucoseBeforeMeal";
                            chart2D.ChartAreas[0].AxisY.Title = "Dose";
                            chart2D.Series["For meal"].ChartType = SeriesChartType.Point;
                            for (int i = 0; i < f.Count; i++)
                            {
                                if ((f[i].DifferenceInGlucose.HasValue) && (f[i].DifferenceInGlucose <= 0))
                                    chart2D.Series["For meal"].Points.AddXY(f[i].Before, f[i].Dose);
                            }
                            IEnumerable<double> a = (IEnumerable<double>)chart2D.Series[0].Points.SelectMany(x => x.YValues.Select(y => x.XValue)).ToArray<double>();
                            IEnumerable<double> b = (IEnumerable<double>)chart2D.Series[0].Points.SelectMany(x => x.YValues).ToArray<double>();
                            double correl = Correlation.Pearson(a, b);
                            chart2D.Series[0].Name = "Correlation " + Math.Round(correl, 2).ToString();
                        }
                        break;

                    default:
                        break;
                }
                UpdateDataGridView();
            }
            catch (Exception)
            {
                MessageBox.Show("Error. " + comboBoxXE.SelectedItem.ToString() + ". Few data");
            }
        }

        private void comboBoxGlucose_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart3D.Series.Clear();
            chart3D.ChartAreas[0].AxisX.Minimum = double.NaN;
            chart3D.ChartAreas[0].AxisX.Maximum = double.NaN;
            chart3D.ChartAreas[0].AxisX.Maximum = double.NaN;
            chart3D.ChartAreas[0].AxisY.Minimum = double.NaN;
            chart3D.ChartAreas[0].AxisY.Maximum = double.NaN;
            chart3D.ChartAreas[0].AxisY.Maximum = double.NaN;
            chart3D.ChartAreas[0].AxisX.IsReversed = false;
            chart3D.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;

            List<Fact> facts = lstFacts.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) <= trackBarGlucose.Value && x.XE <= trackBarXE.Value).ToList();
            try
            {
                switch (comboBoxGlucose.SelectedItem.ToString())
                {
                    case "Dose for 1 XE(Time, XE)":
                        {
                            chart3D.Legends[0].Title = "XE";
                            List<Fact> goodFacts = facts.Where(x => Math.Abs(x.After - x.Before) < (double)numericUpDownCoefForGoodFacts.Value).ToList();
                            List<Fact> badFacts = facts.Where(x => Math.Abs(x.After - x.Before) >= 1).ToList();

                            foreach (var item in goodFacts)
                                if (item.XE != 0)
                                    item.InsXE = Math.Round(item.Dose / item.XE, 4);

                            goodFacts = goodFacts.Where(x => x.InsXE > 0).ToList();

                            double[] xdataXE = goodFacts.Select(x => x.Time).ToArray();
                            double[] ydataXE = goodFacts.Select(x => x.InsXE).ToArray();
                            double[] zdataXE = goodFacts.Select(x => x.XE).ToArray();

                            Func<double, double> lineXE = Fit.LineFunc(xdataXE, ydataXE);
                            double goodnessLineHE = GoodnessOfFit.RSquared(xdataXE.Select(x => lineXE(x)), ydataXE);

                            Func<double, double> polynomOrder2XE = Fit.PolynomialFunc(xdataXE, ydataXE, 2); // polynomial of order 2
                            double goodnessPolynomOrder2HE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder2XE(x)), ydataXE);

                            Func<double, double> polynomOrder3XE = Fit.PolynomialFunc(xdataXE, ydataXE, 3); // polynomial of order 3
                            double goodnessPolynomOrder3HE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder3XE(x)), ydataXE);

                            //Func<double, double> polynomOrder4XE = Fit.PolynomialFunc(xdataXE, ydataXE, 4); // polynomial of order 4
                            //double goodnessPolynomOrder6HE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder4XE(x)), ydataXE);

                            double omega = Math.PI / 12;
                            Func<double, double> sinusoidalCombinationXE = Fit.LinearCombinationFunc(xdataXE, ydataXE,
                            x => 1.0,
                            x => Math.Sin(omega * x),
                            x => Math.Cos(omega * x));
                            double goodnessSinusoidalCombination = GoodnessOfFit.RSquared(xdataXE.Select(x => sinusoidalCombinationXE(x)), ydataXE);

                            Func<double, double> harmonicCombinationXE = Fit.LinearCombinationFunc(xdataXE, ydataXE,
                            x => 1,
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Sin(omega * x)),
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Cos(omega * x)));
                            double goodnessHarmonicCombination = GoodnessOfFit.RSquared(xdataXE.Select(x => harmonicCombinationXE(x)), ydataXE);

                            double exampleLXE = lineXE(13.0);
                            double exampleP2XE = polynomOrder2XE(13.0);
                            double exampleP3XE = polynomOrder3XE(13.0);
                            double exampleLCXE = sinusoidalCombinationXE(13.0);
                            double exampleNCXE = harmonicCombinationXE(13.0);
                            chart3D.Series.Clear();
                            chart3D.ChartAreas[0].AxisX.IsReversed = false;
                            chart3D.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                            chart3D.ChartAreas[0].AxisX.Minimum = 0;
                            chart3D.ChartAreas[0].AxisX.Maximum = 24;
                            chart3D.ChartAreas[0].AxisX.Interval = 2;
                            chart3D.ChartAreas[0].AxisX.Title = "Time";
                            chart3D.ChartAreas[0].AxisY.Title = "Insulin dose for 1 XE";
                            //chart3D.ChartAreas[0].AxisY.Minimum = 0;
                            //chart3D.ChartAreas[0].AxisY.Maximum = (ydataXE.Max() > 10) ? 10 : Math.Ceiling(ydataXE.Max());
                            //chart3D.ChartAreas[0].AxisY.Interval = 0.5;
                            /*
                            chartXE.Series.Add("line");
                            chartXE.Series.Add("polynom");
                            chartXE.Series.Add("sinusoidal");
                            chartXE.Series.Add("harmonic");*/
                            /*chartXE.Series["line"].ChartType = SeriesChartType.Line;
                            chartXE.Series["polynom"].ChartType = SeriesChartType.Spline;
                            chartXE.Series["sinusoidal"].ChartType = SeriesChartType.Spline;
                            chartXE.Series["harmonic"].ChartType = SeriesChartType.Spline;
                            chartXE.Series["data"].MarkerSize = 7;
                            chartXE.Series["line"].BorderWidth = 4;
                            chartXE.Series["polynom"].BorderWidth = 4;
                            chartXE.Series["sinusoidal"].BorderWidth = 4;
                            chartXE.Series["harmonic"].BorderWidth = 4;
                            for (int i = 0; i < 25; i++)
                            {
                                if (Math.Abs(lineXE(i)) < 10000)
                                    chartXE.Series["line"].Points.AddXY(i, Math.Round(lineXE(i), 2));
                                if (Math.Abs(polynomOrder3XE(i)) < 10000)
                                    chartXE.Series["polynom"].Points.AddXY(i, Math.Round(polynomOrder3XE(i), 2));
                                if (Math.Abs(sinusoidalCombinationXE(i)) < 10000)
                                    chartXE.Series["sinusoidal"].Points.AddXY(i, Math.Round(sinusoidalCombinationXE(i), 2));
                                if (Math.Abs(harmonicCombinationXE(i)) < 10000)
                                    chartXE.Series["harmonic"].Points.AddXY(i, Math.Round(harmonicCombinationXE(i), 2));

                            }*/
                            int max = (int)Math.Ceiling(zdataXE.Max());
                            for (int j = max; j >= 0; j -= 2)
                            {
                                chart3D.Series.Add(j.ToString());
                                chart3D.Series[j.ToString()].ChartType = SeriesChartType.Column;
                                //chart3D.Series[j.ToString()].MarkerSize = 15;
                                for (int i = 0; i < xdataXE.Length; i++)
                                {
                                    if ((zdataXE[i] >= j) && (zdataXE[i] < j + 2))
                                        chart3D.Series[j.ToString()].Points.AddXY(xdataXE[i], ydataXE[i]);
                                }
                                foreach (var item in chart3D.Series[j.ToString()].Points)
                                {
                                    double valueY = item.YValues.Average();
                                    double valueX = item.XValue;
                                    item.SetDefault(true);
                                    item.XValue = valueX;
                                    item.YValues.SetValue(valueY, 0);

                                }
                            }
                        }
                        break;

                    case "Dose to lower glucose by 1 mmol/L(Time, DifferenceInGlucose)":
                        {
                            chart3D.Legends[0].Title = "DifferenceInGlucose";
                            List<Fact> goodFacts = facts.Where(x => Math.Abs(x.After - x.Before) < (double)numericUpDownCoefForGoodFacts.Value).ToList();
                            List<Fact> badFacts = facts.Where(x => Math.Abs(x.After - x.Before) >= 1).ToList();

                            foreach (var item in goodFacts)
                                if (item.XE != 0)
                                    item.InsXE = Math.Round(item.Dose / item.XE, 4);

                            goodFacts = goodFacts.Where(x => x.InsXE > 0).ToList();

                            double[] xdataXE = goodFacts.Select(x => x.Time).ToArray();
                            double[] ydataXE = goodFacts.Select(x => x.InsXE).ToArray();
                            double[] zdataXE = goodFacts.Select(x => x.XE).ToArray();

                            Func<double, double> lineXE = Fit.LineFunc(xdataXE, ydataXE);
                            double goodnessLineXE = GoodnessOfFit.RSquared(xdataXE.Select(x => lineXE(x)), ydataXE);

                            Func<double, double> polynomOrder2XE = Fit.PolynomialFunc(xdataXE, ydataXE, 2); // polynomial of order 2
                            double goodnessPolynomOrder2XE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder2XE(x)), ydataXE);

                            Func<double, double> polynomOrder3XE = Fit.PolynomialFunc(xdataXE, ydataXE, 3); // polynomial of order 3
                            double goodnessPolynomOrder3XE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder3XE(x)), ydataXE);

                            //Func<double, double> polynomOrder4XE = Fit.PolynomialFunc(xdataXE, ydataXE, 4); // polynomial of order 4
                            //double goodnessPolynomOrder6XE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder4XE(x)), ydataXE);

                            double omega = Math.PI / 12;
                            Func<double, double> sinusoidalCombinationXE = Fit.LinearCombinationFunc(xdataXE, ydataXE,
                            x => 1.0,
                            x => Math.Sin(omega * x),
                            x => Math.Cos(omega * x));
                            double goodnessSinusoidalCombinationXE = GoodnessOfFit.RSquared(xdataXE.Select(x => sinusoidalCombinationXE(x)), ydataXE);

                            Func<double, double> harmonicCombinationXE = Fit.LinearCombinationFunc(xdataXE, ydataXE,
                            x => 1,
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Sin(omega * x)),
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Cos(omega * x)));
                            double goodnessHarmonicCombinationXE = GoodnessOfFit.RSquared(xdataXE.Select(x => harmonicCombinationXE(x)), ydataXE);

                            double exampleLXE = lineXE(13.0);
                            double exampleP2XE = polynomOrder2XE(13.0);
                            double exampleP3XE = polynomOrder3XE(13.0);
                            double exampleLCXE = sinusoidalCombinationXE(13.0);
                            double exampleNCXE = harmonicCombinationXE(13.0);

                            foreach (var item in badFacts)
                            {
                                //if (item.Before > item.After)
                                if (item.Before - item.After != 0)
                                {
                                    item.InsGlu = Math.Round((item.Dose - (harmonicCombinationXE(item.Time) * item.XE)) / (item.DifferenceInGlucose.Value), 4);
                                }
                            }
                            badFacts = badFacts.Where(x => x.DifferenceInGlucose.HasValue && x.InsGlu > 0).ToList();

                            double[] xdataGlucose = badFacts.Select(x => x.Time).ToArray();
                            double[] ydataGlucose = badFacts.Select(x => x.InsGlu).ToArray();
                            double[] zdataGlucose = badFacts.Select(x => Math.Abs(x.DifferenceInGlucose.Value)).ToArray();
                            //double[] zdataGlucose = badFacts.Select(x => x.Before - x.After).ToArray();

                            Func<double, double> lineGlucose = Fit.LineFunc(xdataGlucose, ydataGlucose);
                            double goodnessLineGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => lineGlucose(x)), ydataGlucose);

                            Func<double, double> polynomOrder2Glucose = Fit.PolynomialFunc(xdataGlucose, ydataGlucose, 2); // polynomial of order 2
                            double goodnessPolynomOrder2Glucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => polynomOrder2Glucose(x)), ydataGlucose);

                            Func<double, double> polynomOrder3Glucose = Fit.PolynomialFunc(xdataGlucose, ydataGlucose, 3); // polynomial of order 3
                            double goodnessPolynomOrder3Glucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => polynomOrder3Glucose(x)), ydataGlucose);

                            //Func<double, double> polynomOrder4Glucose = Fit.PolynomialFunc(xdataGlucose, ydataGlucose, 4); // polynomial of order 3
                            //double goodnessPolynomOrder6Glucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => polynomOrder4Glucose(x)), ydataGlucose);

                            Func<double, double> sinusoidalCombinationGlucose = Fit.LinearCombinationFunc(
                            xdataGlucose,
                            ydataGlucose,
                            x => 1.0,
                            x => Math.Sin(omega * x),
                            x => Math.Cos(omega * x));
                            double goodnesssinusoidalCombinationGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => sinusoidalCombinationGlucose(x)), ydataGlucose);

                            Func<double, double> harmonicCombinationGlucose = Fit.LinearCombinationFunc(
                            xdataGlucose,
                            ydataGlucose,
                            x => 1,
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Sin(omega * x)),
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Cos(omega * x)));
                            double goodnessHarmonicCombinationGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => harmonicCombinationGlucose(x)), ydataGlucose);

                            //пример
                            double exampleLGlucose = lineGlucose(13.0);
                            double exampleP2Glucose = polynomOrder2Glucose(13.0);
                            double exampleP3Glucose = polynomOrder3Glucose(13.0);
                            double exampleLCGlucose = sinusoidalCombinationGlucose(13.0);
                            double exampleHCGlucose = harmonicCombinationGlucose(13.0);

                            double nowGlu = 9.9;
                            double nowXE = 4;
                            double nowTime = 13;
                            //double nowInsulin = ?;

                            double exampleLFull = lineXE(nowTime) * nowXE + (nowGlu - 6) * lineGlucose(nowTime);
                            double exampleP2Full = polynomOrder2XE(nowTime) * nowXE + (nowGlu - 6) * polynomOrder2Glucose(nowTime);
                            double exampleP3Full = polynomOrder3XE(nowTime) * nowXE + (nowGlu - 6) * polynomOrder3Glucose(nowTime);
                            double exampleLCFull = sinusoidalCombinationXE(nowTime) * nowXE + (nowGlu - 6) * sinusoidalCombinationGlucose(nowTime);
                            double exampleHCFull = harmonicCombinationXE(nowTime) * nowXE + (nowGlu - 6) * harmonicCombinationGlucose(nowTime);

                            chart3D.Series.Clear();
                            chart3D.ChartAreas[0].AxisX.IsReversed = false;
                            chart3D.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                            chart3D.ChartAreas[0].AxisX.Minimum = 0;
                            chart3D.ChartAreas[0].AxisX.Maximum = 24;
                            chart3D.ChartAreas[0].AxisX.Interval = 2;
                            chart3D.ChartAreas[0].AxisX.Title = "Time";
                            chart3D.ChartAreas[0].AxisY.Title = "Insulin dose to lower glucose by 1 mmol/L";
                            //chart3D.ChartAreas[0].AxisY.Minimum = 0;
                            //chart3D.ChartAreas[0].AxisY.Maximum = (ydataGlucose.Max() > 10) ? 10 : Math.Ceiling(ydataGlucose.Max());
                            //chart3D.ChartAreas[0].AxisY.Interval = 0.5;
                            /*
                            chartXE.Series.Add("line");
                            chartXE.Series.Add("polynom");
                            chartXE.Series.Add("sinusoidal");
                            chartXE.Series.Add("harmonic");
                            chartXE.Series.Add("data");
                            chartXE.ChartAreas[0].AxisX.Minimum = 0;
                            chartXE.ChartAreas[0].AxisX.Maximum = 24;
                            chartXE.ChartAreas[0].AxisX.Interval = 2;
                            chartXE.ChartAreas[0].AxisY.Minimum = 0;
                            chartXE.ChartAreas[0].AxisY.Maximum = (ydataGlucose.Max() > 10) ? 10 : Math.Ceiling(ydataGlucose.Max());
                            chartXE.ChartAreas[0].AxisY.Interval = 0.5;
                            chartXE.Series["data"].ChartType = SeriesChartType.Point;
                            chartXE.Series["data"].Color = Color.Black;
                            chartXE.Series["line"].ChartType = SeriesChartType.Line;
                            chartXE.Series["polynom"].ChartType = SeriesChartType.Spline;
                            chartXE.Series["sinusoidal"].ChartType = SeriesChartType.Spline;
                            chartXE.Series["harmonic"].ChartType = SeriesChartType.Spline;
                            chartXE.Series["data"].MarkerSize = 7;
                            chartXE.Series["line"].BorderWidth = 4;
                            chartXE.Series["polynom"].BorderWidth = 4;
                            chartXE.Series["sinusoidal"].BorderWidth = 4;
                            chartXE.Series["harmonic"].BorderWidth = 4;
                            for (int i = 0; i < 25; i++)
                            {
                                if (Math.Abs(lineGlucose(i)) < 10000)
                                    chartXE.Series["line"].Points.AddXY(i, Math.Round(lineGlucose(i), 2));
                                if (Math.Abs(polynomOrder3Glucose(i)) < 10000)
                                    chartXE.Series["polynom"].Points.AddXY(i, Math.Round(polynomOrder3Glucose(i), 2));
                                if (Math.Abs(sinusoidalCombinationGlucose(i)) < 10000)
                                    chartXE.Series["sinusoidal"].Points.AddXY(i, Math.Round(sinusoidalCombinationGlucose(i), 2));
                                if (Math.Abs(harmonicCombinationGlucose(i)) < 10000)
                                    chartXE.Series["harmonic"].Points.AddXY(i, Math.Round(harmonicCombinationGlucose(i), 2));

                            }
                            for (int i = 0; i < xdataGlucose.Length; i++)
                            {
                                chartXE.Series["data"].Points.AddXY(xdataGlucose[i], ydataGlucose[i]);
                            }*/
                            int max = (int)Math.Ceiling(zdataGlucose.Max());
                            for (int j = max; j >= 0; j -= 2)
                            {
                                chart3D.Series.Add(j.ToString());
                                chart3D.Series[j.ToString()].ChartType = SeriesChartType.Column;
                                //chart3D.Series[j.ToString()].MarkerSize = 15;
                                for (int i = 0; i < xdataGlucose.Length; i++)
                                {
                                    if ((zdataGlucose[i] >= j) && (zdataGlucose[i] < j + 2))
                                    {
                                        if (chart3D.Series[j.ToString()].Points.Where(x => x.XValue == xdataGlucose[i] && x.YValues.First() == ydataGlucose[i]).Count() == 0)
                                            chart3D.Series[j.ToString()].Points.AddXY(xdataGlucose[i], ydataGlucose[i]);
                                    }
                                }
                                foreach (var item in chart3D.Series[j.ToString()].Points)
                                {
                                    double valueY = item.YValues.Average();
                                    double valueX = item.XValue;
                                    item.SetDefault(true);
                                    item.XValue = valueX;
                                    item.YValues.SetValue(valueY, 0);

                                }

                            }
                        }
                        break;
                    case "Dose for 1 XE(Time, BeforeMeal)":
                        {
                            chart3D.Legends[0].Title = "BeforeMeal";
                            List<Fact> goodFacts = facts.Where(x => Math.Abs(x.DifferenceInGlucose.Value) < (double)numericUpDownCoefForGoodFacts.Value).ToList();
                            List<Fact> badFacts = facts.Where(x => Math.Abs(x.DifferenceInGlucose.Value) >= 1).ToList();

                            foreach (var item in goodFacts)
                                if (item.XE != 0)
                                    item.InsXE = Math.Round(item.Dose / item.XE, 4);

                            goodFacts = goodFacts.Where(x => x.InsXE > 0).ToList();

                            double[] xdataXE = goodFacts.Select(x => x.Time).ToArray();
                            double[] ydataXE = goodFacts.Select(x => x.InsXE).ToArray();
                            double[] zdataXE = goodFacts.Select(x => x.Before).ToArray();

                            Func<double, double> lineXE = Fit.LineFunc(xdataXE, ydataXE);
                            double goodnessLineHE = GoodnessOfFit.RSquared(xdataXE.Select(x => lineXE(x)), ydataXE);

                            Func<double, double> polynomOrder2XE = Fit.PolynomialFunc(xdataXE, ydataXE, 2); // polynomial of order 2
                            double goodnessPolynomOrder2HE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder2XE(x)), ydataXE);

                            Func<double, double> polynomOrder3XE = Fit.PolynomialFunc(xdataXE, ydataXE, 3); // polynomial of order 3
                            double goodnessPolynomOrder3HE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder3XE(x)), ydataXE);

                            //Func<double, double> polynomOrder4XE = Fit.PolynomialFunc(xdataXE, ydataXE, 4); // polynomial of order 4
                            //double goodnessPolynomOrder6HE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder4XE(x)), ydataXE);

                            double omega = Math.PI / 12;
                            Func<double, double> sinusoidalCombinationXE = Fit.LinearCombinationFunc(xdataXE, ydataXE,
                            x => 1.0,
                            x => Math.Sin(omega * x),
                            x => Math.Cos(omega * x));
                            double goodnessSinusoidalCombination = GoodnessOfFit.RSquared(xdataXE.Select(x => sinusoidalCombinationXE(x)), ydataXE);

                            Func<double, double> harmonicCombinationXE = Fit.LinearCombinationFunc(xdataXE, ydataXE,
                            x => 1,
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Sin(omega * x)),
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Cos(omega * x)));
                            double goodnessHarmonicCombination = GoodnessOfFit.RSquared(xdataXE.Select(x => harmonicCombinationXE(x)), ydataXE);

                            double exampleLXE = lineXE(13.0);
                            double exampleP2XE = polynomOrder2XE(13.0);
                            double exampleP3XE = polynomOrder3XE(13.0);
                            double exampleLCXE = sinusoidalCombinationXE(13.0);
                            double exampleNCXE = harmonicCombinationXE(13.0);
                            chart3D.Series.Clear();
                            chart3D.ChartAreas[0].AxisX.IsReversed = false;
                            chart3D.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                            chart3D.ChartAreas[0].AxisX.Minimum = 0;
                            chart3D.ChartAreas[0].AxisX.Maximum = 24;
                            chart3D.ChartAreas[0].AxisX.Interval = 2;
                            chart3D.ChartAreas[0].AxisX.Title = "Time";
                            chart3D.ChartAreas[0].AxisY.Title = "Insulin dose for 1 XE";
                            //chart3D.ChartAreas[0].AxisY.Minimum = 0;
                            //chart3D.ChartAreas[0].AxisY.Maximum = (ydataXE.Max() > 10) ? 10 : Math.Ceiling(ydataXE.Max());
                            //chart3D.ChartAreas[0].AxisY.Interval = 0.5;
                            /*
                            chartXE.Series.Add("line");
                            chartXE.Series.Add("polynom");
                            chartXE.Series.Add("sinusoidal");
                            chartXE.Series.Add("harmonic");*/
                            /*chartXE.Series["line"].ChartType = SeriesChartType.Line;
                            chartXE.Series["polynom"].ChartType = SeriesChartType.Spline;
                            chartXE.Series["sinusoidal"].ChartType = SeriesChartType.Spline;
                            chartXE.Series["harmonic"].ChartType = SeriesChartType.Spline;
                            chartXE.Series["data"].MarkerSize = 7;
                            chartXE.Series["line"].BorderWidth = 4;
                            chartXE.Series["polynom"].BorderWidth = 4;
                            chartXE.Series["sinusoidal"].BorderWidth = 4;
                            chartXE.Series["harmonic"].BorderWidth = 4;
                            for (int i = 0; i < 25; i++)
                            {
                                if (Math.Abs(lineXE(i)) < 10000)
                                    chartXE.Series["line"].Points.AddXY(i, Math.Round(lineXE(i), 2));
                                if (Math.Abs(polynomOrder3XE(i)) < 10000)
                                    chartXE.Series["polynom"].Points.AddXY(i, Math.Round(polynomOrder3XE(i), 2));
                                if (Math.Abs(sinusoidalCombinationXE(i)) < 10000)
                                    chartXE.Series["sinusoidal"].Points.AddXY(i, Math.Round(sinusoidalCombinationXE(i), 2));
                                if (Math.Abs(harmonicCombinationXE(i)) < 10000)
                                    chartXE.Series["harmonic"].Points.AddXY(i, Math.Round(harmonicCombinationXE(i), 2));

                            }*/
                            int max = (int)Math.Ceiling(zdataXE.Max());
                            for (int j = max; j >= 0; j -= 2)
                            {
                                chart3D.Series.Add(j.ToString());
                                chart3D.Series[j.ToString()].ChartType = SeriesChartType.Column;
                                //chart3D.Series[j.ToString()].MarkerSize = 15;
                                for (int i = 1; i < xdataXE.Length; i++)
                                {
                                    if ((zdataXE[i] >= j) && (zdataXE[i] < j + 2))
                                        chart3D.Series[j.ToString()].Points.AddXY(xdataXE[i], ydataXE[i]);
                                }
                                foreach (var item in chart3D.Series[j.ToString()].Points)
                                {
                                    double valueY = item.YValues.Average();
                                    double valueX = item.XValue;
                                    item.SetDefault(true);
                                    item.XValue = valueX;
                                    item.YValues.SetValue(valueY, 0);

                                }
                            }
                        }
                        break;

                    case "Dose to lower glucose by 1 mmol/L(Time, BeforeMeal)":
                        {
                            chart3D.Legends[0].Title = "BeforeMeal";
                            List<Fact> goodFacts = facts.Where(x => Math.Abs(x.DifferenceInGlucose.Value) < (double)numericUpDownCoefForGoodFacts.Value).ToList();
                            List<Fact> badFacts = facts.Where(x => Math.Abs(x.DifferenceInGlucose.Value) >= 1).ToList();

                            foreach (var item in goodFacts)
                                if (item.XE != 0)
                                    item.InsXE = Math.Round(item.Dose / item.XE, 4);

                            goodFacts = goodFacts.Where(x => x.InsXE > 0).ToList();

                            double[] xdataXE = goodFacts.Select(x => x.Time).ToArray();
                            double[] ydataXE = goodFacts.Select(x => x.InsXE).ToArray();
                            double[] zdataXE = goodFacts.Select(x => x.Before).ToArray();

                            Func<double, double> lineXE = Fit.LineFunc(xdataXE, ydataXE);
                            double goodnessLineXE = GoodnessOfFit.RSquared(xdataXE.Select(x => lineXE(x)), ydataXE);

                            Func<double, double> polynomOrder2XE = Fit.PolynomialFunc(xdataXE, ydataXE, 2); // polynomial of order 2
                            double goodnessPolynomOrder2XE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder2XE(x)), ydataXE);

                            Func<double, double> polynomOrder3XE = Fit.PolynomialFunc(xdataXE, ydataXE, 3); // polynomial of order 3
                            double goodnessPolynomOrder3XE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder3XE(x)), ydataXE);

                            //Func<double, double> polynomOrder4XE = Fit.PolynomialFunc(xdataXE, ydataXE, 4); // polynomial of order 4
                            //double goodnessPolynomOrder6XE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder4XE(x)), ydataXE);

                            double omega = Math.PI / 12;
                            Func<double, double> sinusoidalCombinationXE = Fit.LinearCombinationFunc(xdataXE, ydataXE,
                            x => 1.0,
                            x => Math.Sin(omega * x),
                            x => Math.Cos(omega * x));
                            double goodnessSinusoidalCombinationXE = GoodnessOfFit.RSquared(xdataXE.Select(x => sinusoidalCombinationXE(x)), ydataXE);

                            Func<double, double> harmonicCombinationXE = Fit.LinearCombinationFunc(xdataXE, ydataXE,
                            x => 1,
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Sin(omega * x)),
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Cos(omega * x)));
                            double goodnessHarmonicCombinationXE = GoodnessOfFit.RSquared(xdataXE.Select(x => harmonicCombinationXE(x)), ydataXE);

                            double exampleLXE = lineXE(13.0);
                            double exampleP2XE = polynomOrder2XE(13.0);
                            double exampleP3XE = polynomOrder3XE(13.0);
                            double exampleLCXE = sinusoidalCombinationXE(13.0);
                            double exampleNCXE = harmonicCombinationXE(13.0);

                            foreach (var item in badFacts)
                            {
                                //if (item.Before > item.After)
                                if (item.Before - item.After != 0)
                                {
                                    item.InsGlu = Math.Round((item.Dose - (harmonicCombinationXE(item.Time) * item.XE)) / (item.Before - item.After), 4);
                                }
                            }
                            badFacts = badFacts.Where(x => x.DifferenceInGlucose.HasValue && x.InsGlu > 0).ToList();

                            double[] xdataGlucose = badFacts.Select(x => x.Time).ToArray();
                            double[] ydataGlucose = badFacts.Select(x => x.InsGlu).ToArray();
                            double[] zdataGlucose = badFacts.Select(x => x.Before).ToArray();
                            //double[] zdataGlucose = badFacts.Select(x => Math.Abs(x.DifferenceInGlucose.Value)).ToArray();

                            Func<double, double> lineGlucose = Fit.LineFunc(xdataGlucose, ydataGlucose);
                            double goodnessLineGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => lineGlucose(x)), ydataGlucose);

                            Func<double, double> polynomOrder2Glucose = Fit.PolynomialFunc(xdataGlucose, ydataGlucose, 2); // polynomial of order 2
                            double goodnessPolynomOrder2Glucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => polynomOrder2Glucose(x)), ydataGlucose);

                            Func<double, double> polynomOrder3Glucose = Fit.PolynomialFunc(xdataGlucose, ydataGlucose, 3); // polynomial of order 3
                            double goodnessPolynomOrder3Glucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => polynomOrder3Glucose(x)), ydataGlucose);

                            //Func<double, double> polynomOrder4Glucose = Fit.PolynomialFunc(xdataGlucose, ydataGlucose, 4); // polynomial of order 3
                            //double goodnessPolynomOrder6Glucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => polynomOrder4Glucose(x)), ydataGlucose);

                            Func<double, double> sinusoidalCombinationGlucose = Fit.LinearCombinationFunc(
                            xdataGlucose,
                            ydataGlucose,
                            x => 1.0,
                            x => Math.Sin(omega * x),
                            x => Math.Cos(omega * x));
                            double goodnesssinusoidalCombinationGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => sinusoidalCombinationGlucose(x)), ydataGlucose);

                            Func<double, double> harmonicCombinationGlucose = Fit.LinearCombinationFunc(
                            xdataGlucose,
                            ydataGlucose,
                            x => 1,
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Sin(omega * x)),
                            x => SpecialFunctions.GeneralHarmonic(100, Math.Cos(omega * x)));
                            double goodnessHarmonicCombinationGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => harmonicCombinationGlucose(x)), ydataGlucose);

                            //пример
                            double exampleLGlucose = lineGlucose(13.0);
                            double exampleP2Glucose = polynomOrder2Glucose(13.0);
                            double exampleP3Glucose = polynomOrder3Glucose(13.0);
                            double exampleLCGlucose = sinusoidalCombinationGlucose(13.0);
                            double exampleHCGlucose = harmonicCombinationGlucose(13.0);

                            double nowGlu = 9.9;
                            double nowXE = 4;
                            double nowTime = 13;
                            //double nowInsulin = ?;

                            double exampleLFull = lineXE(nowTime) * nowXE + (nowGlu - 6) * lineGlucose(nowTime);
                            double exampleP2Full = polynomOrder2XE(nowTime) * nowXE + (nowGlu - 6) * polynomOrder2Glucose(nowTime);
                            double exampleP3Full = polynomOrder3XE(nowTime) * nowXE + (nowGlu - 6) * polynomOrder3Glucose(nowTime);
                            double exampleLCFull = sinusoidalCombinationXE(nowTime) * nowXE + (nowGlu - 6) * sinusoidalCombinationGlucose(nowTime);
                            double exampleHCFull = harmonicCombinationXE(nowTime) * nowXE + (nowGlu - 6) * harmonicCombinationGlucose(nowTime);

                            chart3D.Series.Clear();
                            chart3D.ChartAreas[0].AxisX.IsReversed = false;
                            chart3D.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                            chart3D.ChartAreas[0].AxisX.Minimum = 0;
                            chart3D.ChartAreas[0].AxisX.Maximum = 24;
                            chart3D.ChartAreas[0].AxisX.Interval = 2;
                            chart3D.ChartAreas[0].AxisX.Title = "Time";
                            chart3D.ChartAreas[0].AxisY.Title = "Insulin dose to lower glucose by 1 mmol/L";
                            //chart3D.ChartAreas[0].AxisY.Minimum = 0;
                            //chart3D.ChartAreas[0].AxisY.Maximum = (ydataGlucose.Max() > 10) ? 10 : Math.Ceiling(ydataGlucose.Max());
                            //chart3D.ChartAreas[0].AxisY.Interval = 0.5;
                            /*
                            chartXE.Series.Add("line");
                            chartXE.Series.Add("polynom");
                            chartXE.Series.Add("sinusoidal");
                            chartXE.Series.Add("harmonic");
                            chartXE.Series.Add("data");
                            chartXE.ChartAreas[0].AxisX.Minimum = 0;
                            chartXE.ChartAreas[0].AxisX.Maximum = 24;
                            chartXE.ChartAreas[0].AxisX.Interval = 2;
                            chartXE.ChartAreas[0].AxisY.Minimum = 0;
                            chartXE.ChartAreas[0].AxisY.Maximum = (ydataGlucose.Max() > 10) ? 10 : Math.Ceiling(ydataGlucose.Max());
                            chartXE.ChartAreas[0].AxisY.Interval = 0.5;
                            chartXE.Series["data"].ChartType = SeriesChartType.Point;
                            chartXE.Series["data"].Color = Color.Black;
                            chartXE.Series["line"].ChartType = SeriesChartType.Line;
                            chartXE.Series["polynom"].ChartType = SeriesChartType.Spline;
                            chartXE.Series["sinusoidal"].ChartType = SeriesChartType.Spline;
                            chartXE.Series["harmonic"].ChartType = SeriesChartType.Spline;
                            chartXE.Series["data"].MarkerSize = 7;
                            chartXE.Series["line"].BorderWidth = 4;
                            chartXE.Series["polynom"].BorderWidth = 4;
                            chartXE.Series["sinusoidal"].BorderWidth = 4;
                            chartXE.Series["harmonic"].BorderWidth = 4;
                            for (int i = 0; i < 25; i++)
                            {
                                if (Math.Abs(lineGlucose(i)) < 10000)
                                    chartXE.Series["line"].Points.AddXY(i, Math.Round(lineGlucose(i), 2));
                                if (Math.Abs(polynomOrder3Glucose(i)) < 10000)
                                    chartXE.Series["polynom"].Points.AddXY(i, Math.Round(polynomOrder3Glucose(i), 2));
                                if (Math.Abs(sinusoidalCombinationGlucose(i)) < 10000)
                                    chartXE.Series["sinusoidal"].Points.AddXY(i, Math.Round(sinusoidalCombinationGlucose(i), 2));
                                if (Math.Abs(harmonicCombinationGlucose(i)) < 10000)
                                    chartXE.Series["harmonic"].Points.AddXY(i, Math.Round(harmonicCombinationGlucose(i), 2));

                            }
                            for (int i = 0; i < xdataGlucose.Length; i++)
                            {
                                chartXE.Series["data"].Points.AddXY(xdataGlucose[i], ydataGlucose[i]);
                            }*/
                            int max = (int)Math.Ceiling(zdataGlucose.Max());
                            for (int j = max; j >= 0; j -= 2)
                            {
                                chart3D.Series.Add(j.ToString());
                                chart3D.Series[j.ToString()].ChartType = SeriesChartType.Column;
                                //chart3D.Series[j.ToString()].MarkerSize = 15;
                                for (int i = 0; i < xdataGlucose.Length; i++)
                                {
                                    if ((zdataGlucose[i] >= j) && (zdataGlucose[i] < j + 2))
                                    {
                                        if (chart3D.Series[j.ToString()].Points.Where(x => x.XValue == xdataGlucose[i] && x.YValues.First() == ydataGlucose[i]).Count() == 0)
                                            chart3D.Series[j.ToString()].Points.AddXY(xdataGlucose[i], ydataGlucose[i]);
                                    }
                                }
                                foreach (var item in chart3D.Series[j.ToString()].Points)
                                {
                                    double valueY = item.YValues.Average();
                                    double valueX = item.XValue;
                                    item.SetDefault(true);
                                    item.XValue = valueX;
                                    item.YValues.SetValue(valueY, 0);

                                }

                            }
                        }
                        break;
                    case "My dose(DifferenceInGlucose, XE)":
                        {
                            chart3D.Legends[0].Title = "XE";
                            List<Fact> f = new List<Fact>();
                            f = facts;
                            chart3D.Series.Clear();
                            //chart3D.ChartAreas[0].AxisX.IsReversed = true;
                            //chart3D.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                            chart3D.ChartAreas[0].AxisY.Minimum = 0;
                            chart3D.ChartAreas[0].AxisY.Maximum = 20;
                            chart3D.ChartAreas[0].AxisX.Minimum = -10;
                            chart3D.ChartAreas[0].AxisX.Maximum = 20;
                            chart3D.ChartAreas[0].AxisX.Maximum = f.Select(x => x.XE).Max();
                            chart3D.ChartAreas[0].AxisX.Title = "DifferenceInGlucose";
                            chart3D.ChartAreas[0].AxisY.Title = "Insulin dose";
                            int max = (int)Math.Ceiling(f.Select(x => x.XE).Max());
                            max = 14;
                            for (int j = max; j >= 0; j -= 1)
                            {
                                chart3D.Series.Add(j.ToString());
                                chart3D.Series[j.ToString()].ChartType = SeriesChartType.Column;
                                chart3D.Series[j.ToString()].MarkerSize = 15;
                                chart3D.Series[j.ToString()].Points.AddXY(0, 0);
                                for (int i = 0; i < f.Count; i++)
                                {
                                    if ((f[i].XE >= j) && (f[i].XE < j + 1))
                                    {
                                        chart3D.Series[j.ToString()].Points.AddXY(Math.Round(f[i].DifferenceInGlucose.Value,0), f[i].Dose);
                                    }
                                    foreach (var item in chart3D.Series[j.ToString()].Points)
                                    {
                                        double valueY = item.YValues.Average();
                                        double valueX = item.XValue;
                                        item.SetDefault(true);
                                        item.XValue = valueX;
                                        item.YValues.SetValue(valueY, 0);

                                    }
                                }
                            }
                        }
                        break;
                    case "Dose(DifferenceInGlucose, XE)":
                        {
                            chart3D.ChartAreas[0].AxisY.Minimum = 0;
                            chart3D.ChartAreas[0].AxisY.Maximum = 20;
                            chart3D.ChartAreas[0].AxisX.Minimum = -10;
                            chart3D.ChartAreas[0].AxisX.Maximum = 14;
                            double[][] xyData = facts.Select(x => new[] { x.DifferenceInGlucose.Value, x.XE }).ToArray();
                            double[] zData = facts.Select(x => x.Dose).ToArray();
                            Func<Double[], double> linearMultiDimFunc = Fit.LinearMultiDimFunc(xyData, zData,
                            d => 1.0,
                            d => d[0],
                            d => d[1],
                            d => d[0] * d[0],
                            d => d[1] * d[1],
                            d => d[0] * d[1],
                            d => d[0] * d[0] * d[0],
                            d => d[1] * d[1] * d[1],
                            d => d[0] * d[0] * d[1],
                            d => d[0] * d[1] * d[1]);/*
                            d => d[0] * d[0] * d[0] * d[0],
                            d => d[0] * d[0] * d[0] * d[1],
                            d => d[0] * d[0] * d[1] * d[1],
                            d => d[0] * d[1] * d[1] * d[1],
                            d => d[1] * d[1] * d[1] * d[1]);*/
                            double goodnessLinearMultiDimFunc = GoodnessOfFit.RSquared(facts.Select(x => linearMultiDimFunc(new[] { x.DifferenceInGlucose.Value, x.XE })).ToArray(), facts.Select(x => x.Dose).ToArray());

                            Func<Double[], double> multiDimFuncQR = Fit.MultiDimFunc(xyData, zData, false, DirectRegressionMethod.QR);
                            double goodnessMultiDimFuncQR = GoodnessOfFit.RSquared(facts.Select(x => multiDimFuncQR(new[] { x.DifferenceInGlucose.Value, x.XE })).ToArray(), facts.Select(x => x.Dose).ToArray());

                            Func<Double[], double> multiDimFuncNormalEquations = Fit.MultiDimFunc(xyData, zData, false, DirectRegressionMethod.NormalEquations);
                            double goodnessMultiDimFuncNormalEquations = GoodnessOfFit.RSquared(facts.Select(x => multiDimFuncNormalEquations(new[] { x.DifferenceInGlucose.Value, x.XE })).ToArray(), facts.Select(x => x.Dose).ToArray());

                            Func<Double[], double> multiDimFuncSvd = Fit.MultiDimFunc(xyData, zData, false, DirectRegressionMethod.Svd);
                            double goodnessMultiDimFuncSvd = GoodnessOfFit.RSquared(facts.Select(x => multiDimFuncSvd(new[] { x.DifferenceInGlucose.Value, x.XE })).ToArray(), facts.Select(x => x.Dose).ToArray());

                            double[][] xykData = facts.Select(x => new[] { x.DifferenceInGlucose.Value, x.XE, x.Time }).ToArray();
                            Func<Double[], double> multiDimFunc3 = Fit.MultiDimFunc(xykData, zData, false, DirectRegressionMethod.QR);
                            double goodnessMultiDimFunc3 = GoodnessOfFit.RSquared(facts.Select(x => multiDimFunc3(new[] { x.DifferenceInGlucose.Value, x.XE, x.Time })).ToArray(), facts.Select(x => x.Dose).ToArray());

                            double[][] xykjData = facts.Select(x => new[] { x.DifferenceInGlucose.Value, x.XE, x.Time, x.Before }).ToArray();
                            Func<Double[], double> multiDimFunc4 = Fit.MultiDimFunc(xykjData, zData, false, DirectRegressionMethod.QR);
                            double goodnessMultiDimFunc4 = GoodnessOfFit.RSquared(facts.Select(x => multiDimFunc4(new[] { x.DifferenceInGlucose.Value, x.XE, x.Time, x.Before})).ToArray(), facts.Select(x => x.Dose).ToArray());

                            chart3D.Legends[0].Title = "XE";
                            chart3D.ChartAreas[0].AxisX.Title = "DifferenceInGlucose";
                            chart3D.ChartAreas[0].AxisY.Title = "Insulin dose";
                            for (int y = 14; y >= 0; y--)
                            {
                                chart3D.Series.Add(y.ToString());
                                chart3D.Series[y.ToString()].ChartType = SeriesChartType.Column;
                                for (int x = -10; x < 13; x++)
                                {
                                    chart3D.Series[y.ToString()].Points.AddXY(x, Math.Round(linearMultiDimFunc(new double[] { ((double)x), ((double)y) }), 2));
                                }
                            }

                            
                        }
                        break;
                    case "My dose(XE, Time)":
                        {
                            chart3D.Legends[0].Title = "Time";
                            List<Fact> f = new List<Fact>();
                            f = facts;
                            chart3D.Series.Clear();
                            chart3D.ChartAreas[0].AxisX.IsReversed = true;
                            chart3D.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                            chart3D.ChartAreas[0].AxisX.Minimum = 0;
                            chart3D.ChartAreas[0].AxisX.Maximum = f.Select(x => x.XE).Max();
                            chart3D.ChartAreas[0].AxisX.Title = "XE";
                            chart3D.ChartAreas[0].AxisY.Title = "Insulin dose";
                            int max = (int)Math.Ceiling(f.Select(x => x.Time).Max());
                            for (int j = max; j >= 0; j -= 2)
                            {
                                chart3D.Series.Add(j.ToString());
                                chart3D.Series[j.ToString()].ChartType = SeriesChartType.Column;
                                chart3D.Series[j.ToString()].MarkerSize = 15;
                                for (int i = 0; i < f.Count; i++)
                                {
                                    if ((f[i].Time >= j) && (f[i].Time < j + 2))
                                    {
                                        chart3D.Series[j.ToString()].Points.AddXY(f[i].XE, f[i].Dose);
                                        //chart3D.Series[j.ToString()].Points.Last().Label = j.ToString();
                                    }
                                    foreach (var item in chart3D.Series[j.ToString()].Points)
                                    {
                                        double valueY = item.YValues.Average();
                                        double valueX = item.XValue;
                                        item.SetDefault(true);
                                        item.XValue = valueX;
                                        item.YValues.SetValue(valueY, 0);

                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
                UpdateDataGridView();

            }
            catch (Exception)
            {
                MessageBox.Show("Error. " + comboBoxGlucose.SelectedItem.ToString() + ". Few data");
            }
        }


        private void trackBarGlucose_Scroll(object sender, EventArgs e)
        {
            labelGlucose.Text = String.Format("Glucose up to {0}", trackBarGlucose.Value);
            comboBoxGlucose_SelectedIndexChanged(sender, e);
            comboBoxXE_SelectedIndexChanged(sender, e);
        }

        private void trackBarXE_Scroll(object sender, EventArgs e)
        {
            labelXE.Text = String.Format("XE up to {0}", trackBarXE.Value);
            comboBoxGlucose_SelectedIndexChanged(sender, e);
            comboBoxXE_SelectedIndexChanged(sender, e);
        }

        private void numericUpDownCoefForGoodFacts_ValueChanged(object sender, EventArgs e)
        {
            comboBoxGlucose_SelectedIndexChanged(sender, e);
            comboBoxXE_SelectedIndexChanged(sender, e);
        }
    }
}
