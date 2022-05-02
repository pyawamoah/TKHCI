using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TKHCI.Finance
{
    public partial class Wallet_Info : Form
    {
        public Wallet_Info()
        {
            InitializeComponent();          
            _ = DefualtLoad();
        }
        #region SQL CONNECTIONS
        #region TASK MASTER 
        public async Task DefualtLoad()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(GHS_Display());
            tasks.Add(fillChart());
            await Task.WhenAll(tasks);
        }
        #endregion
        private Task GHS_Display()
        {
            SqlConnection connection = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
            connection.Open();
            DataTable ds = new DataTable();
            SqlDataAdapter adapt = new SqlDataAdapter("[usp_GL_Master_Balance]", connection);
            adapt.Fill(ds);
            connection.Close();
            foreach (DataRow dr in ds.Rows)
            {

                Tx_GHS_Cur_Bal.Text = $"¢{String.Format("{0:#,##0.00}", double.Parse($"{dr["GHS_Bal"]}"))}";
                Tx_USD_Cur_Bal.Text = $"${String.Format("{0:#,##0.00}", double.Parse($"{dr["USD_Bal"]}"))}";
                Tx_GBP_Cur_Bal.Text = $"£{String.Format("{0:#,##0.00}", double.Parse($"{dr["GBP_Bal"]}"))}";
                Tx_EUR_Cur_Bal.Text = $"€{String.Format("{0:#,##0.00}", double.Parse($"{dr["EUR_Bal"]}"))}";

                Tx_GHS_Exp_Bal.Text = $"¢{String.Format("{0:#,##0.00}", double.Parse($"{dr["GHS_Exp"]}"))}";
                Tx_USD_Exp_Bal.Text = $"${String.Format("{0:#,##0.00}", double.Parse($"{dr["USD_Exp"]}"))}";
                Tx_GBP_Exp_Bal.Text = $"£{String.Format("{0:#,##0.00}", double.Parse($"{dr["GBP_Exp"]}"))}";
                Tx_EUR_Exp_Bal.Text = $"€{String.Format("{0:#,##0.00}", double.Parse($"{dr["EUR_Exp"]}"))}";

            }
            return Task.CompletedTask;
        }
        #endregion SQL CONNECTIONS
        #region TOGGLE COLORS
        private void Color_Toggle_CheckedChanged(object sender, EventArgs e)
        {
            if (Color_Toggle.Checked)
            {
                Panel_Expense_Cedis.ShadowDecoration.Color = Color.FromArgb(0, 123, 85);
                Panel_Expense_Dollars.ShadowDecoration.Color = Color.FromArgb(248, 25, 14);
                Panel_Expense_Euros.ShadowDecoration.Color = Color.FromArgb(94, 148, 255);

                Panel_GHS_Total_Bal.ShadowDecoration.Color = Color.FromArgb(0, 123, 85);
                Panel_USD_Total_Bal.ShadowDecoration.Color = Color.FromArgb(0, 123, 85);
                Panel_GBP_Total_Bal.ShadowDecoration.Color = Color.FromArgb(0, 123, 85);
                Panel_EUR_Total_Bal.ShadowDecoration.Color = Color.FromArgb(0, 123, 85);




                BackGround_Mat.FillColor = Color.FromArgb (0,0,64);
                BackGround_Mat.FillColor2 = Color.FromArgb(0,0,64);
                BackGround_Mat.FillColor3 = Color.FromArgb(0,0,64);
                BackGround_Mat.FillColor4 = Color.FromArgb(0,0,64);
                BackGround_Mat.FillColor4 = Color.FromArgb(0,0,64);
                WalletInfo_Tab.BackColor = Color.FromArgb (10, 10, 10);
            }
            else
            {
                Panel_Expense_Cedis.ShadowDecoration.Color = Color.FromArgb(255, 211, 53);
                Panel_Expense_Dollars.ShadowDecoration.Color = Color.FromArgb(255, 255, 255);
                Panel_Expense_Euros.ShadowDecoration.Color = Color.FromArgb(255, 211, 53);

                BackGround_Mat.FillColor = Color.FromArgb (10, 10, 10);
                BackGround_Mat.FillColor2 = Color.FromArgb(10, 10, 10);
                BackGround_Mat.FillColor3 = Color.FromArgb(10, 10, 10);
                BackGround_Mat.FillColor4 = Color.FromArgb(10, 10, 10);
                WalletInfo_Tab.BackColor = Color.FromArgb(255, 255, 255);
            }
        }
        #endregion
        #region SHOW CURRENCY
        private void CheckBX_GHS_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBX_GHS.Checked)
            {
                string a = Tx_GHS_Cur_Bal.Text;
                Tx_GHS_Cur_Bal.PasswordChar = '\0';
            }
            else
            {
                Tx_GHS_Cur_Bal.PasswordChar = '*';
            }
        }

        private void CheckBX_USD_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBX_USD.Checked)
            {
                string a = Tx_USD_Cur_Bal.Text;
                Tx_USD_Cur_Bal.PasswordChar = '\0';
            }
            else
            {
                Tx_USD_Cur_Bal.PasswordChar = '*';
            }
        }

        private void CheckBX_GBP_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBX_GBP.Checked)
            {
                string a = Tx_GBP_Cur_Bal.Text;
                Tx_GBP_Cur_Bal.PasswordChar = '\0';
            }
            else
            {
                Tx_GBP_Cur_Bal.PasswordChar = '*';
            }
        }

        private void CheckBX_EUR_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBX_EUR.Checked)
            {
                string a = Tx_EUR_Cur_Bal.Text;
                Tx_EUR_Cur_Bal.PasswordChar = '\0';
            }
            else
            {
                Tx_EUR_Cur_Bal.PasswordChar = '*';
            }
        }
        #endregion
        #region LOAD GRAPH CHARTS
        private Task fillChart()
        {
            GHS.CEDIS(Chart_GHS);
            USD.DOLLARS(Chart_USD);
            GBP.POUNDS(Chart_GBP);
            EUR.EUROS(Chart_EUR);
            return Task.CompletedTask;
        }
        #endregion
        #region FORM LOAD
        private void Finance_Report_Load(object sender, EventArgs e)
        {
            fillChart();
        }
        #endregion
    }
}
