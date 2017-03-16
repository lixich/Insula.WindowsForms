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
        public List<Fact> facts = new List<Fact>();

        public void UpdateDataGridView()
        {
            Patient p = currentPatient;
            dataGridViewAll.DataSource = null;
            dataGridViewAll.Rows.Clear();
            dataGridViewAll.ColumnCount = 8;
            dataGridViewAll.ColumnHeadersVisible = true;

            dataGridViewAll.Columns[0].Name = "Time";
            dataGridViewAll.Columns[1].Name = "XE";
            dataGridViewAll.Columns[2].Name = "Glucose before meal";
            dataGridViewAll.Columns[3].Name = "Glucose after meal";
            dataGridViewAll.Columns[4].Name = "Dose";
            dataGridViewAll.Columns[5].Name = "Dose for 1 XE";
            dataGridViewAll.Columns[6].Name = "Dose for 1 mmol/L";
            dataGridViewAll.Columns[7].Name = "Comment";

            foreach (Fact f in p.Facts.ToList())
                if ((f.After == 0) || (f.After == 0))
                {
                    dataGridViewAll.Rows.Add(new string[] { f.DateTime.Value.TimeOfDay.ToString().Substring(0, 8), f.XE.ToString(), f.Before.ToString(), f.After.ToString(), f.Dose.ToString(), f.InsXE != 0 ? f.InsXE.ToString() : "-", f.InsGlu != 0 ? f.InsGlu.ToString(): "-", "Error" });
                }
                else
                {
                    if ((f.After > 8) || (f.After < 4))
                        dataGridViewAll.Rows.Add(new string[] { f.DateTime.Value.TimeOfDay.ToString().Substring(0, 8), f.XE.ToString(), f.Before.ToString(), f.After.ToString(), f.Dose.ToString(), f.InsXE != 0 ? f.InsXE.ToString() : "-", f.InsGlu != 0 ? f.InsGlu.ToString() : "-", "Bad level of glucose" });
                    else
                        dataGridViewAll.Rows.Add(new string[] { f.DateTime.Value.TimeOfDay.ToString().Substring(0, 8), f.XE.ToString(), f.Before.ToString(), f.After.ToString(), f.Dose.ToString(), f.InsXE != 0 ? f.InsXE.ToString() : "-", f.InsGlu != 0 ? f.InsGlu.ToString() : "-", "" });
                }
            foreach (DataGridViewRow item in dataGridViewAll.Rows)
            {
                if (double.Parse(item.Cells[2].Value.ToString()) < 4)
                    item.Cells[2].Style.ForeColor = Color.Blue;
                if (double.Parse(item.Cells[3].Value.ToString()) < 4)
                    item.Cells[3].Style.ForeColor = Color.Blue;
                if (double.Parse(item.Cells[2].Value.ToString()) > 8)
                    item.Cells[2].Style.ForeColor = Color.Red;
                if (double.Parse(item.Cells[3].Value.ToString()) > 8)
                    item.Cells[3].Style.ForeColor = Color.Red;
            }
        }

        public FormStatistics(Patient p)
        {
            InitializeComponent();
            currentPatient = p;
            facts = p.Facts;
            UpdateDataGridView();
            comboBoxGlucose.SelectedIndex = 0;
            comboBoxXE.SelectedIndex = 0;
        }

        private void comboBoxXE_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (comboBoxXE.SelectedItem.ToString())
            {
                case "Insulin dose for 1 XE":
                    List<Fact> goodFacts = facts.Where(x => Math.Abs(x.After - x.Before) <= 1.5).ToList();
                    List<Fact> badFacts = facts.Where(x => Math.Abs(x.After - x.Before) >= 1).ToList();

                    foreach (var item in goodFacts)
                        if (item.XE != 0)
                            item.InsXE = Math.Round(item.Dose / item.XE, 4);

                    goodFacts = goodFacts.Where(x => x.InsXE > 0).ToList();

                    double[] xdataXE = goodFacts.Select(x => x.Time).ToArray();
                    double[] ydataXE = goodFacts.Select(x => x.InsXE).ToArray();

                    Func<double, double> lineXE = Fit.LineFunc(xdataXE, ydataXE);
                    double goodnessLine = GoodnessOfFit.RSquared(xdataXE.Select(x => lineXE(x)), ydataXE);

                    Func<double, double> polynomOrder2XE = Fit.PolynomialFunc(xdataXE, ydataXE, 2); // polynomial of order 2
                    double goodnessPolynomOrder2 = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder2XE(x)), ydataXE);

                    Func<double, double> polynomOrder3XE = Fit.PolynomialFunc(xdataXE, ydataXE, 3); // polynomial of order 3
                    double goodnessPolynomOrder3 = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder3XE(x)), ydataXE);

                    Func<double, double> polynomOrder4XE = Fit.PolynomialFunc(xdataXE, ydataXE, 4); // polynomial of order 4
                    double goodnessPolynomOrder6 = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder4XE(x)), ydataXE);

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
                    double exampleP6XE = polynomOrder4XE(13.0);
                    double exampleLCXE = sinusoidalCombinationXE(13.0);
                    double exampleNCXE = harmonicCombinationXE(13.0);
                    chartXE.Series.Clear();
                    chartXE.Series.Add("line");
                    chartXE.Series.Add("polynom");
                    chartXE.Series.Add("sinusoidal");
                    chartXE.Series.Add("harmonic");
                    chartXE.Series.Add("data");
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
                    for (int i = 0; i < 24; i++)
                    {
                        //chartXE.Series["line"].Points.AddXY(i, lineXE(i));
                        //chartXE.Series["polynom"].Points.AddXY(i, polynomOrder3XE(i));
                        chartXE.Series["sinusoidal"].Points.AddXY(i, sinusoidalCombinationXE(i));
                        chartXE.Series["harmonic"].Points.AddXY(i, harmonicCombinationXE(i));

                    }
                    for (int i = 0; i < xdataXE.Length; i++)
                    {
                        chartXE.Series["data"].Points.AddXY(xdataXE[i], ydataXE[i]);
                    }
                    break;
                case "XE":
                    chartXE.Series.Clear();
                    chartXE.Series.Add("data");
                    chartXE.Series["data"].ChartType = SeriesChartType.Point;
                    for (int i = 0; i < facts.Count; i++)
                    {
                        if (facts[i].XE != 0)
                            chartXE.Series["data"].Points.AddXY(facts[i].Time, facts[i].XE);
                    }


                    break;
                default:
                    break;
            }
            UpdateDataGridView();
        }

        private void comboBoxGlucose_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBoxGlucose.SelectedItem.ToString())
                {
                    case "Insulin dose to lower glucose by 1 mmol/L":

                        List<Fact> goodFacts = facts.Where(x => Math.Abs(x.After - x.Before) <= 1.5).ToList();
                        List<Fact> badFacts = facts.Where(x => Math.Abs(x.After - x.Before) >= 1 && x.Before <= trackBarGlucose.Value && x.XE <= trackBarXE.Value).ToList();

                        foreach (var item in goodFacts)
                            if (item.XE != 0)
                                item.InsXE = Math.Round(item.Dose / item.XE, 4);

                        goodFacts = goodFacts.Where(x => x.InsXE > 0).ToList();

                        double[] xdataXE = goodFacts.Select(x => x.Time).ToArray();
                        double[] ydataXE = goodFacts.Select(x => x.InsXE).ToArray();

                        Func<double, double> lineXE = Fit.LineFunc(xdataXE, ydataXE);
                        double goodnessLine = GoodnessOfFit.RSquared(xdataXE.Select(x => lineXE(x)), ydataXE);

                        Func<double, double> polynomOrder2XE = Fit.PolynomialFunc(xdataXE, ydataXE, 2); // polynomial of order 2
                        double goodnessPolynomOrder2XE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder2XE(x)), ydataXE);

                        Func<double, double> polynomOrder3XE = Fit.PolynomialFunc(xdataXE, ydataXE, 3); // polynomial of order 3
                        double goodnessPolynomOrder3 = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder3XE(x)), ydataXE);

                        Func<double, double> polynomOrder4XE = Fit.PolynomialFunc(xdataXE, ydataXE, 4); // polynomial of order 4
                        double goodnessPolynomOrder6XE = GoodnessOfFit.RSquared(xdataXE.Select(x => polynomOrder4XE(x)), ydataXE);

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
                        double exampleP6XE = polynomOrder4XE(13.0);
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

                        Func<double, double> polynomOrder6Glucose = Fit.PolynomialFunc(xdataGlucose, ydataGlucose, 6); // polynomial of order 3
                        double goodnessPolynomOrder6Glucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => polynomOrder6Glucose(x)), ydataGlucose);

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
                        double exampleP6Glucose = polynomOrder6Glucose(13.0);
                        double exampleLCGlucose = sinusoidalCombinationGlucose(13.0);
                        double exampleHCGlucose = harmonicCombinationGlucose(13.0);

                        double nowGlu = 9.9;
                        double nowXE = 4;
                        double nowTime = 13;
                        //double nowInsulin = ?;

                        double exampleLFull = lineXE(nowTime) * nowXE + (nowGlu - 6) * lineGlucose(nowTime);
                        double exampleP2Full = polynomOrder2XE(nowTime) * nowXE + (nowGlu - 6) * polynomOrder2Glucose(nowTime);
                        double exampleP3Full = polynomOrder3XE(nowTime) * nowXE + (nowGlu - 6) * polynomOrder3Glucose(nowTime);
                        double exampleP6Full = polynomOrder4XE(nowTime) * nowXE + (nowGlu - 6) * polynomOrder6Glucose(nowTime);
                        double exampleLCFull = sinusoidalCombinationXE(nowTime) * nowXE + (nowGlu - 6) * sinusoidalCombinationGlucose(nowTime);
                        double exampleHCFull = harmonicCombinationXE(nowTime) * nowXE + (nowGlu - 6) * harmonicCombinationGlucose(nowTime);

                        chartGlucose.Series.Clear();
                        chartGlucose.Series.Add("line");
                        chartGlucose.Series.Add("polynom");
                        chartGlucose.Series.Add("sinusoidal");
                        chartGlucose.Series.Add("harmonic");
                        chartGlucose.Series.Add("data");
                        chartGlucose.Series["data"].ChartType = SeriesChartType.Point;
                        chartGlucose.Series["data"].Color = Color.Black;
                        chartGlucose.Series["line"].ChartType = SeriesChartType.Line;
                        chartGlucose.Series["polynom"].ChartType = SeriesChartType.Spline;
                        chartGlucose.Series["sinusoidal"].ChartType = SeriesChartType.Spline;
                        chartGlucose.Series["harmonic"].ChartType = SeriesChartType.Spline;
                        chartGlucose.Series["data"].MarkerSize = 7;
                        chartGlucose.Series["line"].BorderWidth = 4;
                        chartGlucose.Series["polynom"].BorderWidth = 4;
                        chartGlucose.Series["sinusoidal"].BorderWidth = 4;
                        chartGlucose.Series["harmonic"].BorderWidth = 4;
                        for (int i = 0; i < 25; i++)
                        {
                            //chartGlucose.Series["line"].Points.AddXY(i, lineGlucose(i));
                            //chartGlucose.Series["polynom"].Points.AddXY(i, polynomOrder3Glucose(i));
                            chartGlucose.Series["sinusoidal"].Points.AddXY(i, sinusoidalCombinationGlucose(i));
                            chartGlucose.Series["harmonic"].Points.AddXY(i, harmonicCombinationGlucose(i));

                        }
                        for (int i = 0; i < xdataGlucose.Length; i++)
                        {
                            chartGlucose.Series["data"].Points.AddXY(xdataGlucose[i], ydataGlucose[i]);
                        }

                        break;
                    case "Glucose":
                        chartGlucose.Series.Clear();
                        chartGlucose.Series.Add("data");
                        chartGlucose.Series["data"].ChartType = SeriesChartType.Point;
                        for (int i = 0; i < facts.Count; i++)
                        {
                            if (facts[i].Before != 0)
                                chartGlucose.Series["data"].Points.AddXY(facts[i].Time, facts[i].Before);
                        }


                        break;
                    default:
                        break;
                }
                UpdateDataGridView();

            }
            catch (Exception)
            {

                MessageBox.Show("Error");
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
