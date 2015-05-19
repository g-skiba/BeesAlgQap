using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BeesAlgQAP
{
    public partial class Form1 : Form
    {

        private CallbackFields callbacks;
        public Form1()
        {
            InitializeComponent();

            beesUpDown.Value = BeesAlgorithm.getBees();
            bestUpDown.Value = BeesAlgorithm.getBest();
            eliteUpDown.Value = BeesAlgorithm.getElite();
            iterationsUpDown.Value = BeesAlgorithm.getIterations();

            beesUpDown.Minimum = bestUpDown.Value;
            bestUpDown.Maximum = beesUpDown.Value;
            bestUpDown.Minimum = eliteUpDown.Value;
            eliteUpDown.Maximum = bestUpDown.Value;


            chart1.Series.Clear();
            
            callbacks = new CallbackFields(firstLabel, finalLabel, improvementLabel, chart1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BeesAlgorithm.perform(callbacks);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void beesUpDown_ValueChanged(object sender, EventArgs e)
        {
            bestUpDown.Maximum = beesUpDown.Value;
            BeesAlgorithm.setBees(Convert.ToInt32(beesUpDown.Value));
        }

        private void bestUpDown_ValueChanged(object sender, EventArgs e)
        {
            beesUpDown.Minimum = bestUpDown.Value;
            eliteUpDown.Maximum = bestUpDown.Value;
            BeesAlgorithm.setBest(Convert.ToInt32(bestUpDown.Value));
        }

        private void eliteUpDown_ValueChanged(object sender, EventArgs e)
        {
            bestUpDown.Minimum = eliteUpDown.Value;
            BeesAlgorithm.setIElite(Convert.ToInt32(eliteUpDown.Value));
        }

        private void iterationsUpDown_ValueChanged(object sender, EventArgs e)
        {
            BeesAlgorithm.setIterations(Convert.ToInt32(iterationsUpDown.Value));
        }
    }
}
