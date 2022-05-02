using Guna.Charts.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;

namespace TKHCI.Finance
{
    internal class USD
    {
        private static readonly string monthQuery = "SELECT  MONTH(DATEADD(mm, -m, GETDATE())) AS monthCode, LEFT(DATENAME(mm,  DATEADD(mm, -m, GETDATE())), 3) AS monthName FROM (VALUES (0),(1),(2),(3),(4),(5)) t(m) ";
        public async static void DOLLARS(GunaChart chart)
        {
            SqlConnection con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
            DataTable ds = new DataTable();
            con.Open();
            SqlDataAdapter adapt = new SqlDataAdapter(monthQuery, con);
            adapt.Fill(ds);

            var dataset = new GunaSplineAreaDataset();
            dataset.Label = "USD";
            dataset.PointStyle = PointStyle.Circle;

            foreach (DataRow dr in ds.Rows)
            {
                SqlCommand command = new SqlCommand("usp_TotalAmountByMonth", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@monthCode", Convert.ToInt32(dr["monthCode"]));
                command.Parameters.AddWithValue("@currencyCode", 2);
                var amount = await command.ExecuteScalarAsync();
                dataset.DataPoints.Add(dr["monthName"].ToString(), Convert.ToDouble(amount));
            }
            con.Close();
            chart.Datasets.Add(dataset);
            chart.Update();
        }
    }
}
