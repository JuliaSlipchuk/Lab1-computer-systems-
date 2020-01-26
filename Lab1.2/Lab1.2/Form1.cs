using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1._2
{
    public partial class Form1 : Form
    {
        static string filePath;
        static List<char> lettersInBase64;

        public Form1()
        {
            lettersInBase64 = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S'
                                            , 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm'
                                            , 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6'
                                            , '7', '8', '9', '+', '/' };
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
            if (richTextBox1.Text.Length == 0)
                MessageBox.Show("The text is missing");
            string strInBase64 = GetStrInBase64();
            richTextBox2.Text = strInBase64;
        }

        private string GetStrInBase64()
        {
            string strInBase64 = "";
            List<byte> textInBytes = System.Text.Encoding.UTF8.GetBytes(richTextBox1.Text).ToList();
            List<string> bitsRes;
            FillBitsArr(out bitsRes, textInBytes);
            ConvertBitsToLetters(out strInBase64, bitsRes);
            return strInBase64;
        }

        private void FillBitsArr(out List<string> bitsRes, List<byte> textInBytes)
        {
            while(textInBytes.Count % 3 != 0)
            {
                textInBytes.Add(0);
            }
            bitsRes = new List<string>();
            for (int i = 0; i < textInBytes.Count; i += 3)
            {
                for (int j = i; j < i + 3; j++)
                {
                    List<string> bits = new List<string>();
                    int[] arrBitsRev = new int[16];
                    int k;
                    for (k = 0; textInBytes[j] > 0; k++)
                    {
                        arrBitsRev[k] = textInBytes[j] % 2;
                        textInBytes[j] /= 2;
                    }
                    if (k < 8)
                    {
                        for (int p = 0; p < 8 - k; p++)
                        {
                            bits.Add("0");
                        }
                    }
                    for (k = k - 1; k >= 0; k--)
                    {
                        bits.Add(arrBitsRev[k].ToString());
                    }
                    bitsRes.AddRange(bits);
                }
            }
        }

        private void ConvertBitsToLetters(out string strInBase64, List<string> bitsRes)
        {
            strInBase64 = "";
            for (int i = 0; i < bitsRes.Count; i += 6)
            {
                int counter = 5;
                int res = 0;
                for (int j = i; j < i + 6; j++)
                {
                    if (bitsRes[j] == "1")
                    {
                        res += (int)Math.Pow(2, counter);
                    }
                    counter--;
                }
                strInBase64 += lettersInBase64[res];
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string normalText = "";
            if (richTextBox2.Text.Length == 0)
            {
                MessageBox.Show("The text is missing");
            }
            else
            {
                List<string> bits6 = new List<string>();
                for (int i = 0; i < richTextBox2.Text.Length; i++)
                {
                    bits6.Add(lettersInBase64.IndexOf(richTextBox2.Text[i]).ToString());
                }
                List<char> bits = new List<char>();
                List<char> sextets = new List<char>();
                for (int i = 0; i < bits6.Count; i++)
                {
                    sextets.Clear();
                    sextets.AddRange(Convert.ToString(Int32.Parse(bits6[i]), 2).ToCharArray());
                    if (sextets.Count < 6)
                    {
                        for (int j = 0; j < 6 - sextets.Count; j++)
                        {
                            bits.Add('0');
                        }
                    }
                    bits.AddRange(sextets);
                }
                byte[] bytes = ToByteArray(bits);
                normalText = Encoding.UTF8.GetString(bytes);
                richTextBox1.Text = normalText;
            }
        }

        private byte[] ToByteArray(List<char> bits)
        {
            int numBytes = bits.Count / 8;
            if (bits.Count % 8 != 0)
            {
                numBytes++;
            }
            byte[] bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0, pow = 7;
            for(int i = 0; i < bits.Count; i++)
            {
                if (bits[i] == '1')
                {
                    bytes[byteIndex] += (byte)Math.Pow(2, pow);
                }
                pow--;
                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                    pow = 7;
                }
            }
            return bytes;
        }
    }
}
