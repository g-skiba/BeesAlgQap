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
        private Label first, final, improvement;
        private Chart chart;

        public CallbackFields(Label first, Label final, Label improvement, Chart chart)
        {
            this.first = first;
            this.final = final;
            this.improvement = improvement;
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

        public void setIMprovementValue(double n)
        {
            improvement.Text = n.ToString("0.00") + " %";
        }

        public void setDatapoints(double[] array)
        {
            chart.Series.Clear();
            chart.Series.Add("Series1");
            chart.Series["Series1"].ChartType = SeriesChartType.Spline;
            chart.Series["Series1"].Color = Color.Red;

            for(int i = 0 ; i < array.Length ; i++)
            {
                chart.Series["Series1"].Points.AddXY(Convert.ToDouble(i + 1), array[i]);
            }
     
            chart.Update();
        }
    }
}
