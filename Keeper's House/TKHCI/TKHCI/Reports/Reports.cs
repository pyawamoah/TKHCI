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

namespace TKHCI.Reports
{

    public partial class Reports : Form
    {
        #region START
        //private readonly string load_GLTH_CurCode = "Select 'Select Currency Code' as curCode union all SELECT distinct CONVERT(VARCHAR(12),GL_Master.IL_Cur_ID)from GL_Master with (nolock);";
        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
        int page = 0;
        int rowperpage;
        Boolean Load1;
        Boolean Load2;
        Boolean Load3;
        Boolean Load4;
        Boolean Load5;
        Boolean Load6;
        Boolean Load7;
        Boolean Load8;
        #endregion
        #region START REPORT
        public Reports()
        {
            InitializeComponent();
            _ = page_Load();
        }
        #endregion
        #region TASK MASTER
        public async Task page_Load()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(Load_Combo_Box());
            //tasks.Add(Keepers_Load_DisposalID());
            //tasks.Add(Keepers_Load_AssetID());
            //tasks.Add(Keepers_Load_CategoryID());
            await Task.WhenAll(tasks);
        }
        #endregion
        #region LOAD COMBO BOX'S
        private void Combo_Cur_Code_GLH_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_GLTH();
        }
        private Task Load_Combo_Box()
        {
            //Con.Open();
            //SqlDataAdapter da = new SqlDataAdapter(load_GLTH_CurCode, Con);
            //DataTable CurCode = new DataTable();
            //da.Fill(CurCode);
            //Combo_Cur_Code_GLH.DataSource = CurCode;
            //Combo_Cur_Code_GLH.DisplayMember = "curCode";
            //Combo_Cur_Code_GLH.ValueMember = "curCode";
            return Task.CompletedTask;
        }
        #endregion
        #region POPULATE FROM DATABASE TO CREATE GRIDVIEW
        public object PopulateDataGridView_Asset()
        {
            if (Load1 == true)
            {
                rowperpage = Convert.ToInt32(FixAsst_Combo_Row_count.Text);
                DataTable dt = new DataTable();
                string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("[usp_GenerateAssetsReport]", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@rowperpage", rowperpage);
                command.Parameters.AddWithValue("@runDateFrom", DateTime.Now.Date);
                command.Parameters.AddWithValue("@runDateTo", DateTime.Now.Date);
                command.Parameters.AddWithValue("@startRecordNumber", (page * rowperpage) + 1);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    lbl_NoRecord.Visible = true;
                }
                else
                {
                    lbl_NoRecord.Visible = false;
                }
                return dt;
            }
            else
            {
                Load1 = false;
                rowperpage = Convert.ToInt32(FixAsst_Combo_Row_count.Text);
                DataTable dt = new DataTable();
                string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("[usp_GenerateAssetsReport]", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@rowperpage", rowperpage);
                command.Parameters.AddWithValue("@runDateFrom", FA_Start_DT.Value.Date);
                command.Parameters.AddWithValue("@runDateTo", FA_End_DT.Value.Date);
                command.Parameters.AddWithValue("@startRecordNumber", (page * rowperpage) + 1);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    lbl_NoRecord.Visible = true;
                }
                else
                {
                    lbl_NoRecord.Visible = false;
                }
                return dt;

            }
        }
        public object PopulateDataGridView_Offeringt_ALL()
        {
            if (Load2 == true)
            {
                rowperpage = Convert.ToInt32(Combo_Offering_Count.Text);
                DataTable dt = new DataTable();
                string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("[usp_GenerateOfferingReport_All]", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@rowperpage", rowperpage);
                command.Parameters.AddWithValue("@runDateFrom", DateTime.Now.Date);
                command.Parameters.AddWithValue("@runDateTo", DateTime.Now.Date);
                command.Parameters.AddWithValue("@startRecordNumber", (page * rowperpage) + 1);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    llb_NoRecord_All.Visible = true;
                }
                else
                {
                    llb_NoRecord_All.Visible = false;
                }
                return dt;
            }
            else
            {
                Load2 = false;
                rowperpage = Convert.ToInt32(Combo_Offering_Count.Text);
                DataTable dt = new DataTable();
                string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("[usp_GenerateOfferingReport_All]", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@rowperpage", rowperpage);
                command.Parameters.AddWithValue("@runDateFrom", Offering_Start_DT.Value.Date);
                command.Parameters.AddWithValue("@runDateTo", Offering_End_DT.Value.Date);
                command.Parameters.AddWithValue("@startRecordNumber", (page * rowperpage) + 1);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    llb_NoRecord_All.Visible = true;
                }
                else
                {
                    llb_NoRecord_All.Visible = false;
                }
                return dt;

            }
        }
        public object PopulateDataGridView_Offeringt_Total()
        {
            if (Load3 == true)
            {
                //rowperpage = Convert.ToInt32(Combo_Off_Count_Total.Text);
                DataTable dt1 = new DataTable();
                string connection1 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection1 = new SqlConnection(connection1);
                sqlConnection1.Open();
                SqlCommand command = new SqlCommand("[usp_GenerateOfferingReport_Total]", sqlConnection1);
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@rowperpage", rowperpage);
                //command.Parameters.AddWithValue("@runDateFrom", DateTime.Now.Date);
                //command.Parameters.AddWithValue("@runDateTo", DateTime.Now.Date);
                //command.Parameters.AddWithValue("@startRecordNumber", (page * rowperpage) + 1);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt1);
                if (dt1.Rows.Count == 2)
                {
                    lbl_GetTotal.Visible = true;
                }
                else
                {
                    lbl_GetTotal.Visible = false;
                }
                return dt1;
            }
            else
            {
                Load3 = false;
                //rowperpage = Convert.ToInt32(Combo_Off_Count_Total.Text);
                DataTable dt1 = new DataTable();
                string connection1 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection1 = new SqlConnection(connection1);
                sqlConnection1.Open();
                SqlCommand command = new SqlCommand("[usp_GenerateOfferingReport_Total]", sqlConnection1);
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@rowperpage", rowperpage);
                //command.Parameters.AddWithValue("@runDateFrom", Offering_Start_DT.Value.Date);
                //command.Parameters.AddWithValue("@runDateTo", Offering_End_DT.Value.Date);
                //command.Parameters.AddWithValue("@startRecordNumber", (page * rowperpage) + 1);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt1);
                if (dt1.Rows.Count == 0)
                {
                    lbl_GetTotal.Visible = true;
                }
                else
                {
                    lbl_GetTotal.Visible = false;
                }
                return dt1;

            }
        }
        public object PopulateDataGridView_GLTHL()
        {
            if (Load4 == true && GLTH_Combo.SelectedIndex == 0)
            {
                #region
                //if (Combo_Cur_Code_GLH.SelectedIndex == 0)
                //{
                //    curID = 0;
                //}
                //else if (Combo_Cur_Code_GLH.SelectedIndex == 1)
                //{
                //    curID = 1;
                //}
                //else if (Combo_Cur_Code_GLH.SelectedIndex == 2)
                //{
                //    curID = 2;
                //}
                //else if (Combo_Cur_Code_GLH.SelectedIndex == 3)
                //{
                //    curID = 3;
                //}
                //else if (Combo_Cur_Code_GLH.SelectedIndex == 4)
                //{
                //    curID = 4;
                //}
                #endregion
                DataTable dt = new DataTable();
                string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("[usp_GL_Trans_History]", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@currencyCode", Convert.ToInt32(Combo_Cur_Code_GLH.SelectedIndex));
                command.Parameters.AddWithValue("@rowperpage", Convert.ToInt32(GLTH_Combo.Text));
                command.Parameters.AddWithValue("@runDateFrom", DateTime.Now.Date);
                command.Parameters.AddWithValue("@runDateTo", DateTime.Now.Date);
                command.Parameters.AddWithValue("@startRecordNumber", (page * Convert.ToInt32(GLTH_Combo.Text)) + 1);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt);
                _ = dt.Rows.Count == 0 ? lbl_GLTH.Visible = true : lbl_GLTH.Visible = false;
                //if (dt.Rows.Count == 0)
                //{
                //    lbl_GLTH.Visible = true;
                //}
                //else
                //{
                //    lbl_GLTH.Visible = false;
                //}
                return dt;
            }
            else
            {
                Load4 = false;
                #region
                //if (Combo_Cur_Code_GLH.SelectedIndex == 0)
                //{
                //    curID = 0;
                //}
                //else if (Combo_Cur_Code_GLH.SelectedIndex == 1)
                //{
                //    curID = 1;
                //}
                //else if (Combo_Cur_Code_GLH.SelectedIndex == 2)
                //{
                //    curID = 2;
                //}
                //else if (Combo_Cur_Code_GLH.SelectedIndex == 3)
                //{
                //    curID = 3;
                //}
                //else if (Combo_Cur_Code_GLH.SelectedIndex == 4)
                //{
                //    curID = 4;
                //}
                #endregion
                DataTable dt = new DataTable();
                string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("[usp_GL_Trans_History]", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@currencyCode", Convert.ToInt32(Combo_Cur_Code_GLH.SelectedIndex));
                command.Parameters.AddWithValue("@rowperpage", Convert.ToInt32(GLTH_Combo.Text));
                command.Parameters.AddWithValue("@runDateFrom", GLTH_StartDT.Value.Date);
                command.Parameters.AddWithValue("@runDateTo", GLTH_EndDT.Value.Date);
                command.Parameters.AddWithValue("@startRecordNumber", (page * Convert.ToInt32(GLTH_Combo.Text)) + 1);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    lbl_GLTH.Visible = true;
                }
                else
                {
                    lbl_GLTH.Visible = false;
                }
                return dt;

            }
        }
        public object PopulateDataGridView_First_Fruit_All()
        {
            var cur = Convert.ToInt32(Combo_FirstFruit_Cur_ID.SelectedIndex);
            if (Load5 == true && Combo_FirstFruit_Page_NO.SelectedIndex == 0)
            {
                #region
                //if (Combo_FirstFruit_Cur_ID.SelectedIndex == 0)
                //{
                //    curID = 0;
                //}
                //else if (Combo_FirstFruit_Cur_ID.SelectedIndex == 1)
                //{
                //    curID = 1;
                //}
                //else if (Combo_FirstFruit_Cur_ID.SelectedIndex == 2)
                //{
                //    curID = 2;
                //}
                //else if (Combo_FirstFruit_Cur_ID.SelectedIndex == 3)
                //{
                //    curID = 3;
                //}
                //else if (Combo_FirstFruit_Cur_ID.SelectedIndex == 4)
                //{
                //    curID = 4;
                //}
                #endregion
                DataTable dt = new DataTable();
                string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("[usp_GenerateFirstFruitReport_All]", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@searchText", Tx_Search_MEM_First_Fruit.Text.Trim());
                command.Parameters.AddWithValue("@currencyCode", Convert.ToInt32(Combo_FirstFruit_Cur_ID.SelectedIndex));
                command.Parameters.AddWithValue("@rowperpage", Convert.ToInt32(Combo_FirstFruit_Page_NO.Text));
                command.Parameters.AddWithValue("@runDateFrom", DateTime.Now.Date);
                command.Parameters.AddWithValue("@runDateTo", DateTime.Now.Date);
                command.Parameters.AddWithValue("@startRecordNumber", (page * Convert.ToInt32(Combo_FirstFruit_Page_NO.Text)) + 1);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    lbl_All_NO_Display.Visible = true;
                }
                else
                {
                    lbl_All_NO_Display.Visible = false;
                }
                return dt;
            }
            else
            {
                Load5 = false;
                DataTable dt = new DataTable();
                string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("[usp_GenerateFirstFruitReport_All]", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@searchText", Tx_Search_MEM_First_Fruit.Text.Trim());
                command.Parameters.AddWithValue("@currencyCode", Convert.ToInt32(Combo_FirstFruit_Cur_ID.SelectedIndex));
                command.Parameters.AddWithValue("@rowperpage", Convert.ToInt32(Combo_FirstFruit_Page_NO.Text));
                command.Parameters.AddWithValue("@runDateFrom", FirstFruit_Start_DT.Value.Date);
                command.Parameters.AddWithValue("@runDateTo", FirstFruit_End_DT.Value.Date);
                command.Parameters.AddWithValue("@startRecordNumber", (page * Convert.ToInt32(Combo_FirstFruit_Page_NO.Text)) + 1);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    lbl_All_NO_Display.Visible = true;
                }
                else
                {
                    lbl_All_NO_Display.Visible = false;
                }
                return dt;
            }
        }
        public object PopulateDataGridView_FirstFruit_Total()
        {
            if (Load6 == true)
            {
                DataTable dt1 = new DataTable();
                string connection1 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection1 = new SqlConnection(connection1);
                sqlConnection1.Open();
                SqlCommand command = new SqlCommand("[usp_Generate_FirstFruit_Report_Total]", sqlConnection1);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@runDateFrom", DateTime.Now.Date);
                command.Parameters.AddWithValue("@runDateTo", DateTime.Now.Date);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt1);
                if (dt1.Rows.Count == 2)
                {
                    lbl_Total_NO_Display.Visible = true;
                }
                else
                {
                    lbl_Total_NO_Display.Visible = false;
                }
                return dt1;
            }
            else
            {
                Load6 = false;
                //rowperpage = Convert.ToInt32(Combo_Off_Count_Total.Text);
                DataTable dt1 = new DataTable();
                string connection1 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection1 = new SqlConnection(connection1);
                sqlConnection1.Open();
                SqlCommand command = new SqlCommand("[usp_Generate_FirstFruit_Report_Total]", sqlConnection1);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@runDateFrom", FirstFruit_Start_DT.Value.Date);
                command.Parameters.AddWithValue("@runDateTo", FirstFruit_End_DT.Value.Date);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt1);
                if (dt1.Rows.Count == 0)
                {
                    lbl_Total_NO_Display.Visible = true;
                }
                else
                {
                    lbl_Total_NO_Display.Visible = false;
                }
                return dt1;

            }
        }
        public object PopulateDataGridView_Tithe_Total()
        {
            if (Load7 == true)
            {
                DataTable dt7 = new DataTable();
                string connection1 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection1 = new SqlConnection(connection1);
                sqlConnection1.Open();
                SqlCommand command = new SqlCommand("[usp_Generate_Tithe_Report_Total]", sqlConnection1);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@runDateFrom", null);
                command.Parameters.AddWithValue("@runDateTo", null);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt7);
                _ = dt7.Rows.Count == 0 ? lbl_Tithe_Total_NORECORD.Visible = true : lbl_Tithe_Total_NORECORD.Visible = false;
                return dt7;
            }
            else
            {
                Load7 = false;
                DataTable dt7 = new DataTable();
                string connection1 = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection1 = new SqlConnection(connection1);
                sqlConnection1.Open();
                SqlCommand command = new SqlCommand("[usp_Generate_Tithe_Report_Total]", sqlConnection1);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@runDateFrom", Tithe_Start_DT.Value.Date);
                command.Parameters.AddWithValue("@runDateTo", Tithe_End_DT.Value.Date);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt7);
                _ = dt7.Rows.Count == 0 ? lbl_Tithe_Total_NORECORD.Visible = true : lbl_Tithe_Total_NORECORD.Visible = false;
                return dt7;

            }
        }
        public object PopulateDataGridView_Tithe_All()
        {
            var cur = Convert.ToInt32(Combo_Tithe_Cur_Filter.SelectedIndex);
            if (Load8 == true && Combo_Tithe_Page_No.SelectedIndex == 0)
            {
                DataTable dt8 = new DataTable();
                string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("[usp_Generate_Tithe_Report_All]", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@searchText", Tx_Search_Tithe_BY_Name.Text.Trim());
                command.Parameters.AddWithValue("@currencyCode", Convert.ToInt32(Combo_Tithe_Cur_Filter.SelectedIndex));
                command.Parameters.AddWithValue("@rowperpage", Convert.ToInt32(Combo_Tithe_Page_No.Text));
                command.Parameters.AddWithValue("@runDateFrom", DateTime.Now.Date);
                command.Parameters.AddWithValue("@runDateTo", DateTime.Now.Date);
                command.Parameters.AddWithValue("@startRecordNumber", (page * Convert.ToInt32(Combo_Tithe_Page_No.Text)) + 1);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt8);
                _ = dt8.Rows.Count == 0 ? lbl_Tithe_All_NORECORD.Visible = true : lbl_Tithe_All_NORECORD.Visible = false;
                return dt8;
            }
            else
            {
                Load8 = false;
                DataTable dt8 = new DataTable();
                string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("[usp_Generate_Tithe_Report_All]", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@searchText", Tx_Search_Tithe_BY_Name.Text.Trim());
                command.Parameters.AddWithValue("@currencyCode", Convert.ToInt32(Combo_Tithe_Cur_Filter.SelectedIndex));
                command.Parameters.AddWithValue("@rowperpage", Convert.ToInt32(Combo_Tithe_Page_No.Text));
                command.Parameters.AddWithValue("@runDateFrom", Tithe_Start_DT.Value.Date);
                command.Parameters.AddWithValue("@runDateTo", Tithe_End_DT.Value.Date);
                command.Parameters.AddWithValue("@startRecordNumber", (page * Convert.ToInt32(Combo_Tithe_Page_No.Text)) + 1);
                command.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt8);
                _ = dt8.Rows.Count == 0 ? lbl_Tithe_All_NORECORD.Visible = true : lbl_Tithe_All_NORECORD.Visible = false;
                return dt8;
            }
        }
        #endregion
        #region GRIDVIEW ASSIGNED
        public void Load_GLTH()
        {
            DateTime startDate;
            DateTime endDate;
            int dateTime;

            startDate = GLTH_StartDT.Value.Date;
            endDate = GLTH_EndDT.Value.Date;
            if (startDate > endDate)
            {
                dateTime = 1;
            }
            else
            {
                dateTime = 0;
            }

            if (dateTime != 1)
            {
                DataGrid_GLTH.DataSource = this.PopulateDataGridView_GLTHL();
            }
            else
            {
                MessageBox.Show("End Date cannot be greater than Start Date");
            }
        }
        public void Load_Asset()
        {
            DateTime startDate;
            DateTime endDate;
            int dateTime;

            startDate = FA_Start_DT.Value.Date;
            endDate = FA_End_DT.Value.Date;
            if (startDate > endDate)
            {
                dateTime = 1;
            }
            else
            {
                dateTime = 0;
            }

            if (dateTime != 1)
            {
                DataGrid_Asset.DataSource = this.PopulateDataGridView_Asset();
            }
            else
            {
                MessageBox.Show("End Date cannot be greater than Start Date");
            }
        }
        public void Load_Offering_ALL()
        {
            DateTime startDate;
            DateTime endDate;
            int dateTime;

            startDate = Offering_Start_DT.Value.Date;
            endDate = Offering_End_DT.Value.Date;
            if (startDate > endDate)
            {
                dateTime = 1;
            }
            else
            {
                dateTime = 0;
            }

            if (dateTime != 1)
            {
                Offering_DGV_ALL.DataSource = this.PopulateDataGridView_Offeringt_ALL();
            }
            else
            {
                MessageBox.Show("End Date cannot be greater than Start Date");
            }
        }
        public void Load_Offering_Total()
        {
            DateTime startDate;
            DateTime endDate;
            int dateTime;

            startDate = Offering_Start_DT.Value.Date;
            endDate = Offering_End_DT.Value.Date;
            if (startDate > endDate)
            {
                dateTime = 1;
            }
            else
            {
                dateTime = 0;
            }

            if (dateTime != 1)
            {
                DataGrid_OF_Total.DataSource = this.PopulateDataGridView_Offeringt_Total();
            }
            else
            {
                MessageBox.Show("End Date cannot be greater than Start Date");
            }
        }
        public void Load_First_Fruit_ALL()
        {
            DateTime startDate;
            DateTime endDate;
            int dateTime;

            startDate = FirstFruit_Start_DT.Value.Date;
            endDate = FirstFruit_End_DT.Value.Date;
            if (startDate > endDate)
            {
                dateTime = 1;
            }
            else
            {
                dateTime = 0;
            }

            if (dateTime != 1)
            {
                DataGrid_FirstFruit_All.DataSource = this.PopulateDataGridView_First_Fruit_All();
            }
            else
            {
                MessageBox.Show("End Date cannot be greater than Start Date");
            }
        }
        public void Load_FirstFruit_Total()
        {
            DateTime startDate;
            DateTime endDate;
            int dateTime;

            startDate = FirstFruit_Start_DT.Value.Date;
            endDate = FirstFruit_End_DT.Value.Date;
            if (startDate > endDate)
            {
                dateTime = 1;
            }
            else
            {
                dateTime = 0;
            }

            if (dateTime != 1)
            {
                DataGrid_FirstFruit_Total.DataSource = this.PopulateDataGridView_FirstFruit_Total();
            }
            else
            {
                MessageBox.Show("End Date cannot be greater than Start Date");
            }
        }
        public void Load_Tithe_Total()
        {
            DateTime startDate;
            DateTime endDate;
            int dateTime;

            startDate = Tithe_Start_DT.Value.Date;
            endDate = Tithe_End_DT.Value.Date;
            if (startDate > endDate)
            {
                dateTime = 1;
            }
            else
            {
                dateTime = 0;
            }

            if (dateTime != 1)
            {
                DataGrid_Tithe_Total.DataSource = this.PopulateDataGridView_Tithe_Total();
            }
            else
            {
                MessageBox.Show("End Date cannot be greater than Start Date");
            }
        }
        public void Load_Tithe_All()
        {
            DateTime startDate;
            DateTime endDate;
            int dateTime;

            startDate = Tithe_Start_DT.Value.Date;
            endDate = Tithe_End_DT.Value.Date;
            if (startDate > endDate)
            {
                dateTime = 1;
            }
            else
            {
                dateTime = 0;
            }

            if (dateTime != 1)
            {
                DataGrid_Tithe_All.DataSource = this.PopulateDataGridView_Tithe_All();
            }
            else
            {
                MessageBox.Show("End Date cannot be greater than Start Date");
            }
        }
        #endregion
        #region LOAD REPORTS TO VIEW MIAN FORM LOAD
        private void Reports_Load(object sender, EventArgs e)
        {
            Load1 = true;
            Load2 = true;
            Load3 = true;
            Load4 = true;
            Load5 = true;
            Load6 = true;
            Load7 = true;
            Load8 = true;

            lbl_All_NO_Display.Visible = false;
            lbl_Total_NO_Display.Visible = false;
            lbl_GetTotal.Visible = false;
            lbl_GetTotal.Visible = false;
            lbl_GLTH.Visible = false;
            lbl_GLTH.Visible = false;
            lbl_Total_NO_Display.Visible = false;
            lbl_Tithe_Total_NORECORD.Visible = false;
            lbl_Tithe_All_NORECORD.Visible = false;
            lbl_NoRecord.Visible = false;
            llb_NoRecord_All.Visible = false;

            Load_Tithe_Total();
            Load_Asset();
            Load_Offering_ALL();
            Load_Offering_Total();
            Load_First_Fruit_ALL();
            Load_GLTH();
            Load_FirstFruit_Total();
            Load_Tithe_All();

            Tithe_Start_DT.Value = DateTime.Now.Date;
            Tithe_End_DT.Value = DateTime.Now.Date;
            Combo_Cur_Code_GLH.StartIndex = 0;
            GLTH_StartDT.Value = DateTime.Now.Date;
            GLTH_EndDT.Value = DateTime.Now.Date;
            FA_Start_DT.Value = DateTime.Now.Date;
            FA_End_DT.Value = DateTime.Now.Date;
            Offering_Start_DT.Value = DateTime.Now.Date;
            Offering_End_DT.Value = DateTime.Now.Date;
            FirstFruit_Start_DT.Value = DateTime.Now.Date;
            FirstFruit_End_DT.Value = DateTime.Now.Date;

        }
        #endregion
        #region CLEAR TO DEFUALT
        private void BU_FA_Clear_Click(object sender, EventArgs e)
        {
            FA_Start_DT.Value = DateTime.Now;
            FA_End_DT.Value = DateTime.Now;
        }
        private void BU_Income_Clear_Click(object sender, EventArgs e)
        {
            Income_Start_DT.Value = DateTime.Now;
            Income_End_DT.Value = DateTime.Now;
        }
        private void BU_Expense_Clear_Click(object sender, EventArgs e)
        {
            Expense_Start_DT.Value = DateTime.Now;
            Expense_End_DT.Value = DateTime.Now;
        }
        private void BU_BS_Clear_Click(object sender, EventArgs e)
        {
            //BS_Start_DT.Value = DateTime.Now;
            //BS_End_DT.Value = DateTime.Now;
        }
        private void BU_Offering_Clear_Click(object sender, EventArgs e)
        {
            Offering_Start_DT.Value = DateTime.Now;
            Offering_End_DT.Value = DateTime.Now;
        }
        private void BU_Tithe_Clear_Click_1(object sender, EventArgs e)
        {
            Tithe_Start_DT.Value = DateTime.Now;
            Tithe_End_DT.Value = DateTime.Now;
        }
        private void BU_IE_Clear_Click(object sender, EventArgs e)
        {
            IE_Start_DT.Value = DateTime.Now;
            IE_End_DT.Value = DateTime.Now;
        }
        private void BU_FirstFruit_Clear_Click(object sender, EventArgs e)
        {
            FirstFruit_Start_DT.Value = DateTime.Now;
            FirstFruit_End_DT.Value = DateTime.Now;
        }
        private void GLTH_Clear_Click(object sender, EventArgs e)
        {
            GLTH_StartDT.Value = DateTime.Now;
            GLTH_EndDT.Value = DateTime.Now;
        }
        #endregion
        #region NEXT PAGE
        private void BU_GLTH_NP_Click(object sender, EventArgs e)
        {
            page = page + 1;
            Load_GLTH();
        }
        private void BU_FA_NP_Click(object sender, EventArgs e)
        {
            page = page + 1;
            Load_Asset();
        }
        private void BU_Offering_NP_Click(object sender, EventArgs e)
        {
            page = page + 1;
            Load_Offering_ALL();
        }
        private void BU_Offering_NP_All_Click(object sender, EventArgs e)
        {
            page = page + 1;
            Load_Offering_Total();
        }
        private void BU_FirstFruit_NP_Click(object sender, EventArgs e)
        {
            page = page + 1;
            Load_First_Fruit_ALL();
        }
        private void BU_Tithe_NP_Click(object sender, EventArgs e)
        {
            page = page + 1;
            Load_Tithe_All();
        }
        #endregion
        #region PREVIOUSE PAGE
        private void BU_FA_PP_Click(object sender, EventArgs e)
        {
            page = page - 1;

            if (page >= 0)
            {
                Load_Asset();

            }
            else
            {
                MessageBox.Show(page + " is out of report range");
                page = 0;
            }

        }
        private void BU_Offering_PP_All_Click(object sender, EventArgs e)
        {
            page = page - 1;

            if (page >= 0)
            {
                Load_Offering_Total();

            }
            else
            {
                MessageBox.Show(page + " is out of report range");
                page = 0;
            }
        }
        private void BU_Offering_PP_Click(object sender, EventArgs e)
        {
            page = page - 1;

            if (page >= 0)
            {
                Load_Offering_ALL();

            }
            else
            {
                MessageBox.Show(page + " is out of report range");
                page = 0;
            }
        }
        private void BU_GLTH_PP_Click(object sender, EventArgs e)
        {
            page = page - 1;

            if (page >= 0)
            {
                Load_GLTH();

            }
            else
            {
                MessageBox.Show(page + " is out of report range");
                page = 0;
            }
        }
        private void BU_Tithe_PP_Click(object sender, EventArgs e)
        {
            page = page - 1;

            if (page >= 0)
            {
                Load_Tithe_All();

            }
            else
            {
                MessageBox.Show("Report is out of range");
                page = 0;
            }
        }
        private void BU_FirstFruit_PP_Click(object sender, EventArgs e)
        {
            page = page - 1;

            if (page >= 0)
            {
                Load_First_Fruit_ALL();

            }
            else
            {
                MessageBox.Show(page + " is out of report range");
                page = 0;
            }
        }
        #endregion
        #region GET REPORTS
        private void BU_Asset_Get_Click(object sender, EventArgs e)
        {
            Load1 = false;
            Load_Asset();
        }
        private void BU_Offering_Get_All_Click(object sender, EventArgs e)
        {
            Load2 = false;
            Load_Offering_ALL();
        }
        private void BU_Offering_Get_Click(object sender, EventArgs e)
        {
            Load3 = false;
            Load_Offering_Total();
        }
        private void GLTH_Get_Report_Click(object sender, EventArgs e)
        {
            Load4 = false;
            Load_GLTH();
        }
        private void BU_FirstFruit_Get_All_Click(object sender, EventArgs e)
        {
            Load5 = false;
            Load_First_Fruit_ALL();
        }
        private void BU_FirstFruit_Get_Total_Click(object sender, EventArgs e)
        {
            Load6 = false;
            Load_FirstFruit_Total();
        }
        private void BU_Tithe_Get_Total_Click(object sender, EventArgs e)
        {
            Load7 = false;
            Load_Tithe_Total();
        }
        private void BU_Tithe_Get_All_Click(object sender, EventArgs e)
        {
            Load8 = false;
            Load_Tithe_All();
        }
        #endregion
        #region DOWNLOAD REPORT
        private void BU_FA_Download_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Exported from gridview";
            for (int i = 1; i < DataGrid_Asset.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = DataGrid_Asset.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < DataGrid_Asset.Rows.Count - 1; i++)
            {
                for (int j = 0; j < DataGrid_Asset.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = DataGrid_Asset.Rows[i].Cells[j].Value.ToString();
                }
            }
            app.Quit();
        }
        private void BU_Offering_Down_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Exported from gridview";
            for (int i = 1; i < Offering_DGV_ALL.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = Offering_DGV_ALL.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < Offering_DGV_ALL.Rows.Count - 1; i++)
            {
                for (int j = 0; j < Offering_DGV_ALL.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = Offering_DGV_ALL.Rows[i].Cells[j].Value.ToString();
                }
            }
            app.Quit();
        }
        private void BU_Offering_Down_All_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Exported from gridview";
            for (int i = 1; i < Offering_DGV_ALL.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = Offering_DGV_ALL.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < Offering_DGV_ALL.Rows.Count - 1; i++)
            {
                for (int j = 0; j < Offering_DGV_ALL.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = Offering_DGV_ALL.Rows[i].Cells[j].Value.ToString();
                }
            }
            app.Quit();
        }
        private void BU_Offering_Down_Total_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Exported from gridview";
            for (int i = 1; i < DataGrid_OF_Total.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = DataGrid_OF_Total.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < DataGrid_OF_Total.Rows.Count - 1; i++)
            {
                for (int j = 0; j < DataGrid_OF_Total.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = DataGrid_OF_Total.Rows[i].Cells[j].Value.ToString();
                }
            }
            app.Quit();
        }
        private void GLTH_Get_Download_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Exported from gridview";
            for (int i = 1; i < DataGrid_GLTH.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = DataGrid_GLTH.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < DataGrid_GLTH.Rows.Count - 1; i++)
            {
                for (int j = 0; j < DataGrid_GLTH.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = DataGrid_GLTH.Rows[i].Cells[j].Value.ToString();
                }
            }
            app.Quit();
        }
        private void BU_Tithe_Down_Total_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Exported from gridview";
            for (int i = 1; i < DataGrid_Tithe_Total.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = DataGrid_Tithe_Total.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < DataGrid_Tithe_Total.Rows.Count - 1; i++)
            {
                for (int j = 0; j < DataGrid_Tithe_Total.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = DataGrid_Tithe_Total.Rows[i].Cells[j].Value.ToString();
                }
            }
            app.Quit();
        }
        private void BU_Tithe_Down_All_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Exported from gridview";
            for (int i = 1; i < DataGrid_Tithe_All.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = DataGrid_Tithe_All.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < DataGrid_Tithe_All.Rows.Count - 1; i++)
            {
                for (int j = 0; j < DataGrid_Tithe_All.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = DataGrid_Tithe_All.Rows[i].Cells[j].Value.ToString();
                }
            }
            app.Quit();
        }
        #endregion
        #region ROW COUNT INDEX CHANGE
        private void FixAsst_Combo_Row_count_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Asset();
        }
        private void Combo_Offering_Count_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Offering_ALL();
        }
        private void Combo_Off_Count_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Offering_Total();
        }
        private void GLTH_Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_GLTH();
        }
        private void Combo_FirstFruit_Cur_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_First_Fruit_ALL();
        }
        private void Combo_Tithe_Cur_Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Tithe_All();
        }
        private void Combo_FirstFruit_Page_NO_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_First_Fruit_ALL();
        }
        private void Combo_Tithe_Page_No_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Tithe_All();
        }
        #endregion
        #region FILTER BY NAME
        private void Tx_Search_MEM_First_Fruit_TextChanged(object sender, EventArgs e)
        {
            Load_First_Fruit_ALL();
        }
        private void Tx_Search_Tithe_BY_Name_TextChanged(object sender, EventArgs e)
        {
            Load_Tithe_All();
        }

        #endregion

    }
}
