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

namespace TKHCI.Admin_Module.Service_CRM
{
    public partial class Service : Form
    {
        #region BEFORE FORM 
        KeeperDBEntities DBS = new KeeperDBEntities();
        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
        decimal ID;
        decimal Updt;
        public static class serviceSession
        {
            public static string serviceID;
        }
        #endregion
        #region Main Form Start
        public Service()
        {
            InitializeComponent();
            _ = Master();
        }
        #endregion
        #region TASK MANAGER
        public async Task Master()
        {
            List<Task> masterTask = new List<Task>();
            masterTask.Add(Keepers_Member_LoadID());
            await Task.WhenAll(masterTask);

        }
        #endregion
        #region LOAD SERVICE ID
        private Task Keepers_Member_LoadID()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_Service_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();
            if (xml < 1)
            {
                ID = 1;
                Tx_Service_ID.Text = $"SER-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            else if (xml >= 1)
            {
                ID = xml + 1;
                Tx_Service_ID.Text = $"SER-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            return Task.CompletedTask;
        }
        #endregion
        #region LOAD DATAGRID VIEW & SEARCH CHURCH BY NAME
        public object PopulateDataGridView_Service()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_Search_Service]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_Service_By_Name.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }

        public void Gridview()
        {
            DataGrib_Service.DataSource = this.PopulateDataGridView_Service();
        }
        private void Tx_Search_Service_By_Name_TextChanged(object sender, EventArgs e)
        {
            DataGrib_Service.DataSource = this.PopulateDataGridView_Service();
        }
        #endregion
        #region GRID VIEW CLICK 
        public void CLICK()
        {
            var ser = DBS.ServiceTBs.FirstOrDefault(a => a.Service_ID.Equals(serviceSession.serviceID));
            if (!String.IsNullOrEmpty(serviceSession.serviceID))
            {
                if (ser != null)
                {

                    Tx_Service_ID.Text = ser.Service_ID;
                    Tx_Service_Name.Text = ser.Service_Name;
                    Tx_Service_Lead_Pas.Text = ser.Service_Lead_Pastor;
                    Tx_Service_Assist_Pas.Text = ser.Service_Assist_Pastor;

                    string time1 = Convert.ToString(ser.Service_Start_Time.ToString());
                    DT_Service_Start_TIME.Value =Convert.ToDateTime( time1);

                    string time2 = Convert.ToString(ser.Service_Start_Time.ToString());
                    DT_Service_End_TIME.Value = Convert.ToDateTime(time2);
                }
            }
        }
        private void DataGrib_Service_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_Hide.Text = DataGrib_Service.SelectedRows[0].Cells[0].Value.ToString();
            serviceSession.serviceID = Tx_Hide.Text;
            CLICK();
        }
        #endregion
        #region MAIN FORM LOAD
        private void Service_Load(object sender, EventArgs e)
        {
            Gridview();
           
        }
        #endregion
        #region CLEAR FORM
        private void BT_ClearForm_Click(object sender, EventArgs e)
        {
            Keepers_Member_LoadID();
            Tx_Service_Name.Text = "";
            Tx_Service_Lead_Pas.Text = "";
            Tx_Service_Assist_Pas.Text = "";
        }
        #endregion
        #region ADD
        private void BT_AddNew_Service_Click(object sender, EventArgs e)
        {
            ServiceTB serAdd = new ServiceTB();
            //try
            //{
                if(Tx_Service_ID.Text !="" && Tx_Service_Name.Text != "")
                {
                    serAdd.Service_ID = Tx_Service_ID.Text;
                    serAdd.Service_Lead_Pastor = Tx_Service_Lead_Pas.Text;
                    serAdd.Service_Name = Tx_Service_Name.Text;
                    serAdd.CreatedBy = TKHCI_Main_Login.MainLogin.Username;
                    serAdd.Create_DT = DateTime.Now;
                    serAdd.Service_Assist_Pastor = Tx_Service_Assist_Pas.Text;
                    DateTime sratT = DT_Service_Start_TIME.Value;
                    TimeSpan ST = new TimeSpan(sratT.Hour, sratT.Month, sratT.Second);
                    serAdd.Service_Start_Time = ST;

                    DateTime endT = DT_Service_End_TIME.Value;
                    TimeSpan ET = new TimeSpan(endT.Hour, endT.Month, endT.Second);
                    serAdd.Service_End_Time = ET;


                string message = "Are you sure you want to add this record?";
                    string title = "Adding Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.ServiceTBs.Add(serAdd);
                        DBS.SaveChanges();
                        MessageBox.Show("User Created Successfully");
                        BT_ClearForm_Click(null, null);
                        Gridview();



                    }
                    else return;
                }

            //}catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

        }
        #endregion
        #region UPDATE
        private void But_Update_Service_Click(object sender, EventArgs e)
        {
            if (Tx_Service_ID.Text != "" && Tx_Service_Name.Text != "" && Tx_Hide.Text != "")
            {
                string update = Tx_Hide.Text;
                var updtService = (from s in DBS.ServiceTBs where s.Service_ID == update select s).FirstOrDefault();


                updtService.Service_ID = Tx_Service_ID.Text;
                updtService.Service_Lead_Pastor = Tx_Service_Lead_Pas.Text;
                updtService.Service_Name = Tx_Service_Name.Text;
                updtService.CreatedBy = TKHCI_Main_Login.MainLogin.Username;
                updtService.Create_DT = DateTime.Now;
                updtService.Service_Assist_Pastor = Tx_Service_Assist_Pas.Text;
                DateTime sratT = DT_Service_Start_TIME.Value;
                TimeSpan ST = new TimeSpan(sratT.Hour, sratT.Month, sratT.Second);
                updtService.Service_Start_Time = ST;

                DateTime endT = DT_Service_End_TIME.Value;
                TimeSpan ET = new TimeSpan(endT.Hour, endT.Month, endT.Second);
                updtService.Service_End_Time = ET;

                string message = "Are you sure you want to update this record?";
                string title = "updating Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {

                    DBS.SaveChanges();
                    MessageBox.Show("Member Record Updated Successfully");
                    BT_ClearForm_Click(null, null);
                    Gridview();

                }
                else return;


            }
        }

        #endregion
        #region DELATE
        private void Bt_Delete_Service_Click(object sender, EventArgs e)
        {
            ServiceTB acc = new ServiceTB();
            if (Tx_Hide.Text == "")
            {
                return;
            }
            else
            {
                string del = Tx_Hide.Text;
                if (!del.Equals(null))
                {
                    var delete = (from s in DBS.ServiceTBs where s.Service_ID == del select s).First();
                    string message = "Are you sure you want to delete this Service?";
                    string title = "Deleting Service...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.ServiceTBs.Remove(delete);
                        DBS.SaveChanges();
                        MessageBox.Show("Service Deleted");
                        BT_ClearForm_Click(null, null);
                        Gridview();


                    }
                    else return;
                }
                else MessageBox.Show("Select User to delete");
            }
        }
        #endregion
    }
}
