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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private float[] nums;
        private double[] nums1;
        private double[] nums2;
        private float T;
        private double t;
        private int k=0;

        public Form1()
        {
            InitializeComponent();
        }

        private void TBTemp_TextChanged(object sender, EventArgs e)
        {
            T = float.Parse(TBTemp.Text);
            T = 273 + T;
        }

        private void B1_Click(object sender, EventArgs e)
        {
            int c = int.Parse(TB1.Text);
            string[] SNums = TB2.Text.Split(' ');
            if (c < SNums.Length)
            {
                MessageBox.Show("Количество данных больше, чем в таблице");
                return;
            }
            if (c > SNums.Length)
            {
                MessageBox.Show("Количество данных меньше, чем в таблице");
                TB1.ReadOnly = false;
                TB2.ReadOnly = false;
                return;
            }
            nums = new float[c];
            for (int i = 0; i < c; i++)
            {
                nums[i] = float.Parse(SNums[i]);
                if (nums[i]>=100)
                {
                    MessageBox.Show("Концентрация NH3 не может быть больше либо равной 100%! Поменяйте концентрацию!");
                    TB2.ReadOnly = false;
                    return;
                }
                if (nums[i] >= 31)
                {
                    MessageBox.Show("Превышена придельная концентрация NH3 в системе (31.09%)! Поменяйте концентрацию!");
                    return;
                }
            }
             t = Convert.ToDouble((589.9 / T) - 3.205);
            nums1 = new double[c];
            for (int i = 0; i < nums.Length; i++)
            {
               nums1[i] = Convert.ToDouble ((3.05 - (631.5 / T)) + 0.4343 * nums[i] * Math.Pow(10, t));
                nums1[i] = Math.Pow(10, nums1[i]);
            }
            nums2 = new double[c];
            for (int i = 0; i < nums.Length; i++)
            {
                nums2[i] = Convert.ToDouble(100 - (nums[i] + nums1[i]));
                if (nums2[i]<0)
                {
                    MessageBox.Show("Задана слишком высокая температура! Поменяйте температуру!");
                    return;
                }
            }
            RTB1.Text += ("\n" + "Температура процесса в Кельвинах: " + T.ToString() + "\n\n");
            RTB1.Text += ("NH3" + "\t" + "CO2" + "\t" + "H2O" + "\n");
            for (int i = 0; i < nums.Length; i++)
            {
                RTB1.Text += (nums[i].ToString("#.###") + "\t");
                RTB1.Text += (nums1[i].ToString("#.###") + "\t");
                RTB1.Text += (nums2[i].ToString("#.###") + "\n");
            }
            this.tabControl1.SelectedTab = TP2;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void вставитьДатуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RTB1.AppendText("\n"+DateTime.Now.ToShortDateString());
        }

        private void вставитьВремяToolStripMenuItem_Click(object sender, EventArgs e)
        {
           RTB1.AppendText("\t"+DateTime.Now.ToShortTimeString() + "\n");
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.DefaultExt = "*.rez";
            saveFile1.Filter = "Text files|*.rez";
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                saveFile1.FileName.Length > 0)
            {
                using (StreamWriter sw = new StreamWriter(saveFile1.FileName, true))
                {
                    sw.WriteLine(RTB1.Text);
                    sw.Close();
                }
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Текстовые файлы|*.rez";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    RTB1.Text = File.ReadAllText(dialog.FileName);
                }
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа для расчета концентрации углекислого газа и паров воды" + "\n" + "при известной концентрации аммиака. В процессе синтеза" + "\n" + "углеаммонийных солей. Построения графика в координатах CO2-NH3" + "\n\n" + "Автор: Игорь Мукминов: fatalrew@gmail.com");
        }


        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить картинку как...";
            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.Filter = "Image Files(*.JPG)|*.JPG";
            savedialog.ShowHelp = true;
            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Ch1.SaveImage(savedialog.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Jpeg);
                }
                catch
                {
                    MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void B5_Click(object sender, EventArgs e)
        {
            TB1.ReadOnly = true;
            TB2.ReadOnly = false;
        }

        private void B4_Click(object sender, EventArgs e)
        {
            TB2.ReadOnly = true;
            TBTemp.ReadOnly = false;
        }

        private void B6_Click(object sender, EventArgs e)
        {
            TBTemp.ReadOnly = true;
        }

        private void B7_Click(object sender, EventArgs e)
        {
            RTB1.Clear();
        }

        private void B8_Click(object sender, EventArgs e)
        {
            string s = CMB1.Text;
            switch (s)
            {
                case "Данных в таблице":
                    {
                        TB1.Clear();
                        TB1.ReadOnly = false;
                    }
                    break;
                case "Концентрацию NH3":
                    {
                        TB2.Clear();
                        TB2.ReadOnly = false;
                    }
                    break;
                case "Температуру процесса":
                    {
                        TBTemp.Clear();
                        TBTemp.ReadOnly = false;
                    }
                    break;
            }
        }

        private void B9_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                Ch1.Series[i].Points.Clear();
            }
        }

        private void B10_Click(object sender, EventArgs e)
        {
            if (k == 11)
            {
                MessageBox.Show("Превышено количество серий на графике!!! Максимально возможно до десяти серий на одном графике.");
            }
            else
            {

                for (int i = 0; i < nums.Length; i++)
                {
                    Ch1.Series[k].Points.AddXY(nums[i], nums1[i]);
                }
                k++;
            }

            Ch1.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
            Ch1.ChartAreas[0].CursorX.IsUserEnabled = true;
            Ch1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            Ch1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            Ch1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            Ch1.ChartAreas[0].AxisY.ScaleView.Zoom(0, 31);
            Ch1.ChartAreas[0].CursorY.IsUserEnabled = true;
            Ch1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            Ch1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            Ch1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;

            this.tabControl1.SelectedTab = TP3;
        }
    }
}
