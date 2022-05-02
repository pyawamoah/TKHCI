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
using System.Configuration;

namespace TKHCI.Finance
{
    public partial class GeneralExpense : Form
    {
        #region START
        private readonly string load_deptName = "Select 'Select Department' as deptName union all SELECT distinct DepartmentTB.Dept_Name nameDept from DepartmentTB with (nolock)";
        private readonly string load_ExpCat = "Select 'Select Expense Category Name' as expCategory union all SELECT distinct GE_CategoryTB.GE_Cat_Name expCatName from GE_CategoryTB with (nolock)";
        decimal GrdTotal = 0;
        decimal n = 0;
        string cashMode;
        decimal amtNOW;
        decimal prevBal;
        decimal curBal;
        KeeperDBEntities DBS = new KeeperDBEntities();
        decimal Disposal;
        decimal Disposal1;
        public static class GE_Cat
        {
            public static string geCat;
        }
        #endregion
        public GeneralExpense()
        {
            InitializeComponent();
            _ = DefualtLoad();
        }
        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
        #region TASK MASTER FOR ID's LOAD STARTS
        public async Task DefualtLoad()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(Keepers_Load_GE_CAT());
            tasks.Add(Load_Dept_Name_GE());
            tasks.Add(Keepers_Load_General_Expense());
            tasks.Add(Load_Expense_Cat_Name());
            //tasks.Add(Keepers_Load_Pledge());

            await Task.WhenAll(tasks);
        }
        private Task Keepers_Load_GE_CAT()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_EX_CAT_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();

            if (xml < 1)
            {
                Disposal = 1;
                Tx_eP_Cat_No.Text = $"GE-CAT-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            else if (xml >= 1)
            {
                Disposal = xml + 1;
                Tx_eP_Cat_No.Text = $"GE-CAT-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            return Task.CompletedTask;
        }
        private Task Keepers_Load_General_Expense()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_ExpenseTB_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var GE = (decimal)command.ExecuteScalar();
            Con.Close();

            if (GE < 1)
            {
                Disposal1 = 1;
                Tx_GE_Ticket_No.Text = $"TICKET-{DateTime.Now.Year}-" + Convert.ToString(Disposal1);
            }
            else if (GE >= 1)
            {
                Disposal1 = GE + 1;
                Tx_GE_Ticket_No.Text = $"TICKET-{DateTime.Now.Year}-" + Convert.ToString(Disposal1);
            }
            return Task.CompletedTask;
        }

