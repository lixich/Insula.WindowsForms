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
using System.Numerics;
using System.Windows.Forms.DataVisualization.Charting;


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

            foreach (Fact f in p.Facts.Where(x =>x.Before <= trackBarGlucose.Value && x.XE <= trackBarXE.Value).ToList())
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
            List<Fact> facts = lstFacts.Where(x => x.Before <= trackBarGlucose.Value && x.XE <= trackBarXE.Value).ToList();
            try
            {
                switch (comboBoxXE.SelectedItem.ToString())
                {
                    case "Insulin dose for 1 XE":
                        {
                            List<Fact> goodFacts = facts.Where(x => Math.Abs(x.After - x.Before) <= 1.5).ToList();
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
                            for (int i = 1; i < xdataXE.Length; i++)
                            {
                                chart2D.Series["data"].Points.AddXY(xdataXE[i], ydataXE[i]);
                            }
                        }
                        break;
                    case "XE":
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
                            chart2D.ChartAreas[0].AxisY.Interval = 1;
                            chart2D.Series["meal"].ChartType = SeriesChartType.Point;
                            for (int i = 0; i < f.Count; i++)
                            {
                                if (f[i].XE != 0)
                                    chart2D.Series["meal"].Points.AddXY(f[i].Time, f[i].XE);
                            }
                        }
                        break;
                    case "Insulin dose to lower glucose by 1 mmol/L":
                        {
                            List<Fact> goodFacts = facts.Where(x => Math.Abs(x.After - x.Before) <= 1.5).ToList();
                            List<Fact> badFacts = facts.Where(x => Math.Abs(x.After - x.Before) >= 1).ToList();

                            foreach (var item in goodFacts)
                                if (item.XE != 0)
                                    item.InsXE = Math.Round(item.Dose / item.XE, 4);

                            goodFacts = goodFacts.Where(x => x.InsXE > 0).ToList();

                            double[] xdataXE = goodFacts.Select(x => x.Time).ToArray();
                            double[] ydataXE = goodFacts.Select(x => x.InsXE).ToArray();

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
                            badFacts = badFacts.Where(x => x.InsGlu > 0).ToList();

                            double[] xdataGlucose = badFacts.Select(x => x.Time).ToArray();
                            double[] ydataGlucose = badFacts.Select(x => x.InsGlu).ToArray();

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
                        }
                        break;
                    case "Glucose":
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
                            chart2D.ChartAreas[0].AxisY.Interval = 1;
                            chart2D.Series["Before meal"].ChartType = SeriesChartType.Point;
                            for (int i = 0; i < f.Count; i++)
                            {
                                if (f[i].Before != 0)
                                    chart2D.Series["Before meal"].Points.AddXY(f[i].Time, f[i].Before);
                            }
                        }
                        break;
                    case "Dose":
                        {
                            List<Fact> f = new List<Fact>();
                            f = facts;
                            chart2D.Series.Clear();
                            chart2D.Series.Add("For meal");
                            chart2D.ChartAreas[0].AxisX.Minimum = 0;
                            chart2D.ChartAreas[0].AxisX.Maximum = 24;
                            chart2D.ChartAreas[0].AxisX.Interval = 2;
                            chart2D.ChartAreas[0].AxisY.Minimum = 0;
                            chart2D.ChartAreas[0].AxisY.Maximum = 20;
                            chart2D.ChartAreas[0].AxisY.Interval = 1;
                            chart2D.Series["For meal"].ChartType = SeriesChartType.Point;
                            for (int i = 0; i < f.Count; i++)
                            {
                                chart2D.Series["For meal"].Points.AddXY(f[i].Time, f[i].Dose);
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
                MessageBox.Show("Error. "+ comboBoxXE.SelectedItem.ToString()  + ". Few data");
            }
        }

        private void comboBoxGlucose_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Fact> facts = lstFacts.Where(x => x.Before <= trackBarGlucose.Value && x.XE <= trackBarXE.Value).ToList();
            try
            {
                switch (comboBoxGlucose.SelectedItem.ToString())
                {
                    case "Insulin dose for 1 XE":
                        {
                            List<Fact> goodFacts = facts.Where(x => Math.Abs(x.After - x.Before) <= 1.5).ToList();
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
                            chart3D.ChartAreas[0].AxisX.Minimum = 0;
                            chart3D.ChartAreas[0].AxisX.Maximum = 24;
                            chart3D.ChartAreas[0].AxisX.Interval = 2;
                            chart3D.ChartAreas[0].AxisY.Minimum = 0;
                            chart3D.ChartAreas[0].AxisY.Maximum = (ydataXE.Max() > 10) ? 10 : Math.Ceiling(ydataXE.Max());
                            chart3D.ChartAreas[0].AxisY.Interval = 0.5;
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
                            for (int j = 18; j >= 0; j-=2)
                            {
                                chart3D.Series.Add(j.ToString());
                                chart3D.Series[j.ToString()].ChartType = SeriesChartType.Column;
                                chart3D.Series[j.ToString()].MarkerSize = 10;
                                for (int i = 1; i < xdataXE.Length; i++)
                                {
                                    if ((zdataXE[i] >= j) && (zdataXE[i] < j+2))
                                    chart3D.Series[j.ToString()].Points.AddXY(xdataXE[i], ydataXE[i]);
                                }
                            }
                        }
                        break;

                    case "Insulin dose to lower glucose by 1 mmol/L":
                        {
                            
                            List<Fact> goodFacts = facts.Where(x => Math.Abs(x.After - x.Before) <= 1.5).ToList();
                            List<Fact> badFacts = facts.Where(x => Math.Abs(x.After - x.Before) >= 1).ToList();

                            foreach (var item in goodFacts)
                                if (item.XE != 0)
                                    item.InsXE = Math.Round(item.Dose / item.XE, 4);

                            goodFacts = goodFacts.Where(x => x.InsXE > 0).ToList();

                            double[] xdataXE = goodFacts.Select(x => x.Time).ToArray();
                            double[] ydataXE = goodFacts.Select(x => x.InsXE).ToArray();

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
                            badFacts = badFacts.Where(x => x.InsGlu > 0).ToList();

                            double[] xdataGlucose = badFacts.Select(x => x.Time).ToArray();
                            double[] ydataGlucose = badFacts.Select(x => x.InsGlu).ToArray();

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
                            /*
                            chartXE.Series.Clear();
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
                        }
                        break;

                    case "Dose":
                        {
                            List<Fact> f = new List<Fact>();
                            f = facts;
                            chart3D.Series.Clear();
                            chart3D.ChartAreas[0].AxisX.IsReversed = true;
                            chart3D.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                            for (int j = 10; j >= 0; j-=2)
                            {
                                chart3D.Series.Add(j.ToString());
                                chart3D.Series[j.ToString()].ChartType = SeriesChartType.Column;
                                //chart3D.Series[j.ToString()].MarkerSize = 10;
                                for (int i = 0; i < f.Count; i++)
                                {
                                    if ((f[i].Before - f[i].After >= j) && (f[i].Before - f[i].After < j+2))
                                    chart3D.Series[j.ToString()].Points.AddXY(f[i].XE, f[i].Dose);
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

    }
}
