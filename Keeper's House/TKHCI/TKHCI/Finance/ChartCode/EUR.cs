﻿using Guna.Charts.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;

namespace TKHCI.Finance
{
    internal class EUR
    {
        private static readonly string monthQuery = "SELECT  MONTH(DATEADD(mm, -m, GETDATE())) AS monthCode, LEFT(DATENAME(mm,  DATEADD(mm, -m, GETDATE())), 3) AS monthName FROM (VALUES (0),(1),(2),(3),(4),(5)) t(m) ";
        public async static void EUROS(GunaChart chart)
        {
            SqlConnection con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
            DataTable ds = new DataTable();
            con.Open();
            SqlDataAdapter adapt = new SqlDataAdapter(monthQuery, con);
            adapt.Fill(ds);

            var dataset = new GunaSplineAreaDataset();
            dataset.Label = "EUR";

            dataset.PointStyle = PointStyle.Circle;

            foreach (DataRow dr in ds.Rows)
            {
                SqlCommand command = new SqlCommand("usp_TotalAmountByMonth", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@monthCode", Convert.ToInt32(dr["monthCode"]));
                command.Parameters.AddWithValue("@currencyCode", 4);
                var amount = await command.ExecuteScalarAsync();
                dataset.DataPoints.Add(dr["monthName"].ToString(), Convert.ToDouble(amount));
            }
            con.Close();

            //Add a new dataset to a chart.Datasets
            chart.Datasets.Add(dataset);

            //An update was made to re-render the chart  
            chart.Update();
        }
    }
}
