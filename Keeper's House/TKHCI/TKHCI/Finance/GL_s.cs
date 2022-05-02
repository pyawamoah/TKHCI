using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TKHCI.Finance
{
    public partial class GL_s : Form
    {
        #region SQL FROM DB
        private readonly string load_salGrade = "Select 'Select Salary Grade' as SalGrade union all SELECT distinct GL_Setup.GL_Name salGrade from GL_Setup with (nolock)";
        private readonly string load_comboMB1 = "Select 'Select Salary Grade' as MBL1 union all SELECT distinct GL_Setup.GL_Name salGrade from GL_Setup with (nolock)";
        #endregion
        //*******************************************************************************************************END
        #region BEFORE FORM
        KeeperDBEntities DBS = new KeeperDBEntities();
        decimal glCreation;
        decimal glDeposit;
        string gltype;
        string cashMode;
        decimal curBal = 0;
        decimal il_Bal = 0;
        decimal currentBal;
        decimal newAmount;
        decimal nowAmount;
        string one;
        string glType_up;
        string ILtype;

        #endregion
        //*******************************************************************************************************END
        public static class glCreate
        {
            public static string glset;
        }
        public static class glMaster
        {
            public static string masGL;
        }
        public GL_s()
        {
            InitializeComponent();
            _ = DefualtLoad();
        }
        #region SQL CONNECTIONS
        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
        #endregion SQL CONNECTIONS
        //*******************************************************************************************************END
        #region TASK MASTER FOR ID's LOAD STARTS
        public async Task DefualtLoad()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(Keepers_glSetup_Load());
            tasks.Add(Load_Combo_ME_Name());
            tasks.Add(Keepers_GL_Master_Load());
            tasks.Add(Keepers_Depo_Load());
            tasks.Add(Load_Combo_MB1());
            //tasks.Add(Keepers_Load_Pledge());

            await Task.WhenAll(tasks);
        }
        private Task Keepers_GL_Master_Load()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_GL_Master_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();

            if (xml < 1)
            {
                glCreation = 1;
                Tx_IL_No.Text = $"20000" + Convert.ToString(glCreation);
            }
            else if (xml >= 1)
            {
                glCreation = xml + 1;
                Tx_IL_No.Text = $"20000" + Convert.ToString(glCreation);
            }
            return Task.CompletedTask;
        }
        private Task Keepers_glSetup_Load()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_GL_Maker_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();

            if (xml < 1)
            {
                glCreation = 1;
                Tx_ADD_GL_No.Text = $"10000" + Convert.ToString(glCreation);
            }
            else if (xml >= 1)
            {
                glCreation = xml + 1;
                Tx_ADD_GL_No.Text = $"10000" + Convert.ToString(glCreation);
            }
            return Task.CompletedTask;
        }
        private Task Keepers_Depo_Load()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_glDepo_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var depo = (decimal)command.ExecuteScalar();
            Con.Close();

            if (depo < 1)
            {
                glDeposit = 1;
                Tx_glDeposit_NO.Text = $"DEPO-{DateTime.Now.Year}-" + Convert.ToString(glDeposit);
            }
            else if (depo >= 1)
            {
                glDeposit = depo + 1;
                Tx_glDeposit_NO.Text = $"DEPO-{DateTime.Now.Year}-" + Convert.ToString(glDeposit);
            }
            return Task.CompletedTask;
        }
        #endregion TASK MASTER FOR ID's LOAD STARTS
        //*******************************************************************************************************END
        #region COMBOBOX LOAD
        private Task Load_Combo_ME_Name()
        {
            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter(load_salGrade, Con);
            DataTable glDepo = new DataTable();
            da.Fill(glDepo);
            Combo_GL_Name.DataSource = glDepo;
            Combo_GL_Name.DisplayMember = "SalGrade";
            Combo_GL_Name.ValueMember = "SalGrade";
            Con.Close();
            return Task.CompletedTask;
        }
        private Task Load_Combo_MB1()
        {
            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter(load_comboMB1, Con);
            DataTable DMB1 = new DataTable();
            DataTable DMB2 = new DataTable();
            DataTable DMB3 = new DataTable();
            DataTable DMB4 = new DataTable();
            da.Fill(DMB1);
            da.Fill(DMB2);
            da.Fill(DMB3);
            da.Fill(DMB4);
            Combo_GLNO_CB1.DataSource = DMB1;
            Combo_GLNO_CB1.DisplayMember = "MBL1";
            Combo_GLNO_CB1.ValueMember = "MBL1";


            Combo_GLNO_CB2.DataSource = DMB2;
            Combo_GLNO_CB2.DisplayMember = "MBL1";
            Combo_GLNO_CB2.ValueMember = "MBL1";

            Combo_GLNO_CB3.DataSource = DMB3;
            Combo_GLNO_CB3.DisplayMember = "MBL1";
            Combo_GLNO_CB3.ValueMember = "MBL1";

            Combo_GLNO_CB4.DataSource = DMB4;
            Combo_GLNO_CB4.DisplayMember = "MBL1";
            Combo_GLNO_CB4.ValueMember = "MBL1";

            Con.Close();
            return Task.CompletedTask;
        }
        private void Combo_Gl_No_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection Conn = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
            Conn.Open();
            string Query = "SELECT GL_NO, GL_Type, Cur_Bal, Cur_Symbol, Cur_Short_Code from GL_Setup where GL_Name = '" + Combo_GL_Name.Text + "' ";
            SqlCommand cmd = new SqlCommand(Query, Conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Tx_GL_No.Text = dr["GL_NO"].ToString();
                Tx_GL_Type.Text = dr["GL_Type"].ToString();
                Tx_GL_Bal.Text = dr["Cur_Bal"].ToString();
                Tx_Cur_Symbol.Text = dr["Cur_Symbol"].ToString();
                Tx_Cur_ShortCode.Text = dr["Cur_Short_Code"].ToString();

            }
            Conn.Close();
        }
        private void Combo_GLNO_CB1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(Combo_GLNO_CB1.SelectedIndex != 0)
            {
                DataTable MBD1 = new DataTable();
                string connection1 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection1 = new SqlConnection(connection1);
                sqlConnection1.Open();
                SqlCommand command = new SqlCommand("[usp_Load_MoneyB1]", sqlConnection1);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@searchText", Combo_GLNO_CB1.Text.Trim());
                SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    Tx_CB_1_GL_NO.Text = dr["GL_NO"].ToString();
                    Tx_CB_1_GL_Sign.Text = dr["Cur_Symbol"].ToString();
                    Tx_CB_1_Descript.Text = dr["Cur_Description"].ToString();
                    Tx_CB_1_Bal.Text = dr["Cur_Bal"].ToString();
                }
                sqlConnection1.Close();
            }
            else
            {
                Tx_CB_1_GL_NO.Text = "";
                Tx_CB_1_GL_Sign.Text = "";
                Tx_CB_1_Descript.Text = "";
                Tx_CB_1_Bal.Text = "";
            }
               
        }
        private void Combo_GLNO_CB2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Combo_GLNO_CB2.SelectedIndex != 0)
            {
                DataTable MBD2 = new DataTable();
                string connection2 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection2 = new SqlConnection(connection2);
                sqlConnection2.Open();
                SqlCommand command = new SqlCommand("[usp_Load_MoneyB1]", sqlConnection2);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@searchText", Combo_GLNO_CB2.Text.Trim());
                SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    Tx_CB_2_GL_NO.Text = dr["GL_NO"].ToString();
                    Tx_CB_2_GL_Sign.Text = dr["Cur_Symbol"].ToString();
                    Tx_CB_2_Descript.Text = dr["Cur_Description"].ToString();
                    Tx_CB_2_Bal.Text = dr["Cur_Bal"].ToString();
                }
                sqlConnection2.Close();
            }
            else
            {
                Tx_CB_1_GL_NO.Text = "";
                Tx_CB_1_GL_Sign.Text = "";
                Tx_CB_1_Descript.Text = "";
                Tx_CB_1_Bal.Text = "";
            }

        }
        private void Combo_GLNO_CB3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Combo_GLNO_CB3.SelectedIndex != 0)
            {
                DataTable MBD3 = new DataTable();
                string connection3 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection3 = new SqlConnection(connection3);
                sqlConnection3.Open();
                SqlCommand command = new SqlCommand("[usp_Load_MoneyB1]", sqlConnection3);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@searchText", Combo_GLNO_CB3.Text.Trim());
                SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    Tx_CB_3_GL_NO.Text = dr["GL_NO"].ToString();
                    Tx_CB_3_GL_Sign.Text = dr["Cur_Symbol"].ToString();
                    Tx_CB_3_Descript.Text = dr["Cur_Description"].ToString();
                    Tx_CB_3_Bal.Text = dr["Cur_Bal"].ToString();
                }
                sqlConnection3.Close();
            }
            else
            {
                Tx_CB_1_GL_NO.Text = "";
                Tx_CB_1_GL_Sign.Text = "";
                Tx_CB_1_Descript.Text = "";
                Tx_CB_1_Bal.Text = "";
            }

        }
        private void Combo_GLNO_CB4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Combo_GLNO_CB4.SelectedIndex != 0)
            {
                DataTable MBD4 = new DataTable();
                string connection4 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection4 = new SqlConnection(connection4);
                sqlConnection4.Open();
                SqlCommand command = new SqlCommand("[usp_Load_MoneyB1]", sqlConnection4);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@searchText", Combo_GLNO_CB4.Text.Trim());
                SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    Tx_CB_4_GL_NO.Text = dr["GL_NO"].ToString();
                    Tx_CB_4_GL_Sign.Text = dr["Cur_Symbol"].ToString();
                    Tx_CB_4_Descript.Text = dr["Cur_Description"].ToString();
                    Tx_CB_4_Bal.Text = dr["Cur_Bal"].ToString();
                }
                sqlConnection4.Close();
            }
            else
            {
                Tx_CB_1_GL_NO.Text = "";
                Tx_CB_1_GL_Sign.Text = "";
                Tx_CB_1_Descript.Text = "";
                Tx_CB_1_Bal.Text = "";
            }

        }
        #endregion COMBOBOX LOAD
        //*******************************************************************************************************END
        #region DATAGRIDVIEW LOAD
        private object PopulateData_MasterGL()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_glMaster]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_MGL.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public object PopulateData_glSetup()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_glSetup]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_GL_Setup.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public object PopulateData_glDepo()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_Search_GL_DEPO]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_GL_Depo.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public void Load_GE_CAT_DataView()
        {
            DataView_GL_Setup.DataSource = this.PopulateData_glSetup();
        }
        public void Load_glDep_DataView()
        {
            DataGrib_GL_DEPO.DataSource = this.PopulateData_glDepo();
        }
        #endregion DATAGRIDVIEW LOAD
        //*******************************************************************************************************END
        #region ADD
        private void BT_Add_glSetup_Click(object sender, EventArgs e)
        {
            GL_Setup glSetupAd = new GL_Setup();
            try
            {
                if(Combo_ADD_GL_Type.SelectedIndex == 0)
                {
                    gltype = Tx_Custom_glType.Text;
                }
                else
                {
                    gltype = Combo_ADD_GL_Type.Text;
                }
                    glSetupAd.Cur_ID = int.Parse(Combo_CurID.Text);
                if(Tx_ADD_GL_No.Text != "" && Tx_ADD_glSetup_Name.Text != "" && Combo_CurID.SelectedIndex !=0)
                {
                    glSetupAd.GL_NO =Convert.ToDecimal( Tx_ADD_GL_No.Text);
                    glSetupAd.GL_Type = gltype;
                    glSetupAd.Cur_Bal = curBal;
                    glSetupAd.GL_Name = Tx_ADD_glSetup_Name.Text;
                    glSetupAd.Cur_Short_Code = Combo_Add_CurShort_Code.Text;
                    glSetupAd.Cur_Description = Tx_glSetupDescription.Text;
                    glSetupAd.Cur_Symbol = Combo_ADD_Cur_Symbol.Text;
                    glSetupAd.Cur_ID = int.Parse(Combo_CurID.Text);
                    glSetupAd.GLS_CreatedBY = TKHCI_Main_Login.MainLogin.Username;
                    glSetupAd.GLS_CreateDT = DateTime.Now;
                    string message = "Are you sure you want to add this Expense?";
                    string title = "Adding Expense......";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.GL_Setup.Add(glSetupAd);
                        DBS.SaveChanges();
                        MessageBox.Show("Expense Created Successfully");                     
                        BU_glSetup_Clear_Click(null, null);
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
        private void BT_Deposit_Click(object sender, EventArgs e)
        {
            decimal amt = Convert.ToDecimal(Tx_Amt.Text);
            decimal total = 0;
            string chqueNoEmpty;
            total = total + amt;
            GL_Deposit glDepositM = new GL_Deposit();
            if (Tx_GL_No.Text != "")
            {
                GL_Setup acc = new GL_Setup();
                decimal GLNO = Convert.ToDecimal(Tx_GL_No.Text);
                var item = (from u in DBS.GL_Setup where u.GL_NO == GLNO select u).FirstOrDefault();
            }
            else return;
            try
            {
                if (Radio_Cash.Checked)
                {
                    cashMode = "Cash";
                }
                else
                {
                    cashMode = "Cheque";
                }

                if(Tx_Chq_No.Text == "")
                {
                    chqueNoEmpty = "0";
                }
                else
                {
                    chqueNoEmpty = Tx_Chq_No.Text;
                }

                if (Tx_glDepo_Desc.Text != "" && Tx_Amt.Text != "")
                {
                        DataTable MBD1 = new DataTable();
                        string connection1 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                        SqlConnection sqlConnection1 = new SqlConnection(connection1);
                        sqlConnection1.Open();
                        SqlCommand command = new SqlCommand("[usp_Load_gL_lastBal]", sqlConnection1);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@searchText", Tx_GL_No.Text.Trim());
                        SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    one=(dr["Balance"].ToString());

                    if (one != "")
                    {
                        currentBal = Convert.ToDecimal(one);
                        newAmount = Convert.ToDecimal(Tx_Amt.Text);
                        nowAmount = currentBal + newAmount;
                    }
                    else
                    {
                        one = "0";
                        currentBal = Convert.ToDecimal(one);
                        newAmount = Convert.ToDecimal(Tx_Amt.Text);
                        nowAmount = currentBal + newAmount;
                    }                    
                } sqlConnection1.Close();
                    total = nowAmount;
                    glDepositM.DepositID = Tx_glDeposit_NO.Text;
                    glDepositM.GL_AcctNo =Convert.ToDecimal( Tx_GL_No.Text);
                    glDepositM.Depo_GL_Name = Combo_GL_Name.Text;
                    glDepositM.GL_Acct_Bal = total;
                    glDepositM.Cur_Symbol = Tx_Cur_Symbol.Text;
                    glDepositM.Description = Tx_glDepo_Desc.Text;
                    glDepositM.GL_Type = Tx_GL_Type.Text;
                    glDepositM.Mode = cashMode;
                    glDepositM.Chq_No = chqueNoEmpty;
                    glDepositM.Deposit_Amt = Convert.ToDecimal( Tx_Amt.Text );
                    glDepositM.depositedBy = TKHCI_Main_Login.MainLogin.Username;
                    glDepositM.createDT = DateTime.Now;
                    string message = "Are you sure you want to add this Expense?";
                    string title = "Adding Expense......";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.GL_Deposit.Add(glDepositM);
                        DBS.SaveChanges();
                        decimal GL_No = Convert.ToDecimal(Tx_GL_No.Text);
                        var item = (from u in DBS.GL_Setup where u.GL_NO == GL_No select u).FirstOrDefault();
                        item.Cur_Bal = item.Cur_Bal + Convert.ToDecimal(Tx_Amt.Text);
                        DBS.SaveChanges();
                        MessageBox.Show("Deposit Created Successfully");
                        BT_glDepo_Clear_Click(null, null);
                        Load_glDep_DataView();
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
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion ADD
        //*******************************************************************************************************END
        #region UPDATE
        private void BT_Update_glSetup_Click(object sender, EventArgs e)
        {
            string updateGLsetup = Tx_glSetup_Hide.Text;
            var updt_SU = (from s in DBS.GL_Setup where s.GL_Name == updateGLsetup select s).FirstOrDefault();

            if(Combo_ADD_GL_Type.SelectedIndex != 0)
            {
                glType_up = Combo_ADD_GL_Type.Text;
            }
            else
            {
                glType_up = Tx_Custom_glType.Text;
            }


            updt_SU.GL_NO = Convert.ToDecimal(Tx_ADD_GL_No.Text);
            updt_SU.GL_Type = glType_up;
            updt_SU.Cur_Bal = curBal;
            updt_SU.GL_Name = Tx_ADD_glSetup_Name.Text;
            updt_SU.Cur_Short_Code = Combo_Add_CurShort_Code.Text;
            updt_SU.Cur_Description = Tx_glSetupDescription.Text;
            updt_SU.Cur_Symbol = Combo_ADD_Cur_Symbol.Text;
            updt_SU.Cur_ID = int.Parse(Combo_CurID.Text);

            string message = "Are you sure you want to update this record?";
            string title = "updating Record...";
            MessageBoxButtons but = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, but);
            if (result == DialogResult.Yes)
            {

                DBS.SaveChanges();
                MessageBox.Show("GL Record Updated Successfully");
                Load_GE_CAT_DataView();
            }
            else return;
        }
        private void BU_UpdateB1_Click(object sender, EventArgs e)
        {
            if (Tx_CB_1_GL_NO.Text != "" && Tx_CB_1_Bal.Text != "")
            {
                var updt_MD1 = (from s in DBS.MoneyBlockTBs where s.MB_TD == 1 select s).FirstOrDefault();
                updt_MD1.MB_GL_NO = Convert.ToDecimal(Tx_CB_1_GL_NO.Text);
                updt_MD1.MB_Bal = Convert.ToDecimal(Tx_CB_1_Bal.Text);
                updt_MD1.MB_Descript = Tx_CB_1_Descript.Text;
                updt_MD1.MB_Cur_Sign = Tx_CB_1_GL_Sign.Text;
                updt_MD1.MB_CreateDT = DateTime.Now;
                updt_MD1.MB_CreatedBy = TKHCI_Main_Login.MainLogin.Username;

                string message = "Are you sure you want to update Display Block One?";
                string title = "updating Display...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.SaveChanges();
                    MessageBox.Show("Display Block One Updated Successfully");
                    BU_Clear_B1_Click(null, null);
                }
                else return;
            }
            else return;

            

        }
        private void BU_UpdateB2_Click(object sender, EventArgs e)
        {
            if (Tx_CB_2_GL_NO.Text != "")
            {
                var updt_MD2 = (from s in DBS.MoneyBlockTBs where s.MB_TD == 2 select s).FirstOrDefault();
                updt_MD2.MB_GL_NO = Convert.ToDecimal(Tx_CB_2_GL_NO.Text);
                updt_MD2.MB_Bal = Convert.ToDecimal(Tx_CB_2_Bal.Text);
                updt_MD2.MB_Descript = Tx_CB_2_Descript.Text;
                updt_MD2.MB_Cur_Sign = Tx_CB_2_GL_Sign.Text;
                updt_MD2.MB_CreateDT = DateTime.Now;
                updt_MD2.MB_CreatedBy = TKHCI_Main_Login.MainLogin.Username;

                string message = "Are you sure you want to update Display Block Two?";
                string title = "updating Display...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.SaveChanges();
                    MessageBox.Show("Display Block Two Updated Successfully");
                    BU_Clear_B2_Click(null, null);
                }
                else return;
            }
            else return;

        }
        private void BU_UpdateB3_Click(object sender, EventArgs e)
        {
            if(Tx_CB_3_GL_NO.Text !="" && Tx_CB_3_Bal.Text != "")
            {
                var updt_MD3 = (from s in DBS.MoneyBlockTBs where s.MB_TD == 3 select s).FirstOrDefault();
                updt_MD3.MB_GL_NO = Convert.ToDecimal(Tx_CB_3_GL_NO.Text);
                updt_MD3.MB_Bal = Convert.ToDecimal(Tx_CB_3_Bal.Text);
                updt_MD3.MB_Descript = Tx_CB_3_Descript.Text;
                updt_MD3.MB_Cur_Sign = Tx_CB_3_GL_Sign.Text;
                updt_MD3.MB_CreateDT = DateTime.Now;
                updt_MD3.MB_CreatedBy = TKHCI_Main_Login.MainLogin.Username;

                string message = "Are you sure you want to update Display Block Three?";
                string title = "updating Display...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.SaveChanges();
                    MessageBox.Show("Display Block Three Updated Successfully");
                    BU_Clear_B3_Click(null, null);
                }
                else return;

            }
            
        }
        private void BU_UpdateB4_Click(object sender, EventArgs e)
        {
            if (Tx_CB_4_GL_NO.Text != "" && Tx_CB_4_Bal.Text != "")
            {
                var updt_MD4 = (from s in DBS.MoneyBlockTBs where s.MB_TD == 4 select s).FirstOrDefault();
                updt_MD4.MB_GL_NO = Convert.ToDecimal(Tx_CB_4_GL_NO.Text);
                updt_MD4.MB_Bal = Convert.ToDecimal(Tx_CB_4_Bal.Text);
                updt_MD4.MB_Descript = Tx_CB_4_Descript.Text;
                updt_MD4.MB_Cur_Sign = Tx_CB_4_GL_Sign.Text;
                updt_MD4.MB_CreateDT = DateTime.Now;
                updt_MD4.MB_CreatedBy = TKHCI_Main_Login.MainLogin.Username;

                string message = "Are you sure you want to update Display Block Four?";
                string title = "updating Display...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.SaveChanges();
                    MessageBox.Show("Display Block Four Updated Successfully");
                    BU_Clear_B4_Click(null, null);
                }
                else return;
            }
            else return;
            
        }
        #endregion UPDATE
        //*******************************************************************************************************END
        #region CELL CONTENT CLICK
        private void DataView_GL_Setup_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_glSetup_Hide.Text = DataView_GL_Setup.SelectedRows[0].Cells[2].Value.ToString();
            glCreate.glset = Tx_glSetup_Hide.Text;
            CellContent_Click_glSetup();
        }
        private void CellContent_Click_glSetup()
        {
            var glSetUp_Click = DBS.GL_Setup.FirstOrDefault(a => a.GL_Name.Equals(glCreate.glset));
            if (!String.IsNullOrEmpty(glCreate.glset))
            {
                if (glSetUp_Click != null)
                {
                    Tx_ADD_GL_No.Text = Convert.ToString(glSetUp_Click.GL_NO);
                    Tx_Custom_glType.Text = glSetUp_Click.GL_Type;
                    Tx_ADD_glSetup_Name.Text = glSetUp_Click.GL_Name;
                    Combo_Add_CurShort_Code.Text = glSetUp_Click.Cur_Short_Code;
                    Combo_ADD_Cur_Symbol.Text = glSetUp_Click.Cur_Symbol;
                    Tx_glSetupDescription.Text = glSetUp_Click.Cur_Description;
                    Combo_CurID.Text = Convert.ToString( glSetUp_Click.Cur_ID);


                }
                else { return; }
            }
            else
            {
                return;
            }
        }

        #endregion CELL CONTENT CLICK
        //*******************************************************************************************************END
        #region DELETE
        private void BT_Del_glSetup_Click(object sender, EventArgs e)
        {
            string message = "Deletion of GL is not allowered, Kindly contact your Systems Administrator";
            string title = "Deletion";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
           
        }


        #endregion DELETE
        //*******************************************************************************************************END
        #region CLEAR FORM
        private void BU_glSetup_Clear_Click(object sender, EventArgs e)
        {
            Keepers_glSetup_Load();
            Combo_ADD_GL_Type.SelectedIndex = 0;
            Tx_Custom_glType.Text = "";
            Tx_ADD_glSetup_Name.Text = "";
            Combo_Add_CurShort_Code.SelectedIndex = 0;
            Combo_ADD_Cur_Symbol.SelectedIndex = 0;
            Tx_glSetupDescription.Text = "";
            Combo_CurID.SelectedIndex = 0;
        }
        private void BT_glDepo_Clear_Click(object sender, EventArgs e)
        {
            Combo_GL_Name.SelectedIndex = 0;
            Tx_GL_No.Text = "";
            Tx_GL_Type.Text = "";
            Tx_GL_Bal.Text = "";
            Tx_Cur_ShortCode.Text = "";
            Tx_glDepo_Desc.Text = "";
            Radio_Cash.Checked = true;
            Tx_Amt.Text = "";
            Tx_Cur_Symbol.Text = "";
            Tx_Chq_No.Text = "";
            Keepers_Depo_Load();
        }
        private void BU_Clear_B1_Click(object sender, EventArgs e)
        {
            Combo_GLNO_CB1.SelectedIndex = 0;
            Tx_CB_1_GL_Sign.Text = "";
            Tx_CB_1_GL_NO.Text = "";
            Tx_CB_1_Descript.Text = "";
            Tx_CB_1_Bal.Text = "";
        }
        private void BU_Clear_B2_Click(object sender, EventArgs e)
        {
            Combo_GLNO_CB2.SelectedIndex = 0;
            Tx_CB_2_GL_Sign.Text = "";
            Tx_CB_2_GL_NO.Text = "";
            Tx_CB_2_Descript.Text = "";
            Tx_CB_2_Bal.Text = "";
        }
        private void BU_Clear_B3_Click(object sender, EventArgs e)
        {
            Combo_GLNO_CB3.SelectedIndex = 0;
            Tx_CB_3_GL_Sign.Text = "";
            Tx_CB_3_GL_NO.Text = "";
            Tx_CB_3_Descript.Text = "";
            Tx_CB_3_Bal.Text = "";
        }
        private void BU_Clear_B4_Click(object sender, EventArgs e)
        {
            Combo_GLNO_CB4.SelectedIndex = 0;
            Tx_CB_4_GL_Sign.Text = "";
            Tx_CB_4_GL_NO.Text = "";
            Tx_CB_4_Descript.Text = "";
            Tx_CB_4_Bal.Text = "";
        }


        #endregion CLEAR FORM
        //*******************************************************************************************************END
        #region SEARCH DATA IN DATAGRIDVIEW
        private void Tx_Search_GL_Setup_TextChanged(object sender, EventArgs e)
        {
            DataView_GL_Setup.DataSource = this.PopulateData_glSetup();
        }
        #endregion SEARCH DATA IN DATAGRIDVIEW
        //*******************************************************************************************************END
        #region Radio Button manipulations
        private void Radio_Cheque_CheckedChanged(object sender, EventArgs e)
        {
            if (Radio_Cheque.Checked)
            {
                Tx_Chq_No.Visible = true;
                Tx_Chq_No.ReadOnly = false;
                Tx_Chq_No.Visible = true;
                PN_ChqNO.Visible = true;
                lblCheqNo.Visible = true;
            }
            else
            {
                Tx_Chq_No.Visible = false;
                Tx_Amt.Visible = true;
                Tx_Chq_No.ReadOnly = true;
                PN_ChqNO.Visible = false;
                lblCheqNo.Visible = false;

            }
        }
        #endregion Radio Button manipulations
        //*******************************************************************************************************END
        #region Main FORM LOAD
        private void GL_s_Load(object sender, EventArgs e)
        {
            Load_GE_CAT_DataView();
            Load_glDep_DataView();
            DisplayOne();
            DisplayTwo();
            DisplayThree();
            DisplayFour();
            lOAD_GL_Master();
            Tx_Chq_No.Visible = false;
            PN_ChqNO.Visible = false;
            lblCheqNo.Visible = false;
        }
        #endregion Main FORM LOAD
        //*******************************************************************************************************END
        #region Validation
        private void Tx_Amt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
           (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        #endregion Validation
        //*******************************************************************************************************END
        #region Display Block
        public void DisplayOne()
        {
            DataTable MBD1 = new DataTable();
            string connection1 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection1 = new SqlConnection(connection1);
            sqlConnection1.Open();
            SqlCommand command = new SqlCommand("[usp_Load_Money_Display1]", sqlConnection1);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = command.ExecuteReader();
            if (dr.Read())
            {
                Tx_B1_lbl.Text = dr["MB_Descript"].ToString();
                Tx_B1_Sign.Text = dr["MB_Cur_Sign"].ToString();
                string DBML1 = dr["MB_Bal"].ToString();
                Tx_B1_Bal.Text = String.Format("{0:#,##0.00}", double.Parse(DBML1));
            }
            sqlConnection1.Close();
        }
        public void DisplayTwo()
        {
            DataTable MBD1 = new DataTable();
            string connection1 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection1 = new SqlConnection(connection1);
            sqlConnection1.Open();
            SqlCommand command = new SqlCommand("[usp_Load_Money_Display2]", sqlConnection1);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = command.ExecuteReader();
            if (dr.Read())
            {
                Tx_B2_lbl.Text = dr["MB_Descript"].ToString();
                Tx_B2_Sign.Text = dr["MB_Cur_Sign"].ToString();
                string DBML2 = dr["MB_Bal"].ToString();
                Tx_B2_Bal.Text = String.Format("{0:#,##0.00}", double.Parse(DBML2));
            }
            sqlConnection1.Close();
        }
        public void DisplayThree()
        {
            DataTable MBD1 = new DataTable();
            string connection1 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection1 = new SqlConnection(connection1);
            sqlConnection1.Open();
            SqlCommand command = new SqlCommand("[usp_Load_Money_Display3]", sqlConnection1);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = command.ExecuteReader();
            if (dr.Read())
            {
                Tx_B3_lbl.Text = dr["MB_Descript"].ToString();
                Tx_B3_Sign.Text = dr["MB_Cur_Sign"].ToString();
                string DBML3 = dr["MB_Bal"].ToString();
                Tx_B3_Bal.Text = String.Format("{0:#,##0.00}", double.Parse(DBML3));
            }
            sqlConnection1.Close();
        }
        public void DisplayFour()
        {
            DataTable MBD1 = new DataTable();
            string connection1 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection1 = new SqlConnection(connection1);
            sqlConnection1.Open();
            SqlCommand command = new SqlCommand("[usp_Load_Money_Display4]", sqlConnection1);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = command.ExecuteReader();
            if (dr.Read())
            {
                Tx_B4_lbl.Text = dr["MB_Descript"].ToString();
                Tx_B4_Sign.Text = dr["MB_Cur_Sign"].ToString();
                string DBML4 = dr["MB_Bal"].ToString();
                Tx_B4_Bal.Text = String.Format("{0:#,##0.00}", double.Parse(DBML4));
            }
            sqlConnection1.Close();
        }

        private void BU_IL_ADD_Click(object sender, EventArgs e)
        {
            GL_Master masAdd = new GL_Master();
          
            try
            {
                if (Tx_Combo_IL_Type.SelectedIndex == 0)
                {
                    ILtype = Tx_Custom_glType.Text;
                }
                else
                {
                    ILtype = Tx_Combo_IL_Type.Text;
                }
                if (Tx_IL_No.Text != "" && Tx_IL_Name.Text != "" && Tx_Combo_IL_Cur_ID.SelectedIndex != 0)
                {
                    masAdd.IL_NO = Convert.ToDecimal(Tx_IL_No.Text);
                    masAdd.IL_Type = ILtype;
                    masAdd.IL_Bal = curBal;
                    masAdd.IL_Name = Tx_IL_Name.Text;
                    masAdd.IL_Cur_Short_Code = Tx_Combo_IL_Cur_Code.Text;
                    masAdd.IL_Description = Tx_IL_Description.Text;
                    masAdd.IL_Cur_Symbol = Tx_Combo_IL_Cur_Symbol.Text;
                    masAdd.IL_Cur_ID = int.Parse(Tx_Combo_IL_Cur_ID.Text);
                    masAdd.IL_GLS_CreatedBY = TKHCI_Main_Login.MainLogin.Username;
                    masAdd.IL_GLS_CreateDT = DateTime.Now;
                    string message = "Are you sure you want to add this Master  GL?";
                    string title = "Adding Master  GL......";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.GL_Master.Add(masAdd);
                        DBS.SaveChanges();
                        MessageBox.Show("Master  GL Created Successfully");
                        BU_IL_Clear_Click(null, null);
                        lOAD_GL_Master();
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
        private void BU_IL_Clear_Click(object sender, EventArgs e)
        {
            Keepers_GL_Master_Load();
            Tx_IL_Name.Text = "";
            Tx_Combo_IL_Type.SelectedIndex = 0;
            Tx_Combo_IL_Cur_Code.SelectedIndex = 0;
            Tx_Combo_IL_Cur_Symbol.SelectedIndex = 0;
            Tx_Combo_IL_Cur_ID.SelectedIndex = 0;
            Tx_IL_Description.Text = "";
            Tx_IL_Type_Custome.Text = "";
        }

        private void BU_IL_UPT_Click(object sender, EventArgs e)
        {
            string ilUpdate = Tx_glSetup_Hide.Text;
            var updt_IL = (from s in DBS.GL_Master where s.IL_Name == ilUpdate select s).FirstOrDefault();

            if (Combo_ADD_GL_Type.SelectedIndex != 0)
            {
                glType_up = Combo_ADD_GL_Type.Text;
            }
            else
            {
                glType_up = Tx_IL_Type_Custome.Text;
            }
            updt_IL.IL_NO = Convert.ToDecimal(Tx_IL_No.Text);
            updt_IL.IL_Type = ILtype;
            updt_IL.IL_Bal = curBal;
            updt_IL.IL_Name = Tx_IL_Name.Text;
            updt_IL.IL_Cur_Short_Code = Tx_Combo_IL_Cur_Code.Text;
            updt_IL.IL_Description = Tx_IL_Description.Text;
            updt_IL.IL_Cur_Symbol = Tx_Combo_IL_Cur_Symbol.Text;
            updt_IL.IL_Cur_ID = int.Parse(Tx_Combo_IL_Cur_ID.Text);

            string message = "Are you sure you want to update this record?";
            string title = "updating Record...";
            MessageBoxButtons but = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, but);
            if (result == DialogResult.Yes)
            {
                DBS.SaveChanges();
                MessageBox.Show("GL Record Updated Successfully");
                lOAD_GL_Master();
            }
            else return;
        }

        private void lOAD_GL_Master()
        {
            DataView_Master_GL.DataSource = this.PopulateData_MasterGL();
        }



        private void DataView_Master_GL_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_MGL_Hide.Text = DataView_GL_Setup.SelectedRows[0].Cells[2].Value.ToString();
            glCreate.glset = Tx_MGL_Hide.Text;
            masterGL_Click();
        }

        private void masterGL_Click()
        {

            var masterClick = DBS.GL_Master.FirstOrDefault(a => a.IL_Name.Equals(glMaster.masGL));
            if (!String.IsNullOrEmpty(glMaster.masGL))
            {
                if (masterClick != null)
                {
                    Tx_IL_No.Text = Convert.ToString(masterClick.IL_NO);
                    Tx_IL_Type_Custome.Text = masterClick.IL_Type;
                    Tx_IL_Name.Text = masterClick.IL_Name;
                    Tx_Combo_IL_Cur_Code.Text = masterClick.IL_Cur_Short_Code;
                    Tx_Combo_IL_Cur_Symbol.Text = masterClick.IL_Cur_Symbol;
                    Tx_IL_Description.Text = masterClick.IL_Description;
                    Tx_Combo_IL_Cur_ID.Text = Convert.ToString(masterClick.IL_Cur_ID);
                }
                else { return; }
            }
            else
            {
                return;
            }
        }
        #endregion Display Block
        //*******************************************************************************************************END
    }
}

