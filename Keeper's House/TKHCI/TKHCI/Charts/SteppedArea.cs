﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKHCI.Charts
{
    internal class SteppedArea
    {
        public static void Example(Guna.Charts.WinForms.GunaChart chart)
        {
            string[] months = { "January", "February", "March", "April", "May", "June", "July" };

            //Chart configuration 
            chart.YAxes.GridLines.Display = false;

            //Create a new dataset 
            var dataset = new Guna.Charts.WinForms.GunaSteppedAreaDataset();
            var r = new Random();
            for (int i = 0; i < months.Length; i++)
            {
                //random number
                int num = r.Next(10, 100);

                dataset.DataPoints.Add(months[i], num);
            }
            dataset.PointRadius = 5;
            dataset.PointFillColors = Guna.Charts.WinForms.ChartUtils.Colors(months.Length, Color.OrangeRed);
            dataset.PointBorderColors = dataset.PointFillColors;

            //Add a new dataset to a chart.Datasets
            chart.Datasets.Add(dataset);

            //An update was made to re-render the chart
            chart.Update();
        }
    }
}
