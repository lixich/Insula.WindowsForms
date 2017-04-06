using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using MathNet.Numerics.Statistics;
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
        List<Fact> lstAll = new List<Fact>();

        public NewDose(Patient p)
        {
            InitializeComponent();
            lstAll = p.Facts.Where(x => x.DifferenceInGlucose.HasValue).ToList();
            tbTime.Text = DateTime.Now.TimeOfDay.ToString();
            numCurrentGlucose.Value = 6;
            cmbNumberNeighbors.Text = "5";
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            try
            {
                List<Fact> lst = new List<Fact>();
                List<Fact> lst2 = new List<Fact>();
                int n;
                n = lstAll.Count / 3 * 2;
                for (int i = 0; i < lstAll.Count; i++)
                {
                    if (i < n)
                        lst.Add(lstAll[i]);
                    else lst2.Add(lstAll[i]);
                    //lst.Add(lstAll[i]);
                    //lst2.Add(lstAll[i]);
                }
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
                    double d4 = (Math.Min(Math.Abs((item.DateTime.TimeOfDay - time).TotalMinutes), 24 * 60 - Math.Abs((item.DateTime.TimeOfDay - time).TotalMinutes))) / (24 * 60);
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
                   dataGridView1.Rows.Add(new string[] { f.DateTime.TimeOfDay.ToString(), f.XE.ToString(), f.Before.ToString(), f.After.ToString(), f.Dose.ToString(), Math.Round(f.Coef,2).ToString() });
                for (int i = 0; i < numNeighbors; i++)
                {
                   dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                }



                //----------------
                List<Fact> goodFacts = lst.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) < 2).ToList();
                List<Fact> badFacts = lst.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) >= 1).ToList();
                
                ///Коэффицент углевдов

                foreach (var item in goodFacts)
                    if (item.XE != 0)
                        item.InsXE = Math.Round(item.Dose / item.XE, 4);

                goodFacts = goodFacts.Where(x => x.InsXE > 0).ToList();

                double[] xdataXE = goodFacts.Select(x => x.Time).ToArray();
                double[] ydataXE = goodFacts.Select(x => x.InsXE).ToArray();
                double[] zdataXE = goodFacts.Select(x => x.XE).ToArray();

                Func<double, double> lineXE = Fit.LineFunc(xdataXE, ydataXE);
                double goodnessLineXE = GoodnessOfFit.RSquared(xdataXE.Select(x => lineXE(x)), ydataXE);

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
                double goodnessHarmonicCombinationXE = GoodnessOfFit.RSquared(xdataXE.Select(x => sinusoidalCombinationXE(x)), ydataXE);

                double exampleLXE = lineXE(13.0);
                double exampleP3XE = polynomOrder3XE(13.0);
                double exampleLCXE = sinusoidalCombinationXE(13.0);
                double exampleNCXE = harmonicCombinationXE(13.0);


                ////Чувствительность к инсулину

                badFacts = lst.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) >= 1).ToList();
                foreach (var item in badFacts)
                {
                    //if (item.Before > item.After)
                    if (item.Before - item.After != 0)
                    {
                        item.InsGlu = Math.Round((item.Dose - (lineXE(item.Time) * item.XE)) / (item.Before - item.After), 4);
                    }
                }
                badFacts = badFacts.Where(x => x.InsGlu > 0).ToList();
                double[] xdataGlucose = badFacts.Select(x => x.Time).ToArray();
                double[] ydataGlucose = badFacts.Select(x => x.InsGlu).ToArray();
                double[] zdataGlucose = badFacts.Select(x => x.XE).ToArray();
                Func<double, double> lineGlucose = Fit.LineFunc(xdataGlucose, ydataGlucose);
                double goodnessLineGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => lineGlucose(x)), ydataGlucose);





                badFacts = lst.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) >= 1).ToList();
                foreach (var item in badFacts)
                {
                    //if (item.Before > item.After)
                    if (item.Before - item.After != 0)
                    {
                        item.InsGlu = Math.Round((item.Dose - (polynomOrder3XE(item.Time) * item.XE)) / (item.Before - item.After), 4);
                    }
                }
                badFacts = badFacts.Where(x => x.InsGlu > 0).ToList();
                xdataGlucose = badFacts.Select(x => x.Time).ToArray();
                ydataGlucose = badFacts.Select(x => x.InsGlu).ToArray();
                zdataGlucose = badFacts.Select(x => x.XE).ToArray();
                Func<double, double> polynomOrder3Glucose = Fit.PolynomialFunc(xdataGlucose, ydataGlucose, 3); // polynomial of order 3
                double goodnessPolynomOrder3Glucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => polynomOrder3Glucose(x)), ydataGlucose);



                badFacts = lst.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) >= 1).ToList();
                foreach (var item in badFacts)
                {
                    //if (item.Before > item.After)
                    if (item.Before - item.After != 0)
                    {
                        item.InsGlu = Math.Round((item.Dose - (sinusoidalCombinationXE(item.Time) * item.XE)) / (item.Before - item.After), 4);
                    }
                }
                badFacts = badFacts.Where(x => x.InsGlu > 0).ToList();
                xdataGlucose = badFacts.Select(x => x.Time).ToArray();
                ydataGlucose = badFacts.Select(x => x.InsGlu).ToArray();
                zdataGlucose = badFacts.Select(x => x.XE).ToArray();
                Func<double, double> sinusoidalCombinationGlucose = Fit.LinearCombinationFunc(
                xdataGlucose,
                ydataGlucose,
                x => 1.0,
                x => Math.Sin(omega * x),
                x => Math.Cos(omega * x));
                double goodnessSinusoidalCombinationGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => sinusoidalCombinationGlucose(x)), ydataGlucose);

                



                badFacts = lst.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) >= 1).ToList();
                foreach (var item in badFacts)
                {
                    //if (item.Before > item.After)
                    if (item.Before - item.After != 0)
                    {
                        item.InsGlu = Math.Round((item.Dose - (harmonicCombinationXE(item.Time) * item.XE)) / (item.Before - item.After), 4);
                    }
                }
                badFacts = badFacts.Where(x => x.InsGlu > 0).ToList();
                xdataGlucose = badFacts.Select(x => x.Time).ToArray();
                ydataGlucose = badFacts.Select(x => x.InsGlu).ToArray();
                zdataGlucose = badFacts.Select(x => x.XE).ToArray();
                Func<double, double> harmonicCombinationGlucose = Fit.LinearCombinationFunc(
                xdataGlucose,
                ydataGlucose,
                x => 1,
                x => SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * x)),
                x => SpecialFunctions.GeneralHarmonic(10, Math.Cos(omega * x)));
                double goodnessHarmonicCombinationGlucose = GoodnessOfFit.RSquared(xdataGlucose.Select(x => harmonicCombinationGlucose(x)), ydataGlucose);




                //пример
                double exampleLGlucose = lineGlucose(13.0);
                double exampleP3Glucose = polynomOrder3Glucose(13.0);
                double exampleLCGlucose = sinusoidalCombinationGlucose(13.0);
                double exampleHCGlucose = harmonicCombinationGlucose(13.0);

                double nowGlu = 9.9;
                double nowXE = 4;
                double nowTime = 13;
                //double nowInsulin = ?;

                double exampleLFull = lineXE(nowTime) * nowXE + (nowGlu - 6) * lineGlucose(nowTime);
                double exampleP3Full = polynomOrder3XE(nowTime) * nowXE + (nowGlu - 6) * polynomOrder3Glucose(nowTime);
                double exampleLCFull = sinusoidalCombinationXE(nowTime) * nowXE + (nowGlu - 6) * sinusoidalCombinationGlucose(nowTime);
                double exampleHCFull = harmonicCombinationXE(nowTime) * nowXE + (nowGlu - 6) * harmonicCombinationGlucose(nowTime);


                #region MyRegion
                /*

                                List<Fact> goodFacts2 = lst.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) < 2).ToList();
                                List<Fact> badFacts2 = lst.Where(x => x.DifferenceInGlucose.HasValue && Math.Abs(x.DifferenceInGlucose.Value) >= 1).ToList();

                                foreach (var item in goodFacts2)
                                    if (item.XE != 0)
                                        item.InsXE = Math.Round(item.Dose / item.XE, 4);

                                goodFacts2 = goodFacts2.Where(x => x.InsXE > 0).ToList();

                                double[][] xyDataXE2 = goodFacts2.Select(x => new[] { x.Time, x.XE }).ToArray();
                                double[] zdataXE2 = goodFacts2.Select(x => x.InsXE).ToArray();

                                Func<double[], double> lineXE2 = Fit.LinearMultiDimFunc(xyDataXE2, zdataXE2,
                                    x => 1.0,
                                    x => x[0] * x[1] * x[1]
                                    );
                                double goodnessLineXE2 = GoodnessOfFit.RSquared(xyDataXE2.Select(x => lineXE2(new[] { x[0], x[1] })), zdataXE2);

                                Func<double[], double> polynomOrder3XE2 = Fit.LinearMultiDimFunc(xyDataXE2, zdataXE2,
                                    x => 1.0,
                                    x => x[0] * x[1] * x[1],
                                    x => x[0] * x[0] * x[1] * x[1],
                                    x => x[0]* x[0] * x[0] * x[1] * x[1]
                                    );
                                double goodnessPolynomOrder3XE2 = GoodnessOfFit.RSquared(xyDataXE2.Select(x => polynomOrder3XE2(new[] { x[0], x[1] })), zdataXE2);

                                Func<double[], double> sinusoidalCombinationXE2 = Fit.LinearMultiDimFunc(xyDataXE2, zdataXE2,
                                x => 1.0,
                                x => Math.Sin(omega * x[0]) * x[1] * x[1]);
                                double goodnessSinusoidalCombinationXE2 = GoodnessOfFit.RSquared(xyDataXE2.Select(x => sinusoidalCombinationXE2(new[] { x[0], x[1] })), zdataXE2);

                                Func<double[], double> harmonicCombinationXE2 = Fit.LinearMultiDimFunc(xyDataXE2, zdataXE2,
                                x => 1,
                                x => SpecialFunctions.GeneralHarmonic(100, Math.Sin(omega * x[0])) * x[1]* x[1]);
                                double goodnessHarmonicCombinationXE2 = GoodnessOfFit.RSquared(xyDataXE2.Select(x => harmonicCombinationXE2(new[] { x[0], x[1] })), zdataXE2);

                                double exampleLXE2 = lineXE2(new[] { 13.0, 8.0 });
                                double exampleP3XE2 = polynomOrder3XE2(new[] { 13.0, 8.0 });
                                double exampleLCXE2 = sinusoidalCombinationXE2(new[] { 13.0, 8.0 });
                                double exampleNCXE2 = harmonicCombinationXE2(new[] { 13.0, 8.0 });

                                foreach (var item in badFacts)
                                {
                                    //if (item.Before > item.After)
                                    if (item.Before - item.After != 0)
                                    {
                                        item.InsGlu = Math.Round((item.Dose - (harmonicCombinationXE2(new[] { item.Time, item.Before }) * item.XE)) / (item.Before - item.After), 4);
                                    }
                                }
                                badFacts = badFacts.Where(x => x.InsGlu > 0).ToList();
                                /*
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

                    */
                #endregion






                lst = lst.Where(x => x.DifferenceInGlucose.HasValue).ToList();

                int successKNN = 0;
                List<double> sumKNN = new List<double>();
                List<double> ans = new List<double>();
                foreach (var it in lst2)
                {
                    foreach (Fact item in lst)
                    {

                        double d1 = (Math.Abs(item.XE - it.XE) / (lst.Select(x => x.XE).Max() - lst.Select(x => x.XE).Min()));
                        double d2 = (Math.Abs(item.Before - it.Before) / (lst.Select(x => x.Before).Max() - lst.Select(x => x.Before).Min()));
                        double d3 = (Math.Abs(item.After - it.After) / (lst.Select(x => x.After).Max() - lst.Select(x => x.After).Min()));
                        double d4 = (Math.Min(Math.Abs((item.DateTime.TimeOfDay - it.DateTime.TimeOfDay).TotalMinutes), 24 * 60 - Math.Abs((item.DateTime.TimeOfDay - it.DateTime.TimeOfDay).TotalMinutes))) / (24 * 60);
                        item.Coef = Math.Sqrt(d1 * d1 + d2 * d2 + d3 * d3 + d4 * d4);
                    }
                    List<Fact> lst1 = lst.OrderBy(x => x.Coef).ToList();
                    double dose1 = 0;

                    for (int i = 0; i < 5; i++)
                    {
                        dose1 += lst1[i].Dose;
                    }
                    dose1 /= 5;
                    ans.Add((int)dose1);
                    dose1 = Math.Round(dose1, 1);
                    sumKNN.Add(Math.Abs(dose1 - it.Dose));
                    if ((it.Dose == Math.Truncate(dose1)) || (it.Dose == Math.Truncate(dose1) + 1))
                        successKNN++;
                }
                double goodnessKNN = GoodnessOfFit.RSquared(ans, lst2.Select(x => x.Dose));


                double[][] xyData = lst.Select(x => new[] { x.DifferenceInGlucose.Value, x.XE }).ToArray();
                double[] zData = lst.Select(x => x.Dose).ToArray();
                double[][] xykjData = lst.Select(x => new[] { x.DifferenceInGlucose.Value, x.XE, x.Time, x.Before }).ToArray();


                ///////
                lst = lst2;
                ///////



                Func<Double[], double> multiDimFunc4 = Fit.MultiDimFunc(xykjData, zData, false, DirectRegressionMethod.QR);
                double goodnessMultiDimFunc4 = GoodnessOfFit.RSquared(lst.Select(x => multiDimFunc4(new[] { x.DifferenceInGlucose.Value, x.XE, x.Time, x.Before })).ToArray(), lst.Select(x => x.Dose).ToArray());
                goodnessMultiDimFunc4 = GoodnessOfFit.RSquared(lst.Select(x => multiDimFunc4(new[] { x.DifferenceInGlucose.Value, x.XE, x.Time, x.Before })).ToArray(), lst.Select(x => x.Dose).ToArray());

                Func<Double[], double> log = Fit.LinearMultiDimFunc(xykjData, zData,
                d => 1.0,
                d => SpecialFunctions.Logistic(d[0]),
                d => SpecialFunctions.Logistic(d[1]),
                d => SpecialFunctions.GeneralHarmonic(1000, Math.Sin(omega * d[2])),
                d => SpecialFunctions.GeneralHarmonic(1000, Math.Cos(omega * d[2])),
                d => SpecialFunctions.Logistic(d[3])
                    );
                double goodnessLog = GoodnessOfFit.RSquared(lst.Select(x => log(new[] { x.DifferenceInGlucose.Value, x.XE, x.Time, x.Before })).ToArray(), lst.Select(x => x.Dose).ToArray());

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
                d => d[0] * d[1] * d[1]);
                double goodnessLinearMultiDimFunc = GoodnessOfFit.RSquared(lst.Select(x => linearMultiDimFunc(new[] { x.DifferenceInGlucose.Value, x.XE })).ToArray(), lst.Select(x => x.Dose).ToArray());

                Func<Double[], double> linearMultiDimFuncTimeHarmonic = Fit.LinearMultiDimFunc(xykjData, zData,
                d => 1.0,
                d => d[0] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])),
                d => d[1] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])),
                d => d[0] * d[0] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])),
                d => d[1] * d[1] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])),
                d => d[0] * d[1] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])),
                d => d[0] * d[0] * d[0] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])),
                d => d[1] * d[1] * d[1] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])),
                d => d[0] * d[0] * d[1] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])),
                d => d[0] * d[1] * d[1] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])));
                double goodnessLinearMultiDimFunvTimeHarmonic = GoodnessOfFit.RSquared(lst.Select(x => linearMultiDimFuncTimeHarmonic(new[] { x.DifferenceInGlucose.Value/100, x.XE/100, x.Time })).ToArray(), lst.Select(x => x.Dose).ToArray());


                Func<Double[], double> linearMultiDimFuncTimeSin = Fit.LinearMultiDimFunc(xykjData, zData,
                d => 1.0,
                d => d[0] * Math.Sin(omega * d[2]),
                d => d[1] * Math.Sin(omega * d[2]),
                d => d[0] * d[0] * Math.Sin(omega * d[2]),
                d => d[1] * d[1] * Math.Sin(omega * d[2]),
                d => d[0] * d[1] * Math.Sin(omega * d[2]),
                d => d[0] * d[0] * d[0] * Math.Sin(omega * d[2]),
                d => d[1] * d[1] * d[1] * Math.Sin(omega * d[2]),
                d => d[0] * d[0] * d[1] * Math.Sin(omega * d[2]),
                d => d[0] * d[1] * d[1] * Math.Sin(omega * d[2]));
                double goodnessLinearMultiDimFunvTimeSin = GoodnessOfFit.RSquared(lst.Select(x => linearMultiDimFuncTimeSin(new[] { x.DifferenceInGlucose.Value, x.XE, x.Time })).ToArray(), lst.Select(x => x.Dose).ToArray());


                Func<Double[], double> linearMultiDimFuncTimeHarmonicBefore = Fit.LinearMultiDimFunc(xykjData, zData,
                d => 1.0,
                d => d[0] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])) * d[3],
                d => d[1] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])) * d[3],
                d => d[0] * d[0] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])) * d[3],
                d => d[1] * d[1] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])) * d[3],
                d => d[0] * d[1] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])) * d[3],
                d => d[0] * d[0] * d[0] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])) * d[3],
                d => d[1] * d[1] * d[1] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])) * d[3],
                d => d[0] * d[0] * d[1] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])) * d[3],
                d => d[0] * d[1] * d[1] * SpecialFunctions.GeneralHarmonic(10, Math.Sin(omega * d[2])) * d[3]);
                double goodnessLinearMultiDimFunvTimeHarmonicBefore = GoodnessOfFit.RSquared(lst.Select(x => linearMultiDimFuncTimeHarmonicBefore(new[] { x.DifferenceInGlucose.Value, x.XE, x.Time, x.Before })).ToArray(), lst.Select(x => x.Dose).ToArray());




                double goodnessLine = GoodnessOfFit.RSquared(lst.Select(x => Math.Round(lineXE(x.Time)*x.XE + lineGlucose(x.Time)*x.DifferenceInGlucose.Value, 0)), lst.Select(x => x.Dose));

                double goodnessPolynomOrder3 = GoodnessOfFit.RSquared(lst.Select(x => Math.Round(polynomOrder3XE(x.Time) * x.XE + polynomOrder3Glucose(x.Time) * x.DifferenceInGlucose.Value, 0)), lst.Select(x => x.Dose));

                double goodnessSinusoidalCombination = GoodnessOfFit.RSquared(lst.Select(x => Math.Round(sinusoidalCombinationXE(x.Time) * x.XE + sinusoidalCombinationGlucose(x.Time) * x.DifferenceInGlucose.Value, 0)), lst.Select(x => x.Dose));

                double goodnessHarmonicCombination = GoodnessOfFit.RSquared(lst.Select(x => Math.Round(harmonicCombinationXE(x.Time) * x.XE + harmonicCombinationGlucose(x.Time) * x.DifferenceInGlucose.Value, 0)), lst.Select(x => x.Dose));


                int successLine = 0;
                int successPolynom = 0;
                int successSinusoidal = 0;
                int successHarmonic = 0;
                int successDirectLinearMultiDimFunc = 0;
                int successDirectLinearMultiDimFuncTimeSin = 0;
                int successDirectLinearMultiDimFuncTimeHarmonic = 0;
                int successDirectLinearMultiDimFuncTimeHarmonicBefore = 0;
                int successDirectRegressionMethod = 0;
                int successLog = 0;
                int successPull = 0;


                List<double> sumLine = new List<double>();
                List<double> sumPolynom = new List<double>();
                List<double> sumSinusoidal = new List<double>();
                List<double> sumHarmonic = new List<double>();
                List<double> sumDirectLinearMultiDimFunc = new List<double>();
                List<double> sumDirectLinearMultiDimFuncTimeSin = new List<double>();
                List<double> sumDirectLinearMultiDimFuncTimeHarmonic = new List<double>();
                List<double> sumDirectLinearMultiDimFuncTimeHarmonicBefore = new List<double>();
                List<double> sumDirectRegressionMethod = new List<double>();
                List<double> sumLog = new List<double>();
                List<double> sumPull = new List<double>();

                int g = 0;
                foreach (var item in lst)
                {
                    double examplePull = 0;

                    examplePull += (harmonicCombinationXE(item.Time) * item.XE + (item.Before - item.After) * harmonicCombinationGlucose(item.Time));
                    int exampleHarmonic = (int)Math.Round(harmonicCombinationXE(item.Time) * item.XE + (item.Before - item.After) * harmonicCombinationGlucose(item.Time), 0);                   
                    sumHarmonic.Add( Math.Abs(item.Dose - (harmonicCombinationXE(item.Time) * item.XE + (item.Before - item.After) * harmonicCombinationGlucose(item.Time))));
                    //int exampleHarmonic = (int)Math.Truncate(harmonicCombinationXE(item.Time) * item.XE + (item.Before - item.After) * harmonicCombinationGlucose(item.Time));
                    if (exampleHarmonic == item.Dose)
                    //if ((exampleHarmonic == item.Dose) || (exampleHarmonic+1 == item.Dose))
                        successHarmonic++;

                    examplePull += (sinusoidalCombinationXE(item.Time) * item.XE + (item.Before - item.After) * sinusoidalCombinationGlucose(item.Time));
                    int exampleSinusoidal = (int)Math.Round(sinusoidalCombinationXE(item.Time) * item.XE + (item.Before - item.After) * sinusoidalCombinationGlucose(item.Time), 0);
                    //int exampleSinusoidal = (int)Math.Truncate(sinusoidalCombinationXE(item.Time) * item.XE + (item.Before - item.After) * sinusoidalCombinationGlucose(item.Time));
                    sumSinusoidal.Add(Math.Abs(item.Dose - (sinusoidalCombinationXE(item.Time) * item.XE + (item.Before - item.After) * sinusoidalCombinationGlucose(item.Time))));
                    if (exampleSinusoidal == item.Dose)
                    //if ((exampleSinusoidal == item.Dose) || (exampleSinusoidal + 1 == item.Dose))
                        successSinusoidal++;

                    examplePull += (polynomOrder3XE(item.Time) * item.XE + (item.Before - item.After) * polynomOrder3Glucose(item.Time));
                    sumPolynom.Add(Math.Abs(item.Dose - (polynomOrder3XE(item.Time) * item.XE + (item.Before - item.After) * polynomOrder3Glucose(item.Time))));
                    int examplePolynom = (int)Math.Round(polynomOrder3XE(item.Time) * item.XE + (item.Before - item.After) * polynomOrder3Glucose(item.Time), 0);
                    //int examplePolynom = (int)Math.Truncate(polynomOrder3XE(item.Time) * item.XE + (item.Before - item.After) * polynomOrder3Glucose(item.Time));
                    if (examplePolynom == item.Dose)
                    //if ((examplePolynom == item.Dose) || (examplePolynom + 1 == item.Dose))
                        successPolynom++;

                    examplePull += (lineXE(item.Time) * item.XE + (item.Before - item.After) * lineGlucose(item.Time));
                    sumLine.Add(Math.Abs(item.Dose - (lineXE(item.Time) * item.XE + (item.Before - item.After) * lineGlucose(item.Time))));
                    int exampleLine = (int)Math.Round(lineXE(item.Time) * item.XE + (item.Before - item.After) * lineGlucose(item.Time), 0);
                    //int exampleLine = (int)Math.Truncate(lineXE(item.Time) * item.XE + (item.Before - item.After) * lineGlucose(item.Time));
                    if (exampleLine == item.Dose)
                    //if ((exampleLine == item.Dose) || (exampleLine + 1 == item.Dose))
                        successLine++;

                    examplePull += multiDimFunc4(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time, item.Before });
                    sumDirectRegressionMethod.Add(Math.Abs(item.Dose - (multiDimFunc4(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time, item.Before }))));
                    int exampleDirectRegressionMethod = (int)Math.Round(multiDimFunc4(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time, item.Before }), 0);
                    //int exampleDirectRegressionMethod = (int)Math.Truncate(multiDimFunc4(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time, item.Before }));
                    if (exampleDirectRegressionMethod == item.Dose)
                    //if ((exampleDirectRegressionMethod == item.Dose) || (exampleDirectRegressionMethod + 1 == item.Dose))
                        successDirectRegressionMethod++;

                    examplePull += linearMultiDimFunc(new[] { item.DifferenceInGlucose.Value, item.XE });
                    sumDirectLinearMultiDimFunc.Add(Math.Abs(item.Dose - (linearMultiDimFunc(new[] { item.DifferenceInGlucose.Value, item.XE }))));
                    int exampleLinearMultiDimFunc = (int)Math.Round(linearMultiDimFunc(new[] {item.DifferenceInGlucose.Value, item.XE }), 0);
                    //int exampleLinearMultiDimFunc = (int)Math.Truncate(linearMultiDimFunc(new[] { item.DifferenceInGlucose.Value, item.XE }));
                    if (exampleLinearMultiDimFunc == item.Dose)
                    //if ((exampleLinearMultiDimFunc == item.Dose) || (exampleLinearMultiDimFunc + 1 == item.Dose))
                        successDirectLinearMultiDimFunc++;

                    examplePull += linearMultiDimFuncTimeHarmonic(new[] { item.DifferenceInGlucose.Value / 100, item.XE / 100, item.Time });
                    sumDirectLinearMultiDimFuncTimeHarmonic.Add(Math.Abs(item.Dose - (linearMultiDimFuncTimeHarmonic(new[] { item.DifferenceInGlucose.Value/100, item.XE/100, item.Time }))));
                    int exampleLinearMultiDimFuncTimeHarmonic = (int)Math.Round(linearMultiDimFuncTimeHarmonic(new[] { item.DifferenceInGlucose.Value/100, item.XE/100, item.Time }), 0);
                    //int exampleLinearMultiDimFuncTimeHarmonic = (int)Math.Truncate(linearMultiDimFuncTimeHarmonic(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time }));
                    if (exampleLinearMultiDimFuncTimeHarmonic == item.Dose)
                    //if ((exampleLinearMultiDimFuncTimeHarmonic == item.Dose) || (exampleLinearMultiDimFuncTimeHarmonic + 1 == item.Dose))
                        successDirectLinearMultiDimFuncTimeHarmonic++;

                    examplePull += linearMultiDimFuncTimeSin(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time });
                    sumDirectLinearMultiDimFuncTimeSin.Add(Math.Abs(item.Dose - (linearMultiDimFuncTimeSin(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time }))));
                    int exampleLinearMultiDimFuncTimeSin = (int)Math.Round(linearMultiDimFuncTimeSin(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time }), 0);
                    //int exampleLinearMultiDimFuncTimeSin = (int)Math.Truncate(linearMultiDimFuncTimeSin(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time }));
                    if (exampleLinearMultiDimFuncTimeSin == item.Dose)
                    //if ((exampleLinearMultiDimFuncTimeSin == item.Dose) || (exampleLinearMultiDimFuncTimeSin + 1 == item.Dose))
                        successDirectLinearMultiDimFuncTimeSin++;

                    examplePull += linearMultiDimFuncTimeHarmonicBefore(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time, item.Before });
                    sumDirectLinearMultiDimFuncTimeHarmonicBefore.Add(Math.Abs(item.Dose - (linearMultiDimFuncTimeHarmonicBefore(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time, item.Before }))));
                    int exampleLinearMultiDimFuncTimeHarmonicBefore = (int)Math.Round(linearMultiDimFuncTimeHarmonicBefore(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time, item.Before }), 0);
                    //int exampleLinearMultiDimFuncTimeHarmonicBefore = (int)Math.Truncate(linearMultiDimFuncTimeHarmonicBefore(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time, item.Before }));
                    if (exampleLinearMultiDimFuncTimeHarmonicBefore == item.Dose)
                    //if ((exampleLinearMultiDimFuncTimeHarmonicBefore == item.Dose) || (exampleLinearMultiDimFuncTimeHarmonicBefore + 1 == item.Dose))
                        successDirectLinearMultiDimFuncTimeHarmonicBefore++;

                    examplePull += sumKNN[g];
                    g++;
                    sumPull.Add(Math.Abs(item.Dose - examplePull/10));
                    //examplePull = Math.Round(examplePull/10, 0);
                    examplePull = Math.Truncate(examplePull / 10);
                    if (((int)examplePull == item.Dose) || ((int)examplePull + 1 == item.Dose))
                    //if ((int)examplePull == item.Dose)
                        successPull++;

                    //examplePull += log(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time, item.Before });
                    sumLog.Add(Math.Abs(item.Dose - (log(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time, item.Before }))));
                    int exampleLog = (int)Math.Round(log(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time, item.Before }), 0);
                    //int exampleLog = (int)Math.Truncate(log(new[] { item.DifferenceInGlucose.Value, item.XE, item.Time, item.Before }));
                    if (exampleLog == item.Dose)
                        //if ((exampleLog == item.Dose) || (exampleLog + 1 == item.Dose))
                        successLog++;
                }

                int k = lst.Count;
                double devKNN = Statistics.StandardDeviation(sumKNN);
                double devLine = Statistics.StandardDeviation(sumLine);
                double devPolynom = Statistics.StandardDeviation(sumPolynom);
                double devSinusoidal = Statistics.StandardDeviation(sumSinusoidal);
                double devHarmonic = Statistics.StandardDeviation(sumHarmonic);
                double devDirectLinearMultiDimFunc = Statistics.StandardDeviation(sumDirectLinearMultiDimFunc);
                double devDirectLinearMultiDimFuncTimeSin = Statistics.StandardDeviation(sumDirectLinearMultiDimFuncTimeSin);
                double devDirectLinearMultiDimFuncTimeHarmonic = Statistics.StandardDeviation(sumDirectLinearMultiDimFuncTimeHarmonic);
                double devDirectLinearMultiDimFuncTimeHarmonicBefore = Statistics.StandardDeviation(sumDirectLinearMultiDimFuncTimeHarmonicBefore);
                double devDirectRegressionMethod = Statistics.StandardDeviation(sumDirectRegressionMethod);
                double devLog = Statistics.StandardDeviation(sumLog);
                double devPull = Statistics.StandardDeviation(sumPull);

                double aveKNN = sumKNN.Average();
                double aveLine = sumLine.Average();
                double avePolynom = sumPolynom.Average();
                double aveSinusoidal = sumSinusoidal.Average();
                double aveHarmonic = sumHarmonic.Average();
                double aveDirectLinearMultiDimFunc = sumDirectLinearMultiDimFunc.Average();
                double aveDirectLinearMultiDimFuncTimeSin = sumDirectLinearMultiDimFuncTimeSin.Average();
                double aveDirectLinearMultiDimFuncTimeHarmonic = sumDirectLinearMultiDimFuncTimeHarmonic.Average();
                double aveDirectLinearMultiDimFuncTimeHarmonicBefore = sumDirectLinearMultiDimFuncTimeHarmonicBefore.Average();
                double aveDirectRegressionMethod = sumDirectRegressionMethod.Average();
                double aveLog = sumLog.Average();
                double avePull = sumPull.Average();

                double normaKNN = 0;
                double normaLine = 0;
                double normaPolynom = 0;
                double normaSinusoidal = 0;
                double normaHarmonic = 0;
                double normaDirectLinearMultiDimFunc = 0;
                double normaDirectLinearMultiDimFuncTimeSin = 0;
                double normaDirectLinearMultiDimFuncTimeHarmonic = 0;
                double normaDirectLinearMultiDimFuncTimeHarmonicBefore = 0;
                double normaDirectRegressionMethod = 0;
                double normaLog = 0;
                double normaPull = 0;

                for (int i = 0; i < k; i++)
                {

                     normaKNN += sumKNN[i]/lst[i].Dose;
                     normaLine += sumLine[i] / lst[i].Dose;
                    normaPolynom += sumPolynom[i] / lst[i].Dose;
                    normaSinusoidal += sumSinusoidal[i] / lst[i].Dose;
                    normaHarmonic += sumHarmonic[i] / lst[i].Dose;
                    normaDirectLinearMultiDimFunc += sumDirectLinearMultiDimFunc[i] / lst[i].Dose;
                    normaDirectLinearMultiDimFuncTimeSin += sumDirectLinearMultiDimFuncTimeSin[i] / lst[i].Dose;
                    normaDirectLinearMultiDimFuncTimeHarmonic += sumDirectLinearMultiDimFuncTimeHarmonic[i] / lst[i].Dose;
                    normaDirectLinearMultiDimFuncTimeHarmonicBefore += sumDirectLinearMultiDimFuncTimeHarmonicBefore[i] / lst[i].Dose;
                    normaDirectRegressionMethod += sumDirectRegressionMethod[i] / lst[i].Dose;
                    normaLog += sumLog[i] / lst[i].Dose; 
                    normaPull += sumPull[i] / lst[i].Dose; 
                }

                 normaKNN /= k;
                 normaLine /= k;
                normaPolynom /= k;
                normaSinusoidal /= k;
                normaHarmonic /= k;
                normaDirectLinearMultiDimFunc /= k;
                normaDirectLinearMultiDimFuncTimeSin /= k;
                normaDirectLinearMultiDimFuncTimeHarmonic /= k;
                normaDirectLinearMultiDimFuncTimeHarmonicBefore /= k;
                normaDirectRegressionMethod /= k;
                normaLog /= k;
                normaPull /= k;

                double example = harmonicCombinationXE(time.TotalMinutes/60) * XE + (before - after) * harmonicCombinationGlucose(time.TotalMinutes / 60);
                tbDose.Text = ((int)example).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
