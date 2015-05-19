namespace BeesAlgQAP
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.btnRun = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.iterationsUpDown = new System.Windows.Forms.NumericUpDown();
            this.eliteUpDown = new System.Windows.Forms.NumericUpDown();
            this.bestUpDown = new System.Windows.Forms.NumericUpDown();
            this.beesUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.finalLabel = new System.Windows.Forms.Label();
            this.firstLabel = new System.Windows.Forms.Label();
            this.improvementLabel = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.firstLbl = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iterationsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eliteUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bestUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beesUpDown)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(28, 207);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(116, 23);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run algorithm";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.iterationsUpDown);
            this.groupBox1.Controls.Add(this.eliteUpDown);
            this.groupBox1.Controls.Add(this.bestUpDown);
            this.groupBox1.Controls.Add(this.beesUpDown);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(28, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(656, 166);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "input";
            // 
            // iterationsUpDown
            // 
            this.iterationsUpDown.Location = new System.Drawing.Point(171, 121);
            this.iterationsUpDown.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.iterationsUpDown.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.iterationsUpDown.Name = "iterationsUpDown";
            this.iterationsUpDown.Size = new System.Drawing.Size(120, 20);
            this.iterationsUpDown.TabIndex = 7;
            this.iterationsUpDown.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.iterationsUpDown.ValueChanged += new System.EventHandler(this.iterationsUpDown_ValueChanged);
            // 
            // eliteUpDown
            // 
            this.eliteUpDown.Location = new System.Drawing.Point(171, 88);
            this.eliteUpDown.Name = "eliteUpDown";
            this.eliteUpDown.Size = new System.Drawing.Size(120, 20);
            this.eliteUpDown.TabIndex = 6;
            this.eliteUpDown.ValueChanged += new System.EventHandler(this.eliteUpDown_ValueChanged);
            // 
            // bestUpDown
            // 
            this.bestUpDown.Location = new System.Drawing.Point(171, 56);
            this.bestUpDown.Name = "bestUpDown";
            this.bestUpDown.Size = new System.Drawing.Size(120, 20);
            this.bestUpDown.TabIndex = 5;
            this.bestUpDown.ValueChanged += new System.EventHandler(this.bestUpDown_ValueChanged);
            // 
            // beesUpDown
            // 
            this.beesUpDown.Location = new System.Drawing.Point(171, 26);
            this.beesUpDown.Name = "beesUpDown";
            this.beesUpDown.Size = new System.Drawing.Size(120, 20);
            this.beesUpDown.TabIndex = 4;
            this.beesUpDown.ValueChanged += new System.EventHandler(this.beesUpDown_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Number of iterations:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Number of elite bees:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of best bees:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of bees:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.firstLbl);
            this.groupBox2.Controls.Add(this.finalLabel);
            this.groupBox2.Controls.Add(this.firstLabel);
            this.groupBox2.Controls.Add(this.improvementLabel);
            this.groupBox2.Controls.Add(this.chart1);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(28, 247);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(656, 342);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "output";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // finalLabel
            // 
            this.finalLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.finalLabel.Location = new System.Drawing.Point(130, 53);
            this.finalLabel.Name = "finalLabel";
            this.finalLabel.Size = new System.Drawing.Size(100, 23);
            this.finalLabel.TabIndex = 8;
            // 
            // firstLabel
            // 
            this.firstLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.firstLabel.Location = new System.Drawing.Point(130, 30);
            this.firstLabel.Name = "firstLabel";
            this.firstLabel.Size = new System.Drawing.Size(100, 23);
            this.firstLabel.TabIndex = 7;
            // 
            // improvementLabel
            // 
            this.improvementLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.improvementLabel.Location = new System.Drawing.Point(130, 76);
            this.improvementLabel.Name = "improvementLabel";
            this.improvementLabel.Size = new System.Drawing.Size(100, 23);
            this.improvementLabel.TabIndex = 6;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(10, 119);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(640, 217);
            this.chart1.TabIndex = 3;
            this.chart1.Text = "chart1";
            title1.Name = "Title1";
            title1.Text = "Fitness(iteration)";
            this.chart1.Titles.Add(title1);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Improvement:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Final solution fitness:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "First solution fitness:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // firstLbl
            // 
            this.firstLbl.AutoSize = true;
            this.firstLbl.Location = new System.Drawing.Point(153, 34);
            this.firstLbl.Name = "firstLbl";
            this.firstLbl.Size = new System.Drawing.Size(0, 13);
            this.firstLbl.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 601);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRun);
            this.Name = "Form1";
            this.Text = "BEES QAP";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iterationsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eliteUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bestUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beesUpDown)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown iterationsUpDown;
        private System.Windows.Forms.NumericUpDown eliteUpDown;
        private System.Windows.Forms.NumericUpDown bestUpDown;
        private System.Windows.Forms.NumericUpDown beesUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label finalLabel;
        private System.Windows.Forms.Label firstLabel;
        private System.Windows.Forms.Label improvementLabel;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label firstLbl;
    }
}