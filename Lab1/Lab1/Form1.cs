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

namespace Lab1
{
    public partial class Form1 : Form
    {
        static string filePath;
        int countOfLetters = 0;
        List<char> alphavit = new List<char>() { 'а', 'б', 'в', 'г', 'д', 'е', 'є', 'ж', 'з', 'и', 'і', 'ї', 'й', 'к', 'л',
            'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я' };
        Dictionary<char, float> frequency;

        public Form1()
        {
            FillDictionary();

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
                using (StreamReader reader = new StreamReader(filePath, Encoding.Default, true))
                {
                    richTextBox1.Clear();
                    richTextBox1.Text = reader.ReadToEnd();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength == 0)
                MessageBox.Show("Text is missing");
            countOfLetters = richTextBox1.Text.Count(c => char.IsLetter(c));
            for (int i = 0; i < richTextBox1.Text.Length; i++)
            {
                if (frequency.ContainsKey(Char.ToLower(richTextBox1.Text[i])))
                {
                    frequency[Char.ToLower(richTextBox1.Text[i])] += 1;
                }
            }
            foreach(char item in alphavit)
            {
                frequency[item] /= countOfLetters;
            }
            FillChart();
            label2.Text = "The entropy is " + Math.Round(GetEntropy(), 4);
            label3.Text = "The amount of information is " + Math.Round(GetAmountOfInform(GetEntropy()), 4);
        }

        private void FillChart()
        {
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.Series["Frequency"].Points.Clear();
            foreach (char item in alphavit)
            {
                chart1.Series["Frequency"].Points.AddXY(item.ToString(), frequency[item]);
            }
        }

        private double GetEntropy()
        {
            double entropy = 0;
            foreach(char item in frequency.Keys)
            {
                if (frequency[item] == 0)
                    continue;
                entropy += frequency[item] * Math.Log(frequency[item], 2);
            }
            return (-entropy);
        }

        private double GetAmountOfInform(double entropy)
        {
            return entropy * countOfLetters;
        }

        private void FillDictionary()
        {
            frequency = new Dictionary<char, float>();
            for (int i = 0; i < alphavit.Count; i++)
            {
                frequency.Add(alphavit[i], 0);
            }
        }
    }
}
