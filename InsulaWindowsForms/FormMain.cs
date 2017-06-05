using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace InsulaWindowsForms
{
    public partial class FormMain : Form
    {
        public string path = @"C:\Users\Public\db.xml";
        public List<Patient> lstPatient = new List<Patient>();
        public Patient currentPatient = new Patient();

        public void UpdateForm()
        {
            comboBoxPatient.Items.AddRange(lstPatient.Select(x => x.Name).ToArray());
            if (comboBoxPatient.Items.Count != 0)
                comboBoxPatient.SelectedIndex = 0;
        }

        public void SaveData()
        {
            SaveFileDialog xmlSvfDialog = new SaveFileDialog();
            xmlSvfDialog.Filter = "XML Point file (*.xml)|*.xml";
            xmlSvfDialog.DefaultExt = "xml";
            if (xmlSvfDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream xmlFStream = null;
                XmlSerializer xmlFormatter = new XmlSerializer(typeof(List<Patient>), new Type[] { typeof(List<Fact>) });

                try
                {
                    xmlFStream = new FileStream(xmlSvfDialog.FileName, FileMode.Create, FileAccess.Write);
                    xmlFormatter.Serialize(xmlFStream, lstPatient);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (xmlFStream != null) xmlFStream.Close();
                }
            }
        }

        public void SaveData(string path)
        {

            FileStream xmlFStream = null;
            XmlSerializer xmlFormatter = new XmlSerializer(typeof(List<Patient>), new Type[] { typeof(List<Fact>) });

            try
            {
                xmlFStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                xmlFormatter.Serialize(xmlFStream, lstPatient);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (xmlFStream != null) xmlFStream.Close();
            }
        }

        public void OpenData()
        {
            FileStream xmlFStream = null;
            XmlSerializer xmlFormatter = new XmlSerializer(typeof(List<Patient>), new Type[] { typeof(List<Fact>) });
            OpenFileDialog xmlOpfDialog = new OpenFileDialog();
            xmlOpfDialog.Filter = "XML Point file (*.xml)|*.xml";

            if (xmlOpfDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    xmlFStream = new FileStream(xmlOpfDialog.FileName, FileMode.Open, FileAccess.Read);
                    lstPatient = (List<Patient>)xmlFormatter.Deserialize(xmlFStream);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (xmlFStream != null) xmlFStream.Close();
                }
            }
        }


        public void OpenData(string path)
        {
            FileStream xmlFStream = null;
            XmlSerializer xmlFormatter = new XmlSerializer(typeof(List<Patient>), new Type[] { typeof(List<Fact>) });

            try
            {
                xmlFStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                lstPatient = (List<Patient>)xmlFormatter.Deserialize(xmlFStream);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (xmlFStream != null) xmlFStream.Close();
            }
        }

        public void AddData()
        {

            Patient p;
            List<Fact> facts = new List<Fact>();
            #region//Load database
            //Add(Время(Год, Месяц, День, Час, Минута, Секунда), Инсулин, ХЕ, До, После);
            p = new Patient();
            p.Name = "Антонов А.А.";
            p.Insulin = "Регуляр";
            p.Weight = 50;
            p.Growth = 177;
            p.DateOfBirth = new DateTime(2000, 4, 23);
            lstPatient.Add(p);
            facts = new List<Fact>();
            facts.Add(new Fact(new DateTime(2014, 1, 13, 8, 0, 0), 4, 2, 8.0, 9.9));
            facts.Add(new Fact(new DateTime(2014, 1, 13, 13, 0, 0), 8, 4, 9.9, 7.9));
            facts.Add(new Fact(new DateTime(2014, 1, 13, 18, 0, 0), 14, 6, 7.9, 13.0));

            facts.Add(new Fact(new DateTime(2014, 1, 14, 8, 0, 0), 8, 3, 12.0, 5.9));
            facts.Add(new Fact(new DateTime(2014, 1, 14, 13, 0, 0), 4, 5, 5.9, 9.2));
            facts.Add(new Fact(new DateTime(2014, 1, 14, 18, 0, 0), 18, 6, 9.2, 14.0));

            facts.Add(new Fact(new DateTime(2014, 1, 15, 8, 0, 0), 4, 2, 7.8, 4.3));
            facts.Add(new Fact(new DateTime(2014, 1, 15, 13, 0, 0), 6, 5, 4.3, 9.4));
            facts.Add(new Fact(new DateTime(2014, 1, 15, 18, 0, 0), 18, 7, 9.4, 9.0));

            facts.Add(new Fact(new DateTime(2014, 1, 16, 8, 0, 0), 1, 3, 5.4, 6.5));
            facts.Add(new Fact(new DateTime(2014, 1, 16, 13, 0, 0), 4, 4, 6.5, 3.1));
            facts.Add(new Fact(new DateTime(2014, 1, 16, 18, 0, 0), 16, 8, 6.5, 5.5));

            facts.Add(new Fact(new DateTime(2014, 1, 17, 8, 0, 0), 4, 2, 8.4, 4.0));
            facts.Add(new Fact(new DateTime(2014, 1, 17, 13, 0, 0), 6, 5, 4.0, 9.5));
            facts.Add(new Fact(new DateTime(2014, 1, 17, 18, 0, 0), 18, 7, 9.5, 6.2));

            facts.Add(new Fact(new DateTime(2014, 1, 18, 9, 0, 0), 4, 3, 6.5, 4.6));
            facts.Add(new Fact(new DateTime(2014, 1, 18, 14, 0, 0), 8, 6, 4.6, 8.5));
            facts.Add(new Fact(new DateTime(2014, 1, 18, 18, 0, 0), 14, 8, 8.5, 9.0));

            facts.Add(new Fact(new DateTime(2014, 1, 19, 12, 0, 0), 8, 7, 7.1, 9.7));
            facts.Add(new Fact(new DateTime(2014, 1, 19, 15, 0, 0), 4, 4, 9.7, 7.6));
            facts.Add(new Fact(new DateTime(2014, 1, 19, 19, 0, 0), 18, 7, 7.6, 6.1));

            facts.Add(new Fact(new DateTime(2014, 1, 20, 8, 0, 0), 4, 3, 8.3, 8.7));
            facts.Add(new Fact(new DateTime(2014, 1, 20, 13, 0, 0), 6, 5, 8.7, 14.7));
            facts.Add(new Fact(new DateTime(2014, 1, 20, 18, 0, 0), 18, 9, 14.7, 9.8));

            facts.Add(new Fact(new DateTime(2014, 1, 21, 8, 0, 0, 0), 6, 2, 11.4, 5.9));
            facts.Add(new Fact(new DateTime(2014, 1, 21, 13, 0, 0, 0), 4, 4, 5.9, 11.7));
            facts.Add(new Fact(new DateTime(2014, 1, 21, 18, 0, 0, 0), 20, 8, 11.7, 10.7));

            facts.Add(new Fact(new DateTime(2014, 1, 22, 8, 0, 0), 2, 2, 6.9, 8.1));
            facts.Add(new Fact(new DateTime(2014, 1, 22, 13, 0, 0), 8, 7, 8.1, 10.2));
            facts.Add(new Fact(new DateTime(2014, 1, 22, 18, 0, 0), 18, 7, 10.2, 5.8));

            facts.Add(new Fact(new DateTime(2014, 1, 23, 8, 0, 0), 2, 2, 5.9, 7.2));
            facts.Add(new Fact(new DateTime(2014, 1, 23, 13, 0, 0), 4, 4, 7.2, 8.0));
            facts.Add(new Fact(new DateTime(2014, 1, 23, 18, 0, 0), 19, 8, 8.0, 14.1));
            p.Facts = facts;

            p = new Patient();
            p.Name = "Катрова Е.С.";
            p.Insulin = "Хумалог";
            p.Weight = 50;
            p.Growth = 175;
            p.DateOfBirth = new DateTime(1992, 8, 1);
            lstPatient.Add(p);
            facts = new List<Fact>();
            facts.Add(new Fact(new DateTime(2016, 11, 12, 9, 10, 0), 5, 5, 7.8, 7.3));
            facts.Add(new Fact(new DateTime(2016, 11, 12, 11, 50, 0), 2, 2, 7.3, 5.9));
            facts.Add(new Fact(new DateTime(2016, 11, 12, 14, 17, 0), 5, 5, 5.9, 8.9));
            facts.Add(new Fact(new DateTime(2016, 11, 12, 18, 25, 0), 4, 3, 8.9, 15.2));
            facts.Add(new Fact(new DateTime(2016, 11, 12, 21, 50, 0), 4, 0, 15.2, 11.6));

            facts.Add(new Fact(new DateTime(2016, 11, 13, 10, 35, 0), 7, 5, 10.6, 9.6));
            facts.Add(new Fact(new DateTime(2016, 11, 13, 14, 30, 0), 7, 5, 9.6, 10.2));
            facts.Add(new Fact(new DateTime(2016, 11, 13, 19, 00, 0), 7, 4.5, 10.2, 14.4));

            facts.Add(new Fact(new DateTime(2016, 11, 13, 23, 00, 0), 4, 0, 14.4, 9.6));

            facts.Add(new Fact(new DateTime(2016, 11, 14, 8, 25, 0), 6, 5.3, 9.6, 13.4));
            facts.Add(new Fact(new DateTime(2016, 11, 14, 14, 00, 0), 7, 4.5, 13.4, 11.7));
            facts.Add(new Fact(new DateTime(2016, 11, 14, 18, 14, 0), 8, 4, 11.7, 11.2));

            facts.Add(new Fact(new DateTime(2016, 11, 14, 23, 15, 0), 1, 0, 11.2, 8.4));

            facts.Add(new Fact(new DateTime(2016, 11, 15, 8, 40, 0), 6, 5, 8.7, 10.6));
            facts.Add(new Fact(new DateTime(2016, 11, 15, 13, 45, 0), 7, 4, 10.6, 11.9));
            facts.Add(new Fact(new DateTime(2016, 11, 15, 16, 15, 0), 2, 1.5, 11.9, 13.9));
            facts.Add(new Fact(new DateTime(2016, 11, 15, 19, 45, 0), 9, 5, 13.9, 10.1));

            facts.Add(new Fact(new DateTime(2016, 11, 15, 23, 15, 0), 1, 0, 10.1, 9.2));

            facts.Add(new Fact(new DateTime(2016, 11, 16, 9, 25, 0), 6, 5, 9.2, 8.7));
            facts.Add(new Fact(new DateTime(2016, 11, 16, 13, 42, 0), 6, 5, 8.7, 12.4));
            facts.Add(new Fact(new DateTime(2016, 11, 16, 19, 07, 0), 9, 5, 12.4, 10.5));

            facts.Add(new Fact(new DateTime(2016, 11, 16, 23, 10, 0), 1, 0, 10.7, 8.9));

            facts.Add(new Fact(new DateTime(2016, 11, 17, 9, 16, 0), 6, 5, 8.9, 4.6));
            facts.Add(new Fact(new DateTime(2016, 11, 17, 14, 20, 0), 7, 5, 10.0, 12.6));
            facts.Add(new Fact(new DateTime(2016, 11, 17, 19, 03, 0), 10, 5, 12.6, 13.4));
            facts.Add(new Fact(new DateTime(2016, 11, 17, 23, 10, 0), 2, 0, 13.4, 9.4));

            facts.Add(new Fact(new DateTime(2016, 11, 18, 10, 00, 0), 6, 5, 9.6, 7.6));
            facts.Add(new Fact(new DateTime(2016, 11, 18, 13, 47, 0), 5, 5, 7.6, 11.1));
            facts.Add(new Fact(new DateTime(2016, 11, 18, 19, 00, 0), 9, 5, 11.1, 12.4));
            facts.Add(new Fact(new DateTime(2016, 11, 18, 22, 50, 0), 2, 0, 12.4, 7.6));

            facts.Add(new Fact(new DateTime(2016, 11, 19, 10, 30, 0), 6, 5, 9.9, 7.2));
            facts.Add(new Fact(new DateTime(2016, 11, 19, 14, 06, 0), 5, 5, 7.2, 12.8));
            facts.Add(new Fact(new DateTime(2016, 11, 19, 19, 30, 0), 9, 5, 12.8, 11.7));

            facts.Add(new Fact(new DateTime(2016, 11, 19, 22, 50, 0), 2, 1, 11.7, 8.6));

            facts.Add(new Fact(new DateTime(2016, 11, 20, 10, 30, 0), 6, 5, 8.6, 8.4));
            facts.Add(new Fact(new DateTime(2016, 11, 20, 14, 00, 0), 5, 5, 8.4, 10.9));
            facts.Add(new Fact(new DateTime(2016, 11, 20, 19, 10, 0), 8, 6, 10.9, 9.7));

            facts.Add(new Fact(new DateTime(2016, 11, 21, 9, 45, 0), 5, 5, 7.9, 7.9));
            facts.Add(new Fact(new DateTime(2016, 11, 21, 14, 16, 0), 5, 6, 7.9, 13.2));
            facts.Add(new Fact(new DateTime(2016, 11, 21, 20, 09, 0), 9, 5, 13.2, 12.5));

            facts.Add(new Fact(new DateTime(2016, 11, 21, 23, 15, 0), 1, 0, 12.5, 10.7));

            facts.Add(new Fact(new DateTime(2016, 11, 22, 9, 46, 0), 6, 5, 10.7, 10.6));
            facts.Add(new Fact(new DateTime(2016, 11, 22, 13, 40, 0), 6, 5, 10.6, 11.7));
            facts.Add(new Fact(new DateTime(2016, 11, 22, 19, 00, 0), 8, 5, 11.7, 13.9));

            facts.Add(new Fact(new DateTime(2016, 11, 22, 23, 10, 0), 3, 0, 13.9, 8.8));

            facts.Add(new Fact(new DateTime(2016, 11, 23, 9, 46, 0), 6, 5, 8.8, 9.6));
            facts.Add(new Fact(new DateTime(2016, 11, 23, 13, 46, 0), 6, 5, 9.6, 10.1));

            facts.Add(new Fact(new DateTime(2016, 11, 23, 23, 10, 0), 2, 0, 11.1, 8.2));

            facts.Add(new Fact(new DateTime(2016, 11, 24, 10, 00, 0), 5, 5, 8.2, 4.7));
            facts.Add(new Fact(new DateTime(2016, 11, 24, 14, 20, 0), 4, 5, 4.7, 7.3));
            facts.Add(new Fact(new DateTime(2016, 11, 24, 19, 10, 0), 6, 6, 7.3, 10.7));

            facts.Add(new Fact(new DateTime(2016, 11, 25, 10, 0, 0), 5, 5, 9.1, 7.8));
            p.Facts = facts;


            p = new Patient();
            p.Name = "Андрейчук А.А.";
            p.Insulin = "Хумалог";
            p.Weight = 65;
            p.Growth = 178;
            p.DateOfBirth = new DateTime(1996, 5, 5);
            lstPatient.Add(p);
            facts = new List<Fact>();
            facts.Add(new Fact(new DateTime(2016, 11, 17, 9, 0, 0), 4, 4, 13.6, 0));
            //facts.Add(new Fact(new DateTime(2016, 17, 11, 14, 0, 0), 6, 6, 0, 3.7);
            facts.Add(new Fact(new DateTime(2016, 11, 17, 19, 0, 0), 10, 9, 3.7, 11.6));

            facts.Add(new Fact(new DateTime(2016, 11, 18, 7, 30, 0), 4, 5, 11.9, 9.8));
            facts.Add(new Fact(new DateTime(2016, 11, 18, 13, 10, 0), 7, 7, 9.8, 9.5));
            facts.Add(new Fact(new DateTime(2016, 11, 18, 19, 0, 0), 12, 10, 9.5, 14.2));
            facts.Add(new Fact(new DateTime(2016, 11, 18, 23, 0, 0), 2, 0, 14.2, 12.2));

            facts.Add(new Fact(new DateTime(2016, 11, 19, 11, 0, 0), 6, 6, 12.2, 10.7));
            facts.Add(new Fact(new DateTime(2016, 11, 19, 15, 0, 0), 16, 16, 10.7, 0));


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
            p.Facts = facts;


            p = new Patient();
            p.Name = "Острова Н.Н.";
            p.Insulin = "Хумалог";
            p.Weight = 68;
            p.Growth = 168;
            p.DateOfBirth = new DateTime(1967, 3, 1);
            lstPatient.Add(p);
            facts = new List<Fact>();
            //Add(Время(Год, Месяц, День, Час, Минута, Секунда), Инсулин, ХЕ, До, После);

            facts.Add(new Fact(new DateTime(2016, 10, 31, 8, 30, 0), 10, 4, 17.1, 17.5));
            facts.Add(new Fact(new DateTime(2016, 10, 31, 13, 0, 0), 12, 5, 17.5, 11.8));
            facts.Add(new Fact(new DateTime(2016, 10, 31, 17, 00, 0), 9, 5, 11.8, 10.3));
            facts.Add(new Fact(new DateTime(2016, 10, 31, 22, 30, 0), 0, 2, 10.3, 2.3));
            facts.Add(new Fact(new DateTime(2016, 11, 1, 2, 30, 0), 0, 3, 2.3, 10.2));

            facts.Add(new Fact(new DateTime(2016, 11, 1, 8, 30, 0), 6, 4, 10.2, 10.0));
            facts.Add(new Fact(new DateTime(2016, 11, 1, 13, 0, 0), 6, 4, 10.0, 15.7));
            facts.Add(new Fact(new DateTime(2016, 11, 1, 17, 30, 0), 9, 4, 15.7, 13.2));
            facts.Add(new Fact(new DateTime(2016, 11, 1, 23, 0, 0), 1, 0, 13.2, 3.5));
            facts.Add(new Fact(new DateTime(2016, 11, 2, 2, 0, 0), 0, 3, 3.5, 14.6));

            facts.Add(new Fact(new DateTime(2016, 11, 2, 8, 0, 0), 9, 4, 14.6, 14.1));
            facts.Add(new Fact(new DateTime(2016, 11, 2, 13, 30, 0), 8, 4, 14.1, 8.0));
            facts.Add(new Fact(new DateTime(2016, 11, 2, 17, 30, 0), 6, 5, 8.0, 12.1));
            facts.Add(new Fact(new DateTime(2016, 11, 2, 22, 0, 0), 1, 2, 12.1, 18.3));

            facts.Add(new Fact(new DateTime(2016, 11, 3, 8, 30, 0), 10, 4, 18.3, 11.4));
            facts.Add(new Fact(new DateTime(2016, 11, 3, 13, 0, 0), 7, 4, 11.4, 13.8));
            facts.Add(new Fact(new DateTime(2016, 11, 3, 17, 30, 0), 9, 5, 13.8, 11.8));
            //facts.Add(new Fact(new DateTime(2016, 11, 3, 22, 30, 0), 1, 0, 11.8, --));

            facts.Add(new Fact(new DateTime(2016, 11, 6, 9, 0, 0), 9, 4, 15.0, 10.3));
            facts.Add(new Fact(new DateTime(2016, 11, 6, 13, 0, 0), 9, 5, 10.3, 7.2));
            facts.Add(new Fact(new DateTime(2016, 11, 6, 17, 30, 0), 6, 5, 7.2, 16.1));
            facts.Add(new Fact(new DateTime(2016, 11, 6, 22, 0, 0), 2, 0, 16.1, 16.8));

            facts.Add(new Fact(new DateTime(2016, 11, 7, 8, 0, 0), 10, 4, 16.8, 12.0));
            facts.Add(new Fact(new DateTime(2016, 11, 7, 13, 0, 0), 8, 4, 12.0, 10.0));
            facts.Add(new Fact(new DateTime(2016, 11, 7, 18, 0, 0), 6, 4, 10.0, 15.5));
            facts.Add(new Fact(new DateTime(2016, 11, 7, 22, 0, 0), 3, 0, 15.5, 18.9));

            facts.Add(new Fact(new DateTime(2016, 11, 8, 8, 30, 0), 10, 4, 18.9, 6.8));
            facts.Add(new Fact(new DateTime(2016, 11, 8, 13, 0, 0), 6, 4, 6.8, 13.8));
            facts.Add(new Fact(new DateTime(2016, 11, 8, 17, 15, 0), 10, 5.5, 13.8, 12.5));
            facts.Add(new Fact(new DateTime(2016, 11, 8, 21, 0, 0), 2, 2, 12.5, 5.2));

            facts.Add(new Fact(new DateTime(2016, 11, 9, 6, 0, 0), 4, 4, 5.2, 13.4));
            facts.Add(new Fact(new DateTime(2016, 11, 9, 13, 0, 0), 8, 4, 13.4, 12.0));
            facts.Add(new Fact(new DateTime(2016, 11, 9, 17, 30, 0), 8, 4, 12.0, 4.1));
            facts.Add(new Fact(new DateTime(2016, 11, 9, 22, 0, 0), 0, 2, 4.1, 15.7));

            facts.Add(new Fact(new DateTime(2016, 11, 10, 8, 30, 0), 9, 4, 15.7, 10.2));
            facts.Add(new Fact(new DateTime(2016, 11, 10, 13, 0, 0), 6, 4, 10.2, 2.9));
            facts.Add(new Fact(new DateTime(2016, 11, 10, 17, 0, 0), 6, 6, 2.9, 5.3));
            facts.Add(new Fact(new DateTime(2016, 11, 10, 22, 0, 0), 0, 2, 5.3, 18.0));

            facts.Add(new Fact(new DateTime(2016, 11, 11, 8, 30, 0), 8, 4, 18.0, 11.1));
            facts.Add(new Fact(new DateTime(2016, 11, 11, 13, 0, 0), 8, 4, 11.1, 7.2));
            facts.Add(new Fact(new DateTime(2016, 11, 11, 17, 0, 0), 6, 5, 7.2, 12.3));
            facts.Add(new Fact(new DateTime(2016, 11, 11, 22, 0, 0), 2, 0, 12.3, 14.0));

            facts.Add(new Fact(new DateTime(2016, 11, 12, 8, 0, 0), 8, 4, 14.0, 5.1));//физ нагрузка
            facts.Add(new Fact(new DateTime(2016, 11, 12, 13, 30, 0), 5, 3, 5.1, 5.2));//физ нагрузка
            facts.Add(new Fact(new DateTime(2016, 11, 12, 17, 30, 0), 5, 3, 5.2, 10.4));
            facts.Add(new Fact(new DateTime(2016, 11, 12, 22, 0, 0), 1, 2, 10.4, 9.5));

            facts.Add(new Fact(new DateTime(2016, 11, 13, 9, 0, 0), 6, 4, 9.5, 11.3));
            facts.Add(new Fact(new DateTime(2016, 11, 13, 13, 0, 0), 8, 5, 11.3, 3.4));
            facts.Add(new Fact(new DateTime(2016, 11, 13, 17, 0, 0), 6, 5, 3.4, 13.6));
            facts.Add(new Fact(new DateTime(2016, 11, 13, 22, 0, 0), 2+2, 0, 13.6, 5.3));

            facts.Add(new Fact(new DateTime(2016, 11, 14, 8, 30, 0), 6, 4, 5.3, 4.4));
            facts.Add(new Fact(new DateTime(2016, 11, 14, 13, 0, 0), 5, 3, 4.4, 9.5));
            facts.Add(new Fact(new DateTime(2016, 11, 14, 17, 0, 0), 6, 4, 9.5, 7.8));
            facts.Add(new Fact(new DateTime(2016, 11, 14, 22, 0, 0), 0+2, 2, 7.8, 10.6));

            facts.Add(new Fact(new DateTime(2016, 11, 15, 8, 30, 0), 6, 4, 10.6, 7.9));
            facts.Add(new Fact(new DateTime(2016, 11, 15, 13, 30, 0), 9, 3, 7.9, 20.9));
            facts.Add(new Fact(new DateTime(2016, 11, 15, 17, 30, 0), 14, 4, 20.9, 8.9));
            facts.Add(new Fact(new DateTime(2016, 11, 15, 22, 0, 0), 0+2, 2, 8.9, 15.7));

            facts.Add(new Fact(new DateTime(2016, 11, 16, 8, 0, 0), 9, 4, 15.7, 9.4));
            facts.Add(new Fact(new DateTime(2016, 11, 16, 13, 0, 0), 6, 4, 9.4, 15.0));
            facts.Add(new Fact(new DateTime(2016, 11, 16, 17, 0, 0), 11, 5, 15.0, 9.2));
            facts.Add(new Fact(new DateTime(2016, 11, 16, 22, 0, 0), 0+2, 2, 9.2, 8.4));

            facts.Add(new Fact(new DateTime(2016, 11, 17, 8, 0, 0), 6, 4.5, 8.4, 4.9));
            facts.Add(new Fact(new DateTime(2016, 11, 17, 13, 0, 0), 6, 4, 4.9, 6.2));
            facts.Add(new Fact(new DateTime(2016, 11, 17, 17, 0, 0), 9, 6, 6.2, 15.2));
            facts.Add(new Fact(new DateTime(2016, 11, 17, 22, 0, 0), 2, 0, 15.2, 15.4));

            facts.Add(new Fact(new DateTime(2016, 11, 18, 8, 0, 0), 8, 4, 15.4, 5.4));
            facts.Add(new Fact(new DateTime(2016, 11, 18, 13, 0, 0), 6, 4, 5.4, 14.3));
            facts.Add(new Fact(new DateTime(2016, 11, 18, 17, 30, 0), 9, 4, 14.3, 14.5));
            facts.Add(new Fact(new DateTime(2016, 11, 18, 22, 0, 0), 2, 0, 14.5, 15.7));

            facts.Add(new Fact(new DateTime(2016, 11, 19, 9, 0, 0), 8, 4, 15.7, 11.5));
            facts.Add(new Fact(new DateTime(2016, 11, 19, 13, 30, 0), 8, 4, 11.5, 16.1));
            facts.Add(new Fact(new DateTime(2016, 11, 19, 17, 30, 0), 10, 4, 16.1, 8.1));
            facts.Add(new Fact(new DateTime(2016, 11, 19, 23, 0, 0), 0, 2, 8.1, 16.0));

            facts.Add(new Fact(new DateTime(2016, 11, 20, 9, 0, 0), 10, 4, 16.0, 4.3));
            facts.Add(new Fact(new DateTime(2016, 11, 20, 13, 0, 0), 7, 5, 4.3, 7.1));
            facts.Add(new Fact(new DateTime(2016, 11, 20, 17, 0, 0), 8, 5, 7.1, 6.5));
            facts.Add(new Fact(new DateTime(2016, 11, 20, 22, 30, 0), 0, 0, 6.5, 15.7));

            facts.Add(new Fact(new DateTime(2016, 11, 21, 9, 0, 0), 10, 4, 15.7, 8.5));
            facts.Add(new Fact(new DateTime(2016, 11, 21, 13, 0, 0), 6, 6, 8.5, 5.5));
            facts.Add(new Fact(new DateTime(2016, 11, 21, 17, 30, 0), 8, 6, 5.5, 6.9));
            //facts.Add(new Fact(new DateTime(2016, 11, 21, 22, 0, 0), 0, 0, 6.9, --));

            facts.Add(new Fact(new DateTime(2016, 11, 26, 8, 0, 0), 10, 4, 16.3, 6.6));
            facts.Add(new Fact(new DateTime(2016, 11, 26, 13, 0, 0), 6, 4, 6.6, 10.9));
            facts.Add(new Fact(new DateTime(2016, 11, 26, 17, 0, 0), 7, 4, 10.9, 16.5));
            facts.Add(new Fact(new DateTime(2016, 11, 26, 21, 0, 0), 4, 2, 16.5, 9.1));

            facts.Add(new Fact(new DateTime(2016, 11, 27, 9, 0, 0), 6, 4, 9.1, 11.1));
            facts.Add(new Fact(new DateTime(2016, 11, 27, 14, 0, 0), 7, 5, 11.1, 11.4));
            facts.Add(new Fact(new DateTime(2016, 11, 27, 18, 0, 0), 8, 4, 11.4, 6.4));
            //facts.Add(new Fact(new DateTime(2016, 11, 27, 23, 0, 0), 0, 2, 6.4, --));

            facts.Add(new Fact(new DateTime(2016, 12, 4, 9, 0, 0), 7, 4, 11.7, 13.7));
            facts.Add(new Fact(new DateTime(2016, 12, 4, 14, 0, 0), 10, 5, 13.7, 15.5));
            facts.Add(new Fact(new DateTime(2016, 12, 4, 17, 30, 0), 10, 5, 15.5, 6.1));
            facts.Add(new Fact(new DateTime(2016, 12, 4, 22, 0, 0), 0, 1, 6.1, 6.0));

            facts.Add(new Fact(new DateTime(2016, 12, 5, 8, 0, 0), 6, 5, 6.0, 12.7));
            facts.Add(new Fact(new DateTime(2016, 12, 5, 13, 0, 0), 11, 5, 12.7, 11.9));
            facts.Add(new Fact(new DateTime(2016, 12, 5, 17, 30, 0), 10, 4.5, 11.9, 5.3));
            facts.Add(new Fact(new DateTime(2016, 12, 5, 22, 0, 0), 0, 1, 5.3, 18.0));

            facts.Add(new Fact(new DateTime(2016, 12, 6, 8, 0, 0), 12, 5, 18.0, 9.0));
            facts.Add(new Fact(new DateTime(2016, 12, 6, 13, 0, 0), 8, 5, 9.0, 12.9));
            facts.Add(new Fact(new DateTime(2016, 12, 6, 17, 30, 0), 8, 4, 12.9, 8.1));
            facts.Add(new Fact(new DateTime(2016, 12, 6, 22, 0, 0), 0, 2, 8.1, 15.2));

            facts.Add(new Fact(new DateTime(2016, 12, 7, 8, 0, 0), 11, 5, 15.2, 4.9));
            facts.Add(new Fact(new DateTime(2016, 12, 7, 13, 0, 0), 6, 1, 4.9, 8.6));
            facts.Add(new Fact(new DateTime(2016, 12, 7, 17, 0, 0), 6, 4, 8.6, 12.7));
            //facts.Add(new Fact(new DateTime(2016, 12, 7, 21, 0, 0), 2, 1, 12.7, --));
            p.Facts = facts;


            p = new Patient();
            p.Name = "Калинина А.Е.";
            p.Insulin = "Новорапид";
            p.Weight = 63;
            p.Growth = 158;
            p.DateOfBirth = new DateTime(1991, 8, 8);
            lstPatient.Add(p);
            facts = new List<Fact>();
            //Add(Время(Год, Месяц, День, Час, Минута, Секунда), Инсулин, ХЕ, До, После);

            facts.Add(new Fact(new DateTime(2016, 12, 16, 23, 0, 0), 0, 0, 8.2, 4.8));

            facts.Add(new Fact(new DateTime(2016, 12, 17, 8, 0, 0), 10, 3, 4.8, 7.3));
            facts.Add(new Fact(new DateTime(2016, 12, 17, 13, 0, 0), 10, 5, 7.3, 6.3));
            facts.Add(new Fact(new DateTime(2016, 12, 17, 18, 0, 0), 10, 5, 6.3, 7.0));
            facts.Add(new Fact(new DateTime(2016, 12, 17, 23, 0, 0), 0, 0, 7.0, 5.1));

            facts.Add(new Fact(new DateTime(2016, 12, 19, 8, 0, 0), 0, 2, 2.8, 4.3));
            facts.Add(new Fact(new DateTime(2016, 12, 19, 23, 0, 0), 0, 0, 4.8, 3.4));

            facts.Add(new Fact(new DateTime(2016, 12, 21, 8, 0, 0), 10, 4, 3.4, 6.1));
            facts.Add(new Fact(new DateTime(2016, 12, 21, 23, 0, 0), 0, 0, 9.4, 2.8));

            facts.Add(new Fact(new DateTime(2016, 12, 22, 8, 0, 0), 4, 3, 2.8, 4.7));
            facts.Add(new Fact(new DateTime(2016, 12, 22, 13, 0, 0), 10, 4, 4.7, 6.2));

            facts.Add(new Fact(new DateTime(2016, 12, 23, 18, 0, 0), 10, 3, 15.2, 7.8));
            facts.Add(new Fact(new DateTime(2016, 12, 23, 23, 0, 0), 0, 0, 7.8, 4.5));

            facts.Add(new Fact(new DateTime(2016, 12, 25, 8, 0, 0), 10, 3, 4.9, 5.7));
            facts.Add(new Fact(new DateTime(2016, 12, 25, 23, 0, 0), 0, 0, 9.3, 4.1));

            facts.Add(new Fact(new DateTime(2016, 12, 26, 23, 0, 0), 0, 0, 21.2, 9.6));

            facts.Add(new Fact(new DateTime(2016, 12, 29, 8, 0, 0), 10, 4, 3.6, 6.4));
            facts.Add(new Fact(new DateTime(2016, 12, 29, 13, 0, 0), 10, 4, 6.4, 7.9));

            facts.Add(new Fact(new DateTime(2017, 1, 1, 18, 0, 0), 10, 4, 9.5, 8.6));
            facts.Add(new Fact(new DateTime(2017, 1, 1, 23, 0, 0), 0, 0, 8.6, 4.1));

            facts.Add(new Fact(new DateTime(2016, 1, 3, 10, 0, 0), 10, 3, 6.9, 4.8));
            facts.Add(new Fact(new DateTime(2016, 1, 3, 15, 0, 0), 10, 4, 4.8, 9.6));
            facts.Add(new Fact(new DateTime(2016, 1, 3, 23, 0, 0), 0, 0, 9.6, 4.8));

            facts.Add(new Fact(new DateTime(2016, 1, 4, 10, 0, 0), 10, 1, 14.8, 17.0));

            facts.Add(new Fact(new DateTime(2016, 1, 5, 8, 0, 0), 10, 1, 12.3, 6.4));

            facts.Add(new Fact(new DateTime(2016, 1, 7, 18, 0, 0), 10, 6, 8.0, 7.1));

            facts.Add(new Fact(new DateTime(2016, 1, 8, 8, 0, 0), 10, 2, 9.8, 5.1));
            p.Facts = facts;

            #endregion


        }

        public FormMain()
        {

            InitializeComponent();
            //AddData();
            OpenData(path);
            UpdateForm();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newDoseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewDose fm = new NewDose(currentPatient);
            fm.Show();
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormStatistics frm = new FormStatistics(currentPatient);
            frm.Show();
        }

        private void comboBoxPatient_SelectedIndexChanged(object sender, EventArgs e)
        {

            currentPatient = lstPatient.Where(x => x.Name == comboBoxPatient.SelectedItem.ToString()).First();
            linkLabelName.Text = currentPatient.Name;
            linkLabelWeight.Text = currentPatient.Weight.ToString();
            linkLabelAge.Text = currentPatient.Age.ToString();
            linkLabelInsulin.Text = currentPatient.Insulin;
            linkLabelGrowth.Text = currentPatient.Growth.ToString();

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenData();
            UpdateForm();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData(path);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
        }
    }
}
