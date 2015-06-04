using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BeesAlgQAP
{
    class CallbackFields
    {
        private Label first, final, improvement, reference, error;
        private Chart chart;

        public CallbackFields(Label first, Label final, Label improvement, Label reference, Label error, Chart chart)
        {
            this.first = first;
            this.final = final;
            this.improvement = improvement;
            this.reference = reference;
            this.error = error;
            this.chart = chart;
        }

        public void setFirstValue(double n)
        {
            first.Text = Convert.ToString(n);
        }

        public void setFinalValue(double n)
        {
            final.Text = Convert.ToString(n);
        }

        public void setImprovementValue(double n)
        {
            improvement.Text = n.ToString("0.00") + " %";
        }

        public void setReferenceSolution(double n)
        {
            reference.Text = Convert.ToString(n);
        }

        public void setError(double n)
        {
            error.Text = n.ToString("0.00") + " %";
        }

        public void setDatapoints(double[] bestSolution, double[] maxOfIteration)
        {
            chart.Series.Clear();
            chart.Series.Add("Series1");
            chart.Series["Series1"].ChartType = SeriesChartType.Spline;
            chart.Series["Series1"].Color = Color.Red;

            chart.Series.Add("Series2");
            chart.Series["Series2"].ChartType = SeriesChartType.Spline;
            chart.Series["Series2"].Color = Color.Blue;

            for (int i = 0; i < bestSolution.Length; i++)
            {
                chart.Series["Series1"].Points.AddXY(Convert.ToDouble(i + 1), bestSolution[i]);
                chart.Series["Series2"].Points.AddXY(Convert.ToDouble(i + 1), maxOfIteration[i]);
            }

            chart.ChartAreas[0].AxisY.Minimum = bestSolution[bestSolution.Length - 1] * 0.99;
            chart.ChartAreas[0].AxisY.Maximum = bestSolution[0] * 1.01;
            chart.Update();
        }
    }
}
