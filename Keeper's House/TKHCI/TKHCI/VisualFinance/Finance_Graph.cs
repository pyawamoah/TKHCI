using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TKHCI.VisualFinance
{
    public partial class Finance_Graph : Form
    {
        private string selected = string.Empty;
        public Finance_Graph()
        {
            InitializeComponent();
        }

        private void Finance_Graph_Load(object sender, EventArgs e)
        {
            string[] names = { "Area", "Bar", "Bubble", "Doughnut", "HorizontalBar", "Line", "Pie", "PolarArea", "Radar", 
                "RoundedBar", "Scatter", "Spline", "SplineArea", "StackedBar", "StackedHorizontalBar", "SteppedArea", 
                 "SteppedLine", "MixedBarAndArea", "MixedBarAndLine", "MixedBarAndSpline", "MixedBarAndSplineArea", "MixedBarAndSteppedArea", "MixedBarAndSteppedLine" };
            panel1.BackColor = Color.FromArgb(20, 0, 0, 0);
            for (int i = names.Length - 1; i > -1; i--)
            {
                var button = new Button()
                {
                    BackColor = Color.Empty,
                    Text = names[i],
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Top,
                    Height = 25,
                    FlatStyle = FlatStyle.Flat,
                    Padding = new Padding(10, 0, 0, 0),
                    FlatAppearance =
                    {
                         BorderSize = 0,
                         MouseOverBackColor = Color.DodgerBlue
                    }
                };
                button.Click += (s, evnt) =>
                {
                    selected = button.Text;
                    SelectBasicExamples(selected);
                    panel2.Size = new Size(5, button.Height);
                    panel2.Location = new Point(panel1.Width - 5, button.Top);
                    panel2.BringToFront();
                };
                panel1.Controls.Add(button);
            }
            radioButton1.CheckedChanged += (s, evnt) =>
            {
                if (radioButton1.Checked)
                {
                    SelectBasicExamples(selected);
                    BackColor = Color.FromArgb(38, 41, 59);
                    ForeColor = Color.Black; 
                    guna2ControlBox2.FillColor = Color.FromArgb(38, 41, 59);
                    guna2ControlBox1.FillColor = Color.FromArgb(38, 41, 59);
                }
            };
            radioButton2.CheckedChanged += (s, evnt) =>
            {
                if (radioButton2.Checked)
                {
                    SelectBasicExamples(selected);
                    BackColor = Color.FromArgb(0, 0, 64);
                    ForeColor = Color.Black;
                    guna2ControlBox2.FillColor = Color.FromArgb(0, 0, 64);
                    guna2ControlBox1.FillColor = Color.FromArgb(0, 0, 64);
                }
            };
            radioButton1.Checked = true;

        }
        private void ApplyConfig()
        {
            if (radioButton1.Checked)
                gunaChart1.ApplyConfig(ConfigBK_Change.Dark.Config(), Color.FromArgb(38, 41, 59));
            else
                gunaChart1.ApplyConfig(ConfigBK_Change.Light.Config(), Color.FromArgb(0, 0, 64));
        }
        private void SelectBasicExamples(string name)
        {
            gunaChart1.Datasets.Clear();
            ApplyConfig();

            if (name == "Area")
                Charts.Area.Example(gunaChart1);
            else if (name == "Bar")
                Charts.Bar.Example(gunaChart1);
            else if (name == "RoundedBar")
                Charts.RoundedBar.Example(gunaChart1);
            else if (name == "Bubble")
                Charts.Bubble.Example(gunaChart1);
            else if (name == "Doughnut")
                Charts.Doughnut.Example(gunaChart1);
            else if (name == "HorizontalBar")
                Charts.HorizontalBar.Example(gunaChart1);
            else if (name == "Line")
                Charts.Line.Example(gunaChart1);
            else if (name == "Pie")
                Charts.Pie.Example(gunaChart1);
            else if (name == "PolarArea")
                Charts.PolarArea.Example(gunaChart1);
            else if (name == "Radar")
                Charts.Radar.Example(gunaChart1);
            else if (name == "Scatter")
                Charts.Scatter.Example(gunaChart1);
            else if (name == "Spline")
                Charts.Spline.Example(gunaChart1);
            else if (name == "SplineArea")
                Charts.SplineArea.Example(gunaChart1);
            else if (name == "StackedBar")
                Charts.StackedBar.Example(gunaChart1);
            else if (name == "StackedHorizontalBar")
                Charts.StackedHorizontalBar.Example(gunaChart1);
            else if (name == "SteppedArea")
                Charts.SteppedArea.Example(gunaChart1);
            else if (name == "SteppedLine")
                Charts.SteppedLine.Example(gunaChart1);
            else if (name == "MixedBarAndArea")
                Charts.MixedBarAndArea.Example(gunaChart1);
            else if (name == "MixedBarAndSpline")
                Charts.MixedBarAndSpline.Example(gunaChart1);
            else if (name == "MixedBarAndSplineArea")
                Charts.MixedBarAndSplineArea.Example(gunaChart1);
            else if (name == "MixedBarAndSteppedArea")
                Charts.MixedBarAndSteppedArea.Example(gunaChart1);
            else if (name == "MixedBarAndSteppedLine")
                Charts.MixedBarAndSteppedLine.Example(gunaChart1);
            else if (name == "MixedBarAndLine")
                Charts.MixedBarAndLine.Example(gunaChart1);

        }


    }
}
