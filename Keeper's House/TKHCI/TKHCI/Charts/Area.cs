using Guna.Charts.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKHCI.Charts
{
    internal class Area
    {
        public static void Example(Guna.Charts.WinForms.GunaChart chart)
        {
            //Create a new dataset
            var dataset = new GunaAreaDataset
            {
                PointStyle = PointStyle.Circle
            };
            var r = new Random();
            dataset.DataPoints.Add("January", r.Next(10, 100));
            dataset.DataPoints.Add("February", r.Next(10, 100));
            dataset.DataPoints.Add("March", r.Next(10, 100));
            dataset.DataPoints.Add("April", r.Next(10, 100));
            //dataset.DataPoints.Add("May", r.Next(10, 100));
            //dataset.DataPoints.Add("June", r.Next(10, 100));
            //dataset.DataPoints.Add("July", r.Next(10, 100));

            //Add a new dataset to a chart.Datasets
            chart.Datasets.Add(dataset);

            //An update was made to re-render the chart  
            chart.Update();
        }
    }
}
