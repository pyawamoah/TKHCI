using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TKHCI.Finance
{
    public partial class PayrollCRM_ORigin : Form
    {
        private readonly string load_salGrade = "Select 'Select Salary Grade' as SalGrade union all SELECT distinct Sal_GradeTB.Grade_Code salGrade from Sal_GradeTB with (nolock)";
        private readonly string load_attGrade = "Select 'Select Members Name' as memName union all SELECT distinct UserTB.Fullname salGrade from UserTB with (nolock)";
        private readonly string load_Bonus_salGrade = "Select 'Select Salary Grade' as Bonus_salGrade union all SELECT distinct Sal_GradeTB.Grade_Code salGrade from Sal_GradeTB with (nolock)";
        private readonly string load_emplName = "Select 'Select Members Name' as meName union all SELECT distinct Salary_UserTB.NameFull salGrade from Salary_UserTB with (nolock)";
        KeeperDBEntities  DBS = new KeeperDBEntities();
        decimal Disposal;
        decimal pres,abst,exdty;
        public static class SalGradeOne
        {
            public static string salGrade_one;
        }
        public static class SalUser
        {
            public static string salUser;
        }
        public static class LOANS
        {
            public static string loans;
        }
        public static class ATT
        {
            public static string att;
        }
        public static class BONUST
        {
            public static string bonus;
        }
        public PayrollCRM_ORigin()
        {
            InitializeComponent();
            _ = DefualtLoad();
        }
        //-----------------------------------------------------------------------------------
        //START Salary Calculations
        #region Salary Calculations
        public void Salary()
        {

        }
        #endregion Salary Calculations
        //-----------------------------------------------------------------------------------
        //END Salary Calculations

        //-----------------------------------------------------------------------------------
        //START Code Public 
        #region Code Public 
        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
        public async Task DefualtLoad()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(Keepers_Load_SalGrade());
            tasks.Add(Keepers_Load_Sal_User());
            tasks.Add(Keepers_Load_Loans());
            tasks.Add(Keepers_Bonus());
            tasks.Add(Load_Combo_Box());
            tasks.Add(Load_Combo_ME_Name());
            tasks.Add(Load_Combo_Bonus_SG());
            tasks.Add(Keepers_ATT_Loans());
            tasks.Add(Keepers_SalaryPAY());
            tasks.Add(MemberName());
            await Task.WhenAll(tasks); 
        }

        private Task Load_Combo_ME_Name()
        {
            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter(load_emplName, Con);
            DataTable meNa = new DataTable();
            da.Fill(meNa);
            ComboEMPL_Name.DataSource = meNa;
            ComboEMPL_Name.DisplayMember = "meName";
            ComboEMPL_Name.ValueMember = "meName";
            Con.Close();
            return Task.CompletedTask;
        }
        private Task Load_Combo_Bonus_SG()
        {
            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter(load_Bonus_salGrade, Con);
            DataTable BsalGrade = new DataTable();
            da.Fill(BsalGrade);
            ComboBox_SG_Code.DataSource = BsalGrade;
            ComboBox_SG_Code.DisplayMember = "Bonus_salGrade";
            ComboBox_SG_Code.ValueMember = "Bonus_salGrade";
            Con.Close();
            return Task.CompletedTask;
        }
        private Task Load_Combo_Box()
        {
            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter(load_salGrade, Con);
            DataTable salGrade = new DataTable();
            da.Fill(salGrade);
            ComboBox_Sal_Grade.DataSource = salGrade;
            ComboBox_Sal_Grade.DisplayMember = "SalGrade";
            ComboBox_Sal_Grade.ValueMember = "SalGrade";
            Con.Close();
            return Task.CompletedTask;
        }
        private Task MemberName()
        {
            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter(load_attGrade, Con);
            DataTable attD = new DataTable();
            da.Fill(attD);
            ComboBox_Empl_Name.DataSource = attD;
            ComboBox_Empl_Name.DisplayMember = "memName";
            ComboBox_Empl_Name.ValueMember = "memName";
            Con.Close();
            return Task.CompletedTask;
        }
        private void ComboBox_Empl_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection PY = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
            PY.Open();
            string Query = "select TKHS_ID from UserTB where Fullname = '" + ComboBox_Empl_Name.Text + "' ";
            SqlCommand cmd = new SqlCommand(Query, PY);
            SqlDataReader dr = cmd.ExecuteReader();         
            if (dr.Read())
            {
                Tx_ATT_MemberID.Text = dr["TKHS_ID"].ToString();

            }
            PY.Close();
        }
        #endregion Code Public 
        //END LCode Public 
        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------
        //START Load ID's 
        #region Load ID's 
        private Task Keepers_Load_SalGrade()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_SalaryGrade_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();


            if (xml < 1)
            {
                Disposal = 1;
                Tx_SalGrade_No.Text = $"KH-SALG-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            else if (xml >= 1)
            {
                Disposal = xml + 1;
                Tx_SalGrade_No.Text = $"KH-SALG-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            return Task.CompletedTask;

        }
        private Task Keepers_Load_Sal_User()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_PayrollUser_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();


            if (xml < 1)
            {
                Disposal = 1;
                Tx_Salary_Acct_No.Text = $"SALNO-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            else if (xml >= 1)
            {
                Disposal = xml + 1;
                Tx_Salary_Acct_No.Text = $"SALNO-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            return Task.CompletedTask;

        }
        private Task Keepers_Load_Loans()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_LOANS_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();


            if (xml < 1)
            {
                Disposal = 1;
                Tx_Loan_No.Text = $"KHIL0-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            else if (xml >= 1)
            {
                Disposal = xml + 1;
                Tx_Loan_No.Text = $"KHIL0-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            return Task.CompletedTask;

        }
        private Task Keepers_ATT_Loans()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_Attendance_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();


            if (xml < 1)
            {
                Disposal = 1;
                Tx_ATT_NO.Text = $"ATT-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            else if (xml >= 1)
            {
                Disposal = xml + 1;
                Tx_ATT_NO.Text = $"ATT-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            return Task.CompletedTask;

        }
        private Task Keepers_SalaryPAY()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_SalPay_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();


            if (xml < 1)
            {
                Disposal = 1;
                Tx_Payslip_No.Text = $"PL-00-" + Convert.ToString(Disposal);
            }
            else if (xml >= 1)
            {
                Disposal = xml + 1;
                Tx_Payslip_No.Text = $"PL-00-" + Convert.ToString(Disposal);
            }
            return Task.CompletedTask;

        }
        private Task Keepers_Bonus()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_Bonus_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();


            if (xml < 1)
            {
                Disposal = 1;
                Tx_Bonus_NO.Text = $"KHBONUS-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            else if (xml >= 1)
            {
                Disposal = xml + 1;
                Tx_Bonus_NO.Text = $"KHBONUS-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            return Task.CompletedTask;

        }
        #endregion Load ID's
        //END Load ID's 
        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------
        //START Populate
        #region Populate
        public object PopulateData_Bonus()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_Search_Bonus]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_Bonus.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public object PopulateData_MemID()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_MemID]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", ComboBox_Empl_Name.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            SqlDataReader dr = command.ExecuteReader();
            Tx_ATT_MemberID.Text = dr["TKHS_ID"].ToString();
            sda.Fill(dt);
            return dt;
        }
        public object PopulateData_SalGrade()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_Search_SalGrade]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_SearchBY_GradeName.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public object PopulateData_SalaryUser()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_Salary_User]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_SearchBY_GradeName.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public object PopulateData_LOANS()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_Search_LOANS]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_Loan.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public object PopulateData_Attendance()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_Search_Attendance]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_ATT.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public object PopulateData_Sal_Payment()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_Search_SalaryPay]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_SearchSAL.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        
        public void Load_Bonus_DataView()
        {
            DataGridView_Bonus.DataSource = this.PopulateData_Bonus();
        }
        public void Load_PaySal_DataView()
        {
            DataGridView_PaySal.DataSource = this.PopulateData_Sal_Payment();
        }
        public void Load_SalGrade_DataView()
        {
            DataGridView_SalGrade.DataSource = this.PopulateData_SalGrade();
        }
        public void Load_SalUser_DataView()
        {
            DataView_PayrollUser.DataSource = this.PopulateData_SalaryUser();
        }
        public void Load_Loans_DataView()
        {
            DataGrid_Loans.DataSource = this.PopulateData_LOANS();
        }
        public void Load_ATT_DataView()
        {
            DataGrid_Attendance.DataSource = this.PopulateData_Attendance();
        }

        private void CellClick_Bonus_DataGriD_View()
        {
            var BonusClick = DBS.BonusTBs.FirstOrDefault(a => a.Bonus_ID.Equals(BONUST.bonus));
            if (!String.IsNullOrEmpty(BONUST.bonus))
            {
                if (BonusClick != null)
                {
                    Tx_Bonus_NO.Text = BonusClick.Bonus_ID;
                    ComboBox_SG_Code.Text = BonusClick.Sal_Grade_Code2;
                    Tx_Bonus_Name.Text = BonusClick.Bonus_Name;
                    Tx_Bonus_Amount.Text = Convert.ToString(BonusClick.Amt);

                }
                else { return; }
            }
            else
            {
                return;
            }
        }
        private void CellClick_Salary_Grade_DataGriD_View()
        {
            var salGradeClick = DBS.Sal_GradeTB.FirstOrDefault(a => a.Grade_ID.Equals(SalGradeOne.salGrade_one));
            if (!String.IsNullOrEmpty(SalGradeOne.salGrade_one))
            {
                if (salGradeClick != null)
                {
                    Tx_SalGrade_No.Text = salGradeClick.Grade_ID;
                    Tx_Grade_Name.Text = salGradeClick.Grade_Name;
                    Tx_Grade_Code.Text = salGradeClick.Grade_Code;    
                    Tx_Base_Amt.Text = Convert.ToString(salGradeClick.Base_Amt);
                    Tx_OV.Text = Convert.ToString( salGradeClick.OT_Per);
                    Tx_Performance_Bonus.Text = Convert.ToString(salGradeClick.Perf_Bonus);
                }
                else { return; }
            }
            else
            {
                return;
            }
        }
        private void CellClick_Salary_User_DataGriD_View()
        {
            var salSUClick = DBS.Salary_UserTB.FirstOrDefault(a => a.Pay_No.Equals(SalUser.salUser));
            if (!String.IsNullOrEmpty(SalUser.salUser))
            {
                if (salSUClick != null)
                {
                    Tx_Salary_Acct_No.Text = salSUClick.Pay_No;
                    DateTime_PRU_DOB.Value = salSUClick.DOB.Value;
                    Tx_EmailAddress.Text = salSUClick.Email;
                    Tx_Fullname.Text = salSUClick.NameFull;
                    Tx_SU_Phone.Text = salSUClick.PhoneNo;
                    Combo_SalUser_Status.Text = salSUClick.Acct_Status;
                    ComboBox_Sal_Grade.Text = salSUClick.Sal_Grade;
                    Tx_MemberID.Text = salSUClick.TKHS_ID;
                    
                }
                else { return; }
            }
            else
            {
                return;
            }
        }
        private void CellClick_LOANS_DataGriD_View()
        {
            var loansClick = DBS.LoanTBs.FirstOrDefault(a => a.Loan_NO.Equals(LOANS.loans));
            if (!String.IsNullOrEmpty(LOANS.loans))
            {
                if (loansClick != null)
                {
                    Tx_Loan_No.Text = loansClick.Loan_NO;
                    Tx_Loan_SalNo.Text = loansClick.LPay_No;
                    Tx_SalaryL_Grade.Text = loansClick.Sal_Grade_Code;
                    Tx_Member_Name.Text = loansClick.FullName;
                    Tx_Mem_Phone.Text = loansClick.PhoneNoR;
                    Tx_Loan_Amt.Text =Convert.ToString(loansClick.LoanAmt);
                    Tx_Address.Text = loansClick.Address;
                    Tx_Loan_Purpose.Text = loansClick.LoanPurpose;
                    Tx_Monthly_Deduct.Text = Convert.ToString(loansClick.MonthlyDeduct);
                    Tx_Mem_ID.Text = loansClick.Member_ID;
                    DateTime_Need_DT.Value = loansClick.LoanNeededDT.Value.Date;
                }
                else { return; }
            }
            else
            {
                return;
            }
        }
        private void CellClick_ATT_DataGriD_View()
        {
            var attendClick = DBS.AttendanceTBs.FirstOrDefault(a => a.Atten_No.Equals(ATT.att));
            if (!String.IsNullOrEmpty(ATT.att))
            {
                if (attendClick != null)
                {
                    Tx_ATT_NO.Text = attendClick.Atten_No;
                    ComboBox_Empl_Name.Text = attendClick.FullName1;
                    Tx_ATT_MemberID.Text = attendClick.EmplID;
                    DateTime_ATT_Date.Value = attendClick.ATT_Date.Value.Date;
                    RD_Present.Text = Convert.ToString(attendClick.Present);
                    RD_Absent.Text = Convert.ToString(attendClick.Adsent);
                    RD_ExcusedDuty.Text = Convert.ToString(attendClick.Excuse);

                }
                else { return; }
            }
            else
            {
                return;
            }
            
        }
        private void DataGridView_Bonus_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_HideBonus.Text = DataGridView_Bonus.SelectedRows[0].Cells[1].Value.ToString();
            BONUST.bonus = Tx_HideBonus.Text;
            CellClick_Bonus_DataGriD_View();
        }
        private void DataGridView_SalGrade_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_SalGrade_HIDE.Text = DataGridView_SalGrade.SelectedRows[0].Cells[1].Value.ToString();
            SalGradeOne.salGrade_one = Tx_SalGrade_HIDE.Text;
            CellClick_Salary_Grade_DataGriD_View();
        }
        private void DataView_PayrollUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_PayrollUser_HIDE.Text = DataView_PayrollUser.SelectedRows[0].Cells[1].Value.ToString();
            SalUser.salUser = Tx_PayrollUser_HIDE.Text;
            CellClick_Salary_User_DataGriD_View();
        }
        private void DataGrid_Loans_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_Loan_Hide.Text = DataGrid_Loans.SelectedRows[0].Cells[1].Value.ToString();
            LOANS.loans = Tx_Loan_Hide.Text;
            CellClick_LOANS_DataGriD_View();
        }
        private void DataGrid_Attendance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_ATT_Hide.Text = DataGrid_Attendance.SelectedRows[0].Cells[1].Value.ToString();
            ATT.att = Tx_ATT_Hide.Text;
            CellClick_ATT_DataGriD_View();
        }
        private void DataGridView_Bonus_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            Tx_HideBonus.Text = DataGridView_Bonus.SelectedRows[0].Cells[1].Value.ToString();
            BONUST.bonus = Tx_HideBonus.Text;
            CellClick_Bonus_DataGriD_View();
        }


        #endregion Populate
        //END Populate
        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------
        //START Adding, Clear, Update and Delete
        #region Adding, Clear, Update and Delete
        //************************** ADD ********************************************************** STARTS ********************
        private void BU_Add_Grade_Click(object sender, EventArgs e)
        {
            Sal_GradeTB salAdd = new Sal_GradeTB();
            try
            {

                if (Tx_SalGrade_No.Text != "" && Tx_Grade_Name.Text != "" && Tx_Grade_Code.Text != "" 
                    && Tx_Base_Amt.Text != "" && Tx_OV.Text != "" && Tx_Performance_Bonus.Text != "")
                {


                    salAdd.CreateDATE = DateTime.Now;
                    salAdd.OT_Per = Convert.ToDecimal(Tx_OV.Text);
                    salAdd.CreatedBY_Who = TKHCI_Main_Login.MainLogin.Username;
                    salAdd.Grade_ID = Tx_SalGrade_No.Text;
                    salAdd.Grade_Code = Tx_Grade_Code.Text;
                    salAdd.Perf_Bonus = Convert.ToDecimal(Tx_Performance_Bonus.Text);
                    salAdd.Base_Amt = Convert.ToDecimal(Tx_Base_Amt.Text);
                    salAdd.Grade_Name = Tx_Grade_Name.Text;


                    string message = "Are you sure you want to add this record?";
                    string title = "Adding Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.Sal_GradeTB.Add(salAdd);
                        DBS.SaveChanges();
                        MessageBox.Show("Salary Grade Created Successfully");
                        BU_Clear_Grade_Click(null, null);
                        Load_SalGrade_DataView();


                    }
                    else return;
                }
                else
                {
                    if (Application.OpenForms.OfType<Error_Fill_All_Form>().Count() == 1)
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
        private void BU_Add_SalU_Click(object sender, EventArgs e)
        {
            Salary_UserTB AddSU = new Salary_UserTB();
            try
            {

                if (Tx_Salary_Acct_No.Text != "" &&
                    DateTime_PRU_DOB.Text != "" &&
                    Tx_EmailAddress.Text != "" &&
                    Tx_Fullname.Text != "" &&
                    Tx_SU_Phone.Text != "" &&
                    Combo_SalUser_Status.Text != "" &&
                    ComboBox_Sal_Grade.Text != "" &&
                    Tx_MemberID.Text != ""
                    
                    )
                {

                    AddSU.Pay_No = Tx_Salary_Acct_No.Text;
                    AddSU.DOB = DateTime_PRU_DOB.Value.Date;
                    AddSU.Email= Tx_EmailAddress.Text;
                    AddSU.NameFull= Tx_Fullname.Text;
                    AddSU.PhoneNo= Tx_SU_Phone.Text;
                    AddSU.Acct_Status= Combo_SalUser_Status.Text;
                    AddSU.Sal_Grade= ComboBox_Sal_Grade.Text;
                    AddSU.TKHS_ID= Tx_MemberID.Text;

                    string message = "Are you sure you want to add this record?";
                    string title = "Adding Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.Salary_UserTB.Add(AddSU);
                        DBS.SaveChanges();
                        MessageBox.Show("User Created Successfully");
                        BU_Clear_SalU_Click(null, null);
                        Load_SalUser_DataView();


                    }
                    else return;
                }
                else
                {
                    if (Application.OpenForms.OfType<Error_Fill_All_Form>().Count() == 1)
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
        private void BU_Add_Loan_Click(object sender, EventArgs e)
        {
            LoanTB addLoans = new LoanTB();
            try
            {

                if (Tx_Loan_No.Text != "" &&
                    Tx_Loan_SalNo.Text != "" &&
                    Tx_SalaryL_Grade.Text != "" &&
                    Tx_Member_Name.Text != "" &&
                    Tx_Mem_Phone.Text != "" &&
                    Tx_Loan_Amt.Text != "" &&
                    Tx_Address.Text != "" &&
                    Tx_Loan_Purpose.Text != "" &&
                    Tx_Monthly_Deduct.Text != "" &&
                    Tx_Mem_ID.Text != ""

                    )
                {

                    addLoans.Loan_NO= Tx_Loan_No.Text;
                    addLoans.LPay_No = Tx_Loan_SalNo.Text;
                    addLoans.Sal_Grade_Code = Tx_SalaryL_Grade.Text;
                    addLoans.FullName =Tx_Member_Name.Text;
                    addLoans.PhoneNoR=Tx_Mem_Phone.Text;
                    addLoans.LoanAmt =  Convert.ToDecimal( Tx_Loan_Amt.Text);
                    addLoans.Address =Tx_Address.Text;
                    addLoans.LoanPurpose=Tx_Loan_Purpose.Text;
                    addLoans.MonthlyDeduct =Convert.ToDecimal( Tx_Monthly_Deduct.Text);
                    addLoans.Member_ID =Tx_Mem_ID.Text;
                    addLoans.LoanNeededDT = DateTime_Need_DT.Value.Date;
                    addLoans.CreatedLBy = TKHCI_Main_Login.MainLogin.Username;
                    addLoans.Create_LDT = DateTime.Now;
                    addLoans.EmailADD = Tx_Mem_Email.Text;

                    string message = "Are you sure you want to add this record?";
                    string title = "Adding Loan Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.LoanTBs.Add(addLoans);
                        DBS.SaveChanges();
                        MessageBox.Show("Loan Created Successfully");
                        BU_Clear_Loan_Click(null, null);
                        Load_Loans_DataView();


                    }
                    else return;
                }
                else
                {
                    if (Application.OpenForms.OfType<Error_Fill_All_Form>().Count() == 1)
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
        private void BU_Add_ATT_Click(object sender, EventArgs e)
        {
            AttendanceTB addATT = new AttendanceTB();
            try
            {

                if (
                    Tx_ATT_NO.Text != "" &&
                    ComboBox_Empl_Name.Text != "" &&
                    Tx_ATT_MemberID.Text != "" &&
                    RD_Present.Text != "" &&
                    RD_Absent.Text != "" &&
                    RD_ExcusedDuty.Text != "" 

                    )
                {
                    if (RD_Present.Checked)
                    {

                        pres = 1;
                        abst = 0;
                        exdty = 0;
                        addATT.Present = pres;
                        addATT.Adsent =abst;
                        addATT.Excuse = exdty;
                    }
                    else if (RD_Absent.Checked)
                    {
                        pres = 0;
                        abst = 1;
                        exdty = 0;
                        addATT.Present = pres;
                        addATT.Adsent = abst;
                        addATT.Excuse = exdty;
                    }
                    else if (RD_ExcusedDuty.Checked)
                    {
                        pres = 0;
                        abst = 0;
                        exdty = 1;
                        addATT.Present = pres;
                        addATT.Adsent = abst;
                        addATT.Excuse = exdty;
                    }

                    addATT.Atten_No = Tx_ATT_NO.Text;
                    addATT.FullName1= ComboBox_Empl_Name.Text;
                    addATT.EmplID = Tx_ATT_MemberID.Text;
                    addATT.ATT_Date = DateTime_ATT_Date.Value.Date;
                    addATT.CreatedByEmpl = TKHCI_Main_Login.MainLogin.Username;
                    addATT.DateCreatedDT = DateTime.Now;



                    string message = "Are you sure you want to add this record?";
                    string title = "Adding Loan Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.AttendanceTBs.Add(addATT);
                        DBS.SaveChanges();
                        MessageBox.Show("Attendance Created Successfully");
                        BU_Clear_ATT_Click(null, null);
                        Load_ATT_DataView();


                    }
                    else return;
                }
                else
                {
                    if (Application.OpenForms.OfType<Error_Fill_All_Form>().Count() == 1)
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
        private void BU_ADD_Bonus_Click(object sender, EventArgs e)
        {
            BonusTB addBonus = new BonusTB();
            try
            {

                if (Tx_Bonus_NO.Text != "" &&
                    ComboBox_SG_Code.SelectedIndex !=0 &&
                    Tx_Bonus_Name.Text != "" &&
                    Tx_Bonus_Amount.Text != "" 

                    )
                {

                    addBonus.Bonus_ID = Tx_Bonus_NO.Text;
                    addBonus.Sal_Grade_Code2 = ComboBox_SG_Code.Text;
                    addBonus.Bonus_Name = Tx_Bonus_Name.Text;
                    addBonus.Amt = Convert.ToDecimal(Tx_Bonus_Amount.Text);                 
                    addBonus.CreatedBY = TKHCI_Main_Login.MainLogin.Username;
                    addBonus.CreateDT = DateTime.Now;


                    string message = "Are you sure you want to add this record?";
                    string title = "Adding Loan Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.BonusTBs.Add(addBonus);
                        DBS.SaveChanges();
                        MessageBox.Show("Bonus Created Successfully");
                        BU_CLEAR_Bonus_Click(null, null);
                        CellClick_Bonus_DataGriD_View();
                    }
                    else return;
                }
                else
                {
                    if (Application.OpenForms.OfType<Error_Fill_All_Form>().Count() == 1)
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
        private void BU_PS_Add_Click(object sender, EventArgs e)
        {
            Sal_PaymentTB addSal_Pay = new Sal_PaymentTB();
            try
            {

                if (Tx_Payslip_No.Text != "" &&
                    ComboEMPL_Name.Text != "" &&
                    Tx_PaySal_No.Text != "" &&
                    Tx_SP_Mem_ID.Text != "" &&
                    Tx_SP_SalGrade.Text != "" &&
                    Tx_SP_BaseAmt.Text != "" &&
                    Tx_LoanDEBIT.Text != "" &&
                    Tx_NetSal.Text != "" &&
                    Tx_PS_Present.Text != "" &&
                    Tx_PS_Absent.Text != "" &&
                    Tx_PS_Excuss.Text != "" &&
                    Tx_BonusSal.Text != ""

                    )
                {

                    addSal_Pay.Pay_Slip_No = Tx_Payslip_No.Text;
                    addSal_Pay.Name_Of_Member = ComboEMPL_Name.Text;
                    addSal_Pay.Pay_Acct_No = Tx_PaySal_No.Text;
                    addSal_Pay.MembershipID = Tx_SP_Mem_ID.Text;
                    addSal_Pay.SalGrade = Tx_SP_SalGrade.Text;
                    addSal_Pay.BaseAmt = Convert.ToDecimal(Tx_SP_BaseAmt.Text);
                    addSal_Pay.LoanDedt = Convert.ToDecimal(Tx_LoanDEBIT.Text);
                    addSal_Pay.NetSal = Convert.ToDecimal(Tx_NetSal.Text);
                    addSal_Pay.Mark_Present = Convert.ToDecimal(Tx_PS_Present.Text);
                    addSal_Pay.Mark_Absent = Convert.ToDecimal(Tx_PS_Absent.Text);
                    addSal_Pay.Mark_Excuse = Convert.ToDecimal(Tx_PS_Excuss.Text);
                    addSal_Pay.MonthBonus = Convert.ToDecimal(Tx_BonusSal.Text);
                    addSal_Pay.CreatedSBY = TKHCI_Main_Login.MainLogin.Username;
                    addSal_Pay.CreateSDT = DateTime.Now;

                    string message = "Are you sure you want to add this record?";
                    string title = "Adding Loan Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.Sal_PaymentTB.Add(addSal_Pay);
                        DBS.SaveChanges();
                        MessageBox.Show("Payment Made Successfully");
                        BU_PS_Clear_Click(null, null);
                        CellClick_Bonus_DataGriD_View();
                        Load_PaySal_DataView();
                    }
                    else return;
                }
                else
                {
                    if (Application.OpenForms.OfType<Error_Fill_All_Form>().Count() == 1)
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

        //************************** ADD ********************************************************** END********************
        //************************** CLEAR ********************************************************** STARTS ********************
        private void BU_CLEAR_Bonus_Click(object sender, EventArgs e)
        {
            Keepers_Bonus();
            ComboBox_SG_Code.SelectedIndex = 0;
            Tx_Bonus_Name.Text = "";
            Tx_Bonus_Amount.Text = "";
        }
        private void BU_Clear_Grade_Click(object sender, EventArgs e)
        {
            Tx_SalGrade_No.Text ="";
            Tx_Grade_Name.Text = "";
            Tx_Grade_Code.Text = "";
            Tx_Base_Amt.Text = "";
            Tx_OV.Text = "";
            Tx_Performance_Bonus.Text = "";
            Keepers_Load_SalGrade();
        }
        private void BU_Clear_SalU_Click(object sender, EventArgs e)
        {   
            Tx_Salary_Acct_No.Text = "";
            DateTime_PRU_DOB.Text = "";
            Tx_EmailAddress.Text = "";
            Tx_Fullname.Text = "";
            Tx_SU_Phone.Text = "";
            Combo_SalUser_Status.Text = "";
            ComboBox_Sal_Grade.Text = "";
            Tx_MemberID.Text = "";
            Tx_SearchBU_SalU.Text = "";
            Keepers_Load_Sal_User();

        }
        private void BU_Clear_Loan_Click(object sender, EventArgs e)
        {
            Tx_Mem_Email.Text = "";
            Tx_Loan_No.Text = "";
            Tx_Loan_SalNo.Text = "";
            Tx_SalaryL_Grade.Text = "";
            Tx_Member_Name.Text = "";
            Tx_Mem_Phone.Text = "";
            Tx_Loan_Amt.Text = "";
            Tx_Address.Text = "";
            Tx_Loan_Purpose.Text = "";
            Tx_Monthly_Deduct.Text = "";
            Tx_Mem_ID.Text = "";
            Tx_Search_Loan.Text = "";
            Keepers_ATT_Loans();
        }
        private void BU_Clear_ATT_Click(object sender, EventArgs e)
        {
            ComboBox_Empl_Name.SelectedIndex = 0;
            Tx_ATT_MemberID.Text = "";
            Keepers_ATT_Loans();
        }

        private void BU_PS_Clear_Click(object sender, EventArgs e)
        {
            PayrollCRM_ORigin_Load(null,null);
            Keepers_SalaryPAY();
            ComboEMPL_Name.SelectedIndex = 0;
            Tx_PaySal_No.Text = "";
            Tx_SP_Mem_ID.Text = "";
            Tx_SP_SalGrade.Text = "";
            Tx_SP_BaseAmt.Text = "";
            Tx_LoanDEBIT.Text = "";
            Tx_NetSal.Text = "";
            Tx_PS_Present.Text = "";
            Tx_PS_Absent.Text = "";
            Tx_PS_Excuss.Text = "";
            Tx_BonusSal.Text = "";
        }

        //************************** CLEAR ********************************************************** ENDS ********************
        //************************** UPDATE ********************************************************** STARTS ********************
        private void BU_Update_SalU_Click(object sender, EventArgs e)
        {
            string updateSU = Tx_PayrollUser_HIDE.Text;
            var updt_SU = (from s in DBS.Salary_UserTB where s.Pay_No == updateSU select s).FirstOrDefault();

            updt_SU.Pay_No = Tx_Salary_Acct_No.Text;
            updt_SU.DOB = DateTime_PRU_DOB.Value.Date;
            updt_SU.Email = Tx_EmailAddress.Text;
            updt_SU.NameFull = Tx_Fullname.Text;
            updt_SU.PhoneNo = Tx_SU_Phone.Text;
            updt_SU.Acct_Status = Combo_SalUser_Status.Text;
            updt_SU.Sal_Grade = ComboBox_Sal_Grade.Text;
            updt_SU.TKHS_ID = Tx_MemberID.Text;

            string message = "Are you sure you want to update this record?";
            string title = "updating Record...";
            MessageBoxButtons but = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, but);
            if (result == DialogResult.Yes)
            {

                DBS.SaveChanges();
                MessageBox.Show("Zone Record Updated Successfully");
                Load_SalUser_DataView();
            }
            else return;
        }
        private void BU_Update_Grade_Click(object sender, EventArgs e)
        {
            string updateSG = Tx_SalGrade_HIDE.Text;
            var updt_SG = (from s in DBS.Sal_GradeTB where s.Grade_ID == updateSG select s).FirstOrDefault();

            updt_SG.OT_Per = Convert.ToDecimal(Tx_OV.Text);
            updt_SG.Grade_ID = Tx_SalGrade_No.Text;
            updt_SG.Grade_Code = Tx_Grade_Code.Text;
            updt_SG.Perf_Bonus = Convert.ToDecimal(Tx_Performance_Bonus.Text);
            updt_SG.Base_Amt = Convert.ToDecimal(Tx_Base_Amt.Text);
            updt_SG.Grade_Name = Tx_Grade_Name.Text;


            string message = "Are you sure you want to update this record?";
            string title = "updating Record...";
            MessageBoxButtons but = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, but);
            if (result == DialogResult.Yes)
            {

                DBS.SaveChanges();
                MessageBox.Show(" Record Updated Successfully");
                Load_SalGrade_DataView();
            }
            else return;
        }
        private void BU_Update_Loan_Click(object sender, EventArgs e)
        {
            string updateLaon = Tx_Loan_Hide.Text;
            var updt_Loan = (from s in DBS.LoanTBs where s.Loan_NO == updateLaon select s).FirstOrDefault();
            updt_Loan.Loan_NO = Tx_Loan_No.Text;
            updt_Loan.LPay_No = Tx_Loan_SalNo.Text;
            updt_Loan.Sal_Grade_Code = Tx_SalaryL_Grade.Text;
            updt_Loan.FullName = Tx_Member_Name.Text;
            updt_Loan.PhoneNoR = Tx_Mem_Phone.Text;
            updt_Loan.LoanAmt = Convert.ToDecimal(Tx_Loan_Amt.Text);
            updt_Loan.Address = Tx_Address.Text;
            updt_Loan.LoanPurpose = Tx_Loan_Purpose.Text;
            updt_Loan.MonthlyDeduct = Convert.ToDecimal(Tx_Monthly_Deduct.Text);
            updt_Loan.Member_ID = Tx_Mem_ID.Text;
            updt_Loan.EmailADD = Tx_Mem_Email.Text;
            updt_Loan.LoanNeededDT = DateTime_Need_DT.Value.Date;

            string message = "Are you sure you want to update this record?";
            string title = "updating Record...";
            MessageBoxButtons but = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, but);
            if (result == DialogResult.Yes)
            {

                DBS.SaveChanges();
                MessageBox.Show("Record Updated Successfully");
                Load_Loans_DataView();
                BU_Clear_Loan_Click(null,null);
            }
            else return;
        }
        private void BU_Update_Bonus_Click(object sender, EventArgs e)
        {
            string updateBonus = Tx_Loan_Hide.Text;
            var updt_Bonus = (from s in DBS.BonusTBs where s.Bonus_ID == updateBonus select s).FirstOrDefault();

            updt_Bonus.Bonus_ID = Tx_Bonus_NO.Text;
            updt_Bonus.Sal_Grade_Code2 = ComboBox_SG_Code.Text;
            updt_Bonus.Bonus_Name = Tx_Bonus_Name.Text;
            updt_Bonus.Amt = Convert.ToDecimal(Tx_Bonus_Amount.Text);

            string message = "Are you sure you want to update this record?";
            string title = "updating Record...";
            MessageBoxButtons but = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, but);
            if (result == DialogResult.Yes)
            {

                DBS.SaveChanges();
                MessageBox.Show("Record Updated Successfully");
                Load_Loans_DataView();
                BU_Clear_Loan_Click(null, null);
            }
            else return;
        }
        private void BU_Update_ATT_Click(object sender, EventArgs e)
        {
            string updateATT = Tx_ATT_Hide.Text;
            var updt_ATT = (from s in DBS.AttendanceTBs where s.Atten_No == updateATT select s).FirstOrDefault();
            if (RD_Present.Checked)
            {

                pres = 1;
                abst = 0;
                exdty = 0;
                updt_ATT.Present = pres;
                updt_ATT.Adsent = abst;
                updt_ATT.Excuse = exdty;
            }
            else if (RD_Absent.Checked)
            {
                pres = 0;
                abst = 1;
                exdty = 0;
                updt_ATT.Present = pres;
                updt_ATT.Adsent = abst;
                updt_ATT.Excuse = exdty;
            }
            else if (RD_ExcusedDuty.Checked)
            {
                pres = 0;
                abst = 0;
                exdty = 1;
                updt_ATT.Present = pres;
                updt_ATT.Adsent = abst;
                updt_ATT.Excuse = exdty;
            }

            updt_ATT.Atten_No = Tx_ATT_NO.Text;
            updt_ATT.FullName1 = ComboBox_Empl_Name.Text;
            updt_ATT.EmplID = Tx_ATT_MemberID.Text;
            updt_ATT.ATT_Date = DateTime_ATT_Date.Value.Date;

            string message = "Are you sure you want to update this record?";
            string title = "updating Record...";
            MessageBoxButtons but = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, but);
            if (result == DialogResult.Yes)
            {

                DBS.SaveChanges();
                MessageBox.Show("Record Updated Successfully");
                Load_Loans_DataView();
                BU_Clear_Loan_Click(null, null);
            }
            else return;
        }

        //************************** UPDATE ********************************************************** ENDS ********************
        //************************** DELETE ********************************************************** STARTS ********************
        private void BU_Dell_SalU_Click(object sender, EventArgs e)
        {
            Salary_UserTB catiGo = new Salary_UserTB();
            if (Tx_PayrollUser_HIDE.Text == "")
            {
                MessageBox.Show("Select Family to delete");
            }
            else
            {
                //string Del = Tx_Hide_Fam_ID.Text;
                var delSU = (from s in DBS.Salary_UserTB where s.Pay_No.Equals(Tx_PayrollUser_HIDE.Text) select s).First();

                string message = "Are you sure you want to delete this Asset?";
                string title = "Deleting Categoty...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.Salary_UserTB.Remove(delSU);
                    DBS.SaveChanges();
                    MessageBox.Show("Asset  Deleted");
                    Load_SalUser_DataView();
                    BU_Clear_SalU_Click(null,null);

                }
                else return;

            }
        }
        private void BU_Del_Grade_Click(object sender, EventArgs e)
        {
            Sal_GradeTB catiGo = new Sal_GradeTB();
            if (Tx_SalGrade_HIDE.Text == "")
            {
                MessageBox.Show("Select Family to delete");
            }
            else
            {
                //string Del = Tx_Hide_Fam_ID.Text;
                var delSG = (from s in DBS.Sal_GradeTB where s.Grade_ID.Equals(Tx_SalGrade_HIDE.Text) select s).First();

                string message = "Are you sure you want to delete this?";
                string title = "Deleting Salary Grade...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.Sal_GradeTB.Remove(delSG);
                    DBS.SaveChanges();
                    MessageBox.Show("Asset  Deleted");
                    Load_SalGrade_DataView();
                    BU_Clear_Grade_Click(null,null);

                }
                else return;

            }
        }

        private void BU_Del_Loan_Click(object sender, EventArgs e)
        {
            LoanTB catiGo = new LoanTB();
            if (Tx_Loan_Hide.Text == "")
            {
                MessageBox.Show("Select Loan to delete");
            }
            else
            {
               
                var delLoan = (from s in DBS.LoanTBs where s.Loan_NO.Equals(Tx_Loan_Hide.Text) select s).First();

                string message = "Are you sure you want to delete this?";
                string title = "Deleting Loan...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.LoanTBs.Remove(delLoan);
                    DBS.SaveChanges();
                    MessageBox.Show("Asset  Deleted");
                    Load_SalGrade_DataView();
                    BU_Clear_Loan_Click(null, null);

                }
                else return;

            }
        }
        private void BU_DEL_Bonus_Click(object sender, EventArgs e)
        {
            BonusTB delBonus = new BonusTB();
            if (Tx_HideBonus.Text == "")
            {
                MessageBox.Show("Select Bonus to delete");
            }
            else
            {

                var delATT = (from s in DBS.BonusTBs where s.Bonus_ID.Equals(Tx_HideBonus.Text) select s).First();

                string message = "Are you sure you want to delete this?";
                string title = "Deleting Bonus...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.BonusTBs.Remove(delBonus);
                    DBS.SaveChanges();
                    MessageBox.Show("Bonus  Deleted");
                    Load_Bonus_DataView();
                    BU_CLEAR_Bonus_Click(null, null);
                    PayrollCRM_ORigin_Load(null, null);

                }
                else return;

            }
        }
        private void BU_Delete_ATT_Click(object sender, EventArgs e)
        {
            AttendanceTB catiGo = new AttendanceTB();
            if (Tx_ATT_Hide.Text == "")
            {
                MessageBox.Show("Select Loan to delete");
            }
            else
            {

                var delATT = (from s in DBS.AttendanceTBs where s.Atten_No.Equals(Tx_ATT_Hide.Text) select s).First();

                string message = "Are you sure you want to delete this?";
                string title = "Deleting Loan...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.AttendanceTBs.Remove(delATT);
                    DBS.SaveChanges();
                    MessageBox.Show("Attendance  Deleted");
                    Load_ATT_DataView();
                    BU_Clear_ATT_Click(null, null);
                    PayrollCRM_ORigin_Load(null,null);

                }
                else return;

            }
        }
        //************************** DELETE ********************************************************** ENDS **********************
        private void PayrollCRM_ORigin_Load(object sender, EventArgs e)
        {
            Load_SalGrade_DataView();
            Load_SalUser_DataView();
            Load_Loans_DataView();
            Load_ATT_DataView();
            Load_Bonus_DataView();
            Load_PaySal_DataView();
            DateTime_ATT_Date.Value = DateTime.Now;
            DateTimePicker_End.Value = DateTime.Now;



            Tx_SP_Hide.Enabled = false;
            Tx_Payslip_No.Enabled = false;
            ComboEMPL_Name.Enabled = false;
            Tx_PaySal_No.Enabled = false;
            Tx_SP_Mem_ID.Enabled = false;
            Tx_SP_SalGrade.Enabled = false;
            Tx_SP_BaseAmt.Enabled = false;
            Tx_LoanDEBIT.Enabled = false;
            Tx_NetSal.Enabled = false;
            DateTimePicker_Start.Enabled = false;
            DateTimePicker_End.Enabled = false;
            Tx_PS_Present.Enabled = false;
            Tx_PS_Absent.Enabled = false;
            Tx_PS_Excuss.Enabled = false;
            Tx_BonusSal.Enabled = false;


        }

        private void Tx_SearchBY_GradeName_TextChanged(object sender, EventArgs e)
        {
            DataGridView_SalGrade.DataSource = this.PopulateData_SalGrade();
        }

        private void Tx_Search_ATT_TextChanged(object sender, EventArgs e)
        {
            DataGrid_Attendance.DataSource = this.PopulateData_Attendance();
        }

        private void Tx_SearchSAL_TextChanged(object sender, EventArgs e)
        {
            DataGridView_PaySal.DataSource = this.PopulateData_Sal_Payment();
        }

        

        private void Tx_SearchBU_SalU_TextChanged(object sender, EventArgs e)
        {
            DataView_PayrollUser.DataSource = this.PopulateData_SalaryUser();
        }

        private void Tx_Search_Bonus_TextChanged(object sender, EventArgs e)
        {
            DataGridView_Bonus.DataSource = this.PopulateData_Bonus();
        }

        private void Tx_Activate_Payment_Click(object sender, EventArgs e)
        {
            Tx_SP_Hide.Enabled = true;
            Tx_Payslip_No.Enabled = true;
            ComboEMPL_Name.Enabled = true;
            Tx_PaySal_No.Enabled = true;
            Tx_SP_Mem_ID.Enabled = true;
            Tx_SP_SalGrade.Enabled = true;
            Tx_SP_BaseAmt.Enabled = true;
            Tx_LoanDEBIT.Enabled = true;
            Tx_NetSal.Enabled = true;
            DateTimePicker_Start.Enabled = true;
            DateTimePicker_End.Enabled = true;
            Tx_PS_Present.Enabled = true;
            Tx_PS_Absent.Enabled = true;
            Tx_PS_Excuss.Enabled = true;
            Tx_BonusSal.Enabled = true;
            Tx_Payslip_No.ForeColor = Color.FromArgb(192, 0, 0);
            Tx_SP_Hide.ForeColor = Color.FromArgb(0, 0, 192);
            Tx_Payslip_No.ForeColor = Color.FromArgb(0, 0, 192);
            ComboEMPL_Name.ForeColor = Color.FromArgb(0, 0, 192);
            Tx_PaySal_No.ForeColor = Color.FromArgb(0, 0, 192);
            Tx_SP_Mem_ID.ForeColor = Color.FromArgb(0, 0, 192);
            Tx_SP_SalGrade.ForeColor = Color.FromArgb(0, 0, 192);
            Tx_SP_BaseAmt.ForeColor = Color.FromArgb(0, 0, 192);
            Tx_LoanDEBIT.ForeColor = Color.FromArgb(0, 0, 192);
            Tx_NetSal.ForeColor = Color.FromArgb(0, 0, 192);
            DateTimePicker_Start.ForeColor = Color.FromArgb(0, 0, 192);
            DateTimePicker_End.ForeColor = Color.FromArgb(0, 0, 192);
            Tx_PS_Present.ForeColor = Color.FromArgb(0, 0, 192);
            Tx_PS_Absent.ForeColor = Color.FromArgb(0, 0, 192);
            Tx_PS_Excuss.ForeColor = Color.FromArgb(0, 0, 192);
            Tx_BonusSal.ForeColor = Color.FromArgb(0, 0, 192);

        }



        private void Tx_Search_Loan_TextChanged(object sender, EventArgs e)
        {
            DataGrid_Loans.DataSource = this.PopulateData_LOANS();
        }

        #endregion Adding, Clear, Update and Delete
        //END Adding, Clear, Update and Delete
        //-----------------------------------------------------------------------------------
        private void ComboEMPL_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime startDate;
            DateTime endDate;
            int dateTime;

            startDate = DateTimePicker_Start.Value.Date;
            endDate = DateTimePicker_End.Value.Date;
            if(startDate > endDate)
            {
                dateTime = 1;
            }
            else
            {
                dateTime = 0;
            }
            //dateTime = DateTime.Compare(startDate, endDate);
            //string n = Convert.ToString(dateTime);
            //MessageBox.Show(n);

            if (dateTime !=1)
            {
                DataTable dt = new DataTable();
                string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("usp_Load_and_Search_PYSal_Load", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@searchText", ComboEMPL_Name.Text.Trim());
                command.Parameters.AddWithValue("@startdate", DateTimePicker_Start.Value);
                command.Parameters.AddWithValue("@enddate", DateTimePicker_End.Value);
                command.ExecuteNonQuery();
                SqlDataReader dr = command.ExecuteReader();

                if (dr.Read())
                {
                    Tx_PaySal_No.Text = dr["NameFull"].ToString();
                    Tx_SP_Mem_ID.Text = dr["TKC_MemID"].ToString();
                    Tx_SP_SalGrade.Text = dr["Grade_Code"].ToString();
                    Tx_SP_BaseAmt.Text = dr["Base_Amt"].ToString();
                    Tx_LoanDEBIT.Text = dr["MonthlyDeduct"].ToString();
                    Tx_PS_Present.Text = dr["Present"].ToString();
                    Tx_PS_Absent.Text = dr["Adsent"].ToString();
                    Tx_PS_Excuss.Text = dr["Excuse"].ToString();
                    Tx_BonusSal.Text = dr["Amt"].ToString();
                    Calculate_Sal();
                }
            }
            else
            {
                MessageBox.Show("End Date cannot be greater than Start Date");
            }
            
        }



        public void Calculate_Sal()
        {
            decimal netPay = 0;
            decimal present = 0;
            decimal excuse = 0;
            decimal bonus = 0;
            decimal loanD = 0;
            decimal baseSal = 0;

            if (Tx_PS_Present.Text != "" && Tx_PS_Absent.Text != "" && Tx_BonusSal.Text != "" && Tx_LoanDEBIT.Text != "" && Tx_SP_BaseAmt.Text != "")
            {
                present = Convert.ToDecimal(Tx_PS_Present.Text);
                excuse = Convert.ToDecimal(Tx_PS_Absent.Text);
                bonus = Convert.ToDecimal(Tx_BonusSal.Text);
                loanD = Convert.ToDecimal(Tx_LoanDEBIT.Text);
                baseSal = Convert.ToDecimal(Tx_SP_BaseAmt.Text);

                decimal totalWorkDays = present + excuse;
                decimal netWithOutLoan = totalWorkDays * baseSal;
                decimal netMinusLoan = netWithOutLoan - loanD;
                netPay = netMinusLoan + bonus;
                Tx_NetSal.Text = Convert.ToString(netPay);
            }
            else
                return;

            
        } 
        private void BU_Gen_ATT_Click(object sender, EventArgs e)
        {
            ComboEMPL_Name_SelectedIndexChanged(null,null);
            Calculate_Sal();
        }

    }
}
