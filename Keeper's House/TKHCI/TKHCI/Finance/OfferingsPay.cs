using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TKHCI.Finance
{
    public partial class OfferingsPay : Form
    {
        #region START
        KeeperDBEntities DBS = new KeeperDBEntities();

        string paychannel;
        int curID;
        string paychannel2;
        string paychannel3;
        string curType;
        string curType2;
        string curType3;
        string isMember;
        decimal zero = 0;
        decimal amtNOW;
        decimal prevBal;
        decimal totalBal;
        decimal realTotal = 0;
        decimal grandTotalN;
        decimal quantity1;
        decimal price1 = 100;
        decimal of100 = 0;
        decimal quantity2;
        decimal price2 = 50;
        decimal of50 = 0;
        decimal quantity3;
        decimal price3 = 20;
        decimal of20 = 0;
        decimal quantity4;
        decimal price4 = 10;
        decimal of10 = 0;
        decimal quantity5;
        decimal price5 = 5;
        decimal of5 = 0;
        decimal quantity6;
        decimal price6 = 1;
        decimal of1 = 0;
        string curSymbol;
        string typeofService;
        public static class serviceType
        {
            public static string stype;
        }
        public static class Pledge
        {
            public static string pledge;
        }
        public static class tithePay
        {
            public static string tithe;
        }
        public static class FF
        {
            public static string first;
        }
        decimal Disposal;
        #endregion
        public OfferingsPay()
        {
            InitializeComponent();
            _ = DefualtLoad();
        }
        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
#region TASK MASTER FOR ID's LOAD STARTS
        public async Task DefualtLoad()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(Keepers_Load_Tithe());
            tasks.Add(Keepers_Load_First_Fruit());
            tasks.Add(Keepers_Load_Pledge());
            tasks.Add(Keepers_Load_ServicecO());

            //tasks.Add(Keepers_Load_Pledge());
            await Task.WhenAll(tasks);
        }

        private Task Keepers_Load_ServicecO()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_ServiceO_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal) command.ExecuteScalar();
            Con.Close();

            if (xml < 1)
            {
                Disposal = 1;
                Tx_RO_ID.Text =
                    $"KH-SO-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            else if (xml >= 1)
            {
                Disposal = xml + 1;
                Tx_RO_ID.Text =
                    $"KH-SO-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            return Task.CompletedTask;
        }

        private Task Keepers_Load_Pledge()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_Tithe_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal) command.ExecuteScalar();
            Con.Close();

            if (xml < 1)
            {
                Disposal = 1;
                Tx_Pledge_ID.Text =
                    $"KH-PP-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            else if (xml >= 1)
            {
                Disposal = xml + 1;
                Tx_Pledge_ID.Text =
                    $"KH-PP-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            return Task.CompletedTask;
        }

        private Task Keepers_Load_Tithe()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_Tithe_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal) command.ExecuteScalar();
            Con.Close();

            if (xml < 1)
            {
                Disposal = 1;
                Tx_Tithe_No.Text =
                    $"KH-TI-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            else if (xml >= 1)
            {
                Disposal = xml + 1;
                Tx_Tithe_No.Text =
                    $"KH-TI-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            return Task.CompletedTask;
        }

        private Task Keepers_Load_First_Fruit()
        {
            Con.Open();
            SqlCommand command =
                new SqlCommand("[usp_First_Fruit_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal) command.ExecuteScalar();
            Con.Close();

            if (xml < 1)
            {
                Disposal = 1;
                Tx_FF_No.Text =
                    $"KH-FF-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            else if (xml >= 1)
            {
                Disposal = xml + 1;
                Tx_FF_No.Text =
                    $"KH-FF-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            return Task.CompletedTask;
        }
#endregion TASK MASTER FOR LOAD ID's ENDS HERE..............
#region TASK POPULATING FORMS STARTS HERE
        public object PopulateData_GeneralPay()
        {
            DataTable dt = new DataTable();
            string connection =
                @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command =
                new SqlCommand("[usp_Load_And_Search_Member]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command
                .Parameters
                .AddWithValue("@searchText", txtSearch_By_NamePay.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill (dt);
            return dt;
        }

        public object PopulateData_Tithe()
        {
            DataTable dt = new DataTable();
            string connection =
                @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command =
                new SqlCommand("[usp_Load_and_Search_TithePay]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command
                .Parameters
                .AddWithValue("@searchText", txtSearch_By_NamePay.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill (dt);
            return dt;
        }

        public object PopulateData_First_Fruit()
        {
            DataTable dt = new DataTable();
            string connection =
                @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command =
                new SqlCommand("[usp_Load_and_Search_First_Fruit]",
                    sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command
                .Parameters
                .AddWithValue("@searchText", Tx_FF_Search.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill (dt);
            return dt;
        }

        public object PopulateData_Pledge()
        {
            DataTable dt = new DataTable();
            string connection =
                @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command =
                new SqlCommand("[usp_Load_and_Search_Pledge]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command
                .Parameters
                .AddWithValue("@searchText", Tx_PP_Search.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill (dt);
            return dt;
        }

        public object PopulateData_ServiceOffering()
        {
            DataTable dt = new DataTable();
            string connection =
                @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command =
                new SqlCommand("[usp_Load_and_Search_ServiceO]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command
                .Parameters
                .AddWithValue("@searchText", Tx_Search_RO.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill (dt);
            return dt;
        }

        public void Load_ServiceO_DataView()
        {
            DataGrid_RO.DataSource = this.PopulateData_ServiceOffering();
        }

        public void Load_FF_DataView()
        {
            DataGrid_FF.DataSource = this.PopulateData_First_Fruit();
        }

        public void Load_PP_DataView()
        {
            DataGrid_Pledge.DataSource = this.PopulateData_Pledge();
        }

        public void Load_GenPay1_DataView()
        {
            DataGrib_GenralPayment.DataSource = this.PopulateData_GeneralPay();
        }

        public void Load_TithePay_DataView()
        {
            DatagRid_Tithe.DataSource = this.PopulateData_Tithe();
            DateTime_Paid_DT.Value = DateTime.Now;
        }
#endregion TASK POPULATING FORMS ENDS HERE
#region Main Form Load Starts
        private void Offerings_Load(object sender, EventArgs e)
        {
            Load_TithePay_DataView();
            Load_GenPay1_DataView();
            Load_FF_DataView();
            Load_FF_DataView();
            Load_PP_DataView();
            Load_ServiceO_DataView();
            realTotal = of100 + of50 + of20 + of5 + of1;
            Tx_SUB_Total.Text = Convert.ToString(realTotal);

            Radio_RO_MW.Checked = true;
            Tx_eTran_No.Visible = false;
            Tx_Chq_NoT.Visible = false;
            Tx_FF_eTran.Visible = false;
            Tx_FF_Chq.Visible = false;

            Radio_CashT.Checked = true;
            Radio_CedisT.Checked = true;

            Radio_FF_Cash.Checked = true;
            Radio_FF_Cedis.Checked = true;

            Radio_PP_Cedis.Checked = true;
            Radio_PP_Cash.Checked = true;

            Tx_PP_eTran.Visible = false;
            Tx_PP_Chq.Visible = false;

            Radio_PP_Member.Checked = true;
            lbl_Hide.Visible = false;
            N_M_PN.Visible = false;
            Tx_PP_Address.Visible = false;

            Tx_RO_MF.Visible = false;
            Tx_RO_SF.Visible = false;
            Tx_RO_SS.Visible = false;

            PN_SS.Visible = false;
            lbl_SS.Visible = false;
            lbl_SF.Visible = false;
            PN_SF.Visible = false;
            lbl_MF.Visible = false;
            PN_MF.Visible = false;
            lbl_MW.Visible = true;
            PN_MW.Visible = true;
        }
#endregion Main Form Load Ends
#region Searching Data DridView Starts
        private void txtSearch_By_NamePay_TextChanged(
            object sender,
            EventArgs e
        )
        {
            DataGrib_GenralPayment.DataSource = this.PopulateData_GeneralPay();
        }

        private void Tx_FF_Search_TextChanged(object sender, EventArgs e)
        {
            DataGrid_FF.DataSource = this.PopulateData_First_Fruit();
        }

        private void Tx_Search_Tithe_User_TextChanged(
            object sender,
            EventArgs e
        )
        {
            DatagRid_Tithe.DataSource = this.PopulateData_Tithe();
        }

        private void Tx_Search_RO_TextChanged(object sender, EventArgs e)
        {
            DataGrid_RO.DataSource = this.PopulateData_ServiceOffering();
        }
#endregion Searching Data DridView  Ends
#region Cell Content Click Starts
        private void CellClick_Pledge_DataGriD_View()
        {
            var pledgeClick =
                DBS
                    .MembTBs
                    .FirstOrDefault(a => a.TKC_MemID.Equals(Pledge.pledge));
            if (!String.IsNullOrEmpty(Pledge.pledge))
            {
                if (pledgeClick != null)
                {
                    Tx_PP_Phone.Text = pledgeClick.Phone;
                    Tx_PP_Name.Text = pledgeClick.Fullname;
                    Tx_PP_Cell.Text = pledgeClick.Cell_Name;
                    Tx_PP_Mem_ID.Text = pledgeClick.TKC_MemID;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void CellClick_Bonus_DataGriD_View()
        {
            var titheClick =
                DBS
                    .MembTBs
                    .FirstOrDefault(a => a.TKC_MemID.Equals(tithePay.tithe));
            if (!String.IsNullOrEmpty(tithePay.tithe))
            {
                if (titheClick != null)
                {
                    Tx_MemberPhone_Tithe.Text = titheClick.Phone;
                    Tx_MemName_Tithe.Text = titheClick.Fullname;
                    Tx_MemCell_Tithe.Text = titheClick.Cell_Name;
                    Tx_MemberID_Tithe.Text = titheClick.TKC_MemID;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void CellClick_FF_DataGriD_View()
        {
            var ffClick =
                DBS
                    .MembTBs
                    .FirstOrDefault(a => a.TKC_MemID.Equals(tithePay.tithe));
            if (!String.IsNullOrEmpty(tithePay.tithe))
            {
                if (ffClick != null)
                {
                    Tx_FF_Mem_Phone.Text = ffClick.Phone;
                    Tx_FF_Mem_Name.Text = ffClick.Fullname;
                    Tx_FF_Mem_Cell.Text = ffClick.Cell_Name;
                    Tx_FF_Mem_ID.Text = ffClick.TKC_MemID;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }


#endregion Cell Content Click Ends
#region DataGridView Cell Click Starts Here
        private void DatagRid_Tithe_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e
        )
        {
        }

        private void DataGrib_GenralPayment_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e
        )
        {
            Tx_TithePay_HIDE.Text =
                DataGrib_GenralPayment
                    .SelectedRows[0]
                    .Cells[0]
                    .Value
                    .ToString();
            tithePay.tithe = Tx_TithePay_HIDE.Text;
            CellClick_Bonus_DataGriD_View();

            Tx_FF_Hide.Text =
                DataGrib_GenralPayment
                    .SelectedRows[0]
                    .Cells[0]
                    .Value
                    .ToString();
            FF.first = Tx_FF_Hide.Text;
            CellClick_FF_DataGriD_View();

            Tx_PP_Hide.Text =
                DataGrib_GenralPayment
                    .SelectedRows[0]
                    .Cells[0]
                    .Value
                    .ToString();
            Pledge.pledge = Tx_PP_Hide.Text;
            CellClick_Pledge_DataGriD_View();
        }
#endregion DataGridView Cell Click Ends Here
#region Tithe Radio Button
        private void Radio_PP_Member_CheckedChanged(object sender, EventArgs e)
        {
            if (Radio_PP_Member.Checked)
            {
                N_M_PN.Visible = false;
                lbl_Hide.Visible = false;
                Tx_PP_Address.Visible = false;
                Tx_PP_Name.ReadOnly = true;
                Tx_PP_Mem_ID.ReadOnly = true;
                Tx_PP_Phone.ReadOnly = true;
            }
            else
            {
                N_M_PN.Visible = true;
                lbl_Hide.Visible = true;
                Tx_PP_Address.Visible = true;
                Tx_PP_Name.ReadOnly = false;
                Tx_PP_Mem_ID.ReadOnly = false;
                Tx_PP_Phone.ReadOnly = false;
            }
        }

        private void Radio_Cheque_CheckedChanged(object sender, EventArgs e)
        {
            if (Radio_ChequeT.Checked)
            {
                Tx_Chq_NoT.Visible = true;
            }
            else
            {
                Tx_Chq_NoT.Visible = false;
            }
        }

        private void Radio_Electronic_CheckedChanged(object sender, EventArgs e)
        {
            if (Radio_ElectronicT.Checked)
            {
                Tx_eTran_No.Visible = true;
            }
            else
            {
                Tx_eTran_No.Visible = false;
            }
        }

        private void Radio_FF_Check_CheckedChanged(object sender, EventArgs e)
        {
            if (Radio_FF_Check.Checked)
            {
                Tx_FF_Chq.Visible = true;
            }
            else
            {
                Tx_FF_Chq.Visible = false;
            }
        }

        private void Radio_FF_eTran_CheckedChanged(object sender, EventArgs e)
        {
            if (Radio_FF_eTran.Checked)
            {
                Tx_FF_eTran.Visible = true;
            }
            else
            {
                Tx_FF_eTran.Visible = false;
            }
        }

        private void Radio_PP_Chq_CheckedChanged(object sender, EventArgs e)
        {
            if (Radio_PP_Chq.Checked)
            {
                Tx_PP_Chq.Visible = true;
            }
            else
            {
                Tx_PP_Chq.Visible = false;
            }
        }

        private void Radio_PP_eTran_CheckedChanged(object sender, EventArgs e)
        {
            if (Radio_PP_eTran.Checked)
            {
                Tx_PP_eTran.Visible = true;
            }
            else
            {
                Tx_PP_eTran.Visible = false;
            }
        }


#endregion Tithe Radio Button
#region Adding Tithe Data To DB Starts
        private void BU_Tithe_Pay_Click(object sender, EventArgs e)
        {
            GL_Setup acc = new GL_Setup();
            GL_Transactions trans = new GL_Transactions();
            GL_Master_History Mastrans = new GL_Master_History();
            TitheTB addTithe = new TitheTB();
            amtNOW = Convert.ToDecimal(Tx_Tithe_Amount.Text);

            //try
            //{
            if (Radio_ChequeT.Checked)
            {
                paychannel = "Cheque";
            }
            else if (Radio_ElectronicT.Checked)
            {
                paychannel = "Electronic Transaction";
            }
            else
            {
                paychannel = "Cash";
            }

            if (Radio_PoundT.Checked)
            {
                curType = "Pound";
                curID = 3;
                curSymbol = "£"; 
                var itemP =
                    (from u in DBS.GL_Setup where u.GL_NO == 100003 select u)
                        .FirstOrDefault();
                itemP.Cur_Bal = itemP.Cur_Bal + amtNOW;

                var itemCP =
                    (from u in DBS.GL_Master where u.IL_NO == 200003 select u)
                        .FirstOrDefault();
                prevBal = (decimal) itemCP.IL_Bal;
                totalBal = prevBal + amtNOW;
                itemCP.IL_Bal = itemCP.IL_Bal + amtNOW;

                trans.Acct_no = (decimal) itemP.GL_NO;
                trans.Acct_type = itemP.GL_Type;
                trans.Description = "Payment of Tithe";
                trans.Member_ID = Tx_MemberID_Tithe.Text;
                trans.Create_dt = DateTime.Now;
                trans.Effective_dt = DateTime_Paid_DT.Value.Date;
                trans.Amt = amtNOW;
                trans.Debit_credit = 0;
                trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                trans.Cur_ID = (int) itemP.Cur_ID;
                DBS.GL_Transactions.Add (trans);

                Mastrans.GMH_GMH_Acct_no = (decimal) itemCP.IL_NO;
                Mastrans.GMH_Acct_type = itemCP.IL_Type;
                Mastrans.GMH_Description = "Payment of Tithe";
                Mastrans.GMH_Member_ID = Tx_MemberID_Tithe.Text;
                Mastrans.GMH_Create_dt = DateTime.Now;
                Mastrans.GMH_Effective_dt = DateTime_Paid_DT.Value.Date;
                Mastrans.GMH_Amt = amtNOW;
                Mastrans.GMH_Prev_Balance = prevBal;
                Mastrans.GMH_Total_Balance = totalBal;
                Mastrans.GMH_Debit_credit = 0;
                Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                Mastrans.GMH_Cur_ID = (int) itemCP.IL_Cur_ID;
                DBS.GL_Master_History.Add (Mastrans);

                DBS.SaveChanges();
            }
            else if (Radio_EuroT.Checked)
            {
                curType = "Euro";
                curID = 5;
                curSymbol = "€";
                var itemE =
                    (from u in DBS.GL_Setup where u.GL_NO == 100004 select u)
                        .FirstOrDefault();
                itemE.Cur_Bal = itemE.Cur_Bal + amtNOW;

                var itemCE =
                    (from u in DBS.GL_Master where u.IL_NO == 200004 select u)
                        .FirstOrDefault();
                prevBal = (decimal) itemCE.IL_Bal;
                totalBal = prevBal + amtNOW;
                itemCE.IL_Bal = itemCE.IL_Bal + amtNOW;

                trans.Acct_no = (decimal) itemE.GL_NO;
                trans.Acct_type = itemE.GL_Type;
                trans.Acct_type = itemE.GL_Type;
                trans.Description = "Payment of Tithe";
                trans.Member_ID = Tx_MemberID_Tithe.Text;
                trans.Create_dt = DateTime.Now;
                trans.Effective_dt = DateTime_Paid_DT.Value.Date;
                trans.Amt = amtNOW;
                trans.Debit_credit = 0;
                trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                trans.Cur_ID = (int) itemE.Cur_ID;
                DBS.GL_Transactions.Add (trans);
                DBS.SaveChanges();

                Mastrans.GMH_GMH_Acct_no = (decimal) itemCE.IL_NO;
                Mastrans.GMH_Acct_type = itemCE.IL_Type;
                Mastrans.GMH_Description = "Payment of Tithe";
                Mastrans.GMH_Member_ID = Tx_MemberID_Tithe.Text;
                Mastrans.GMH_Create_dt = DateTime.Now;
                Mastrans.GMH_Effective_dt = DateTime_Paid_DT.Value.Date;
                Mastrans.GMH_Amt = amtNOW;
                Mastrans.GMH_Prev_Balance = prevBal;
                Mastrans.GMH_Total_Balance = totalBal;
                Mastrans.GMH_Debit_credit = 0;
                Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                Mastrans.GMH_Cur_ID = (int) itemCE.IL_Cur_ID;
                DBS.GL_Master_History.Add (Mastrans);
            }
            else if (Radio_DollersT.Checked)
            {
                curType = "Dollars";
                curID = 2;
                curSymbol = "$";
                var itemD =
                    (from u in DBS.GL_Setup where u.GL_NO == 100002 select u)
                        .FirstOrDefault();
                itemD.Cur_Bal = itemD.Cur_Bal + amtNOW;

                var itemCD =
                    (from u in DBS.GL_Master where u.IL_NO == 200002 select u)
                        .FirstOrDefault();
                prevBal = (decimal) itemCD.IL_Bal;
                totalBal = prevBal + amtNOW;
                itemCD.IL_Bal = itemCD.IL_Bal + amtNOW;

                trans.Acct_no = (decimal) itemD.GL_NO;
                trans.Acct_type = itemD.GL_Type;
                trans.Acct_type = itemD.GL_Type;
                trans.Description = "Payment of Tithe";
                trans.Member_ID = Tx_MemberID_Tithe.Text;
                trans.Create_dt = DateTime.Now;
                trans.Effective_dt = DateTime_Paid_DT.Value.Date;
                trans.Amt = amtNOW;
                trans.Debit_credit = 0;
                trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                trans.Cur_ID = (int) itemD.Cur_ID;
                DBS.GL_Transactions.Add (trans);
                DBS.SaveChanges();

                Mastrans.GMH_GMH_Acct_no = (decimal) itemCD.IL_NO;
                Mastrans.GMH_Acct_type = itemCD.IL_Type;
                Mastrans.GMH_Description = "Payment of Tithe";
                Mastrans.GMH_Member_ID = Tx_MemberID_Tithe.Text;
                Mastrans.GMH_Create_dt = DateTime.Now;
                Mastrans.GMH_Effective_dt = DateTime_Paid_DT.Value.Date;
                Mastrans.GMH_Amt = amtNOW;
                Mastrans.GMH_Prev_Balance = prevBal;
                Mastrans.GMH_Total_Balance = totalBal;
                Mastrans.GMH_Debit_credit = 0;
                Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                Mastrans.GMH_Cur_ID = (int) itemCD.IL_Cur_ID;
                DBS.GL_Master_History.Add (Mastrans);
            }
            else
            {
                curType = "Cedis";
                curID = 1;
                curSymbol = "¢";
                var itemC =
                    (from u in DBS.GL_Setup where u.GL_NO == 100001 select u)
                        .FirstOrDefault();
                itemC.Cur_Bal = itemC.Cur_Bal + amtNOW;

                var itemCC =
                    (from u in DBS.GL_Master where u.IL_NO == 200001 select u)
                        .FirstOrDefault();
                prevBal = (decimal) itemCC.IL_Bal;
                totalBal = prevBal + amtNOW;
                itemCC.IL_Bal = itemCC.IL_Bal + amtNOW;

                trans.Acct_no = (decimal) itemC.GL_NO;
                trans.Acct_type = itemC.GL_Type;
                trans.Acct_type = itemC.GL_Type;
                trans.Member_ID = Tx_MemberID_Tithe.Text;
                trans.Description = "Payment of Tithe";
                trans.Create_dt = DateTime.Now;
                trans.Effective_dt = DateTime_Paid_DT.Value.Date;
                trans.Amt = amtNOW;
                trans.Debit_credit = 0;
                trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                trans.Cur_ID = (int) itemC.Cur_ID;
                DBS.GL_Transactions.Add (trans);
                DBS.SaveChanges();

                Mastrans.GMH_GMH_Acct_no = (decimal) itemCC.IL_NO;
                Mastrans.GMH_Acct_type = itemCC.IL_Type;
                Mastrans.GMH_Description = "Payment of Tithe";
                Mastrans.GMH_Member_ID = Tx_MemberID_Tithe.Text;
                Mastrans.GMH_Create_dt = DateTime.Now;
                Mastrans.GMH_Effective_dt = DateTime_Paid_DT.Value.Date;
                Mastrans.GMH_Amt = amtNOW;
                Mastrans.GMH_Prev_Balance = prevBal;
                Mastrans.GMH_Total_Balance = totalBal;
                Mastrans.GMH_Debit_credit = 0;
                Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                Mastrans.GMH_Cur_ID = (int) itemCC.IL_Cur_ID;
                DBS.GL_Master_History.Add (Mastrans);
            }

            if (
                Tx_Tithe_No.Text != "" &&
                Tx_Tithe_Amount.Text != "" &&
                Tx_MemberID_Tithe.Text != "" &&
                Tx_MemName_Tithe.Text != "" &&
                Tx_MemberPhone_Tithe.Text != "" &&
                Tx_MemCell_Tithe.Text != "" &&
                curType != null &&
                paychannel != null
            )
            {
                addTithe.T_eTran_No = Tx_eTran_No.Text;
                addTithe.T_Chq_No = Tx_Chq_NoT.Text;
                addTithe.T_Amount = amtNOW; /*Convert.ToDecimal( Tx_Tithe_Amount.Text);*/
                addTithe.T_Mem_ID = Tx_MemberID_Tithe.Text;
                addTithe.T_Mem_Name = Tx_MemName_Tithe.Text;
                addTithe.T_Mem_Phone = Tx_MemberPhone_Tithe.Text;
                addTithe.T_Mem_Cell = Tx_MemCell_Tithe.Text;
                addTithe.T_Pay_Channel = paychannel;
                addTithe.T_Cur_Type = curType;
                addTithe.T_CreatedBy = TKHCI_Main_Login.MainLogin.Username;
                addTithe.T_CreateDT = DateTime.Now;
                addTithe.Tithe_ID = Tx_Tithe_No.Text;
                addTithe.T_Paid_DT = DateTime_Paid_DT.Value.Date;
                addTithe.T_Description = Tx_Description.Text;
                addTithe.T_CurID = curID;
                addTithe.T_Cur_Symbol = curSymbol;

                string message = "Are you sure you want to add this record?";
                string title = "Adding Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.TitheTBs.Add (addTithe);
                    DBS.SaveChanges();
                    MessageBox.Show("Tithe Created Successfully");
                    BU_Tithe_Clear_Click(null, null);
                    Load_TithePay_DataView();
                }
                else
                    return;
            }
            else
            {
                if (
                    Application
                        .OpenForms
                        .OfType<Error_Fill_All_Form>()
                        .Count() ==
                    1
                )
                {
                    return;
                }
                else
                {
                    Error_Fill_All_Form eropen = new Error_Fill_All_Form();
                    eropen.Show();
                }
            }
            //}

            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        private void BU_FF_Pay_Click(object sender, EventArgs e)
        {
            GL_Transactions trans = new GL_Transactions();
            GL_Master_History Mastrans = new GL_Master_History();
            First_FruitTB addFF = new First_FruitTB();
            amtNOW = Convert.ToDecimal(Tx_FF_Amt.Text);
            try
            {
                if (Radio_FF_Check.Checked)
                {
                    paychannel2 = "Cheque";
                }
                else if (Radio_FF_eTran.Checked)
                {
                    paychannel2 = "Electronic Transaction";
                }
                else
                {
                    paychannel2 = "Cash";
                }

                if (Radio_FF_Pound.Checked)
                {
                    curType2 = "Pound";
                    curID = 3;
                    curSymbol = "£";
                    var item =
                        (
                        from u in DBS.GL_Setup where u.GL_NO == 100007 select u
                        ).FirstOrDefault();
                    item.Cur_Bal = item.Cur_Bal + amtNOW;

                    var itemCP =
                        (
                        from u in DBS.GL_Master where u.IL_NO == 200003 select u
                        ).FirstOrDefault();
                    prevBal = (decimal) itemCP.IL_Bal;
                    totalBal = prevBal + amtNOW;
                    itemCP.IL_Bal = itemCP.IL_Bal + amtNOW;

                    trans.Acct_no = (decimal) item.GL_NO;
                    trans.Acct_type = item.GL_Type;
                    trans.Description = "Payment of First Fruit";
                    trans.Member_ID = Tx_FF_Mem_ID.Text;
                    trans.Create_dt = DateTime.Now;
                    trans.Effective_dt = DateTime_Paid_DT.Value.Date;
                    trans.Amt = amtNOW;
                    trans.Debit_credit = 0;
                    trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    trans.Cur_ID = (int) item.Cur_ID;
                    DBS.GL_Transactions.Add (trans);
                    DBS.SaveChanges();

                    Mastrans.GMH_GMH_Acct_no = (decimal) itemCP.IL_NO;
                    Mastrans.GMH_Acct_type = itemCP.IL_Type;
                    Mastrans.GMH_Description = "Payment of First Fruit";
                    Mastrans.GMH_Member_ID = Tx_MemberID_Tithe.Text;
                    Mastrans.GMH_Create_dt = DateTime.Now;
                    Mastrans.GMH_Effective_dt = DateTime_Paid_DT.Value.Date;
                    Mastrans.GMH_Amt = amtNOW;
                    Mastrans.GMH_Prev_Balance = prevBal;
                    Mastrans.GMH_Total_Balance = totalBal;
                    Mastrans.GMH_Debit_credit = 0;
                    Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    Mastrans.GMH_Cur_ID = (int) itemCP.IL_Cur_ID;
                    DBS.GL_Master_History.Add (Mastrans);
                }
                else if (Radio_FF_Euro.Checked)
                {
                    curType2 = "Euro";
                    curID = 4;
                    curSymbol = "€";
                    var item =
                        (
                        from u in DBS.GL_Setup where u.GL_NO == 100008 select u
                        ).FirstOrDefault();
                    item.Cur_Bal = item.Cur_Bal + amtNOW;

                    var itemCE =
                        (
                        from u in DBS.GL_Master where u.IL_NO == 200004 select u
                        ).FirstOrDefault();
                    prevBal = (decimal) itemCE.IL_Bal;
                    totalBal = prevBal + amtNOW;
                    itemCE.IL_Bal = itemCE.IL_Bal + amtNOW;

                    trans.Acct_no = (decimal) item.GL_NO;
                    trans.Acct_type = item.GL_Type;
                    trans.Description = "Payment of First First Fruit";
                    trans.Member_ID = Tx_FF_Mem_ID.Text;
                    trans.Create_dt = DateTime.Now;
                    trans.Effective_dt = DateTime_Paid_DT.Value.Date;
                    trans.Amt = amtNOW;
                    trans.Debit_credit = 0;
                    trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    trans.Cur_ID = (int) item.Cur_ID;
                    DBS.GL_Transactions.Add (trans);
                    DBS.SaveChanges();

                    Mastrans.GMH_GMH_Acct_no = (decimal) itemCE.IL_NO;
                    Mastrans.GMH_Acct_type = itemCE.IL_Type;
                    Mastrans.GMH_Description = "Payment of First Fruit";
                    Mastrans.GMH_Member_ID = Tx_MemberID_Tithe.Text;
                    Mastrans.GMH_Create_dt = DateTime.Now;
                    Mastrans.GMH_Effective_dt = DateTime_Paid_DT.Value.Date;
                    Mastrans.GMH_Amt = amtNOW;
                    Mastrans.GMH_Prev_Balance = prevBal;
                    Mastrans.GMH_Total_Balance = totalBal;
                    Mastrans.GMH_Debit_credit = 0;
                    Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    Mastrans.GMH_Cur_ID = (int) itemCE.IL_Cur_ID;
                    DBS.GL_Master_History.Add (Mastrans);
                }
                else if (Radio_FF_Dollers.Checked)
                {
                    curType2 = "Dollars";
                    curID = 2;
                    curSymbol = "$";
                    var item =
                        (
                        from u in DBS.GL_Setup where u.GL_NO == 100005 select u
                        ).FirstOrDefault();
                    item.Cur_Bal = item.Cur_Bal + amtNOW;

                    var itemCD =
                        (
                        from u in DBS.GL_Master where u.IL_NO == 200002 select u
                        ).FirstOrDefault();
                    prevBal = (decimal) itemCD.IL_Bal;
                    totalBal = prevBal + amtNOW;
                    itemCD.IL_Bal = itemCD.IL_Bal + amtNOW;

                    trans.Acct_no = (decimal) item.GL_NO;
                    trans.Acct_type = item.GL_Type;
                    trans.Description = "Payment of First Fruit";
                    trans.Member_ID = Tx_FF_Mem_ID.Text;
                    trans.Create_dt = DateTime.Now;
                    trans.Effective_dt = DateTime_Paid_DT.Value.Date;
                    trans.Amt = amtNOW;
                    trans.Debit_credit = 0;
                    trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    trans.Cur_ID = (int) item.Cur_ID;
                    DBS.GL_Transactions.Add (trans);
                    DBS.SaveChanges();

                    Mastrans.GMH_GMH_Acct_no = (decimal) itemCD.IL_NO;
                    Mastrans.GMH_Acct_type = itemCD.IL_Type;
                    Mastrans.GMH_Description = "Payment of First Fruit";
                    Mastrans.GMH_Member_ID = Tx_MemberID_Tithe.Text;
                    Mastrans.GMH_Create_dt = DateTime.Now;
                    Mastrans.GMH_Effective_dt = DateTime_Paid_DT.Value.Date;
                    Mastrans.GMH_Amt = amtNOW;
                    Mastrans.GMH_Prev_Balance = prevBal;
                    Mastrans.GMH_Total_Balance = totalBal;
                    Mastrans.GMH_Debit_credit = 0;
                    Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    Mastrans.GMH_Cur_ID = (int) itemCD.IL_Cur_ID;
                    DBS.GL_Master_History.Add (Mastrans);
                }
                else if (Radio_FF_Cedis.Checked)
                {
                    curType2 = "Cedis";
                    curID = 1;
                    curSymbol = "¢";
                    var item =
                        (
                        from u in DBS.GL_Setup where u.GL_NO == 100006 select u
                        ).FirstOrDefault();
                    item.Cur_Bal = item.Cur_Bal + amtNOW;

                    var itemCC =
                        (
                        from u in DBS.GL_Master where u.IL_NO == 200001 select u
                        ).FirstOrDefault();
                    prevBal = (decimal) itemCC.IL_Bal;
                    totalBal = prevBal + amtNOW;
                    itemCC.IL_Bal = itemCC.IL_Bal + amtNOW;

                    trans.Acct_no = (decimal) item.GL_NO;
                    trans.Acct_type = item.GL_Type;
                    trans.Description = "Payment of First Fruit";
                    trans.Member_ID = Tx_FF_Mem_ID.Text;
                    trans.Create_dt = DateTime.Now;
                    trans.Effective_dt = DateTime_Paid_DT.Value.Date;
                    trans.Amt = amtNOW;
                    trans.Debit_credit = 0;
                    trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    trans.Cur_ID = (int) item.Cur_ID;
                    DBS.GL_Transactions.Add (trans);
                    DBS.SaveChanges();

                    Mastrans.GMH_GMH_Acct_no = (decimal) itemCC.IL_NO;
                    Mastrans.GMH_Acct_type = itemCC.IL_Type;
                    Mastrans.GMH_Description = "Payment of First Fruit";
                    Mastrans.GMH_Member_ID = Tx_MemberID_Tithe.Text;
                    Mastrans.GMH_Create_dt = DateTime.Now;
                    Mastrans.GMH_Effective_dt = DateTime_Paid_DT.Value.Date;
                    Mastrans.GMH_Amt = amtNOW;
                    Mastrans.GMH_Prev_Balance = prevBal;
                    Mastrans.GMH_Total_Balance = totalBal;
                    Mastrans.GMH_Debit_credit = 0;
                    Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    Mastrans.GMH_Cur_ID = (int) itemCC.IL_Cur_ID;
                    DBS.GL_Master_History.Add (Mastrans);
                }

                if (
                    Tx_FF_Amt.Text != "" &&
                    Tx_FF_Mem_ID.Text != "" &&
                    Tx_FF_Mem_Name.Text != "" &&
                    Tx_FF_Mem_Phone.Text != "" &&
                    Tx_FF_Mem_Cell.Text != "" &&
                    curType2 != null &&
                    paychannel2 != null
                )
                {
                    addFF.FF_eTran_No = Tx_FF_eTran.Text;
                    addFF.FF_Chq_No = Tx_FF_Chq.Text;
                    addFF.FF_Amount = Convert.ToDecimal(Tx_FF_Amt.Text);
                    addFF.FF_Mem_ID = Tx_FF_Mem_ID.Text;
                    addFF.FF_Mem_Name = Tx_FF_Mem_Name.Text;
                    addFF.FF_Mem_Phone = Tx_FF_Mem_Phone.Text;
                    addFF.FF_Mem_Cell = Tx_FF_Mem_Cell.Text;
                    addFF.FF_Pay_Channel = paychannel2;
                    addFF.FF_Cur_Type = curType2;
                    addFF.FF_CreatedBy = TKHCI_Main_Login.MainLogin.Username;
                    addFF.FF_CreateDT = DateTime.Now;
                    addFF.FF_NO = Tx_FF_No.Text;
                    addFF.FF_Cur_ID = curID;
                    addFF.FF_Cur_Symbol = curSymbol;

                    string message =
                        "Are you sure you want to add this record?";
                    string title = "Adding Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.First_FruitTB.Add (addFF);
                        DBS.SaveChanges();
                        MessageBox.Show("First Fruit Created Successfully");
                        BU_FF_Clear_Click(null, null);
                        Load_FF_DataView();
                    }
                    else
                        return;
                }
                else
                {
                    if (
                        Application
                            .OpenForms
                            .OfType<Error_Fill_All_Form>()
                            .Count() ==
                        1
                    )
                    {
                        return;
                    }
                    else
                    {
                        Error_Fill_All_Form eropen = new Error_Fill_All_Form();
                        eropen.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BU_Pay_Pldge_Click(object sender, EventArgs e)
        {
            GL_Transactions trans = new GL_Transactions();
            PledgeTB addPP = new PledgeTB();
            GL_Master_History Mastrans = new GL_Master_History();
            amtNOW = Convert.ToDecimal(Tx_PP_Amt.Text);
            try
            {
                if (Radio_PP_Chq.Checked)
                {
                    paychannel3 = "Cheque";
                }
                else if (Radio_PP_eTran.Checked)
                {
                    paychannel3 = "Electronic Transaction";
                }
                else
                {
                    paychannel3 = "Cash";
                }

                if (Radio_PP_Pound.Checked)
                {
                    curType3 = "Pound";
                    curID = 3;
                    curSymbol = "£";
                    var item =
                        (
                        from u in DBS.GL_Setup where u.GL_NO == 1000016 select u
                        ).FirstOrDefault();
                    item.Cur_Bal = item.Cur_Bal + amtNOW;

                    var itemCP =
                        (
                        from u in DBS.GL_Master where u.IL_NO == 200003 select u
                        ).FirstOrDefault();
                    prevBal = (decimal) itemCP.IL_Bal;
                    totalBal = prevBal + amtNOW;
                    itemCP.IL_Bal = itemCP.IL_Bal + amtNOW;

                    trans.Acct_no = (decimal) item.GL_NO;
                    trans.Acct_type = item.GL_Type;
                    trans.Description = "Payment of Pledge";
                    trans.Member_ID = Tx_PP_Mem_ID.Text;
                    trans.Create_dt = DateTime.Now;
                    trans.Effective_dt = DateTime_Paid_DT.Value.Date;
                    trans.Amt = amtNOW;
                    trans.Debit_credit = 0;
                    trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    trans.Cur_ID = (int) item.Cur_ID;
                    DBS.GL_Transactions.Add (trans);
                    DBS.SaveChanges();

                    Mastrans.GMH_GMH_Acct_no = (decimal) itemCP.IL_NO;
                    Mastrans.GMH_Acct_type = itemCP.IL_Type;
                    Mastrans.GMH_Description = "Payment of Pledge";
                    Mastrans.GMH_Member_ID = Tx_MemberID_Tithe.Text;
                    Mastrans.GMH_Create_dt = DateTime.Now;
                    Mastrans.GMH_Effective_dt = DateTime_Paid_DT.Value.Date;
                    Mastrans.GMH_Amt = amtNOW;
                    Mastrans.GMH_Prev_Balance = prevBal;
                    Mastrans.GMH_Total_Balance = totalBal;
                    Mastrans.GMH_Debit_credit = 0;
                    Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    Mastrans.GMH_Cur_ID = (int) itemCP.IL_Cur_ID;
                    DBS.GL_Master_History.Add (Mastrans);
                }
                else if (Radio_PP_Euro.Checked)
                {
                    curType3 = "Euro";
                    curID = 4;
                    curSymbol = "€";
                    var item =
                        (
                        from u in DBS.GL_Setup where u.GL_NO == 1000015 select u
                        ).FirstOrDefault();
                    item.Cur_Bal = item.Cur_Bal + amtNOW;

                    var itemCE =
                        (
                        from u in DBS.GL_Master where u.IL_NO == 200004 select u
                        ).FirstOrDefault();
                    prevBal = (decimal) itemCE.IL_Bal;
                    totalBal = prevBal + amtNOW;
                    itemCE.IL_Bal = itemCE.IL_Bal + amtNOW;

                    trans.Acct_no = (decimal) item.GL_NO;
                    trans.Acct_type = item.GL_Type;
                    trans.Acct_type = item.GL_Type;
                    trans.Description = "Payment of Pledge";
                    trans.Member_ID = Tx_PP_Mem_ID.Text;
                    trans.Create_dt = DateTime.Now;
                    trans.Effective_dt = DateTime_Paid_DT.Value.Date;
                    trans.Amt = amtNOW;
                    trans.Debit_credit = 0;
                    trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    trans.Cur_ID = (int) item.Cur_ID;
                    DBS.GL_Transactions.Add (trans);
                    DBS.SaveChanges();

                    Mastrans.GMH_GMH_Acct_no = (decimal) itemCE.IL_NO;
                    Mastrans.GMH_Acct_type = itemCE.IL_Type;
                    Mastrans.GMH_Description = "Payment of Pledge";
                    Mastrans.GMH_Member_ID = Tx_MemberID_Tithe.Text;
                    Mastrans.GMH_Create_dt = DateTime.Now;
                    Mastrans.GMH_Effective_dt = DateTime_Paid_DT.Value.Date;
                    Mastrans.GMH_Amt = amtNOW;
                    Mastrans.GMH_Prev_Balance = prevBal;
                    Mastrans.GMH_Total_Balance = totalBal;
                    Mastrans.GMH_Debit_credit = 0;
                    Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    Mastrans.GMH_Cur_ID = (int) itemCE.IL_Cur_ID;
                    DBS.GL_Master_History.Add (Mastrans);
                }
                else if (Radio_PP_Dollors.Checked)
                {
                    curType3 = "Dollars";
                    curID = 2;
                    curSymbol = "$";
                    var item =
                        (
                        from u in DBS.GL_Setup where u.GL_NO == 1000014 select u
                        ).FirstOrDefault();
                    item.Cur_Bal = item.Cur_Bal + amtNOW;

                    var itemCD =
                        (
                        from u in DBS.GL_Master where u.IL_NO == 200002 select u
                        ).FirstOrDefault();
                    prevBal = (decimal) itemCD.IL_Bal;
                    totalBal = prevBal + amtNOW;
                    itemCD.IL_Bal = itemCD.IL_Bal + amtNOW;

                    trans.Acct_no = (decimal) item.GL_NO;
                    trans.Acct_type = item.GL_Type;
                    trans.Acct_type = item.GL_Type;
                    trans.Description = "Payment of Pledge";
                    trans.Member_ID = Tx_PP_Mem_ID.Text;
                    trans.Create_dt = DateTime.Now;
                    trans.Effective_dt = DateTime_Paid_DT.Value.Date;
                    trans.Amt = amtNOW;
                    trans.Debit_credit = 0;
                    trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    trans.Cur_ID = (int) item.Cur_ID;
                    DBS.GL_Transactions.Add (trans);
                    DBS.SaveChanges();

                    Mastrans.GMH_GMH_Acct_no = (decimal) itemCD.IL_NO;
                    Mastrans.GMH_Acct_type = itemCD.IL_Type;
                    Mastrans.GMH_Description = "Payment of Pledge";
                    Mastrans.GMH_Member_ID = Tx_MemberID_Tithe.Text;
                    Mastrans.GMH_Create_dt = DateTime.Now;
                    Mastrans.GMH_Effective_dt = DateTime_Paid_DT.Value.Date;
                    Mastrans.GMH_Amt = amtNOW;
                    Mastrans.GMH_Prev_Balance = prevBal;
                    Mastrans.GMH_Total_Balance = totalBal;
                    Mastrans.GMH_Debit_credit = 0;
                    Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    Mastrans.GMH_Cur_ID = (int) itemCD.IL_Cur_ID;
                    DBS.GL_Master_History.Add (Mastrans);
                }
                else
                {
                    curType3 = "Cedis";
                    curID = 1;
                    curSymbol = "¢";
                    var item =
                        (
                        from u in DBS.GL_Setup where u.GL_NO == 1000013 select u
                        ).FirstOrDefault();
                    item.Cur_Bal = item.Cur_Bal + amtNOW;

                    var itemCC =
                        (
                        from u in DBS.GL_Master where u.IL_NO == 200001 select u
                        ).FirstOrDefault();
                    prevBal = (decimal) itemCC.IL_Bal;
                    totalBal = prevBal + amtNOW;
                    itemCC.IL_Bal = itemCC.IL_Bal + amtNOW;

                    trans.Acct_no = (decimal) item.GL_NO;
                    trans.Acct_type = item.GL_Type;
                    trans.Acct_type = item.GL_Type;
                    trans.Description = "Payment of Pledge";
                    trans.Member_ID = Tx_PP_Mem_ID.Text;
                    trans.Create_dt = DateTime.Now;
                    trans.Effective_dt = DateTime_Paid_DT.Value.Date;
                    trans.Amt = amtNOW;
                    trans.Debit_credit = 0;
                    trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    trans.Cur_ID = (int) item.Cur_ID;
                    DBS.GL_Transactions.Add (trans);
                    DBS.SaveChanges();

                    Mastrans.GMH_GMH_Acct_no = (decimal) itemCC.IL_NO;
                    Mastrans.GMH_Acct_type = itemCC.IL_Type;
                    Mastrans.GMH_Description = "Payment of Pledge";
                    Mastrans.GMH_Member_ID = Tx_MemberID_Tithe.Text;
                    Mastrans.GMH_Create_dt = DateTime.Now;
                    Mastrans.GMH_Effective_dt = DateTime_Paid_DT.Value.Date;
                    Mastrans.GMH_Amt = amtNOW;
                    Mastrans.GMH_Prev_Balance = prevBal;
                    Mastrans.GMH_Total_Balance = totalBal;
                    Mastrans.GMH_Debit_credit = 0;
                    Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                    Mastrans.GMH_Cur_ID = (int) itemCC.IL_Cur_ID;
                    DBS.GL_Master_History.Add (Mastrans);
                }
                if (Radio_PP_Member.Checked)
                {
                    isMember = "Member";
                }
                else
                {
                    isMember = "Not a Member";
                }

                if (
                    Tx_PP_Amt.Text != "" &&
                    Tx_PP_Name.Text != "" &&
                    curType3 != null &&
                    paychannel3 != null
                )
                {
                    addPP.PP_eTran_No = Tx_PP_eTran.Text;
                    addPP.PP_Chq_No = Tx_PP_Chq.Text;
                    addPP.PP_Amount = Convert.ToDecimal(Tx_PP_Amt.Text);
                    addPP.PP_Mem_ID = Tx_PP_Mem_ID.Text;
                    addPP.PP_Mem_Name = Tx_PP_Name.Text;
                    addPP.PP_Mem_Phone = Tx_PP_Phone.Text;
                    addPP.PP_Mem_Cell = Tx_PP_Cell.Text;
                    addPP.PP_Pay_Channel = paychannel3;
                    addPP.PP_Cur_Type = curType3;
                    addPP.PP_CreatedBy = TKHCI_Main_Login.MainLogin.Username;
                    addPP.PP_CreateDT = DateTime.Now;
                    addPP.PP_ID = Tx_Pledge_ID.Text;
                    addPP.PP_Address = Tx_PP_Address.Text;
                    addPP.PP_iSMember = isMember;
                    addPP.PP_CurID = curID;
                    addPP.PP_Cur_Symbol = curSymbol;

                    string message =
                        "Are you sure you want to add this record?";
                    string title = "Adding Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.PledgeTBs.Add (addPP);
                        DBS.SaveChanges();
                        MessageBox.Show("Pledge Created Successfully");
                        BU_Clear_Pledge_Click(null, null);
                        Load_PP_DataView();
                    }
                    else
                        return;
                }
                else
                {
                    if (
                        Application
                            .OpenForms
                            .OfType<Error_Fill_All_Form>()
                            .Count() ==
                        1
                    )
                    {
                        return;
                    }
                    else
                    {
                        Error_Fill_All_Form eropen = new Error_Fill_All_Form();
                        eropen.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BU_ADD_RO_Click(object sender, EventArgs e)
        {
            GL_Transactions trans = new GL_Transactions();
            ServiceOfferingTB addSO = new ServiceOfferingTB();
            GL_Master_History Mastrans = new GL_Master_History();

            //try
            //{
            if (Radio_RO_MW.Checked)
            {
                typeofService = "Mid-Week Service";
            }
            else if (Radio_RO_MF.Checked)
            {
                typeofService = "Morning Flavour Service";
            }
            else if (Radio_RO_SF.Checked)
            {
                typeofService = "Sunday First Service";
            }
            else
            {
                typeofService = "Sunday Second Service";
            }
            if (Tx_RO_MF.Text == "")
            {
                decimal Mf = 0;
                Tx_RO_MF.Text = Convert.ToString(Mf);
            }
            if (Tx_RO_MW.Text == "")
            {
                decimal MW = 0;
                Tx_RO_MW.Text = Convert.ToString(MW);
            }
            if (Tx_RO_SF.Text == "")
            {
                decimal SF = 0;
                Tx_RO_SF.Text = Convert.ToString(SF);
            }
            if (Tx_RO_SS.Text == "")
            {
                decimal SS = 0;
                Tx_RO_SS.Text = Convert.ToString(SS);
            }

            amtNOW =
                Convert.ToDecimal(Tx_RO_MF.Text) +
                Convert.ToDecimal(Tx_RO_MW.Text) +
                Convert.ToDecimal(Tx_RO_SF.Text) +
                Convert.ToDecimal(Tx_RO_SS.Text);

            if (
                Tx_RO_MW.Text != "" ||
                Tx_RO_MF.Text != "" ||
                Tx_RO_SF.Text != "" ||
                Tx_RO_SS.Text != ""
            )
            {
                addSO.RO_CreatedBY = TKHCI_Main_Login.MainLogin.Username;
                addSO.RO_CreateDT = DateTime.Now;

                addSO.MW_Offering = Convert.ToDecimal(Tx_RO_MW.Text);
                addSO.MF_Offering = Convert.ToDecimal(Tx_RO_MF.Text);
                addSO.SF_Offering = Convert.ToDecimal(Tx_RO_SF.Text);
                addSO.SS_Offering = Convert.ToDecimal(Tx_RO_SS.Text);
                addSO.OR_ID = Tx_RO_ID.Text;
                addSO.Service_Type = typeofService;
                var item =
                    (from u in DBS.GL_Setup where u.GL_NO == 100009 select u)
                        .FirstOrDefault();

                var itemCC =
                    (from u in DBS.GL_Master where u.IL_NO == 200001 select u)
                        .FirstOrDefault();
                prevBal = (decimal) itemCC.IL_Bal;
                totalBal = prevBal + amtNOW;

                itemCC.IL_Bal = itemCC.IL_Bal + amtNOW;
                item.Cur_Bal = item.Cur_Bal + amtNOW;
                trans.Acct_no = (decimal) item.GL_NO;
                trans.Acct_type = item.GL_Type;
                trans.Acct_type = item.GL_Type;
                trans.Description = typeofService;
                trans.Create_dt = DateTime.Now;
                trans.Effective_dt = DateTime.Now;
                trans.Amt = amtNOW;
                trans.Debit_credit = 0;
                trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                trans.Cur_ID = (int) item.Cur_ID;
                DBS.GL_Transactions.Add (trans);

                Mastrans.GMH_GMH_Acct_no = (decimal) itemCC.IL_NO;
                Mastrans.GMH_Acct_type = itemCC.IL_Type;
                Mastrans.GMH_Description = typeofService;
                Mastrans.GMH_Member_ID = "Offering Deposit";
                Mastrans.GMH_Create_dt = DateTime.Now;
                Mastrans.GMH_Effective_dt = DateTime_Paid_DT.Value.Date;
                Mastrans.GMH_Amt = amtNOW;
                Mastrans.GMH_Prev_Balance = prevBal;
                Mastrans.GMH_Total_Balance = totalBal;
                Mastrans.GMH_Debit_credit = 0;
                Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                Mastrans.GMH_Cur_ID = (int) itemCC.IL_Cur_ID;
                DBS.GL_Master_History.Add (Mastrans);
                DBS.SaveChanges();

                string message = "Are you sure you want to add this record?";
                string title = "Adding Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.ServiceOfferingTBs.Add (addSO);
                    DBS.SaveChanges();
                    MessageBox.Show("Offering Created Successfully");
                    BU_CLEAR_RO_Click(null, null);
                    Load_ServiceO_DataView();
                }
            }

            //    else return;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
#endregion Adding Tithe Data To DB Ends
#region FORM CLEARING
        private void BU_CLEAR_RO_Click(object sender, EventArgs e)
        {
            Tx_RO_MW.Text = "";
            Tx_RO_MF.Text = "";
            Tx_RO_SF.Text = "";
            Tx_RO_SS.Text = "";
            Tx_Search_RO.Text = "";
            Keepers_Load_ServicecO();
        }

        private void BU_Tithe_Clear_Click(object sender, EventArgs e)
        {
            Keepers_Load_Tithe();
            Tx_eTran_No.Text = "";
            Tx_Chq_NoT.Text = "";
            Tx_Tithe_Amount.Text = "";
            Tx_MemberID_Tithe.Text = "";
            Tx_MemName_Tithe.Text = "";
            Tx_MemberPhone_Tithe.Text = "";
            Tx_MemCell_Tithe.Text = "";
            Radio_CedisT.Checked = true;
            Radio_CashT.Checked = true;
        }

        private void BU_FF_Clear_Click(object sender, EventArgs e)
        {
            Tx_FF_Chq.Text = "";
            Tx_FF_Amt.Text = "";
            Radio_FF_Cash.Checked = true;
            Radio_FF_Cedis.Checked = true;
            Tx_eTran_No.Text = "";
            Tx_FF_Mem_Cell.Text = "";
            Tx_FF_Mem_Name.Text = "";
            Tx_FF_Mem_Phone.Text = "";
            Tx_FF_Search.Text = "";
            Keepers_Load_First_Fruit();
        }

        private void BU_Clear_Pledge_Click(object sender, EventArgs e)
        {
            Keepers_Load_Pledge();
            Tx_PP_eTran.Text = "";
            Tx_PP_Chq.Text = "";
            Tx_PP_Address.Text = "";
            Tx_PP_Amt.Text = "";
            Tx_PP_Cell.Text = "";
            Tx_PP_Chq.Text = "";
            Tx_PP_Mem_ID.Text = "";
            Tx_PP_Name.Text = "";
            Tx_PP_Phone.Text = "";
            Tx_PP_Pledge_Type.Text = "";
            Radio_PP_Cash.Checked = true;
            Radio_PP_Member.Checked = true;
            Radio_PP_Cedis.Checked = true;
        }


#endregion FORM CLEARING
#region Key Press OnLY NUMBERS
        private void Tx_RO_MW_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (
                !char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.')
            )
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (
                (e.KeyChar == '.') &&
                ((sender as TextBox).Text.IndexOf('.') > -1)
            )
            {
                e.Handled = true;
            }
        }


#endregion Key Press OnLY NUMBERS
#region RADIO LOAD SERVICE OFFERING
        private void Radio_RO_MW_CheckedChanged(object sender, EventArgs e)
        {
            if (Radio_RO_MW.Checked)
            {
                Tx_RO_MW.Visible = true;
                Tx_RO_MF.Visible = false;
                Tx_RO_SF.Visible = false;
                Tx_RO_SS.Visible = false;

                lbl_MW.Visible = true;
                PN_MW.Visible = true;
                lbl_MF.Visible = false;
                PN_MF.Visible = false;
                lbl_SF.Visible = false;
                PN_SF.Visible = false;
                PN_SS.Visible = false;
                lbl_SS.Visible = false;
            }
            else
            {
                Tx_RO_MW.Visible = false;
                Tx_RO_MF.Visible = false;
                Tx_RO_SF.Visible = false;
                Tx_RO_SS.Visible = false;

                lbl_MW.Visible = false;
                PN_MW.Visible = false;
                lbl_MF.Visible = false;
                PN_MF.Visible = false;
                lbl_SF.Visible = false;
                PN_SF.Visible = false;
                PN_SS.Visible = false;
                lbl_SS.Visible = false;
            }
        }

        private void Radio_RO_MF_CheckedChanged(object sender, EventArgs e)
        {
            if (Radio_RO_MF.Checked)
            {
                Tx_RO_MW.Visible = false;
                Tx_RO_MF.Visible = true;
                Tx_RO_SF.Visible = false;
                Tx_RO_SS.Visible = false;

                lbl_MW.Visible = false;
                PN_MW.Visible = false;
                lbl_MF.Visible = true;
                PN_MF.Visible = true;
                lbl_SF.Visible = false;
                PN_SF.Visible = false;
                PN_SS.Visible = false;
                lbl_SS.Visible = false;
            }
            else
            {
                Tx_RO_MW.Visible = true;
                Tx_RO_MF.Visible = false;
                Tx_RO_SF.Visible = false;
                Tx_RO_SS.Visible = false;

                lbl_MW.Visible = false;
                PN_MW.Visible = false;
                lbl_MF.Visible = false;
                PN_MF.Visible = false;
                lbl_SF.Visible = false;
                PN_SF.Visible = false;
                PN_SS.Visible = false;
                lbl_SS.Visible = false;
            }
        }

        private void Radio_RO_SF_CheckedChanged(object sender, EventArgs e)
        {
            if (Radio_RO_SF.Checked)
            {
                Tx_RO_MW.Visible = false;
                Tx_RO_MF.Visible = false;
                Tx_RO_SF.Visible = true;
                Tx_RO_SS.Visible = false;

                lbl_MW.Visible = false;
                PN_MW.Visible = false;
                lbl_MF.Visible = false;
                PN_MF.Visible = false;
                lbl_SF.Visible = true;
                PN_SF.Visible = true;
                PN_SS.Visible = false;
                lbl_SS.Visible = false;
            }
            else
            {
                Tx_RO_MW.Visible = true;
                Tx_RO_MF.Visible = false;
                Tx_RO_SF.Visible = false;
                Tx_RO_SS.Visible = false;

                lbl_MW.Visible = false;
                PN_MW.Visible = false;
                lbl_MF.Visible = false;
                PN_MF.Visible = false;
                lbl_SF.Visible = false;
                PN_SF.Visible = false;
                PN_SS.Visible = false;
                lbl_SS.Visible = false;
            }
        }

        private void Radio_RO_SS_CheckedChanged(object sender, EventArgs e)
        {
            if (Radio_RO_SS.Checked)
            {
                Tx_RO_MW.Visible = false;
                Tx_RO_MF.Visible = false;
                Tx_RO_SF.Visible = false;
                Tx_RO_SS.Visible = true;

                lbl_MW.Visible = false;
                PN_MW.Visible = false;
                lbl_MF.Visible = false;
                PN_MF.Visible = false;
                lbl_SF.Visible = false;
                PN_SF.Visible = false;
                PN_SS.Visible = true;
                lbl_SS.Visible = true;
            }
            else
            {
                Tx_RO_MW.Visible = true;
                Tx_RO_MF.Visible = false;
                Tx_RO_SF.Visible = false;
                Tx_RO_SS.Visible = false;

                lbl_MW.Visible = false;
                PN_MW.Visible = false;
                lbl_MF.Visible = false;
                PN_MF.Visible = false;
                lbl_SF.Visible = false;
                PN_SF.Visible = false;
                PN_SS.Visible = false;
                lbl_SS.Visible = false;
            }
        }
#endregion RADIO LOAD SERVICE OFFERING
#region MONEY COUNTER SHEET
        private void Tx_100_Qty_TextChanged(object sender, EventArgs e)
        {
            if (Tx_100_Qty.Text != "")
            {
                quantity1 = Convert.ToDecimal(Tx_100_Qty.Text);
                of100 = price1 * quantity1;
                Tx_100_Total.Text = Convert.ToString(of100);
                grandTotalN = of100 + of50 + of20 + of20 + of10 + of5 + of1;
                Tx_SUB_Total.Text = Convert.ToString(grandTotalN);
            }
            else
            {
                grandTotalN = realTotal + of50 + of20 + +of10 + of5 + of1;
                Tx_SUB_Total.Text = Convert.ToString(grandTotalN);
                Tx_100_Total.Text = "";
                of100 = zero;
            }
        }

        private void Tx_50_Qty_TextChanged(object sender, EventArgs e)
        {
            if (Tx_50_Qty.Text != "")
            {
                quantity2 = Convert.ToDecimal(Tx_50_Qty.Text);
                of50 = price2 * quantity2;
                Tx_50_Total.Text = Convert.ToString(of50);
                grandTotalN = of100 + of50 + of20 + of10 + of5 + of1;
                Tx_SUB_Total.Text = Convert.ToString(grandTotalN);
            }
            else
            {
                grandTotalN = realTotal + of100 + of20 + of10 + of5 + of1;
                Tx_SUB_Total.Text = Convert.ToString(grandTotalN);
                Tx_50_Total.Text = "";
                of50 = zero;
            }
        }

        private void Tx_20_Qty_TextChanged(object sender, EventArgs e)
        {
            if (Tx_20_Qty.Text != "")
            {
                quantity3 = Convert.ToDecimal(Tx_20_Qty.Text);
                of20 = price3 * quantity3;
                Tx_20_Total.Text = Convert.ToString(of20);
                grandTotalN = of100 + of50 + of20 + of10 + of5 + of1;
                Tx_SUB_Total.Text = Convert.ToString(grandTotalN);
            }
            else
            {
                grandTotalN = realTotal + of100 + of50 + of10 + of5 + of1;
                Tx_SUB_Total.Text = Convert.ToString(grandTotalN);
                Tx_20_Total.Text = "";
                of20 = zero;
            }
        }

        private void Tx_10_Qty_TextChanged(object sender, EventArgs e)
        {
            if (Tx_10_Qty.Text != "")
            {
                quantity4 = Convert.ToDecimal(Tx_10_Qty.Text);
                of10 = price4 * quantity4;
                Tx_10_Total.Text = Convert.ToString(of10);
                grandTotalN = of100 + of50 + of20 + of10 + of5 + of1;
                Tx_SUB_Total.Text = Convert.ToString(grandTotalN);
            }
            else
            {
                grandTotalN = realTotal + of100 + of50 + of20 + of5 + of1;
                Tx_SUB_Total.Text = Convert.ToString(grandTotalN);
                Tx_10_Total.Text = "";
                of10 = zero;
            }
        }

        private void Tx_5_Qty_TextChanged(object sender, EventArgs e)
        {
            if (Tx_5_Qty.Text != "")
            {
                quantity5 = Convert.ToDecimal(Tx_5_Qty.Text);
                of5 = price5 * quantity5;
                Tx_5_Total.Text = Convert.ToString(of5);
                grandTotalN = of100 + of50 + of20 + of10 + of5 + of1;
                Tx_SUB_Total.Text = Convert.ToString(grandTotalN);
            }
            else
            {
                grandTotalN = realTotal + of100 + of50 + of20 + of10 + of1;
                Tx_SUB_Total.Text = Convert.ToString(grandTotalN);
                Tx_5_Total.Text = "";
                of5 = zero;
            }
        }

        private void Tx_1_Qty_TextChanged(object sender, EventArgs e)
        {
            if (Tx_1_Qty.Text != "")
            {
                quantity6 = Convert.ToDecimal(Tx_1_Qty.Text);
                of1 = price6 * quantity6;
                Tx_1_Total.Text = Convert.ToString(of1);
                grandTotalN = of100 + of50 + of20 + of10 + of5 + of1;
                Tx_SUB_Total.Text = Convert.ToString(grandTotalN);
            }
            else
            {
                grandTotalN = realTotal + of100 + of50 + of20 + of10 + of5;
                Tx_SUB_Total.Text = Convert.ToString(grandTotalN);
                Tx_1_Total.Text = "";
                of1 = zero;
            }
        }


#endregion
    }
}