        #endregion TASK MASTER FOR LOAD ID's ENDS HERE..............
        #region Load ComboBox from DB
        private Task Load_Dept_Name_GE()
        {
            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter(load_deptName, Con);
            DataTable nameDept = new DataTable();
            da.Fill(nameDept);
            ComboBox_Dept_Name.DataSource = nameDept;
            ComboBox_Dept_Name.DisplayMember = "deptName";
            ComboBox_Dept_Name.ValueMember = "deptName";
            Con.Close();
            return Task.CompletedTask;
        }
        private Task Load_Expense_Cat_Name()
        {
            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter(load_ExpCat, Con);
            DataTable nameCat = new DataTable();
            da.Fill(nameCat);
            Combo_Expense_Category.DataSource = nameCat;
            Combo_Expense_Category.DisplayMember = "expCategory";
            Combo_Expense_Category.ValueMember = "expCategory";
            Con.Close();
            return Task.CompletedTask;
        }
        #endregion Load ComboBox from DB
        #region TASK POPULATING FORMS STARTS HERE
        public object PopulateData_GE_Cat()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_Search_GE_Category]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_GX_Cat.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public object PopulateData_Members()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_And_Search_GE_Member]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_Mem_Name.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public object PopulateData_Expenses()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_Search_General_Expense]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_Expenses.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public void Load_GE_CAT_DataView()
        {
            DataGrid_GE_CAT.DataSource = this.PopulateData_GE_Cat();
        }
        public void Load_General_Expenses_DataView()
        {
            DataView_Expenses.DataSource = this.PopulateData_Expenses();
        }
        public void Load_Members_Load_DataView()
        {
            DataGrib_Members.DataSource = this.PopulateData_Members();
        }
        #endregion TASK POPULATING FORMS ENDS HERE
        #region FROM MAIN LOAD
        private void GeneralExpense_Load(object sender, EventArgs e)
        {
            Load_GE_CAT_DataView();
            Load_General_Expenses_DataView();
            Load_Members_Load_DataView();
            Tx_Chq_No.Visible = false;
            Tx_eTrans_No.Visible = false;
            RD_GE_Cash.Checked = true;
        }
        #endregion FROM MAIN LOAD
        #region SEARCH DATA
        private void Tx_Search_Mem_Name_TextChanged(object sender, EventArgs e)
        {
            DataGrib_Members.DataSource = this.PopulateData_Members();
        }

        private void Tx_Search_GX_Cat_TextChanged(object sender, EventArgs e)
        {
            DataGrid_GE_CAT.DataSource = this.PopulateData_GE_Cat();
        }
        private void Tx_Search_Expenses_TextChanged(object sender, EventArgs e)
        {
            DataView_Expenses.DataSource = this.PopulateData_Expenses();
        }
        #endregion SEARCH DATA
        #region ADD Date to DB
        private void BU_Add_GE_CAT_Click(object sender, EventArgs e)
        {
            GE_CategoryTB addGE_Cat = new GE_CategoryTB();
            try
            {
                if (
                        Tx_Category_Name.Text != "" && Tx_eP_Cat_No.Text != "")
                {

                    addGE_Cat.GE_CAT_ID = Tx_eP_Cat_No.Text;
                    addGE_Cat.GE_Cat_Name = Tx_Category_Name.Text;
                    addGE_Cat.GE_Cat_Des = Tx_Cat_Description.Text;
                    addGE_Cat.GE_Liability_Type = Combo_Liability_Type.SelectedIndex;
                    addGE_Cat.GE_CreatedBy = TKHCI_Main_Login.MainLogin.Username;
                    addGE_Cat.GE_CreateDT = DateTime.Now;


                    string message = "Are you sure you want to add this record?";
                    string title = "Adding Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.GE_CategoryTB.Add(addGE_Cat);
                        DBS.SaveChanges();
                        MessageBox.Show("Expense Category Created Successfully");
                        BU_Clear_GE_CAT_Click(null, null);
                        Load_GE_CAT_DataView();
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


        private void BU_Add_Expenses_Click(object sender, EventArgs e)
        {

            BU_Cart_ADD_Click(null, null);
            GL_Transactions trans = new GL_Transactions();
            ExpenseTB addExpense = new ExpenseTB();
            GL_Master_History Mastrans = new GL_Master_History();
            GL_Master masterOne = new GL_Master();
            if(Tx_TotalExpense.Text != "")
            {
                amtNOW = Convert.ToDecimal(Tx_TotalExpense.Text);
            }
            else
            {
                string message = "Kindly Add items to Cart before commiting the transaction";
                string title = "Cart Empty";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);

            }


            try
            {
                if (!amtNOW.Equals(null))
                {
                    if (RD_GE_Cash.Checked)
                    {
                        cashMode = "Cash";
                    }
                    else if (RD_GE_Cheque.Checked)
                    {
                        cashMode = "Cheque";
                    }
                    else
                    {
                        cashMode = "eTransfer";
                    }

                    if (Tx_GE_Ticket_No.Text != "" && Tx_TotalExpense.Text != "")
                    {
                        addExpense.Exp_BatchNO = Tx_GE_Ticket_No.Text;
                        addExpense.Exp_Dept_Name = ComboBox_Dept_Name.Text;
                        addExpense.Exp_Cash_Mode = cashMode;
                        addExpense.Exp_Chq_No = Tx_Chq_No.Text;
                        addExpense.Exp_eTran_No = Tx_eTrans_No.Text;
                        addExpense.Exp_Cur_ID = Combo_Cur_ID.SelectedIndex;
                        addExpense.Exp_Batch_Total = Convert.ToDecimal(Tx_TotalExpense.Text);
                        addExpense.Exp_CreatedBY = TKHCI_Main_Login.MainLogin.Username;
                        addExpense.Exp_Create_DT = DateTime.Now;

                        string message = "Are you sure you want to add this Expense?";
                        string title = "Adding Expense......";
                        MessageBoxButtons but = MessageBoxButtons.YesNo;
                        DialogResult result = MessageBox.Show(message, title, but);
                        if (result == DialogResult.Yes)
                        {
                            if (Combo_Cur_ID.Text == "GHS")
                            {
                                var glDebit = (from u in DBS.GL_Master where u.IL_NO == 200001 select u).FirstOrDefault();
                                prevBal = (decimal)glDebit.IL_Bal;
                                curBal = prevBal - amtNOW;
                                glDebit.IL_Bal = curBal;

                                Mastrans.GMH_GMH_Acct_no = (decimal)glDebit.IL_NO;
                                Mastrans.GMH_Acct_type = glDebit.IL_Type;
                                Mastrans.GMH_Description = ComboBox_Dept_Name.Text;
                                Mastrans.GMH_Member_ID = Tx_GE_Ticket_No.Text;
                                Mastrans.GMH_Create_dt = DateTime.Now;
                                Mastrans.GMH_Effective_dt = DateTime.Now;
                                Mastrans.GMH_Prev_Balance = prevBal;
                                Mastrans.GMH_Amt = amtNOW;
                                Mastrans.GMH_Total_Balance = curBal;
                                Mastrans.GMH_Debit_credit = 1;
                                Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                                Mastrans.GMH_Cur_ID = (int)glDebit.IL_Cur_ID;
                                DBS.GL_Master_History.Add(Mastrans);
                                DBS.SaveChanges();

                                trans.Acct_no = (decimal)glDebit.IL_NO;
                                trans.Acct_type = glDebit.IL_Type;
                                trans.Description = ComboBox_Dept_Name.Text;
                                trans.Member_ID = Tx_GE_Ticket_No.Text;
                                trans.Create_dt = DateTime.Now;
                                trans.Effective_dt = DateTime.Now;
                                trans.Amt = amtNOW;
                                trans.Debit_credit = 1;
                                trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                                trans.Cur_ID = (int)glDebit.IL_Cur_ID;
                                DBS.GL_Transactions.Add(trans);
                                DBS.SaveChanges();

                                trans.Acct_no = (decimal)glDebit.IL_NO;
                                trans.Acct_type = glDebit.IL_Type;
                                trans.Description = ComboBox_Dept_Name.Text;
                                trans.Member_ID = Tx_GE_Ticket_No.Text;
                                trans.Create_dt = DateTime.Now;
                                trans.Effective_dt = DateTime.Now;
                                trans.Amt = amtNOW;
                                trans.Debit_credit = 1;
                                trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                                trans.Cur_ID = (int)glDebit.IL_Cur_ID;
                                DBS.GL_Transactions.Add(trans);
                                DBS.SaveChanges();
                            }
                            else if (Combo_Cur_ID.Text == "USD")
                            {
                                var glDebit = (from u in DBS.GL_Master where u.IL_NO == 200002 select u).FirstOrDefault();
                                prevBal = (decimal)glDebit.IL_Bal;
                                curBal = prevBal - amtNOW;
                                glDebit.IL_Bal = curBal;

                                Mastrans.GMH_GMH_Acct_no = (decimal)glDebit.IL_NO;
                                Mastrans.GMH_Acct_type = glDebit.IL_Type;
                                Mastrans.GMH_Description = ComboBox_Dept_Name.Text;
                                Mastrans.GMH_Member_ID = Tx_GE_Ticket_No.Text;
                                Mastrans.GMH_Create_dt = DateTime.Now;
                                Mastrans.GMH_Effective_dt = DateTime.Now;
                                Mastrans.GMH_Prev_Balance = prevBal;
                                Mastrans.GMH_Amt = amtNOW;
                                Mastrans.GMH_Total_Balance = curBal;
                                Mastrans.GMH_Debit_credit = 1;
                                Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                                Mastrans.GMH_Cur_ID = (int)glDebit.IL_Cur_ID;
                                DBS.GL_Master_History.Add(Mastrans);
                                DBS.SaveChanges();

                                trans.Acct_no = (decimal)glDebit.IL_NO;
                                trans.Acct_type = glDebit.IL_Type;
                                trans.Description = ComboBox_Dept_Name.Text;
                                trans.Member_ID = Tx_GE_Ticket_No.Text;
                                trans.Create_dt = DateTime.Now;
                                trans.Effective_dt = DateTime.Now;
                                trans.Amt = amtNOW;
                                trans.Debit_credit = 1;
                                trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                                trans.Cur_ID = (int)glDebit.IL_Cur_ID;
                                DBS.GL_Transactions.Add(trans);
                                DBS.SaveChanges();

                                trans.Acct_no = (decimal)glDebit.IL_NO;
                                trans.Acct_type = glDebit.IL_Type;
                                trans.Description = ComboBox_Dept_Name.Text;
                                trans.Member_ID = Tx_GE_Ticket_No.Text;
                                trans.Create_dt = DateTime.Now;
                                trans.Effective_dt = DateTime.Now;
                                trans.Amt = amtNOW;
                                trans.Debit_credit = 1;
                                trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                                trans.Cur_ID = (int)glDebit.IL_Cur_ID;
                                DBS.GL_Transactions.Add(trans);
                                DBS.SaveChanges();
                            }
                            else if (Combo_Cur_ID.Text == "GBP")
                            {
                                var glDebit = (from u in DBS.GL_Master where u.IL_NO == 200003 select u).FirstOrDefault();
                                prevBal = (decimal)glDebit.IL_Bal;
                                curBal = prevBal - amtNOW;
                                glDebit.IL_Bal = curBal;

                                Mastrans.GMH_GMH_Acct_no = (decimal)glDebit.IL_NO;
                                Mastrans.GMH_Acct_type = glDebit.IL_Type;
                                Mastrans.GMH_Description = ComboBox_Dept_Name.Text;
                                Mastrans.GMH_Member_ID = Tx_GE_Ticket_No.Text;
                                Mastrans.GMH_Create_dt = DateTime.Now;
                                Mastrans.GMH_Effective_dt = DateTime.Now;
                                Mastrans.GMH_Prev_Balance = prevBal;
                                Mastrans.GMH_Amt = amtNOW;
                                Mastrans.GMH_Total_Balance = curBal;
                                Mastrans.GMH_Debit_credit = 1;
                                Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                                Mastrans.GMH_Cur_ID = (int)glDebit.IL_Cur_ID;
                                DBS.GL_Master_History.Add(Mastrans);
                                DBS.SaveChanges();

                                trans.Acct_no = (decimal)glDebit.IL_NO;
                                trans.Acct_type = glDebit.IL_Type;
                                trans.Description = ComboBox_Dept_Name.Text;
                                trans.Member_ID = Tx_GE_Ticket_No.Text;
                                trans.Create_dt = DateTime.Now;
                                trans.Effective_dt = DateTime.Now;
                                trans.Amt = amtNOW;
                                trans.Debit_credit = 1;
                                trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                                trans.Cur_ID = (int)glDebit.IL_Cur_ID;
                                DBS.GL_Transactions.Add(trans);
                                DBS.SaveChanges();

                                trans.Acct_no = (decimal)glDebit.IL_NO;
                                trans.Acct_type = glDebit.IL_Type;
                                trans.Description = ComboBox_Dept_Name.Text;
                                trans.Member_ID = Tx_GE_Ticket_No.Text;
                                trans.Create_dt = DateTime.Now;
                                trans.Effective_dt = DateTime.Now;
                                trans.Amt = amtNOW;
                                trans.Debit_credit = 1;
                                trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                                trans.Cur_ID = (int)glDebit.IL_Cur_ID;
                                DBS.GL_Transactions.Add(trans);
                                DBS.SaveChanges();
                            }
                            else if (Combo_Cur_ID.Text == "EUR")
                            {
                                var glDebit = (from u in DBS.GL_Master where u.IL_NO == 200004 select u).FirstOrDefault();
                                prevBal = (decimal)glDebit.IL_Bal;
                                curBal = prevBal - amtNOW;
                                glDebit.IL_Bal = curBal;

                                Mastrans.GMH_GMH_Acct_no = (decimal)glDebit.IL_NO;
                                Mastrans.GMH_Acct_type = glDebit.IL_Type;
                                Mastrans.GMH_Description = ComboBox_Dept_Name.Text;
                                Mastrans.GMH_Member_ID = Tx_GE_Ticket_No.Text;
                                Mastrans.GMH_Create_dt = DateTime.Now;
                                Mastrans.GMH_Effective_dt = DateTime.Now;
                                Mastrans.GMH_Prev_Balance = prevBal;
                                Mastrans.GMH_Amt = amtNOW;
                                Mastrans.GMH_Total_Balance = curBal;
                                Mastrans.GMH_Debit_credit = 1;
                                Mastrans.GMH_Empl_id = TKHCI_Main_Login.MainLogin.Username;
                                Mastrans.GMH_Cur_ID = (int)glDebit.IL_Cur_ID;
                                DBS.GL_Master_History.Add(Mastrans);
                                DBS.SaveChanges();

                                trans.Acct_no = (decimal)glDebit.IL_NO;
                                trans.Acct_type = glDebit.IL_Type;
                                trans.Description = ComboBox_Dept_Name.Text;
                                trans.Member_ID = Tx_GE_Ticket_No.Text;
                                trans.Create_dt = DateTime.Now;
                                trans.Effective_dt = DateTime.Now;
                                trans.Amt = amtNOW;
                                trans.Debit_credit = 1;
                                trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                                trans.Cur_ID = (int)glDebit.IL_Cur_ID;
                                DBS.GL_Transactions.Add(trans);
                                DBS.SaveChanges();

                                trans.Acct_no = (decimal)glDebit.IL_NO;
                                trans.Acct_type = glDebit.IL_Type;
                                trans.Description = ComboBox_Dept_Name.Text;
                                trans.Member_ID = Tx_GE_Ticket_No.Text;
                                trans.Create_dt = DateTime.Now;
                                trans.Effective_dt = DateTime.Now;
                                trans.Amt = amtNOW;
                                trans.Debit_credit = 1;
                                trans.Empl_id = TKHCI_Main_Login.MainLogin.Username;
                                trans.Cur_ID = (int)glDebit.IL_Cur_ID;
                                DBS.GL_Transactions.Add(trans);
                                DBS.SaveChanges();
                            }
                            DBS.ExpenseTBs.Add(addExpense);
                            DBS.SaveChanges();
                            MessageBox.Show("Expense Created Successfully");
                            MessageBox.Show("Expense Batch Created Successfully");
                            BU_NewBatch_Click(null, null);
                            Load_General_Expenses_DataView();
                            Tx_TotalExpense.Text = "";
                            GrdTotal = 0;

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
                else
                {
                    string message = "Kindly Add items to Cart before commiting the transaction";
                    string title = "Cart Empty";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                    
                }
                

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion ADD Date to DB
        #region All Updates
        private void BU_Updt_GE_CAT_Click(object sender, EventArgs e)
        {
            string updateGE_Cat = Tx_GX_Cat_HIDE.Text;
            var updt_GE_Cat = (from s in DBS.GE_CategoryTB where s.GE_CAT_ID == updateGE_Cat select s).FirstOrDefault();

            updt_GE_Cat.GE_CAT_ID = Tx_eP_Cat_No.Text;
            updt_GE_Cat.GE_Cat_Name = Tx_Category_Name.Text;
            updt_GE_Cat.GE_Cat_Des = Tx_Cat_Description.Text;
            updt_GE_Cat.GE_Liability_Type = Combo_Liability_Type.SelectedIndex;


            string message = "Are you sure you want to update this record?";
            string title = "updating Record...";
            MessageBoxButtons but = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, but);
            if (result == DialogResult.Yes)
            {

                DBS.SaveChanges();
                MessageBox.Show("Category Record Updated Successfully");
                BU_Clear_GE_CAT_Click(null, null);
                Load_GE_CAT_DataView();
            }
            else return;

        }
        #endregion  All Updates
        #region Clear Forms
        private void BU_Clear_GE_CAT_Click(object sender, EventArgs e)
        {
            Tx_Category_Name.Text = "";
            Tx_Cat_Description.Text = "";
            Combo_Liability_Type.SelectedIndex = 0;
            Keepers_Load_GE_CAT();
        }
        #endregion Clear Forms
        #region Delete
        private void BU_Dell_GE_CAT_Click(object sender, EventArgs e)
        {
            GE_CategoryTB delGe_Cat = new GE_CategoryTB();
            if (Tx_GX_Cat_HIDE.Text == "")
            {
                MessageBox.Show("Select Category to delete");
            }
            else
            {
                //string Del = Tx_Hide_Fam_ID.Text;
                var del_Cat = (from s in DBS.GE_CategoryTB where s.GE_CAT_ID.Equals(Tx_GX_Cat_HIDE.Text) select s).First();

                string message = "Are you sure you want to delete this Asset?";
                string title = "Deleting Categoty...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.GE_CategoryTB.Remove(del_Cat);
                    DBS.SaveChanges();
                    MessageBox.Show("Category  Deleted");
                    Load_GE_CAT_DataView();
                    BU_Clear_GE_CAT_Click(null, null);

                }
                else return;

            }
        }
        #endregion Delete
        #region Cell Clicks
        private void CellClick_GE_Category()
        {
            var click_Ge_Cat = DBS.GE_CategoryTB.FirstOrDefault(a => a.GE_CAT_ID.Equals(GE_Cat.geCat));
            if (!String.IsNullOrEmpty(GE_Cat.geCat))
            {
                if (click_Ge_Cat != null)
                {
                    Tx_Category_Name.Text = click_Ge_Cat.GE_Cat_Name;
                    Tx_Cat_Description.Text = click_Ge_Cat.GE_Cat_Des;
                    Tx_eP_Cat_No.Text = click_Ge_Cat.GE_CAT_ID;
                    Combo_Liability_Type.SelectedIndex = (int)click_Ge_Cat.GE_Liability_Type;
                }
                else { return; }
            }
            else
            {
                return;
            }
        }
        #endregion Cell Clicks
        #region Expense Cart & Billing
        private void DataGrid_GE_CAT_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_GX_Cat_HIDE.Text = DataGrid_GE_CAT.SelectedRows[0].Cells[1].Value.ToString();
            GE_Cat.geCat = Tx_GX_Cat_HIDE.Text;
            CellClick_GE_Category();
        }

        private void BU_Add_CartITEMS_Click(object sender, EventArgs e)
        {
            if (Tx_GE_Ticket_No.Text != "" && Combo_Expense_Category.SelectedIndex != 0 && Combo_Cur_ID.SelectedIndex != 0)
            {
                if (ComboBox_Dept_Name.SelectedIndex != 0)
                {
                    if (Tx_Unit_Price.Text != "" && Tx_Item_Name.Text != "" && Tx_Unit_Price.Text != "")
                    {
                        decimal Total = Convert.ToDecimal(Tx_Unit_Price.Text) * Convert.ToDecimal(Tx_Quantity.Text);
                        DataGridViewRow newRow = new DataGridViewRow();
                        newRow.CreateCells(Expense_CartDATA);
                        newRow.Cells[0].Value = n + 1;
                        newRow.Cells[1].Value = Tx_GE_Ticket_No.Text;
                        newRow.Cells[2].Value = Combo_Expense_Category.Text;
                        newRow.Cells[3].Value = Tx_Item_Name.Text;
                        newRow.Cells[4].Value = Tx_Unit_Price.Text;
                        newRow.Cells[5].Value = Combo_Cur_ID.SelectedIndex;
                        newRow.Cells[6].Value = Tx_Quantity.Text;
                        newRow.Cells[7].Value = Convert.ToDecimal(Tx_Unit_Price.Text) * Convert.ToDecimal(Tx_Quantity.Text);
                        Expense_CartDATA.Rows.Add(newRow);
                        n++;

                        GrdTotal = GrdTotal + Total;
                        Tx_TotalExpense.Text = Convert.ToString(GrdTotal);
                    }
                    else
                    {
                        string message = "Item Name, Unit Price and Quantity cannot be left blank";
                        string title = "Cart Empty";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    string message = "Kindly Select requesting Department to continue";
                    string title = "Cart Empty";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Information);
                }
            }
            else
            {
                string message = "Kindly fill the entire form to continue";
                string title = "Cart Empty";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Information);
            }
            Tx_Item_Name.Text = "";
            Tx_Quantity.Text = "";
            Tx_Unit_Price.Text = "";
        }

        private void BU_Clear_Cart_Click(object sender, EventArgs e)
        {
            ComboBox_Dept_Name.SelectedIndex = 0;
            Tx_Item_Name.Text = "";
            Tx_Unit_Price.Text = "";
            Tx_Quantity.Text = "";
            Combo_Expense_Category.SelectedIndex = 0;
        }

        private void BU_NewBatch_Click(object sender, EventArgs e)
        {
            Keepers_Load_General_Expense();
            BU_Clear_Cart_Click(null, null);
            Expense_CartDATA.Rows.Clear();
            Tx_TotalExpense.Text = "";
            RD_GE_Cash.Checked = true;
            Tx_Chq_No.Text = "";
            Tx_eTrans_No.Text = "";

        }

        private void RD_GE_Cash_CheckedChanged(object sender, EventArgs e)
        {
            if (RD_GE_Cash.Checked)
            {
                Tx_Chq_No.Visible = false;
                Tx_eTrans_No.Visible = false;
            }
            else
            {
                Tx_Chq_No.Visible = false;
                Tx_eTrans_No.Visible = false;
            }
        }

        private void RD_GE_Cheque_CheckedChanged(object sender, EventArgs e)
        {
            if (RD_GE_Cheque.Checked)
            {
                Tx_Chq_No.Visible = true;
                Tx_eTrans_No.Visible = false;
            }
            else
            {
                Tx_Chq_No.Visible = false;
                Tx_eTrans_No.Visible = false;
            }
        }

        private void RD_GE_Transfer_CheckedChanged(object sender, EventArgs e)
        {
            if (RD_GE_Transfer.Checked)
            {
                Tx_Chq_No.Visible = false;
                Tx_eTrans_No.Visible = true;
            }
            else
            {
                Tx_Chq_No.Visible = false;
                Tx_eTrans_No.Visible = false;
            }
        }

        private void BU_Cart_ADD_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in Expense_CartDATA.Rows)
            {
                string sqlQuerry = "INSERT INTO Expense_CartTB VALUES (@EXP_Cart_Row_No,@EXP_Cart_Batch_No," +
                                                                      "@EXP_Cart_Exp_Cat,@EXP_Cart_Pro_Name," +
                                                                      "@EXP_Cart_Unit_Price,@EXP_Cur_ID,@EXP_Cart_Quantity," +
                                                                      "@EXP_Cart_TotalPrice)";
                SqlCommand cmd = new SqlCommand(sqlQuerry, Con);
                if (dr.IsNewRow) continue;
                {
                    cmd.Parameters.AddWithValue("@EXP_Cart_Row_No", dr.Cells["Prodid"].Value ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EXP_Cart_Batch_No", dr.Cells["ticNo"].Value ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EXP_Cart_Exp_Cat", dr.Cells["ExpCat"].Value ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EXP_Cart_Pro_Name", dr.Cells["PName"].Value ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EXP_Cart_Unit_Price", dr.Cells["Price"].Value ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EXP_Cur_ID", dr.Cells["CurID"].Value ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EXP_Cart_Quantity", dr.Cells["Qunatity"].Value ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EXP_Cart_TotalPrice", dr.Cells["Total"].Value ?? DBNull.Value);
                }
                Con.Open();
                cmd.ExecuteNonQuery();
                Con.Close();
            }
        }
        #endregion  Expense Cart & Billing
    }
}
